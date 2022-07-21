namespace Maple2Storage.Enums;

/// <summary>
/// Active = 0,
/// Passive = 1,
/// GM = 3
/// </summary>
public enum SkillType
{
    Active = 0,
    Passive = 1,
    GM = 3
}

public enum SkillSubType
{
    None = 0,
    Status = 2,
    GM = 5,
    Global = 6
}

public enum BuffType
{
    None = 0,
    Buff = 1,
    Debuff = 2
}

public enum BuffSubType
{
    None = 0,
    Owner = 1,
    Entity = 2,
    Element = 4, // Type Element, Damage_Per_Second
    Shield = 6,
    CrowdControl = 8, // Stun, Slow, Freeze, Shock
    Recovery = 16,
    Unknown2 = 64,
    Unknown3 = 1024
}

public enum BuffCategroy
{
    Stun = 7,
    Slow = 8
}

public enum SkillRangeType : byte
{
    Special = 0x00,
    Melee = 0x01,
    Ranged = 0x02
}

public enum CompulsionType : byte
{
    AlwaysCrit = 2
}

public enum ApplyTarget : byte
{
    Closest = 0,
    Owner = 1,
    Target = 2,
    Caster = 3,
    PetOwner = 4,
    Attacker = 5,
    RegionBuff = 6,
    RegionDebuff = 7,
    HungryMobs = 8
}

// separate enum in case anything is not quite right
public enum SkillOwner : byte
{
    None = 0,
    Owner = 1,
    Target = 2,
    Caster = 3,
    PetOwner = 4,
    Attacker = 5, // example: Soul Crystal applying a hit to a target when external attacker hits, but damage is credited to that attacker rather than Soul Crystal's caster
    RegionBuff = 6,
    RegionDebuff = 7,
}
