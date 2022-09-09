using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Managers.Actors;

public class Character : FieldActor<Player>
{
    public override Stats Stats
    {
        get => Value.Stats;
        set => Value.Stats = value;
    }

    private CancellationTokenSource? CombatCTS;

    private Task? HpRegenThread;
    private Task? SpRegenThread;
    private Task? StaRegenThread;
    public IFieldObject<LiftableObject>? CarryingLiftable;
    public Pet? ActivePet;
    public override AdditionalEffects AdditionalEffects { get => Value.AdditionalEffects; }

    private DateTime LastConsumeStaminaTime;

    public override FieldManager? FieldManager { get => Value?.Session?.FieldManager; }

    public Character(int objectId, Player value, FieldManager fieldManager) : base(objectId, value, fieldManager)
    {
        if (HpRegenThread == null || HpRegenThread.IsCompleted)
        {
            HpRegenThread = StartRegen(StatAttribute.Hp, StatAttribute.HpRegen, StatAttribute.HpRegenInterval);
        }

        if (SpRegenThread == null || SpRegenThread.IsCompleted)
        {
            SpRegenThread = StartRegen(StatAttribute.Spirit, StatAttribute.SpRegen, StatAttribute.SpRegenInterval);
        }

        if (StaRegenThread == null || StaRegenThread.IsCompleted)
        {
            StaRegenThread = StartRegen(StatAttribute.Stamina, StatAttribute.StaminaRegen, StatAttribute.StaminaRegenInterval);
        }

        value.AdditionalEffects.Parent = this;
    }

    public override void Cast(SkillCast skillCast)
    {
        int spiritCost = skillCast.GetSpCost();
        int staminaCost = skillCast.GetStaCost();

        if (Value.Stats[StatAttribute.Spirit].Total < spiritCost || Value.Stats[StatAttribute.Stamina].Total < staminaCost)
        {
            return;
        }

        SkillCast = skillCast;

        ConsumeSp(spiritCost);
        ConsumeStamina(staminaCost);

        QuestManager.OnSkillUse(Value, skillCast.SkillId);

        // TODO: Move this and all others combat cases like recover sp to its own class.
        // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
        List<SkillCondition>? conditionSkills = SkillMetadataStorage.GetSkill(skillCast.SkillId)?.SkillLevels
            ?.FirstOrDefault(level => level.Level == skillCast.SkillLevel)?.ConditionSkills;
        if (conditionSkills is null)
        {
            return;
        }

        EffectTriggers triggers = new()
        {
            Caster = this,
            Owner = this
        };

        SkillTriggerHandler.FireTriggers(conditionSkills, triggers);

        Value.Session.FieldManager.BroadcastPacket(SkillUsePacket.SkillUse(skillCast));
        Value.Session.Send(StatPacket.SetStats(this));

        StartCombatStance();
    }

