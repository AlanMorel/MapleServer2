using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        public bool IsDead { get; set; }
        public short ZRotation; // In degrees * 10

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            Id = mob.Id;
            Animation = 255;
            mob.Stats.Hp.Max = mob.Stats.Hp.Total;
            Stats = mob.Stats;
            Friendly = mob.Friendly;
        }

        public void UpdateStats(double damage)
        {
            Mob mob = this;
            mob.Stats.Hp.Max -= (long) damage;
            mob.IsDead = mob.Stats.Hp.Max <= 0;
        }
    }
}
