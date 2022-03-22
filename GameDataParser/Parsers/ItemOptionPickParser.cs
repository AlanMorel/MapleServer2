using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Xml.Npc;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemOptionPickParser : Exporter<List<ItemOptionPickMetadata>>
{
    public ItemOptionPickParser(MetadataResources resources) : base(resources, "item-option-pick") { }

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

                List<string> constants = node.Attributes["constant_value"].Value.Split(",").ToList();
                List<string> statics = node.Attributes["static_value"].Value.Split(",").ToList();

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

                for (int i = 0; i < statics.Count; i += 2)
                {
                    if (statics[i] == "")
                    {
                        continue;
                    }
                    StaticPick staticPick = new()
                    {
                        Stat = ParseItemOptionPickStat(statics[i]),
                        DeviationValue = int.Parse(statics[i + 1])
                    };
                    optionPick.Statics.Add(staticPick);
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

    private static StatId ParseItemOptionPickStat(string stat) => stat switch
    {
        "ndd" => StatId.Defense,
        "str" => StatId.Str,
        "dex" => StatId.Dex,
        "int" => StatId.Int,
        "luk" => StatId.Luk,
        "hp" => StatId.Hp,
        "pap" => StatId.PhysicalAtk,
        "map" => StatId.MagicAtk,
        "par" => StatId.PhysicalRes,
        "mar" => StatId.MagicRes,
        "cap" => StatId.CritRate,
        "abp" => StatId.PerfectGuard,
        "wapmin" => StatId.MinWeaponAtk,
        "wapmax" => StatId.MaxWeaponAtk,
        _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, $"Unhandled stat: {stat}")
    };
}
