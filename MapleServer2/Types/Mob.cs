using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public class Mob : NpcMetadata
    {
        //public readonly int Id;
        public short ZRotation; // In degrees * 10
        //public CoordS Speed;
        //public byte Animation;
        //public PlayerStats Stats;

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            Id = mob.Id;
            Animation = 255;
            Stats = mob.Stats;
        }
    }
}
