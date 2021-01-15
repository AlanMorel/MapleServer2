using Maple2Storage.Types;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class SyncState
    {
        public byte Animation1;
        public byte Animation2;
        public SyncStateFlag Flag = SyncStateFlag.None;
        public CoordS Coord;
        public short Rotation;
        public byte Animation3;
        public float UnknownFloat1;
        public float UnknownFloat2;
        public CoordS Speed;
        public byte Unknown1;
        public short Unknown2;
        public short Unknown3;
        public int Unknown4;

        // Flag1
        public int Flag1Unknown1;
        public short Flag1Unknown2;

        // Flag2
        public CoordF Flag2Unknown1;
        public string Flag2Unknown2;

        // Flag3
        public int Flag3Unknown1;
        public string Flag3Unknown2;

        // Flag4
        public string Flag4Unknown;

        // Flag5
        public int Flag5Unknown1;
        public string Flag5Unknown2;

        // Flag6
        public int Flag6Unknown1;
        public int Flag6Unknown2;
        public byte Flag6Unknown3;
        public CoordF Flag6Unknown4;
        public CoordF Flag6Unknown5;
    }
}
