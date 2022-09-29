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
    Owner = 1, // immunities, "logic effects" that read other effects
    Entity = 2, // stat changes
    Element = 4, // Type Element, Damage_Per_Second
    Shield = 6,
    CrowdControl = 8, // Stun, Slow, Freeze, Shock
    Recovery = 16,
    EnemyRecovery = 32,
    PcCafe = 64,
    FishingLure = 128,
    AccountBuff = 256,
    LapentaResonance = 512,
    Socketing = 1024
}

public enum BuffCategory
{
    PlayerEffect = 0, // not quite sure
    EnvironmentalHazard = 1, // not quite sure
    //Unknown = 2,
    // Stun1 = 4, // not sure what the difference is with 7
    EnemyDot = 6,
    Stun = 7,
    Slow = 8,
    BossResistance = 9,
    // Unknown = 99,
    EnemyStunned = 1007,
    EnemyRecovery = 2001 // some kind of positive buff for enemies
}

public enum SgiTarget : byte
{
    None = 0,
    Elite = 3,
    Boss = 4
}

public enum SkillDirection : byte
{
    Unknown1 = 0,
    LockOn = 1,
    Unknown2 = 2,
    Facing = 3,
    Unknown3 = 4
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
    None = 0, // used on skill attacks whos sole purpose is deploying a region skill/self buff
    Enemy = 1,
    Ally = 2,
    Player1 = 3, // Unknown, 
    Player2 = 5, // Unknown, 
    Player3 = 6, // Unknown, Recovery
    Player4 = 7, // Unknown, Debuff (Archeon's ice bombs)

    HungryMobs = 8
}

public enum SkillTarget : byte
{
    SkillTarget = 0,
    Owner = 1,
    Target = 2,
    Caster = 3,
    PetOwner = 4
}

// separate enum in case anything is not quite right
public enum SkillOwner : byte
{
    Inherit = 0,
    Owner = 1,
    Target = 2,
    Caster = 3,
    Attacker = 5, // example: Soul Crystal applying a hit to a target when external attacker hits, but damage is credited to that attacker rather than Soul Crystal's caster
}

public enum EffectEvent : byte
{
    Activate = 0, // always 0 in skills
    Tick = 0,
    OnEvade = 1, // owner
    OnBlock = 2, // owner
    OnAttacked = 4, // owner, target
    OnOwnerAttackCrit = 5, // owner
    OnOwnerAttackHit = 6, // owner
    OnSkillCasted = 7, // owner, caster

    OnBuffStacksReached = 10, // owner, caster
    OnInvestigate = 11, // owner. not fired in homes
    OnBuffTimeExpiring = 13, // owner
    OnSkillCastEnd = 14, // owner. unsure
    OnEffectApplied = 16, // owner
    OnEffectRemoved = 17, // owner
    OnLifeSkillGather = 18, // owner
    OnAttackMiss = 19, // owner,

    UnknownKritiasPuzzleEvent = 20, // owner
    UnknownWizardEvent = 102,
    UnknownStrikerEvent = 103 // owner
}

public enum TargetAllieganceType : byte
{
    None = 0,
    ClosestEnemy = 1,
    ClosestAlly = 2
}

public enum ConditionOperator : byte
{
    None,
    Equals,
    LessEquals,
    GreaterEquals,
    Less,
    Greater
}

public enum DungeonRoomGroupType : byte
{
    Normal,
    Raid,
    ChaosRaid,
    DarkStream,
    Vip,
    Event,
    GuildRaid,
    FameChallenge,
    Lapenta,
    Colosseum
}

public enum ContinentCode : byte
{
    None = 0,
    Kritias = 202 // probably correct?
}


public enum ItemPresetType : byte
{
    None = 0,
    Blade = 54
}

public enum EffectKeepCondition : byte
{
    TimerDuration = 0,
    SkillDuration = 1,
    TrackCooldown = 5, // same as timer but track cooldown info after expiration
    UnlimitedDuration = 99
}

public enum EffectResetCondition : byte
{
    ResetTimer = 0,
    DontResetTimer = 1,
    ResetTimer2 = 2, // Difference between this and 0 is unclear
    ChangeTimer = 3 // set timer to be lower if necessary
}

public enum EffectDotCondition : byte
{
    ImmediateFire = 0,
    DelayedFire = 1,
    // Unknown = 2
}

public enum CasterIndividualEffect : byte
{
    SingleEffect = 0,
    PerCasterStack = 1
}

public enum SkillGroupType : byte
{
    None = 0,
    Class = 1,
    Lapenshard = 2
}

public enum InvokeEffectType : byte
{
    ReduceCooldown = 1,
    IncreaseSkillDamage = 2,
    IncreaseDuration = 3,
    IncreaseDotDamage = 5,
    // 20 (90050324)
    // 21 (90050324)
    IncreaseEvasionDebuff = 23,
    IncreaseCritEvasionDebuff = 26,
    // 34 (90050853)
    // 35 (90050854)
    // 38 (90050806)
    // 40 (90050827)
    ReduceSpiritCost = 56, // flame wave 0 spirit cost? value=0, rate=2 on 10300250. subtract rate from cost (10500201, rate=20%)
    // 57 (90050351)
    IncreaseHealing = 58
}
