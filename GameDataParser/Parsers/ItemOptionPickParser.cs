using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionPickParser : Exporter<List<ItemOptionPickMetadata>>
{
    public ItemOptionPickParser(MetadataResources resources) : base(resources, MetadataName.ItemOptionPick) { }

    protected override List<ItemOptionPickMetadata> Parse()
    {
        List<ItemOptionPickMetadata> items = new();
        Dictionary<int, List<ItemOptionPick>> itemOptionPick = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/itemoptionpick"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodeList = innerDocument.SelectNodes("/ms2/itemOptionPick");
            foreach (XmlNode node in nodeList)
            {
                int id = int.Parse(node.Attributes["optionPickID"].Value);

                ItemOptionPick optionPick = new()
                {
                    Rarity = byte.Parse(node.Attributes["itemGrade"].Value)
                };

                //TODO: Add support for constant rates, random values, and random rates
                List<string> constants = node.Attributes["constant_value"].Value.Split(",").ToList();
                List<string> staticValues = node.Attributes["static_value"].Value.Split(",").ToList();
                List<string> staticRates = node.Attributes["static_rate"].Value.Split(",").ToList();

                for (int i = 0; i < constants.Count; i += 2)
                {
                    if (constants[i] == "")
                    {
                        continue;
                    }
                    ConstantPick constantPick = new()
                    {
                        Stat = ParseItemOptionPickStat(constants[i]),
                        DeviationValue = int.Parse(constants[i + 1])
                    };
                    optionPick.Constants.Add(constantPick);
                }

                for (int i = 0; i < staticValues.Count; i += 2)
                {
                    if (staticValues[i] == "")
                    {
                        continue;
                    }
                    StaticPick staticPick = new()
                    {
                        Stat = ParseItemOptionPickStat(staticValues[i]),
                        DeviationValue = int.Parse(staticValues[i + 1])
                    };
                    optionPick.StaticValues.Add(staticPick);
                }

                for (int i = 0; i < staticRates.Count; i += 2)
                {
                    if (staticRates[i] == "")
                    {
                        continue;
                    }
                    StaticPick staticPick = new()
                    {
                        Stat = ParseItemOptionPickStat(staticRates[i]),
                        DeviationValue = int.Parse(staticRates[i + 1])
                    };
                    optionPick.StaticRates.Add(staticPick);
                }

                if (itemOptionPick.ContainsKey(id))
                {
                    itemOptionPick[id].Add(optionPick);
                }
                else
                {
                    itemOptionPick[id] = new()
                    {
                        optionPick
                    };
                }
            }

            foreach ((int id, List<ItemOptionPick> itemOptions) in itemOptionPick)
            {
                ItemOptionPickMetadata metadata = new()
                {
                    Id = id,
                    ItemOptions = itemOptions
                };
                items.Add(metadata);
            }
        }
        return items;
    }

    private static StatAttribute ParseItemOptionPickStat(string stat) => stat switch
    {
        "ndd" => StatAttribute.Defense,
        "str" => StatAttribute.Str,
        "dex" => StatAttribute.Dex,
        "int" => StatAttribute.Int,
        "luk" => StatAttribute.Luk,
        "hp" => StatAttribute.Hp,
        "pap" => StatAttribute.PhysicalAtk,
        "map" => StatAttribute.MagicAtk,
        "par" => StatAttribute.PhysicalRes,
        "mar" => StatAttribute.MagicRes,
        "cap" => StatAttribute.CritRate,
        "abp" => StatAttribute.PerfectGuard,
        "wapmin" => StatAttribute.MinWeaponAtk,
        "wapmax" => StatAttribute.MaxWeaponAtk,
        _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Unhandled stat: {stat}")
    };
}
