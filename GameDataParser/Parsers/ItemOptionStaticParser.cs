using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionStaticParser : Exporter<List<ItemOptionStaticMetadata>>
{
    public ItemOptionStaticParser(MetadataResources resources) : base(resources, MetadataName.ItemOptionStatic) { }

    protected override List<ItemOptionStaticMetadata> Parse()
    {
        List<ItemOptionStaticMetadata> items = new();
        Dictionary<int, List<ItemOptionsStatic>> itemOptionsStatic = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("itemoption/option/static"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodeList = innerDocument.SelectNodes("/ms2/option");
            if (nodeList is null)
            {
                continue;
            }

            foreach (XmlNode node in nodeList)
            {
                if (node.Attributes is null)
                {
                    continue;
                }

                _ = int.TryParse(node.Attributes["code"]?.Value ?? "0", out int id);
                ItemOptionsStatic optionsStatic = new();

                foreach (XmlNode item in node.Attributes)
                {
                    switch (item.Name)
                    {
                        case "grade":
                            _ = byte.TryParse(node.Attributes["grade"]?.Value ?? "0", out optionsStatic.Rarity);
                            break;
                        case "optionNumPick":
                            optionsStatic.Slots = node.Attributes[item.Name]!.Value.SplitAndParseToByte(',').ToArray();
                            break;
                        case "abp_rate_base":
                            optionsStatic.Stats.Add(new(StatAttribute.PerfectGuard, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "asp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.AttackSpeed, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "atp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Accuracy, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "bap_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.BonusAtk, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Flat));
                            break;
                        case "cad_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.CritDamage, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "cap_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.CritRate, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "car_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.CritEvasion, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "dex_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Dex, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "evp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Evasion, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "finaladditionaldamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.TotalDamage, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "firedamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.FireDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "firedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.FireDamageReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "heal_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.Heal, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "hp_rgp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.HpRegen, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Flat));
                            break;
                        case "hp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Hp, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "icedamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.IceDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "icedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.IceDamageReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "int_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Int, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "killhprestore_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.HpOnKill, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Flat));
                            break;
                        case "knockbackreduce_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.KnockbackReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "lddincrease_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.RangedDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "longdistancedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.RangedDamageReduce,
                                float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()), StatAttributeType.Rate));
                            break;
                        case "lightdamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.HolyDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "lightdamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.HolyDamageReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "luk_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Luk, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "map_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MagicAtk, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "mar_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MagicRes, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "marpen_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.MagicPiercing, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "msp_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MovementSpeed, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "ndd_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Defense, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "nddincrease_rate_base":
                            optionsStatic.SpecialStats.Add(
                                new(StatAttribute.MeleeDamage, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "neardistancedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.MeleeDamageReduce,
                                float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()), StatAttributeType.Rate));
                            break;
                        case "pap_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.PhysicalAtk, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "par_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.PhysicalRes, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "parpen_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PhysicalPiercing,
                                float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "pen_rate_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Pierce, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "poisondamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PoisonDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "poisondamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PoisonDamageReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "sgi_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.BossDamage, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "skillcooldown_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.CooldownReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "str_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.Str, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "stunreduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.StunReduce, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Rate));
                            break;
                        case "thunderdamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ElectricDamage, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "thunderdamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ElectricDamageReduce, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "wapmax_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MaxWeaponAtk, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "wapmin_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MinWeaponAtk, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "bap_pet_value_base":
                            optionsStatic.Stats.Add(new(StatAttribute.PetBonusAtk, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Flat));
                            break;
                        case "receivedhealincrease_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.AllyRecovery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "reduce_darkstream_recive_damage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DarkDescentDamageReduce,
                                float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()), StatAttributeType.Rate));
                            break;
                        case "smd_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.MesoBonus, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "sss_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.SwimSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "wapmin_rate_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MinWeaponAtk, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "wapmax_rate_base":
                            optionsStatic.Stats.Add(new(StatAttribute.MaxWeaponAtk, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "pvpdamagereduce_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PvPDefense, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "pvpdamageincrease_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PvPDamage, float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()),
                                StatAttributeType.Flat));
                            break;
                        case "improve_pvp_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PvPExp, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "improve_honor_token_rate_base":
                            optionsStatic.SpecialStats.Add(
                                new(StatAttribute.ValorTokens, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "npckilldropitemincrate_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DropRate, float.Parse(node.Attributes[item.Name]!.Value), StatAttributeType.Rate));
                            break;
                        case "improve_massive_ox_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.OXQuizExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_finalsurvival_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.SoleSurvivorExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_trapmaster_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.TrapMasterExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_crazyrunner_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.CrazyRunnerExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_escape_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.LudiEscapeExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_springbeach_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.SpringBeachExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_dancedance_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DanceDanceExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_massive_ox_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.OXMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_finalsurvival_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.SoleSurvivorMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_trapmaster_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.TrapMasterMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_crazyrunner_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.CrazyRunnerMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_escape_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.LudiEscapeMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_springbeach_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.SpringBeachMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_massive_dancedance_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DanceDanceStopMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "seg_fishingreward_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.FishingExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "seg_playinstrumentreward_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.PerformanceExp, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "npc_hit_reward_sp_ball_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.GenerateSpiritOrbs, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "npc_hit_reward_ep_ball_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.GenerateStaminaOrbs, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "complete_fieldmission_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ExploredAreasMovementSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_glide_vertical_velocity_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.AirMountAscentSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "fishing_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoubleFishingMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "playinstrument_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoublePerformanceMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "gathering_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoubleForagingMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "farming_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoubleFarmingMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "mining_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoubleMiningMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "breeding_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DoubleRanchingMastery, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_darkstream_damage_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DarkDescentDamageBonus,
                                float.Parse(node.Attributes[item.Name]!.Value.Split(",").First()), StatAttributeType.Rate));
                            break;
                        case "improve_chaosraid_wap_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ChaosRaidWeaponAttack, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Rate));
                            break;
                        case "improve_chaosraid_asp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ChaosRaidAttackSpeed, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_chaosraid_atp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ChaosRaidAccuracy, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "improve_chaosraid_hp_value_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.ChaosRaidHealth, float.Parse(node.Attributes[item.Name]!.Value),
                                StatAttributeType.Flat));
                            break;
                        case "hiddennddadd_value_base":
                            optionsStatic.HiddenDefenseAdd = int.Parse(node.Attributes[item.Name]!.Value);
                            break;
                        case "hiddenwapadd_value_base":
                            optionsStatic.HiddenWeaponAtkAdd = int.Parse(node.Attributes[item.Name]!.Value);
                            break;
                        case "nddcalibrationfactor_rate_base":
                            optionsStatic.DefenseCalibrationFactor = float.Parse(node.Attributes[item.Name]!.Value);
                            break;
                        case "wapcalibrationfactor_rate_base":
                            optionsStatic.WeaponAtkCalibrationFactor = float.Parse(node.Attributes[item.Name]!.Value);
                            break;
                        case "conditionreduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(StatAttribute.DebuffDurationReduce, float.Parse(node.Attributes[item.Name]!.Value), 0));
                            break;
                        case "additionaleffect_95000012_value_base":
                        case "additionaleffect_95000014_value_base":
                        case "sgi_target":
                        case "code":
                            break;
                    }
                }

                if (itemOptionsStatic.ContainsKey(id))
                {
                    itemOptionsStatic[id].Add(optionsStatic);
                }
                else
                {
                    itemOptionsStatic[id] = new()
                    {
                        optionsStatic
                    };
                }
            }

            foreach ((int id, List<ItemOptionsStatic> itemOptions) in itemOptionsStatic)
            {
                ItemOptionStaticMetadata metadata = new()
                {
                    Id = id,
                    ItemOptions = itemOptions
                };
                items.Add(metadata);
            }
        }

        return items;
    }
}
