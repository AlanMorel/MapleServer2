using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionRandomParser : Exporter<List<ItemOptionRandomMetadata>>
{
    public ItemOptionRandomParser(MetadataResources resources) : base(resources, "item-option-random") { }

    protected override List<ItemOptionRandomMetadata> Parse()
    {
        List<ItemOptionRandomMetadata> items = new();
        Dictionary<int, List<ItemOptionRandom>> itemOptionsRandom = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("itemoption/option/random"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
            string filename = Path.GetFileNameWithoutExtension(entry.Name);
            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes["code"]?.Value ?? "0");
                ItemOptionRandom itemOption = new();

                foreach (XmlNode item in node.Attributes)
                {
                    switch (item.Name)
                    {
                        case "grade":
                            itemOption.Rarity = byte.Parse(node.Attributes["grade"]?.Value ?? "0");
                            break;
                        case "optionNumPick":
                            itemOption.Slots = node.Attributes[item.Name].Value.Split(",").Select(byte.Parse).ToArray();
                            break;
                        case "multiply_factor":
                            itemOption.MultiplyFactor = float.Parse(node.Attributes[item.Name].Value);
                            break;
                        case "abp_rate_base":
                            itemOption.Stats.Add(new(StatId.PerfectGuard, float.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "asp_value_base":
                            itemOption.Stats.Add(new(StatId.AttackSpeed, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "atp_value_base":
                            itemOption.Stats.Add(new(StatId.Accuracy, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "bap_value_base":
                            itemOption.Stats.Add(new(StatId.BonusAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "cad_value_base":
                            itemOption.Stats.Add(new(StatId.CritDamage, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "cap_value_base":
                            itemOption.Stats.Add(new(StatId.CritRate, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "car_value_base":
                            itemOption.Stats.Add(new(StatId.CritEvasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "dex_value_base":
                            itemOption.Stats.Add(new(StatId.Dex, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "evp_value_base":
                            itemOption.Stats.Add(new(StatId.Evasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "finaladditionaldamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.TotalDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "firedamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.FireDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "firedamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.FireDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "heal_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.Heal, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "hp_rgp_value_base":
                            itemOption.Stats.Add(new(StatId.HpRegen, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "hp_value_base":
                            itemOption.Stats.Add(new(StatId.Hp, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "icedamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.IceDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "icedamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.IceDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "int_value_base":
                            itemOption.Stats.Add(new(StatId.Int, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "killhprestore_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.HpOnKill, 0, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "knockbackreduce_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.KnockbackReduce, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "lddincrease_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.RangedDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "longdistancedamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.RangedDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "lightdamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.HolyDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "lightdamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.HolyDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "luk_value_base":
                            itemOption.Stats.Add(new(StatId.Luk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "map_value_base":
                            itemOption.Stats.Add(new(StatId.MagicAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "mar_value_base":
                            itemOption.Stats.Add(new(StatId.MagicRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "marpen_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.MagicPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "msp_value_base":
                            itemOption.Stats.Add(new(StatId.MovementSpeed, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "ndd_value_base":
                            itemOption.Stats.Add(new(StatId.Defense, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "nddincrease_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.MeleeDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "neardistancedamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.MeleeDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pap_value_base":
                            itemOption.Stats.Add(new(StatId.PhysicalAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "par_value_base":
                            itemOption.Stats.Add(new(StatId.PhysicalRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "parpen_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PhysicalPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pen_rate_base":
                            itemOption.Stats.Add(new(StatId.Pierce, float.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "poisondamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PoisonDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "poisondamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PoisonDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "sgi_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.BossDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "skillcooldown_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.CooldownReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "str_value_base":
                            itemOption.Stats.Add(new(StatId.Str, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "stunreduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.StunReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "thunderdamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ElectricDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "thunderdamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ElectricDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "wapmax_value_base":
                            itemOption.Stats.Add(new(StatId.MaxWeaponAtk, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "wapmin_value_base":
                            itemOption.Stats.Add(new(StatId.MinWeaponAtk, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "bap_pet_value_base":
                            itemOption.Stats.Add(new(StatId.PetBonusAtk, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "receivedhealincrease_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.AllyRecovery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "reduce_darkstream_recive_damage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DarkDescentDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "smd_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.MesoBonus, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "sss_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.SwimSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "wapmin_rate_base":
                            itemOption.Stats.Add(new(StatId.MinWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "wapmax_rate_base":
                            itemOption.Stats.Add(new(StatId.MaxWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "pvpdamagereduce_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PvPDefense, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "pvpdamageincrease_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PvPDamage, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_pvp_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PvPExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_honor_token_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ValorTokens, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "npckilldropitemincrate_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DropRate, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_ox_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.OXQuizExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_finalsurvival_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.SoleSurvivorExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_trapmaster_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.TrapMasterExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_crazyrunner_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.CrazyRunnerExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_escape_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.LudiEscapeExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_springbeach_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.SpringBeachExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_dancedance_exp_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DanceDanceExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_ox_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.OXMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_finalsurvival_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.SoleSurvivorMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_trapmaster_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.TrapMasterMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_crazyrunner_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.CrazyRunnerMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_escape_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.LudiEscapeMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_springbeach_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.SpringBeachMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_dancedance_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DanceDanceStopMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "seg_fishingreward_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.FishingExp, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "seg_playinstrumentreward_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PerformanceExp, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "npc_hit_reward_sp_ball_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.GenerateSpiritOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "npc_hit_reward_ep_ball_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.GenerateStaminaOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "complete_fieldmission_msp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ExploredAreasMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_glide_vertical_velocity_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.AirMountAscentSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "fishing_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoubleFishingMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "playinstrument_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoublePerformanceMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "gathering_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoubleForagingMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "farming_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoubleFarmingMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "mining_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoubleMiningMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "breeding_double_mastery_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DoubleRanchingMastery, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "improve_darkstream_damage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DarkDescentDamageBonus, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_chaosraid_wap_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ChaosRaidWeaponAttack, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_chaosraid_asp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ChaosRaidAttackSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_chaosraid_atp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ChaosRaidAccuracy, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_chaosraid_hp_value_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.ChaosRaidHealth, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "darkdamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DarkDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "darkdamage_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.DarkDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pvpdamageincrease_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PvPDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pvpdamagereduce_rate_base":
                            itemOption.SpecialStats.Add(new(SpecialStatId.PvPDefense, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "additionaleffect_95000012_value_base":
                        case "additionaleffect_95000014_value_base":
                        case "hiddenbapadd_value_base":
                        case "hiddennddadd_value_base":
                        case "hiddenwapadd_value_base":
                        case "nddcalibrationfactor_rate_base":
                        case "wapcalibrationfactor_rate_base":
                        case "conditionreduce_rate_base":
                        case "sgi_target":
                        case "code":
                            break;
                    }
                }

                if (itemOptionsRandom.ContainsKey(id))
                {
                    itemOptionsRandom[id].Add(itemOption);
                }
                else
                {
                    itemOptionsRandom[id] = new()
                    {
                        itemOption
                    };
                }
            }

            foreach (KeyValuePair<int, List<ItemOptionRandom>> optionsData in itemOptionsRandom)
            {
                ItemOptionRandomMetadata metadata = new();
                metadata.Id = optionsData.Key;
                metadata.ItemOptions.AddRange(optionsData.Value);
                items.Add(metadata);
            }
        }
        return items;
    }
}
