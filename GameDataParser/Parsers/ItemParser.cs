using System.Web;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Item;
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

        // Item rarity
        Dictionary<int, int> rarities = ParseItemRarities();

        // Items
        List<ItemMetadata> items = new();
        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.ItemParser parser = new(Resources.XmlReader);
        foreach ((int id, string name, ItemData data) in parser.Parse())
        {
            Limit limit = data.limit;
            Property property = data.property;
            Function function = data.function;
            Install install = data.install;
            MusicScore musicScore = data.MusicScore;

            ItemMetadata metadata = new()
            {
                Id = id,
                Name = name,
                Tag = data.basic.stringTag,
                ColorIndex = data.customize.defaultColorIndex,
                ColorPalette = data.customize.colorPalette,
                Gem = (GemSlot) data.gem.system,
                Tab = GetTab(property.type, property.subtype, property.skin != 0),
                IsTemplate = property.skinType == 99,
                TradeableCount = (byte) property.tradableCount,
                DisableTradeWithinAccount = property.moveDisable == 1,
                RepackageCount = (byte) property.rePackingLimitCount,
                RepackageItemConsumeCount = (byte) property.rePackingItemConsumeCount,
                BlackMarketCategory = property.blackMarketCategory,
                Category = property.category,
                SellPrice = property.sell.price.ToList(),
                SellPriceCustom = property.sell.priceCustom.ToList(),
                StackLimit = property.slotMax,
                OptionStatic = data.option.@static,
                OptionRandom = data.option.random,
                OptionConstant = data.option.constant,
                OptionLevelFactor = data.option.optionLevelFactor,
                FunctionData =
                {
                    Name = function.name
                },
                PlayCount = musicScore.playCount,
                FileName = musicScore.fileName,
                IsCustomScore = musicScore.isCustomNote,
                ShopID = data.Shop?.systemShopID ?? 0,
                PetId = data.pet?.petID ?? 0,
                SkillID = data.skill.skillID,
                EnableBreak = limit.enableBreak != 0,
                Level = limit.levelLimit,
                TransferType = (TransferType) limit.transferType,
                TradeLimitByRarity = limit.tradeLimitRank,
                Sellable = limit.shopSell != 0,
                RecommendJobs = limit.recommendJobs.ToList(),
                Gender = (Gender) limit.genderLimit,
                IsCubeSolid = install.cubeProp != 0,
                ObjectId = install.objCode
            };

            // if globalTransferType is present, override with these values
            if (limit.globalTransferType is not null)
            {
                metadata.TransferType = (TransferType) limit.globalTransferType;
            }

            // if globalTransferTypeNA is present, override with these values
            if (limit.globalTransferTypeNA is not null)
            {
                metadata.TransferType = (TransferType) limit.globalTransferTypeNA;
            }

            // if globalRePackingLimit is present, override repacking with these values
            if (property.globalRePackingLimitCount is not null)
            {
                metadata.RepackageCount = (byte) property.globalRePackingLimitCount;
                metadata.RepackageItemConsumeCount = (byte) property.globalRePackingItemConsumeCount;
            }

            // Item boxes
            ParseBoxes(function, metadata);

            Slot firstSlot = data.slots.slot.First();
            bool slotResult = Enum.TryParse(firstSlot.name, out metadata.Slot);
            if (!slotResult && !string.IsNullOrEmpty(firstSlot.name))
            {
                Console.WriteLine($"Failed to parse item slot for {id}: {firstSlot.name}");
            }

            if (data.slots.slot.Count > 1)
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

            if (metadata.Slot is ItemSlot.HR)
            {
                ParseHair(firstSlot, metadata);
            }

            if (!string.IsNullOrEmpty(data.housing.categoryTag))
            {
                string[] tags = data.housing.categoryTag.Split(',');
                _ = short.TryParse(tags[0], out short category);
                metadata.HousingCategory = (ItemHousingCategory) category;
            }

            // Item breaking ingredients
            if (rewards.ContainsKey(id))
            {
                metadata.BreakRewards = rewards[id];
            }

            // Item rarities
            if (rarities.ContainsKey(id))
            {
                metadata.Rarity = rarities[id];
            }

            items.Add(metadata);
        }

        return items;
    }

    private static void ParseBoxes(Function function, ItemMetadata metadata)
    {
        switch (function.name)
        {
            // selection boxes are SelectItemBox and 1,boxid
            // normal boxes are OpenItemBox and 0,1,0,boxid
            // fragments are OpenItemBox and 0,1,0,boxid,required_amount
            case "OpenItemBox" when function.parameter.Contains('l'):
                return; // TODO: Implement these CN items. Skipping for now
            case "OpenItemBox":
                {
                    List<string> parameters = new(function.parameter.Split(','));
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
            case "SelectItemBox" when function.parameter.Contains('l'):
                return; // TODO: Implement these CN items. Skipping for now
            case "SelectItemBox":
                {
                    List<string> parameters = new(function.parameter.Split(','));
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
                    string rawParameter = function.parameter;
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
                    string rawParameter = function.parameter;
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
                    string rawParameter = function.parameter;
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
                    string rawParameter = function.parameter;
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
                    string rawParameter = function.parameter;
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
                    string[] parameters = function.parameter.Split(",");
                    metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Not sure if the server uses the other 2. 
                    break;
                }
            case "OpenGachaBox":
                {
                    string[] parameters = function.parameter.Split(",");
                    metadata.FunctionData.Id = int.Parse(parameters[0]); // only storing the first parameter. Unknown what the second parameter is used for.
                    break;
                }
            case "OpenCoupleEffectBox":
                {
                    string[] parameters = function.parameter.Split(",");
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
                    string rawParameter = function.parameter;
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
            case "SurvivalSkin":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    MedalSlot medalSlot = functionParameters.Attributes["type"].Value switch
                    {
                        "effectTail" => MedalSlot.Tail,
                        "riding" => MedalSlot.GroundMount,
                        "gliding" => MedalSlot.Glider,
                        _ => throw new ArgumentException($"Unknown slot for: {functionParameters.Attributes["type"].Value}")
                    };
                    metadata.FunctionData.SurvivalSkin = new()
                    {
                        Id = int.Parse(functionParameters.Attributes["id"].Value),
                        Slot = medalSlot
                    };
                }
                break;
            case "SurvivalLevelExp":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode functionParameters = xmlParameter.SelectSingleNode("v");
                    metadata.FunctionData.SurvivalLevelExp = new()
                    {
                        SurvivalExp = int.Parse(functionParameters.Attributes["SurvivalExp"].Value)
                    };
                }
                break;
            case "TitleScroll":
            case "ItemExchangeScroll":
            case "OpenInstrument":
            case "StoryBook":
            case "FishingRod":
            case "ItemChangeBeauty":
            case "ItemRePackingScroll":
                metadata.FunctionData.Id = int.Parse(function.parameter);
                break;
        }
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
            XmlNodeList nodes = innerDocument.SelectNodes("/ms2/key");
            foreach (XmlNode node in nodes)
            {
                int itemId = int.Parse(node.Attributes["id"].Value);
                int rarity = int.Parse(node.Attributes["grade"].Value);
                rarities[itemId] = rarity;
            }
        }

        return rarities;
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
            XmlNodeList individualItems = innerDocument.SelectNodes("/ms2/item");
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

    private static void ParseHair(Slot slot, ItemMetadata metadata)
    {
        Slot.Scale scaleNode = slot.scale?.FirstOrDefault();

        switch (slot.asset.Count)
        {
            // This hair has a front and back positionable hair
            case 3:
                {
                    Slot.Asset backHair = slot.asset[1]; // back hair info
                    Slot.Asset frontHair = slot.asset[2]; // front hair info

                    int backHairNodes = backHair.custom.Count;

                    CoordF[] bPosCord = new CoordF[backHairNodes];
                    CoordF[] bPosRotation = new CoordF[backHairNodes];
                    CoordF[] fPosCord = new CoordF[backHairNodes];
                    CoordF[] fPosRotation = new CoordF[backHairNodes];

                    for (int i = 0; i < backHairNodes; i++)
                    {
                        foreach (Slot.Custom backPresets in backHair.custom)
                        {
                            bPosCord[i] = CoordF.Parse(backPresets.position);
                            bPosRotation[i] = CoordF.Parse(backPresets.rotation);
                        }

                        foreach (Slot.Custom frontPresets in frontHair.custom)
                        {
                            fPosCord[i] = CoordF.Parse(frontPresets.position);
                            fPosRotation[i] = CoordF.Parse(frontPresets.rotation);
                        }

                        HairPresets hairPresets = new()
                        {
                            BackPositionCoord = bPosCord[i],
                            BackPositionRotation = bPosRotation[i],
                            FrontPositionCoord = fPosCord[i],
                            FrontPositionRotation = fPosRotation[i],
                            MinScale = scaleNode?.min ?? 0,
                            MaxScale = scaleNode?.max ?? 0
                        };

                        metadata.HairPresets.Add(hairPresets);
                    }

                    break;
                }
            // This hair only has back positionable hair
            case 2:
                {
                    Slot.Asset backHair = slot.asset[1]; // back hair info

                    int backHairNodes = backHair.custom.Count;

                    CoordF[] bPosCord = new CoordF[backHairNodes];
                    CoordF[] bPosRotation = new CoordF[backHairNodes];

                    for (int i = 0; i < backHairNodes; i++)
                    {
                        foreach (Slot.Custom backPresets in backHair.custom)
                        {
                            bPosCord[i] = CoordF.Parse(backPresets.position);
                            bPosRotation[i] = CoordF.Parse(backPresets.rotation);
                        }

                        HairPresets hairPresets = new()
                        {
                            BackPositionCoord = bPosCord[i],
                            BackPositionRotation = bPosRotation[i],
                            FrontPositionCoord = default,
                            FrontPositionRotation = default,
                            MinScale = scaleNode?.min ?? 0,
                            MaxScale = scaleNode?.max ?? 0
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
                        BackPositionCoord = default,
                        BackPositionRotation = default,
                        FrontPositionCoord = default,
                        FrontPositionRotation = default,
                        MinScale = scaleNode?.min ?? 0,
                        MaxScale = scaleNode?.max ?? 0
                    };

                    metadata.HairPresets.Add(hairPresets);
                    break;
                }
        }
    }

    // This is an approximation and may not be 100% correct
    private static InventoryTab GetTab(int type, int subType, bool skin = false)
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
                    case 11: // Survival Medals
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
