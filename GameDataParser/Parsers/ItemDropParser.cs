﻿using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemDropParser : Exporter<List<ItemDropMetadata>>
{
    public ItemDropParser(MetadataResources resources) : base(resources, MetadataName.ItemDrop) { }

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
            XmlNodeList? individualBoxItems = document.SelectNodes("/ms2/individualDropBox");
            if (individualBoxItems is null)
            {
                continue;
            }

            foreach (XmlNode node in individualBoxItems)
            {
                if (node.Attributes is null)
                {
                    continue;
                }

                string locale = node.Attributes["locale"]?.Value ?? "";

                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(node, "individualDropBoxID", "dropGroup", "item", "minCount", "maxCount"))
                {
                    continue;
                }

                int boxId = int.Parse(node.Attributes["individualDropBoxID"]!.Value);
                int dropGroupId = int.Parse(node.Attributes["dropGroup"]!.Value);

                DropGroupContent contents = new();

                List<int> itemIds = new()
                {
                    int.Parse(node.Attributes["item"]!.Value)
                };

                if (node.Attributes["item2"] != null)
                {
                    itemIds.Add(int.Parse(node.Attributes["item2"]!.Value));
                }

                contents.SmartDropRate = int.Parse(node.Attributes["smartDropRate"]?.Value ?? "0");
                contents.EnchantLevel = byte.Parse(node.Attributes["enchantLevel"]?.Value ?? "0");

                if (node.Attributes["isApplySmartGenderDrop"] != null)
                {
                    contents.SmartGender = node.Attributes["isApplySmartGenderDrop"]!.Value.ToLower() == "true";
                }

                contents.MinAmount = float.Parse(node.Attributes["minCount"]!.Value);
                contents.MaxAmount = float.Parse(node.Attributes["maxCount"]!.Value);
                contents.Rarity = 1;

                _ = byte.TryParse(node.Attributes["PackageUIShowGrade"]?.Value ?? "0", out contents.Rarity);

                contents.ItemIds.AddRange(itemIds);
                DropGroup newGroup = new();

                if (itemGroups.ContainsKey(boxId))
                {
                    DropGroup? dropGroup = itemGroups[boxId].FirstOrDefault(x => x.Id == dropGroupId);
                    if (dropGroup is not null)
                    {
                        dropGroup.Contents.Add(contents);
                        continue;
                    }

                    newGroup.Id = dropGroupId;
                    newGroup.Contents.Add(contents);
                    itemGroups[boxId].Add(newGroup);
                    continue;
                }

                itemGroups[boxId] = new();
                newGroup = new()
                {
                    Id = dropGroupId
                };
                newGroup.Contents.Add(contents);
                itemGroups[boxId].Add(newGroup);
            }

            foreach ((int id, List<DropGroup> groups) in itemGroups)
            {
                ItemDropMetadata metadata = new()
                {
                    Id = id,
                    DropGroups = groups
                };
                drops.Add(metadata);
            }
        }

        return drops;
    }
}
