using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class DefaultItemsParser : Exporter<List<DefaultItemsMetadata>>
{
    public DefaultItemsParser(MetadataResources resources) : base(resources, MetadataName.DefaultItems) { }

    protected override List<DefaultItemsMetadata> Parse()
    {
        List<DefaultItemsMetadata> items = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/defaultitems"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList keyNodes = document.GetElementsByTagName("key");

            Dictionary<int, List<DefaultItem>> jobDictionary = new();

            foreach (XmlNode keyNode in keyNodes)
            {
                int jobCode = int.Parse(keyNode.Attributes["jobCode"].Value);

                DefaultItemsMetadata metadata = new()
                {
                    JobCode = jobCode
                };

                foreach (XmlNode childNode in keyNode)
                {
                    _ = Enum.TryParse(childNode.Attributes["name"].Value, out ItemSlot slot);
                    foreach (XmlNode itemNode in childNode)
                    {
                        DefaultItem defaultItem = new()
                        {
                            ItemSlot = slot,
                            ItemId = int.Parse(itemNode.Attributes["id"].Value)
                        };
                        metadata.DefaultItems.Add(defaultItem);
                    }
                }

                if (jobDictionary.ContainsKey(jobCode))
                {
                    jobDictionary[jobCode].AddRange(metadata.DefaultItems);
                }
                else
                {
                    jobDictionary[jobCode] = new(metadata.DefaultItems);
                }
            }

            foreach ((int id, List<DefaultItem> defaultItems) in jobDictionary)
            {
                DefaultItemsMetadata jobMetadata = new()
                {
                    JobCode = id,
                    DefaultItems = defaultItems
                };
                items.Add(jobMetadata);
            }
        }

        return items;
    }
}
