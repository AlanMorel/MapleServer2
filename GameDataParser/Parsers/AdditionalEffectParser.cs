using GameDataParser.Files;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.AdditionalEffect;
using Maple2.File.Parser.Xml.Skill;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class AdditionalEffectParser : Exporter<List<AdditionalEffectMetadata>>
{
    public AdditionalEffectParser(MetadataResources resources) : base(resources, MetadataName.AdditionalEffect) { }

    protected override List<AdditionalEffectMetadata> Parse()
    {
        List<AdditionalEffectMetadata> effects = new();

        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.AdditionalEffectParser parser = new(Resources.XmlReader);
        foreach ((int id, IList<AdditionalEffectData> data) in parser.Parse())
        {
            AdditionalEffectMetadata metadata = new()
            {
                Id = id,
                Levels = new()
            };

            for (int i = 0; i < data.Count; ++i)
            {
                int levelIndex = data[i].BasicProperty.level;

                AdditionalEffectLevelMetadata level = new()
                {
                    BeginCondition = ParseBeginCondition(data[i].beginCondition),
                    Basic = new()
                    {
                        MaxBuffCount = data[i].BasicProperty.maxBuffCount,
                        SkillGroupType = data[i].BasicProperty.skillGroupType,
                        GroupIds = data[i].BasicProperty.groupIDs,
                        DotCondition = data[i].BasicProperty.dotCondition,
                        DurationTick = data[i].BasicProperty.durationTick,
                        KeepCondition = data[i].BasicProperty.keepCondition,
                        IntervalTick = data[i].BasicProperty.intervalTick,
                        BuffType = (BuffType) data[i].BasicProperty.buffType,
                        BuffSubType = (BuffSubType) data[i].BasicProperty.buffSubType,
                        CooldownTime = float.Parse(data[i].BasicProperty.coolDownTime ?? "0"),
                        DelayTick = data[i].BasicProperty.delayTick
                    },
                    Status = new()
                    {
                        Stats = new()
                    }
                };

                Stat stat = data[i].StatusProperty.Stat;

                if (stat != null)
                {
                    AddStat(level.Status, StatAttribute.Str, stat.strvalue, stat.strrate);
                    AddStat(level.Status, StatAttribute.Int, stat.intvalue, stat.intrate);
                    AddStat(level.Status, StatAttribute.Luk, stat.lukvalue, stat.lukrate);
                    AddStat(level.Status, StatAttribute.Dex, stat.dexvalue, stat.dexrate);
                    AddStat(level.Status, StatAttribute.Hp, stat.hpvalue, stat.hprate);
                    AddStat(level.Status, StatAttribute.HpRegen, stat.hp_rgpvalue, stat.hp_rgprate);               // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.HpRegenInterval, stat.hp_invvalue, stat.hp_invrate);       // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Spirit, stat.spvalue, stat.sprate);                    // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegen, stat.sp_rgpvalue, stat.sp_rgprate);               // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.SpRegenInterval, stat.sp_invvalue, stat.sp_invrate);       // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.Stamina, stat.epvalue, stat.eprate);                   // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegen, stat.ep_rgpvalue, stat.ep_rgprate);          // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.StaminaRegenInterval, stat.ep_invvalue, stat.ep_invrate);  // these might need to be looked at in closer detail
                    AddStat(level.Status, StatAttribute.AttackSpeed, stat.aspvalue, stat.asprate);
                    AddStat(level.Status, StatAttribute.MovementSpeed, stat.mspvalue, stat.msprate);
                    AddStat(level.Status, StatAttribute.Accuracy, stat.atpvalue, stat.atprate);
                    AddStat(level.Status, StatAttribute.Evasion, stat.evpvalue, stat.evprate);
                    AddStat(level.Status, StatAttribute.CritRate, stat.capvalue, stat.caprate);
                    AddStat(level.Status, StatAttribute.CritDamage, stat.cadvalue, stat.cadrate);
                    AddStat(level.Status, StatAttribute.CritEvasion, stat.carvalue, stat.carrate);
                    AddStat(level.Status, StatAttribute.BonusAtk, stat.bapvalue, stat.baprate);
                    AddStat(level.Status, StatAttribute.MountMovementSpeed, stat.rmspvalue, stat.rmsprate);
                    AddStat(level.Status, StatAttribute.Pierce, stat.penvalue, stat.penrate);
                    AddStat(level.Status, StatAttribute.Damage, stat.dmgvalue, stat.dmgrate);
                    AddStat(level.Status, StatAttribute.MinWeaponAtk, stat.wapminvalue, stat.wapminrate);
                    AddStat(level.Status, StatAttribute.MaxWeaponAtk, stat.wapmaxvalue, stat.wapmaxrate);
                    AddStat(level.Status, StatAttribute.MagicRes, stat.marvalue, stat.marrate);  // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.MagicAtk, stat.mapvalue, stat.maprate);
                    AddStat(level.Status, StatAttribute.PhysicalRes, stat.parvalue, stat.parrate);  // revisit these to make sure they're right
                    AddStat(level.Status, StatAttribute.PhysicalAtk, stat.papvalue, stat.paprate);
                    AddStat(level.Status, StatAttribute.JumpHeight, stat.jmpvalue, stat.jmprate);
                    AddStat(level.Status, StatAttribute.PerfectGuard, stat.abpvalue, stat.abprate);
                    AddStat(level.Status, StatAttribute.PetBonusAtk, stat.bap_petvalue, stat.bap_petrate);
                }

                SpecialAbility special = data[i].StatusProperty.SpecialAbility;

                if (special != null)
                {
                    // TODO: finish adding all these stats to the correct attrib type
                    AddStat(level.Status, StatAttribute.ExpBonus, special.segvalue, special.segrate); // gm emote exp boost for monster hunting, fishing, performing?
                    AddStat(level.Status, StatAttribute.MesoBonus, special.smdvalue, special.smdrate); // smh. gm emote meso boost?
                    AddStat(level.Status, StatAttribute.SwimSpeed, special.sssvalue, special.sssrate); // swim speed
                    AddStat(level.Status, StatAttribute.DashDistance, special.dashdistancevalue, special.dashdistancerate); // 
                    AddStat(level.Status, StatAttribute.TonicDropRate, special.spdvalue, special.spdrate); // 
                    AddStat(level.Status, StatAttribute.GearDropRate, special.sidvalue, special.sidrate); // 
                    AddStat(level.Status, StatAttribute.TotalDamage, special.finaladditionaldamagevalue, special.finaladditionaldamagerate); // 
                    AddStat(level.Status, StatAttribute.CriticalDamage, special.crivalue, special.crirate); // 
                    AddStat(level.Status, StatAttribute.Damage, special.sgivalue, special.sgirate); //
                    AddStat(level.Status, StatAttribute.LeaderDamage, special.sgi_leadervalue, special.sgi_leaderrate); // 
                    AddStat(level.Status, StatAttribute.EliteDamage, special.sgi_elitevalue, special.sgi_eliterate); // 
                    AddStat(level.Status, StatAttribute.BossDamage, special.sgi_bossvalue, special.sgi_bossrate); // 
                    AddStat(level.Status, StatAttribute.HpOnKill, special.killhprestorevalue, special.killhprestorerate); // 
                    AddStat(level.Status, StatAttribute.SpiritOnKill, special.killsprestorevalue, special.killsprestorerate); //  
                    AddStat(level.Status, StatAttribute.StaminaOnKill, special.killeprestorevalue, special.killeprestorerate); // 
                    AddStat(level.Status, StatAttribute.Heal, special.healvalue, special.healrate); // 
                    AddStat(level.Status, StatAttribute.AllyRecovery, special.receivedhealincreasevalue, special.receivedhealincreaserate); // 
                    AddStat(level.Status, StatAttribute.IceDamage, special.icedamagevalue, special.icedamagerate); // 
                    AddStat(level.Status, StatAttribute.FireDamage, special.firedamagevalue, special.firedamagerate); // 
                    AddStat(level.Status, StatAttribute.DarkDamage, special.darkdamagevalue, special.darkdamagerate); //
                    AddStat(level.Status, StatAttribute.HolyDamage, special.lightdamagevalue, special.lightdamagerate); //  
                    AddStat(level.Status, StatAttribute.PoisonDamage, special.poisondamagevalue, special.poisondamagerate); // 
                    AddStat(level.Status, StatAttribute.ElectricDamage, special.thunderdamagevalue, special.thunderdamagerate); // 
                    AddStat(level.Status, StatAttribute.MeleeDamage, special.nddincreasevalue, special.nddincreaserate); // 
                    AddStat(level.Status, StatAttribute.RangedDamage, special.lddincreasevalue, special.lddincreaserate); // 
                    AddStat(level.Status, StatAttribute.PhysicalPiercing, special.parpenvalue, special.parpenrate); //  
                    AddStat(level.Status, StatAttribute.MagicPiercing, special.marpenvalue, special.marpenrate); //  
                    AddStat(level.Status, StatAttribute.IceDamageReduce, special.icedamagereducevalue, special.icedamagereducerate); //  
                    AddStat(level.Status, StatAttribute.FireDamageReduce, special.firedamagereducevalue, special.firedamagereducerate); //  
                    AddStat(level.Status, StatAttribute.DarkDamageReduce, special.darkdamagereducevalue, special.darkdamagereducerate); //  
                    AddStat(level.Status, StatAttribute.HolyDamageReduce, special.lightdamagereducevalue, special.lightdamagereducerate); // 
                    AddStat(level.Status, StatAttribute.PoisonDamageReduce, special.poisondamagereducevalue, special.poisondamagereducerate); // 
                    AddStat(level.Status, StatAttribute.ElectricDamageReduce, special.thunderdamagereducevalue, special.thunderdamagereducerate); // 
                    AddStat(level.Status, StatAttribute.StunReduce, special.stunreducevalue, special.stunreducerate); // 
                    AddStat(level.Status, StatAttribute.DebuffDurationReduce, special.conditionreducevalue, special.conditionreducerate); // 
                    AddStat(level.Status, StatAttribute.CooldownReduce, special.skillcooldownvalue, special.skillcooldownrate); // 
                    AddStat(level.Status, StatAttribute.MeleeDamageReduce, special.neardistancedamagereducevalue, special.neardistancedamagereducerate); //
                    AddStat(level.Status, StatAttribute.RangedDamageReduce, special.longdistancedamagereducevalue, special.longdistancedamagereducerate); //  
                    AddStat(level.Status, StatAttribute.KnockbackReduce, special.knockbackreducevalue, special.knockbackreducerate); // 
                    AddStat(level.Status, StatAttribute.MeleeStun, special.stunprocnddvalue, special.stunprocnddrate); // 
                    AddStat(level.Status, StatAttribute.RangedStun, special.stunproclddvalue, special.stunproclddrate); // 
                    AddStat(level.Status, StatAttribute.MeeleeKnockback, special.knockbackprocnddvalue, special.knockbackprocnddrate); // 
                    AddStat(level.Status, StatAttribute.RangedKnockback, special.knockbackproclddvalue, special.knockbackproclddrate); // 
                    AddStat(level.Status, StatAttribute.MeleeImmob, special.snareprocnddvalue, special.snareprocnddrate); // 
                    AddStat(level.Status, StatAttribute.RangedImmob, special.snareproclddvalue, special.snareproclddrate); // 
                    AddStat(level.Status, StatAttribute.MeleeAoeDamage, special.aoeprocnddvalue, special.aoeprocnddrate); // 
                    AddStat(level.Status, StatAttribute.RangedAoeDamage, special.aoeproclddvalue, special.aoeproclddrate); //
                    AddStat(level.Status, StatAttribute.DropRate, special.npckilldropitemincratevalue, special.npckilldropitemincraterate); // 
                    AddStat(level.Status, StatAttribute.QuestExp, special.seg_questrewardvalue, special.seg_questrewardrate); // 
                    AddStat(level.Status, StatAttribute.QuestMeso, special.smd_questrewardvalue, special.smd_questrewardrate); // 
                    AddStat(level.Status, StatAttribute.FishingExp, special.seg_fishingrewardvalue, special.seg_fishingrewardrate); // 
                    AddStat(level.Status, StatAttribute.ArcadeExp, special.seg_arcaderewardvalue, special.seg_arcaderewardrate); // 
                    AddStat(level.Status, StatAttribute.PerformanceExp, special.seg_playinstrumentrewardvalue, special.seg_playinstrumentrewardrate); // 
                    AddStat(level.Status, StatAttribute.InvokeEffect1, special.invoke_effect1value, special.invoke_effect1rate); //  
                    AddStat(level.Status, StatAttribute.InvokeEffect2, special.invoke_effect2value, special.invoke_effect2rate); //
                    AddStat(level.Status, StatAttribute.InvokeEffect3, special.invoke_effect3value, special.invoke_effect3rate); // 
                    AddStat(level.Status, StatAttribute.PvPDamage, special.pvpdamageincreasevalue, special.pvpdamageincreaserate); // 
                    AddStat(level.Status, StatAttribute.PvPDefense, special.pvpdamagereducevalue, special.pvpdamagereducerate); // 
                    AddStat(level.Status, StatAttribute.GuildExp, special.improveguildexpvalue, special.improveguildexprate); // 
                    AddStat(level.Status, StatAttribute.GuildCoin, special.improveguildcoinvalue, special.improveguildcoinrate); // 
                    AddStat(level.Status, StatAttribute.McKayXpOrb, special.improvemassiveeventbexpballvalue, special.improvemassiveeventbexpballrate); // 
                    AddStat(level.Status, StatAttribute.BlackMarketReduce, special.reduce_meso_trade_feevalue, special.reduce_meso_trade_feerate); // 
                    AddStat(level.Status, StatAttribute.EnchantCatalystDiscount, special.reduce_enchant_matrial_feevalue, special.reduce_enchant_matrial_feerate); // 
                    AddStat(level.Status, StatAttribute.MeretReviveFee, special.reduce_merat_revival_feevalue, special.reduce_merat_revival_feerate); // 
                    AddStat(level.Status, StatAttribute.MiningBonus, special.improve_mining_reward_itemvalue, special.improve_mining_reward_itemrate); // 
                    AddStat(level.Status, StatAttribute.RanchingBonus, special.improve_breeding_reward_itemvalue, special.improve_breeding_reward_itemrate); // 
                    AddStat(level.Status, StatAttribute.SmithingExp, special.improve_blacksmithing_reward_masteryvalue, special.improve_blacksmithing_reward_masteryrate); // 
                    AddStat(level.Status, StatAttribute.HandicraftMastery, special.improve_engraving_reward_masteryvalue, special.improve_engraving_reward_masteryrate); //  
                    AddStat(level.Status, StatAttribute.ForagingBonus, special.improve_gathering_reward_itemvalue, special.improve_gathering_reward_itemrate); // 
                    AddStat(level.Status, StatAttribute.FarmingBonus, special.improve_farming_reward_itemvalue, special.improve_farming_reward_itemrate); //  
                    AddStat(level.Status, StatAttribute.AlchemyMastery, special.improve_alchemist_reward_masteryvalue, special.improve_alchemist_reward_masteryrate); // 
                    AddStat(level.Status, StatAttribute.CookingMastery, special.improve_cooking_reward_masteryvalue, special.improve_cooking_reward_masteryrate); // 
                    AddStat(level.Status, StatAttribute.ForagingExp, special.improve_acquire_gathering_expvalue, special.improve_acquire_gathering_exprate); //
                    AddStat(level.Status, StatAttribute.TECH, special.skill_levelup_tier_1value, special.skill_levelup_tier_1rate); // 
                    AddStat(level.Status, StatAttribute.TECH_2, special.skill_levelup_tier_2value, special.skill_levelup_tier_2rate); // 
                    AddStat(level.Status, StatAttribute.TECH_10, special.skill_levelup_tier_3value, special.skill_levelup_tier_3rate); // 
                    AddStat(level.Status, StatAttribute.TECH_13, special.skill_levelup_tier_4value, special.skill_levelup_tier_4rate); // 
                    AddStat(level.Status, StatAttribute.TECH_16, special.skill_levelup_tier_5value, special.skill_levelup_tier_5rate); // 
                    AddStat(level.Status, StatAttribute.TECH_19, special.skill_levelup_tier_6value, special.skill_levelup_tier_6rate); // 
                    AddStat(level.Status, StatAttribute.TECH_22, special.skill_levelup_tier_7value, special.skill_levelup_tier_7rate); // 
                    AddStat(level.Status, StatAttribute.TECH_25, special.skill_levelup_tier_8value, special.skill_levelup_tier_8rate); // 
                    AddStat(level.Status, StatAttribute.TECH_28, special.skill_levelup_tier_9value, special.skill_levelup_tier_9rate); // 
                    AddStat(level.Status, StatAttribute.TECH_31, special.skill_levelup_tier_10value, special.skill_levelup_tier_10rate); // 
                    AddStat(level.Status, StatAttribute.TECH_34, special.skill_levelup_tier_11value, special.skill_levelup_tier_11rate); // 
                    AddStat(level.Status, StatAttribute.TECH_37, special.skill_levelup_tier_12value, special.skill_levelup_tier_12rate); // 
                    AddStat(level.Status, StatAttribute.TECH_40, special.skill_levelup_tier_13value, special.skill_levelup_tier_13rate); // 
                    AddStat(level.Status, StatAttribute.TECH_43, special.skill_levelup_tier_14value, special.skill_levelup_tier_14rate); // 
                    AddStat(level.Status, StatAttribute.OXQuizExp, special.improve_massive_ox_expvalue, special.improve_massive_ox_exprate); // 
                    AddStat(level.Status, StatAttribute.TrapMasterExp, special.improve_massive_trapmaster_expvalue, special.improve_massive_trapmaster_exprate); // 
                    AddStat(level.Status, StatAttribute.SoleSurvivorExp, special.improve_massive_finalsurvival_expvalue, special.improve_massive_finalsurvival_exprate); //
                    AddStat(level.Status, StatAttribute.CrazyRunnerExp, special.improve_massive_crazyrunner_expvalue, special.improve_massive_crazyrunner_exprate); //
                    AddStat(level.Status, StatAttribute.ShanghaiCrazyRunnersExp, special.improve_massive_sh_crazyrunner_expvalue, special.improve_massive_sh_crazyrunner_exprate); //
                    AddStat(level.Status, StatAttribute.LudiEscapeExp, special.improve_massive_escape_expvalue, special.improve_massive_escape_exprate); //
                    AddStat(level.Status, StatAttribute.SpringBeachExp, special.improve_massive_springbeach_expvalue, special.improve_massive_springbeach_exprate); //
                    AddStat(level.Status, StatAttribute.DanceDanceExp, special.improve_massive_dancedance_expvalue, special.improve_massive_dancedance_exprate); //
                    AddStat(level.Status, StatAttribute.OXMovementSpeed, special.improve_massive_ox_mspvalue, special.improve_massive_ox_msprate); //
                    AddStat(level.Status, StatAttribute.TrapMasterMovementSpeed, special.improve_massive_trapmaster_mspvalue, special.improve_massive_trapmaster_msprate); // 
                    AddStat(level.Status, StatAttribute.SoleSurvivorMovementSpeed, special.improve_massive_finalsurvival_mspvalue, special.improve_massive_finalsurvival_msprate); //
                    AddStat(level.Status, StatAttribute.CrazyRunnerMovementSpeed, special.improve_massive_crazyrunner_mspvalue, special.improve_massive_crazyrunner_msprate); //
                    AddStat(level.Status, StatAttribute.ShanghaiCrazyRunnersMovementSpeed, special.improve_massive_sh_crazyrunner_mspvalue, special.improve_massive_sh_crazyrunner_msprate); //
                    AddStat(level.Status, StatAttribute.LudiEscapeMovementSpeed, special.improve_massive_escape_mspvalue, special.improve_massive_escape_msprate); //
                    AddStat(level.Status, StatAttribute.SpringBeachMovementSpeed, special.improve_massive_springbeach_mspvalue, special.improve_massive_springbeach_msprate); //
                    AddStat(level.Status, StatAttribute.DanceDanceStopMovementSpeed, special.improve_massive_dancedance_mspvalue, special.improve_massive_dancedance_msprate); //
                    AddStat(level.Status, StatAttribute.GenerateSpiritOrbs, special.npc_hit_reward_sp_ballvalue, special.npc_hit_reward_sp_ballrate); //
                    AddStat(level.Status, StatAttribute.GenerateStaminaOrbs, special.npc_hit_reward_ep_ballvalue, special.npc_hit_reward_ep_ballrate); //
                    AddStat(level.Status, StatAttribute.ValorTokens, special.improve_honor_tokenvalue, special.improve_honor_tokenrate); //
                    AddStat(level.Status, StatAttribute.PvPExp, special.improve_pvp_expvalue, special.improve_pvp_exprate); //
                    AddStat(level.Status, StatAttribute.DarkDescentDamageBonus, special.improve_darkstream_damagevalue, special.improve_darkstream_damagerate); //
                    AddStat(level.Status, StatAttribute.DarkDescentDamageReduce, special.improve_darkstream_evpvalue, special.improve_darkstream_evprate); //
                    AddStat(level.Status, StatAttribute.DarkDescentEvasion, special.reduce_darkstream_recive_damagevalue, special.reduce_darkstream_recive_damagerate); //
                    AddStat(level.Status, StatAttribute.DoubleFishingMastery, special.fishing_double_masteryvalue, special.fishing_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoublePerformanceMastery, special.playinstrument_double_masteryvalue, special.playinstrument_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.ExploredAreasMovementSpeed, special.complete_fieldmission_mspvalue, special.complete_fieldmission_msprate); //
                    AddStat(level.Status, StatAttribute.AirMountAscentSpeed, special.improve_glide_vertical_velocityvalue, special.improve_glide_vertical_velocityrate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.additionaleffect_95000018value, special.additionaleffect_95000018rate); //
                    AddStat(level.Status, StatAttribute.EnemyDefenseDecreaseOnHit, special.additionaleffect_95000012value, special.additionaleffect_95000012rate); //
                    AddStat(level.Status, StatAttribute.EnemyAttackDecreaseOnHit, special.additionaleffect_95000014value, special.additionaleffect_95000014rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageIf1NearbyEnemy, special.additionaleffect_95000020value, special.additionaleffect_95000020rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageIf3NearbyEnemies, special.additionaleffect_95000021value, special.additionaleffect_95000021rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageIf80Spirit, special.additionaleffect_95000022value, special.additionaleffect_95000022rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageIfFullStamina, special.additionaleffect_95000023value, special.additionaleffect_95000023rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageIfHerbEffectActive, special.additionaleffect_95000024value, special.additionaleffect_95000024rate); //
                    AddStat(level.Status, StatAttribute.IncreaseTotalDamageToWorldBoss, special.additionaleffect_95000025value, special.additionaleffect_95000025rate); //
                    AddStat(level.Status, StatAttribute.Effect95000026, special.additionaleffect_95000026value, special.additionaleffect_95000026rate); //
                    AddStat(level.Status, StatAttribute.Effect95000027, special.additionaleffect_95000027value, special.additionaleffect_95000027rate); //
                    AddStat(level.Status, StatAttribute.Effect95000028, special.additionaleffect_95000028value, special.additionaleffect_95000028rate); //
                    AddStat(level.Status, StatAttribute.Effect95000029, special.additionaleffect_95000029value, special.additionaleffect_95000029rate); //
                    AddStat(level.Status, StatAttribute.StaminaRecoverySpeed, special.reduce_recovery_ep_invvalue, special.reduce_recovery_ep_invrate);
                    AddStat(level.Status, StatAttribute.MaxWeaponAttack, special.improve_stat_wap_uvalue, special.improve_stat_wap_urate); //
                    AddStat(level.Status, StatAttribute.DoubleMiningProduction, special.mining_double_rewardvalue, special.mining_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleRanchingProduction, special.breeding_double_rewardvalue, special.breeding_double_rewardrate);
                    AddStat(level.Status, StatAttribute.DoubleForagingProduction, special.gathering_double_rewardvalue, special.gathering_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleFarmingProduction, special.farming_double_rewardvalue, special.farming_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleSmithingProduction, special.blacksmithing_double_rewardvalue, special.blacksmithing_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleHandicraftProduction, special.engraving_double_rewardvalue, special.engraving_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleAlchemyProduction, special.alchemist_double_rewardvalue, special.alchemist_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleCookingProduction, special.cooking_double_rewardvalue, special.cooking_double_rewardrate); //
                    AddStat(level.Status, StatAttribute.DoubleMiningMastery, special.mining_double_masteryvalue, special.mining_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleRanchingMastery, special.breeding_double_masteryvalue, special.breeding_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleForagingMastery, special.gathering_double_masteryvalue, special.gathering_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleFarmingMastery, special.farming_double_masteryvalue, special.farming_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleSmithingMastery, special.blacksmithing_double_masteryvalue, special.blacksmithing_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleHandicraftMastery, special.engraving_double_masteryvalue, special.engraving_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleAlchemyMastery, special.alchemist_double_masteryvalue, special.alchemist_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.DoubleCookingMastery, special.cooking_double_masteryvalue, special.cooking_double_masteryrate); //
                    AddStat(level.Status, StatAttribute.ChaosRaidWeaponAttack, special.improve_chaosraid_wapvalue, special.improve_chaosraid_waprate); //
                    AddStat(level.Status, StatAttribute.ChaosRaidAttackSpeed, special.improve_chaosraid_aspvalue, special.improve_chaosraid_asprate); //
                    AddStat(level.Status, StatAttribute.ChaosRaidAccuracy, special.improve_chaosraid_atpvalue, special.improve_chaosraid_atprate); //
                    AddStat(level.Status, StatAttribute.ChaosRaidHealth, special.improve_chaosraid_hpvalue, special.improve_chaosraid_hprate);
                    AddStat(level.Status, StatAttribute.StaminaAndSpiritFromOrbs, special.improve_recovery_ballvalue, special.improve_recovery_ballrate); //
                    AddStat(level.Status, StatAttribute.WorldBossExp, special.improve_fieldboss_kill_expvalue, special.improve_fieldboss_kill_exprate); //
                    AddStat(level.Status, StatAttribute.WorldBossDropRate, special.improve_fieldboss_kill_dropvalue, special.improve_fieldboss_kill_droprate); //
                    AddStat(level.Status, StatAttribute.WorldBossDamageReduce, special.reduce_fieldboss_recive_damagevalue, special.reduce_fieldboss_recive_damagerate); //
                    AddStat(level.Status, StatAttribute.Effect9500016, special.additionaleffect_95000016value, special.additionaleffect_95000016rate); //
                    AddStat(level.Status, StatAttribute.PetCaptureRewards, special.improve_pettrap_rewardvalue, special.improve_pettrap_rewardrate); //
                    AddStat(level.Status, StatAttribute.MiningEfficency, special.mining_multiactionvalue, special.mining_multiactionrate); //
                    AddStat(level.Status, StatAttribute.RanchingEfficiency, special.breeding_multiactionvalue, special.breeding_multiactionrate); //
                    AddStat(level.Status, StatAttribute.ForagingEfficiency, special.gathering_multiactionvalue, special.gathering_multiactionrate); //
                    AddStat(level.Status, StatAttribute.FarmingEfficiency, special.farming_multiactionvalue, special.farming_multiactionrate); //
                    AddStat(level.Status, StatAttribute.HealthBasedDamageReduce, special.reduce_damage_by_targetmaxhpvalue, special.reduce_damage_by_targetmaxhprate); //
                    // TODO: add enum values for things after here. enum ended here so i stopped adding
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_meso_revival_feevalue, special.reduce_meso_revival_feerate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_riding_run_speedvalue, special.improve_riding_run_speedrate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_dungeon_reward_mesovalue, special.improve_dungeon_reward_mesorate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_shop_buying_mesovalue, special.improve_shop_buying_mesorate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_itembox_reward_mesovalue, special.improve_itembox_reward_mesorate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_remakeoption_feevalue, special.reduce_remakeoption_feerate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_airtaxi_feevalue, special.reduce_airtaxi_feerate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_socket_unlock_probabilityvalue, special.improve_socket_unlock_probabilityrate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_gemstone_upgrade_feevalue, special.reduce_gemstone_upgrade_feerate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_pet_remakeoption_feevalue, special.reduce_pet_remakeoption_feerate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_riding_speedvalue, special.improve_riding_speedrate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_survival_kill_expvalue, special.improve_survival_kill_exprate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.improve_survival_time_expvalue, special.improve_survival_time_exprate); //
                    AddStat(level.Status, StatAttribute.PhysicalAtk, special.offensive_physicaldamagevalue, special.offensive_physicaldamagerate); // revisit these
                    AddStat(level.Status, StatAttribute.MagicAtk, special.offensive_magicaldamagevalue, special.offensive_magicaldamagerate); // revisit these
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_gameitem_socket_unlock_feevalue, special.reduce_gameitem_socket_unlock_feerate); //
                }

                OffensiveProperty offensive = data[i].OffensiveProperty;

                if (offensive != null)
                {
                    level.Offensive = new()
                    {
                        AlwaysCrit = offensive.attackSuccessCritical != 0,
                        ImmuneBreak = offensive.hitImmuneBreak
                    };

                    AddStat(level.Status, StatAttribute.PhysicalAtk, offensive.papDamageV, offensive.papDamageR);
                    AddStat(level.Status, StatAttribute.MagicAtk, offensive.mapDamageV, offensive.mapDamageR);
                }

                DefensiveProperty defensive = data[i].DefensiveProperty;

                if (defensive != null)
                {
                    level.Defesive = new()
                    {
                        Invincible = defensive.invincible != 0
                    };

                    AddStat(level.Status, StatAttribute.PhysicalRes, defensive.papDamageV, defensive.papDamageR);
                    AddStat(level.Status, StatAttribute.MagicRes, defensive.mapDamageV, defensive.mapDamageR);
                }

                RecoveryProperty recovery = data[i].RecoveryProperty;

                if (recovery != null)
                {

                }

                CancelEffectProperty cancel = data[i].CancelEffectProperty;

                if (cancel != null)
                {
                    level.CancelEffect = new()
                    {
                        CancelCheckSameCaster = cancel.cancelCheckSameCaster,
                        CancelPassiveEffect = cancel.cancelPassiveEffect,
                        CancelEffectCodes = cancel.cancelEffectCodes,
                        CancelBuffCategories = cancel.cancelBuffCategories
                    };
                }

                ImmuneEffectProperty immune = data[i].ImmuneEffectProperty;

                if (immune != null)
                {
                    level.ImmuneEffect = new()
                    {
                        ImmuneEffectCodes = immune.immuneEffectCodes,
                        ImmuneBuffCategories = immune.immuneBuffCategories
                    };
                }

                List<TriggerSkill> splashSkills = data[i].splashSkill;

                if (splashSkills != null)
                {
                    level.SplashSkill = new();

                    for (int skillIndex = 0; skillIndex < splashSkills.Count; skillIndex++)
                    {
                        TriggerSkill splashSkill = data[i].splashSkill[skillIndex];

                        level.SplashSkill.Add(new(splashSkill.skillID, GetSkillLevels(splashSkill.level), splashSkill.splash, (byte) splashSkill.skillTarget, (byte) splashSkill.skillOwner, (short) splashSkill.fireCount, splashSkill.interval, splashSkill.immediateActive)
                        {
                            Delay = splashSkill.delay,
                            RemoveDelay = splashSkill.removeDelay,
                            UseDirection = splashSkill.useDirection,
                            BeginCondition = ParseBeginCondition(splashSkill.beginCondition)
                        });
                    }
                }

                List<TriggerSkill> conditionSkills = data[i].conditionSkill;

                if (conditionSkills != null)
                {
                    level.ConditionSkill = new();

                    for (int skillIndex = 0; skillIndex < conditionSkills.Count; skillIndex++)
                    {
                        TriggerSkill conditionSkill = data[i].conditionSkill[skillIndex];

                        level.ConditionSkill.Add(new(conditionSkill.skillID, GetSkillLevels(conditionSkill.level), conditionSkill.splash, (byte) conditionSkill.skillTarget, (byte) conditionSkill.skillOwner, (short) conditionSkill.fireCount, conditionSkill.interval, conditionSkill.immediateActive)
                        {
                            Delay = conditionSkill.delay,
                            RemoveDelay = conditionSkill.removeDelay,
                            UseDirection = conditionSkill.useDirection,
                            BeginCondition = ParseBeginCondition(conditionSkill.beginCondition)
                        });
                    }
                }

                InvokeEffectProperty invoke = data[i].InvokeEffectProperty;

                if (invoke != null)
                {
                    level.InvokeEffect = new()
                    {
                        Values = invoke.values,
                        Rates = invoke.rates,
                        Types = invoke.types,
                        EffectId = invoke.effectID,
                        EffectGroupId = invoke.effectGroupID,
                        SkillId = invoke.skillID,
                        SkillGroupId = invoke.skillGroupID
                    };
                }

                DotDamageProperty dotDamage = data[i].DotDamageProperty;

                if (dotDamage != null)
                {
                    level.DotDamage = new()
                    {
                        DamageType = (byte) dotDamage.type,
                        Rate = dotDamage.rate,
                        Value = dotDamage.value,
                        Element = dotDamage.element,
                        UseGrade = dotDamage.useGrade
                    };
                }

                ModifyEffectDurationProperty modifyEffect = data[i].ModifyEffectDurationProperty;

                if (modifyEffect != null)
                {
                    level.ModifyEffectDuration = new()
                    {
                        EffectCodes = modifyEffect.effectCodes,
                        DurationFactors = modifyEffect.durationFactors,
                        DurationValues = modifyEffect.durationValues,
                    };
                }

                ModifyOverlapCountProperty modifyOverlapCount = data[i].ModifyOverlapCountProperty;

                if (modifyOverlapCount != null)
                {
                    level.ModifyOverlapCount = new()
                    {
                        EffectCodes = modifyOverlapCount.effectCodes,
                        OffsetCounts = modifyOverlapCount.offsetCounts,
                    };
                }

                metadata.Levels.Add(levelIndex, level);
            }

            effects.Add(metadata);
        }

        return effects;
    }

    private short[] GetSkillLevels(int[] levels)
    {
        short[] result = new short[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            result[i] = (short) levels[i];
        }

        return result;
    }

    private SkillBeginCondition ParseBeginCondition(BeginCondition beginCondition)
    {
        if (beginCondition == null)
        {
            return null;
        }

        return new()
        {
            Owner = ParseOwnerCondition(beginCondition.skillOwner),
            Target = ParseOwnerCondition(beginCondition.skillTarget),
            Caster = ParseOwnerCondition(beginCondition.skillCaster),
            Probability = beginCondition.probability,
        };
    }

    private BeginConditionSubject ParseOwnerCondition(SubConditionTarget ownerCondition)
    {
        if (ownerCondition == null)
        {
            return null;
        }

        return new()
        {
            EventSkillIDs = ownerCondition.eventSkillID,
            EventEffectIDs = ownerCondition.eventEffectID,
            HasBuffId = ownerCondition.hasBuffID[0],
            HasNotBuffId = ownerCondition.hasNotBuffID?.FirstOrDefault(0) ?? 0,
            HasBuffCount = ownerCondition.hasBuffCount[0],
        };
    }

    private void AddStat(EffectStatusMetadata status, StatAttribute stat, float flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (status.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += (long) (flat * 1000);
            currentValue.Rate += rate;

            return;
        }

        status.Stats.Add(stat, new()
        {
            Flat = (long) (flat * 1000),
            Rate = rate,
            AttributeType = StatAttributeType.Rate
        });
    }

    private void AddStat(EffectStatusMetadata status, StatAttribute stat, long flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        EffectStatMetadata currentValue;

        if (status.Stats.TryGetValue(stat, out currentValue))
        {
            currentValue.Flat += flat;
            currentValue.Rate += rate;

            return;
        }

        status.Stats.Add(stat, new()
        {
            Flat = flat,
            Rate = rate,
            AttributeType = StatAttributeType.Flat
        });
    }
}
