using Maple2.PathEngine;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Managers.Actors;

public abstract class FieldActor<T> : FieldObject<T>, IFieldActor<T>
{
    public CoordF Velocity { get; set; }
    public short Animation { get; set; }
    public short Animation2 { get; set; }

    public virtual Stats Stats { get; set; } = null!;
    public bool IsDead { get; set; }

    public List<Status>? Statuses { get; set; }
    public SkillCast? SkillCast { get; set; }
    public bool OnCooldown { get; set; }
    public Agent? Agent { get; set; }
    public virtual AdditionalEffects AdditionalEffects { get; }
    public SkillTriggerHandler SkillTriggerHandler { get; }
    public virtual TickingTaskScheduler TaskScheduler { get; }
    public virtual ProximityTracker ProximityTracker { get; }
    public SkillCastTracker SkillCastTracker { get; }

    public virtual FieldManager? FieldManager { get; }
    public FieldNavigator Navigator { get; }

    protected FieldActor(int objectId, T value, FieldManager fieldManager) : base(objectId, value)
    {
        FieldManager = fieldManager;
        Navigator = fieldManager.Navigator;

        AdditionalEffects = new(this);
        SkillTriggerHandler = new(this);
        TaskScheduler = new(FieldManager);
        ProximityTracker = new(this);
        SkillCastTracker = new(this);
    }

    public virtual void Cast(SkillCast skillCast)
    {
        SkillCast = skillCast;

        // TODO: Move this and all others combat cases like recover sp to its own class.
        // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
        if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
        {
            // Status status = new(skillCast, ObjectId, ObjectId, 1);
            //StatusHandler.Handle(Value.Session, status);
        }
    }

    public virtual void Heal(GameSession session, Status status, int amount)
    {
        session.FieldManager.BroadcastPacket(SkillDamagePacket.Heal(status, amount));
        Stats[StatAttribute.Hp].AddValue(amount);
        session.Send(StatPacket.UpdateStats(this, StatAttribute.Hp));
    }

    public virtual void Heal(GameSession session, AdditionalEffect effect, int amount)
    {
        session?.FieldManager.BroadcastPacket(SkillDamagePacket.Heal(effect, amount));
        Stats[StatAttribute.Hp].AddValue(amount);
        session?.Send(StatPacket.UpdateStats(this, StatAttribute.Hp));
    }

