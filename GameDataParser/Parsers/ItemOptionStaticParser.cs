using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionStaticParser : Exporter<List<ItemOptionStaticMetadata>>
{
    public ItemOptionStaticParser(MetadataResources resources) : base(resources, "item-option-static") { }

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
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
            string filename = Path.GetFileNameWithoutExtension(entry.Name);
            foreach (XmlNode node in nodeList)
            {
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
                            optionsStatic.Slots = node.Attributes[item.Name].Value.Split(",").Select(byte.Parse).ToArray();
                            break;
                        case "abp_rate_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.PerfectGuard, float.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "asp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.AttackSpeed, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "atp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Accuracy, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "bap_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.BonusAtk, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "cad_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.CriticalDamage, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "cap_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.CriticalRate, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "car_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.CriticalEvasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "dex_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Dexterity, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "evp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Evasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "finaladditionaldamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.TotalDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "firedamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.FireDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "firedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.FireDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "heal_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.Heal, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "hp_rgp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.HpRegen, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "hp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Health, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "icedamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.IceDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "icedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.IceDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "int_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Intelligence, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "killhprestore_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.HpOnKill, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "knockbackreduce_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.KnockbackReduce, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "lddincrease_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.RangedDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "longdistancedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.RangedDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "lightdamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.HolyDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "lightdamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.HolyDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "luk_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Luck, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "map_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MagicalAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "mar_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MagicalRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "marpen_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.MagicPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "msp_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MovementSpeed, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "ndd_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Defense, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "nddincrease_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.MeleeDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "neardistancedamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.MeleeDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pap_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.PhysicalAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "par_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.PhysicalRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "parpen_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PhysicalPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "pen_rate_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Piercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "poisondamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PoisonDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "poisondamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PoisonDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "sgi_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.BossDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "skillcooldown_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.CooldownReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "str_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.Strength, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "stunreduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.StunReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "thunderdamage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ElectricDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "thunderdamagereduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ElectricDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "wapmax_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MaxWeaponAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "wapmin_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MinWeaponAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "bap_pet_value_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.PetBonusAtk, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "receivedhealincrease_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.AllyRecovery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "reduce_darkstream_recive_damage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DarkDescentDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "smd_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.MesoBonus, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "sss_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.SwimSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "wapmin_rate_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MinWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "wapmax_rate_base":
                            optionsStatic.Stats.Add(new(ItemAttribute.MaxWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "pvpdamagereduce_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PvPDefense, 0, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "pvpdamageincrease_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PvPDamage, 0, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                            break;
                        case "improve_pvp_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PvPExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_honor_token_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ValorTokens, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "npckilldropitemincrate_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DropRate, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_ox_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.OXQuizExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_finalsurvival_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.SoleSurvivorExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_trapmaster_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.TrapMasterExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_crazyrunner_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.CrazyRunnerExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_escape_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.LudiEscapeExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_springbeach_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.SpringBeachExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_dancedance_exp_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DanceDanceExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_massive_ox_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.OXMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_finalsurvival_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.SoleSurvivorMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_trapmaster_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.TrapMasterMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_crazyrunner_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.CrazyRunnerMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_escape_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.LudiEscapeMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_springbeach_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.SpringBeachMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_massive_dancedance_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DanceDanceStopMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "seg_fishingreward_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.FishingExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "seg_playinstrumentreward_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.PerformanceExp, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "npc_hit_reward_sp_ball_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.GenerateSpiritOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "npc_hit_reward_ep_ball_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.GenerateStaminaOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "complete_fieldmission_msp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ExploredAreasMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_glide_vertical_velocity_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.AirMountAscentSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "fishing_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoubleFishingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "playinstrument_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoublePerformanceMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "gathering_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoubleForagingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "farming_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoubleFarmingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "mining_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoubleMiningMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "breeding_double_mastery_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DoubleRanchingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_darkstream_damage_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DarkDescentDamageBonus, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                            break;
                        case "improve_chaosraid_wap_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ChaosRaidWeaponAttack, float.Parse(node.Attributes[item.Name].Value), 0));
                            break;
                        case "improve_chaosraid_asp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ChaosRaidAttackSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_chaosraid_atp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ChaosRaidAccuracy, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "improve_chaosraid_hp_value_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.ChaosRaidHealth, 0, int.Parse(node.Attributes[item.Name].Value)));
                            break;
                        case "hiddennddadd_value_base":
                            optionsStatic.HiddenDefenseAdd = int.Parse(node.Attributes[item.Name].Value);
                            break;
                        case "hiddenwapadd_value_base":
                            optionsStatic.HiddenWeaponAtkAdd = int.Parse(node.Attributes[item.Name].Value);
                            break;
                        case "nddcalibrationfactor_rate_base":
                            optionsStatic.DefenseCalibrationFactor = float.Parse(node.Attributes[item.Name].Value);
                            break;
                        case "wapcalibrationfactor_rate_base":
                            optionsStatic.WeaponAtkCalibrationFactor = float.Parse(node.Attributes[item.Name].Value);
                            break;
                        case "conditionreduce_rate_base":
                            optionsStatic.SpecialStats.Add(new(SpecialItemAttribute.DebuffDurationReduce, float.Parse(node.Attributes[item.Name].Value), 0));
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

            foreach (KeyValuePair<int, List<ItemOptionsStatic>> optionsData in itemOptionsStatic)
            {
                ItemOptionStaticMetadata metadata = new();
                metadata.Id = optionsData.Key;
                metadata.ItemOptions.AddRange(optionsData.Value);
                items.Add(metadata);
            }
        }
        return items;
    }
}
