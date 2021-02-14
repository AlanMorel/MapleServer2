using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        public short ZRotation; // In degrees * 10
        private bool IsDead;

        public bool GetIsDead()
        {
            return IsDead;
        }

        public bool SetIsDead(bool state)
        {
            return IsDead = state;
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
