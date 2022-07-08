using Maple2Storage.Enums;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public struct AdditionalEffectParameters
{
    public int Id;
    public int Level;
    public int Stacks;
    public int Source;
    public int Duration;
    public bool IsBuff;
    public SkillCast ParentSkill;

    public AdditionalEffectParameters(int id, int level)
    {
        Id = id;
        Level = level;
        Stacks = 1;
        Source = -1;
        Duration = 0;
        IsBuff = false;
        ParentSkill = null;
    }
}

public class AdditionalEffects
{
    public List<AdditionalEffect> Effects;
    public List<AdditionalEffect> TimedEffects;
    public IFieldActor Parent;
    private int NextBuffExpiration = -1;

    public AdditionalEffects(IFieldActor parent = null)
    {
        Effects = new();
        TimedEffects = new();
        Parent = parent;
    }

    public void UpdateEffects()
    {
        bool recomputedLastIteration = false;

        while (NextBuffExpiration != -1 && TimedEffects.Count > 0 && Environment.TickCount >= NextBuffExpiration)
        {
            int index = FindExpiredBuff();

            if (index == -1)
            {
                if (recomputedLastIteration)
                {
                    return;
                }

                RecomputeExpiration();

                recomputedLastIteration = true;

                continue;
            }

            recomputedLastIteration = false;

            RemoveEffect(Effects[index], index);
        }
    }

    public AdditionalEffect AddEffect(AdditionalEffectParameters parameters)
    {
        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls

        AdditionalEffect effect = new(parameters.Id, parameters.Level, parameters.Stacks);

        if (effect.LevelMetadata == null)
        {
            return null;
        }

        effect.ParentSkill = parameters.ParentSkill;

        EffectCancelEffectMetadata cancel = effect.LevelMetadata.CancelEffect;

        for (int i = 0; i < Effects.Count; i++)
        {
            AdditionalEffect oldEffect = Effects[i];
            EffectImmuneEffectMetadata immune = oldEffect.LevelMetadata.ImmuneEffect;

            if (immune?.ImmuneEffectCodes?.Contains(effect.Id) == true)
            {
                return null;
            }

            if (oldEffect.Id == effect.Id && oldEffect.Level > effect.Level)
            {
                return null;
            }

            if (oldEffect.Id == effect.Id)
            {
                parameters.Stacks = Math.Min(oldEffect.LevelMetadata.Basic.MaxBuffCount, parameters.Stacks + oldEffect.Stacks);

                RemoveEffect(oldEffect, i, true);

                break;
            }

            if (cancel?.CancelEffectCodes?.Contains(oldEffect.Id) == true)
            {
                RemoveEffect(oldEffect, i, true);

                break;
            }
        }

        effect.BuffId = GuidGenerator.Int();
        effect.SourceId = parameters.Source == -1 ? Parent.ObjectId : parameters.Source;
        effect.Start = Environment.TickCount;
        effect.Duration = effect.LevelMetadata.Basic?.DurationTick ?? parameters.Duration;
        effect.TickRate = effect.LevelMetadata.Basic?.IntervalTick ?? 0;

        if (parameters.Duration != 0)
        {
            TimedEffects.Add(effect);

            AddExpiringBuffTime(effect.End);
        }

        if (parameters.Duration == 0)
        {
            --effect.Start;
        }

        Effects.Add(effect);

        Parent.EffectAdded(effect);

        Parent.FieldManager.BroadcastPacket(BuffPacket.AddBuff(effect, Parent.ObjectId));

        if (effect.LevelMetadata.SplashSkill != null)
        {
            foreach (EffectTriggerSkillMetadata skill in effect.LevelMetadata.SplashSkill)
            {
                FireTriggerSkill(effect, skill, effect.ParentSkill, Parent, effect.Start);
            }
        }

        EffectDotDamageMetadata dotDamage = effect.LevelMetadata.DotDamage;

        if ((dotDamage?.Rate ?? 0) == 0)
        {
            return effect;
        }

        DamageSourceParameters dotParameters = new()
        {
            IsSkill = false,
            GuaranteedCrit = false,
            Element = (Element) dotDamage.Element,
            RangeType = SkillRangeType.Special,
            DamageType = (DamageType) dotDamage.DamageType,
            DamageRate = dotDamage.Rate,
            ParentSkill = parameters.ParentSkill
        };

        IFieldActor sourceActor = Parent.FieldManager.State.GetActor(parameters.Source);

        // TODO: fix dot damage handling with damage numbers and game session tracking

        GameSession session = null;

        if (sourceActor is Character character)
        {
            session = character.Value.Session;
        }

        DamageHandler.ApplyDotDamage(session, sourceActor, Parent, dotParameters);

        if (effect.Duration < effect.TickRate)
        {
            Task.Run(async () =>
            {
                while (effect.IsAlive && Environment.TickCount - effect.Start < effect.Duration)
                {
                    await Task.Delay(effect.TickRate);

                    if (!effect.IsAlive)
                    {
                        break;
                    }

                    DamageHandler.ApplyDotDamage(session, sourceActor, Parent, dotParameters);
                }
            });
        }

        return effect;
    }

