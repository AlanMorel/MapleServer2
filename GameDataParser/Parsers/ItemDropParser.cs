using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemDropParser : Exporter<List<ItemDropMetadata>>
    {
        public ItemDropParser(MetadataResources resources) : base(resources, "item-drop") { }

        protected override List<ItemDropMetadata> Parse()
        {
            Dictionary<int, List<DropGroup>> itemGroups = new Dictionary<int, List<DropGroup>>();
            List<ItemDropMetadata> drops = new List<ItemDropMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/individualitemdrop") && !entry.Name.StartsWith("table/na/individualitemdrop"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList individualBoxItems = document.SelectNodes($"/ms2/individualDropBox");
                foreach (XmlNode node in individualBoxItems)
                {
                    string locale = string.IsNullOrEmpty(node.Attributes["locale"]?.Value) ? "" : node.Attributes["locale"].Value;

                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    int boxId = int.Parse(node.Attributes["individualDropBoxID"].Value);
                    int dropGroupId = int.Parse(node.Attributes["dropGroup"].Value);

                    DropGroupContent contents = new DropGroupContent();

                    List<int> itemIds = new List<int>();
                    itemIds.Add(int.Parse(node.Attributes["item"].Value));
                    if (node.Attributes["item2"] != null)
                    {
                        itemIds.Add(int.Parse(node.Attributes["item2"].Value));
                    }

                    if (node.Attributes["smartDropRate"] != null)
                    {
                        contents.SmartDropRate = int.Parse(node.Attributes["smartDropRate"].Value);
                    }

                    if (node.Attributes["enchantLevel"] != null)
                    {
                        contents.EnchantLevel = byte.Parse(node.Attributes["enchantLevel"].Value);
                    }

                    if (node.Attributes["isApplySmartGenderDrop"] != null)
                    {
                        contents.SmartGender = node.Attributes["isApplySmartGenderDrop"].Value.ToLower() == "true";
                    }

                    contents.MinAmount = float.Parse(node.Attributes["minCount"].Value);
                    contents.MaxAmount = float.Parse(node.Attributes["maxCount"].Value);
                    contents.Rarity = 1;
                    if (node.Attributes["PackageUIShowGrade"] != null)
                    {
                        contents.Rarity = (byte) (string.IsNullOrEmpty(node.Attributes["PackageUIShowGrade"]?.Value) ? 1 : byte.Parse(node.Attributes["PackageUIShowGrade"].Value));
                    }

                    contents.ItemIds.AddRange(itemIds);

                    if (itemGroups.ContainsKey(boxId))
                    {
                        if (itemGroups[boxId].FirstOrDefault(x => x.Id == dropGroupId) != default)
                        {
                            DropGroup group = itemGroups[boxId].FirstOrDefault(x => x.Id == dropGroupId);
                            group.Contents.Add(contents);
                            continue;
                        }

                        DropGroup newGroup = new DropGroup();
                        newGroup.Id = dropGroupId;
                        newGroup.Contents.Add(contents);
                        itemGroups[boxId].Add(newGroup);
                        continue;
                    }
                    else
                    {
                        itemGroups[boxId] = new List<DropGroup>();
                        DropGroup newGroup = new DropGroup();
                        newGroup.Id = dropGroupId;
                        newGroup.Contents.Add(contents);
                        itemGroups[boxId].Add(newGroup);
                    }
                }

                foreach (KeyValuePair<int, List<DropGroup>> kvp in itemGroups)
                {
                    ItemDropMetadata metadata = new ItemDropMetadata();
                    metadata.Id = kvp.Key;
                    metadata.DropGroups = kvp.Value;
                    drops.Add(metadata);
                }
            }
            return drops;
        }
    }
}
