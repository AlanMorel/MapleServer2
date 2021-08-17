using System.Diagnostics;
using System.Web;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
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
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/individualitemdrop") && !entry.Name.StartsWith("table/na/individualitemdrop"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList individualBoxItems = innerDocument.SelectNodes($"/ms2/individualDropBox");
                foreach (XmlNode individualBoxItem in individualBoxItems)
                {
                    // Skip locales other than NA and null
                    string locale = string.IsNullOrEmpty(individualBoxItem.Attributes["locale"]?.Value) ? "" : individualBoxItem.Attributes["locale"].Value;

                    if (locale != "NA" && locale != "")
                    {
                        continue;
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
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/itembreakingredient"))
                {
                    continue;
                }

                XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
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

            // Item rarity
            Dictionary<int, int> rarities = new Dictionary<int, int>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/na/itemwebfinder"))
                {
                    continue;
                }
                XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList nodes = innerDocument.SelectNodes($"/ms2/key");
                foreach (XmlNode node in nodes)
                {
                    int itemId = int.Parse(node.Attributes["id"].Value);
                    int rarity = int.Parse(node.Attributes["grade"].Value);
                    rarities[itemId] = rarity;
                }
            }

            // Items
            List<ItemMetadata> items = new List<ItemMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
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
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNode item = document.SelectSingleNode("ms2/environment");

                // Tag
                XmlNode basic = item.SelectSingleNode("basic");
                metadata.Tag = basic.Attributes["stringTag"].Value;

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
                // Hair data
                if (slot.Attributes["name"].Value == "HR")
                {
                    int assetNodeCount = slot.SelectNodes("asset").Count;
                    XmlNode asset = slot.FirstChild;

                    XmlNode scaleNode = slot.SelectSingleNode("scale");

                    if (assetNodeCount == 3) // This hair has a front and back positionable hair
                    {
                        XmlNode backHair = asset.NextSibling; // back hair info
                        XmlNode frontHair = backHair.NextSibling; // front hair info

                        int backHairNodes = backHair.SelectNodes("custom").Count;

                        CoordF[] bPosCord = new CoordF[backHairNodes];
                        CoordF[] bPosRotation = new CoordF[backHairNodes];
                        CoordF[] fPosCord = new CoordF[backHairNodes];
                        CoordF[] fPosRotation = new CoordF[backHairNodes];

                        for (int i = 0; i < backHairNodes; i++)
                        {
                            foreach (XmlNode backPresets in backHair)
                            {
                                if (backPresets.Name == "custom")
                                {
                                    bPosCord[i] = CoordF.Parse(backPresets.Attributes["position"].Value);
                                    bPosRotation[i] = CoordF.Parse(backPresets.Attributes["rotation"].Value);
                                }
                            }
                            foreach (XmlNode frontPresets in frontHair)
                            {
                                if (frontPresets.Name == "custom")
                                {
                                    fPosCord[i] = CoordF.Parse(frontPresets.Attributes["position"].Value);
                                    fPosRotation[i] = CoordF.Parse(frontPresets.Attributes["position"].Value);
                                }
                            }
                            HairPresets hairPresets = new HairPresets() { };

                            hairPresets.BackPositionCoord = bPosCord[i];
                            hairPresets.BackPositionRotation = bPosRotation[i];
                            hairPresets.FrontPositionCoord = fPosCord[i];
                            hairPresets.FrontPositionRotation = fPosRotation[i];
                            if (scaleNode != null)
                            {
                                hairPresets.MinScale = float.Parse(scaleNode.Attributes["min"].Value ?? "0");
                                hairPresets.MaxScale = float.Parse(scaleNode.Attributes["max"].Value ?? "0");
                            }
                            else
                            {
                                hairPresets.MinScale = 0;
                                hairPresets.MaxScale = 0;
                            }

                            metadata.HairPresets.Add(hairPresets);
                        }
                    }
                    else if (assetNodeCount == 2) // This hair only has back positionable hair
                    {
                        XmlNode backHair = asset.NextSibling; // back hair info

                        int backHairNodes = backHair.SelectNodes("custom").Count;

                        CoordF[] bPosCord = new CoordF[backHairNodes];
                        CoordF[] bPosRotation = new CoordF[backHairNodes];

                        for (int i = 0; i < backHairNodes; i++)
                        {
                            foreach (XmlNode backPresets in backHair)
                            {
                                if (backPresets.Name == "custom")
                                {
                                    bPosCord[i] = CoordF.Parse(backPresets.Attributes["position"].Value);
                                    bPosRotation[i] = CoordF.Parse(backPresets.Attributes["rotation"].Value);
                                }
                            }

                            HairPresets hairPresets = new HairPresets() { };

                            hairPresets.BackPositionCoord = bPosCord[i];
                            hairPresets.BackPositionRotation = bPosRotation[i];
                            hairPresets.FrontPositionCoord = CoordF.Parse("0, 0, 0");
                            hairPresets.FrontPositionRotation = CoordF.Parse("0, 0, 0");
                            if (scaleNode != null)
                            {
                                hairPresets.MinScale = float.Parse(scaleNode.Attributes["min"].Value ?? "0");
                                hairPresets.MaxScale = float.Parse(scaleNode.Attributes["max"].Value ?? "0");
                            }
                            else
                            {
                                hairPresets.MinScale = 0;
                                hairPresets.MaxScale = 0;
                            }

                            metadata.HairPresets.Add(hairPresets);
                        }
                    }
                    else // hair does not have back or front positionable hair
                    {
                        HairPresets hairPresets = new HairPresets() { };
                        hairPresets.BackPositionCoord = CoordF.Parse("0, 0, 0");
                        hairPresets.BackPositionRotation = CoordF.Parse("0, 0, 0");
                        hairPresets.FrontPositionCoord = CoordF.Parse("0, 0, 0");
                        hairPresets.FrontPositionRotation = CoordF.Parse("0, 0, 0");
                        if (scaleNode != null)
                        {
                            hairPresets.MinScale = float.Parse(scaleNode.Attributes["min"].Value ?? "0");
                            hairPresets.MaxScale = float.Parse(scaleNode.Attributes["max"].Value ?? "0");
                        }
                        else
                        {
                            hairPresets.MinScale = 0;
                            hairPresets.MaxScale = 0;
                        }

                        metadata.HairPresets.Add(hairPresets);
                    }
                }


                // Color data
                XmlNode customize = item.SelectSingleNode("customize");
                metadata.ColorIndex = int.Parse(customize.Attributes["defaultColorIndex"].Value);
                metadata.ColorPalette = int.Parse(customize.Attributes["colorPalette"].Value);

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

                    // sales price
                    XmlNode sell = property.SelectSingleNode("sell");
                    metadata.SellPrice = string.IsNullOrEmpty(sell.Attributes["price"]?.Value) ? null : sell.Attributes["price"].Value.Split(',').Select(int.Parse).ToList();
                    metadata.SellPriceCustom = string.IsNullOrEmpty(sell.Attributes["priceCustom"]?.Value) ? null : sell.Attributes["priceCustom"].Value.Split(',').Select(int.Parse).ToList();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed to parse tab slot for {itemId}: {e.Message}");
                }
                metadata.StackLimit = int.Parse(property.Attributes["slotMax"].Value);

                // Rarity
                XmlNode option = item.SelectSingleNode("option");
                metadata.OptionStatic = int.Parse(option.Attributes["static"].Value);
                metadata.OptionRandom = int.Parse(option.Attributes["random"].Value);
                metadata.OptionConstant = int.Parse(option.Attributes["constant"].Value);
                metadata.OptionLevelFactor = int.Parse(option.Attributes["optionLevelFactor"].Value);

                // Item boxes
                XmlNode function = item.SelectSingleNode("function");
                string contentType = function.Attributes["name"].Value;
                metadata.FunctionData.Name = contentType;
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
                else if (contentType == "ChatEmoticonAdd")
                {
                    string rawParameter = function.Attributes["parameter"].Value;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);
                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.Id = byte.Parse(functionParameters.Attributes["id"].Value);

                    int durationSec = 0;

                    if (functionParameters.Attributes["durationSec"] != null)
                    {
                        durationSec = int.Parse(functionParameters.Attributes["durationSec"].Value);
                    }
                    metadata.FunctionData.Duration = durationSec;
                }
                else if (contentType == "OpenMassive")
                {
                    string rawParameter = function.Attributes["parameter"].Value;
                    string cleanParameter = rawParameter.Remove(1, 1); // remove the unwanted space
                    string decodedParameter = HttpUtility.HtmlDecode(cleanParameter);

                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.FieldId = int.Parse(functionParameters.Attributes["fieldID"].Value);
                    metadata.FunctionData.Duration = int.Parse(functionParameters.Attributes["portalDurationTick"].Value);
                    metadata.FunctionData.Capacity = byte.Parse(functionParameters.Attributes["maxCount"].Value);
                }
                else if (contentType == "LevelPotion")
                {
                    string rawParameter = function.Attributes["parameter"].Value;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);
                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.TargetLevel = byte.Parse(functionParameters.Attributes["targetLevel"].Value);
                }
                else if (contentType == "VIPCoupon")
                {
                    string rawParameter = function.Attributes["parameter"].Value;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);
                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.Duration = int.Parse(functionParameters.Attributes["period"].Value);
                }
                else if (contentType == "HongBao")
                {
                    string rawParameter = function.Attributes["parameter"].Value;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);
                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.Id = int.Parse(functionParameters.Attributes["itemId"].Value);
                    metadata.FunctionData.Count = short.Parse(functionParameters.Attributes["totalCount"].Value);
                    metadata.FunctionData.TotalUser = byte.Parse(functionParameters.Attributes["totalUser"].Value);
                    metadata.FunctionData.Duration = int.Parse(functionParameters.Attributes["durationSec"].Value);
                }
                else if (contentType == "SuperWorldChat")
                {
                    string[] parameters = function.Attributes["parameter"].Value.Split(",");
                    metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Not sure if the server uses the other 2. 
                }
                else if (contentType == "OpenGachaBox")
                {
                    string[] parameters = function.Attributes["parameter"].Value.Split(",");
                    metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Unknown what the second parameter is used for.
                }
                else if (contentType == "OpenCoupleEffectBox")
                {
                    string[] parameters = function.Attributes["parameter"].Value.Split(",");
                    metadata.FunctionData.Id = int.Parse(parameters[0]);
                    metadata.FunctionData.Rarity = byte.Parse(parameters[1]);
                }
                else if (contentType == "InstallBillBoard")
                {
                    AdBalloonData balloon = new AdBalloonData();
                    string rawParameter = function.Attributes["parameter"].Value;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);
                    XmlDocument xmlParameter = new XmlDocument();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    balloon.InteractId = int.Parse(functionParameters.Attributes["interactID"].Value);
                    balloon.Duration = int.Parse(functionParameters.Attributes["durationSec"].Value);
                    balloon.Model = functionParameters.Attributes["model"].Value;
                    if (functionParameters.Attributes["asset"] != null)
                    {
                        balloon.Asset = functionParameters.Attributes["asset"].Value;
                    }
                    balloon.NormalState = functionParameters.Attributes["normal"].Value;
                    balloon.Reactable = functionParameters.Attributes["reactable"].Value;
                    if (functionParameters.Attributes["scale"] != null)
                    {
                        balloon.Scale = float.Parse(functionParameters.Attributes["scale"].Value);
                    }
                    metadata.AdBalloonData = balloon;
                }
                else if (contentType == "TitleScroll" || contentType == "ItemExchangeScroll" || contentType == "OpenInstrument" || contentType == "StoryBook" || contentType == "FishingRod" || contentType == "ItemChangeBeauty")
                {
                    metadata.FunctionData.Id = int.Parse(function.Attributes["parameter"].Value);
                }

                // Music score charges
                XmlNode musicScore = item.SelectSingleNode("MusicScore");
                int playCount = int.Parse(musicScore.Attributes["playCount"].Value);
                metadata.PlayCount = playCount;
                string fileName = musicScore.Attributes["fileName"].Value;
                metadata.IsCustomScore = bool.Parse(musicScore.Attributes["isCustomNote"].Value);
                metadata.FileName = fileName;

                // Shop ID from currency items
                if (item["Shop"] != null)
                {
                    XmlNode shop = item.SelectSingleNode("Shop");
                    metadata.ShopID = int.Parse(shop.Attributes["systemShopID"].Value);
                }

                XmlNode skill = item.SelectSingleNode("skill");
                int skillID = int.Parse(skill.Attributes["skillID"].Value);
                metadata.SkillID = skillID;

                XmlNode limit = item.SelectSingleNode("limit");
                bool enableBreak = byte.Parse(limit.Attributes["enableBreak"].Value) == 1;
                metadata.EnableBreak = enableBreak;

                int level = int.Parse(limit.Attributes["levelLimit"].Value);
                metadata.Level = level;

                if (!string.IsNullOrEmpty(limit.Attributes["recommendJobs"].Value))
                {
                    List<string> recommendJobs = new List<string>(limit.Attributes["recommendJobs"].Value.Split(","));
                    foreach (string recommendJob in recommendJobs)
                    {
                        metadata.RecommendJobs.Add(int.Parse(recommendJob));
                    }
                }

                metadata.Gender = byte.Parse(limit.Attributes["genderLimit"].Value);

                XmlNode installNode = item.SelectSingleNode("install");
                bool isCubeSolid = byte.Parse(installNode.Attributes["cubeProp"].Value) == 1;
                metadata.IsCubeSolid = isCubeSolid;

                XmlNode housingNode = item.SelectSingleNode("housing");
                string value = housingNode.Attributes["categoryTag"].Value;
                if (!string.IsNullOrEmpty(value))
                {
                    List<string> categories = new List<string>(value.Split(","));
                    short category = short.Parse(categories[0]);

                    metadata.HousingCategory = (ItemHousingCategory) category;
                }

                // Item breaking ingredients
                if (rewards.ContainsKey(itemId))
                {
                    metadata.BreakRewards = rewards[itemId];
                }

                if (rarities.ContainsKey(itemId))
                {
                    metadata.Rarity = rarities[itemId];
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
                case 14: // Gem dust
                    return InventoryTab.Gemstone;
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
