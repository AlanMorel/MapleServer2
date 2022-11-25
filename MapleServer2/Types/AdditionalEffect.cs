using System.Reflection;
using System.Threading.Tasks;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Serilog;

namespace MapleServer2.Types;

public class AdditionalEffect
{
    public int Id;
    public int Level;
    public AdditionalEffectMetadata Metadata;
    public AdditionalEffectLevelMetadata LevelMetadata;
    public int References = 0; // Reference counter. Only remove 
    public int Stacks;
    public IFieldActor? Caster;
    public int BuffId = -1;
    public int Start = -1;
    public int Duration = 0;
    public int End { get => Start + Duration; }
    public bool IsAlive = true;
    public SkillCast? ParentSkill;
    public bool HasEvents = false;
    private GameSession? Session = null;
    public IFieldActor Parent;
    private bool WasActiveLastUpdate = false;

    public AdditionalEffect(IFieldActor parent, int id, int level, int stacks = 1)
    {
        Parent = parent;
        Id = id;
        Level = level;
        Stacks = stacks;

        AdditionalEffectMetadata? metadata = AdditionalEffectMetadataStorage.GetMetadata(id);
        AdditionalEffectLevelMetadata? levelMetadata = AdditionalEffectMetadataStorage.GetLevelMetadata(id, level);

        if (metadata is null)
        {
            Log.Logger.Error("Attempted to apply additional effect {0}, but no metadata was found", id);

            return;
        }

        if (levelMetadata is null)
        {
            Log.Logger.Error("Attempted to apply additional effect {0} with level {1}, but that level was not found", id, level);

            return;
        }

        if (stacks > Math.Max(1, levelMetadata.Basic.MaxBuffCount))
        {
            Log.Logger.Error("Attempted to apply {0} stacks of additional effect {1} with level {2}, but the max stacks is {3}", stacks, id, level, levelMetadata.Basic.MaxBuffCount);

            return;
        }

        Metadata = metadata;
        LevelMetadata = levelMetadata;
    }

    public bool Matches(int id)
    {
        return Id == id;
    }

    public bool AreStatsStale(IFieldActor parent, EffectEvent effectEvent = EffectEvent.Tick)
    {
        if (!LevelMetadata.HasStats)
        {
            return false;
        }

        ConditionSkillTarget effectInfo = new ConditionSkillTarget(parent, parent, Caster);

        bool shouldBeActive = parent.SkillTriggerHandler.ShouldTick(LevelMetadata.BeginCondition, effectInfo, effectEvent);

        return shouldBeActive != WasActiveLastUpdate;
    }

    public bool UpdateStatStatus(IFieldActor parent, EffectEvent effectEvent = EffectEvent.Tick)
    {
        if (!LevelMetadata.HasStats)
        {
            return false;
        }

        ConditionSkillTarget effectInfo = new ConditionSkillTarget(parent, parent, Caster);

        WasActiveLastUpdate = parent.SkillTriggerHandler.ShouldTick(LevelMetadata.BeginCondition, effectInfo, effectEvent);

        return WasActiveLastUpdate;
    }

    public void Invoke(IFieldActor parent, EffectEvent effectEvent = EffectEvent.Tick)
    {
        ConditionSkillTarget effectInfo = new ConditionSkillTarget(parent, parent, Caster);

        if (!parent.SkillTriggerHandler.ShouldTick(LevelMetadata.BeginCondition, effectInfo, effectEvent))
        {
            return;
        }

        if (Session is null && Caster is Character character)
        {
            Session = character.Value.Session;
        }

        if (LevelMetadata.ModifyOverlapCount is not null)
        {
            ModifyOverlap(parent, LevelMetadata.ModifyOverlapCount);
        }

        if (LevelMetadata.ModifyEffectDuration is not null)
        {
            ModifyDuration(parent, LevelMetadata.ModifyEffectDuration);
        }

        if (LevelMetadata.ResetCoolDownTime is not null)
        {
            ResetCooldown(parent, LevelMetadata.ResetCoolDownTime);
        }

        if (LevelMetadata.Recovery is not null)
        {
            Recovery(parent, LevelMetadata.Recovery);
        }

        if (LevelMetadata.DotDamage is not null)
        {
            DotDamage(parent, LevelMetadata.DotDamage);
        }

        FireEvent(parent, Caster, EffectEvent.OnEffectApplied);
        parent.SkillTriggerHandler.FireTriggerSkills(LevelMetadata.ConditionSkill, ParentSkill, effectInfo);
        parent.SkillTriggerHandler.FireTriggerSkills(LevelMetadata.SplashSkill, ParentSkill, effectInfo);
    }

