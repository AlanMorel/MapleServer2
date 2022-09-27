using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Types;

public class SkillTriggerHandler
{
    public IFieldActor Parent;
    public Dictionary<int, int> SkillFiredLast = new();
    public Dictionary<int, int> HostileSkillProccedLast = new();

    public SkillTriggerHandler(IFieldActor parent)
    {
        Parent = parent;
    }

    public IFieldActor? GetPetOwner()
    {
        if (Parent is Pet pet)
        {
            return pet.Owner;
        }

        return null;
    }

    public IFieldActor? GetTarget(SkillTarget target, ConditionSkillTarget castInfo)
    {
        return target switch
        {
            SkillTarget.SkillTarget => castInfo.Target,
            SkillTarget.Owner => castInfo.Owner,
            SkillTarget.Target => castInfo.Target,
            SkillTarget.Caster => castInfo.Caster,
            SkillTarget.PetOwner => GetPetOwner(),
            _ => null
        };
    }

    public IFieldActor? GetOwner(SkillOwner owner, ConditionSkillTarget castInfo)
    {
        return owner switch
        {
            SkillOwner.Inherit => castInfo.Caster,
            SkillOwner.Owner => castInfo.Owner,
            SkillOwner.Target => castInfo.Target,
            SkillOwner.Caster => castInfo.Caster,
            SkillOwner.Attacker => castInfo.Attacker,
            _ => null
        };
    }

