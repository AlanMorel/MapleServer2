namespace MapleServer2.Enums;

public enum HitType : byte
{
    Normal = 0,
    Critical = 1,
    Miss = 2
}

public enum DamageRangeType : byte
{
    Status = 0x00,
    Melee = 0x01,
    Ranged = 0x02
}

public enum DamageType : byte
{
    None = 0x00,
    Physical = 0x01,
    Magic = 0x02
}

public enum Element : byte
{
    None = 0x00,
    Fire = 0x01,
    Ice = 0x02,
    Electric = 0x03,
    Holy = 0x04,
    Dark = 0x05,
    Poison = 0x06
}
