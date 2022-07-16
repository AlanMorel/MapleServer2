using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2Storage.Types.Metadata;
using Maple2Storage.Types;
using Maple2Storage.Enums;
using MapleServer2.Data.Static;

namespace MapleServer2.Types;

public struct EffectTriggers
{
    public bool IsEvent;
    public int SkillId;
    public int EffectId;
    public IFieldActor Owner;
    public IFieldActor Target;
    public IFieldActor Caster;

    public EffectTriggers()
    {
        IsEvent = false;
        SkillId = 0;
        EffectId = 0;
        Owner = null;
        Target = null;
        Caster = null;
    }

    public void CopyEvents(EffectTriggers source)
    {
        IsEvent = source.IsEvent;

        if (!IsEvent)
        {
            return;
        }

        SkillId = source.SkillId;
        EffectId = source.EffectId;
    }
}

public class SkillTriggerHandler
{
    public IFieldActor Parent;
    public Dictionary<int, int> SkillFiredLast = new();
    public Dictionary<int, int> HostileSkillProccedLast = new();

    public SkillTriggerHandler(IFieldActor parent)
    {
        Parent = parent;
    }

    public IFieldActor GetTarget(EffectTriggers triggers, ApplyTarget target)
    {
        return target switch
        {
            ApplyTarget.Closest => triggers.Target, // TODO: change to fetch closest instead
            ApplyTarget.Owner => triggers.Owner,
            ApplyTarget.Target => triggers.Target,
            ApplyTarget.Caster => triggers.Caster,
            //ApplyTarget.PetOwner => 4,
            //ApplyTarget.Attacker => 5,
            //ApplyTarget.RegionBuff => 6,
            //ApplyTarget.RegionDebuff => 7,
            //ApplyTarget.HungryMobs => 8
            _ => null
        };
    }

    public IFieldActor GetOwner(EffectTriggers triggers, SkillOwner owner)
    {
        return owner switch
        {
            //SkillOwner.None => triggers.Target,
            SkillOwner.Owner => triggers.Owner,
            SkillOwner.Target => triggers.Target,
            SkillOwner.Caster => triggers.Caster,
            //SkillOwner.PetOwner => 4,
            //SkillOwner.Attacker => 5,
            //SkillOwner.RegionBuff => 6,
            //SkillOwner.RegionDebuff => 7,
            _ => null
        };
    }

    public bool IsConditionMet(BeginConditionSubject subjectCondition, EffectTriggers triggers, ApplyTarget target)
    {
        if (subjectCondition == null)
        {
            return true;
        }

        IFieldActor conditionTarget = GetTarget(triggers, target);

        if (conditionTarget == null)
        {
            return false;
        }

        if (!conditionTarget.AdditionalEffects.HasEffect(subjectCondition.HasBuffId, subjectCondition.HasBuffCount))
        {
            return false;
        }

        if (subjectCondition.HasNotBuffId != 0 && conditionTarget.AdditionalEffects.HasEffect(subjectCondition.HasNotBuffId, subjectCondition.HasBuffCount))
        {
            return false;
        }

        return true;
    }

    public bool IsConditionMet(SkillBeginCondition condition, EffectTriggers triggers, bool checkEvent = true)
    {
        BeginConditionSubject owner = condition.Owner;

        bool isSkillEvent = triggers.SkillId != 0;
        bool listeningForSkills = condition.Owner?.EventSkillIDs != null;

        bool conditionMet = !checkEvent || isSkillEvent == listeningForSkills;

        conditionMet &= !checkEvent || (!isSkillEvent && (condition.Owner?.EventSkillIDs?.Length ?? 0) == 0) || condition.Owner?.EventSkillIDs?.Contains(triggers.SkillId) == true;
        conditionMet &= IsConditionMet(condition.Owner, triggers, ApplyTarget.Owner);
        conditionMet &= IsConditionMet(condition.Target, triggers, ApplyTarget.Target);
        conditionMet &= IsConditionMet(condition.Caster, triggers, ApplyTarget.Caster);

        return conditionMet;
    }

    public bool IsConditionMet(SkillCondition trigger, EffectTriggers triggers)
    {
        return IsConditionMet(trigger.BeginCondition, triggers);
    }

    public bool ShouldEffectBeActive(AdditionalEffect effect, EffectTriggers triggers)
    {
        if (!effect.IsAlive)
        {
            return false;
        }

        return IsConditionMet(effect.LevelMetadata.BeginCondition, triggers, false);
    }

    public static int GetTriggerCooldown(int skillId, int skillLevel)
    {
        SkillMetadata skillMeta = SkillMetadataStorage.GetSkill(skillId);
        SkillLevel level = skillMeta?.SkillLevels?.FirstOrDefault(level => level.Level == skillLevel);

        if (level != null)
        {
            return (int) (1000 * level.CooldownTime);
        }

        AdditionalEffectLevelMetadata levelMeta = AdditionalEffectMetadataStorage.GetLevelMetadata(skillId, skillLevel);

        if (levelMeta?.Basic != null)
        {
            return (int) (1000 * levelMeta.Basic.CooldownTime);
        }

        return 0;
    }

