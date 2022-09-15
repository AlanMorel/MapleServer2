using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class EnchantLimitParser : Exporter<List<EnchantLimitMetadata>>
{
    public EnchantLimitParser(MetadataResources resources) : base(resources, MetadataName.EnchantLimit) { }

    protected override List<EnchantLimitMetadata> Parse()
    {
        List<EnchantLimitMetadata> items = new();

        PackFileEntry? entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/enchantlimittable"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList? nodes = document.SelectNodes("/ms2/limit");
        if (nodes is null)
        {
            return items;
        }

        foreach (XmlNode node in nodes)
        {
            string locale = node.Attributes?["locale"]?.Value ?? "";
            if (locale != "NA" && locale != "")
            {
                continue;
            }

            if (ParserHelper.CheckForNull(node, "slot", "minLv", "maxLv", "maxGrade"))
            {
                continue;
            }

            EnchantLimitMetadata metadata = new()
            {
                ItemType = (ItemType) int.Parse(node.Attributes!["slot"]!.Value),
                MinLevel = int.Parse(node.Attributes["minLv"]!.Value),
                MaxLevel = int.Parse(node.Attributes["maxLv"]!.Value),
                MaxEnchantLevel = int.Parse(node.Attributes["maxGrade"]!.Value)
            };

            items.Add(metadata);
        }

        return items;
    }
}
