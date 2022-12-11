﻿namespace Maple2Storage.Enums;

// Player Stats in Packet Order - Count: 35 (0x23)
public enum StatAttribute : short
{
    Str = 0,
    Dex = 1,
    Int = 2,
    Luk = 3,
    Hp = 4,
    HpRegen = 5,
    HpRegenInterval = 6,
    Spirit = 7,
    SpRegen = 8,
    SpRegenInterval = 9,
    Stamina = 10,
    StaminaRegen = 11,
    StaminaRegenInterval = 12,
    AttackSpeed = 13,
    MovementSpeed = 14,
    Accuracy = 15,
    Evasion = 16,
    CritRate = 17,
    CritDamage = 18,
    CritEvasion = 19,
    Defense = 20,
    PerfectGuard = 21,
    JumpHeight = 22,
    PhysicalAtk = 23,
    MagicAtk = 24,
    PhysicalRes = 25,
    MagicRes = 26,
    MinWeaponAtk = 27,
    MaxWeaponAtk = 28,
    MinDamage = 29,
    MaxDamage = 30,
    Pierce = 31,
    MountMovementSpeed = 32,
    BonusAtk = 33,
    PetBonusAtk = 34,
    ExpBonus = 11001,
    MesoBonus = 11002,
    SwimSpeed = 11003,
    DashDistance = 11004,
    TonicDropRate = 11005,
    GearDropRate = 11006,
    TotalDamage = 11007,
    CriticalDamage = 11008,
    Damage = 11009,
    LeaderDamage = 11010,
    EliteDamage = 11011,
    BossDamage = 11012, // This is actually used in conjunction with "sgi_target" in the XMLs. It's not boss damage, it's for a specified monster type. Currently ignoring it and just using it as boss damage, as nothing in the data uses it for non-boss mobs
    HpOnKill = 11013,
    SpiritOnKill = 11014,
    StaminaOnKill = 11015,
    Heal = 11016,
    AllyRecovery = 11017,
    IceDamage = 11018,
    FireDamage = 11019,
    DarkDamage = 11020,
    HolyDamage = 11021,
    PoisonDamage = 11022,
    ElectricDamage = 11023,
    MeleeDamage = 11024,
    RangedDamage = 11025,
    PhysicalPiercing = 11026,
    MagicPiercing = 11027,
    IceDamageReduce = 11028,
    FireDamageReduce = 11029,
    DarkDamageReduce = 11030,
    HolyDamageReduce = 11031,
    PoisonDamageReduce = 11032,
    ElectricDamageReduce = 11033,
    StunReduce = 11034,
    CooldownReduce = 11035,
    DebuffDurationReduce = 11036,
    MeleeDamageReduce = 11037,
    RangedDamageReduce = 11038,
    KnockbackReduce = 11039,
    MeleeStun = 11040, //melee chance to stun
    RangedStun = 11041, //melee chance to stun
    MeeleeKnockback = 11042, //chance of knockback after meele att
    RangedKnockback = 11043, //chance of knockback after ranged att
    MeleeImmob = 11044, //ranged chance to immob
    RangedImmob = 11045, //ranged chance to immob
    MeleeAoeDamage = 11046, //melee chance to do aoe damage
    RangedAoeDamage = 11047, //ranged chance to do aoe damage
    DropRate = 11048,
    QuestExp = 11049,
    QuestMeso = 11050,
    InvokeEffect1 = 11051, // needs better name
    InvokeEffect2 = 11052, // needs better name
    InvokeEffect3 = 11053, // needs better name
    PvPDamage = 11054,
    PvPDefense = 11055,
    GuildExp = 11056,
    GuildCoin = 11057,
    McKayXpOrb = 11058, //mc-kay experience orb value bonus
    FishingExp = 11059,
    ArcadeExp = 11060,

    PerformanceExp = 11063,
    BlackMarketReduce = 11064,
    EnchantCatalystDiscount = 11065,
    MeretReviveFee = 11066,
    MiningBonus = 11067,
    RanchingBonus = 11068,
    SmithingExp = 11069,
    HandicraftMastery = 11070,
    ForagingBonus = 11071,
    FarmingBonus = 11072,
    AlchemyMastery = 11073,
    CookingMastery = 11074,
    ForagingExp = 11075,

    //techs
    TECH = 11077, //level 1 skill
    TECH_2 = 11078, //2nd level 1 skill
    TECH_10 = 11079, //lv 10 skill
    TECH_13 = 11080, //lv 13 skill
    TECH_16 = 11081, // lv 16 skill
    TECH_19 = 11082, // lv 19 skill
    TECH_22 = 11083, // lv 22 skill
    TECH_25 = 11084, // lv 25 skill
    TECH_28 = 11085, // lv 28 skill
    TECH_31 = 11086, // lv 31 skill
    TECH_34 = 11087, // lv 34 skill
    TECH_37 = 11088, // lv 37 skill
    TECH_40 = 11089, // lv 40 skill
    TECH_43 = 11090, // lv 43 skill