    public override void RecoverHp(int amount)
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
                Value.Session.Send(StatPacket.UpdateStats(this, StatAttribute.Hp));
            }
        }
    }

    public override void ConsumeHp(int amount)
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

        if (HpRegenThread == null || HpRegenThread.IsCompleted)
        {
            HpRegenThread = StartRegen(StatAttribute.Hp, StatAttribute.HpRegen, StatAttribute.HpRegenInterval);
        }
    }

    public override void RecoverSp(int amount)
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
                Value.Session.Send(StatPacket.UpdateStats(this, StatAttribute.Spirit));
            }
        }
    }

    public override void ConsumeSp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stats[StatAttribute.Spirit].AddValue(-amount);
        }

        if (SpRegenThread == null || SpRegenThread.IsCompleted)
        {
            SpRegenThread = StartRegen(StatAttribute.Spirit, StatAttribute.SpRegen, StatAttribute.SpRegenInterval);
        }
    }

    public override void RecoverStamina(int amount)
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
                Value.Session.Send(StatPacket.UpdateStats(this, StatAttribute.Stamina));
            }
        }
    }

    /// <summary>
    /// Consumes stamina.
    /// </summary>
    /// <param name="amount">The amount</param>
    /// <param name="noRegen">If regen should be stopped</param>
    public override void ConsumeStamina(int amount, bool noRegen = false)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stats[StatAttribute.Stamina].AddValue(-amount);
            LastConsumeStaminaTime = DateTime.Now;
        }

        if (StaRegenThread == null || StaRegenThread.IsCompleted)
        {
            StaRegenThread = StartRegen(StatAttribute.Stamina, StatAttribute.StaminaRegen, StatAttribute.StaminaRegenInterval, noRegen);
        }
    }

    /// <summary>
    /// Starts the regen task for the given stat. If noRegen is true and last consume time is less than 1.5 seconds ago, the regen will not be started.
    /// </summary>
    /// <param name="statAttribute">The stat it self. E.g: Stamina</param>
    /// <param name="regenStatAttribute">The stat for the regen amount. E.g: StaminaRegen</param>
    /// <param name="timeStatAttribute">The stat for the regen interval. E.g: StaminaRegenInterval</param>
    /// <param name="noRegen">If regen should pause</param>
    private Task StartRegen(StatAttribute statAttribute, StatAttribute regenStatAttribute, StatAttribute timeStatAttribute, bool noRegen = false)
    {
        // TODO: merge regen updates with larger packets
        return Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(Math.Max(Stats[timeStatAttribute].Total, 100));

                lock (Stats)
                {
                    if (Stats[statAttribute].Total >= Stats[statAttribute].Bonus)
                    {
                        return;
                    }

                    // If noRegen is true and last consume time is less than 1.5 seconds ago, the regen will not be started.
                    if (statAttribute is StatAttribute.Stamina && noRegen && DateTime.Now - LastConsumeStaminaTime < TimeSpan.FromSeconds(1.5))
                    {
                        continue;
                    }

                    AddStatRegen(statAttribute, regenStatAttribute);
                    Value.Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, statAttribute));
                    Value.Party?.BroadcastPacketParty(PartyPacket.UpdateHitpoints(Value));
                }
            }
        });
    }

    public Task StartCombatStance()
    {
        // Refresh out-of-combat timer
        CombatCTS?.Cancel();
        CancellationTokenSource cts = new();
        cts.Token.Register(() => cts.Dispose());
        CombatCTS = cts;

        // Enter combat
        Value.Session.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(this, true));
        return Task.Run(async () =>
        {
            await Task.Delay(5000);

            if (!cts.Token.IsCancellationRequested)
            {
                CombatCTS = null;
                cts.Dispose();
                Value.Session?.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(this, false));
            }
        }, cts.Token);
    }

    private void AddStatRegen(StatAttribute statIndex, StatAttribute regenStatIndex)
    {
        int regenAmount = Stats[regenStatIndex].Total;
        if (regenAmount <= 0)
        {
            return;
        }

        Stat stat = Stats[statIndex];
        lock (stat)
        {
            if (stat.Total < stat.Bonus)
            {
                stat.AddValue(regenAmount);
            }
        }
    }

    public override void EffectAdded(AdditionalEffect? effect)
    {
        if (effect is null)
        {
            return;
        }

        base.EffectAdded(effect);
        Value.EffectAdded(effect);
    }

    public override void EffectRemoved(AdditionalEffect? effect)
    {
        if (effect is null)
        {
            return;
        }

        base.EffectRemoved(effect);
        Value.EffectRemoved(effect);
    }

    public override void InitializeEffects()
    {
        Value.InitializeEffects();

        base.InitializeEffects();

        ComputeStats();
    }

    public override void StatsComputed()
    {
        Value.Session.Send(StatPacket.SetStats(this));
    }

    public override void AddStats()
    {
        Value.AddStats();
    }
}
