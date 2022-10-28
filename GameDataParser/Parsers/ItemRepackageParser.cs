using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemRepackageParser : Exporter<List<ItemRepackageMetadata>>
{
    public ItemRepackageParser(MetadataResources resources) : base(resources, MetadataName.ItemRepackage) { }

    protected override List<ItemRepackageMetadata> Parse()
    {
        List<ItemRepackageMetadata> items = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/itemrepackingscroll"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            if (document.DocumentElement?.ChildNodes is null)
            {
                continue;
            }

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (ParserHelper.CheckForNull(node, "id", "minLv", "maxLv", "slot", "rank"))
                {
                    continue;
                }

                ItemRepackageMetadata metadata = new()
                {
                    Id = int.Parse(node.Attributes!["id"]!.Value),
                    MinLevel = int.Parse(node.Attributes["minLv"]!.Value),
                    MaxLevel = int.Parse(node.Attributes["maxLv"]!.Value),
                    Slots = node.Attributes["slot"]!.Value.SplitAndParseToInt(',').ToList(),
                    PetType = int.Parse(node.Attributes["petType"]?.Value ?? "0"),
                    Rarities = node.Attributes["rank"]!.Value.SplitAndParseToInt(',').ToList()
                };

                items.Add(metadata);
            }
        }

        return items;
    }
}