    public virtual void RecoverHp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stat stat = Stats[StatAttribute.Hp];
            if (stat.Total < stat.Bonus)
            {
                stat.AddValue(amount);
            }
        }
    }

    public virtual void ConsumeHp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stat stat = Stats[StatAttribute.Hp];
            stat.AddValue(-amount);
        }
    }

    public virtual void RecoverSp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stat stat = Stats[StatAttribute.Spirit];
            if (stat.Total < stat.Bonus)
            {
                stat.AddValue(amount);
            }
        }
    }

    public virtual void ConsumeSp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stats[StatAttribute.Spirit].AddValue(-amount);
        }
    }

    public virtual void RecoverStamina(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stat stat = Stats[StatAttribute.Stamina];
            if (stat.Total < stat.Bonus)
            {
                Stats[StatAttribute.Stamina].AddValue(amount);
            }
        }
    }

    public virtual void ConsumeStamina(int amount, bool noRegen = false)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stats[StatAttribute.Stamina].AddValue(-amount);
        }
    }

    public virtual void Damage(DamageHandler damage, GameSession session)
    {
        Stat health = Stats[StatAttribute.Hp];
        health.AddValue(-(long) damage.Damage);
        health.SetValue(Math.Max(health.TotalLong, AdditionalEffects.MinimumHp));

        if (health.TotalLong <= 0)
        {
            Perish();
        }
    }

    public virtual void Perish()
    {
        IsDead = true;
    }

    public virtual void Animate(string sequenceName, float duration = -1) { }

    public void IncreaseStats(AdditionalEffect effect)
    {
        ConditionSkillTarget effectInfo = new ConditionSkillTarget(this, this, effect.Caster);

        if (!SkillTriggerHandler.ShouldTick(effect.LevelMetadata.BeginCondition, effectInfo, EffectEvent.Tick, 0, effect.ProximityQuery))
        {
            return;
        }

        effect.ApplyStatuses(this);

        if (effect.LevelMetadata.Status.Stats is not null)
        {
            foreach ((StatAttribute stat, EffectStatMetadata statValue) in effect.LevelMetadata.Status.Stats)
            {
                Stats.AddStat(stat, statValue.AttributeType, statValue.Flat, statValue.Rate);
            }
        }

        EffectInvokeMetadata? invoke = effect.LevelMetadata?.InvokeEffect;

        if (invoke is null)
        {
            return;
        }

        if (invoke.SkillId != 0)
        {
            for (int i = 0; i < invoke.Types.Length; ++i)
            {
                Stats.AddSkillStats(invoke.SkillId, invoke.Types[i], invoke.Values[i], invoke.Rates[i]);
            }
        }

        if (invoke.SkillGroupId != 0)
        {
            for (int i = 0; i < invoke.Types.Length; ++i)
            {
                Stats.AddSkillGroupStats(invoke.SkillGroupId, invoke.Types[i], invoke.Values[i], invoke.Rates[i]);
            }
        }

        if (invoke.EffectId != 0)
        {
            for (int i = 0; i < invoke.Types.Length; ++i)
            {
                Stats.AddEffectStats(invoke.EffectId, invoke.Types[i], invoke.Values[i], invoke.Rates[i]);
            }
        }

        if (invoke.EffectGroupId != 0)
        {
            for (int i = 0; i < invoke.Types.Length; ++i)
            {
                Stats.AddEffectGroupStats(invoke.EffectGroupId, invoke.Types[i], invoke.Values[i], invoke.Rates[i]);
            }
        }
    }

    public bool IsRemoved()
    {
        bool sessionDisconnected = false;
        bool isPlayer = false;

        if (this is Character character)
        {
            isPlayer = true;
            sessionDisconnected = character.Value.Session is null;
        }

        return FieldManager is null && !(isPlayer && !sessionDisconnected);
    }

    public virtual void EffectAdded(AdditionalEffect? effect)
    {
        if (!IsRemoved() && (effect?.LevelMetadata?.HasStats ?? false))
        {
            ComputeStats();
        }
    }

    public virtual void EffectRemoved(AdditionalEffect? effect)
    {
        if (!IsRemoved() && (effect?.LevelMetadata?.HasStats ?? false))
        {
            ComputeStats();
        }
    }

    public virtual void InitializeEffects()
    {
        StatsComputed();
    }

    public void ComputeStats()
    {
        Stat hp = Stats[StatAttribute.Hp];
        Stat spirit = Stats[StatAttribute.Spirit];
        Stat stamina = Stats[StatAttribute.Stamina];

        long hpValue = hp.TotalLong;
        long spiritValue = spirit.TotalLong;
        long staminaValue = stamina.TotalLong;

        ComputeBaseStats();
        AddStats();

        hp.SetValue(hpValue);
        spirit.SetValue(spiritValue);
        stamina.SetValue(staminaValue);

        StatsComputed();
    }

    public virtual void ComputeBaseStats()
    {
        AdditionalEffects.ResetStatus();
    }

    public virtual void AddStats()
    {
        foreach (AdditionalEffect effect in AdditionalEffects.Effects)
        {
            IncreaseStats(effect);
        }
    }

    public virtual void StatsComputed() { }

    public void Update(long delta)
    {
        if (IsDead)
        {
            return;
        }

        AdditionalEffects.UpdateStatsIfStale();
        ProximityTracker.Update();
        SkillCastTracker.Update();
        TaskScheduler.Update(delta);
    }
}
