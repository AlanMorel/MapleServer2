using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        public bool IsDead { get; set; }
        public short ZRotation; // In degrees * 10
        public IFieldObject<MobSpawn> OriginSpawn;

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            if (mob != null)
            {
                Id = mob.Id;
                Animation = 255;
                Stats = mob.Stats;
                Experience = mob.Experience;
                Friendly = mob.Friendly;
            }
        }

        public Mob(int id, IFieldObject<MobSpawn> originSpawn) : this(id)
        {
            OriginSpawn = originSpawn;
        }

        public void Damage(double damage)
        {
            Stats.Hp.Total -= (long) damage;
            IsDead = Stats.Hp.Total <= 0;
        }
    }
}
