using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
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
    public long ShieldHealth = 0;
    private GameSession? Session = null;
    public IFieldActor Parent;
    public IFieldObject<Mount>? Mount = null;
    private bool WasActiveLastUpdate = false;
    private bool ShouldBeActive = false;
    public ProximityQuery? ProximityQuery = null;
    public bool IsActive { get => ShouldBeActive; }

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
        ShieldHealth = LevelMetadata.Shield?.HpValue ?? 0;
    }

    public bool Matches(int id)
    {
        return Id == id;
    }

    public void SendBuffStatus(IFieldActor parent)
    {
        if (!IsAlive)
        {
            if (BuffId != -1)
            {
                parent?.FieldManager?.BroadcastPacket(BuffPacket.RemoveBuff(this, Parent.ObjectId));

                BuffId = -1;
            }

            return;
        }

        if (BuffId == -1)
        {
            BuffId = GuidGenerator.Int();

            parent.FieldManager?.BroadcastPacket(BuffPacket.AddBuff(this, Parent.ObjectId));

            return;
        }

        if (ShieldHealth > 0)
        {
            parent.FieldManager?.BroadcastPacket(BuffPacket.UpdateShieldBuff(this, Parent.ObjectId));

            return;
        }

        parent.FieldManager?.BroadcastPacket(BuffPacket.UpdateBuff(this, Parent.ObjectId));
    }

    public bool AreStatsStale(IFieldActor parent, bool forceCheck = false, EffectEvent effectEvent = EffectEvent.Tick, ConditionSkillTarget? eventEffectInfo = null, int eventIdArgument = 0)
    {
        bool hasSubjectCondition = LevelMetadata.BeginCondition.Owner is not null;
        hasSubjectCondition |= LevelMetadata.BeginCondition.Caster is not null;
        hasSubjectCondition |= LevelMetadata.BeginCondition.Target is not null;

        if (hasSubjectCondition && effectEvent == EffectEvent.Tick)
        {
            bool hasEvent = (LevelMetadata.BeginCondition.Owner?.EventCondition ?? EffectEvent.Tick) != EffectEvent.Tick;
            hasEvent |= (LevelMetadata.BeginCondition.Caster?.EventCondition ?? EffectEvent.Tick) != EffectEvent.Tick;
            hasEvent |= (LevelMetadata.BeginCondition.Target?.EventCondition ?? EffectEvent.Tick) != EffectEvent.Tick;

            if (hasEvent)
            {
                return false;
            }
        }

        bool shouldCheckStatus = LevelMetadata.HasStats || hasSubjectCondition || LevelMetadata.BeginCondition.RequireSkillCodes?.Codes.Length > 0;

        if (!shouldCheckStatus && !forceCheck)// && ShouldBeActive)
        {
            return false;
        }

        ConditionSkillTarget effectInfo = eventEffectInfo ?? new ConditionSkillTarget(parent, parent, Caster);

        ShouldBeActive = parent.SkillTriggerHandler.ShouldTick(LevelMetadata.BeginCondition, effectInfo, effectEvent, eventIdArgument, ProximityQuery);

        return (ShouldBeActive != WasActiveLastUpdate || forceCheck) && IsAlive;
    }

    public bool UpdateStatStatus(IFieldActor parent)
    {
        WasActiveLastUpdate = ShouldBeActive;

        SendBuffStatus(parent);

        return LevelMetadata.HasStats;
    }

    public bool UpdateIfStale(IFieldActor parent, bool forceCheck = false, EffectEvent effectEvent = EffectEvent.Tick, ConditionSkillTarget? eventEffectInfo = null, int eventIdArgument = 0)
    {
        if (AreStatsStale(parent, forceCheck, effectEvent, eventEffectInfo, eventIdArgument))
        {
            bool recompute = UpdateStatStatus(parent);

            if (recompute)
            {
                parent.ComputeStats();

                return true;
            }
        }

        return false;
    }

    private bool FirstInvoke = false;

    public void InvokeInitial(IFieldActor parent)
    {
        if (FirstInvoke)
        {
            return;
        }

        FirstInvoke = true;

        if (LevelMetadata.CancelEffect is not null)
        {
            CancelEffects(parent, LevelMetadata.CancelEffect);
        }

        if (LevelMetadata.ResetCoolDownTime is not null)
        {
            ResetCooldown(parent, LevelMetadata.ResetCoolDownTime);
        }
    }

    public void Invoke(IFieldActor parent, EffectEvent effectEvent = EffectEvent.Tick, ConditionSkillTarget? effectEventInfo = null, int eventIdArgument = 0)
    {
        ConditionSkillTarget effectInfo = effectEventInfo ?? new ConditionSkillTarget(parent, parent, Caster);

        parent.AdditionalEffects.DebugPrint(this, EffectEvent.Tick, effectInfo);

        if (!parent.SkillTriggerHandler.ShouldTick(LevelMetadata.BeginCondition, effectInfo, effectEvent, eventIdArgument, ProximityQuery))
        {
            return;
        }

        if (Session is null && Caster is Character character)
        {
            Session = character.Value.Session;
        }

        InvokeInitial(parent);

        if (LevelMetadata.ModifyOverlapCount is not null)
        {
            ModifyOverlap(parent, LevelMetadata.ModifyOverlapCount);
        }

        if (LevelMetadata.ModifyEffectDuration is not null)
        {
            ModifyDuration(parent, LevelMetadata.ModifyEffectDuration);
        }

        if (LevelMetadata.Recovery is not null)
        {
            Recovery(parent, LevelMetadata.Recovery);
        }

        if (LevelMetadata.DotDamage is not null)
        {
            DotDamage(parent, LevelMetadata.DotDamage);
        }

        SkillCast skillCast = new(0, 0, ParentSkill?.SkillSn ?? 0, (int) parent.TaskScheduler.CurrentTick)
        {
            Owner = effectInfo.Owner,
            Caster = effectInfo.Caster,
            Position = effectInfo.Target?.Coord ?? default,
            Rotation = effectInfo.Caster?.Rotation ?? default,
            Direction = effectInfo.Caster is not null ? Maple2Storage.Types.CoordF.From(1, effectInfo.Caster.LookDirection) : default,
            LookDirection = effectInfo.Caster?.LookDirection ?? default,
            ParentSkill = ParentSkill
        };

        FireEvent(parent, Caster, EffectEvent.OnEffectApplied);
        parent.SkillTriggerHandler.FireTriggerSkills(LevelMetadata.ConditionSkill, skillCast, effectInfo);
        parent.SkillTriggerHandler.FireTriggerSkills(LevelMetadata.SplashSkill, skillCast, effectInfo);
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

        if (durationCodes is null || durationCodes.Length == 0)
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

        int[] originSkillIds = new int[resetCooldown.SkillCodes.Length];

        for (int i = 0; i < resetCooldown.SkillCodes.Length; ++i)
        {
            originSkillIds[i] = SkillMetadataStorage.GetChangeOriginSkillId(resetCooldown.SkillCodes[i]);
        }

        parent.SkillTriggerHandler.SetSkillCooldowns(resetCooldown.SkillCodes, originSkillIds, null);
    }

    public void Recovery(IFieldActor parent, EffectRecoveryMetadata recovery)
    {
        GameSession? session = null;

        if (parent is Character character)
        {
            session = character.Value.Session;
        }

        int healedAmount = (int) ((Caster?.Stats?[StatAttribute.MagicAtk]?.TotalLong ?? 0) * recovery.RecoveryRate);
        healedAmount += (int) (parent.Stats[StatAttribute.Hp].BonusLong * recovery.HpRate) + (int) recovery.HpValue;

        float healRate = (float) (1000 + (Caster?.Stats?[StatAttribute.Heal]?.TotalLong ?? 0)) / 1000;
        InvokeStatValue? invokeStat = Caster?.Stats?.GetEffectStats(Id, LevelMetadata.Basic.Group, InvokeEffectType.IncreaseHealing);

        if (invokeStat is not null)
        {
            healRate *= Math.Max(0, 1 + invokeStat.Rate);
        }

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
        if ((dotDamage.Rate == 0 && dotDamage.Value == 0 && dotDamage.DamageByTargetMaxHp == 0) || Session is null)
        {
            return;
        }

        AdditionalEffect? activeShield = parent.AdditionalEffects.ActiveShield;

        if (activeShield is not null)
        {
            int[]? allowedSkills = activeShield.LevelMetadata?.Basic?.AllowedSkillAttacks;
            int[]? allowedDotEffects = activeShield.LevelMetadata?.Basic?.AllowedDotEffectAttacks;

            if ((allowedSkills?.Length > 0 || allowedDotEffects?.Length > 0) && allowedDotEffects?.Contains(Id) != true)
            {
                return;
            }
        }

        bool damageVarianceEnabled = true;

        if (Caster is Managers.Actors.Character character)
        {
            damageVarianceEnabled = character.Value.DamageVarianceEnabled;
        }

        Stat hp = parent.Stats[StatAttribute.Hp];

        DamageSourceParameters dotParameters = new()
        {
            IsSkill = false,
            GuaranteedCrit = Caster?.AdditionalEffects?.AlwaysCrit ?? false,
            CritRateOverride = Caster?.AdditionalEffects?.GetCompulsionRate(CompulsionEventType.CritChanceOverride) ?? 0,
            EvadeRateOverride = Parent.AdditionalEffects.GetCompulsionRate(CompulsionEventType.EvasionChanceOverride),
            BlockRate = Parent.AdditionalEffects.GetCompulsionRate(CompulsionEventType.BlockChance),
            CanCrit = LevelMetadata.DotDamage.UseGrade,
            Element = (Element) dotDamage.Element,
            RangeType = SkillRangeType.Special,
            DamageType = (DamageType) dotDamage.DamageType,
            DamageRate = dotDamage.Rate * Stacks,
            DamageValue = dotDamage.Value + (long) (dotDamage.DamageByTargetMaxHp * hp.BonusLong),
            ParentSkill = ParentSkill,
            Id = Id,
            EventGroup = LevelMetadata.Basic.Group,
            DamageVarianceEnabled = damageVarianceEnabled
        };

        DamageHandler.ApplyDotDamage(Session, Caster, parent, dotParameters);
    }

    public void DamageShield(IFieldActor parent, long amount)
    {
        ShieldHealth = Math.Max(0, ShieldHealth - amount);

        if (ShieldHealth == 0)
        {
            Stop(parent);

            return;
        }

        parent.FieldManager?.BroadcastPacket(BuffPacket.UpdateShieldBuff(this, parent.ObjectId));
    }

    public void CancelEffects(IFieldActor parent, EffectCancelEffectMetadata cancel)
    {
        for (int i = 0; i < parent.AdditionalEffects.Effects.Count; i++)
        {
            AdditionalEffect oldEffect = parent.AdditionalEffects.Effects[i];

            if (cancel?.CancelEffectCodes?.Contains(oldEffect.Id) == true)
            {
                oldEffect.Stop(parent);
                --i;
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

        SendBuffStatus(parent);

        if (Mount is not null && parent is Character character)
        {
            character.Value.Mount = null; // Remove mount from player
            Mount = null;
            character.FieldManager?.BroadcastPacket(MountPacket.StopRide(character, true));
        }

        parent.AdditionalEffects.EffectStopped(this);

        parent.TaskScheduler.QueueBufferedTask(() => FireEvent(parent, Caster, EffectEvent.OnEffectRemoved));

        parent.TaskScheduler.RemoveTasksFromSubject(this);

        if (ProximityQuery is not null)
        {
            parent.ProximityTracker.Queries.Remove(ProximityQuery);

            ProximityQuery = null;
        }
    }

    public void ApplyStatuses(IFieldActor parent)
    {
        parent.AdditionalEffects.AlwaysCrit |= LevelMetadata.Offensive.AlwaysCrit;
        parent.AdditionalEffects.Invincible |= LevelMetadata.Defesive.Invincible;
        parent.AdditionalEffects.MinimumHp = Math.Max(parent.AdditionalEffects.MinimumHp, LevelMetadata.Status.DeathResistanceHp);

        if (LevelMetadata.Shield?.HpValue > 0)
        {
            parent.AdditionalEffects.ActiveShield = this;
        }

        if (LevelMetadata.Status.CompulsionEventType != CompulsionEventType.None)
        {
            parent.AdditionalEffects.CompulsionEvents.Add(new()
            {
                Type = LevelMetadata.Status.CompulsionEventType,
                Rate = LevelMetadata.Status.CompulsionEventRate * Stacks,
                SkillCodes = LevelMetadata.Status.CompulsionEventSkillCodes
            });
        }

        if (LevelMetadata.Status.Resistances is null)
        {
            return;
        }

        foreach ((StatAttribute attribute, float value) in LevelMetadata.Status.Resistances)
        {
            parent.AdditionalEffects.Resistances.TryAdd(attribute, 0);
            parent.AdditionalEffects.Resistances[attribute] += value;
        }
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
        if (LevelMetadata.Basic.InvokeEvent || true)
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
        ConditionSkillTarget effectInfo = new(parent, parent, Caster);

        FireEvent(parent, Caster, EffectEvent.OnEffectApplied);

        int delay = LevelMetadata.Basic.DelayTick;
        int duration = LevelMetadata.Basic.DurationTick;
        int interval = LevelMetadata.Basic.IntervalTick;

        bool delayFirstInterval = LevelMetadata.Basic.DotCondition != EffectDotCondition.ImmediateFire && interval != 0;

        UpdateIfStale(parent, true);

        if (delay == 0 && !delayFirstInterval)
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

        Character? character = parent as Character;

        if (LevelMetadata.Ride is not null && parent.FieldManager is not null && character is not null)
        {
            Mount = parent.FieldManager.RequestFieldObject(new Mount
            {
                Type = RideType.AdditionalEffect,
                Id = LevelMetadata.Ride.RideId,
                Uid = 0
            });

            Mount.Value.Players[0] = character;
            character.Value.Mount = Mount;

            parent.FieldManager.BroadcastPacket(MountPacket.StartRide(character));
        }

        BeginConditionSubject? owner = LevelMetadata.BeginCondition.Owner;

        if (owner is not null && (owner.TargetCheckRange != 0 || owner.TargetCheckMinRange != 0))
        {
            ProximityQuery = new()
            {
                TargetRange = owner.TargetCheckRange,
                TargetMinRange = owner.TargetCheckMinRange,
                Type = owner.TargetFriendly
            };

            parent.ProximityTracker.Queries.Add(ProximityQuery);
        }

        parent.TaskScheduler.RemoveTasks(Caster, this);

        parent.TaskScheduler.QueueTask(new(interval)
        {
            Delay = delay,
            Duration = duration,
            Executions = interval == 0 ? 1 : -1,
            Origin = Caster,
            Subject = this,
        }, (currentTick, task) => OnTick(parent, effectInfo));

        if (taskFinishedCallback is not null)
        {
            parent.TaskScheduler.QueueTask(new(delay + duration)
            {
                Delay = delay + duration,
                Executions = 1,
                Origin = Caster,
                Subject = this,
            }, (currentTick, task) => { taskFinishedCallback(currentTick, task); return 0; });
        }
    }
}