    OXQuizExp = 11091,
    TrapMasterExp = 11092,
    SoleSurvivorExp = 11093,
    CrazyRunnerExp = 11094,
    LudiEscapeExp = 11095,
    SpringBeachExp = 11096,
    DanceDanceExp = 11097,

    OXMovementSpeed = 11098,
    TrapMasterMovementSpeed = 11099,
    SoleSurvivorMovementSpeed = 11100,
    CrazyRunnerMovementSpeed = 11101,
    LudiEscapeMovementSpeed = 11102,
    SpringBeachMovementSpeed = 11103,
    DanceDanceStopMovementSpeed = 11104,

    GenerateSpiritOrbs = 11105,
    GenerateStaminaOrbs = 11106,
    ValorTokens = 11107,
    PvPExp = 11108,
    DarkDescentDamageBonus = 11109,
    DarkDescentEvasion = 11110,
    DarkDescentDamageReduce = 11111,

    DoubleFishingMastery = 11112,
    DoublePerformanceMastery = 11113,

    ExploredAreasMovementSpeed = 11114,
    AirMountAscentSpeed = 11115,
    AdditionalEffect_95000018 = 11116,
    EnemyDefenseDecreaseOnHit = 11117,
    EnemyAttackDecreaseOnHit = 11118,

    IncreaseTotalDamageIf1NearbyEnemy = 11119, // Increases damage if there is an enemy within 5m
    IncreaseTotalDamageIf3NearbyEnemies = 11120, // Increases damage if there is at least 3 enemies within 5m
    IncreaseTotalDamageIf80Spirit = 11121, // Increase damage if you have 80 or more spirit
    IncreaseTotalDamageIfFullStamina = 11122,
    IncreaseTotalDamageIfHerbEffectActive = 11123, // Increase damage if you have a herb-like effect active
    IncreaseTotalDamageToWorldBoss = 11124,

    Effect95000026 = 11125,
    Effect95000027 = 11126,
    Effect95000028 = 11127,
    Effect95000029 = 11128,
    StaminaRecoverySpeed = 11129,
    MaxWeaponAttack = 11130,

    DoubleMiningProduction = 11131,
    DoubleRanchingProduction = 11132,
    DoubleForagingProduction = 11133,
    DoubleFarmingProduction = 11134,
    DoubleSmithingProduction = 11135,
    DoubleHandicraftProduction = 11136,
    DoubleAlchemyProduction = 11137,
    DoubleCookingProduction = 11138,

    DoubleMiningMastery = 11139,
    DoubleRanchingMastery = 11140,
    DoubleForagingMastery = 11141,
    DoubleFarmingMastery = 11142,
    DoubleSmithingMastery = 11143,
    DoubleHandicraftMastery = 11144,
    DoubleAlchemyMastery = 11145,
    DoubleCookingMastery = 11146,

    ChaosRaidWeaponAttack = 11147,
    ChaosRaidAttackSpeed = 11148,
    ChaosRaidAccuracy = 11149,
    ChaosRaidHealth = 11150,

    StaminaAndSpiritFromOrbs = 11151,

    WorldBossExp = 11152,
    WorldBossDropRate = 11153,
    WorldBossDamageReduce = 11154,

    Effect9500016 = 11155,
    PetCaptureRewards = 11156,

    MiningEfficency = 11157,
    RanchingEfficiency = 11158,
    ForagingEfficiency = 11159,
    FarmingEfficiency = 11160,

    ShanghaiCrazyRunnersExp = 11161,
    ShanghaiCrazyRunnersMovementSpeed = 11162,

    HealthBasedDamageReduce = 11163,
    ReduceMesoRevivalFee = 11164,
    ImproveRidingRunSpeed = 11165,
    ImproveDungeonRewardMeso = 11166,
    ImproveShopBuyingMeso = 11167,
    ImproveItemboxRewardMeso = 11168,
    ReduceRemakeOptionRee = 11169,
    ReduceAirTaxiFee = 11170,
    ImproveSocketUnlockProbability = 11171,
    ReduceGemstoneUpgradeFee = 11172,
    ReducePetRemakeOptionFee = 11173,
    ImproveRidingSpeed = 11174,
    ImproveSurvivalKill_exp = 11175,
    ImproveSurvivalTime_exp = 11176,
    OffensivePhysicalDamage = 11177,
    OffensiveMagicalDamage = 11178,
    ReduceGameitemSocketUnlockFee = 11179,
}

public enum StatAttributeType
{
    Rate,
    Flat
}
