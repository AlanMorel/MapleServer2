using System.Diagnostics;
using System.Web;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace GameDataParser.Parsers;

public class ItemParser : Exporter<List<ItemMetadata>>
{
    public ItemParser(MetadataResources resources) : base(resources, "item") { }

    protected override List<ItemMetadata> Parse()
    {
        // Item breaking ingredients
        Dictionary<int, List<ItemBreakReward>> rewards = ParseItemBreakingIngredients();

        //Item Name
        Dictionary<int, string> names = ParseItemNames();

        // Item rarity
        Dictionary<int, int> rarities = ParseItemRarities();

        // Items
        List<ItemMetadata> items = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("item/"))
            {
                continue;
            }

            ItemMetadata metadata = new();
            string filename = Path.GetFileNameWithoutExtension(entry.Name);
            int itemId = int.Parse(filename);

            if (items.Exists(item => item.Id == itemId))
            {
                continue;
            }

            metadata.Id = itemId;
            Debug.Assert(metadata.Id > 0, $"Invalid Id {metadata.Id} from {itemId}");

            // Parse XML
            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

            XmlNodeList environments = document.SelectNodes("ms2/environment");
            XmlNode item = environments[0];

            foreach (XmlNode environment in environments)
            {
                // If there is an environment with locale "NA", use that.
                if (environment.Attributes["locale"]?.Value == "NA")
                {
                    item = environment;
                }
            }

            // Tag
            XmlNode basic = item.SelectSingleNode("basic");
            metadata.Tag = basic.Attributes["stringTag"].Value;

            // Gear/Cosmetic slot
            XmlNode slots = item.SelectSingleNode("slots");
            XmlNode slot = slots.FirstChild;
            bool slotResult = Enum.TryParse(slot.Attributes["name"].Value, out metadata.Slot);
            if (!slotResult && !string.IsNullOrEmpty(slot.Attributes["name"].Value))
            {
                Console.WriteLine($"Failed to parse item slot for {itemId}: {slot.Attributes["name"].Value}");
            }

            int totalSlots = slots.SelectNodes("slot").Count;
            if (totalSlots > 1)
            {
                switch (metadata.Slot)
                {
                    case ItemSlot.CL or ItemSlot.PA:
                        metadata.IsDress = true;
                        break;
                    case ItemSlot.RH or ItemSlot.LH:
                        metadata.IsTwoHand = true;
                        break;
                }
            }

            // Hair data
            if (slot.Attributes["name"].Value == "HR")
            {
                ParseHair(slot, metadata);
            }

            // Color data
            XmlNode customize = item.SelectSingleNode("customize");
            metadata.ColorIndex = int.Parse(customize.Attributes["defaultColorIndex"].Value);
            metadata.ColorPalette = int.Parse(customize.Attributes["colorPalette"].Value);

            // Badge slot
            XmlNode gem = item.SelectSingleNode("gem");
            bool gemResult = Enum.TryParse(gem.Attributes["system"].Value, out metadata.Gem);
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
                metadata.TradeableCount = byte.Parse(property.Attributes["tradableCount"].Value);
                metadata.RepackageCount = byte.Parse(property.Attributes["rePackingLimitCount"].Value);
                metadata.RepackageItemConsumeCount = byte.Parse(property.Attributes["rePackingItemConsumeCount"].Value);
                metadata.BlackMarketCategory = property.Attributes["blackMarketCategory"].Value;

                // sales price
                XmlNode sell = property.SelectSingleNode("sell");
                metadata.SellPrice = sell.Attributes["price"]?.Value.Split(',').Select(int.Parse).ToList() ?? null;
                metadata.SellPriceCustom = sell.Attributes["priceCustom"]?.Value.Split(',').Select(int.Parse).ToList() ?? null;
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

            XmlNode function = item.SelectSingleNode("function");
            string contentType = function.Attributes["name"].Value;
            metadata.FunctionData.Name = contentType;

