using System.Xml;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace Maple2Storage.Types;

public class StatEntry
{
    public StatAttribute Attribute;
    public StatAttributeType AttributeType;
    public string Name;

    public StatEntry(StatAttribute attribute, StatAttributeType attributeType, string name)
    {
        Attribute = attribute;
        AttributeType = attributeType;
        Name = name;
    }

    public static readonly Dictionary<string, StatEntry> Entries = new()
    {
        ["str"] = new(StatAttribute.Str, StatAttributeType.Flat, "str"),
        ["int"] = new(StatAttribute.Int, StatAttributeType.Flat, "int"),
        ["luk"] = new(StatAttribute.Luk, StatAttributeType.Flat, "luk"),
        ["dex"] = new(StatAttribute.Dex, StatAttributeType.Flat, "dex"),
        ["hp"] = new(StatAttribute.Hp, StatAttributeType.Flat, "hp"),
        ["hp_rgp"] = new(StatAttribute.HpRegen, StatAttributeType.Flat, "hp_rgp"),
        ["hp_inv"] = new(StatAttribute.HpRegenInterval, StatAttributeType.Flat, "hp_inv"),
        ["sp"] = new(StatAttribute.Spirit, StatAttributeType.Flat, "sp"),
        ["sp_rgp"] = new(StatAttribute.SpRegen, StatAttributeType.Flat, "sp_rgp"),
        ["sp_inv"] = new(StatAttribute.SpRegenInterval, StatAttributeType.Flat, "sp_inv"),
        ["ep"] = new(StatAttribute.Stamina, StatAttributeType.Flat, "ep"),
        ["ep_rgp"] = new(StatAttribute.StaminaRegen, StatAttributeType.Flat, "ep_rgp"),
        ["ep_inv"] = new(StatAttribute.StaminaRegenInterval, StatAttributeType.Flat, "ep_inv"),
        ["asp"] = new(StatAttribute.AttackSpeed, StatAttributeType.Flat, "asp"),
        ["msp"] = new(StatAttribute.MovementSpeed, StatAttributeType.Flat, "msp"),
        ["atp"] = new(StatAttribute.Accuracy, StatAttributeType.Flat, "atp"),
        ["evp"] = new(StatAttribute.Evasion, StatAttributeType.Flat, "evp"),
        ["cap"] = new(StatAttribute.CritRate, StatAttributeType.Flat, "cap"),
        ["cad"] = new(StatAttribute.CritDamage, StatAttributeType.Flat, "cad"),
        ["car"] = new(StatAttribute.CritEvasion, StatAttributeType.Flat, "car"),
        ["ndd"] = new(StatAttribute.Defense, StatAttributeType.Flat, "ndd"),
        ["bap"] = new(StatAttribute.BonusAtk, StatAttributeType.Flat, "bap"),
        ["rmsp"] = new(StatAttribute.MountMovementSpeed, StatAttributeType.Flat, "rmsp"),
        ["pen"] = new(StatAttribute.Pierce, StatAttributeType.Flat, "pen"),
        ["dmg"] = new(StatAttribute.Damage, StatAttributeType.Flat, "dmg"),
        ["wapmin"] = new(StatAttribute.MinWeaponAtk, StatAttributeType.Flat, "wapmin"),
        ["wapmax"] = new(StatAttribute.MaxWeaponAtk, StatAttributeType.Flat, "wapmax"),
        ["mar"] = new(StatAttribute.MagicRes, StatAttributeType.Flat, "mar"),
        ["map"] = new(StatAttribute.MagicAtk, StatAttributeType.Flat, "map"),
        ["par"] = new(StatAttribute.PhysicalRes, StatAttributeType.Flat, "par"),
        ["pap"] = new(StatAttribute.PhysicalAtk, StatAttributeType.Flat, "pap"),
        ["jmp"] = new(StatAttribute.JumpHeight, StatAttributeType.Flat, "jmp"),
        ["abp"] = new(StatAttribute.PerfectGuard, StatAttributeType.Flat, "abp"),
        ["bap_pet"] = new(StatAttribute.PetBonusAtk, StatAttributeType.Flat, "bap_pet"),

        ["seg"] = new(StatAttribute.ExpBonus, StatAttributeType.Rate, "seg"),
        ["smd"] = new(StatAttribute.MesoBonus, StatAttributeType.Rate, "smd"),
        ["sss"] = new(StatAttribute.SwimSpeed, StatAttributeType.Rate, "sss"),
        ["dashdistance"] = new(StatAttribute.DashDistance, StatAttributeType.Rate, "dashdistance"),
        ["spd"] = new(StatAttribute.TonicDropRate, StatAttributeType.Rate, "spd"),
        ["sid"] = new(StatAttribute.GearDropRate, StatAttributeType.Rate, "sid"),
        ["finaladditionaldamage"] = new(StatAttribute.TotalDamage, StatAttributeType.Rate, "finaladditionaldamage"),
        ["cri"] = new(StatAttribute.CriticalDamage, StatAttributeType.Rate, "cri"),
        ["sgi"] = new(StatAttribute.Damage, StatAttributeType.Rate, "sgi"),
        ["sgi_leader"] = new(StatAttribute.LeaderDamage, StatAttributeType.Rate, "sgi_leader"),
        ["sgi_elite"] = new(StatAttribute.EliteDamage, StatAttributeType.Rate, "sgi_elite"),
        ["sgi_boss"] = new(StatAttribute.BossDamage, StatAttributeType.Rate, "sgi_boss"),
        ["killhprestore"] = new(StatAttribute.HpOnKill, StatAttributeType.Rate, "killhprestore"),
        ["killsprestore"] = new(StatAttribute.SpiritOnKill, StatAttributeType.Rate, "killsprestore"),
        ["killeprestore"] = new(StatAttribute.StaminaOnKill, StatAttributeType.Rate, "killeprestore"),
        ["heal"] = new(StatAttribute.Heal, StatAttributeType.Rate, "heal"),
        ["receivedhealincrease"] = new(StatAttribute.AllyRecovery, StatAttributeType.Rate, "receivedhealincrease"),
        ["icedamage"] = new(StatAttribute.IceDamage, StatAttributeType.Rate, "icedamage"),
        ["firedamage"] = new(StatAttribute.FireDamage, StatAttributeType.Rate, "firedamage"),
        ["darkdamage"] = new(StatAttribute.DarkDamage, StatAttributeType.Rate, "darkdamage"),
        ["lightdamage"] = new(StatAttribute.HolyDamage, StatAttributeType.Rate, "lightdamage"),
        ["poisondamage"] = new(StatAttribute.PoisonDamage, StatAttributeType.Rate, "poisondamage"),
        ["thunderdamage"] = new(StatAttribute.ElectricDamage, StatAttributeType.Rate, "thunderdamage"),
        ["nddincrease"] = new(StatAttribute.MeleeDamage, StatAttributeType.Rate, "nddincrease"),
        ["lddincrease"] = new(StatAttribute.RangedDamage, StatAttributeType.Rate, "lddincrease"),
        ["parpen"] = new(StatAttribute.PhysicalPiercing, StatAttributeType.Rate, "parpen"),
        ["marpen"] = new(StatAttribute.MagicPiercing, StatAttributeType.Rate, "marpen"),
        ["icedamagereduce"] = new(StatAttribute.IceDamageReduce, StatAttributeType.Rate, "icedamagereduce"),
        ["firedamagereduce"] = new(StatAttribute.FireDamageReduce, StatAttributeType.Rate, "firedamagereduce"),
        ["darkdamagereduce"] = new(StatAttribute.DarkDamageReduce, StatAttributeType.Rate, "darkdamagereduce"),
        ["lightdamagereduce"] = new(StatAttribute.HolyDamageReduce, StatAttributeType.Rate, "lightdamagereduce"),
        ["poisondamagereduce"] = new(StatAttribute.PoisonDamageReduce, StatAttributeType.Rate, "poisondamagereduce"),
        ["thunderdamagereduce"] = new(StatAttribute.ElectricDamageReduce, StatAttributeType.Rate, "thunderdamagereduce"),
        ["stunreduce"] = new(StatAttribute.StunReduce, StatAttributeType.Rate, "stunreduce"),
        ["conditionreduce"] = new(StatAttribute.DebuffDurationReduce, StatAttributeType.Rate, "conditionreduce"),
        ["skillcooldown"] = new(StatAttribute.CooldownReduce, StatAttributeType.Rate, "skillcooldown"),
        ["neardistancedamagereduce"] = new(StatAttribute.MeleeDamageReduce, StatAttributeType.Rate, "neardistancedamagereduce"),
        ["longdistancedamagereduce"] = new(StatAttribute.RangedDamageReduce, StatAttributeType.Rate, "longdistancedamagereduce"),
        ["knockbackreduce"] = new(StatAttribute.KnockbackReduce, StatAttributeType.Rate, "knockbackreduce"),
        ["stunprocndd"] = new(StatAttribute.MeleeStun, StatAttributeType.Rate, "stunprocndd"),
        ["stunprocldd"] = new(StatAttribute.RangedStun, StatAttributeType.Rate, "stunprocldd"),
        ["knockbackprocndd"] = new(StatAttribute.MeeleeKnockback, StatAttributeType.Rate, "knockbackprocndd"),
        ["knockbackprocldd"] = new(StatAttribute.RangedKnockback, StatAttributeType.Rate, "knockbackprocldd"),
        ["snareprocndd"] = new(StatAttribute.MeleeImmob, StatAttributeType.Rate, "snareprocndd"),
        ["snareprocldd"] = new(StatAttribute.RangedImmob, StatAttributeType.Rate, "snareprocldd"),
        ["aoeprocndd"] = new(StatAttribute.MeleeAoeDamage, StatAttributeType.Rate, "aoeprocndd"),
        ["aoeprocldd"] = new(StatAttribute.RangedAoeDamage, StatAttributeType.Rate, "aoeprocldd"),
        ["npckilldropitemincrate"] = new(StatAttribute.DropRate, StatAttributeType.Rate, "npckilldropitemincrate"),
        ["seg_questreward"] = new(StatAttribute.QuestExp, StatAttributeType.Rate, "seg_questreward"),
        ["smd_questreward"] = new(StatAttribute.QuestMeso, StatAttributeType.Rate, "smd_questreward"),
        ["seg_fishingreward"] = new(StatAttribute.FishingExp, StatAttributeType.Rate, "seg_fishingreward"),
        ["seg_arcadereward"] = new(StatAttribute.ArcadeExp, StatAttributeType.Rate, "seg_arcadereward"),
        ["seg_playinstrumentreward"] = new(StatAttribute.PerformanceExp, StatAttributeType.Rate, "seg_playinstrumentreward"),
        ["invoke_effect1"] = new(StatAttribute.InvokeEffect1, StatAttributeType.Rate, "invoke_effect1"),
        ["invoke_effect2"] = new(StatAttribute.InvokeEffect2, StatAttributeType.Rate, "invoke_effect2"),
        ["invoke_effect3"] = new(StatAttribute.InvokeEffect3, StatAttributeType.Rate, "invoke_effect3"),
        ["pvpdamageincrease"] = new(StatAttribute.PvPDamage, StatAttributeType.Rate, "pvpdamageincrease"),
        ["pvpdamagereduce"] = new(StatAttribute.PvPDefense, StatAttributeType.Rate, "pvpdamagereduce"),
        ["improveguildexp"] = new(StatAttribute.GuildExp, StatAttributeType.Rate, "improveguildexp"),
        ["improveguildcoin"] = new(StatAttribute.GuildCoin, StatAttributeType.Rate, "improveguildcoin"),
        ["improvemassiveeventbexpball"] = new(StatAttribute.McKayXpOrb, StatAttributeType.Rate, "improvemassiveeventbexpball"),
        ["reduce_meso_trade_fee"] = new(StatAttribute.BlackMarketReduce, StatAttributeType.Rate, "reduce_meso_trade_fee"),
        ["reduce_enchant_matrial_fee"] = new(StatAttribute.EnchantCatalystDiscount, StatAttributeType.Rate, "reduce_enchant_matrial_fee"),
        ["reduce_merat_revival_fee"] = new(StatAttribute.MeretReviveFee, StatAttributeType.Rate, "reduce_merat_revival_fee"),
        ["improve_mining_reward_item"] = new(StatAttribute.MiningBonus, StatAttributeType.Rate, "improve_mining_reward_item"),
        ["improve_breeding_reward_item"] = new(StatAttribute.RanchingBonus, StatAttributeType.Rate, "improve_breeding_reward_item"),
        ["improve_blacksmithing_reward_mastery"] = new(StatAttribute.SmithingExp, StatAttributeType.Rate, "improve_blacksmithing_reward_mastery"),
        ["improve_engraving_reward_mastery"] = new(StatAttribute.HandicraftMastery, StatAttributeType.Rate, "improve_engraving_reward_mastery"),
        ["improve_gathering_reward_item"] = new(StatAttribute.ForagingBonus, StatAttributeType.Rate, "improve_gathering_reward_item"),
        ["improve_farming_reward_item"] = new(StatAttribute.FarmingBonus, StatAttributeType.Rate, "improve_farming_reward_item"),
        ["improve_alchemist_reward_mastery"] = new(StatAttribute.AlchemyMastery, StatAttributeType.Rate, "improve_alchemist_reward_mastery"),
        ["improve_cooking_reward_mastery"] = new(StatAttribute.CookingMastery, StatAttributeType.Rate, "improve_cooking_reward_mastery"),
        ["improve_acquire_gathering_exp"] = new(StatAttribute.ForagingExp, StatAttributeType.Rate, "improve_acquire_gathering_exp"),
        ["skill_levelup_tier_1"] = new(StatAttribute.TECH, StatAttributeType.Rate, "skill_levelup_tier_1"),
        ["skill_levelup_tier_2"] = new(StatAttribute.TECH_2, StatAttributeType.Rate, "skill_levelup_tier_2"),
        ["skill_levelup_tier_3"] = new(StatAttribute.TECH_10, StatAttributeType.Rate, "skill_levelup_tier_3"),
        ["skill_levelup_tier_4"] = new(StatAttribute.TECH_13, StatAttributeType.Rate, "skill_levelup_tier_4"),
        ["skill_levelup_tier_5"] = new(StatAttribute.TECH_16, StatAttributeType.Rate, "skill_levelup_tier_5"),
        ["skill_levelup_tier_6"] = new(StatAttribute.TECH_19, StatAttributeType.Rate, "skill_levelup_tier_6"),
        ["skill_levelup_tier_7"] = new(StatAttribute.TECH_22, StatAttributeType.Rate, "skill_levelup_tier_7"),
        ["skill_levelup_tier_8"] = new(StatAttribute.TECH_25, StatAttributeType.Rate, "skill_levelup_tier_8"),
        ["skill_levelup_tier_9"] = new(StatAttribute.TECH_28, StatAttributeType.Rate, "skill_levelup_tier_9"),
        ["skill_levelup_tier_10"] = new(StatAttribute.TECH_31, StatAttributeType.Rate, "skill_levelup_tier_10"),
        ["skill_levelup_tier_11"] = new(StatAttribute.TECH_34, StatAttributeType.Rate, "skill_levelup_tier_11"),
        ["skill_levelup_tier_12"] = new(StatAttribute.TECH_37, StatAttributeType.Rate, "skill_levelup_tier_12"),
        ["skill_levelup_tier_13"] = new(StatAttribute.TECH_40, StatAttributeType.Rate, "skill_levelup_tier_13"),
        ["skill_levelup_tier_14"] = new(StatAttribute.TECH_43, StatAttributeType.Rate, "skill_levelup_tier_14"),
        ["improve_massive_ox_exp"] = new(StatAttribute.OXQuizExp, StatAttributeType.Rate, "improve_massive_ox_exp"),
        ["improve_massive_trapmaster_exp"] = new(StatAttribute.TrapMasterExp, StatAttributeType.Rate, "improve_massive_trapmaster_exp"),
        ["improve_massive_finalsurvival_exp"] = new(StatAttribute.SoleSurvivorExp, StatAttributeType.Rate, "improve_massive_finalsurvival_exp"),
        ["improve_massive_crazyrunner_exp"] = new(StatAttribute.CrazyRunnerExp, StatAttributeType.Rate, "improve_massive_crazyrunner_exp"),
        ["improve_massive_sh_crazyrunner_exp"] = new(StatAttribute.ShanghaiCrazyRunnersExp, StatAttributeType.Rate, "improve_massive_sh_crazyrunner_exp"),
        ["improve_massive_escape_exp"] = new(StatAttribute.LudiEscapeExp, StatAttributeType.Rate, "improve_massive_escape_exp"),
        ["improve_massive_springbeach_exp"] = new(StatAttribute.SpringBeachExp, StatAttributeType.Rate, "improve_massive_springbeach_exp"),
        ["improve_massive_dancedance_exp"] = new(StatAttribute.DanceDanceExp, StatAttributeType.Rate, "improve_massive_dancedance_exp"),
        ["improve_massive_ox_msp"] = new(StatAttribute.OXMovementSpeed, StatAttributeType.Rate, "improve_massive_ox_msp"),
        ["improve_massive_trapmaster_msp"] = new(StatAttribute.TrapMasterMovementSpeed, StatAttributeType.Rate, "improve_massive_trapmaster_msp"),
        ["improve_massive_finalsurvival_msp"] = new(StatAttribute.SoleSurvivorMovementSpeed, StatAttributeType.Rate, "improve_massive_finalsurvival_msp"),
        ["improve_massive_crazyrunner_msp"] = new(StatAttribute.CrazyRunnerMovementSpeed, StatAttributeType.Rate, "improve_massive_crazyrunner_msp"),
        ["improve_massive_sh_crazyrunner_msp"] = new(StatAttribute.ShanghaiCrazyRunnersMovementSpeed, StatAttributeType.Rate, "improve_massive_sh_crazyrunner_msp"),
        ["improve_massive_escape_msp"] = new(StatAttribute.LudiEscapeMovementSpeed, StatAttributeType.Rate, "improve_massive_escape_msp"),
        ["improve_massive_springbeach_msp"] = new(StatAttribute.SpringBeachMovementSpeed, StatAttributeType.Rate, "improve_massive_springbeach_msp"),
        ["improve_massive_dancedance_msp"] = new(StatAttribute.DanceDanceStopMovementSpeed, StatAttributeType.Rate, "improve_massive_dancedance_msp"),
        ["npc_hit_reward_sp_ball"] = new(StatAttribute.GenerateSpiritOrbs, StatAttributeType.Rate, "npc_hit_reward_sp_ball"),
        ["npc_hit_reward_ep_ball"] = new(StatAttribute.GenerateStaminaOrbs, StatAttributeType.Rate, "npc_hit_reward_ep_ball"),
        ["improve_honor_token"] = new(StatAttribute.ValorTokens, StatAttributeType.Rate, "improve_honor_token"),
        ["improve_pvp_exp"] = new(StatAttribute.PvPExp, StatAttributeType.Rate, "improve_pvp_exp"),
        ["improve_darkstream_damage"] = new(StatAttribute.DarkDescentDamageBonus, StatAttributeType.Rate, "improve_darkstream_damage"),
        ["reduce_darkstream_recive_damage"] = new(StatAttribute.DarkDescentEvasion, StatAttributeType.Rate, "reduce_darkstream_recive_damage"),
        ["improve_darkstream_evp"] = new(StatAttribute.DarkDescentDamageReduce, StatAttributeType.Rate, "improve_darkstream_evp"),
        ["fishing_double_mastery"] = new(StatAttribute.DoubleFishingMastery, StatAttributeType.Rate, "fishing_double_mastery"),
        ["playinstrument_double_mastery"] = new(StatAttribute.DoublePerformanceMastery, StatAttributeType.Rate, "playinstrument_double_mastery"),
        ["complete_fieldmission_msp"] = new(StatAttribute.ExploredAreasMovementSpeed, StatAttributeType.Rate, "complete_fieldmission_msp"),
        ["improve_glide_vertical_velocity"] = new(StatAttribute.AirMountAscentSpeed, StatAttributeType.Rate, "improve_glide_vertical_velocity"),
        ["additionaleffect_95000018"] = new(StatAttribute.AdditionalEffect_95000018, StatAttributeType.Rate, "additionaleffect_95000018"),
        ["additionaleffect_95000012"] = new(StatAttribute.EnemyDefenseDecreaseOnHit, StatAttributeType.Rate, "additionaleffect_95000012"),
        ["additionaleffect_95000014"] = new(StatAttribute.EnemyAttackDecreaseOnHit, StatAttributeType.Rate, "additionaleffect_95000014"),
        ["additionaleffect_95000020"] = new(StatAttribute.IncreaseTotalDamageIf1NearbyEnemy, StatAttributeType.Rate, "additionaleffect_95000020"),
        ["additionaleffect_95000021"] = new(StatAttribute.IncreaseTotalDamageIf3NearbyEnemies, StatAttributeType.Rate, "additionaleffect_95000021"),
        ["additionaleffect_95000022"] = new(StatAttribute.IncreaseTotalDamageIf80Spirit, StatAttributeType.Rate, "additionaleffect_95000022"),
        ["additionaleffect_95000023"] = new(StatAttribute.IncreaseTotalDamageIfFullStamina, StatAttributeType.Rate, "additionaleffect_95000023"),
        ["additionaleffect_95000024"] = new(StatAttribute.IncreaseTotalDamageIfHerbEffectActive, StatAttributeType.Rate, "additionaleffect_95000024"),
        ["additionaleffect_95000025"] = new(StatAttribute.IncreaseTotalDamageToWorldBoss, StatAttributeType.Rate, "additionaleffect_95000025"),
        ["additionaleffect_95000026"] = new(StatAttribute.Effect95000026, StatAttributeType.Rate, "additionaleffect_95000026"),
        ["additionaleffect_95000027"] = new(StatAttribute.Effect95000027, StatAttributeType.Rate, "additionaleffect_95000027"),
        ["additionaleffect_95000028"] = new(StatAttribute.Effect95000028, StatAttributeType.Rate, "additionaleffect_95000028"),
        ["additionaleffect_95000029"] = new(StatAttribute.Effect95000029, StatAttributeType.Rate, "additionaleffect_95000029"),
        ["reduce_recovery_ep_inv"] = new(StatAttribute.StaminaRecoverySpeed, StatAttributeType.Rate, "reduce_recovery_ep_inv"),
        ["improve_stat_wap_u"] = new(StatAttribute.MaxWeaponAttack, StatAttributeType.Rate, "improve_stat_wap_u"),
        ["mining_double_reward"] = new(StatAttribute.DoubleMiningProduction, StatAttributeType.Rate, "mining_double_reward"),
        ["breeding_double_reward"] = new(StatAttribute.DoubleRanchingProduction, StatAttributeType.Rate, "breeding_double_reward"),
        ["gathering_double_reward"] = new(StatAttribute.DoubleForagingProduction, StatAttributeType.Rate, "gathering_double_reward"),
        ["farming_double_reward"] = new(StatAttribute.DoubleFarmingProduction, StatAttributeType.Rate, "farming_double_reward"),
        ["blacksmithing_double_reward"] = new(StatAttribute.DoubleSmithingProduction, StatAttributeType.Rate, "blacksmithing_double_reward"),
        ["engraving_double_reward"] = new(StatAttribute.DoubleHandicraftProduction, StatAttributeType.Rate, "engraving_double_reward"),
        ["alchemist_double_reward"] = new(StatAttribute.DoubleAlchemyProduction, StatAttributeType.Rate, "alchemist_double_reward"),
        ["cooking_double_reward"] = new(StatAttribute.DoubleCookingProduction, StatAttributeType.Rate, "cooking_double_reward"),
        ["mining_double_mastery"] = new(StatAttribute.DoubleMiningMastery, StatAttributeType.Rate, "mining_double_mastery"),
        ["breeding_double_mastery"] = new(StatAttribute.DoubleRanchingMastery, StatAttributeType.Rate, "breeding_double_mastery"),
        ["gathering_double_mastery"] = new(StatAttribute.DoubleForagingMastery, StatAttributeType.Rate, "gathering_double_mastery"),
        ["farming_double_mastery"] = new(StatAttribute.DoubleFarmingMastery, StatAttributeType.Rate, "farming_double_mastery"),
        ["blacksmithing_double_mastery"] = new(StatAttribute.DoubleSmithingMastery, StatAttributeType.Rate, "blacksmithing_double_mastery"),
        ["engraving_double_mastery"] = new(StatAttribute.DoubleHandicraftMastery, StatAttributeType.Rate, "engraving_double_mastery"),
        ["alchemist_double_mastery"] = new(StatAttribute.DoubleAlchemyMastery, StatAttributeType.Rate, "alchemist_double_mastery"),
        ["cooking_double_mastery"] = new(StatAttribute.DoubleCookingMastery, StatAttributeType.Rate, "cooking_double_mastery"),
        ["improve_chaosraid_wap"] = new(StatAttribute.ChaosRaidWeaponAttack, StatAttributeType.Rate, "improve_chaosraid_wap"),
        ["improve_chaosraid_asp"] = new(StatAttribute.ChaosRaidAttackSpeed, StatAttributeType.Rate, "improve_chaosraid_asp"),
        ["improve_chaosraid_atp"] = new(StatAttribute.ChaosRaidAccuracy, StatAttributeType.Rate, "improve_chaosraid_atp"),
        ["improve_chaosraid_hp"] = new(StatAttribute.ChaosRaidHealth, StatAttributeType.Rate, "improve_chaosraid_hp"),
        ["improve_recovery_ball"] = new(StatAttribute.StaminaAndSpiritFromOrbs, StatAttributeType.Rate, "improve_recovery_ball"),
        ["improve_fieldboss_kill_exp"] = new(StatAttribute.WorldBossExp, StatAttributeType.Rate, "improve_fieldboss_kill_exp"),
        ["improve_fieldboss_kill_drop"] = new(StatAttribute.WorldBossDropRate, StatAttributeType.Rate, "improve_fieldboss_kill_drop"),
        ["reduce_fieldboss_recive_damage"] = new(StatAttribute.WorldBossDamageReduce, StatAttributeType.Rate, "reduce_fieldboss_recive_damage"),
        ["additionaleffect_95000016"] = new(StatAttribute.Effect9500016, StatAttributeType.Rate, "additionaleffect_95000016"),
        ["improve_pettrap_reward"] = new(StatAttribute.PetCaptureRewards, StatAttributeType.Rate, "improve_pettrap_reward"),
        ["mining_multiaction"] = new(StatAttribute.MiningEfficency, StatAttributeType.Rate, "mining_multiaction"),
        ["breeding_multiaction"] = new(StatAttribute.RanchingEfficiency, StatAttributeType.Rate, "breeding_multiaction"),
        ["gathering_multiaction"] = new(StatAttribute.ForagingEfficiency, StatAttributeType.Rate, "gathering_multiaction"),
        ["farming_multiaction"] = new(StatAttribute.FarmingEfficiency, StatAttributeType.Rate, "farming_multiaction"),
        ["reduce_damage_by_targetmaxhp"] = new(StatAttribute.HealthBasedDamageReduce, StatAttributeType.Rate, "reduce_damage_by_targetmaxhp"),
        ["reduce_meso_revival_fee"] = new(StatAttribute.ReduceMesoRevivalFee, StatAttributeType.Rate, "reduce_meso_revival_fee"),
        ["improve_riding_run_speed"] = new(StatAttribute.ImproveRidingRunSpeed, StatAttributeType.Rate, "improve_riding_run_speed"),
        ["improve_dungeon_reward_meso"] = new(StatAttribute.ImproveDungeonRewardMeso, StatAttributeType.Rate, "improve_dungeon_reward_meso"),
        ["improve_shop_buying_meso"] = new(StatAttribute.ImproveShopBuyingMeso, StatAttributeType.Rate, "improve_shop_buying_meso"),
        ["improve_itembox_reward_meso"] = new(StatAttribute.ImproveItemboxRewardMeso, StatAttributeType.Rate, "improve_itembox_reward_meso"),
        ["reduce_remakeoption_fee"] = new(StatAttribute.ReduceRemakeOptionRee, StatAttributeType.Rate, "reduce_remakeoption_fee"),
        ["reduce_airtaxi_fee"] = new(StatAttribute.ReduceAirTaxiFee, StatAttributeType.Rate, "reduce_airtaxi_fee"),
        ["improve_socket_unlock_probability"] = new(StatAttribute.ImproveSocketUnlockProbability, StatAttributeType.Rate, "improve_socket_unlock_probability"),
        ["reduce_gemstone_upgrade_fee"] = new(StatAttribute.ReduceGemstoneUpgradeFee, StatAttributeType.Rate, "reduce_gemstone_upgrade_fee"),
        ["reduce_pet_remakeoption_fee"] = new(StatAttribute.ReducePetRemakeOptionFee, StatAttributeType.Rate, "reduce_pet_remakeoption_fee"),
        ["improve_riding_speed"] = new(StatAttribute.ImproveRidingSpeed, StatAttributeType.Rate, "improve_riding_speed"),
        ["improve_survival_kill_exp"] = new(StatAttribute.ImproveSurvivalKill_exp, StatAttributeType.Rate, "improve_survival_kill_exp"),
        ["improve_survival_time_exp"] = new(StatAttribute.ImproveSurvivalTime_exp, StatAttributeType.Rate, "improve_survival_time_exp"),
        ["offensive_physicaldamage"] = new(StatAttribute.OffensivePhysicalDamage, StatAttributeType.Rate, "offensive_physicaldamage"),
        ["offensive_magicaldamage"] = new(StatAttribute.OffensiveMagicalDamage, StatAttributeType.Rate, "offensive_magicaldamage"),
        ["reduce_gameitem_socket_unlock_fee"] = new(StatAttribute.ReduceGameitemSocketUnlockFee, StatAttributeType.Rate, "reduce_gameitem_socket_unlock_fee"),
    };

