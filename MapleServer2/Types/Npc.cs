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
            this.Id = id;
            this.Animation = 255;
            this.stats = PlayerStats.Default();
        }
    }
}