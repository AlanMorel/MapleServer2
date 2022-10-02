using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
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
                int groupId = 0;

                if (data[i].BasicProperty.groupIDs.Length > 0)
                {
                    groupId = data[i].BasicProperty.groupIDs[0];
                }

                AdditionalEffectLevelMetadata level = new()
                {
                    BeginCondition = ParseBeginCondition(data[i].beginCondition),
                    Basic = new()
                    {
                        MaxBuffCount = data[i].BasicProperty.maxBuffCount,
                        SkillGroupType = (SkillGroupType) data[i].BasicProperty.skillGroupType,
                        Group = groupId,
                        DotCondition = (EffectDotCondition) data[i].BasicProperty.dotCondition,
                        ResetCondition = (EffectResetCondition) data[i].BasicProperty.resetCondition,
                        DurationTick = data[i].BasicProperty.durationTick,
                        KeepCondition = (EffectKeepCondition) data[i].BasicProperty.keepCondition,
                        IntervalTick = data[i].BasicProperty.intervalTick,
                        BuffType = (BuffType) data[i].BasicProperty.buffType,
                        BuffSubType = (BuffSubType) data[i].BasicProperty.buffSubType,
                        CooldownTime = float.Parse(data[i].BasicProperty.coolDownTime ?? "0"),
                        DelayTick = data[i].BasicProperty.delayTick,
                        UseInGameTime = data[i].BasicProperty.useInGameTime,
                        InvokeEvent = data[i].BasicProperty.invokeEvent,
                        DeadKeepEffect = data[i].BasicProperty.deadKeepEffect,
                        LogoutClearEffect = data[i].BasicProperty.logoutClearEffect,
                        LeaveFieldClearEffect = data[i].BasicProperty.leaveFieldClearEffect,
                        CasterIndividualEffect = (CasterIndividualEffect) (data[i].BasicProperty.casterIndividualEffect ? 1 : 0),
                        ClearDistanceFromCaster = float.Parse(data[i].BasicProperty.clearDistanceFromCaster),
                        ClearEffectFromPvpZone = data[i].BasicProperty.clearEffectFromPVPZone,
                        DoNotClearEffectFromEnterPvpZone = data[i].BasicProperty.doNotClearEffectFromEnterPVPZone,
                        ClearCooldownFromPvpZone = data[i].BasicProperty.clearCooldownFromPVPZone
                    },
                    Status = new()
                    {
                        Stats = new()
                    }
                };

                Stat stat = data[i].StatusProperty.Stat;

                if (stat is not null)
                {
                    AddStatFlat(level.Status, StatAttribute.Str, stat.strvalue, stat.strrate);
                    AddStatFlat(level.Status, StatAttribute.Int, stat.intvalue, stat.intrate);
                    AddStatFlat(level.Status, StatAttribute.Luk, stat.lukvalue, stat.lukrate);
                    AddStatFlat(level.Status, StatAttribute.Dex, stat.dexvalue, stat.dexrate);
                    AddStatFlat(level.Status, StatAttribute.Hp, stat.hpvalue, stat.hprate);
                    AddStatFlat(level.Status, StatAttribute.HpRegen, stat.hp_rgpvalue, stat.hp_rgprate);               // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.HpRegenInterval, stat.hp_invvalue, stat.hp_invrate);       // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.Spirit, stat.spvalue, stat.sprate);                    // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.SpRegen, stat.sp_rgpvalue, stat.sp_rgprate);               // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.SpRegenInterval, stat.sp_invvalue, stat.sp_invrate);       // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.Stamina, stat.epvalue, stat.eprate);                   // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.StaminaRegen, stat.ep_rgpvalue, stat.ep_rgprate);          // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.StaminaRegenInterval, stat.ep_invvalue, stat.ep_invrate);  // these might need to be looked at in closer detail
                    AddStatFlat(level.Status, StatAttribute.AttackSpeed, stat.aspvalue, stat.asprate);
                    AddStatFlat(level.Status, StatAttribute.MovementSpeed, stat.mspvalue, stat.msprate);
                    AddStatFlat(level.Status, StatAttribute.Accuracy, stat.atpvalue, stat.atprate);
                    AddStatFlat(level.Status, StatAttribute.Evasion, stat.evpvalue, stat.evprate);
                    AddStatFlat(level.Status, StatAttribute.CritRate, stat.capvalue, stat.caprate);
                    AddStatFlat(level.Status, StatAttribute.CritDamage, stat.cadvalue, stat.cadrate);
                    AddStatFlat(level.Status, StatAttribute.CritEvasion, stat.carvalue, stat.carrate);
                    AddStatFlat(level.Status, StatAttribute.BonusAtk, stat.bapvalue, stat.baprate);
                    AddStatFlat(level.Status, StatAttribute.MountMovementSpeed, stat.rmspvalue, stat.rmsprate);
                    AddStatFlat(level.Status, StatAttribute.Pierce, stat.penvalue, stat.penrate);
                    AddStatFlat(level.Status, StatAttribute.Damage, stat.dmgvalue, stat.dmgrate);
                    AddStatFlat(level.Status, StatAttribute.MinWeaponAtk, stat.wapminvalue, stat.wapminrate);
                    AddStatFlat(level.Status, StatAttribute.MaxWeaponAtk, stat.wapmaxvalue, stat.wapmaxrate);
                    AddStatFlat(level.Status, StatAttribute.MagicRes, stat.marvalue, stat.marrate);  // revisit these to make sure they're right
                    AddStatFlat(level.Status, StatAttribute.MagicAtk, stat.mapvalue, stat.maprate);
                    AddStatFlat(level.Status, StatAttribute.PhysicalRes, stat.parvalue, stat.parrate);  // revisit these to make sure they're right
                    AddStatFlat(level.Status, StatAttribute.PhysicalAtk, stat.papvalue, stat.paprate);
                    AddStatFlat(level.Status, StatAttribute.JumpHeight, stat.jmpvalue, stat.jmprate);
                    AddStatFlat(level.Status, StatAttribute.PerfectGuard, stat.abpvalue, stat.abprate);
                    AddStatFlat(level.Status, StatAttribute.PetBonusAtk, stat.bap_petvalue, stat.bap_petrate);
                }

                SpecialAbility special = data[i].StatusProperty.SpecialAbility;

                if (special is not null)
                {
                    // TODO: finish adding all these stats to the correct attrib type
                    AddStatRate(level.Status, StatAttribute.ExpBonus, special.segvalue, special.segrate); // gm emote exp boost for monster hunting, fishing, performing?
                    AddStatRate(level.Status, StatAttribute.MesoBonus, special.smdvalue, special.smdrate); // smh. gm emote meso boost?
                    AddStatRate(level.Status, StatAttribute.SwimSpeed, special.sssvalue, special.sssrate); // swim speed
                    AddStatRate(level.Status, StatAttribute.DashDistance, special.dashdistancevalue, special.dashdistancerate); // 
                    AddStatRate(level.Status, StatAttribute.TonicDropRate, special.spdvalue, special.spdrate); // 
                    AddStatRate(level.Status, StatAttribute.GearDropRate, special.sidvalue, special.sidrate); // 
                    AddStatRate(level.Status, StatAttribute.TotalDamage, special.finaladditionaldamagevalue, special.finaladditionaldamagerate); // 
                    AddStatRate(level.Status, StatAttribute.CriticalDamage, special.crivalue, special.crirate); // 
                    AddStatRate(level.Status, StatAttribute.Damage, special.sgivalue, special.sgirate); //
                    AddStatRate(level.Status, StatAttribute.LeaderDamage, special.sgi_leadervalue, special.sgi_leaderrate); // 
                    AddStatRate(level.Status, StatAttribute.EliteDamage, special.sgi_elitevalue, special.sgi_eliterate); // 
                    AddStatRate(level.Status, StatAttribute.BossDamage, special.sgi_bossvalue, special.sgi_bossrate); // 
                    AddStatRate(level.Status, StatAttribute.HpOnKill, special.killhprestorevalue, special.killhprestorerate); // 
                    AddStatRate(level.Status, StatAttribute.SpiritOnKill, special.killsprestorevalue, special.killsprestorerate); //  
                    AddStatRate(level.Status, StatAttribute.StaminaOnKill, special.killeprestorevalue, special.killeprestorerate); // 
                    AddStatRate(level.Status, StatAttribute.Heal, special.healvalue, special.healrate); // 
                    AddStatRate(level.Status, StatAttribute.AllyRecovery, special.receivedhealincreasevalue, special.receivedhealincreaserate); // 
                    AddStatRate(level.Status, StatAttribute.IceDamage, special.icedamagevalue, special.icedamagerate); // 
                    AddStatRate(level.Status, StatAttribute.FireDamage, special.firedamagevalue, special.firedamagerate); // 
                    AddStatRate(level.Status, StatAttribute.DarkDamage, special.darkdamagevalue, special.darkdamagerate); //
                    AddStatRate(level.Status, StatAttribute.HolyDamage, special.lightdamagevalue, special.lightdamagerate); //  
                    AddStatRate(level.Status, StatAttribute.PoisonDamage, special.poisondamagevalue, special.poisondamagerate); // 
                    AddStatRate(level.Status, StatAttribute.ElectricDamage, special.thunderdamagevalue, special.thunderdamagerate); // 
                    AddStatRate(level.Status, StatAttribute.MeleeDamage, special.nddincreasevalue, special.nddincreaserate); // 
                    AddStatRate(level.Status, StatAttribute.RangedDamage, special.lddincreasevalue, special.lddincreaserate); // 
                    AddStatRate(level.Status, StatAttribute.PhysicalPiercing, special.parpenvalue, special.parpenrate); //  
                    AddStatRate(level.Status, StatAttribute.MagicPiercing, special.marpenvalue, special.marpenrate); //  
                    AddStatRate(level.Status, StatAttribute.IceDamageReduce, special.icedamagereducevalue, special.icedamagereducerate); //  
                    AddStatRate(level.Status, StatAttribute.FireDamageReduce, special.firedamagereducevalue, special.firedamagereducerate); //  
                    AddStatRate(level.Status, StatAttribute.DarkDamageReduce, special.darkdamagereducevalue, special.darkdamagereducerate); //  
                    AddStatRate(level.Status, StatAttribute.HolyDamageReduce, special.lightdamagereducevalue, special.lightdamagereducerate); // 
                    AddStatRate(level.Status, StatAttribute.PoisonDamageReduce, special.poisondamagereducevalue, special.poisondamagereducerate); // 
                    AddStatRate(level.Status, StatAttribute.ElectricDamageReduce, special.thunderdamagereducevalue, special.thunderdamagereducerate); // 
                    AddStatRate(level.Status, StatAttribute.StunReduce, special.stunreducevalue, special.stunreducerate); // 
                    AddStatRate(level.Status, StatAttribute.DebuffDurationReduce, special.conditionreducevalue, special.conditionreducerate); // 
                    AddStatRate(level.Status, StatAttribute.CooldownReduce, special.skillcooldownvalue, special.skillcooldownrate); // 
                    AddStatRate(level.Status, StatAttribute.MeleeDamageReduce, special.neardistancedamagereducevalue, special.neardistancedamagereducerate); //
                    AddStatRate(level.Status, StatAttribute.RangedDamageReduce, special.longdistancedamagereducevalue, special.longdistancedamagereducerate); //  
                    AddStatRate(level.Status, StatAttribute.KnockbackReduce, special.knockbackreducevalue, special.knockbackreducerate); // 
                    AddStatRate(level.Status, StatAttribute.MeleeStun, special.stunprocnddvalue, special.stunprocnddrate); // 
                    AddStatRate(level.Status, StatAttribute.RangedStun, special.stunproclddvalue, special.stunproclddrate); // 
                    AddStatRate(level.Status, StatAttribute.MeeleeKnockback, special.knockbackprocnddvalue, special.knockbackprocnddrate); // 
                    AddStatRate(level.Status, StatAttribute.RangedKnockback, special.knockbackproclddvalue, special.knockbackproclddrate); // 
                    AddStatRate(level.Status, StatAttribute.MeleeImmob, special.snareprocnddvalue, special.snareprocnddrate); // 
                    AddStatRate(level.Status, StatAttribute.RangedImmob, special.snareproclddvalue, special.snareproclddrate); // 
                    AddStatRate(level.Status, StatAttribute.MeleeAoeDamage, special.aoeprocnddvalue, special.aoeprocnddrate); // 
                    AddStatRate(level.Status, StatAttribute.RangedAoeDamage, special.aoeproclddvalue, special.aoeproclddrate); //
                    AddStatRate(level.Status, StatAttribute.DropRate, special.npckilldropitemincratevalue, special.npckilldropitemincraterate); // 
                    AddStatRate(level.Status, StatAttribute.QuestExp, special.seg_questrewardvalue, special.seg_questrewardrate); // 
                    AddStatRate(level.Status, StatAttribute.QuestMeso, special.smd_questrewardvalue, special.smd_questrewardrate); // 
                    AddStatRate(level.Status, StatAttribute.FishingExp, special.seg_fishingrewardvalue, special.seg_fishingrewardrate); // 
                    AddStatRate(level.Status, StatAttribute.ArcadeExp, special.seg_arcaderewardvalue, special.seg_arcaderewardrate); // 
                    AddStatRate(level.Status, StatAttribute.PerformanceExp, special.seg_playinstrumentrewardvalue, special.seg_playinstrumentrewardrate); // 
                    AddStatRate(level.Status, StatAttribute.InvokeEffect1, special.invoke_effect1value, special.invoke_effect1rate); //  
                    AddStatRate(level.Status, StatAttribute.InvokeEffect2, special.invoke_effect2value, special.invoke_effect2rate); //
                    AddStatRate(level.Status, StatAttribute.InvokeEffect3, special.invoke_effect3value, special.invoke_effect3rate); // 
                    AddStatRate(level.Status, StatAttribute.PvPDamage, special.pvpdamageincreasevalue, special.pvpdamageincreaserate); // 
                    AddStatRate(level.Status, StatAttribute.PvPDefense, special.pvpdamagereducevalue, special.pvpdamagereducerate); // 
                    AddStatRate(level.Status, StatAttribute.GuildExp, special.improveguildexpvalue, special.improveguildexprate); // 
                    AddStatRate(level.Status, StatAttribute.GuildCoin, special.improveguildcoinvalue, special.improveguildcoinrate); // 
                    AddStatRate(level.Status, StatAttribute.McKayXpOrb, special.improvemassiveeventbexpballvalue, special.improvemassiveeventbexpballrate); // 
                    AddStatRate(level.Status, StatAttribute.BlackMarketReduce, special.reduce_meso_trade_feevalue, special.reduce_meso_trade_feerate); // 
                    AddStatRate(level.Status, StatAttribute.EnchantCatalystDiscount, special.reduce_enchant_matrial_feevalue, special.reduce_enchant_matrial_feerate); // 
                    AddStatRate(level.Status, StatAttribute.MeretReviveFee, special.reduce_merat_revival_feevalue, special.reduce_merat_revival_feerate); // 
                    AddStatRate(level.Status, StatAttribute.MiningBonus, special.improve_mining_reward_itemvalue, special.improve_mining_reward_itemrate); // 
                    AddStatRate(level.Status, StatAttribute.RanchingBonus, special.improve_breeding_reward_itemvalue, special.improve_breeding_reward_itemrate); // 
                    AddStatRate(level.Status, StatAttribute.SmithingExp, special.improve_blacksmithing_reward_masteryvalue, special.improve_blacksmithing_reward_masteryrate); // 
                    AddStatRate(level.Status, StatAttribute.HandicraftMastery, special.improve_engraving_reward_masteryvalue, special.improve_engraving_reward_masteryrate); //  
                    AddStatRate(level.Status, StatAttribute.ForagingBonus, special.improve_gathering_reward_itemvalue, special.improve_gathering_reward_itemrate); // 
                    AddStatRate(level.Status, StatAttribute.FarmingBonus, special.improve_farming_reward_itemvalue, special.improve_farming_reward_itemrate); //  
                    AddStatRate(level.Status, StatAttribute.AlchemyMastery, special.improve_alchemist_reward_masteryvalue, special.improve_alchemist_reward_masteryrate); // 
                    AddStatRate(level.Status, StatAttribute.CookingMastery, special.improve_cooking_reward_masteryvalue, special.improve_cooking_reward_masteryrate); // 
                    AddStatRate(level.Status, StatAttribute.ForagingExp, special.improve_acquire_gathering_expvalue, special.improve_acquire_gathering_exprate); //
                    AddStatRate(level.Status, StatAttribute.TECH, special.skill_levelup_tier_1value, special.skill_levelup_tier_1rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_2, special.skill_levelup_tier_2value, special.skill_levelup_tier_2rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_10, special.skill_levelup_tier_3value, special.skill_levelup_tier_3rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_13, special.skill_levelup_tier_4value, special.skill_levelup_tier_4rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_16, special.skill_levelup_tier_5value, special.skill_levelup_tier_5rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_19, special.skill_levelup_tier_6value, special.skill_levelup_tier_6rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_22, special.skill_levelup_tier_7value, special.skill_levelup_tier_7rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_25, special.skill_levelup_tier_8value, special.skill_levelup_tier_8rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_28, special.skill_levelup_tier_9value, special.skill_levelup_tier_9rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_31, special.skill_levelup_tier_10value, special.skill_levelup_tier_10rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_34, special.skill_levelup_tier_11value, special.skill_levelup_tier_11rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_37, special.skill_levelup_tier_12value, special.skill_levelup_tier_12rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_40, special.skill_levelup_tier_13value, special.skill_levelup_tier_13rate); // 
                    AddStatRate(level.Status, StatAttribute.TECH_43, special.skill_levelup_tier_14value, special.skill_levelup_tier_14rate); // 
                    AddStatRate(level.Status, StatAttribute.OXQuizExp, special.improve_massive_ox_expvalue, special.improve_massive_ox_exprate); // 
                    AddStatRate(level.Status, StatAttribute.TrapMasterExp, special.improve_massive_trapmaster_expvalue, special.improve_massive_trapmaster_exprate); // 
                    AddStatRate(level.Status, StatAttribute.SoleSurvivorExp, special.improve_massive_finalsurvival_expvalue, special.improve_massive_finalsurvival_exprate); //
                    AddStatRate(level.Status, StatAttribute.CrazyRunnerExp, special.improve_massive_crazyrunner_expvalue, special.improve_massive_crazyrunner_exprate); //
                    AddStatRate(level.Status, StatAttribute.ShanghaiCrazyRunnersExp, special.improve_massive_sh_crazyrunner_expvalue, special.improve_massive_sh_crazyrunner_exprate); //
                    AddStatRate(level.Status, StatAttribute.LudiEscapeExp, special.improve_massive_escape_expvalue, special.improve_massive_escape_exprate); //
                    AddStatRate(level.Status, StatAttribute.SpringBeachExp, special.improve_massive_springbeach_expvalue, special.improve_massive_springbeach_exprate); //
                    AddStatRate(level.Status, StatAttribute.DanceDanceExp, special.improve_massive_dancedance_expvalue, special.improve_massive_dancedance_exprate); //
                    AddStatRate(level.Status, StatAttribute.OXMovementSpeed, special.improve_massive_ox_mspvalue, special.improve_massive_ox_msprate); //
                    AddStatRate(level.Status, StatAttribute.TrapMasterMovementSpeed, special.improve_massive_trapmaster_mspvalue, special.improve_massive_trapmaster_msprate); // 
                    AddStatRate(level.Status, StatAttribute.SoleSurvivorMovementSpeed, special.improve_massive_finalsurvival_mspvalue, special.improve_massive_finalsurvival_msprate); //
                    AddStatRate(level.Status, StatAttribute.CrazyRunnerMovementSpeed, special.improve_massive_crazyrunner_mspvalue, special.improve_massive_crazyrunner_msprate); //
                    AddStatRate(level.Status, StatAttribute.ShanghaiCrazyRunnersMovementSpeed, special.improve_massive_sh_crazyrunner_mspvalue, special.improve_massive_sh_crazyrunner_msprate); //
                    AddStatRate(level.Status, StatAttribute.LudiEscapeMovementSpeed, special.improve_massive_escape_mspvalue, special.improve_massive_escape_msprate); //
                    AddStatRate(level.Status, StatAttribute.SpringBeachMovementSpeed, special.improve_massive_springbeach_mspvalue, special.improve_massive_springbeach_msprate); //
                    AddStatRate(level.Status, StatAttribute.DanceDanceStopMovementSpeed, special.improve_massive_dancedance_mspvalue, special.improve_massive_dancedance_msprate); //
                    AddStatRate(level.Status, StatAttribute.GenerateSpiritOrbs, special.npc_hit_reward_sp_ballvalue, special.npc_hit_reward_sp_ballrate); //
                    AddStatRate(level.Status, StatAttribute.GenerateStaminaOrbs, special.npc_hit_reward_ep_ballvalue, special.npc_hit_reward_ep_ballrate); //
                    AddStatRate(level.Status, StatAttribute.ValorTokens, special.improve_honor_tokenvalue, special.improve_honor_tokenrate); //
                    AddStatRate(level.Status, StatAttribute.PvPExp, special.improve_pvp_expvalue, special.improve_pvp_exprate); //
                    AddStatRate(level.Status, StatAttribute.DarkDescentDamageBonus, special.improve_darkstream_damagevalue, special.improve_darkstream_damagerate); //
                    AddStatRate(level.Status, StatAttribute.DarkDescentDamageReduce, special.improve_darkstream_evpvalue, special.improve_darkstream_evprate); //
                    AddStatRate(level.Status, StatAttribute.DarkDescentEvasion, special.reduce_darkstream_recive_damagevalue, special.reduce_darkstream_recive_damagerate); //
                    AddStatRate(level.Status, StatAttribute.DoubleFishingMastery, special.fishing_double_masteryvalue, special.fishing_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoublePerformanceMastery, special.playinstrument_double_masteryvalue, special.playinstrument_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.ExploredAreasMovementSpeed, special.complete_fieldmission_mspvalue, special.complete_fieldmission_msprate); //
                    AddStatRate(level.Status, StatAttribute.AirMountAscentSpeed, special.improve_glide_vertical_velocityvalue, special.improve_glide_vertical_velocityrate); //
                    //AddStat(level.Status, StatAttribute.Unknown, special.additionaleffect_95000018value, special.additionaleffect_95000018rate); //
                    AddStatRate(level.Status, StatAttribute.EnemyDefenseDecreaseOnHit, special.additionaleffect_95000012value, special.additionaleffect_95000012rate); //
                    AddStatRate(level.Status, StatAttribute.EnemyAttackDecreaseOnHit, special.additionaleffect_95000014value, special.additionaleffect_95000014rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageIf1NearbyEnemy, special.additionaleffect_95000020value, special.additionaleffect_95000020rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageIf3NearbyEnemies, special.additionaleffect_95000021value, special.additionaleffect_95000021rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageIf80Spirit, special.additionaleffect_95000022value, special.additionaleffect_95000022rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageIfFullStamina, special.additionaleffect_95000023value, special.additionaleffect_95000023rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageIfHerbEffectActive, special.additionaleffect_95000024value, special.additionaleffect_95000024rate); //
                    AddStatRate(level.Status, StatAttribute.IncreaseTotalDamageToWorldBoss, special.additionaleffect_95000025value, special.additionaleffect_95000025rate); //
                    AddStatRate(level.Status, StatAttribute.Effect95000026, special.additionaleffect_95000026value, special.additionaleffect_95000026rate); //
                    AddStatRate(level.Status, StatAttribute.Effect95000027, special.additionaleffect_95000027value, special.additionaleffect_95000027rate); //
                    AddStatRate(level.Status, StatAttribute.Effect95000028, special.additionaleffect_95000028value, special.additionaleffect_95000028rate); //
                    AddStatRate(level.Status, StatAttribute.Effect95000029, special.additionaleffect_95000029value, special.additionaleffect_95000029rate); //
                    AddStatRate(level.Status, StatAttribute.StaminaRecoverySpeed, special.reduce_recovery_ep_invvalue, special.reduce_recovery_ep_invrate);
                    AddStatRate(level.Status, StatAttribute.MaxWeaponAttack, special.improve_stat_wap_uvalue, special.improve_stat_wap_urate); //
                    AddStatRate(level.Status, StatAttribute.DoubleMiningProduction, special.mining_double_rewardvalue, special.mining_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleRanchingProduction, special.breeding_double_rewardvalue, special.breeding_double_rewardrate);
                    AddStatRate(level.Status, StatAttribute.DoubleForagingProduction, special.gathering_double_rewardvalue, special.gathering_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleFarmingProduction, special.farming_double_rewardvalue, special.farming_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleSmithingProduction, special.blacksmithing_double_rewardvalue, special.blacksmithing_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleHandicraftProduction, special.engraving_double_rewardvalue, special.engraving_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleAlchemyProduction, special.alchemist_double_rewardvalue, special.alchemist_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleCookingProduction, special.cooking_double_rewardvalue, special.cooking_double_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleMiningMastery, special.mining_double_masteryvalue, special.mining_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleRanchingMastery, special.breeding_double_masteryvalue, special.breeding_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleForagingMastery, special.gathering_double_masteryvalue, special.gathering_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleFarmingMastery, special.farming_double_masteryvalue, special.farming_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleSmithingMastery, special.blacksmithing_double_masteryvalue, special.blacksmithing_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleHandicraftMastery, special.engraving_double_masteryvalue, special.engraving_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleAlchemyMastery, special.alchemist_double_masteryvalue, special.alchemist_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.DoubleCookingMastery, special.cooking_double_masteryvalue, special.cooking_double_masteryrate); //
                    AddStatRate(level.Status, StatAttribute.ChaosRaidWeaponAttack, special.improve_chaosraid_wapvalue, special.improve_chaosraid_waprate); //
                    AddStatRate(level.Status, StatAttribute.ChaosRaidAttackSpeed, special.improve_chaosraid_aspvalue, special.improve_chaosraid_asprate); //
                    AddStatRate(level.Status, StatAttribute.ChaosRaidAccuracy, special.improve_chaosraid_atpvalue, special.improve_chaosraid_atprate); //
                    AddStatRate(level.Status, StatAttribute.ChaosRaidHealth, special.improve_chaosraid_hpvalue, special.improve_chaosraid_hprate);
                    AddStatRate(level.Status, StatAttribute.StaminaAndSpiritFromOrbs, special.improve_recovery_ballvalue, special.improve_recovery_ballrate); //
                    AddStatRate(level.Status, StatAttribute.WorldBossExp, special.improve_fieldboss_kill_expvalue, special.improve_fieldboss_kill_exprate); //
                    AddStatRate(level.Status, StatAttribute.WorldBossDropRate, special.improve_fieldboss_kill_dropvalue, special.improve_fieldboss_kill_droprate); //
                    AddStatRate(level.Status, StatAttribute.WorldBossDamageReduce, special.reduce_fieldboss_recive_damagevalue, special.reduce_fieldboss_recive_damagerate); //
                    AddStatRate(level.Status, StatAttribute.Effect9500016, special.additionaleffect_95000016value, special.additionaleffect_95000016rate); //
                    AddStatRate(level.Status, StatAttribute.PetCaptureRewards, special.improve_pettrap_rewardvalue, special.improve_pettrap_rewardrate); //
                    AddStatRate(level.Status, StatAttribute.MiningEfficency, special.mining_multiactionvalue, special.mining_multiactionrate); //
                    AddStatRate(level.Status, StatAttribute.RanchingEfficiency, special.breeding_multiactionvalue, special.breeding_multiactionrate); //
                    AddStatRate(level.Status, StatAttribute.ForagingEfficiency, special.gathering_multiactionvalue, special.gathering_multiactionrate); //
                    AddStatRate(level.Status, StatAttribute.FarmingEfficiency, special.farming_multiactionvalue, special.farming_multiactionrate); //
                    AddStatRate(level.Status, StatAttribute.HealthBasedDamageReduce, special.reduce_damage_by_targetmaxhpvalue, special.reduce_damage_by_targetmaxhprate); //

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
                    AddStatRate(level.Status, StatAttribute.PhysicalAtk, special.offensive_physicaldamagevalue, special.offensive_physicaldamagerate); // revisit these
                    AddStatRate(level.Status, StatAttribute.MagicAtk, special.offensive_magicaldamagevalue, special.offensive_magicaldamagerate); // revisit these
                    //AddStat(level.Status, StatAttribute.Unknown, special.reduce_gameitem_socket_unlock_feevalue, special.reduce_gameitem_socket_unlock_feerate); //
                }

                OffensiveProperty offensive = data[i].OffensiveProperty;

                if (offensive is not null)
                {
                    level.Offensive = new()
                    {
                        AlwaysCrit = offensive.attackSuccessCritical != 0,
                        ImmuneBreak = offensive.hitImmuneBreak
                    };

                    AddStatFlat(level.Status, StatAttribute.PhysicalAtk, offensive.papDamageV, offensive.papDamageR);
                    AddStatFlat(level.Status, StatAttribute.MagicAtk, offensive.mapDamageV, offensive.mapDamageR);
                }

                DefensiveProperty defensive = data[i].DefensiveProperty;

                if (defensive is not null)
                {
                    level.Defesive = new()
                    {
                        Invincible = defensive.invincible != 0
                    };

                    AddStatFlat(level.Status, StatAttribute.PhysicalRes, defensive.papDamageV, defensive.papDamageR);
                    AddStatFlat(level.Status, StatAttribute.MagicRes, defensive.mapDamageV, defensive.mapDamageR);
                }

                RecoveryProperty recovery = data[i].RecoveryProperty;

                if (recovery is not null)
                {
                    level.Recovery = new()
                    {
                        RecoveryRate = recovery.RecoveryRate
                    };
                }

                CancelEffectProperty cancel = data[i].CancelEffectProperty;

                if (cancel is not null)
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

                if (immune is not null)
                {
                    level.ImmuneEffect = new()
                    {
                        ImmuneEffectCodes = immune.immuneEffectCodes,
                        ImmuneBuffCategories = immune.immuneBuffCategories
                    };
                }

                ResetSkillCoolDownTimeProperty cooldownReset = data[i].ResetSkillCoolDownTimeProperty;

                if (cooldownReset is not null)
                {
                    level.ResetCoolDownTime = new()
                    {
                        SkillCodes = cooldownReset.skillCodes.Select(x => (long) x).ToArray()
                    };
                }

                List<TriggerSkill> splashSkills = data[i].splashSkill;

                if (splashSkills is not null)
                {
                    level.SplashSkill = new();

                    for (int skillIndex = 0; skillIndex < splashSkills.Count; skillIndex++)
                    {
                        TriggerSkill splashSkill = data[i].splashSkill[skillIndex];

                        level.SplashSkill.Add(new(splashSkill.skillID, GetSkillLevels(splashSkill.level), splashSkill.splash, (byte) splashSkill.skillTarget,
                            (byte) splashSkill.skillOwner, (short) splashSkill.fireCount, splashSkill.interval, splashSkill.immediateActive)
                        {
                            Delay = splashSkill.delay,
                            RemoveDelay = splashSkill.removeDelay,
                            UseDirection = splashSkill.useDirection,
                            RandomCast = splashSkill.randomCast,
                            LinkSkillId = splashSkill.linkSkillID,
                            OverlapCount = splashSkill.overlapCount,
                            NonTargetActive = splashSkill.nonTargetActive,
                            OnlySensingActive = splashSkill.onlySensingActive,
                            DependOnCasterState = splashSkill.dependOnCasterState,
                            ActiveByIntervalTick = splashSkill.activeByIntervalTick,
                            DependOnDamageCount = splashSkill.dependOnDamageCount,
                            BeginCondition = ParseBeginCondition(splashSkill.beginCondition)
                        });
                    }
                }

                List<TriggerSkill> conditionSkills = data[i].conditionSkill;

                if (conditionSkills is not null)
                {
                    level.ConditionSkill = new();

                    for (int skillIndex = 0; skillIndex < conditionSkills.Count; skillIndex++)
                    {
                        TriggerSkill conditionSkill = data[i].conditionSkill[skillIndex];

                        level.ConditionSkill.Add(new(conditionSkill.skillID, GetSkillLevels(conditionSkill.level), conditionSkill.splash,
                            (byte) conditionSkill.skillTarget, (byte) conditionSkill.skillOwner, (short) conditionSkill.fireCount, conditionSkill.interval,
                            conditionSkill.immediateActive)
                        {
                            Delay = conditionSkill.delay,
                            RemoveDelay = conditionSkill.removeDelay,
                            UseDirection = conditionSkill.useDirection,
                            RandomCast = conditionSkill.randomCast,
                            LinkSkillId = conditionSkill.linkSkillID,
                            OverlapCount = conditionSkill.overlapCount,
                            NonTargetActive = conditionSkill.nonTargetActive,
                            OnlySensingActive = conditionSkill.onlySensingActive,
                            DependOnCasterState = conditionSkill.dependOnCasterState,
                            ActiveByIntervalTick = conditionSkill.activeByIntervalTick,
                            DependOnDamageCount = conditionSkill.dependOnDamageCount,
                            BeginCondition = ParseBeginCondition(conditionSkill.beginCondition)
                        });
                    }
                }

                InvokeEffectProperty invoke = data[i].InvokeEffectProperty;

                if (invoke is not null)
                {
                    level.InvokeEffect = new()
                    {
                        Values = invoke.values,
                        Rates = invoke.rates,
                        Types = new InvokeEffectType[invoke.types.Length],
                        EffectId = invoke.effectID,
                        EffectGroupId = invoke.effectGroupID,
                        SkillId = invoke.skillID,
                        SkillGroupId = invoke.skillGroupID
                    };

                    for (int type = 0; type < invoke.types.Length; ++type)
                    {
                        level.InvokeEffect.Types[type] = (InvokeEffectType) invoke.types[type];
                    }
                }

                DotDamageProperty dotDamage = data[i].DotDamageProperty;

                if (dotDamage is not null)
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

                if (modifyEffect is not null)
                {
                    level.ModifyEffectDuration = new()
                    {
                        EffectCodes = modifyEffect.effectCodes,
                        DurationFactors = modifyEffect.durationFactors,
                        DurationValues = modifyEffect.durationValues,
                    };
                }

                ModifyOverlapCountProperty modifyOverlapCount = data[i].ModifyOverlapCountProperty;

                if (modifyOverlapCount is not null)
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

    private SkillBeginCondition? ParseBeginCondition(BeginCondition? beginCondition)
    {
        if (beginCondition is null)
        {
            return null;
        }

        return new()
        {
            Owner = ParseOwnerCondition(beginCondition.skillOwner),
            Target = ParseOwnerCondition(beginCondition.skillTarget),
            Caster = ParseOwnerCondition(beginCondition.skillCaster),
            Probability = beginCondition.probability,
            InvokeEffectFactor = beginCondition.invokeEffectFactor,
            CooldownTime = beginCondition.cooldownTime,
            DefaultRechargingCooldownTime = beginCondition.defaultRechargingCooldownTime,
            AllowDeadState = beginCondition.allowDeadState,
            RequireDurationWithoutMove = beginCondition.requireDurationWithoutMove,
            UseTargetCountFactor = beginCondition.useTargetCountFactor,
            //RequireSkillCodes = new(),
            //RequireMapCodes = new(),
            //RequireMapCategoryCodes = new(),
            //RequireDungeonRooms = new(),
            //Jobs = new(),
            //MapContinents = new()
        };
    }

    private BeginConditionSubject? ParseOwnerCondition(SubConditionTarget? ownerCondition)
    {
        if (ownerCondition is null)
        {
            return null;
        }

        if (!Enum.TryParse(ownerCondition.targetCountSign, out ConditionOperator targetCountSign))
        {
            targetCountSign = ConditionOperator.None;
        }

        if (!Enum.TryParse(ownerCondition.hasBuffCountCompare[0], out ConditionOperator hasBuffCountCompare))
        {
            targetCountSign = ConditionOperator.None;
        }

        return new()
        {
            EventSkillIDs = ownerCondition.eventSkillID,
            EventEffectIDs = ownerCondition.eventEffectID,
            RequireBuffId = ownerCondition.hasBuffID[0],
            HasNotBuffId = ownerCondition.hasNotBuffID?.FirstOrDefault(0) ?? 0,
            RequireBuffCount = ownerCondition.hasBuffCount[0],
            RequireBuffCountCompare = hasBuffCountCompare,
            RequireBuffLevel = ownerCondition.hasBuffLevel[0],
            EventCondition = (EffectEvent) ownerCondition.eventCondition,
            IgnoreOwnerEvent = ownerCondition.ignoreOwnerEvent,
            TargetCheckRange = ownerCondition.targetCheckRange,
            TargetCheckMinRange = ownerCondition.targetCheckMinRange,
            TargetInRangeCount = ownerCondition.targetInRangeCount,
            TargetFriendly = (TargetAllieganceType) ownerCondition.targetFriendly,
            TargetCountSign = targetCountSign
        };
    }

    private void AddStatRate(EffectStatusMetadata status, StatAttribute stat, float flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        if (status.Stats.TryGetValue(stat, out EffectStatMetadata? currentValue))
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

    private void AddStatFlat(EffectStatusMetadata status, StatAttribute stat, long flat, float rate)
    {
        if (flat == 0 && rate == 0)
        {
            return;
        }

        if (status.Stats.TryGetValue(stat, out EffectStatMetadata? currentValue))
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
