using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Types;

public struct ExternalEventListener
{
    public bool TickEvent = false;
    public bool TriggerEvent = false;
    public AdditionalEffect Effect;
    public EffectEventOrigin Origin;

    public ExternalEventListener(AdditionalEffect effect, EffectEventOrigin origin)
    {
        Effect = effect;
        Origin = origin;
    }
}

public class SkillTriggerHandler
{
    public IFieldActor Parent;
    public Dictionary<int, int> SkillFiredLast = new();
    public Dictionary<int, int> HostileSkillProccedLast = new();
    private Dictionary<EffectEvent, List<ExternalEventListener>> ListeningEvents = new();

    public SkillTriggerHandler(IFieldActor parent)
    {
        Parent = parent;
    }

    public void AddListeningExternalEffect(EffectEvent effectEvent, AdditionalEffect effect, EffectEventOrigin origin)
    {
        List<ExternalEventListener> listeners = ListeningEvents.GetValueOrDefault(effectEvent, new List<ExternalEventListener>());

        if (listeners.Exists(listener => listener.Effect == effect && listener.Origin == origin))
        {
            return;
        }

        listeners.Add(new(effect, origin));
    }

    public void RemoveListeningExternalEffect(EffectEvent effectEvent, AdditionalEffect effect)
    {
        List<ExternalEventListener> listeners = ListeningEvents[effectEvent];

        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i].Effect == effect)
            {
                listeners.RemoveAt(i);
            }
        }
    }

    private IFieldActor? GetPetOwner()
    {
        if (Parent is Pet pet)
        {
            return pet.Owner;
        }

        return null;
    }

    private IFieldActor? GetTarget(SkillTarget target, ConditionSkillTarget castInfo)
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

    private IFieldActor? GetOwner(SkillOwner owner, ConditionSkillTarget castInfo)
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

    private bool IsConditionMet(BeginConditionSubject? subjectCondition, IFieldActor? subject, EffectEvent effectEvent, int eventIdArgument)
    {
        if (subjectCondition is null)
        {
            return true;
        }

        if (subject is null)
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
                EffectEvent.OnOwnerAttackHit => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || (subjectCondition.EventSkillIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.OnSkillCasted => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || (subjectCondition.EventSkillIDs?.Contains(eventIdArgument) ?? false),

                EffectEvent.OnBuffStacksReached => subjectCondition.RequireBuffId == eventIdArgument,
                EffectEvent.OnInvestigate => true,
                EffectEvent.OnBuffTimeExpiring => true, // check
                EffectEvent.OnSkillCastEnd => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || (subjectCondition.EventSkillIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.OnEffectApplied => (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || (subjectCondition.EventEffectIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.OnEffectRemoved => (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || (subjectCondition.EventEffectIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.OnLifeSkillGather => true,
                EffectEvent.OnAttackMiss => true,

                EffectEvent.UnknownKritiasPuzzleEvent => (subjectCondition.EventSkillIDs?.Length ?? 0) == 0 || (subjectCondition.EventSkillIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.UnknownWizardEvent => (subjectCondition.EventEffectIDs?.Length ?? 0) == 0 || (subjectCondition.EventEffectIDs?.Contains(eventIdArgument) ?? false),
                EffectEvent.UnknownStrikerEvent => subjectCondition.RequireBuffId == 0 || subjectCondition.RequireBuffId == eventIdArgument,
                _ => true
            };

            if (!eventMatches)
            {
                return false;
            }
        }

        for (int i = 0; i < subjectCondition.RequireBuffCount.Length; ++i)
        {
            ConditionOperator buffOperator = subjectCondition.RequireBuffCount[i] == 0 ? ConditionOperator.GreaterEquals : subjectCondition.RequireBuffCountCompare[i];

            if (subjectCondition.RequireBuffId != 0 && !subject.AdditionalEffects.HasEffect(subjectCondition.RequireBuffId, subjectCondition.RequireBuffCount[i], buffOperator, subjectCondition.RequireBuffLevel))
            {
                return false;
            }
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

    private bool IsConditionMet(SkillBeginCondition condition, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, ProximityQuery? query = null)
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

        if (subjectCondition && condition.Owner is not null && (condition.Owner.TargetCheckRange > 0 || condition.Owner.TargetCheckMinRange > 0))
        {
            if (query is null)
            {
                return false;
            }

            subjectCondition = AdditionalEffects.CompareValues(query.TargetCount, condition.Owner.TargetInRangeCount, condition.Owner.TargetCountSign);
        }

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

    private EffectEvent GetEvent(BeginConditionSubject? subjectCondition)
    {
        if (subjectCondition is null)
        {
            return EffectEvent.Activate;
        }

        return subjectCondition.EventCondition;
    }

    private bool EventMatches(SkillBeginCondition condition, EffectEvent effectEvent, EffectEventOrigin origin)
    {
        return origin switch
        {
            EffectEventOrigin.Owner => effectEvent == GetEvent(condition.Owner),
            EffectEventOrigin.Caster => effectEvent == GetEvent(condition.Caster),
            EffectEventOrigin.Target => effectEvent == GetEvent(condition.Target),
            _ => false
        };
    }

    private static int GetEffectCooldown(int effectId, int effectLevel)
    {
        AdditionalEffectLevelMetadata? levelMeta = AdditionalEffectMetadataStorage.GetLevelMetadata(effectId, effectLevel);

        if (levelMeta != null)
        {
            return (int) (1000 * levelMeta.Basic.CooldownTime);
        }

        return 0;
    }

    private bool IsEffectOnCooldown(ConditionSkillTarget castInfo, int skillId, int skillLevel, int start = -1)
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

        int lastProcced = castInfo.Target?.SkillTriggerHandler.HostileSkillProccedLast.GetValueOrDefault(skillId, start - cooldown) ?? start;

        return start - lastProcced < cooldown;
    }

    public bool ShouldTick(SkillBeginCondition beginCondition, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument = 0, ProximityQuery? query = null)
    {
        if (!EventMatches(beginCondition, effectEvent, castInfo.EventOrigin))
        {
            return false;
        }

        return IsConditionMet(beginCondition, castInfo, effectEvent, eventIdArgument, query);
    }

    private bool ShouldFireTrigger(SkillCondition trigger, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, bool targetingNone, int start = -1)
    {
        if (!EventMatches(trigger.BeginCondition, effectEvent, castInfo.EventOrigin))
        {
            return false;
        }

        if (start == -1)
        {
            start = Environment.TickCount;
        }

        if (castInfo.Target is null && !targetingNone)
        {
            return false;
        }

        if (targetingNone && castInfo.Target is not null)
        {
            return false;
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
                if (castInfo.Target is null)
                {
                    continue;
                }

                castInfo.Target.SkillTriggerHandler.HostileSkillProccedLast[skillId] = start;
            }
        }

        return true;
    }

    private void FireTrigger(SkillCondition trigger, SkillCast skillCast, ConditionSkillTarget castInfo)
    {
        if (trigger.IsSplash)
        {
            int removeDelay = trigger.RemoveDelay;

            if (removeDelay == 0)
            {
                removeDelay = trigger.FireCount * trigger.Interval;
            }

            if (Parent.FieldManager is null)
            {
                return;
            }

            RegionSkillHandler.CastRegionSkill(Parent.FieldManager, skillCast, trigger.FireCount, removeDelay, trigger.Interval, castInfo.Target, trigger);

            return;
        }

        if (trigger.Interval == 0 || trigger.FireCount == 1)
        {
            for (int i = 0; i < trigger.FireCount; ++i)
            {
                FireTriggerTick(trigger, skillCast, castInfo);
            }

            return;
        }

        FireTriggerTick(trigger, skillCast, castInfo);

        Parent.TaskScheduler.QueueTask(new(trigger.Interval)
        {
            Interval = trigger.Interval,
            Executions = trigger.FireCount - 1
        }, (currentTick, task) => FireTriggerTick(trigger, skillCast, castInfo));
    }

    private long FireTriggerTick(SkillCondition trigger, SkillCast skillCast, ConditionSkillTarget castInfo)
    {
        if (trigger.RandomCast)
        {
            int index = Random.Shared.Next(trigger.SkillId.Length);

            castInfo.Target?.AdditionalEffects.AddEffect(new(trigger.SkillId[index], trigger.SkillLevel[index])
            {
                Caster = castInfo.Caster,
                ParentSkill = skillCast.ParentSkill
            });

            return 0;
        }

        for (int i = 0; i < trigger.SkillId.Length; ++i)
        {
            castInfo.Target?.AdditionalEffects.AddEffect(new(trigger.SkillId[i], trigger.SkillLevel[i])
            {
                Caster = castInfo.Caster,
                ParentSkill = skillCast.ParentSkill
            });
        }

        return 0;
    }

    private void FireEvent(SkillCondition trigger, SkillCast? parentSkill, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        ConditionSkillTarget eventCastInfo = new(castInfo.Owner, GetTarget(trigger.Target, castInfo) ?? castInfo.Target, GetOwner(trigger.Owner, castInfo) ?? castInfo.Owner, castInfo.Attacker, castInfo.EventOrigin);

        SkillAttack? attack = parentSkill?.SkillAttack;
        bool useTarget = attack?.RangeProperty.ApplyTarget == ApplyTarget.None || trigger.NonTargetActive || (attack?.CubeMagicPathId > 0 && attack?.MagicPathId == 0 && castInfo.Target is null);

        if (!ShouldFireTrigger(trigger, eventCastInfo, effectEvent, eventIdArgument, useTarget, start))
        {
            return;
        }

        bool useDirection = trigger.UseDirection;
        int duration = trigger.RemoveDelay == 0 ? trigger.Interval : trigger.RemoveDelay;

        SkillCast skillCast = new(trigger.SkillId[0], trigger.SkillLevel[0], parentSkill?.SkillSn ?? 0, start)
        {
            Owner = eventCastInfo.Owner,
            Caster = eventCastInfo.Caster,
            Position = useTarget ? parentSkill?.Position ?? default : eventCastInfo.Target?.Coord ?? default,
            //Rotation = eventCastInfo.Caster?.Rotation ?? default,
            //Direction = Maple2Storage.Types.CoordF.From(1, eventCastInfo.Caster?.LookDirection ?? 0),
            //LookDirection = eventCastInfo.Caster?.LookDirection ?? default,
            //Rotation = useDirection ? eventCastInfo.Caster?.Rotation ?? default : parentSkill?.Rotation ?? default,
            //Direction = useDirection ? Maple2Storage.Types.CoordF.From(1, eventCastInfo.Caster?.LookDirection ?? 0) : parentSkill?.Direction ?? default,
            //LookDirection = useDirection ? eventCastInfo.Caster?.LookDirection ?? default : parentSkill?.LookDirection ?? default,
            Rotation = parentSkill?.Rotation ?? default,
            Direction = parentSkill?.Direction ?? default,
            LookDirection = parentSkill?.LookDirection ?? default,
            UseDirection = useDirection || (!parentSkill?.UsingCasterDirection ?? false),//useDirection,
            Duration = duration,
            ParentSkill = parentSkill,
            SkillAttack = parentSkill?.SkillAttack,
            UsingCasterDirection = parentSkill?.UsingCasterDirection ?? false
        };

        if (trigger.IsSplash && trigger.Target == SkillTarget.SkillTarget && trigger.NonTargetActive && parentSkill is not null && parentSkill.ActiveCoord != -1)
        {
            skillCast.Position = parentSkill.EffectCoords[parentSkill.ActiveCoord];
        }

        int delay = (int) trigger.Delay;

        if (!trigger.ImmediateActive)
        {
            delay += trigger.Interval;
        }

        if (delay == 0)
        {
            FireTrigger(trigger, skillCast, eventCastInfo);

            return;
        }

        Parent.TaskScheduler.QueueTask(new(trigger.Interval)
        {
            Delay = delay,
            Executions = 1
        }, (currentTick, task) => FireTriggerDelayed(trigger, skillCast, eventCastInfo));
    }

    private long FireTriggerDelayed(SkillCondition trigger, SkillCast skillCast, ConditionSkillTarget castInfo)
    {
        FireTrigger(trigger, skillCast, castInfo);

        return 0;
    }

    private void FireTriggerSkill(SkillCondition trigger, SkillCast parentSkill, ConditionSkillTarget castInfo, int start = -1, bool hitTarget = true)
    {
        if (!hitTarget && trigger.DependOnDamageCount)
        {
            return;
        }

        FireEvent(trigger, parentSkill, castInfo, EffectEvent.Activate, 0, start);
    }

    private void FireEvents(List<SkillCondition>? triggers, SkillCast? parentSkill, ConditionSkillTarget castInfo, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        if (triggers is null)
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
        if (triggers is null)
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

    public void FireEvents(IFieldActor? target, IFieldActor? attacker, EffectEvent effectEvent, int eventIdArgument, int start = -1)
    {
        if (start == -1)
        {
            start = Environment.TickCount;
        }

        List<AdditionalEffect>? effects = Parent.AdditionalEffects.GetListeningEvents(effectEvent);

        if (effects is null)
        {
            return;
        }

        List<AdditionalEffect> EffectBuffer = new();
        EffectBuffer.AddRange(effects);

        foreach (AdditionalEffect effect in EffectBuffer)
        {
            ConditionSkillTarget effectCastInfo = new(Parent, target, effect.ParentSkill?.Caster, attacker);

            if (IsConditionMet(effect.LevelMetadata.BeginCondition, effectCastInfo, effectEvent, eventIdArgument))
            {
                FireEvents(effect.LevelMetadata.ConditionSkill, effect.ParentSkill, effectCastInfo, effectEvent, eventIdArgument, start);
                FireEvents(effect.LevelMetadata.SplashSkill, effect.ParentSkill, effectCastInfo, effectEvent, eventIdArgument, start);
            }
        }

        if (!ListeningEvents.TryGetValue(effectEvent, out List<ExternalEventListener>? listeners))
        {
            return;
        }

        foreach (ExternalEventListener listener in listeners)
        {
            AdditionalEffect effect = listener.Effect;
            ConditionSkillTarget effectCastInfo = new(Parent, target, effect.ParentSkill?.Caster, attacker, listener.Origin);

            if (IsConditionMet(effect.LevelMetadata.BeginCondition, effectCastInfo, effectEvent, eventIdArgument))
            {
                FireEvents(effect.LevelMetadata.ConditionSkill, effect.ParentSkill, effectCastInfo, effectEvent, eventIdArgument, start);
                FireEvents(effect.LevelMetadata.SplashSkill, effect.ParentSkill, effectCastInfo, effectEvent, eventIdArgument, start);
            }
        }
    }

    public void OnAttacked(IFieldActor? attacker, int skillId, bool hit, bool crit, bool missed, bool blocked)
    {
        Parent.TaskScheduler.QueueTask(new(0)
        {
            Delay = 10,
            Executions = 1
        }, (currentTick, task) => OnAttackedTick(attacker, skillId, hit, crit, missed, blocked));
    }

    private long OnAttackedTick(IFieldActor? attacker, int skillId, bool hit, bool crit, bool missed, bool blocked)
    {
        SkillTriggerHandler? attackerTrigger = attacker?.SkillTriggerHandler;

        if (crit)
        {
            attackerTrigger?.FireEvents(Parent, attacker, EffectEvent.OnOwnerAttackCrit, skillId);
        }

        if (missed)
        {
            attackerTrigger?.FireEvents(Parent, attacker, EffectEvent.OnAttackMiss, skillId);
            FireEvents(Parent, attacker, EffectEvent.OnEvade, skillId);
        }

        if (blocked)
        {
            FireEvents(Parent, attacker, EffectEvent.OnBlock, skillId);
        }

        if (hit)
        {
            attackerTrigger?.FireEvents(Parent, attacker, EffectEvent.OnOwnerAttackHit, skillId);
            FireEvents(Parent, attacker, EffectEvent.OnAttacked, skillId);
        }

        return -1;
    }
}
