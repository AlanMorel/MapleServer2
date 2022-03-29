using Maple2.PathEngine;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public partial class FieldManager
{
    private abstract partial class FieldActor<T> : FieldObject<T>, IFieldActor<T>, IDisposable
    {
        public CoordF Velocity { get; set; }
        public short Animation { get; set; }

        public virtual Stats Stats { get; set; }
        public bool IsDead { get; set; }

        public List<Status> Statuses { get; set; }
        public SkillCast SkillCast { get; set; }
        public bool OnCooldown { get; set; }
        public FieldNavigator Navigator { get; set; }
        public Agent Agent { get; set; }

        public FieldActor(int objectId, T value) : base(objectId, value) { }

        public virtual void UpdateFixed() { }

        public virtual void Update() { }

        public virtual void MoveBy(CoordF displacement)
        {
            // TODO: Break displacement into segments, then set path.
            Velocity = displacement;
        }

        public virtual void MoveTo(CoordF target)
        {
            // TODO: Perform pathfinding, then set path.
            Velocity = target - Coord;
        }

        public virtual void Cast(SkillCast skillCast)
        {
            SkillCast = skillCast;

            // TODO: Move this and all others combat cases like recover sp to its own class.
            // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
            if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
            {
                Status status = new(skillCast, ObjectId, ObjectId, 1);
                //StatusHandler.Handle(Value.Session, status);
            }
        }

        public virtual void Heal(GameSession session, Status status, int amount)
        {
            session.FieldManager.BroadcastPacket(SkillDamagePacket.Heal(status, amount));
            Stats[StatAttribute.Hp].Increase(amount);
            session.Send(StatPacket.UpdateStats(this, StatAttribute.Hp));
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
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
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
                stat.Decrease(Math.Min(amount, stat.Total));
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
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
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
                Stat stat = Stats[StatAttribute.Spirit];
                Stats[StatAttribute.Spirit].Decrease(Math.Min(amount, stat.Total));
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
                    Stats[StatAttribute.Stamina].Increase(Math.Min(amount, stat.Bonus - stat.Total));
                }
            }
        }

        public virtual void ConsumeStamina(int amount)
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
        }

        public virtual void Damage(DamageHandler damage, GameSession session)
        {
            Stat health = Stats[StatAttribute.Hp];
            health.Decrease((long) damage.Damage);
            if (health.Total <= 0)
            {
                Perish();
            }
        }

        public virtual void Perish()
        {
            IsDead = true;
        }

        public virtual void Animate(string sequenceName)
        {
        }

        public void Dispose()
        {
            Agent?.Dispose();
        }
    }
}
