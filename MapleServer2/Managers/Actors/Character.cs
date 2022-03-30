using Maple2Storage.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public partial class FieldManager
{
    private partial class Character : FieldActor<Player>
    {
        public override Stats Stats
        {
            get => Value.Stats;
            set => Value.Stats = value;
        }

        private CancellationTokenSource CombatCTS;

        private Task HpRegenThread;
        private Task SpRegenThread;
        private Task StaRegenThread;

        public Character(int objectId, Player value) : base(objectId, value)
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

            // TODO: Move this and all others combat cases like recover sp to its own class.
            // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
            if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
            {
                Status status = new(skillCast, ObjectId, ObjectId, 1);
                StatusHandler.Handle(Value.Session, status);
            }

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
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
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
                stat.Decrease(Math.Min(amount, stat.Total));
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
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
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
                Stat stat = Stats[StatAttribute.Spirit];
                Stats[StatAttribute.Spirit].Decrease(Math.Min(amount, stat.Total));
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
                    Stats[StatAttribute.Stamina].Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatAttribute.Stamina));
                }
            }
        }

        public override void ConsumeStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatAttribute.Stamina];
                Stats[StatAttribute.Stamina].Decrease(Math.Min(amount, stat.Total));
            }

            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(StatAttribute.Stamina, StatAttribute.StaminaRegen, StatAttribute.StaminaRegenInterval);
            }
        }

        private Task StartRegen(StatAttribute statAttribute, StatAttribute regenStatAttribute, StatAttribute timeStatAttribute)
        {
            // TODO: merge regen updates with larger packets
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(Stats[timeStatAttribute].Total);

                    lock (Stats)
                    {
                        if (Stats[statAttribute].Total >= Stats[statAttribute].Bonus)
                        {
                            return;
                        }

                        // TODO: Check if regen-enabled
                        AddStatRegen(statAttribute, regenStatAttribute);
                        Value.Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, statAttribute));
                        if (Value.Party != null)
                        {
                            Value.Party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(Value));
                        }
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
                    int missingAmount = stat.Bonus - stat.Total;
                    stat.Increase(Math.Clamp(regenAmount, 0, missingAmount));
                }
            }
        }
    }
}
