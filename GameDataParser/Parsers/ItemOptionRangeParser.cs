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
                        metadata.Stats[StatId.Str] = ParseIntValues(StatId.Str, node);
                        break;
                    case "dexValue":
                        metadata.Stats[StatId.Dex] = ParseIntValues(StatId.Dex, node);
                        break;
                    case "intValue":
                        metadata.Stats[StatId.Int] = ParseIntValues(StatId.Int, node);
                        break;
                    case "lukValue":
                        metadata.Stats[StatId.Luk] = ParseIntValues(StatId.Luk, node);
                        break;
                    case "hpValue":
                        metadata.Stats[StatId.Hp] = ParseIntValues(StatId.Hp, node);
                        break;
                    case "aspValue":
                        metadata.Stats[StatId.AttackSpeed] = ParseIntValues(StatId.AttackSpeed, node);
                        break;
                    case "mspValue":
                        metadata.Stats[StatId.MovementSpeed] = ParseIntValues(StatId.MovementSpeed, node);
                        break;
                    case "atpValue":
                        metadata.Stats[StatId.Accuracy] = ParseIntValues(StatId.Accuracy, node);
                        break;
                    case "evpValue":
                        metadata.Stats[StatId.Evasion] = ParseIntValues(StatId.Evasion, node);
                        break;
                    case "capValue":
                        metadata.Stats[StatId.CritRate] = ParseIntValues(StatId.CritRate, node);
                        break;
                    case "cadValue":
                        metadata.Stats[StatId.CritDamage] = ParseIntValues(StatId.CritDamage, node);
                        break;
                    case "carValue":
                        metadata.Stats[StatId.CritEvasion] = ParseIntValues(StatId.CritEvasion, node);
                        break;
                    case "nddValue":
                        metadata.Stats[StatId.Defense] = ParseIntValues(StatId.Defense, node);
                        break;
                    case "papValue":
                        metadata.Stats[StatId.PhysicalAtk] = ParseIntValues(StatId.PhysicalAtk, node);
                        break;
                    case "mapValue":
                        metadata.Stats[StatId.MagicAtk] = ParseIntValues(StatId.MagicAtk, node);
                        break;
                    case "parValue":
                        metadata.Stats[StatId.PhysicalRes] = ParseIntValues(StatId.PhysicalRes, node);
                        break;
                    case "marValue":
                        metadata.Stats[StatId.MagicRes] = ParseIntValues(StatId.MagicRes, node);
                        break;
                    case "penRate":
                        metadata.Stats[StatId.Pierce] = ParseFloatValues(StatId.Pierce, node);
                        break;
                    case "sgi_BossRate":
                        metadata.SpecialStats[SpecialStatId.BossDamage] = ParseSpecialValues(SpecialStatId.BossDamage, node, true);
                        break;
                    case "stunReduceRate":
                        metadata.SpecialStats[SpecialStatId.StunReduce] = ParseSpecialValues(SpecialStatId.StunReduce, node, true);
                        break;
                    case "killHPRestoreValue":
                        metadata.SpecialStats[SpecialStatId.HpOnKill] = ParseSpecialValues(SpecialStatId.HpOnKill, node, false);
                        break;
                    case "skillCooldownRate":
                        metadata.SpecialStats[SpecialStatId.CooldownReduce] = ParseSpecialValues(SpecialStatId.CooldownReduce, node, true);
                        break;
                    case "KnockBackReduceValue":
                        metadata.SpecialStats[SpecialStatId.KnockbackReduce] = ParseSpecialValues(SpecialStatId.KnockbackReduce, node, false);
                        break;
                    case "nearDistanceDamageReduceRate":
                        metadata.SpecialStats[SpecialStatId.MeleeDamageReduce] = ParseSpecialValues(SpecialStatId.MeleeDamageReduce, node, true);
                        break;
                    case "longDistanceDamageReduceRate":
                        metadata.SpecialStats[SpecialStatId.RangedDamageReduce] = ParseSpecialValues(SpecialStatId.RangedDamageReduce, node, true);
                        break;
                    case "finalAdditionalDamageRate":
                        metadata.SpecialStats[SpecialStatId.TotalDamage] = ParseSpecialValues(SpecialStatId.TotalDamage, node, true);
                        break;
                    case "firedamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.FireDamageReduce] = ParseSpecialValues(SpecialStatId.FireDamageReduce, node, true);
                        break;
                    case "icedamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.IceDamageReduce] = ParseSpecialValues(SpecialStatId.IceDamageReduce, node, true);
                        break;
                    case "thunderdamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.ElectricDamageReduce] = ParseSpecialValues(SpecialStatId.ElectricDamageReduce, node, true);
                        break;
                    case "poisondamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.PoisonDamageReduce] = ParseSpecialValues(SpecialStatId.PoisonDamageReduce, node, true);
                        break;
                    case "darkdamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.DarkDamageReduce] = ParseSpecialValues(SpecialStatId.DarkDamageReduce, node, true);
                        break;
                    case "lightdamagereduceRate":
                        metadata.SpecialStats[SpecialStatId.HolyDamageReduce] = ParseSpecialValues(SpecialStatId.HolyDamageReduce, node, true);
                        break;
                    case "parpenRate":
                        metadata.SpecialStats[SpecialStatId.PhysicalPiercing] = ParseSpecialValues(SpecialStatId.PhysicalPiercing, node, true);
                        break;
                    case "marpenRate":
                        metadata.SpecialStats[SpecialStatId.MagicPiercing] = ParseSpecialValues(SpecialStatId.MagicPiercing, node, true);
                        break;
                    case "firedamageRate":
                        metadata.SpecialStats[SpecialStatId.FireDamage] = ParseSpecialValues(SpecialStatId.FireDamage, node, true);
                        break;
                    case "icedamageRate":
                        metadata.SpecialStats[SpecialStatId.IceDamage] = ParseSpecialValues(SpecialStatId.IceDamage, node, true);
                        break;
                    case "thunderdamageRate":
                        metadata.SpecialStats[SpecialStatId.ElectricDamage] = ParseSpecialValues(SpecialStatId.ElectricDamage, node, true);
                        break;
                    case "poisondamageRate":
                        metadata.SpecialStats[SpecialStatId.PoisonDamage] = ParseSpecialValues(SpecialStatId.PoisonDamage, node, true);
                        break;
                    case "darkdamageRate":
                        metadata.SpecialStats[SpecialStatId.DarkDamage] = ParseSpecialValues(SpecialStatId.DarkDamage, node, true);
                        break;
                    case "lightdamageRate":
                        metadata.SpecialStats[SpecialStatId.HolyDamage] = ParseSpecialValues(SpecialStatId.HolyDamage, node, true);
                        break;
                    case "lddIncreaseRate":
                        metadata.SpecialStats[SpecialStatId.RangedDamage] = ParseSpecialValues(SpecialStatId.RangedDamage, node, true);
                        break;
                    case "nddIncreaseRate":
                        metadata.SpecialStats[SpecialStatId.MeleeDamage] = ParseSpecialValues(SpecialStatId.MeleeDamage, node, true);
                        break;
                    case "healRate":
                        metadata.SpecialStats[SpecialStatId.Heal] = ParseSpecialValues(SpecialStatId.Heal, node, true);
                        break;
                    case "conditionReduceRate":
                        metadata.SpecialStats[SpecialStatId.DebuffDurationReduce] = ParseSpecialValues(SpecialStatId.DebuffDurationReduce, node, true);
                        break;
                    case "receivedhealincreaseRate":
                        metadata.SpecialStats[SpecialStatId.AllyRecovery] = ParseSpecialValues(SpecialStatId.AllyRecovery, node, true);
                        break;
                    case "improve_massive_ox_expRate":
                        metadata.SpecialStats[SpecialStatId.OXQuizExp] = ParseSpecialValues(SpecialStatId.OXQuizExp, node, true);
                        break;
                    case "improve_massive_trapmaster_expRate":
                        metadata.SpecialStats[SpecialStatId.TrapMasterExp] = ParseSpecialValues(SpecialStatId.TrapMasterExp, node, true);
                        break;
                    case "improve_massive_finalsurvival_expRate":
                        metadata.SpecialStats[SpecialStatId.SoleSurvivorExp] = ParseSpecialValues(SpecialStatId.SoleSurvivorExp, node, true);
                        break;
                    case "improve_massive_crazyrunner_expRate":
                        metadata.SpecialStats[SpecialStatId.CrazyRunnerExp] = ParseSpecialValues(SpecialStatId.CrazyRunnerExp, node, true);
                        break;
                    case "improve_massive_escape_expRate":
                        metadata.SpecialStats[SpecialStatId.LudiEscapeExp] = ParseSpecialValues(SpecialStatId.LudiEscapeExp, node, true);
                        break;
                    case "improve_massive_springbeach_expRate":
                        metadata.SpecialStats[SpecialStatId.SpringBeachExp] = ParseSpecialValues(SpecialStatId.SpringBeachExp, node, true);
                        break;
                    case "improve_massive_dancedance_expRate":
                        metadata.SpecialStats[SpecialStatId.DanceDanceExp] = ParseSpecialValues(SpecialStatId.DanceDanceExp, node, true);
                        break;
                    case "improve_massive_ox_mspValue":
                        metadata.SpecialStats[SpecialStatId.OXMovementSpeed] = ParseSpecialValues(SpecialStatId.OXMovementSpeed, node, false);
                        break;
                    case "improve_massive_trapmaster_mspValue":
                        metadata.SpecialStats[SpecialStatId.TrapMasterMovementSpeed] = ParseSpecialValues(SpecialStatId.TrapMasterMovementSpeed, node, false);
                        break;
                    case "improve_massive_finalsurvival_mspValue":
                        metadata.SpecialStats[SpecialStatId.SoleSurvivorMovementSpeed] = ParseSpecialValues(SpecialStatId.SoleSurvivorMovementSpeed, node, false);
                        break;
                    case "improve_massive_crazyrunner_mspValue":
                        metadata.SpecialStats[SpecialStatId.CrazyRunnerMovementSpeed] = ParseSpecialValues(SpecialStatId.CrazyRunnerMovementSpeed, node, false);
                        break;
                    case "improve_massive_escape_mspValue":
                        metadata.SpecialStats[SpecialStatId.LudiEscapeMovementSpeed] = ParseSpecialValues(SpecialStatId.LudiEscapeMovementSpeed, node, false);
                        break;
                    case "improve_massive_springbeach_mspValue":
                        metadata.SpecialStats[SpecialStatId.SpringBeachMovementSpeed] = ParseSpecialValues(SpecialStatId.SpringBeachMovementSpeed, node, false);
                        break;
                    case "improve_massive_dancedance_mspValue":
                        metadata.SpecialStats[SpecialStatId.DanceDanceStopMovementSpeed] = ParseSpecialValues(SpecialStatId.DanceDanceStopMovementSpeed, node, false);
                        break;
                    case "npc_hit_reward_sp_ballRate":
                        metadata.SpecialStats[SpecialStatId.GenerateSpiritOrbs] = ParseSpecialValues(SpecialStatId.GenerateSpiritOrbs, node, true);
                        break;
                    case "npc_hit_reward_ep_ballRate":
                        metadata.SpecialStats[SpecialStatId.GenerateStaminaOrbs] = ParseSpecialValues(SpecialStatId.GenerateStaminaOrbs, node, true);
                        break;
                    case "improve_honor_tokenRate":
                        metadata.SpecialStats[SpecialStatId.ValorTokens] = ParseSpecialValues(SpecialStatId.ValorTokens, node, true);
                        break;
                    case "improve_pvp_expRate":
                        metadata.SpecialStats[SpecialStatId.PvPExp] = ParseSpecialValues(SpecialStatId.PvPExp, node, true);
                        break;
                    case "improve_darkstream_damageRate":
                        metadata.SpecialStats[SpecialStatId.DarkDescentDamageBonus] = ParseSpecialValues(SpecialStatId.DarkDescentDamageBonus, node, true);
                        break;
                    case "reduce_darkstream_recive_damageRate":
                        metadata.SpecialStats[SpecialStatId.DarkDescentDamageReduce] = ParseSpecialValues(SpecialStatId.DarkDescentDamageReduce, node, true);
                        break;
                    case "fishing_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleFishingMastery] = ParseSpecialValues(SpecialStatId.DoubleFishingMastery, node, true);
                        break;
                    case "playinstrument_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoublePerformanceMastery] = ParseSpecialValues(SpecialStatId.DoublePerformanceMastery, node, true);
                        break;
                    case "complete_fieldmission_mspValue":
                        metadata.SpecialStats[SpecialStatId.ExploredAreasMovementSpeed] = ParseSpecialValues(SpecialStatId.ExploredAreasMovementSpeed, node, false);
                        break;
                    case "improve_glide_vertical_velocityRate":
                        metadata.SpecialStats[SpecialStatId.AirMountAscentSpeed] = ParseSpecialValues(SpecialStatId.AirMountAscentSpeed, node, true);
                        break;
                    case "seg_fishingrewardRate":
                        metadata.SpecialStats[SpecialStatId.FishingExp] = ParseSpecialValues(SpecialStatId.FishingExp, node, true);
                        break;
                    case "seg_playinstrumentrewardRate":
                        metadata.SpecialStats[SpecialStatId.PerformanceExp] = ParseSpecialValues(SpecialStatId.PerformanceExp, node, true);
                        break;
                    case "mining_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleMiningProduction] = ParseSpecialValues(SpecialStatId.DoubleMiningProduction, node, true);
                        break;
                    case "breeding_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleRanchingProduction] = ParseSpecialValues(SpecialStatId.DoubleRanchingProduction, node, true);
                        break;
                    case "gathering_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleForagingProduction] = ParseSpecialValues(SpecialStatId.DoubleForagingProduction, node, true);
                        break;
                    case "farming_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleFarmingProduction] = ParseSpecialValues(SpecialStatId.DoubleFarmingProduction, node, true);
                        break;
                    case "blacksmithing_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleSmithingProduction] = ParseSpecialValues(SpecialStatId.DoubleSmithingProduction, node, true);
                        break;
                    case "engraving_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleHandicraftProduction] = ParseSpecialValues(SpecialStatId.DoubleHandicraftProduction, node, true);
                        break;
                    case "alchemist_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleAlchemyProduction] = ParseSpecialValues(SpecialStatId.DoubleAlchemyProduction, node, true);
                        break;
                    case "cooking_double_rewardRate":
                        metadata.SpecialStats[SpecialStatId.DoubleCookingProduction] = ParseSpecialValues(SpecialStatId.DoubleCookingProduction, node, true);
                        break;
                    case "mining_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleMiningMastery] = ParseSpecialValues(SpecialStatId.DoubleMiningMastery, node, true);
                        break;
                    case "breeding_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleRanchingMastery] = ParseSpecialValues(SpecialStatId.DoubleRanchingMastery, node, true);
                        break;
                    case "gathering_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleForagingMastery] = ParseSpecialValues(SpecialStatId.DoubleForagingMastery, node, true);
                        break;
                    case "farming_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleFarmingMastery] = ParseSpecialValues(SpecialStatId.DoubleFarmingMastery, node, true);
                        break;
                    case "blacksmithing_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleSmithingMastery] = ParseSpecialValues(SpecialStatId.DoubleSmithingMastery, node, true);
                        break;
                    case "engraving_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleHandicraftMastery] = ParseSpecialValues(SpecialStatId.DoubleHandicraftMastery, node, true);
                        break;
                    case "alchemist_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleAlchemyMastery] = ParseSpecialValues(SpecialStatId.DoubleAlchemyMastery, node, true);
                        break;
                    case "cooking_double_masteryRate":
                        metadata.SpecialStats[SpecialStatId.DoubleCookingMastery] = ParseSpecialValues(SpecialStatId.DoubleCookingMastery, node, true);
                        break;
                    case "smdRate":
                        metadata.SpecialStats[SpecialStatId.MesoBonus] = ParseSpecialValues(SpecialStatId.MesoBonus, node, true);
                        break;
                    case "npckilldropitemincrateRate":
                        metadata.SpecialStats[SpecialStatId.DropRate] = ParseSpecialValues(SpecialStatId.DropRate, node, true);
                        break;
                    case "improve_darkstream_evpValue":
                        metadata.SpecialStats[SpecialStatId.DarkDescentEvasion] = ParseSpecialValues(SpecialStatId.DarkDescentEvasion, node, false);
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

    private static List<ParserStat> ParseIntValues(StatId attribute, XmlNode node)
    {
        List<ParserStat> values = new();
        for (int i = 2; i <= 17; i++)
        {
            values.Add(new(attribute, int.Parse(node.Attributes[$"idx{i}"].Value)));
        }
        return values;
    }

    private static List<ParserStat> ParseFloatValues(StatId attribute, XmlNode node)
    {
        List<ParserStat> values = new();
        for (int i = 2; i <= 17; i++)
        {
            values.Add(new(attribute, float.Parse(node.Attributes[$"idx{i}"].Value)));
        }
        return values;
    }

    private static List<ParserSpecialStat> ParseSpecialValues(SpecialStatId attribute, XmlNode node, bool isPercent)
    {
        List<ParserSpecialStat> values = new();
        for (int i = 2; i <= 17; i++)
        {
            float value = float.Parse(node.Attributes[$"idx{i}"].Value);
            values.Add(new(attribute, isPercent ? value : 0, !isPercent ? value : 0));
        }
        return values;
    }
}
