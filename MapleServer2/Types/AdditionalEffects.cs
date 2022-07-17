using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public struct AdditionalEffectParameters
{
    public int Id;
    public int Level;
    public int Stacks;
    public IFieldActor Caster;
    public int Duration;
    public bool IsBuff;
    public bool AdjustDuration;
    public SkillCast ParentSkill;

    public AdditionalEffectParameters(int id, int level)
    {
        Id = id;
        Level = level;
        Stacks = 1;
        Caster = null;
        Duration = 0;
        IsBuff = false;
        AdjustDuration = true;
        ParentSkill = null;
    }
}

public class AdditionalEffects
{
    public List<AdditionalEffect> Effects;
    public List<AdditionalEffect> TimedEffects;
    public IFieldActor Parent;
    private AdditionalEffect NextExpiringBuff = null;
    public bool AlwaysCrit { get; private set; }
    public bool Invincible { get; private set; }
    private List<(int, AdditionalEffect)> ListeningSkillIds = new();

    public AdditionalEffects(IFieldActor parent = null)
    {
        Effects = new();
        TimedEffects = new();
        Parent = parent;
        AlwaysCrit = false;
        Invincible = false;
    }

    public void UpdateEffects()
    {
        int currentTick = Environment.TickCount;

        while (NextExpiringBuff != null && currentTick - NextExpiringBuff.Start > NextExpiringBuff.Duration)
        {
            int index = GetEffectIndex(NextExpiringBuff.Id);

            RemoveEffect(NextExpiringBuff, index);

            NextExpiringBuff = FindNextExpiringBuff();
        }
    }

    private int BufferingNewEffects;
    private List<AdditionalEffectParameters> NewEffectBuffer = new();
    private List<AdditionalEffectParameters> NewEffectBackBuffer = new();
    private bool FlushingBackBuffer;

    public void BufferNewEffects()
    {
        ++BufferingNewEffects;
    }

    public void FlushEffectsBuffer()
    {
        --BufferingNewEffects;

        if (BufferingNewEffects != 0 || FlushingBackBuffer)
        {
            return;
        }

        FlushingBackBuffer = true;

        while (NewEffectBuffer.Count > 0)
        {
            List<AdditionalEffectParameters> buffer = NewEffectBuffer;
            NewEffectBuffer = NewEffectBackBuffer;
            NewEffectBackBuffer = buffer;

            foreach (AdditionalEffectParameters effectParams in buffer)
            {
                AddEffect(effectParams);
            }

            buffer.Clear();
        }

        FlushingBackBuffer = false;
    }

