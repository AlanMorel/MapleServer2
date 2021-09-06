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
                    if (node.Attributes["slot"].Value != "")
                    {
                        List<string> slots = node.Attributes["slot"].Value.Split(",").ToList();
                        foreach (string slot in slots)
                        {
                            metadata.Slots.Add(int.Parse(slot));
                        }
                    }
                    if (node.Attributes["petType"] != null)
                    {
                        metadata.PetType = int.Parse(node.Attributes["petType"].Value);
                    }
                    List<string> ranks = node.Attributes["rank"].Value.Split(",").ToList();
                    foreach (string rank in ranks)
                    {
                        metadata.Rarities.Add(int.Parse(rank));
                    }
                    items.Add(metadata);
                }
            }
            return items;
        }
    }
}
