using System;
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
            if (mob != null)
            {
                Id = mob.Id;
                Animation = 255;
                Stats = mob.Stats;
                Friendly = mob.Friendly;
            }
        }

        public void UpdateStats(double damage)
        {
            Stats.Hp.Total -= (long) damage;
            IsDead = Stats.Hp.Total <= 0;
        }
    }
}