    public void FireTriggerSkill(SkillCondition trigger, SkillCast parentSkill, EffectTriggers triggers, int start = -1)
    {
        if (trigger == null)
        {
            return;
        }

        if (!IsConditionMet(trigger, triggers))
        {
            IsConditionMet(trigger, triggers);
            return;
        }

        if (start == -1)
        {
            start = Environment.TickCount;
        }

        IFieldActor target = GetTarget(triggers, trigger.Target);

        for (int skillIndex = 0; skillIndex < trigger.SkillId.Length; ++skillIndex)
        {
            int skillId = trigger.SkillId[skillIndex];
            int skillLevel = trigger.SkillLevel[skillIndex];
            int cooldown = GetTriggerCooldown(skillId, skillLevel);
            int lastProcced = target.SkillTriggerHandler.HostileSkillProccedLast.GetValueOrDefault(skillId, start - cooldown);

            if (start - lastProcced < GetTriggerCooldown(skillId, skillLevel))
            {
                continue;
            }

            float probability = trigger.BeginCondition?.Probability ?? 1;

            if (probability != 1 && probability < Random.Shared.NextDouble())
            {
                continue;
            }

            target.SkillTriggerHandler.HostileSkillProccedLast[skillId] = start;

            if (!trigger.IsSplash)
            {

                AdditionalEffectParameters effectParams = new(skillId, skillLevel)
                {
                    Caster = triggers.Caster,
                    ParentSkill = parentSkill
                };

                target.AdditionalEffects.AddEffect(effectParams);

                continue;
            }

            SkillCast skillCast;

            int duration = trigger.FireCount * trigger.Interval + trigger.RemoveDelay;

            if (parentSkill != null)
            {
                skillCast = new(skillId, (short) skillLevel, parentSkill.SkillSn, start, parentSkill)
                {
                    Caster = parentSkill.Caster,
                    Position = target.Coord,
                    Rotation = trigger.UseDirection ? parentSkill.Rotation : new CoordF(),
                    Direction = trigger.UseDirection ? parentSkill.Direction : new CoordF(),
                    LookDirection = trigger.UseDirection ? parentSkill.LookDirection : (short) 0,
                    Duration = duration
                };
            }
            else
            {
                skillCast = new(skillId, (short) skillLevel, 0, start)
                {
                    Caster = Parent,
                    Position = target.Coord,
                    Rotation = new CoordF(),
                    Direction = new CoordF(),
                    LookDirection = 0,
                    Duration = duration
                };
            }

            skillCast.Owner = target;

            if (trigger.FireCount == 1 && trigger.Delay == 0)
            {
                RegionSkillHandler.HandleEffect(Parent.FieldManager, skillCast);

                continue;
            }

            Task.Run(async () =>
            {
                await Task.Delay((int)trigger.Delay);

                for (int i = 0; i < trigger.FireCount; ++i)
                {
                    RegionSkillHandler.HandleEffect(Parent.FieldManager, skillCast);

                    await Task.Delay(trigger.Interval);
                }
            });
        }
    }

    public void FireTriggers(List<SkillCondition> triggers, EffectTriggers parameters, SkillCast parentSkill = null, int start = -1)
    {
        if (triggers == null)
        {
            return;
        }

        foreach (SkillCondition trigger in triggers)
        {
            FireTriggerSkill(trigger, parentSkill, parameters, start);
        }
    }

    public void FireTriggers(AdditionalEffect effect, EffectTriggers parameters, SkillCast parentSkill, bool checkCondition = true)
    {
        if (checkCondition && !ShouldEffectBeActive(effect, parameters))
        {
            return;
        }

        int start = Environment.TickCount;

        if (start - effect.LastTick < effect.LevelMetadata.Basic.DelayTick)
        {
            return;
        }

        FireTriggers(effect.LevelMetadata.ConditionSkill, parameters, parentSkill);
        FireTriggers(effect.LevelMetadata.SplashSkill, parameters, parentSkill);
    }

    public void SkillTrigger(IFieldActor target, SkillCast skillActivated)
    {
        AdditionalEffect listening = Parent.AdditionalEffects.FindListeningForSkill(skillActivated.SkillId);

        if (listening == null)
        {
            return;
        }

        EffectTriggers triggers = new()
        {
            IsEvent = true,
            SkillId = skillActivated.SkillId,
            Target = target,
            Owner = Parent,
            Caster = skillActivated.Caster
        };

        Parent.AdditionalEffects.BufferNewEffects();

        FireTriggers(listening, triggers, skillActivated);

        Parent.AdditionalEffects.FlushEffectsBuffer();
    }

    public void EffectTrigger(IFieldActor target, AdditionalEffect effectActivated)
    {
        EffectTriggers triggers = new()
        {
            IsEvent = true,
            EffectId = effectActivated.Id,
            Target = target,
        };

        Parent.AdditionalEffects.BufferNewEffects();

        foreach (AdditionalEffect effect in Parent.AdditionalEffects.Effects)
        {
            FireTriggers(effect, triggers, effect.ParentSkill);
        }

        Parent.AdditionalEffects.FlushEffectsBuffer();
    }
}
