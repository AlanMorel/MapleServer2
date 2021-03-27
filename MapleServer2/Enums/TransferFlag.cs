using System;

namespace MapleServer2.Enums
{
    [Flags]
    public enum TransferFlag : byte
    {
        None = 0,
        Unknown1 = 1,
        Splitable = 2,
        Tradeable = 4,
        Binds = 8,
        Unknown2 = 16,
        Unknown4 = 24,
        Unknown3 = 32,
    }
}
