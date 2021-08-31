using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class DefaultItemsParser : Exporter<List<DefaultItemsMetadata>>
    {
        public DefaultItemsParser(MetadataResources resources) : base(resources, "default-items") { }

        protected override List<DefaultItemsMetadata> Parse()
        {
            List<DefaultItemsMetadata> items = new List<DefaultItemsMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/defaultitems"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList keyNodes = document.GetElementsByTagName("key");

                DefaultItemsMetadata allClasses = new DefaultItemsMetadata();
                foreach (XmlNode keyNode in keyNodes)
                {
                    int jobCode = int.Parse(keyNode.Attributes["jobCode"].Value);

                    if (jobCode == 0) // for all jobs
                    {
                        foreach (XmlNode childNode in keyNode)
                        {
                            ItemSlot slot;
                            _ = Enum.TryParse(childNode.Attributes["name"].Value, out slot);
                            foreach (XmlNode itemNode in childNode)
                            {
                                DefaultItem defaultItem = new DefaultItem();
                                defaultItem.ItemSlot = slot;
                                defaultItem.ItemId = int.Parse(itemNode.Attributes["id"].Value);
                                allClasses.DefaultItems.Add(defaultItem);
                            }
                        }
                        continue;
                    }

                    DefaultItemsMetadata metadata = new DefaultItemsMetadata();
                    metadata.JobCode = jobCode;

                    foreach (XmlNode childNode in keyNode)
                    {
                        ItemSlot slot;
                        _ = Enum.TryParse<ItemSlot>(childNode.Attributes["name"].Value, out slot);
                        foreach (XmlNode itemNode in childNode)
                        {
                            DefaultItem defaultItem = new DefaultItem();
                            defaultItem.ItemSlot = slot;
                            defaultItem.ItemId = int.Parse(itemNode.Attributes["id"].Value);
                            metadata.DefaultItems.Add(defaultItem);
                        }
                    }
                    metadata.DefaultItems.AddRange(allClasses.DefaultItems);
                    items.Add(metadata);
                }
            }
            return items;
        }
    }
}