    public static void AddStat(Dictionary<StatAttribute, EffectStatMetadata> stats, XmlAttribute attribute, StatEntry entry, bool isValue)
    {
        StatAttribute stat = entry.Attribute;

        if (entry.Name == "sgi")
        {
            stat = StatAttribute.EliteDamage;
        }

        if (entry.Name == "sgi_boss")
        {
            stat = StatAttribute.BossDamage;
        }

        if (entry.AttributeType == StatAttributeType.Flat)
        {
            long flat = 0;
            float rate = 0;

            if (isValue)
            {
                flat = long.Parse(attribute.Value);
            }
            else
            {
                rate = float.Parse(attribute.Value);
            }

            AddStatFlat(stats, stat, flat, rate);
        }

        if (entry.AttributeType == StatAttributeType.Rate)
        {
            float value = float.Parse(attribute.Value);

            AddStatRate(stats, stat, isValue ? value : 0, isValue ? 0 : value);
        }
    }

    public static void AddStatRate(Dictionary<StatAttribute, EffectStatMetadata> stats, StatAttribute stat, float flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += (long) (flat * 1000);
            currentValue.Rate += rate;

            return;
        }

        stats.Add(stat, new()
        {
            Flat = (long) (flat * 1000),
            Rate = rate,
            AttributeType = StatAttributeType.Rate
        });
    }

    public static void AddStatFlat(Dictionary<StatAttribute, EffectStatMetadata> stats, StatAttribute stat, long flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += flat;
            currentValue.Rate += rate;

            return;
        }

        stats.Add(stat, new()
        {
            Flat = flat,
            Rate = rate,
            AttributeType = StatAttributeType.Flat
        });
    }
}
