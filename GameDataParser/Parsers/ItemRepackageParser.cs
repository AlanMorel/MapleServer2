using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemRepackageParser : Exporter<List<ItemRepackageMetadata>>
    {
        public ItemRepackageParser(MetadataResources resources) : base(resources, "item-repackage") { }

        protected override List<ItemRepackageMetadata> Parse()
        {
            List<ItemRepackageMetadata> items = new List<ItemRepackageMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/itemrepackingscroll"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    ItemRepackageMetadata metadata = new ItemRepackageMetadata();

                    metadata.Id = int.Parse(node.Attributes["id"].Value);
                    metadata.MinLevel = int.Parse(node.Attributes["minLv"].Value);
                    metadata.MaxLevel = int.Parse(node.Attributes["maxLv"].Value);
                    metadata.Slots = node.Attributes["slot"].Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                    metadata.PetType = int.Parse(node.Attributes["petType"]?.Value ?? "0");
                    metadata.Rarities = node.Attributes["rank"].Value.Split(",").Select(int.Parse).ToList();

                    items.Add(metadata);
                }
            }
            return items;
        }
    }
}