    public void RemoveEffect(int id, int level, bool sendPacket = true)
    {
        int index;
        AdditionalEffect effect;

        if (TryGet(id, level, out effect, out index))
        {
            RemoveEffect(effect, index, sendPacket);
        }
    }

    public void RemoveEffect(AdditionalEffect effect, int index, bool sendPacket = true)
    {
        RemoveAt(index);

        effect.IsAlive = false;

        Parent.EffectRemoved(effect);

        if (effect.BuffId != -1)
        {
            if (sendPacket)
            {
                Parent.FieldManager.BroadcastPacket(BuffPacket.RemoveBuff(effect, Parent.ObjectId));
            }

            int timedIndex = -1;

            if (TryGet(effect.Id, effect.Level, out effect, out timedIndex, true) && effect.LevelMetadata != null)
            {
                RemoveAt(timedIndex, true);
            }

            RecomputeExpiration();
        }
    }

    public bool HasEffect(int effectId)
    {
        foreach (AdditionalEffect effect in Effects)
        {
            if (effect.Id == effectId && effect.IsAlive)
            {
                return true;
            }
        }

        return false;
    }

    public bool ShouldEffectBeActive(AdditionalEffect effect)
    {
        if (!effect.IsAlive)
        {
            return false;
        }

        EffectBeginConditionOwnerMetadata owner = effect.LevelMetadata.BeginCondition.Owner;

        if (owner?.HasBuffId != null)
        {
            foreach (int buffId in owner.HasBuffId)
            {
                if (!HasEffect(buffId))
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static int GetTriggerCooldown(int skillId, int skillLevel)
    {
        SkillMetadata skillMeta = SkillMetadataStorage.GetSkill(skillId);

        if (skillMeta?.SkillLevels?[skillLevel] != null)
        {
            return (int) (1000 * skillMeta.SkillLevels[skillLevel].CooldownTime);
        }

        AdditionalEffectLevelMetadata levelMeta = AdditionalEffectMetadataStorage.GetLevelMetadata(skillId, skillLevel);

        if (levelMeta?.Basic != null)
        {
            return (int) (1000 * levelMeta.Basic.CooldownTime);
        }

        return 0;
    }

    public void FireTriggerSkill(AdditionalEffect parent, EffectTriggerSkillMetadata trigger, SkillCast parentSkill, IFieldActor target, int start = -1)
    {
        if (start == -1)
        {
            start = Environment.TickCount;
        }

        for (int skillIndex = 0; skillIndex < trigger.SkillId.Length; ++skillIndex)
        {
            int skillId = trigger.SkillId[skillIndex];
            int skillLevel = trigger.SkillLevel[skillIndex];

            if (parent.SkillFiredLast.TryGetValue(skillId, out int lastFired))
            {
                if (start - lastFired < GetTriggerCooldown(skillId, skillLevel))
                {
                    continue;
                }
            }

            float probability = trigger.BeginCondition?.Probability ?? 1;

            if (probability != 1 && probability < Random.Shared.NextDouble())
            {
                continue;
            }

            parent.SkillFiredLast[skillId] = start;

            SkillCast skillCast = new(skillId, (short) skillLevel, parentSkill.SkillSn, start, parentSkill)
            {
                CasterObjectId = parentSkill.CasterObjectId,
                Position = Parent.Coord,
                Interval = trigger.Interval,
                Duration = parent?.LevelMetadata?.Basic?.DurationTick ?? 100
            };

            if (trigger.Splash == true)
            {
                RegionSkillHandler.HandleEffect(Parent.FieldManager, skillCast, skillCast.SkillAttack.AttackPoint);

                continue;
            }

            target.AdditionalEffects.AddEffect(new(skillId, skillLevel)
            {
                Duration = skillCast.DurationTick(),
                Source = parentSkill.CasterObjectId,
                ParentSkill = parentSkill
            });
        }
    }

    public void SkillTrigger(IFieldActor target, SkillCast skillActivated)
    {
        foreach (AdditionalEffect effect in Effects)
        {
            if (!effect.ListensForSkills)
            {
                continue;
            }

            if (!ShouldEffectBeActive(effect))
            {
                continue;
            }

            foreach (EffectTriggerSkillMetadata skill in effect.LevelMetadata.ConditionSkill)
            {
                if (skill.BeginCondition.Owner.EventSkillIDs == null)
                {
                    FireTriggerSkill(effect, skill, skillActivated, target, Environment.TickCount);

                    continue;
                }

                foreach (int skillId in skill.BeginCondition.Owner.EventSkillIDs)
                {
                    if (skillId == skillActivated.SkillId)
                    {
                        FireTriggerSkill(effect, skill, skillActivated, target, Environment.TickCount);
                    }
                }
            }
        }
    }

    public void EffectTrigger(IFieldActor target, AdditionalEffect effectActivated)
    {
        foreach (AdditionalEffect effect in Effects)
        {
            if (!effect.ListensForEffects)
            {
                continue;
            }

            foreach (EffectTriggerSkillMetadata skill in effect.LevelMetadata.ConditionSkill)
            {
                foreach (int effectId in skill.BeginCondition.Owner.EventSkillIDs)
                {
                    if (effectId == effect.Id)
                    {
                        FireTriggerSkill(effect, skill, effectActivated.ParentSkill, target, Environment.TickCount);
                    }
                }
            }
        }
    }

    public int FindExpiredBuff()
    {
        int index = -1;

        for (int i = 0; i < TimedEffects.Count; ++i)
        {
            if (TimedEffects[i].End < Environment.TickCount)
            {
                index = i;
            }
        }

        if (index == -1)
        {
            return -1;
        }

        for (int i = 0; i < Effects.Count; ++i)
        {
            if (Effects[i] == TimedEffects[index])
            {
                return i;
            }
        }

        return -1;
    }

    public void AddExpiringBuffTime(int time)
    {
        if (NextBuffExpiration == -1)
        {
            NextBuffExpiration = time;

            return;
        }

        NextBuffExpiration = Math.Min(NextBuffExpiration, time);
    }

    public void RecomputeExpiration()
    {
        if (TimedEffects.Count > 0)
        {
            NextBuffExpiration = TimedEffects[0].End;

            for (int i = 1; i < TimedEffects.Count; ++i)
            {
                NextBuffExpiration = Math.Min(NextBuffExpiration, TimedEffects[i].End);
            }

            return;
        }

        NextBuffExpiration = -1;
    }

    public void RemoveAt(int index, bool removeTimed = false)
    {
        List<AdditionalEffect> list = removeTimed ? TimedEffects : Effects;

        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }

    public bool TryGet(int id, int level, out AdditionalEffect effect, out int index, bool getTimed = false)
    {
        effect = null;

        List<AdditionalEffect> list = getTimed ? TimedEffects : Effects;

        for (index = 0; index < list.Count; ++index)
        {
            if (list[index].Matches(id))
            {
                effect = list[index];

                return true;
            }
        }

        return false;
    }

    public bool TryGet(int id, int level, out AdditionalEffect effect, bool getTimed = false)
    {
        int index = 0;

        return TryGet(id, level, out effect, out index, getTimed);
    }

    public int CountEffects(AdditionalEffectLevelMetadata level)
    {
        int count = 0;

        return count;
    }
}

public class AdditionalEffect
{
    public int Id;
    public int Level;
    public AdditionalEffectMetadata Metadata;
    public AdditionalEffectLevelMetadata LevelMetadata;
    public int References = 0; // Reference counter. Only remove 
    public int Stacks = 1;
    public int SourceId = -1;
    public int BuffId = -1;
    public int Start = -1;
    public int Duration = 0;
    public int End { get => Start + Duration; }
    public int TickRate;
    public bool IsAlive = true;
    public SkillCast ParentSkill;
    public bool ListensForSkills { get; }
    public bool ListensForEffects { get; }
    public Dictionary<int, int> SkillFiredLast = new();

    public AdditionalEffect(int id, int level, int stacks = 1)
    {
        Id = id;
        Level = level;
        Stacks = stacks;

        Metadata = AdditionalEffectMetadataStorage.GetMetadata(id);
        LevelMetadata = AdditionalEffectMetadataStorage.GetLevelMetadata(id, level);

        if (LevelMetadata?.ConditionSkill == null)
        {
            return;
        }

        foreach (EffectTriggerSkillMetadata skill in LevelMetadata.ConditionSkill)
        {
            if (skill.BeginCondition?.Owner != null)
            {
                ListensForSkills = (skill.BeginCondition.Owner.EventSkillIDs?.Length ?? 0) > 0;
                ListensForEffects = (skill.BeginCondition.Owner.EventEffectIDs?.Length ?? 0) > 0;
            }
        }

        ListensForSkills |= (LevelMetadata.BeginCondition?.Owner?.HasBuffId?.Length ?? 0) > 0;
    }

    public bool Matches(int id)
    {
        return Id == id;
    }
}