            // Item boxes
            switch (contentType)
            {
                // selection boxes are SelectItemBox and 1,boxid
                // normal boxes are OpenItemBox and 0,1,0,boxid
                // fragments are OpenItemBox and 0,1,0,boxid,required_amount
                case "OpenItemBox" when function.Attributes["parameter"].Value.Contains('l'):
                    continue; // TODO: Implement these CN items. Skipping for now
                case "OpenItemBox":
                    {
                        List<string> parameters = new(function.Attributes["parameter"].Value.Split(","));
                        OpenItemBox box = new()
                        {
                            RequiredItemId = int.Parse(parameters[0]),
                            ReceiveOneItem = parameters[1] == "1",
                            BoxId = int.Parse(parameters[3]),
                            AmountRequired = 1
                        };
                        if (parameters.Count == 5)
                        {
                            box.AmountRequired = int.Parse(parameters[4]);
                        }

                        metadata.FunctionData.OpenItemBox = box;
                        break;
                    }
                case "SelectItemBox" when function.Attributes["parameter"].Value.Contains('l'):
                    continue; // TODO: Implement these CN items. Skipping for now
                case "SelectItemBox":
                    {
                        List<string> parameters = new(function.Attributes["parameter"].Value.Split(","));
                        parameters.RemoveAll(param => param.Length == 0);
                        SelectItemBox box = new()
                        {
                            GroupId = int.Parse(parameters[0]),
                            BoxId = int.Parse(parameters[1])
                        };
                        metadata.FunctionData.SelectItemBox = box;
                        break;
                    }
                case "ChatEmoticonAdd":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        ChatEmoticonAdd sticker = new()
                        {
                            Id = byte.Parse(functionParameters.Attributes["id"].Value),
                            Duration = int.Parse(functionParameters.Attributes["durationSec"]?.Value ?? "0")
                        };

                        metadata.FunctionData.ChatEmoticonAdd = sticker;
                        break;
                    }
                case "OpenMassive":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string cleanParameter = rawParameter.Remove(1, 1); // remove the unwanted space
                        string decodedParameter = HttpUtility.HtmlDecode(cleanParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        OpenMassiveEvent massiveEvent = new()
                        {
                            FieldId = int.Parse(functionParameters.Attributes["fieldID"].Value),
                            Duration = int.Parse(functionParameters.Attributes["portalDurationTick"].Value),
                            Capacity = byte.Parse(functionParameters.Attributes["maxCount"].Value)
                        };
                        metadata.FunctionData.OpenMassiveEvent = massiveEvent;
                        break;
                    }
                case "LevelPotion":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        LevelPotion levelPotion = new()
                        {
                            TargetLevel = byte.Parse(functionParameters.Attributes["targetLevel"].Value)
                        };
                        metadata.FunctionData.LevelPotion = levelPotion;
                        break;
                    }
                case "VIPCoupon":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        VIPCoupon coupon = new()
                        {
                            Duration = int.Parse(functionParameters.Attributes["period"].Value)
                        };
                        metadata.FunctionData.VIPCoupon = coupon;
                        break;
                    }
                case "HongBao":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        HongBaoData hongBao = new()
                        {
                            Id = int.Parse(functionParameters.Attributes["itemId"].Value),
                            Count = short.Parse(functionParameters.Attributes["totalCount"].Value),
                            TotalUsers = byte.Parse(functionParameters.Attributes["totalUser"].Value),
                            Duration = int.Parse(functionParameters.Attributes["durationSec"].Value)
                        };
                        metadata.FunctionData.HongBao = hongBao;
                        break;
                    }
                case "SuperWorldChat":
                    {
                        string[] parameters = function.Attributes["parameter"].Value.Split(",");
                        metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Not sure if the server uses the other 2. 
                        break;
                    }
                case "OpenGachaBox":
                    {
                        string[] parameters = function.Attributes["parameter"].Value.Split(",");
                        metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Unknown what the second parameter is used for.
                        break;
                    }
                case "OpenCoupleEffectBox":
                    {
                        string[] parameters = function.Attributes["parameter"].Value.Split(",");
                        OpenCoupleEffectBox box = new()
                        {
                            Id = int.Parse(parameters[0]),
                            Rarity = byte.Parse(parameters[1])
                        };
                        metadata.FunctionData.OpenCoupleEffectBox = box;
                        break;
                    }
                case "InstallBillBoard":
                    {
                        string rawParameter = function.Attributes["parameter"].Value;
                        string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                        XmlDocument xmlParameter = new();
                        xmlParameter.LoadXml(decodedParameter);
                        XmlNode functionParameters = xmlParameter.SelectSingleNode("v");

                        InstallBillboard balloon = new()
                        {
                            InteractId = int.Parse(functionParameters.Attributes["interactID"].Value),
                            Duration = int.Parse(functionParameters.Attributes["durationSec"].Value),
                            Model = functionParameters.Attributes["model"].Value,
                            Asset = functionParameters.Attributes["asset"]?.Value ?? "",
                            NormalState = functionParameters.Attributes["normal"].Value,
                            Reactable = functionParameters.Attributes["reactable"].Value,
                            Scale = float.Parse(functionParameters.Attributes["scale"]?.Value ?? "0")
                        };
                        metadata.FunctionData.InstallBillboard = balloon;
                        break;
                    }
                case "TitleScroll":
                case "ItemExchangeScroll":
                case "OpenInstrument":
                case "StoryBook":
                case "FishingRod":
                case "ItemChangeBeauty":
                case "ItemRePackingScroll":
                    metadata.FunctionData.Id = int.Parse(function.Attributes["parameter"].Value);
                    break;
            }

            // Music score charges
            XmlNode musicScore = item.SelectSingleNode("MusicScore");
            metadata.PlayCount = int.Parse(musicScore.Attributes["playCount"].Value);
            metadata.FileName = musicScore.Attributes["fileName"].Value;
            metadata.IsCustomScore = bool.Parse(musicScore.Attributes["isCustomNote"].Value);

            // Shop ID from currency items
            if (item["Shop"] != null)
            {
                XmlNode shop = item.SelectSingleNode("Shop");
                metadata.ShopID = int.Parse(shop.Attributes["systemShopID"].Value);
            }

            XmlNode skill = item.SelectSingleNode("skill");
            metadata.SkillID = int.Parse(skill.Attributes["skillID"].Value);

            XmlNode limit = item.SelectSingleNode("limit");
            metadata.EnableBreak = byte.Parse(limit.Attributes["enableBreak"].Value) == 1;
            metadata.Level = int.Parse(limit.Attributes["levelLimit"].Value);
            metadata.TransferType = (TransferType) byte.Parse(limit.Attributes["transferType"].Value);
            metadata.Sellable = byte.Parse(limit.Attributes["shopSell"].Value) == 1;
            metadata.RecommendJobs = limit.Attributes["recommendJobs"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
            metadata.Gender = (Gender) byte.Parse(limit.Attributes["genderLimit"].Value);

            XmlNode installNode = item.SelectSingleNode("install");
            metadata.IsCubeSolid = byte.Parse(installNode.Attributes["cubeProp"].Value) == 1;
            metadata.ObjectId = int.Parse(installNode.Attributes["objCode"].Value);

            XmlNode housingNode = item.SelectSingleNode("housing");
            string value = housingNode.Attributes["categoryTag"]?.Value;
            if (value is not null)
            {
                List<string> categories = new(value.Split(","));
                _ = short.TryParse(categories[0], out short category);

                metadata.HousingCategory = (ItemHousingCategory) category;
            }

            // Item breaking ingredients
            if (rewards.ContainsKey(itemId))
            {
                metadata.BreakRewards = rewards[itemId];
            }

            // Item rarities
            if (rarities.ContainsKey(itemId))
            {
                metadata.Rarity = rarities[itemId];
            }

            // Item Names
            if (names.ContainsKey(itemId))
            {
                metadata.Name = names[itemId];
            }

            items.Add(metadata);
        }

        return items;
    }

    private Dictionary<int, int> ParseItemRarities()
    {
        Dictionary<int, int> rarities = new();
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

        return rarities;
    }

    private Dictionary<int, string> ParseItemNames()
    {
        Dictionary<int, string> names = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("string/en/itemname.xml"))
            {
                continue;
            }

            XmlDocument innerDocument = Resources.XmlReader.GetXmlDocument(entry);
            XmlNodeList nodes = innerDocument.SelectNodes($"/ms2/key");
            foreach (XmlNode node in nodes)
            {
                int itemId = int.Parse(node.Attributes["id"].Value);
                if (node.Attributes["name"] == null)
                {
                    continue;
                }

                string itemName = node.Attributes["name"].Value;
                names[itemId] = itemName;
            }
        }

        return names;
    }

    private Dictionary<int, List<ItemBreakReward>> ParseItemBreakingIngredients()
    {
        Dictionary<int, List<ItemBreakReward>> rewards = new();
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
                string locale = nodes.Attributes["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                int itemID = int.Parse(nodes.Attributes["ItemID"].Value);
                rewards[itemID] = new();

                int ingredientItemID1 = int.Parse(nodes.Attributes["IngredientItemID1"]?.Value ?? "0");
                int ingredientCount1 = int.Parse(nodes.Attributes["IngredientCount1"]?.Value ?? "0");
                rewards[itemID].Add(new(ingredientItemID1, ingredientCount1));

                _ = int.TryParse(nodes.Attributes["IngredientItemID2"]?.Value ?? "0", out int ingredientItemID2);
                _ = int.TryParse(nodes.Attributes["IngredientCount2"]?.Value ?? "0", out int ingredientCount2);
                rewards[itemID].Add(new(ingredientItemID2, ingredientCount2));

                _ = int.TryParse(nodes.Attributes["IngredientItemID3"]?.Value ?? "0", out int ingredientItemID3);
                _ = int.TryParse(nodes.Attributes["IngredientCount3"]?.Value ?? "0", out int ingredientCount3);
                rewards[itemID].Add(new(ingredientItemID3, ingredientCount3));
            }
        }

        return rewards;
    }

    private static void ParseHair(XmlNode slot, ItemMetadata metadata)
    {
        int assetNodeCount = slot.SelectNodes("asset").Count;
        XmlNode asset = slot.FirstChild;

        XmlNode scaleNode = slot.SelectSingleNode("scale");

        CoordF defaultCoord = CoordF.Parse("0, 0, 0");
        switch (assetNodeCount)
        {
            // This hair has a front and back positionable hair
            case 3:
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
                            if (backPresets.Name != "custom")
                            {
                                continue;
                            }

                            bPosCord[i] = CoordF.Parse(backPresets.Attributes["position"].Value);
                            bPosRotation[i] = CoordF.Parse(backPresets.Attributes["rotation"].Value);
                        }

                        foreach (XmlNode frontPresets in frontHair)
                        {
                            if (frontPresets.Name != "custom")
                            {
                                continue;
                            }

                            fPosCord[i] = CoordF.Parse(frontPresets.Attributes["position"].Value);
                            fPosRotation[i] = CoordF.Parse(frontPresets.Attributes["position"].Value);
                        }

                        HairPresets hairPresets = new()
                        {
                            BackPositionCoord = bPosCord[i],
                            BackPositionRotation = bPosRotation[i],
                            FrontPositionCoord = fPosCord[i],
                            FrontPositionRotation = fPosRotation[i],
                            MinScale = float.Parse(scaleNode?.Attributes["min"]?.Value ?? "0"),
                            MaxScale = float.Parse(scaleNode?.Attributes["max"]?.Value ?? "0")
                        };

                        metadata.HairPresets.Add(hairPresets);
                    }

                    break;
                }
            // This hair only has back positionable hair
            case 2:
                {
                    XmlNode backHair = asset.NextSibling; // back hair info

                    int backHairNodes = backHair.SelectNodes("custom").Count;

                    CoordF[] bPosCord = new CoordF[backHairNodes];
                    CoordF[] bPosRotation = new CoordF[backHairNodes];

                    for (int i = 0; i < backHairNodes; i++)
                    {
                        foreach (XmlNode backPresets in backHair)
                        {
                            if (backPresets.Name != "custom")
                            {
                                continue;
                            }

                            bPosCord[i] = CoordF.Parse(backPresets.Attributes["position"].Value);
                            bPosRotation[i] = CoordF.Parse(backPresets.Attributes["rotation"].Value);
                        }

                        HairPresets hairPresets = new()
                        {
                            BackPositionCoord = bPosCord[i],
                            BackPositionRotation = bPosRotation[i],
                            FrontPositionCoord = defaultCoord,
                            FrontPositionRotation = defaultCoord,
                            MinScale = float.Parse(scaleNode?.Attributes["min"]?.Value ?? "0"),
                            MaxScale = float.Parse(scaleNode?.Attributes["max"]?.Value ?? "0")
                        };

                        metadata.HairPresets.Add(hairPresets);
                    }

                    break;
                }
            // hair does not have back or front positionable hair
            default:
                {
                    HairPresets hairPresets = new()
                    {
                        BackPositionCoord = defaultCoord,
                        BackPositionRotation = defaultCoord,
                        FrontPositionCoord = defaultCoord,
                        FrontPositionRotation = defaultCoord,
                        MinScale = float.Parse(scaleNode?.Attributes["min"]?.Value ?? "0"),
                        MaxScale = float.Parse(scaleNode?.Attributes["max"]?.Value ?? "0")
                    };

                    metadata.HairPresets.Add(hairPresets);
                    break;
                }
        }
    }

    // This is an approximation and may not be 100% correct
    private static InventoryTab GetTab(byte type, byte subType, bool skin = false)
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
                return InventoryTab.Lapenshard;
            case 22: // Blueprint
                return InventoryTab.Misc;
        }

        throw new ArgumentException($"Unknown Tab for: {type},{subType}");
    }
}