    public void ModifyOverlap(IFieldActor parent, EffectModifyOverlapCountMetadata modifyOverlap)
    {
        int[] overlapCodes = modifyOverlap.EffectCodes;

        if (overlapCodes.Length == 0)
        {
            return;
        }

        for (int i = 0; i < overlapCodes.Length; ++i)
        {
            AdditionalEffect? affectedEffect = parent.AdditionalEffects.GetEffect(overlapCodes[i]);

            if (affectedEffect is null)
            {
                continue;
            }

            parent.AdditionalEffects.AddEffect(new(affectedEffect.Id, affectedEffect.Level)
            {
                Stacks = modifyOverlap.OffsetCounts[i],
                Caster = Caster
            });
        }
    }

    public void ModifyDuration(IFieldActor parent, EffectModifyDurationMetadata modifyDuration)
    {
        int[] durationCodes = modifyDuration.EffectCodes;

        if (durationCodes is null || durationCodes?.Length == 0)
        {
            return;
        }

        for (int i = 0; i < durationCodes.Length; ++i)
        {
            AdditionalEffect? affectedEffect = parent.AdditionalEffects.GetEffect(durationCodes[i]);

            if (affectedEffect is null)
            {
                continue;
            }

            affectedEffect.Duration = (int) (modifyDuration.DurationFactors[i] * affectedEffect.Duration) + (int) (1000 * modifyDuration.DurationValues[i]);

            parent.FieldManager?.BroadcastPacket(BuffPacket.UpdateBuff(affectedEffect, parent.ObjectId));
        }
    }

    public void ResetCooldown(IFieldActor parent, EffectResetSkillCooldownTimeMetadata resetCooldown)
    {
        if (resetCooldown.SkillCodes.Length == 0)
        {
            return;
        }

        parent.FieldManager?.BroadcastPacket(SkillCooldownPacket.SetCooldowns(resetCooldown.SkillCodes, new int[resetCooldown.SkillCodes.Length]));
    }

    public void Recovery(IFieldActor parent, EffectRecoveryMetadata recovery)
    {
        GameSession? session = null;

        if (parent is Character character)
        {
            session = character.Value.Session;
        }

        int healedAmount = (int) (Caster.Stats[StatAttribute.MagicAtk].TotalLong * recovery.RecoveryRate);
        healedAmount += (int) (parent.Stats[StatAttribute.Hp].BonusLong * recovery.HpRate) + (int) recovery.HpValue;

        float healRate = (float) (1000 + Caster.Stats[StatAttribute.Heal].TotalLong) / 1000;
        InvokeStatValue invokeStat = Caster.Stats.GetEffectStats(Id, LevelMetadata.Basic.Group, InvokeEffectType.IncreaseHealing);

        healRate *= Math.Max(0, 1 + invokeStat.Rate);

        if (healedAmount > 0)
        {
            parent.Heal(session, this, (int) (healedAmount * healRate));
        }

        int recoveredSp = (int) (parent.Stats[StatAttribute.Spirit].BonusLong * recovery.SpRate) + (int) recovery.SpValue;

        if (recoveredSp > 0)
        {
            parent.RecoverSp((int) (recoveredSp * healRate));
        }

        int recoveredEp = (int) (parent.Stats[StatAttribute.Stamina].BonusLong * recovery.EpRate) + (int) recovery.EpValue;

        if (recoveredEp > 0)
        {
            parent.RecoverStamina(recoveredEp);
        }
    }

    public void DotDamage(IFieldActor parent, EffectDotDamageMetadata dotDamage)
    {
        if (dotDamage.Rate == 0 || Session is null || ParentSkill is null)
        {
            return;
        }

        DamageSourceParameters dotParameters = new()
        {
            IsSkill = false,
            GuaranteedCrit = Caster.AdditionalEffects.AlwaysCrit,
            CanCrit = LevelMetadata.DotDamage.UseGrade,
            Element = (Element) dotDamage.Element,
            RangeType = SkillRangeType.Special,
            DamageType = (DamageType) dotDamage.DamageType,
            DamageRate = dotDamage.Rate * Stacks,
            ParentSkill = ParentSkill,
            Id = Id,
            EventGroup = LevelMetadata.Basic.Group
        };

        DamageHandler.ApplyDotDamage(Session, Caster, parent, dotParameters);
    }

