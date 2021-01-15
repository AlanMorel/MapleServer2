using Maple2Storage.Types;

namespace MapleServer2.Types
{
    public class Npc
    {
        public readonly int Id;
        public short Rotation; // In degrees * 10
        public CoordS Speed;
        public byte Animation;
        public PlayerStats stats;

        public Npc(int id)
        {
            Id = id;
            Animation = 255;
        }
    }
}
