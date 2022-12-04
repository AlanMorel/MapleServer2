using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
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
        Dictionary<int, List<ItemSocketRarityData>> socketDictionary = new();

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/itemsocket"))
            {
                continue;
            }

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList? properties = document.SelectNodes("/ms2/itemSocket");
            if (properties is null)
            {
                continue;
            }

            foreach (XmlNode property in properties)
            {
                if (ParserHelper.CheckForNull(property, "id", "grade", "maxCount", "fixOpenCount"))
                {
                    continue;
                }

                int id = int.Parse(property.Attributes!["id"]!.Value);
                ItemSocketRarityData data = new()
                {
                    Rarity = int.Parse(property.Attributes["grade"]!.Value),
                    MaxCount = int.Parse(property.Attributes["maxCount"]!.Value),
                    FixedOpenCount = int.Parse(property.Attributes["fixOpenCount"]!.Value)
                };

                if (socketDictionary.ContainsKey(id))
                {
                    socketDictionary[id].Add(data);
                }
                else
                {
                    socketDictionary[id] = new()
                    {
                        data
                    };
                }
            }
        }

        foreach ((int id, List<ItemSocketRarityData> itemSocketRarity) in socketDictionary)
        {
            ItemSocketMetadata metadata = new()
            {
                Id = id,
                RarityData = itemSocketRarity
            };
            itemSockets.Add(metadata);
        }

        return itemSockets;
    }
}
