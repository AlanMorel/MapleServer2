using System;

namespace MapleServer2.Enums {
    [Flags]
    public enum FieldObjectUpdate : byte {
        None = 0,
        Type1 = 1,
        Move = 2,
        Type3 = 4,
        Type4 = 8,
        Type5 = 16,
        Type6 = 32,
        Animate = 64,
    }
}