    public void CancelEffects(IFieldActor parent, EffectCancelEffectMetadata cancel)
    {
        for (int i = 0; i < parent.AdditionalEffects.Effects.Count; i++)
        {
            AdditionalEffect oldEffect = parent.AdditionalEffects.Effects[i];

            if (cancel?.CancelEffectCodes?.Contains(oldEffect.Id) == true)
            {
                oldEffect.Stop(parent);
            }
        }
    }

    public void Stop(IFieldActor parent)
    {
        if (!IsAlive)
        {
            return;
        }

        IsAlive = false;

        parent.AdditionalEffects.EffectStopped(this);

        FireEvent(parent, Caster, EffectEvent.OnEffectRemoved);

        parent.TaskScheduler.RemoveTasksFromSubject(this);
    }

    public void ApplyStatuses(IFieldActor parent)
    {
        parent.AdditionalEffects.AlwaysCrit |= LevelMetadata.Offensive.AlwaysCrit;
        parent.AdditionalEffects.Invincible |= LevelMetadata.Defesive.Invincible;
    }

    public bool IsStillAlive(IFieldActor parent, ConditionSkillTarget effectInfo)
    {
        if (!IsAlive)
        {
            Stop(parent);

            return false;
        }

        return true;
    }

    public void FireEvent(IFieldActor parent, IFieldActor? attacker, EffectEvent effectEvent)
    {
        if (LevelMetadata.Basic.InvokeEvent)
        {
            parent.SkillTriggerHandler.FireEvents(parent, attacker, effectEvent, Id);
        }
    }

    public void TimedOut(IFieldActor parent, ConditionSkillTarget effectInfo)
    {
        if (!IsAlive || LevelMetadata.Basic.KeepCondition == EffectKeepCondition.UnlimitedDuration)
        {
            return;
        }

        FireEvent(parent, effectInfo.Caster, EffectEvent.OnBuffTimeExpiring);

        Stop(parent);
    }

    public long OnTick(IFieldActor parent, ConditionSkillTarget effectInfo)
    {
        if (!IsStillAlive(parent, effectInfo))
        {
            return TriggerTask.End;
        }

        Invoke(parent);

        return TriggerTask.SameInterval;
    }

    public void Process(IFieldActor parent)
    {
        if (LevelMetadata.CancelEffect is not null)
        {
            CancelEffects(parent, LevelMetadata.CancelEffect);
        }

        ApplyStatuses(parent);

        ConditionSkillTarget effectInfo = new(parent, parent, Caster);

        FireEvent(parent, Caster, EffectEvent.OnEffectApplied);

        int delay = LevelMetadata.Basic.DelayTick;
        int duration = LevelMetadata.Basic.DurationTick;
        int interval = LevelMetadata.Basic.IntervalTick;

        if (delay == 0 && !(LevelMetadata.Basic.DotCondition != EffectDotCondition.ImmediateFire && interval != 0))
        {
            Invoke(parent);
        }

        if (LevelMetadata.Basic.DotCondition != EffectDotCondition.ImmediateFire)
        {
            delay += LevelMetadata.Basic.IntervalTick;

            if (duration > 0 && interval > 0 && duration % interval == 0)
            {
                ++duration;
            }
        }

        Action<long, TriggerTask>? taskFinishedCallback = null;

        if (LevelMetadata.Basic.KeepCondition != EffectKeepCondition.UnlimitedDuration)
        {
            taskFinishedCallback = (currentTick, task) => TimedOut(parent, effectInfo);
        }
        else
        {
            duration = -1;
        }

        parent.TaskScheduler.RemoveTasks(Caster, this);
        parent.TaskScheduler.QueueTask(new(interval)
        {
            Delay = delay,
            Duration = duration,
            Executions = interval == 0 ? 1 : -1,
            Origin = Caster,
            Subject = this
        }, (currentTick, task) => OnTick(parent, effectInfo), taskFinishedCallback);
    }
}
