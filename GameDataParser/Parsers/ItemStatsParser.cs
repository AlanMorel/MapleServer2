using System.Collections.Generic;
using System.IO;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemStatsParser : Exporter<List<ItemStatsMetadata>>
    {
        public ItemStatsParser(MetadataResources resources) : base(resources, "item-stats") { }

        protected override List<ItemStatsMetadata> Parse()
        {
            List<ItemStatsMetadata> items = new List<ItemStatsMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("itemoption/constant") && !entry.Name.StartsWith("itemoption/option/random") && !entry.Name.StartsWith("itemoption/option/static"))
                {
                    continue;
                }
                if (entry.Name.Contains("petequipment.xml") || entry.Name.Contains("weaponmanual.xml") || entry.Name.Contains("armormanual.xml") || entry.Name.Contains("accmanual.xml") || entry.Name.Contains("etc.xml") || entry.Name.Contains("gemstone.xml") || entry.Name.Contains("mergematerial.xml") || entry.Name.Contains("skin.xml"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/option");
                string filename = Path.GetFileNameWithoutExtension(entry.Name);

                foreach (XmlNode node in nodeList)
                {
                    ItemStatsMetadata metadata = new ItemStatsMetadata();
                    ItemOptions optionsMetadata = new ItemOptions();
                    List<Stat> statMetadata = new List<Stat>();
                    int itemId = string.IsNullOrEmpty(node.Attributes["code"]?.Value) ? 0 : int.Parse(node.Attributes["code"].Value);
                    metadata.ItemId = itemId;

                    byte grade = string.IsNullOrEmpty(node.Attributes["grade"]?.Value) ? 0 : byte.Parse(node.Attributes["grade"].Value);
                    optionsMetadata.Grade = grade;

                    if (!string.IsNullOrEmpty(node.Attributes["optionNumPick"]?.Value))
                    {
                        List<string> list = new List<string>(node.Attributes["optionNumPick"].Value.Split(","));
                        optionsMetadata.OptionNumPick = byte.Parse(list[0]);
                    }

                    // float abp = string.IsNullOrEmpty(node.Attributes["abp_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["abp_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int asp = string.IsNullOrEmpty(node.Attributes["asp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["asp_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int atp = string.IsNullOrEmpty(node.Attributes["atp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["atp_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int bappet = string.IsNullOrEmpty(node.Attributes["bap_pet_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["bap_pet_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.PetBonusAtk, bappet));

                    int bap = string.IsNullOrEmpty(node.Attributes["bap_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["bap_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.BonusAtk, bap));

                    int cad = string.IsNullOrEmpty(node.Attributes["cad_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["cad_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.CriticalDamage, cad));

                    int cap = string.IsNullOrEmpty(node.Attributes["cap_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["cap_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.CriticalRate, cap));

                    int car = string.IsNullOrEmpty(node.Attributes["car_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["car_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.CriticalEvasion, car));

                    // float conditionreduce = string.IsNullOrEmpty(node.Attributes["conditionreduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["conditionreduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float darkdamage = string.IsNullOrEmpty(node.Attributes["darkdamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["darkdamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float darkdamagereduce = string.IsNullOrEmpty(node.Attributes["darkdamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["darkdamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int dex = string.IsNullOrEmpty(node.Attributes["dex_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["dex_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Dexterity, dex));

                    int evp = string.IsNullOrEmpty(node.Attributes["evp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["evp_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Evasion, evp));

                    // float finaladditionaldamage = string.IsNullOrEmpty(node.Attributes["finaladditionaldamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["finaladditionaldamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float firedamage = string.IsNullOrEmpty(node.Attributes["firedamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["firedamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float firedamagereduce = string.IsNullOrEmpty(node.Attributes["firedamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["firedamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float heal = string.IsNullOrEmpty(node.Attributes["heal_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["heal_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int hiddennddadd = string.IsNullOrEmpty(node.Attributes["hiddennddadd_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["hiddennddadd_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int hiddenwapadd = string.IsNullOrEmpty(node.Attributes["hiddenwapadd_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["hiddenwapadd_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int hprgp = string.IsNullOrEmpty(node.Attributes["hp_rgp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["hp_rgp_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.HpRegen, hprgp));

                    int hp = string.IsNullOrEmpty(node.Attributes["hp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["hp_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Health, hp));

                    // float icedamage = string.IsNullOrEmpty(node.Attributes["icedamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["icedamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float icedamagereduce = string.IsNullOrEmpty(node.Attributes["icedamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["icedamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int int_ = string.IsNullOrEmpty(node.Attributes["int_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["int_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Intelligence, int_));

                    int killhprestore = string.IsNullOrEmpty(node.Attributes["killhprestore_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["killhprestore_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.HpRegen, killhprestore));

                    // int knockbackreduce = string.IsNullOrEmpty(node.Attributes["knockbackreduce_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["knockbackreduce_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float lddincrease = string.IsNullOrEmpty(node.Attributes["lddincrease_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["lddincrease_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float longdistancedamagereduce = string.IsNullOrEmpty(node.Attributes["longdistancedamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["longdistancedamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float lightdamage = string.IsNullOrEmpty(node.Attributes["lightdamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["lightdamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float lightdamagereduce = string.IsNullOrEmpty(node.Attributes["lightdamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["lightdamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int luk = string.IsNullOrEmpty(node.Attributes["luk_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["luk_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Luck, luk));

                    int map = string.IsNullOrEmpty(node.Attributes["map_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["map_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.MagicalAtk, map));

                    int mar = string.IsNullOrEmpty(node.Attributes["mar_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["mar_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.MagicalRes, mar));

                    // float marpen = string.IsNullOrEmpty(node.Attributes["marpen_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["marpen_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int msp = string.IsNullOrEmpty(node.Attributes["msp_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["msp_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.MovementSpeed, msp));

                    float multiplyfactor = string.IsNullOrEmpty(node.Attributes["multiply_factor"]?.Value) ? 0 : float.Parse(node.Attributes["multiply_factor"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int ndd = string.IsNullOrEmpty(node.Attributes["ndd_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["ndd_value_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float nddcalibrationfactor = string.IsNullOrEmpty(node.Attributes["nddcalibrationfactor_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["nddcalibrationfactor_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float nddincrease = string.IsNullOrEmpty(node.Attributes["nddincrease_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["nddincrease_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float neardistancedamagereduce = string.IsNullOrEmpty(node.Attributes["neardistancedamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["neardistancedamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int pap = string.IsNullOrEmpty(node.Attributes["pap_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["pap_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.PhysicalAtk, pap));

                    int par = string.IsNullOrEmpty(node.Attributes["par_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["par_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.PhysicalRes, par));

                    // float parpen = string.IsNullOrEmpty(node.Attributes["parpen_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["parpen_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    float pen = string.IsNullOrEmpty(node.Attributes["pen_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["pen_rate_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Piercing, pen));

                    // float poisondamage = string.IsNullOrEmpty(node.Attributes["poisondamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["poisondamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float poisondamagereduce = string.IsNullOrEmpty(node.Attributes["poisondamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["poisondamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float receivedhealincrease = string.IsNullOrEmpty(node.Attributes["receivedhealincrease_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["receivedhealincrease_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int reducedarkstreamrecivedamage = string.IsNullOrEmpty(node.Attributes["reduce_darkstream_recive_damage_rate_base"]?.Value) ? 0 : int.Parse(node.Attributes["reduce_darkstream_recive_damage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float sgi = string.IsNullOrEmpty(node.Attributes["sgi_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["sgi_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // int sgitarget = string.IsNullOrEmpty(node.Attributes["sgi_target"]?.Value) ? 0 : int.Parse(node.Attributes["sgi_target"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float skillcooldown = string.IsNullOrEmpty(node.Attributes["skillcooldown_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["skillcooldown_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float smd = string.IsNullOrEmpty(node.Attributes["smd_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["smd_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float sss = string.IsNullOrEmpty(node.Attributes["sss_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["sss_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int str = string.IsNullOrEmpty(node.Attributes["str_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["str_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.Strength, str));

                    // float stunreduce = string.IsNullOrEmpty(node.Attributes["stunreduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["stunreduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float thunderdamage = string.IsNullOrEmpty(node.Attributes["thunderdamage_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["thunderdamage_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float thunderdamagereduce = string.IsNullOrEmpty(node.Attributes["thunderdamagereduce_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["thunderdamagereduce_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    // float wapcalibrationfactor = string.IsNullOrEmpty(node.Attributes["wapcalibrationfactor_rate_base"]?.Value) ? 0 : float.Parse(node.Attributes["wapcalibrationfactor_rate_base"].Value);
                    // statMetadata.Add(new StatMetadata(ItemAttribute.?, ?));

                    int wapmax = string.IsNullOrEmpty(node.Attributes["wapmax_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["wapmax_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.MaxWeaponAtk, wapmax));

                    int wapmin = string.IsNullOrEmpty(node.Attributes["wapmin_value_base"]?.Value) ? 0 : int.Parse(node.Attributes["wapmin_value_base"].Value);
                    statMetadata.Add(new Stat(ItemAttribute.MinWeaponAtk, wapmin));

                    foreach (Stat x in statMetadata)
                    {
                        if (x.Value != 0 || x.Percentage != 0)
                        {
                            optionsMetadata.Stats.Add(x);
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
                        items[index].Constant.Add(optionsMetadata);
                    }
                    else if (filename.Contains("static"))
                    {
                        items[index].Static.Add(optionsMetadata);
                    }
                    else if (filename.Contains("random"))
                    {
                        items[index].Random.Add(optionsMetadata);
                    }
                }
            }
            return items;
        }
    }
}
