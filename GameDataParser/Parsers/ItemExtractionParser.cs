using System.Collections.Generic;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemExtractionParser : Exporter<List<ItemExtractionMetadata>>
    {
        public ItemExtractionParser(MetadataResources resources) : base(resources, "item-extraction") { }

        protected override List<ItemExtractionMetadata> Parse()
        {
            List<ItemExtractionMetadata> palette = new List<ItemExtractionMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {

                if (!entry.Name.StartsWith("table/na/itemextraction"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    ItemExtractionMetadata metadata = new ItemExtractionMetadata();

                    if (node.Name == "key")
                    {
                        metadata.SourceItemId = int.Parse(node.Attributes["TargetItemID"].Value);
                        metadata.TryCount = byte.Parse(node.Attributes["TryCount"].Value);
                        metadata.ScrollCount = byte.Parse(node.Attributes["ScrollCount"].Value);
                        metadata.ResultItemId = int.Parse(node.Attributes["ResultItemID"].Value);
                    }
                    palette.Add(metadata);
                }
            }
            return palette;
        }
    }
}
