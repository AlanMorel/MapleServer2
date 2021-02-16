using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class ItemParser : Exporter<List<ItemMetadata>>
    {
        public ItemParser(MetadataResources resources) : base(resources, "item") { }

        protected override List<ItemMetadata> Parse()
        {
            // Item boxes
            Dictionary<string, List<ItemContent>> itemDrops = new Dictionary<string, List<ItemContent>>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/individualitemdrop") && !entry.Name.StartsWith("table/na/individualitemdrop"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList individualBoxItems = innerDocument.SelectNodes($"/ms2/individualDropBox");
                foreach (XmlNode individualBoxItem in individualBoxItems)
                {
                    // Skip locales other than NA in table/na
                    if (entry.Name.StartsWith("table/na/individualitemdrop") && individualBoxItem.Attributes["locale"] != null)
                    {
                        if (!individualBoxItem.Attributes["locale"].Value.Equals("NA"))
                        {
                            continue;
                        }
                    }

                    if (individualBoxItem.Attributes["minCount"].Value.Contains("."))
                    {
                        continue;
                    }

                    string box = individualBoxItem.Attributes["individualDropBoxID"].Value;
                    int id = int.Parse(individualBoxItem.Attributes["item"].Value);
                    int minAmount = int.Parse(individualBoxItem.Attributes["minCount"].Value);
                    int maxAmount = int.Parse(individualBoxItem.Attributes["maxCount"].Value);
                    int dropGroup = int.Parse(individualBoxItem.Attributes["dropGroup"].Value);
                    int smartDropRate = string.IsNullOrEmpty(individualBoxItem.Attributes["smartDropRate"]?.Value) ? 0 : int.Parse(individualBoxItem.Attributes["smartDropRate"].Value);
                    int contentRarity = string.IsNullOrEmpty(individualBoxItem.Attributes["PackageUIShowGrade"]?.Value) ? 0 : int.Parse(individualBoxItem.Attributes["PackageUIShowGrade"].Value);
                    int enchant = string.IsNullOrEmpty(individualBoxItem.Attributes["enchantLevel"]?.Value) ? 0 : int.Parse(individualBoxItem.Attributes["enchantLevel"].Value);
                    int id2 = string.IsNullOrEmpty(individualBoxItem.Attributes["item2"]?.Value) ? 0 : int.Parse(individualBoxItem.Attributes["item2"].Value);

                    ItemContent content = new ItemContent(id, minAmount, maxAmount, dropGroup, smartDropRate, contentRarity, enchant, id2);
                    if (itemDrops.ContainsKey(box))
                    {
                        itemDrops[box].Add(content);
                    }
                    else
                    {
                        itemDrops[box] = new List<ItemContent>() { content };
                    }
                }
            }

            // Item breaking ingredients
            Dictionary<int, List<ItemBreakReward>> rewards = new Dictionary<int, List<ItemBreakReward>>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/itembreakingredient"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList individualItems = innerDocument.SelectNodes($"/ms2/item");
                foreach (XmlNode nodes in individualItems)
                {
                    string locale = string.IsNullOrEmpty(nodes.Attributes["locale"]?.Value) ? "" : nodes.Attributes["locale"].Value;
                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }
                    int itemID = int.Parse(nodes.Attributes["ItemID"].Value);
                    rewards[itemID] = new List<ItemBreakReward>();

                    int ingredientItemID1 = string.IsNullOrEmpty(nodes.Attributes["IngredientItemID1"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientItemID1"].Value);
                    int ingredientCount1 = string.IsNullOrEmpty(nodes.Attributes["IngredientCount1"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientCount1"].Value);
                    rewards[itemID].Add(new ItemBreakReward(ingredientItemID1, ingredientCount1));

                    int ingredientItemID2 = string.IsNullOrEmpty(nodes.Attributes["IngredientItemID2"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientItemID2"].Value);
                    int ingredientCount2 = string.IsNullOrEmpty(nodes.Attributes["IngredientCount2"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientCount2"].Value);
                    rewards[itemID].Add(new ItemBreakReward(ingredientItemID2, ingredientCount2));

                    int ingredientItemID3 = string.IsNullOrEmpty(nodes.Attributes["IngredientItemID3"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientItemID3"].Value);
                    int ingredientCount3 = string.IsNullOrEmpty(nodes.Attributes["IngredientCount3"]?.Value) ? 0 : int.Parse(nodes.Attributes["IngredientCount3"].Value);
                    rewards[itemID].Add(new ItemBreakReward(ingredientItemID3, ingredientCount3));
                }
            }

            // Items
            List<ItemMetadata> items = new List<ItemMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("item/"))
                {
                    continue;
                }

                ItemMetadata metadata = new ItemMetadata();
                string filename = Path.GetFileNameWithoutExtension(entry.Name);
                int itemId = int.Parse(filename);

                if (items.Exists(item => item.Id == itemId))
                {
                    //Console.WriteLine($"Duplicate {entry.Name} was already added.");
                    continue;
                }

                metadata.Id = itemId;
                Debug.Assert(metadata.Id > 0, $"Invalid Id {metadata.Id} from {itemId}");

                // Parse XML
                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNode item = document.SelectSingleNode("ms2/environment");

                // Gear/Cosmetic slot
                XmlNode slots = item.SelectSingleNode("slots");
                XmlNode slot = slots.FirstChild;
                bool slotResult = Enum.TryParse<ItemSlot>(slot.Attributes["name"].Value, out metadata.Slot);
                if (!slotResult && !string.IsNullOrEmpty(slot.Attributes["name"].Value))
                {
                    Console.WriteLine($"Failed to parse item slot for {itemId}: {slot.Attributes["name"].Value}");
                }
                int totalSlots = slots.SelectNodes("slot").Count;
                if (totalSlots > 1)
                {
                    if (metadata.Slot == ItemSlot.CL || metadata.Slot == ItemSlot.PA)
                    {
                        metadata.IsDress = true;
                    }
                    else if (metadata.Slot == ItemSlot.RH || metadata.Slot == ItemSlot.LH)
                    {
                        metadata.IsTwoHand = true;
                    }
                }

                // Badge slot
                XmlNode gem = item.SelectSingleNode("gem");
                bool gemResult = Enum.TryParse<GemSlot>(gem.Attributes["system"].Value, out metadata.Gem);
                if (!gemResult && !string.IsNullOrEmpty(gem.Attributes["system"].Value))
                {
                    Console.WriteLine($"Failed to parse badge slot for {itemId}: {gem.Attributes["system"].Value}");
                }

                // Inventory tab and max stack size
                XmlNode property = item.SelectSingleNode("property");
                try
                {
                    byte type = byte.Parse(property.Attributes["type"].Value);
                    byte subType = byte.Parse(property.Attributes["subtype"].Value);
                    bool skin = byte.Parse(property.Attributes["skin"].Value) != 0;
                    metadata.Tab = GetTab(type, subType, skin);
                    metadata.IsTemplate = byte.Parse(property.Attributes["skinType"]?.Value ?? "0") == 99;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to parse tab slot for {itemId}: {e.Message}");
                }
                metadata.StackLimit = int.Parse(property.Attributes["slotMax"].Value);

                // Rarity
                XmlNode option = item.SelectSingleNode("option");
                int rarity = 1;
                if (option.Attributes["constant"].Value.Length == 1)
                {
                    rarity = int.Parse(option.Attributes["constant"].Value);
                }
                metadata.Rarity = rarity;

                // Item boxes
                XmlNode function = item.SelectSingleNode("function");
                string contentType = function.Attributes["name"].Value;
                if (contentType == "OpenItemBox" || contentType == "SelectItemBox")
                {
                    // selection boxes are SelectItemBox and 1,boxid
                    // normal boxes are OpenItemBox and 0,1,0,boxid
                    // fragments are OpenItemBox and 0,1,0,boxid,required_amount
                    List<string> parameters = new List<string>(function.Attributes["parameter"].Value.Split(","));
                    // Remove empty params
                    parameters.RemoveAll(param => param.Length == 0);

                    if (parameters.Count >= 2)
                    {
                        string boxId = contentType == "OpenItemBox" ? parameters[3] : parameters[1];

                        foreach (KeyValuePair<string, List<ItemContent>> box in itemDrops) // Search for box id and set the rewards previously parsed
                        {
                            if (box.Key == boxId)
                            {
                                metadata.Content = box.Value;
                                break;
                            }
                        }
                    }
                }

                // Music score charges
                XmlNode musicScore = item.SelectSingleNode("MusicScore");
                int playCount = int.Parse(musicScore.Attributes["playCount"].Value);
                metadata.PlayCount = playCount;
                string fileName = musicScore.Attributes["fileName"].Value;
                metadata.FileName = fileName;

                XmlNode skill = item.SelectSingleNode("skill");
                int skillID = int.Parse(skill.Attributes["skillID"].Value);
                metadata.SkillID = skillID;

                XmlNode limit = item.SelectSingleNode("limit");
                bool enableBreak = byte.Parse(limit.Attributes["enableBreak"].Value) == 1;
                metadata.EnableBreak = enableBreak;

                if (!string.IsNullOrEmpty(limit.Attributes["recommendJobs"].Value))
                {
                    List<string> recommendJobs = new List<string>(limit.Attributes["recommendJobs"].Value.Split(","));
                    foreach (string recommendJob in recommendJobs)
                    {
                        metadata.RecommendJobs.Add(int.Parse(recommendJob));
                    }
                }

                // Item breaking ingredients
                if (rewards.ContainsKey(itemId))
                {
                    metadata.BreakRewards = rewards[itemId];
                }

                items.Add(metadata);
            }
            return items;
        }

        // This is an approximation and may not be 100% correct
        public static InventoryTab GetTab(byte type, byte subType, bool skin = false)
        {
            if (skin)
            {
                return InventoryTab.Outfit;
            }

            switch (type)
            {
                case 0: // Unknown
                    return InventoryTab.Misc;
                case 1:
                    return InventoryTab.Gear;
                case 2: // "Usable"
                    switch (subType)
                    {
                        case 2:
                            return InventoryTab.Consumable;
                        case 8: // Skill book for mount?
                            return InventoryTab.Mount;
                        case 14: // Emote
                            return InventoryTab.Misc;
                    }

                    break;
                case 3:
                    return InventoryTab.Quest;
                case 4:
                    return InventoryTab.Misc;
                case 5: // Air mount
                    return InventoryTab.Mount;
                case 6: // Furnishing shows up in FishingMusic
                    return InventoryTab.FishingMusic;
                case 7:
                    return InventoryTab.Badge;
                case 9: // Ground mount
                    return InventoryTab.Mount;
                case 10:
                    switch (subType)
                    {
                        case 0:
                        case 4:
                        case 5: // Ad Balloon
                        case 11: // Tail Medal
                        case 15: // Voucher
                        case 17: // Packages
                        case 18: // Packages
                        case 19:
                            return InventoryTab.Misc;
                        case 20: // Fishing Pole / Instrument
                            return InventoryTab.FishingMusic;
                    }

                    break;
                case 11:
                    return InventoryTab.Pets;
                case 12: // Music Score
                    return InventoryTab.FishingMusic;
                case 13:
                    return InventoryTab.Gemstone;
                case 14: // Gem dust
                    return InventoryTab.Catalyst;
                case 15:
                    return InventoryTab.Catalyst;
                case 16:
                    return InventoryTab.LifeSkill;
                case 19:
                    return InventoryTab.Misc;
                case 20:
                    return InventoryTab.Currency;
                case 21:
                    return InventoryTab.Currency;
                case 22: // Blueprint
                    return InventoryTab.Misc;
            }

            throw new ArgumentException($"Unknown Tab for: {type},{subType}");
        }
    }
}
