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
                HpRegenThread = StartRegen(StatId.Hp, StatId.HpRegen, StatId.HpRegenInterval);
            }

            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(StatId.Spirit, StatId.SpRegen, StatId.SpRegenInterval);
            }

            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(StatId.Stamina, StatId.StaminaRegen, StatId.StaminaRegenInterval);
            }
        }

        public override void Cast(SkillCast skillCast)
        {
            int spiritCost = skillCast.GetSpCost();
            int staminaCost = skillCast.GetStaCost();

            if (Value.Stats[StatId.Spirit].Total < spiritCost || Value.Stats[StatId.Stamina].Total < staminaCost)
            {
                return;
            }

            SkillCast = skillCast;

            ConsumeSp(spiritCost);
            ConsumeStamina(staminaCost);
            Value.Session.SendNotice(skillCast.SkillId.ToString());

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
                Stat stat = Stats[StatId.Hp];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Hp));
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
                Stat stat = Stats[StatId.Hp];
                stat.Decrease(Math.Min(amount, stat.Total));
            }

            if (HpRegenThread == null || HpRegenThread.IsCompleted)
            {
                HpRegenThread = StartRegen(StatId.Hp, StatId.HpRegen, StatId.HpRegenInterval);
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
                Stat stat = Stats[StatId.Spirit];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Spirit));
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
                Stat stat = Stats[StatId.Spirit];
                Stats[StatId.Spirit].Decrease(Math.Min(amount, stat.Total));
            }

            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(StatId.Spirit, StatId.SpRegen, StatId.SpRegenInterval);
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
                Stat stat = Stats[StatId.Stamina];
                if (stat.Total < stat.Bonus)
                {
                    Stats[StatId.Stamina].Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Stamina));
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
                Stat stat = Stats[StatId.Stamina];
                Stats[StatId.Stamina].Decrease(Math.Min(amount, stat.Total));
            }

            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(StatId.Stamina, StatId.StaminaRegen, StatId.StaminaRegenInterval);
            }
        }

        private Task StartRegen(StatId statId, StatId regenStatId, StatId timeStatId)
        {
            // TODO: merge regen updates with larger packets
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(Stats[timeStatId].Total);

                    lock (Stats)
                    {
                        if (Stats[statId].Total >= Stats[statId].Bonus)
                        {
                            return;
                        }

                        // TODO: Check if regen-enabled
                        AddStatRegen(statId, regenStatId);
                        Value.Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, statId));
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

        private void AddStatRegen(StatId statIndex, StatId regenStatIndex)
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
