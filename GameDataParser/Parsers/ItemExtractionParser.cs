using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemExtractionParser : Exporter<List<ItemExtractionMetadata>>
    {
        public ItemExtractionParser(MetadataResources resources) : base(resources, "item-extraction") { }

        protected override List<ItemExtractionMetadata> Parse()
        {
            List<ItemExtractionMetadata> palette = new List<ItemExtractionMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {

                if (!entry.Name.StartsWith("table/na/itemextraction"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
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