    public bool IsConditionMet(BeginConditionSubject? subjectCondition, IFieldActor? subject, EffectEvent effectEvent, int eventIdArgument)
    {
        if (subjectCondition == null)
        {
            return true;
        }

        if (subject == null)
        {
            return false;
        }

        if (subjectCondition.EventCondition != EffectEvent.Activate)
        {
            bool eventMatches = effectEvent switch
            {
                EffectEvent.Activate => true,
                EffectEvent.OnEvade => true,
                EffectEvent.OnBlock => true,
                EffectEvent.OnAttacked => true,
                EffectEvent.OnOwnerAttackCrit => true,
                EffectEvent.OnOwnerAttackHit => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || subjectCondition.EventSkillIDs.Contains(eventIdArgument),
                EffectEvent.OnSkillCasted => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || subjectCondition.EventSkillIDs.Contains(eventIdArgument),

                EffectEvent.OnBuffStacksReached => subjectCondition.HasBuffId == eventIdArgument,
                EffectEvent.OnInvestigate => true,
                EffectEvent.OnBuffTimeExpiring => true, // check
                EffectEvent.OnSkillCastEnd => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || subjectCondition.EventSkillIDs.Contains(eventIdArgument),
                EffectEvent.OnEffectApplied => subjectCondition.HasBuffId == 0 ? (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || subjectCondition.EventEffectIDs.Contains(eventIdArgument) : subjectCondition.HasBuffId == eventIdArgument,
                EffectEvent.OnEffectRemoved => (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || subjectCondition.EventEffectIDs.Contains(eventIdArgument),
                EffectEvent.OnLifeSkillGather => true,
                EffectEvent.OnAttackMiss => true,

                EffectEvent.UnknownKritiasPuzzleEvent => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || subjectCondition.EventSkillIDs.Contains(eventIdArgument),
                EffectEvent.UnknownWizardEvent => (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || subjectCondition.EventEffectIDs.Contains(eventIdArgument),
                EffectEvent.UnknownStrikerEvent => subjectCondition.HasBuffId == 0 || subjectCondition.HasBuffId == eventIdArgument,
                _ => true
            };

            if (!eventMatches)
            {
                return false;
            }
        }

        ConditionOperator buffOperator = subjectCondition.HasBuffCount == 0 ? ConditionOperator.GreaterEquals : subjectCondition.HasBuffCountCompare;

        if (subjectCondition.HasBuffId != 0 && !subject.AdditionalEffects.HasEffect(subjectCondition.HasBuffId, subjectCondition.HasBuffCount, buffOperator, subjectCondition.HasBuffLevel))
        {
            return false;
        }

        if (subjectCondition.HasNotBuffId != 0 && subject.AdditionalEffects.HasEffect(subjectCondition.HasNotBuffId))
        {
            return false;
        }

        if (subjectCondition.CompareStat is not null)
        {
            Stat health = Parent.Stats[StatAttribute.Hp];

            if (!AdditionalEffects.CompareValues(health.Total, subjectCondition.CompareStat.Hp, subjectCondition.CompareStat.Func))
            {
                return false;
            }
        }

        return true;
    }

    public bool IsConditionMet(SkillBeginCondition condition, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument)
    {
        if (!condition.AllowDeadState && Parent.IsDead)
        {
            return false;
        }

        float probability = condition.Probability;

        if (probability != 1 && probability < Random.Shared.NextDouble())
        {
            return false;
        }

        bool subjectCondition = IsConditionMet(condition.Target, castInfo.Target, effectEvent, eventIdArgument);
        subjectCondition &= IsConditionMet(condition.Owner, castInfo.Owner, effectEvent, eventIdArgument);
        subjectCondition &= IsConditionMet(condition.Caster, castInfo.Caster, effectEvent, eventIdArgument);

        if (!subjectCondition)
        {
            return false;
        }

        if (condition.Stat is not null)
        {
            Stat health = Parent.Stats[StatAttribute.Hp];

            if (health.TotalLong < condition.Stat.Hp)
            {
                return false;
            }

            Stat spirit = Parent.Stats[StatAttribute.Spirit];

            if (spirit.TotalLong < condition.Stat.Sp)
            {
                return false;
            }
        }

        return true;
    }

    public EffectEvent GetEvent(BeginConditionSubject? subjectCondition)
    {
        if (subjectCondition == null)
        {
            return EffectEvent.Activate;
        }

        return subjectCondition.EventCondition;
    }

    public bool EventMatches(EffectEvent effectEvent, EffectEvent listener, ref bool foundEvent)
    {
        foundEvent |= effectEvent == listener;

        return effectEvent == listener || listener == EffectEvent.Activate;
    }

    public bool EventMatches(SkillBeginCondition condition, EffectEvent effectEvent)
    {
        bool foundEvent = false;

        bool noConflictsFound = EventMatches(effectEvent, GetEvent(condition.Owner), ref foundEvent);
        noConflictsFound &= EventMatches(effectEvent, GetEvent(condition.Caster), ref foundEvent);
        noConflictsFound &= EventMatches(effectEvent, GetEvent(condition.Target), ref foundEvent);

        return foundEvent && noConflictsFound;
    }

    public static int GetEffectCooldown(int effectId, int effectLevel)
    {
        AdditionalEffectLevelMetadata? levelMeta = AdditionalEffectMetadataStorage.GetLevelMetadata(effectId, effectLevel);

        if (levelMeta != null)
        {
            return (int) (1000 * levelMeta.Basic.CooldownTime);
        }

        return 0;
    }

    public bool IsEffectOnCooldown(ConditionSkillTarget castInfo, int skillId, int skillLevel, int start = -1)
    {
        if (start == -1)
        {
            start = Environment.TickCount;
        }

        int cooldown = GetEffectCooldown(skillId, skillLevel);

        if (cooldown == 0)
        {
            return false;
        }

        int lastProcced = castInfo.Target.SkillTriggerHandler.HostileSkillProccedLast.GetValueOrDefault(skillId, start - cooldown);

        return start - lastProcced < cooldown;
    }

    public bool ShouldTick(SkillBeginCondition beginCondition, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument = 0)
    {
        if (!EventMatches(beginCondition, effectEvent))
        {
            return false;
        }

        return IsConditionMet(beginCondition, castInfo, effectEvent, eventIdArgument);
    }

    public bool ShouldFireTrigger(SkillCondition trigger, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        if (!EventMatches(trigger.BeginCondition, effectEvent))
        {
            return false;
        }

        if (start == -1)
        {
            start = Environment.TickCount;
        }

        if (!IsConditionMet(trigger.BeginCondition, castInfo, effectEvent, eventIdArgument))
        {
            return false;
        }

        for (int i = 0; i < trigger.SkillId.Length; ++i)
        {
            int skillId = trigger.SkillId[i];
            int skillLevel = trigger.SkillLevel[i];

            if (!trigger.IsSplash && IsEffectOnCooldown(castInfo, skillId, skillLevel, start))
            {
                return false;
            }
        }

        if (!trigger.IsSplash)
        {
            foreach (int skillId in trigger.SkillId)
            {
                castInfo.Target.SkillTriggerHandler.HostileSkillProccedLast[skillId] = start;
            }
        }

        return true;
    }

    public void FireTrigger(SkillCondition trigger, SkillCast skillCast, ConditionSkillTarget castInfo)
    {
        if (trigger.IsSplash)
        {
            RegionSkillHandler.HandleEffect(Parent.FieldManager, skillCast, castInfo.Target);
        }
        else
        {
            for (int i = 0; i < trigger.SkillId.Length; ++i)
            {
                skillCast.SkillId = trigger.SkillId[i];
                skillCast.SkillLevel = trigger.SkillLevel[i];

                castInfo.Target.AdditionalEffects.AddEffect(new(trigger.SkillId[i], trigger.SkillLevel[i])
                {
                    Caster = castInfo.Caster,
                    ParentSkill = skillCast.ParentSkill
                });
            }
        }
    }
    
    public void FireEvent(SkillCondition trigger, SkillCast? parentSkill, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        ConditionSkillTarget eventCastInfo = new(castInfo.Owner, GetTarget(trigger.Target, castInfo) ?? castInfo.Target, GetOwner(trigger.Owner, castInfo) ?? castInfo.Owner, castInfo.Attacker);

        if (!ShouldFireTrigger(trigger, eventCastInfo, effectEvent, eventIdArgument, start))
        {
            return;
        }

        bool useDirection = trigger.UseDirection;
        int duration = trigger.Interval;

        SkillCast skillCast = new(trigger.SkillId[0], trigger.SkillLevel[0], parentSkill?.SkillSn ?? 0, start)
        {
            Caster = eventCastInfo.Caster,
            Position = eventCastInfo.Target.Coord,
            Rotation = useDirection ? eventCastInfo.Caster.Rotation : default,
            Direction = useDirection ? Maple2Storage.Types.CoordF.From(1, eventCastInfo.Caster.LookDirection) : default,
            LookDirection = useDirection ? eventCastInfo.Caster.LookDirection : default,
            Duration = duration,
            ParentSkill = parentSkill
        };

        bool isImmediate = trigger.Delay == 0;
        isImmediate &= trigger.Interval == 0 || (trigger.ImmediateActive && trigger.FireCount == 1);

        if (isImmediate)
        {
            for (int fire = 0; fire < trigger.FireCount; ++fire)
            {
                FireTrigger(trigger, skillCast, eventCastInfo);
            }

            return;
        }

        Task.Run(async () =>
        {
            uint delay = trigger.Delay;

            if (trigger.FireCount > 0 && !trigger.ImmediateActive)
            {
                delay += (uint)trigger.Interval;
            }

            if (delay > 0)
            {
                await Task.Delay((int) trigger.Delay);
            }

            for (int fire = 0; fire < trigger.FireCount; ++fire)
            {
                FireTrigger(trigger, skillCast, eventCastInfo);

                if (trigger.FireCount > 1)
                {
                    await Task.Delay(trigger.Interval);
                }
            }
        });
    }

    public void FireTriggerSkill(SkillCondition trigger, SkillCast parentSkill, ConditionSkillTarget castInfo, int start = -1, bool hitTarget = true)
    {
        if (!hitTarget && trigger.DependOnDamageCount)
        {
            return;
        }

        FireEvent(trigger, parentSkill, castInfo, EffectEvent.Activate, 0, start);
    }

    public void FireEvents(List<SkillCondition>? triggers, SkillCast parentSkill, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        if (triggers == null)
        {
            return;
        }

        if (start == -1)
        {
            start = Environment.TickCount;
        }

        foreach (SkillCondition trigger in triggers)
        {
            FireEvent(trigger, parentSkill, castInfo, effectEvent, eventIdArgument, start);
        }
    }

    public void FireTriggerSkills(List<SkillCondition>? triggers, SkillCast parentSkill, ConditionSkillTarget castInfo, int start = -1, bool hitTarget = true)
    {
        if (triggers == null)
        {
            return;
        }

        if (start == -1)
        {
            start = Environment.TickCount;
        }

        foreach (SkillCondition trigger in triggers)
        {
            FireTriggerSkill(trigger, parentSkill, castInfo, start, hitTarget);
        }
    }

    public void FireEvents(ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        if (start == -1)
        {
            start = Environment.TickCount;
        }

        List<AdditionalEffect>? effects = Parent.AdditionalEffects.GetListeningEvents(effectEvent);

        if (effects is not null)
        {
            List<AdditionalEffect> EffectBuffer = new();
            EffectBuffer.AddRange(effects);

            foreach (AdditionalEffect effect in EffectBuffer)
            {
                if (IsConditionMet(effect.LevelMetadata.BeginCondition, castInfo, effectEvent, eventIdArgument))
                {
                    FireEvents(effect.LevelMetadata.ConditionSkill, effect.ParentSkill, castInfo, effectEvent, eventIdArgument, start);
                    FireEvents(effect.LevelMetadata.SplashSkill, effect.ParentSkill, castInfo, effectEvent, eventIdArgument, start);
                }
            }
        }
    }
}
