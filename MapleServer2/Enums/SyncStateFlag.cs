using System;

namespace MapleServer2.Enums
{
    [Flags]
    public enum SyncStateFlag : byte
    {
        None = 0,
        Flag1 = 1,
        Flag2 = 2,
        Flag3 = 4,
        Flag4 = 8,
        Flag5 = 16,
        Flag6 = 32,
    }
}