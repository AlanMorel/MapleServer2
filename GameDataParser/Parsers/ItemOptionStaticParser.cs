using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemOptionStaticParser : Exporter<List<ItemOptionStaticMetadata>>
    {
        public ItemOptionStaticParser(MetadataResources resources) : base(resources, "item-option-static") { }

        protected override List<ItemOptionStaticMetadata> Parse()
        {
            List<ItemOptionStaticMetadata> items = new List<ItemOptionStaticMetadata>();
            Dictionary<int, List<ItemOptionsStatic>> itemOptionsStatic = new Dictionary<int, List<ItemOptionsStatic>>();
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
                    int id = string.IsNullOrEmpty(node.Attributes["code"]?.Value) ? 0 : int.Parse(node.Attributes["code"].Value);
                    ItemOptionsStatic optionsStatic = new ItemOptionsStatic();

                    foreach (XmlNode item in node.Attributes)
                    {
                        switch (item.Name)
                        {
                            case "code":
                                break;
                            case "grade":
                                optionsStatic.Rarity = (byte) (string.IsNullOrEmpty(node.Attributes["grade"]?.Value) ? 0 : byte.Parse(node.Attributes["grade"].Value));
                                break;
                            case "optionNumPick":
                                optionsStatic.Slots = Array.ConvertAll(node.Attributes[item.Name].Value.Split(","), byte.Parse);
                                break;
                            case "abp_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PerfectGuard, float.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PerfectGuard, float.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "asp_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.AttackSpeed, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.AttackSpeed, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "atp_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Accuracy, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Accuracy, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "bap_value_base":
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.BonusAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "cad_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalDamage, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalDamage, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "cap_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalRate, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalRate, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "car_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalEvasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.CriticalEvasion, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "dex_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Dexterity, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Dexterity, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "evp_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Evasion, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Evasion, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "finaladditionaldamage_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.TotalDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.TotalDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "firedamage_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.FireDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "firedamagereduce_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.FireDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "heal_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.Heal, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.Heal, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "hp_rgp_value_base":
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.HpRegen, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "hp_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Health, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Health, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "icedamage_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.IceDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "icedamagereduce_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.IceDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "int_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Intelligence, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Intelligence, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "killhprestore_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.HpOnKill, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "knockbackreduce_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.KnockbackReduce, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "lddincrease_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.RangedDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "longdistancedamagereduce_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.RangedDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.RangedDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "lightdamage_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.HolyDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "lightdamagereduce_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.HolyDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "luk_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Luck, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Luck, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "map_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MagicalAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MagicalAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "mar_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MagicalRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MagicalRes, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "marpen_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MagicPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MagicPiercing, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "msp_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MovementSpeed, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MovementSpeed, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "ndd_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Defense, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Defense, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "nddincrease_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MeleeDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "neardistancedamagereduce_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MeleeDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MeleeDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "pap_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PhysicalAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PhysicalAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "par_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PhysicalRes, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PhysicalRes, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "parpen_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PhysicalPiercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PhysicalPiercing, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "pen_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Piercing, float.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Piercing, float.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "poisondamage_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PoisonDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "poisondamagereduce_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PoisonDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "sgi_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.BossDamage, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.BossDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "skillcooldown_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.CooldownReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "str_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Strength, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.Strength, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "stunreduce_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.StunReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.StunReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "thunderdamage_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ElectricDamage, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "thunderdamagereduce_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ElectricDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "wapmax_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MaxWeaponAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MaxWeaponAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "wapmin_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MinWeaponAtk, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MinWeaponAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "bap_pet_value_base":
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.PetBonusAtk, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "receivedhealincrease_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.AllyRecovery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "reduce_darkstream_recive_damage_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DarkDescentDamageReduce, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DarkDescentDamageReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "smd_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.MesoBonus, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "sss_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.SwimSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "wapmin_rate_base":
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MinWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "wapmax_rate_base":
                                optionsStatic.Stats.Add(new ParserStat(ItemAttribute.MaxWeaponAtk, float.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "pvpdamagereduce_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PvPDefense, 0, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PvPDefense, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "pvpdamageincrease_value_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PvPDamage, 0, int.Parse(node.Attributes[item.Name].Value.Split(",").First())));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PvPDamage, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_pvp_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PvPExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_honor_token_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ValorTokens, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "npckilldropitemincrate_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DropRate, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_ox_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.OXQuizExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_finalsurvival_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.SoleSurvivorExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_trapmaster_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.TrapMasterExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_crazyrunner_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.CrazyRunnerExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_escape_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.LudiEscapeExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_springbeach_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.SpringBeachExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_dancedance_exp_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DanceDanceExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_massive_ox_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.OXMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_finalsurvival_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.SoleSurvivorMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_trapmaster_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.TrapMasterMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_crazyrunner_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.CrazyRunnerMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_escape_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.LudiEscapeMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_springbeach_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.SpringBeachMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_massive_dancedance_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DanceDanceStopMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "seg_fishingreward_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.FishingExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "seg_playinstrumentreward_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.PerformanceExp, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "npc_hit_reward_sp_ball_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.GenerateSpiritOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "npc_hit_reward_ep_ball_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.GenerateStaminaOrbs, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "complete_fieldmission_msp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ExploredAreasMovementSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_glide_vertical_velocity_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.AirMountAscentSpeed, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "fishing_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoubleFishingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "playinstrument_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoublePerformanceMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "gathering_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoubleForagingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "farming_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoubleFarmingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "mining_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoubleMiningMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "breeding_double_mastery_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DoubleRanchingMastery, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_darkstream_damage_rate_base":
                                if (node.Attributes[item.Name].Value.Contains(','))
                                {
                                    optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DarkDescentDamageBonus, float.Parse(node.Attributes[item.Name].Value.Split(",").First()), 0));
                                    break;
                                }
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DarkDescentDamageBonus, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_chaosraid_wap_rate_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ChaosRaidWeaponAttack, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "improve_chaosraid_asp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ChaosRaidAttackSpeed, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_chaosraid_atp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ChaosRaidAccuracy, 0, int.Parse(node.Attributes[item.Name].Value)));
                                break;
                            case "improve_chaosraid_hp_value_base":
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.ChaosRaidHealth, 0, int.Parse(node.Attributes[item.Name].Value)));
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
                                optionsStatic.SpecialStats.Add(new ParserSpecialStat(SpecialItemAttribute.DebuffDurationReduce, float.Parse(node.Attributes[item.Name].Value), 0));
                                break;
                            case "additionaleffect_95000012_value_base":
                            case "additionaleffect_95000014_value_base":
                            case "sgi_target":
                                break;
                        }
                    }
                    if (itemOptionsStatic.ContainsKey(id))
                    {
                        itemOptionsStatic[id].Add(optionsStatic);
                    }
                    else
                    {
                        itemOptionsStatic[id] = new List<ItemOptionsStatic>() { optionsStatic };
                    }
                }
                foreach (KeyValuePair<int, List<ItemOptionsStatic>> optionsData in itemOptionsStatic)
                {
                    ItemOptionStaticMetadata metadata = new ItemOptionStaticMetadata();
                    metadata.Id = optionsData.Key;
                    metadata.ItemOptions.AddRange(optionsData.Value);
                    items.Add(metadata);
                }
            }
            return items;
        }
    }
}
