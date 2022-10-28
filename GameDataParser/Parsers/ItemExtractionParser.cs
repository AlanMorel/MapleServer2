using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemExtractionParser : Exporter<List<ItemExtractionMetadata>>
{
    public ItemExtractionParser(MetadataResources resources) : base(resources, MetadataName.ItemExtraction) { }

    protected override List<ItemExtractionMetadata> Parse()
    {
        List<ItemExtractionMetadata> palette = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/na/itemextraction"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? nodes = document.SelectNodes("/ms2/key");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "TargetItemID", "TryCount", "ScrollCount", "ResultItemID"))
                {
                    continue;
                }

                ItemExtractionMetadata metadata = new()
                {
                    SourceItemId = int.Parse(node.Attributes!["TargetItemID"]!.Value),
                    TryCount = byte.Parse(node.Attributes["TryCount"]!.Value),
                    ScrollCount = byte.Parse(node.Attributes["ScrollCount"]!.Value),
                    ResultItemId = int.Parse(node.Attributes["ResultItemID"]!.Value)
                };

                palette.Add(metadata);
            }
        }

        return palette;
    }
}
