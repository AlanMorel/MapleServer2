using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class AdBannerParser : Exporter<List<AdBannerMetadata>>
{
    public AdBannerParser(MetadataResources resources) : base(resources, MetadataName.AdBanner) { }

    protected override List<AdBannerMetadata> Parse()
    {
        List<AdBannerMetadata> items = new();

        PackFileEntry entry = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/na/banner"));
        if (entry is null)
        {
            return items;
        }

        // Parse XML
        XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
        XmlNodeList nodes = document.SelectNodes("/ms2/banner");

        foreach (XmlNode node in nodes)
        {
            AdBannerMetadata metadata = new()
            {
                Id = int.Parse(node.Attributes["id"].Value),
                MapId = int.Parse(node.Attributes["field"].Value),
                Prices = node.Attributes["price"].Value.SplitAndParseToInt(',').ToList(),
            };

            items.Add(metadata);
        }
        return items;
    }
}