    public AdditionalEffect AddEffect(AdditionalEffectParameters parameters)
    {
        if (BufferingNewEffects != 0)
        {
            NewEffectBuffer.Add(parameters);

            return null;
        }

        // current default behavior of remove and replace, and add stacks
        // doesnt check for shared buff categories (sigils, whetstones, etc)
        // TODO: add correct refreshing behavior based on attributes from the xmls

        AdditionalEffect effect = new(parameters.Id, parameters.Level, parameters.Stacks);
        int oldEffectIndex = -1;
        int start = Environment.TickCount;

        if (effect.LevelMetadata == null)
        {
            return null;
        }

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
                parameters.Stacks = Math.Max(Math.Min(oldEffect.LevelMetadata.Basic.MaxBuffCount, parameters.Stacks + oldEffect.Stacks), 0);

                oldEffect.Level = effect.Level;
                oldEffect.LevelMetadata = effect.LevelMetadata;
                effect = oldEffect;
                oldEffectIndex = i;

                if (parameters.ParentSkill == null)
                {
                    parameters.ParentSkill = oldEffect.ParentSkill;
                    parameters.Caster = oldEffect.Caster;
                }

                if (!parameters.AdjustDuration)
                {
                    start = oldEffect.Start;
                    parameters.Duration = oldEffect.Duration;
                }

                break;
            }
        }

        for (int i = 0; i < Effects.Count; i++)
        {
            AdditionalEffect oldEffect = Effects[i];

            if (cancel?.CancelEffectCodes?.Contains(oldEffect.Id) == true)
            {
                RemoveEffect(oldEffect, i, true);
            }
        }

        if (parameters.Stacks == 0)
        {
            if (oldEffectIndex != -1)
            {
                RemoveEffect(effect, oldEffectIndex, true);
            }

            return null;
        }

        effect.ParentSkill = parameters.ParentSkill;
        effect.Stacks = parameters.Stacks;
        effect.Caster = parameters.Caster ?? Parent;
        effect.Start = start;
        effect.Duration = effect.LevelMetadata.Basic?.DurationTick ?? parameters.Duration;
        effect.TickRate = effect.LevelMetadata.Basic?.IntervalTick ?? 0;

        if (oldEffectIndex != -1)
        {
            Parent.FieldManager.BroadcastPacket(BuffPacket.UpdateBuff(effect, Parent.ObjectId));
        }

        if (oldEffectIndex == -1)
        {
            effect.BuffId = GuidGenerator.Int();
            effect.LastTick = start - effect.TickRate;

            if (parameters.Duration == 0)
            {
                --effect.Start;
            }

            Effects.Add(effect);

            Parent.FieldManager.BroadcastPacket(BuffPacket.AddBuff(effect, Parent.ObjectId));

            DotEffect(effect, parameters);

            if (effect.Duration != 0)
            {
                TimedEffects.Add(effect);
            }

            AddListeningSkills(effect);
        }

        if (effect.Duration != 0)
        {
            NextExpiringBuff = FindNextExpiringBuff();
        }

        Parent.EffectAdded(effect);
        ModifyEffect(effect);
        ApplyStatuses(effect);

        if (oldEffectIndex == -1)
        {
            EffectTriggers triggers = new()
            {
                Caster = effect.Caster,
                Owner = Parent,
                Target = Parent
            };

            Parent.SkillTriggerHandler.FireTriggers(effect, triggers, parameters.ParentSkill, false);
        }

        return effect;
    }

    public void DotEffect(AdditionalEffect effect, AdditionalEffectParameters parameters)
    {
        EffectDotDamageMetadata dotDamage = effect.LevelMetadata.DotDamage;

        // TODO: fix dot damage handling with game session tracking

        GameSession session = null;

        if (effect.Caster is Character character)
        {
            session = character.Value.Session;
        }

        DamageSourceParameters dotParameters = null;

        if ((dotDamage?.Rate ?? 0) != 0 && effect.Caster != null && session != null)
        {
            dotParameters = new()
            {
                IsSkill = false,
                GuaranteedCrit = effect.Caster.AdditionalEffects.AlwaysCrit,
                CanCrit = effect.LevelMetadata.DotDamage.UseGrade,
                Element = (Element) dotDamage.Element,
                RangeType = SkillRangeType.Special,
                DamageType = (DamageType) dotDamage.DamageType,
                DamageRate = dotDamage.Rate * effect.Stacks,
                ParentSkill = parameters.ParentSkill
            };
        }

        if (dotParameters != null)
        {
            DamageHandler.ApplyDotDamage(session, effect.Caster, Parent, dotParameters);
        }

        if (effect.TickRate < effect.Duration && effect.TickRate != 0)
        {
            Task.Run(async () =>
            {
                EffectTriggers triggers = new()
                {
                    Caster = effect.Caster,
                    Owner = Parent,
                    Target = Parent
                };

                while (effect.IsAlive && Environment.TickCount - effect.Start < effect.Duration)
                {
                    await Task.Delay(effect.TickRate);

                    if (!effect.IsAlive)
                    {
                        break;
                    }

                    if (dotParameters != null)
                    {
                        dotParameters.GuaranteedCrit = effect.Caster.AdditionalEffects.AlwaysCrit;
                        dotParameters.DamageRate = dotDamage.Rate * effect.Stacks;

                        DamageHandler.ApplyDotDamage(session, effect.Caster, Parent, dotParameters);
                    }

                    Parent.SkillTriggerHandler.FireTriggers(effect, triggers, parameters.ParentSkill, false);
                }
            });
        }
    }

    public void ModifyEffect(AdditionalEffect effect)
    {
        int[] overlapCodes = effect.LevelMetadata.ModifyOverlapCount?.EffectCodes;

        if ((overlapCodes?.Length ?? 0) > 0)
        {
            for (int i = 0; i < overlapCodes.Length; ++i)
            {
                AdditionalEffect affectedEffect = GetEffect(overlapCodes[i]);

                if (affectedEffect == null)
                {
                    continue;
                }

                AddEffect(new(affectedEffect.Id, affectedEffect.Level)
                {
                    Stacks = effect.LevelMetadata.ModifyOverlapCount.OffsetCounts[i],
                    AdjustDuration = false
                });
            }
        }

        EffectModifyDurationMetadata modifyDuration = effect.LevelMetadata.ModifyEffectDuration;
        int[] durationCodes = modifyDuration?.EffectCodes;

        if (durationCodes?.Length > 0)
        {
            for (int i = 0; i < durationCodes.Length; ++i)
            {
                AdditionalEffect affectedEffect = GetEffect(durationCodes[i]);

                if (affectedEffect == null)
                {
                    continue;
                }

                affectedEffect.Duration = (int) (modifyDuration.DurationFactors[i] * affectedEffect.Duration) + (int) (1000 * modifyDuration.DurationValues[i]);

                Parent.FieldManager.BroadcastPacket(BuffPacket.UpdateBuff(effect, Parent.ObjectId));
            }
        }
    }

    public void ApplyStatuses(AdditionalEffect effect)
    {
        AlwaysCrit |= effect.LevelMetadata.Offensive.AlwaysCrit;
        Invincible |= effect.LevelMetadata.Defesive.Invincible;
    }

    public void RemoveEffect(int id, int level, bool sendPacket = true)
    {
        int index;
        AdditionalEffect effect;

        if (TryGet(id, out effect, out index))
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

            if (TryGet(effect.Id, out AdditionalEffect removeEffect, out timedIndex, true) && removeEffect.LevelMetadata != null)
            {
                RemoveAt(timedIndex, true);
            }

            NextExpiringBuff = FindNextExpiringBuff();
        }

        if (effect.LevelMetadata.Offensive.AlwaysCrit || effect.LevelMetadata.Defesive.Invincible)
        {
            AlwaysCrit = false;
            Invincible = false;

            foreach (AdditionalEffect remainingEffect in Effects)
            {
                ApplyStatuses(remainingEffect);
            }
        }

        if (HasListeningSkills(effect))
        {
            RefreshListeningSkills();
        }
    }

    public bool HasEffect(int effectId, int stacks = 0)
    {
        if (effectId == 0)
        {
            return true;
        }

        AdditionalEffect effect = GetEffect(effectId);

        if (effect == null)
        {
            return false;
        }

        if (stacks != 0)
        {
            return effect.Stacks == stacks;
        }

        return true;
    }

    public int GetEffectIndex(int effectId)
    {
        for (int index = 0; index < Effects.Count; ++index)
        {
            if (Effects[index].Id == effectId && Effects[index].IsAlive)
            {
                return index;
            }
        }

        return -1;
    }

    public AdditionalEffect GetEffect(int effectId)
    {
        int index = GetEffectIndex(effectId);

        return index == -1 ? null : Effects[index];
    }

    public AdditionalEffect FindNextExpiringBuff()
    {
        int currentTick = Environment.TickCount;

        AdditionalEffect effect = null;
        int lowestRemaining = int.MaxValue;

        foreach (AdditionalEffect timedEffect in TimedEffects)
        {
            if (timedEffect.Duration == 0)
            {
                continue;
            }

            int elapsed = currentTick - timedEffect.Start;

            if (elapsed >= timedEffect.Duration)
            {
                return timedEffect;
            }

            int remaining = timedEffect.Duration - elapsed;

            if (remaining < lowestRemaining)
            {
                lowestRemaining = remaining;
                effect = timedEffect;
            }
        }

        return effect;
    }

    public void RemoveAt(int index, bool removeTimed = false)
    {
        List<AdditionalEffect> list = removeTimed ? TimedEffects : Effects;

        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }

    public bool TryGet(int id, out AdditionalEffect effect, out int index, bool getTimed = false)
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

        return TryGet(id, out effect, out index, getTimed);
    }

    public bool IsListeningForSkill(int skillId)
    {
        foreach ((int id, AdditionalEffect effect) in ListeningSkillIds)
        {
            if (id == skillId)
            {
                return true;
            }
        }

        return false;
    }
    public AdditionalEffect FindListeningForSkill(int skillId)
    {
        foreach ((int id, AdditionalEffect effect) in ListeningSkillIds)
        {
            if (id == skillId)
            {
                return effect;
            }
        }

        return null;
    }

    public void AddListeningSkills(AdditionalEffect effect)
    {
        if (effect.LevelMetadata.ConditionSkill == null)
        {
            return;
        }

        foreach (SkillCondition condition in effect.LevelMetadata.ConditionSkill)
        {
            if (condition.BeginCondition?.Owner?.EventSkillIDs == null)
            {
                continue;
            }

            foreach (int id in condition.BeginCondition.Owner.EventSkillIDs)
            {
                ListeningSkillIds.Add((id, effect));
            }
        }
    }

    public bool HasListeningSkills(AdditionalEffect effect)
    {
        if (effect.LevelMetadata.ConditionSkill == null)
        {
            return false;
        }

        foreach (SkillCondition condition in effect.LevelMetadata.ConditionSkill)
        {
            if ((condition.BeginCondition?.Owner?.EventSkillIDs?.Length ?? 0) > 0)
            {
                return true;
            }
        }

        return false;
    }

    public void RefreshListeningSkills()
    {
        ListeningSkillIds.Clear();

        foreach (AdditionalEffect effect in Effects)
        {
            AddListeningSkills(effect);
        }
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
    public IFieldActor Caster;
    public int BuffId = -1;
    public int Start = -1;
    public int Duration = 0;
    public int End { get => Start + Duration; }
    public int TickRate;
    public bool IsAlive = true;
    public SkillCast ParentSkill;
    public int LastTick = 0;

    public AdditionalEffect(int id, int level, int stacks = 1)
    {
        Id = id;
        Level = level;
        Stacks = stacks;

        Metadata = AdditionalEffectMetadataStorage.GetMetadata(id);
        LevelMetadata = AdditionalEffectMetadataStorage.GetLevelMetadata(id, level);
    }

    public bool Matches(int id)
    {
        return Id == id;
    }
}
