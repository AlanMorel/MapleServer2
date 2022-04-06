using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemSocketParser : Exporter<List<ItemSocketMetadata>>
{
    public ItemSocketParser(MetadataResources resources) : base(resources, MetadataName.ItemSocket) { }

    protected override List<ItemSocketMetadata> Parse()
    {
        List<ItemSocketMetadata> itemSockets = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/itemsocket"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList properties = document.SelectNodes("/ms2/itemSocket");

            foreach (XmlNode property in properties)
            {
                ItemSocketMetadata metadata = new()
                {
                    Id = int.Parse(property.Attributes["id"].Value),
                    MaxCount = int.Parse(property.Attributes["maxCount"].Value),
                    FixedOpenCount = int.Parse(property.Attributes["fixOpenCount"].Value)
                };

                itemSockets.Add(metadata);
            }
        }

        return itemSockets;
    }
}
