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

    public IFieldObject<LiftableObject>? CarryingLiftable;
    public Pet? ActivePet;
    public override AdditionalEffects AdditionalEffects { get => Value.AdditionalEffects; }
    public override TickingTaskScheduler TaskScheduler { get => PlayerTaskScheduler; }
    public override ProximityTracker ProximityTracker { get => PlayerProximityTracker; }

    private DateTime LastConsumeStaminaTime;

    public override FieldManager? FieldManager { get => Value?.Session?.FieldManager; }
    private TickingTaskScheduler PlayerTaskScheduler;
    private ProximityTracker PlayerProximityTracker;

    public Character(int objectId, Player value, FieldManager fieldManager) : base(objectId, value, fieldManager)
    {
        value.AdditionalEffects.Parent = this;
        PlayerTaskScheduler = value.TaskScheduler ?? new(fieldManager);
        PlayerProximityTracker = value.ProximityTracker ?? new(this);
        PlayerProximityTracker.Parent = this;

        AnimationHandler.SetActorId(value.Gender == Gender.Male ? "male" : "female");

        StartRegen(StatAttribute.Hp, StatAttribute.HpRegen, StatAttribute.HpRegenInterval);
        StartRegen(StatAttribute.Spirit, StatAttribute.SpRegen, StatAttribute.SpRegenInterval);
        StartRegen(StatAttribute.Stamina, StatAttribute.StaminaRegen, StatAttribute.StaminaRegenInterval);
    }

    public override void Cast(SkillCast skillCast)
    {
        int spiritCost = skillCast.GetSpCost();
        int staminaCost = skillCast.GetStaCost();

        InvokeStatValue invokeStat = Stats.GetSkillStats(skillCast.SkillId, skillCast.GetSkillGroups(), InvokeEffectType.ReduceSpiritCost);

        spiritCost = Math.Max(0, (int) (-invokeStat.Value + (1 - invokeStat.Rate) * spiritCost));

        if (Value.Stats[StatAttribute.Spirit].Total < spiritCost || Value.Stats[StatAttribute.Stamina].Total < staminaCost)
        {
            return;
        }

        SkillCast = skillCast;

        ConsumeSp(spiritCost);
        ConsumeStamina(staminaCost);

        QuestManager.OnSkillUse(Value, skillCast.SkillId);

        StartCombatStance();

        // TODO: Move this and all others combat cases like recover sp to its own class.
        // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
        SkillLevel? skillLevel = SkillMetadataStorage.GetSkill(skillCast.SkillId)?.SkillLevels
            ?.FirstOrDefault(level => level.Level == skillCast.SkillLevel);
        List<SkillCondition>? conditionSkills = skillLevel?.ConditionSkills;

        float cooldown = skillLevel?.BeginCondition.CooldownTime ?? 0;

        SkillTriggerHandler.FireEvents(this, null, EffectEvent.OnSkillCasted, skillCast.SkillId);
        SkillTriggerHandler.LinkSkillCasted(skillCast.SkillId);

        if (conditionSkills is not null)
        {
            skillCast.SkillAttack = null;
            SkillTriggerHandler.FireTriggerSkills(conditionSkills, skillCast, new(this, this, this, this, EffectEventOrigin.Caster));
        }

        Value.Session?.FieldManager.BroadcastPacket(SkillUsePacket.SkillUse(skillCast));
        Value.Session?.Send(StatPacket.SetStats(this));

        InvokeStatValue skillModifier = Stats.GetSkillStats(skillCast.SkillId, InvokeEffectType.ReduceCooldown);

        bool modifyCooldown = skillModifier.Value != 0 || skillModifier.Rate != 0;

        if (modifyCooldown)
        {
            cooldown = Math.Max(0, (1 - skillModifier.Rate) * cooldown - skillModifier.Value);
        }

        SkillTriggerHandler.SetSkillCooldown(skillCast.SkillId, SkillMetadataStorage.GetChangeOriginSkillId(skillCast.SkillId), (int) (1000 * cooldown) + skillCast.ClientTick, modifyCooldown);

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
                Value.Session?.Send(StatPacket.UpdateStats(this, StatAttribute.Hp));
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
                Value.Session?.Send(StatPacket.UpdateStats(this, StatAttribute.Spirit));
            }
        }
    }

    private DateTime LastSpirtConsumeTime;

    public override void ConsumeSp(int amount)
    {
        if (amount <= 0)
        {
            return;
        }

        lock (Stats)
        {
            Stats[StatAttribute.Spirit].AddValue(-amount);
            LastSpirtConsumeTime = DateTime.Now;
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
                Value.Session?.Send(StatPacket.UpdateStats(this, StatAttribute.Stamina));
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
    }

    /// <summary>
    /// Starts the regen task for the given stat. If noRegen is true and last consume time is less than 1.5 seconds ago, the regen will not be started.
    /// </summary>
    /// <param name="statAttribute">The stat it self. E.g: Stamina</param>
    /// <param name="regenStatAttribute">The stat for the regen amount. E.g: StaminaRegen</param>
    /// <param name="timeStatAttribute">The stat for the regen interval. E.g: StaminaRegenInterval</param>
    /// <param name="noRegen">If regen should pause</param>
    private void StartRegen(StatAttribute statAttribute, StatAttribute regenStatAttribute, StatAttribute timeStatAttribute, bool noRegen = false)
    {
        TaskScheduler.QueueTask(new(Stats[timeStatAttribute].Total)
        {
            Executions = -1
        }, (currentTick, task) =>
        {
            if (Value.FieldPlayer is not null && Value.FieldPlayer != this)
            {
                return -1;
            }

            long interval = Math.Max(Stats[timeStatAttribute].Total, 100);

            lock (Stats)
            {
                //if (timeStatAttribute == StatAttribute.StaminaRegenInterval)
                //    Value.Session?.Send(NoticePacket.Notice($"[{(FieldManager.TickCount64 % 60000) / 1000}] {Stats[statAttribute].Total}", NoticeType.Chat));
                if (Stats[statAttribute].Total >= Stats[statAttribute].Bonus)
                {
                    return interval;
                }

                // If noRegen is true and last consume time is less than 1.5 seconds ago, the regen will not be started.
                if (statAttribute is StatAttribute.Stamina && noRegen && DateTime.Now - LastConsumeStaminaTime < TimeSpan.FromSeconds(1.5))
                {
                    return 1500;
                }
                if (statAttribute is StatAttribute.Spirit && true && DateTime.Now - LastSpirtConsumeTime < TimeSpan.FromSeconds(0.1))
                {
                    return 100;
                }

                //if (timeStatAttribute == StatAttribute.SpRegenInterval)
                //Value.Session?.Send(NoticePacket.Notice($"[{(FieldManager.TickCount64 % 60000) / 1000}] {Stats[statAttribute].Total}", NoticeType.Chat));

                AddStatRegen(statAttribute, regenStatAttribute);
                Value.Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, statAttribute));
                Value.Party?.BroadcastPacketParty(PartyPacket.UpdateHitpoints(Value));
            }

            return interval;
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
        Value.Session?.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(this, true));
        return Task.Run(async () =>
        {
            await Task.Delay(5000, cts.Token);

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
        base.StatsComputed();
        Value.StatsComputed();
    }

    public override void ComputeBaseStats()
    {
        base.ComputeBaseStats();
        Value.ComputeBaseStats();
    }

    public override void AddStats()
    {
        base.AddStats();
        Value.AddStats();
    }
}
