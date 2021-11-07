using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemDropParser : Exporter<List<ItemDropMetadata>>
{
    public ItemDropParser(MetadataResources resources) : base(resources, "item-drop") { }

    protected override List<ItemDropMetadata> Parse()
    {
        Dictionary<int, List<DropGroup>> itemGroups = new();
        List<ItemDropMetadata> drops = new();
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
                string locale = node.Attributes["locale"]?.Value ?? "";

                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                int boxId = int.Parse(node.Attributes["individualDropBoxID"].Value);
                int dropGroupId = int.Parse(node.Attributes["dropGroup"].Value);

                DropGroupContent contents = new();

                List<int> itemIds = new()
                {
                    int.Parse(node.Attributes["item"].Value)
                };

                if (node.Attributes["item2"] != null)
                {
                    itemIds.Add(int.Parse(node.Attributes["item2"].Value));
                }

                contents.SmartDropRate = int.Parse(node.Attributes["smartDropRate"]?.Value ?? "0");
                contents.EnchantLevel = byte.Parse(node.Attributes["enchantLevel"]?.Value ?? "0");

                if (node.Attributes["isApplySmartGenderDrop"] != null)
                {
                    contents.SmartGender = node.Attributes["isApplySmartGenderDrop"].Value.ToLower() == "true";
                }

                contents.MinAmount = float.Parse(node.Attributes["minCount"].Value);
                contents.MaxAmount = float.Parse(node.Attributes["maxCount"].Value);
                contents.Rarity = 1;

                _ = byte.TryParse(node.Attributes["PackageUIShowGrade"]?.Value ?? "1", out contents.Rarity);

                contents.ItemIds.AddRange(itemIds);
                DropGroup newGroup = new();

                if (itemGroups.ContainsKey(boxId))
                {
                    if (itemGroups[boxId].FirstOrDefault(x => x.Id == dropGroupId) != default)
                    {
                        DropGroup group = itemGroups[boxId].FirstOrDefault(x => x.Id == dropGroupId);
                        group.Contents.Add(contents);
                        continue;
                    }

                    newGroup.Id = dropGroupId;
                    newGroup.Contents.Add(contents);
                    itemGroups[boxId].Add(newGroup);
                    continue;
                }

                itemGroups[boxId] = new();
                newGroup = new();
                newGroup.Id = dropGroupId;
                newGroup.Contents.Add(contents);
                itemGroups[boxId].Add(newGroup);

            }

            foreach (KeyValuePair<int, List<DropGroup>> kvp in itemGroups)
            {
                ItemDropMetadata metadata = new();
                metadata.Id = kvp.Key;
                metadata.DropGroups = kvp.Value;
                drops.Add(metadata);
            }
        }
        return drops;
    }
}
