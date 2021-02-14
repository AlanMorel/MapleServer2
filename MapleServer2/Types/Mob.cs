using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        public short ZRotation; // In degrees * 10
        public bool IsDead { get; private set; }

        public long UpdateStats(Mob mob, double damage)
        {
            mob.Stats.Hp.Max -= (long) damage;
            IsDead = mob.Stats.Hp.Max <= 0;
            return mob.Stats.Hp.Max;
        }

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            Id = mob.Id;
            Animation = 255;
            mob.Stats.Hp.Max = mob.Stats.Hp.Total;
            Stats = mob.Stats;
            Friendly = mob.Friendly;
        }
    }
}
