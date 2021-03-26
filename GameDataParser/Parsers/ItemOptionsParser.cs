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
    public class ItemOptionsParser : Exporter<List<ItemOptionsMetadata>>
    {
        public ItemOptionsParser(MetadataResources resources) : base(resources, "item-options") { }

        protected override List<ItemOptionsMetadata> Parse()
        {
            List<ItemOptionsMetadata> items = new List<ItemOptionsMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("itemoption/constant") && !entry.Name.StartsWith("itemoption/option/random") && !entry.Name.StartsWith("itemoption/option/static"))
                {
                    continue;
                }
                if (entry.Name.Contains("etc.xml") || entry.Name.Contains("mergematerial.xml"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
                string filename = Path.GetFileNameWithoutExtension(entry.Name);

                foreach (XmlNode node in nodeList)
                {
                    ItemOptionsMetadata metadata = new ItemOptionsMetadata();
                    ItemOption optionsMetadata = new ItemOption();
                    int itemId = string.IsNullOrEmpty(node.Attributes["code"]?.Value) ? 0 : int.Parse(node.Attributes["code"].Value);

                    if (!string.IsNullOrEmpty(node.Attributes["optionNumPick"]?.Value))
                    {
                        List<string> list = new List<string>(node.Attributes["optionNumPick"].Value.Split(","));
                        optionsMetadata.Slots = byte.Parse(list[0]);
                    }

                    // Skip item with 0 slots
                    if ((entry.Name.Contains("random") || entry.Name.Contains("static")) && optionsMetadata.Slots == 0)
                    {
                        continue;
                    }

                    foreach (XmlNode item in node.Attributes)
                    {
                        switch (item.Name)
                        {
                            case "code":
                                metadata.ItemId = itemId;
                                break;
                            case "grade":
                                optionsMetadata.Rarity = (byte) (string.IsNullOrEmpty(node.Attributes["grade"]?.Value) ? 0 : byte.Parse(node.Attributes["grade"].Value));
                                break;
                            case "multiply_factor":
                                optionsMetadata.MultiplyFactor = string.IsNullOrEmpty(node.Attributes["multiply_factor"]?.Value) ? 0 : float.Parse(node.Attributes["multiply_factor"].Value);
                                break;
                            case "abp_rate_base":
                                optionsMetadata.Stats.Add(ItemAttribute.PerfectGuard);
                                break;
                            case "asp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.AttackSpeed);
                                break;
                            case "atp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Accuracy);
                                break;
                            case "bap_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.BonusAtk);
                                break;
                            case "cad_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.CriticalDamage);
                                break;
                            case "cap_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.CriticalRate);
                                break;
                            case "car_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.CriticalEvasion);
                                break;
                            case "darkdamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.DarkDamage);
                                break;
                            case "darkdamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.DarkDamageReduce);
                                break;
                            case "dex_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Dexterity);
                                break;
                            case "evp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Evasion);
                                break;
                            case "finaladditionaldamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.TotalDamage);
                                break;
                            case "firedamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.FireDamage);
                                break;
                            case "firedamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.FireDamageReduce);
                                break;
                            case "heal_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.Heal);
                                break;
                            case "hp_rgp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.HpRegen);
                                break;
                            case "hp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Health);
                                break;
                            case "icedamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.IceDamage);
                                break;
                            case "icedamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.IceDamageReduce);
                                break;
                            case "int_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Intelligence);
                                break;
                            case "killhprestore_value_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.HpOnKill);
                                break;
                            case "knockbackreduce_value_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.KnockbackReduce);
                                break;
                            case "lddincrease_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.RangedDamage);
                                break;
                            case "longdistancedamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.RangedDamageReduce);
                                break;
                            case "lightdamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.HolyDamage);
                                break;
                            case "lightdamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.HolyDamageReduce);
                                break;
                            case "luk_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Luck);
                                break;
                            case "map_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.MagicalAtk);
                                break;
                            case "mar_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.MagicalRes);
                                break;
                            case "marpen_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.MagicPiercing);
                                break;
                            case "msp_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.MovementSpeed);
                                break;
                            case "ndd_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Defense);
                                break;
                            case "nddincrease_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.MeleeDamage);
                                break;
                            case "neardistancedamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.MeleeDamageReduce);
                                break;
                            case "pap_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.PhysicalAtk);
                                break;
                            case "par_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.PhysicalRes);
                                break;
                            case "parpen_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.PhysicalPiercing);
                                break;
                            case "pen_rate_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Piercing);
                                break;
                            case "poisondamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.PoisonDamage);
                                break;
                            case "poisondamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.PoisonDamageReduce);
                                break;
                            case "sgi_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.BossDamage);
                                break;
                            case "skillcooldown_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.CooldownReduce);
                                break;
                            case "str_value_base":
                                optionsMetadata.Stats.Add(ItemAttribute.Strength);
                                break;
                            case "stunreduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.StunReduce);
                                break;
                            case "thunderdamage_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.ElectricDamage);
                                break;
                            case "thunderdamagereduce_rate_base":
                                optionsMetadata.SpecialStats.Add(SpecialItemAttribute.ElectricDamageReduce);
                                break;
                            case "wapmax_value_base":
                                string wapMax = node.Attributes["wapmax_value_base"]?.Value;
                                if (!string.IsNullOrEmpty(wapMax))
                                {
                                    if (wapMax.Contains(","))
                                    {
                                        List<int> list = node.Attributes["wapmax_value_base"].Value.Split(",").Select(int.Parse).ToList();
                                        optionsMetadata.MaxWeaponAtk = list[0];
                                    }
                                    else
                                    {
                                        optionsMetadata.MaxWeaponAtk = int.Parse(wapMax);
                                    }
                                }
                                break;
                            case "wapmin_value_base":
                                int wapmin = string.IsNullOrEmpty(node.Attributes["wapmin_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["wapmin_value_base"].Value);
                                optionsMetadata.MinWeaponAtk = wapmin;
                                break;
                            case "bap_pet_value_base":
                                int petatk = string.IsNullOrEmpty(node.Attributes["bap_pet_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["bap_pet_value_base"].Value);
                                optionsMetadata.PetAtk = petatk;
                                break;
                            case "hiddennddadd_value_base":
                            case "hiddenwapadd_value_base":
                            case "nddcalibrationfactor_rate_base":
                            case "receivedhealincrease_rate_base":
                            case "reduce_darkstream_recive_damage_rate_base":
                            case "smd_rate_base":
                            case "sss_rate_base":
                            case "wapcalibrationfactor_rate_base":
                            case "conditionreduce_rate_base":
                            case "sgi_target":
                            case "wapmin_rate_base":
                            case "wapmax_rate_base":
                            case "hiddenbapadd_value_base":
                            case "pvpdamagereduce_value_base":
                            case "pvpdamageincrease_value_base":
                            case "locale":
                            case "improve_pvp_exp_rate_base":
                            case "improve_honor_token_rate_base":
                            case "optionNumPick":
                            case "npckilldropitemincrate_rate_base":
                            case "improve_massive_ox_exp_rate_base":
                            case "improve_massive_finalsurvival_exp_rate_base":
                            case "improve_massive_trapmaster_exp_rate_base":
                            case "improve_massive_crazyrunner_exp_rate_base":
                            case "improve_massive_escape_exp_rate_base":
                            case "improve_massive_springbeach_exp_rate_base":
                            case "improve_massive_dancedance_exp_rate_base":
                            case "improve_massive_ox_msp_value_base":
                            case "improve_massive_finalsurvival_msp_value_base":
                            case "improve_massive_trapmaster_msp_value_base":
                            case "improve_massive_crazyrunner_msp_value_base":
                            case "improve_massive_escape_msp_value_base":
                            case "improve_massive_springbeach_msp_value_base":
                            case "improve_massive_dancedance_msp_value_base":
                            case "seg_fishingreward_rate_base":
                            case "seg_playinstrumentreward_rate_base":
                            case "npc_hit_reward_sp_ball_rate_base":
                            case "npc_hit_reward_ep_ball_rate_base":
                            case "additionaleffect_95000012_value_base":
                            case "additionaleffect_95000014_value_base":
                            case "complete_fieldmission_msp_value_base":
                            case "improve_glide_vertical_velocity_rate_base":
                            case "fishing_double_mastery_rate_base":
                            case "playinstrument_double_mastery_rate_base":
                            case "gathering_double_mastery_rate_base":
                            case "farming_double_mastery_rate_base":
                            case "mining_double_mastery_rate_base":
                            case "breeding_double_mastery_rate_base":
                            case "improve_darkstream_damage_rate_base":
                                break;
                        }
                    }

                    int index = 0;
                    if (items.Exists(x => itemId == x.ItemId))
                    {
                        index = items.FindIndex(x => itemId == x.ItemId);
                    }
                    else
                    {
                        items.Add(metadata);
                        index = items.Count - 1;
                    }

                    if (filename.Contains("constant"))
                    {
                        items[index].Basic.Add(optionsMetadata);
                    }
                    else if (filename.Contains("static"))
                    {
                        items[index].StaticBonus.Add(optionsMetadata);
                    }
                    else if (filename.Contains("random"))
                    {
                        items[index].RandomBonus.Add(optionsMetadata);
                    }
                }
            }
            return items;
        }
    }
}
