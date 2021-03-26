using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemOptionRangeParser : Exporter<List<ItemOptionRangeMetadata>>
    {
        public ItemOptionRangeParser(MetadataResources resources) : base(resources, "item-option-range") { }

        protected override List<ItemOptionRangeMetadata> Parse()
        {
            List<ItemOptionRangeMetadata> items = new List<ItemOptionRangeMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles.Where(x => x.Name.StartsWith("table/itemoptionvariation_")))
            {
                XmlDocument innerDocument = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
                string filename = Path.GetFileNameWithoutExtension(entry.Name);

                ItemOptionRangeMetadata metadata = new ItemOptionRangeMetadata();
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
                            metadata.Stats[ItemAttribute.Strength] = ParseIntValues(ItemAttribute.Strength, node);
                            break;
                        case "dexValue":
                            metadata.Stats[ItemAttribute.Dexterity] = ParseIntValues(ItemAttribute.Dexterity, node);
                            break;
                        case "intValue":
                            metadata.Stats[ItemAttribute.Intelligence] = ParseIntValues(ItemAttribute.Intelligence, node);
                            break;
                        case "lukValue":
                            metadata.Stats[ItemAttribute.Luck] = ParseIntValues(ItemAttribute.Luck, node);
                            break;
                        case "hpValue":
                            metadata.Stats[ItemAttribute.Health] = ParseIntValues(ItemAttribute.Health, node);
                            break;
                        case "aspValue":
                            metadata.Stats[ItemAttribute.AttackSpeed] = ParseIntValues(ItemAttribute.AttackSpeed, node);
                            break;
                        case "mspValue":
                            metadata.Stats[ItemAttribute.MovementSpeed] = ParseIntValues(ItemAttribute.MovementSpeed, node);
                            break;
                        case "atpValue":
                            metadata.Stats[ItemAttribute.Accuracy] = ParseIntValues(ItemAttribute.Accuracy, node);
                            break;
                        case "evpValue":
                            metadata.Stats[ItemAttribute.Evasion] = ParseIntValues(ItemAttribute.Evasion, node);
                            break;
                        case "capValue":
                            metadata.Stats[ItemAttribute.CriticalRate] = ParseIntValues(ItemAttribute.CriticalRate, node);
                            break;
                        case "cadValue":
                            metadata.Stats[ItemAttribute.CriticalDamage] = ParseIntValues(ItemAttribute.CriticalDamage, node);
                            break;
                        case "carValue":
                            metadata.Stats[ItemAttribute.CriticalEvasion] = ParseIntValues(ItemAttribute.CriticalEvasion, node);
                            break;
                        case "nddValue":
                            metadata.Stats[ItemAttribute.Defense] = ParseIntValues(ItemAttribute.Defense, node);
                            break;
                        case "papValue":
                            metadata.Stats[ItemAttribute.PhysicalAtk] = ParseIntValues(ItemAttribute.PhysicalAtk, node);
                            break;
                        case "mapValue":
                            metadata.Stats[ItemAttribute.MagicalAtk] = ParseIntValues(ItemAttribute.MagicalAtk, node);
                            break;
                        case "parValue":
                            metadata.Stats[ItemAttribute.PhysicalRes] = ParseIntValues(ItemAttribute.PhysicalRes, node);
                            break;
                        case "marValue":
                            metadata.Stats[ItemAttribute.MagicalRes] = ParseIntValues(ItemAttribute.MagicalRes, node);
                            break;
                        case "penRate":
                            metadata.Stats[ItemAttribute.Piercing] = ParseFloatValues(ItemAttribute.Piercing, node);
                            break;
                        case "sgi_BossRate":
                            metadata.SpecialStats[SpecialItemAttribute.BossDamage] = ParseSpecialValues(SpecialItemAttribute.BossDamage, node, true);
                            break;
                        case "stunReduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.StunReduce] = ParseSpecialValues(SpecialItemAttribute.StunReduce, node, true);
                            break;
                        case "killHPRestoreValue":
                            metadata.SpecialStats[SpecialItemAttribute.HpOnKill] = ParseSpecialValues(SpecialItemAttribute.HpOnKill, node, false);
                            break;
                        case "skillCooldownRate":
                            metadata.SpecialStats[SpecialItemAttribute.CooldownReduce] = ParseSpecialValues(SpecialItemAttribute.CooldownReduce, node, true);
                            break;
                        case "KnockBackReduceValue":
                            metadata.SpecialStats[SpecialItemAttribute.KnockbackReduce] = ParseSpecialValues(SpecialItemAttribute.KnockbackReduce, node, false);
                            break;
                        case "nearDistanceDamageReduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.MeleeDamageReduce] = ParseSpecialValues(SpecialItemAttribute.MeleeDamageReduce, node, true);
                            break;
                        case "longDistanceDamageReduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.RangedDamageReduce] = ParseSpecialValues(SpecialItemAttribute.RangedDamageReduce, node, true);
                            break;
                        case "finalAdditionalDamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.TotalDamage] = ParseSpecialValues(SpecialItemAttribute.TotalDamage, node, true);
                            break;
                        case "firedamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.FireDamageReduce] = ParseSpecialValues(SpecialItemAttribute.FireDamageReduce, node, true);
                            break;
                        case "icedamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.IceDamageReduce] = ParseSpecialValues(SpecialItemAttribute.IceDamageReduce, node, true);
                            break;
                        case "thunderdamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.ElectricDamageReduce] = ParseSpecialValues(SpecialItemAttribute.ElectricDamageReduce, node, true);
                            break;
                        case "poisondamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.PoisonDamageReduce] = ParseSpecialValues(SpecialItemAttribute.PoisonDamageReduce, node, true);
                            break;
                        case "darkdamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.DarkDamageReduce] = ParseSpecialValues(SpecialItemAttribute.DarkDamageReduce, node, true);
                            break;
                        case "lightdamagereduceRate":
                            metadata.SpecialStats[SpecialItemAttribute.HolyDamageReduce] = ParseSpecialValues(SpecialItemAttribute.HolyDamageReduce, node, true);
                            break;
                        case "parpenRate":
                            metadata.SpecialStats[SpecialItemAttribute.PhysicalPiercing] = ParseSpecialValues(SpecialItemAttribute.PhysicalPiercing, node, true);
                            break;
                        case "marpenRate":
                            metadata.SpecialStats[SpecialItemAttribute.MagicPiercing] = ParseSpecialValues(SpecialItemAttribute.MagicPiercing, node, true);
                            break;
                        case "firedamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.FireDamage] = ParseSpecialValues(SpecialItemAttribute.FireDamage, node, true);
                            break;
                        case "icedamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.IceDamage] = ParseSpecialValues(SpecialItemAttribute.IceDamage, node, true);
                            break;
                        case "thunderdamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.ElectricDamage] = ParseSpecialValues(SpecialItemAttribute.ElectricDamage, node, true);
                            break;
                        case "poisondamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.PoisonDamage] = ParseSpecialValues(SpecialItemAttribute.PoisonDamage, node, true);
                            break;
                        case "darkdamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.DarkDamage] = ParseSpecialValues(SpecialItemAttribute.DarkDamage, node, true);
                            break;
                        case "lightdamageRate":
                            metadata.SpecialStats[SpecialItemAttribute.HolyDamage] = ParseSpecialValues(SpecialItemAttribute.HolyDamage, node, true);
                            break;
                        case "lddIncreaseRate":
                            metadata.SpecialStats[SpecialItemAttribute.RangedDamage] = ParseSpecialValues(SpecialItemAttribute.RangedDamage, node, true);
                            break;
                        case "nddIncreaseRate":
                            metadata.SpecialStats[SpecialItemAttribute.MeleeDamage] = ParseSpecialValues(SpecialItemAttribute.MeleeDamage, node, true);
                            break;
                        case "healRate":
                            metadata.SpecialStats[SpecialItemAttribute.Heal] = ParseSpecialValues(SpecialItemAttribute.Heal, node, true);
                            break;
                        case "conditionReduceRate":
                        case "receivedhealincreaseRate":
                        case "improve_massive_ox_expRate":
                        case "improve_massive_trapmaster_expRate":
                        case "improve_massive_finalsurvival_expRate":
                        case "improve_massive_crazyrunner_expRate":
                        case "improve_massive_escape_expRate":
                        case "improve_massive_springbeach_expRate":
                        case "improve_massive_dancedance_expRate":
                        case "improve_massive_ox_mspValue":
                        case "improve_massive_trapmaster_mspValue":
                        case "improve_massive_finalsurvival_mspValue":
                        case "improve_massive_crazyrunner_mspValue":
                        case "improve_massive_escape_mspValue":
                        case "improve_massive_springbeach_mspValue":
                        case "improve_massive_dancedance_mspValue":
                        case "npc_hit_reward_sp_ballRate":
                        case "npc_hit_reward_ep_ballRate":
                        case "improve_honor_tokenRate":
                        case "improve_pvp_expRate":
                        case "improve_darkstream_damageRate":
                        case "reduce_darkstream_recive_damageRate":
                        case "improve_darkstream_evpValue":
                        case "fishing_double_masteryRate":
                        case "playinstrument_double_masteryRate":
                        case "complete_fieldmission_mspValue":
                        case "improve_glide_vertical_velocityRate":
                        case "additionaleffect_95000018Value":
                        case "additionaleffect_95000012Value":
                        case "additionaleffect_95000014Value":
                        case "seg_fishingrewardRate":
                        case "seg_playinstrumentrewardRate":
                        case "mining_double_rewardRate":
                        case "breeding_double_rewardRate":
                        case "gathering_double_rewardRate":
                        case "farming_double_rewardRate":
                        case "blacksmithing_double_rewardRate":
                        case "engraving_double_rewardRate":
                        case "alchemist_double_rewardRate":
                        case "cooking_double_rewardRate":
                        case "mining_double_masteryRate":
                        case "breeding_double_masteryRate":
                        case "gathering_double_masteryRate":
                        case "farming_double_masteryRate":
                        case "blacksmithing_double_masteryRate":
                        case "engraving_double_masteryRate":
                        case "alchemist_double_masteryRate":
                        case "cooking_double_masteryRate":
                        case "smdRate":
                        case "npckilldropitemincrateRate":
                            continue;
                    }

                }
                items.Add(metadata);
            }
            return items;
        }

        private static List<ParserStat> ParseIntValues(ItemAttribute attribute, XmlNode node)
        {
            List<ParserStat> values = new List<ParserStat>();
            for (int i = 2; i <= 17; i++)
            {
                values.Add(new ParserStat(attribute, int.Parse(node.Attributes[$"idx{i}"].Value)));
            }
            return values;
        }

        private static List<ParserStat> ParseFloatValues(ItemAttribute attribute, XmlNode node)
        {
            List<ParserStat> values = new List<ParserStat>();
            for (int i = 2; i <= 17; i++)
            {
                values.Add(new ParserStat(attribute, float.Parse(node.Attributes[$"idx{i}"].Value)));
            }
            return values;
        }

        private static List<ParserSpecialStat> ParseSpecialValues(SpecialItemAttribute attribute, XmlNode node, bool isPercent)
        {
            List<ParserSpecialStat> values = new List<ParserSpecialStat>();
            for (int i = 2; i <= 17; i++)
            {
                float value = float.Parse(node.Attributes[$"idx{i}"].Value);
                values.Add(new ParserSpecialStat(attribute, isPercent ? value : 0, !isPercent ? value : 0));
            }
            return values;
        }
    }
}
