using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionRangeParser : Exporter<List<ItemOptionRangeMetadata>>
{
    public ItemOptionRangeParser(MetadataResources resources) : base(resources, "item-option-range") { }

    protected override List<ItemOptionRangeMetadata> Parse()
    {
        List<ItemOptionRangeMetadata> items = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files.Where(x => x.Name.StartsWith("table/itemoptionvariation_")))
        {
            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
            string filename = Path.GetFileNameWithoutExtension(entry.Name);

            ItemOptionRangeMetadata metadata = new();
            foreach (XmlNode node in nodeList)
            {
                switch (filename)
                {
                    case "itemoptionvariation_acc":
                        metadata.RangeType = ItemOptionRangeType.itemoptionvariation_acc;
                        break;
                    case "itemoptionvariation_armor":
                        metadata.RangeType = ItemOptionRangeType.itemoptionvariation_armor;
                        break;
                    case "itemoptionvariation_pet":
                        metadata.RangeType = ItemOptionRangeType.itemoptionvariation_pet;
                        break;
                    case "itemoptionvariation_weapon":
                        metadata.RangeType = ItemOptionRangeType.itemoptionvariation_weapon;
                        break;
                }

                string option = node.Attributes["name"].Value;
                switch (option)
                {
                    case "strValue":
                        metadata.Stats[StatAttribute.Str] = ParseNormalStat(StatAttribute.Str, node, StatAttributeType.Flat);
                        break;
                    case "dexValue":
                        metadata.Stats[StatAttribute.Dex] = ParseNormalStat(StatAttribute.Dex, node, StatAttributeType.Flat);
                        break;
                    case "intValue":
                        metadata.Stats[StatAttribute.Int] = ParseNormalStat(StatAttribute.Int, node, StatAttributeType.Flat);
                        break;
                    case "lukValue":
                        metadata.Stats[StatAttribute.Luk] = ParseNormalStat(StatAttribute.Luk, node, StatAttributeType.Flat);
                        break;
                    case "hpValue":
                        metadata.Stats[StatAttribute.Hp] = ParseNormalStat(StatAttribute.Hp, node, StatAttributeType.Flat);
                        break;
                    case "aspValue":
                        metadata.Stats[StatAttribute.AttackSpeed] = ParseNormalStat(StatAttribute.AttackSpeed, node, StatAttributeType.Flat);
                        break;
                    case "mspValue":
                        metadata.Stats[StatAttribute.MovementSpeed] = ParseNormalStat(StatAttribute.MovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "atpValue":
                        metadata.Stats[StatAttribute.Accuracy] = ParseNormalStat(StatAttribute.Accuracy, node, StatAttributeType.Flat);
                        break;
                    case "evpValue":
                        metadata.Stats[StatAttribute.Evasion] = ParseNormalStat(StatAttribute.Evasion, node, StatAttributeType.Flat);
                        break;
                    case "capValue":
                        metadata.Stats[StatAttribute.CritRate] = ParseNormalStat(StatAttribute.CritRate, node, StatAttributeType.Flat);
                        break;
                    case "cadValue":
                        metadata.Stats[StatAttribute.CritDamage] = ParseNormalStat(StatAttribute.CritDamage, node, StatAttributeType.Flat);
                        break;
                    case "carValue":
                        metadata.Stats[StatAttribute.CritEvasion] = ParseNormalStat(StatAttribute.CritEvasion, node, StatAttributeType.Flat);
                        break;
                    case "nddValue":
                        metadata.Stats[StatAttribute.Defense] = ParseNormalStat(StatAttribute.Defense, node, StatAttributeType.Flat);
                        break;
                    case "papValue":
                        metadata.Stats[StatAttribute.PhysicalAtk] = ParseNormalStat(StatAttribute.PhysicalAtk, node, StatAttributeType.Flat);
                        break;
                    case "mapValue":
                        metadata.Stats[StatAttribute.MagicAtk] = ParseNormalStat(StatAttribute.MagicAtk, node, StatAttributeType.Flat);
                        break;
                    case "parValue":
                        metadata.Stats[StatAttribute.PhysicalRes] = ParseNormalStat(StatAttribute.PhysicalRes, node, StatAttributeType.Flat);
                        break;
                    case "marValue":
                        metadata.Stats[StatAttribute.MagicRes] = ParseNormalStat(StatAttribute.MagicRes, node, StatAttributeType.Flat);
                        break;
                    case "penRate":
                        metadata.Stats[StatAttribute.Pierce] = ParseNormalStat(StatAttribute.Pierce, node, StatAttributeType.Rate);
                        break;
                    case "sgi_BossRate":
                        metadata.SpecialStats[StatAttribute.BossDamage] = ParseSpecialStat(StatAttribute.BossDamage, node, StatAttributeType.Rate);
                        break;
                    case "stunReduceRate":
                        metadata.SpecialStats[StatAttribute.StunReduce] = ParseSpecialStat(StatAttribute.StunReduce, node, StatAttributeType.Rate);
                        break;
                    case "killHPRestoreValue":
                        metadata.SpecialStats[StatAttribute.HpOnKill] = ParseSpecialStat(StatAttribute.HpOnKill, node, StatAttributeType.Flat);
                        break;
                    case "skillCooldownRate":
                        metadata.SpecialStats[StatAttribute.CooldownReduce] = ParseSpecialStat(StatAttribute.CooldownReduce, node, StatAttributeType.Rate);
                        break;
                    case "KnockBackReduceValue":
                        metadata.SpecialStats[StatAttribute.KnockbackReduce] = ParseSpecialStat(StatAttribute.KnockbackReduce, node, StatAttributeType.Flat);
                        break;
                    case "nearDistanceDamageReduceRate":
                        metadata.SpecialStats[StatAttribute.MeleeDamageReduce] = ParseSpecialStat(StatAttribute.MeleeDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "longDistanceDamageReduceRate":
                        metadata.SpecialStats[StatAttribute.RangedDamageReduce] = ParseSpecialStat(StatAttribute.RangedDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "finalAdditionalDamageRate":
                        metadata.SpecialStats[StatAttribute.TotalDamage] = ParseSpecialStat(StatAttribute.TotalDamage, node, StatAttributeType.Rate);
                        break;
                    case "firedamagereduceRate":
                        metadata.SpecialStats[StatAttribute.FireDamageReduce] = ParseSpecialStat(StatAttribute.FireDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "icedamagereduceRate":
                        metadata.SpecialStats[StatAttribute.IceDamageReduce] = ParseSpecialStat(StatAttribute.IceDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "thunderdamagereduceRate":
                        metadata.SpecialStats[StatAttribute.ElectricDamageReduce] = ParseSpecialStat(StatAttribute.ElectricDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "poisondamagereduceRate":
                        metadata.SpecialStats[StatAttribute.PoisonDamageReduce] = ParseSpecialStat(StatAttribute.PoisonDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "darkdamagereduceRate":
                        metadata.SpecialStats[StatAttribute.DarkDamageReduce] = ParseSpecialStat(StatAttribute.DarkDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "lightdamagereduceRate":
                        metadata.SpecialStats[StatAttribute.HolyDamageReduce] = ParseSpecialStat(StatAttribute.HolyDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "parpenRate":
                        metadata.SpecialStats[StatAttribute.PhysicalPiercing] = ParseSpecialStat(StatAttribute.PhysicalPiercing, node, StatAttributeType.Rate);
                        break;
                    case "marpenRate":
                        metadata.SpecialStats[StatAttribute.MagicPiercing] = ParseSpecialStat(StatAttribute.MagicPiercing, node, StatAttributeType.Rate);
                        break;
                    case "firedamageRate":
                        metadata.SpecialStats[StatAttribute.FireDamage] = ParseSpecialStat(StatAttribute.FireDamage, node, StatAttributeType.Rate);
                        break;
                    case "icedamageRate":
                        metadata.SpecialStats[StatAttribute.IceDamage] = ParseSpecialStat(StatAttribute.IceDamage, node, StatAttributeType.Rate);
                        break;
                    case "thunderdamageRate":
                        metadata.SpecialStats[StatAttribute.ElectricDamage] = ParseSpecialStat(StatAttribute.ElectricDamage, node, StatAttributeType.Rate);
                        break;
                    case "poisondamageRate":
                        metadata.SpecialStats[StatAttribute.PoisonDamage] = ParseSpecialStat(StatAttribute.PoisonDamage, node, StatAttributeType.Rate);
                        break;
                    case "darkdamageRate":
                        metadata.SpecialStats[StatAttribute.DarkDamage] = ParseSpecialStat(StatAttribute.DarkDamage, node, StatAttributeType.Rate);
                        break;
                    case "lightdamageRate":
                        metadata.SpecialStats[StatAttribute.HolyDamage] = ParseSpecialStat(StatAttribute.HolyDamage, node, StatAttributeType.Rate);
                        break;
                    case "lddIncreaseRate":
                        metadata.SpecialStats[StatAttribute.RangedDamage] = ParseSpecialStat(StatAttribute.RangedDamage, node, StatAttributeType.Rate);
                        break;
                    case "nddIncreaseRate":
                        metadata.SpecialStats[StatAttribute.MeleeDamage] = ParseSpecialStat(StatAttribute.MeleeDamage, node, StatAttributeType.Rate);
                        break;
                    case "healRate":
                        metadata.SpecialStats[StatAttribute.Heal] = ParseSpecialStat(StatAttribute.Heal, node, StatAttributeType.Rate);
                        break;
                    case "conditionReduceRate":
                        metadata.SpecialStats[StatAttribute.DebuffDurationReduce] = ParseSpecialStat(StatAttribute.DebuffDurationReduce, node, StatAttributeType.Rate);
                        break;
                    case "receivedhealincreaseRate":
                        metadata.SpecialStats[StatAttribute.AllyRecovery] = ParseSpecialStat(StatAttribute.AllyRecovery, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_ox_expRate":
                        metadata.SpecialStats[StatAttribute.OXQuizExp] = ParseSpecialStat(StatAttribute.OXQuizExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_trapmaster_expRate":
                        metadata.SpecialStats[StatAttribute.TrapMasterExp] = ParseSpecialStat(StatAttribute.TrapMasterExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_finalsurvival_expRate":
                        metadata.SpecialStats[StatAttribute.SoleSurvivorExp] = ParseSpecialStat(StatAttribute.SoleSurvivorExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_crazyrunner_expRate":
                        metadata.SpecialStats[StatAttribute.CrazyRunnerExp] = ParseSpecialStat(StatAttribute.CrazyRunnerExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_escape_expRate":
                        metadata.SpecialStats[StatAttribute.LudiEscapeExp] = ParseSpecialStat(StatAttribute.LudiEscapeExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_springbeach_expRate":
                        metadata.SpecialStats[StatAttribute.SpringBeachExp] = ParseSpecialStat(StatAttribute.SpringBeachExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_dancedance_expRate":
                        metadata.SpecialStats[StatAttribute.DanceDanceExp] = ParseSpecialStat(StatAttribute.DanceDanceExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_massive_ox_mspValue":
                        metadata.SpecialStats[StatAttribute.OXMovementSpeed] = ParseSpecialStat(StatAttribute.OXMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_trapmaster_mspValue":
                        metadata.SpecialStats[StatAttribute.TrapMasterMovementSpeed] = ParseSpecialStat(StatAttribute.TrapMasterMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_finalsurvival_mspValue":
                        metadata.SpecialStats[StatAttribute.SoleSurvivorMovementSpeed] = ParseSpecialStat(StatAttribute.SoleSurvivorMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_crazyrunner_mspValue":
                        metadata.SpecialStats[StatAttribute.CrazyRunnerMovementSpeed] = ParseSpecialStat(StatAttribute.CrazyRunnerMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_escape_mspValue":
                        metadata.SpecialStats[StatAttribute.LudiEscapeMovementSpeed] = ParseSpecialStat(StatAttribute.LudiEscapeMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_springbeach_mspValue":
                        metadata.SpecialStats[StatAttribute.SpringBeachMovementSpeed] = ParseSpecialStat(StatAttribute.SpringBeachMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_massive_dancedance_mspValue":
                        metadata.SpecialStats[StatAttribute.DanceDanceStopMovementSpeed] = ParseSpecialStat(StatAttribute.DanceDanceStopMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "npc_hit_reward_sp_ballRate":
                        metadata.SpecialStats[StatAttribute.GenerateSpiritOrbs] = ParseSpecialStat(StatAttribute.GenerateSpiritOrbs, node, StatAttributeType.Rate);
                        break;
                    case "npc_hit_reward_ep_ballRate":
                        metadata.SpecialStats[StatAttribute.GenerateStaminaOrbs] = ParseSpecialStat(StatAttribute.GenerateStaminaOrbs, node, StatAttributeType.Rate);
                        break;
                    case "improve_honor_tokenRate":
                        metadata.SpecialStats[StatAttribute.ValorTokens] = ParseSpecialStat(StatAttribute.ValorTokens, node, StatAttributeType.Rate);
                        break;
                    case "improve_pvp_expRate":
                        metadata.SpecialStats[StatAttribute.PvPExp] = ParseSpecialStat(StatAttribute.PvPExp, node, StatAttributeType.Rate);
                        break;
                    case "improve_darkstream_damageRate":
                        metadata.SpecialStats[StatAttribute.DarkDescentDamageBonus] = ParseSpecialStat(StatAttribute.DarkDescentDamageBonus, node, StatAttributeType.Rate);
                        break;
                    case "reduce_darkstream_recive_damageRate":
                        metadata.SpecialStats[StatAttribute.DarkDescentDamageReduce] = ParseSpecialStat(StatAttribute.DarkDescentDamageReduce, node, StatAttributeType.Rate);
                        break;
                    case "fishing_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleFishingMastery] = ParseSpecialStat(StatAttribute.DoubleFishingMastery, node, StatAttributeType.Rate);
                        break;
                    case "playinstrument_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoublePerformanceMastery] = ParseSpecialStat(StatAttribute.DoublePerformanceMastery, node, StatAttributeType.Rate);
                        break;
                    case "complete_fieldmission_mspValue":
                        metadata.SpecialStats[StatAttribute.ExploredAreasMovementSpeed] = ParseSpecialStat(StatAttribute.ExploredAreasMovementSpeed, node, StatAttributeType.Flat);
                        break;
                    case "improve_glide_vertical_velocityRate":
                        metadata.SpecialStats[StatAttribute.AirMountAscentSpeed] = ParseSpecialStat(StatAttribute.AirMountAscentSpeed, node, StatAttributeType.Rate);
                        break;
                    case "seg_fishingrewardRate":
                        metadata.SpecialStats[StatAttribute.FishingExp] = ParseSpecialStat(StatAttribute.FishingExp, node, StatAttributeType.Rate);
                        break;
                    case "seg_playinstrumentrewardRate":
                        metadata.SpecialStats[StatAttribute.PerformanceExp] = ParseSpecialStat(StatAttribute.PerformanceExp, node, StatAttributeType.Rate);
                        break;
                    case "mining_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleMiningProduction] = ParseSpecialStat(StatAttribute.DoubleMiningProduction, node, StatAttributeType.Rate);
                        break;
                    case "breeding_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleRanchingProduction] = ParseSpecialStat(StatAttribute.DoubleRanchingProduction, node, StatAttributeType.Rate);
                        break;
                    case "gathering_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleForagingProduction] = ParseSpecialStat(StatAttribute.DoubleForagingProduction, node, StatAttributeType.Rate);
                        break;
                    case "farming_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleFarmingProduction] = ParseSpecialStat(StatAttribute.DoubleFarmingProduction, node, StatAttributeType.Rate);
                        break;
                    case "blacksmithing_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleSmithingProduction] = ParseSpecialStat(StatAttribute.DoubleSmithingProduction, node, StatAttributeType.Rate);
                        break;
                    case "engraving_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleHandicraftProduction] = ParseSpecialStat(StatAttribute.DoubleHandicraftProduction, node, StatAttributeType.Rate);
                        break;
                    case "alchemist_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleAlchemyProduction] = ParseSpecialStat(StatAttribute.DoubleAlchemyProduction, node, StatAttributeType.Rate);
                        break;
                    case "cooking_double_rewardRate":
                        metadata.SpecialStats[StatAttribute.DoubleCookingProduction] = ParseSpecialStat(StatAttribute.DoubleCookingProduction, node, StatAttributeType.Rate);
                        break;
                    case "mining_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleMiningMastery] = ParseSpecialStat(StatAttribute.DoubleMiningMastery, node, StatAttributeType.Rate);
                        break;
                    case "breeding_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleRanchingMastery] = ParseSpecialStat(StatAttribute.DoubleRanchingMastery, node, StatAttributeType.Rate);
                        break;
                    case "gathering_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleForagingMastery] = ParseSpecialStat(StatAttribute.DoubleForagingMastery, node, StatAttributeType.Rate);
                        break;
                    case "farming_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleFarmingMastery] = ParseSpecialStat(StatAttribute.DoubleFarmingMastery, node, StatAttributeType.Rate);
                        break;
                    case "blacksmithing_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleSmithingMastery] = ParseSpecialStat(StatAttribute.DoubleSmithingMastery, node, StatAttributeType.Rate);
                        break;
                    case "engraving_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleHandicraftMastery] = ParseSpecialStat(StatAttribute.DoubleHandicraftMastery, node, StatAttributeType.Rate);
                        break;
                    case "alchemist_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleAlchemyMastery] = ParseSpecialStat(StatAttribute.DoubleAlchemyMastery, node, StatAttributeType.Rate);
                        break;
                    case "cooking_double_masteryRate":
                        metadata.SpecialStats[StatAttribute.DoubleCookingMastery] = ParseSpecialStat(StatAttribute.DoubleCookingMastery, node, StatAttributeType.Rate);
                        break;
                    case "smdRate":
                        metadata.SpecialStats[StatAttribute.MesoBonus] = ParseSpecialStat(StatAttribute.MesoBonus, node, StatAttributeType.Rate);
                        break;
                    case "npckilldropitemincrateRate":
                        metadata.SpecialStats[StatAttribute.DropRate] = ParseSpecialStat(StatAttribute.DropRate, node, StatAttributeType.Rate);
                        break;
                    case "improve_darkstream_evpValue":
                        metadata.SpecialStats[StatAttribute.DarkDescentEvasion] = ParseSpecialStat(StatAttribute.DarkDescentEvasion, node, StatAttributeType.Flat);
                        break;
                    case "additionaleffect_95000018Value":
                    case "additionaleffect_95000012Value":
                    case "additionaleffect_95000014Value":
                        continue;
                }
            }

            items.Add(metadata);
        }

        return items;
    }

    private static List<ParserStat> ParseNormalStat(StatAttribute attribute, XmlNode node, StatAttributeType type)
    {
        List<ParserStat> values = new();
        for (int i = 2; i <= 17; i++)
        {
            values.Add(new(attribute, float.Parse(node.Attributes[$"idx{i}"].Value), type));
        }

        return values;
    }
    
    private static List<ParserSpecialStat> ParseSpecialStat(StatAttribute attribute, XmlNode node, StatAttributeType type)
    {
        List<ParserSpecialStat> values = new();
        for (int i = 2; i <= 17; i++)
        {
            values.Add(new(attribute, float.Parse(node.Attributes[$"idx{i}"].Value), type));
        }

        return values;
    }
}
