using Maple2Storage.Types;

namespace MapleServer2.Types.Npcs {
    public class Npc {
        public readonly int Id;
        public short Rotation; // In degrees * 10
        public CoordS Speed;
        public byte Animation;

        public Npc(int id) {
            this.Id = id;
            this.Animation = 255;
        }
    }
}