using System.Web;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Item;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class ItemParser : Exporter<List<ItemMetadata>>
{
    public ItemParser(MetadataResources resources) : base(resources, MetadataName.Item) { }

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
            Skill skill = data.skill;
            Fusion fusion = data.fusion;
            Property property = data.property;
            Function function = data.function;
            Install install = data.install;
            MusicScore musicScore = data.MusicScore;
            Life life = data.life;
            Housing housing = data.housing;
            AdditionalEffect additionalEffect = data.AdditionalEffect;
            ItemMetadata metadata = new()
            {
                Id = id,
                Name = name,
                Tab = GetTab(property.type, property.subtype, property.skin),
                Gem = new()
                {
                    Gem = (GemSlot) data.gem.system
                },
                UGC = new()
                {
                    Mesh = data.ucc.mesh
                },
                Life = new()
                {
                    DurationPeriod = life.usePeriod,
                    ExpirationType = (ItemExpirationType) life.expirationType,
                    ExpirationTypeDuration = life.numberOfWeeksMonths
                },
                Pet = new()
                {
                    PetId = data.pet?.petID ?? 0,
                },
                Basic = new()
                {
                    Tag = data.basic.stringTag
                },
                Limit = new()
                {
                    JobRequirements = limit.jobLimit.Length == 0
                        ? new()
                        {
                            0
                        }
                        : limit.jobLimit.ToList(),
                    JobRecommendations = limit.recommendJobs.ToList(),
                    LevelLimitMin = limit.levelLimit,
                    LevelLimitMax = limit.levelLimitMax,
                    Gender = (Gender) limit.genderLimit,
                    TransferType = (TransferType) limit.transferType,
                    Sellable = limit.shopSell,
                    Breakable = limit.enableBreak,
                    MeretMarketListable = limit.enableRegisterMeratMarket,
                    DisableEnchant = limit.exceptEnchant,
                    TradeLimitByRarity = limit.tradeLimitRank,
                    VipOnly = limit.vip
                },
                Skill = new()
                {
                    SkillId = skill.skillID,
                    SkillLevel = skill.skillLevel
                },
                Fusion = new()
                {
                    Fusionable = fusion.fusionable == 1,
                },
                Install = new()
                {
                    IsCubeSolid = install.cubeProp == 1,
                    ObjectId = install.objCode,
                },
                Property = new()
                {
                    StackLimit = property.slotMax,
                    SkinType = (ItemSkinType) property.skinType,
                    Category = property.category,
                    BlackMarketCategory = property.blackMarketCategory,
                    DisableAttributeChange = property.remakeDisable,
                    GearScoreFactor = property.gearScore,
                    TradeableCount = (byte) property.tradableCount,
                    RepackageCount = (byte) property.rePackingLimitCount,
                    RepackageItemConsumeCount = (byte) property.rePackingItemConsumeCount,
                    DisableTradeWithinAccount = property.moveDisable == 1,
                    DisableDrop = property.disableDrop,
                    SocketDataId = property.socketDataId,
                    Sell = new()
                    {
                        SellPrice = property.sell.price.ToList(),
                        SellPriceCustom = property.sell.priceCustom.ToList(),
                    }
                },
                Customize = new()
                {
                    ColorIndex = data.customize.defaultColorIndex,
                    ColorPalette = data.customize.colorPalette,
                },
                Function = new()
                {
                    Name = function.name
                },
                Option = new()
                {
                    Static = data.option.@static,
                    Random = data.option.random,
                    Constant = data.option.constant,
                    OptionLevelFactor = data.option.optionLevelFactor,
                    OptionId = data.option.optionID,
                },
                Music = new()
                {
                    PlayCount = musicScore.playCount,
                    MasteryValue = musicScore.masteryValue,
                    MasteryValueMax = musicScore.masteryValueMax,
                    IsCustomScore = musicScore.isCustomNote,
                    FileName = musicScore.fileName,
                    PlayTime = musicScore.playTime
                },
                Housing = new()
                {
                    TrophyId = housing.trophyID,
                    TrophyLevel = housing.trophyLevel
                },
                Shop = new()
                {
                    ShopId = data.Shop?.systemShopID ?? 0
                },
                AdditionalEffect = new()
                {
                    Id = additionalEffect.id,
                    Level = Array.ConvertAll(additionalEffect.level, level => (short) level)
                }
            };

            // Parse expiration time
            if (life.expirationPeriod.Length > 0)
            {
                metadata.Life.ExpirationTime = new(life.expirationPeriod[0], life.expirationPeriod[1], life.expirationPeriod[2], life.expirationPeriod[3],
                    life.expirationPeriod[4], life.expirationPeriod[5]);
            }

            // if globalOptionLevelFactor is present, override with these values
            if (data.option.globalOptionLevelFactor is not null)
            {
                metadata.Option.OptionLevelFactor = (float) data.option.globalOptionLevelFactor;
            }

            // if globalTransferType is present, override with these values
            if (limit.globalTransferType is not null)
            {
                metadata.Limit.TransferType = (TransferType) limit.globalTransferType;
            }

            // if globalTransferTypeNA is present, override with these values
            if (limit.globalTransferTypeNA is not null)
            {
                metadata.Limit.TransferType = (TransferType) limit.globalTransferTypeNA;
            }

            // if globalRePackingLimit is present, override repacking with these values
            if (property.globalRePackingLimitCount is not null)
            {
                metadata.Property.RepackageCount = (byte) property.globalRePackingLimitCount;
                metadata.Property.RepackageItemConsumeCount = (byte) (property.globalRePackingItemConsumeCount ?? 0);
            }

            // Item functions
            ParseFunctions(function, metadata);

            Slots slots = data.slots;
            metadata.Slots = new();
            foreach (Slot slot in slots.slot)
            {
                bool slotResult = Enum.TryParse(slot.name, out ItemSlot itemSlot);
                if (!slotResult && !string.IsNullOrEmpty(slot.name))
                {
                    Console.WriteLine($"Failed to parse item slot for {id}: {slot.name}");
                    continue;
                }

                if (itemSlot == ItemSlot.HR)
                {
                    ParseHair(slot, metadata);
                }

                metadata.Slots.Add(itemSlot);
            }

            if (!string.IsNullOrEmpty(housing.categoryTag))
            {
                string[] tags = housing.categoryTag.Split(',');
                _ = short.TryParse(tags[0], out short category);
                metadata.Housing.HousingCategory = (ItemHousingCategory) category;
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

    private static void ParseFunctions(Function function, ItemMetadata metadata)
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

                    metadata.Function.OpenItemBox = box;
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
                    metadata.Function.SelectItemBox = box;
                    break;
                }
            case "ChatEmoticonAdd":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "id"))
                    {
                        break;
                    }

                    ChatEmoticonAdd sticker = new()
                    {
                        Id = byte.Parse(functionParameters!.Attributes!["id"]!.Value),
                        Duration = int.Parse(functionParameters.Attributes["durationSec"]?.Value ?? "0")
                    };

                    metadata.Function.ChatEmoticonAdd = sticker;
                    break;
                }
            case "OpenMassive":
                {
                    string rawParameter = function.parameter;
                    string cleanParameter = rawParameter.Remove(1, 1); // remove the unwanted space
                    string decodedParameter = HttpUtility.HtmlDecode(cleanParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "fieldId", "portalDurationTick", "maxCount"))
                    {
                        break;
                    }

                    OpenMassiveEvent massiveEvent = new()
                    {
                        FieldId = int.Parse(functionParameters!.Attributes!["fieldID"]!.Value),
                        Duration = int.Parse(functionParameters.Attributes["portalDurationTick"]!.Value),
                        Capacity = byte.Parse(functionParameters.Attributes["maxCount"]!.Value)
                    };
                    metadata.Function.OpenMassiveEvent = massiveEvent;
                    break;
                }
            case "LevelPotion":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "targetLevel"))
                    {
                        break;
                    }

                    LevelPotion levelPotion = new()
                    {
                        TargetLevel = byte.Parse(functionParameters!.Attributes!["targetLevel"]!.Value)
                    };
                    metadata.Function.LevelPotion = levelPotion;
                    break;
                }
            case "VIPCoupon":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "period"))
                    {
                        break;
                    }

                    VIPCoupon coupon = new()
                    {
                        Duration = int.Parse(functionParameters!.Attributes!["period"]!.Value)
                    };
                    metadata.Function.VIPCoupon = coupon;
                    break;
                }
            case "HongBao":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "itemId", "totalCount", "totalUser", "durationSec"))
                    {
                        break;
                    }

                    HongBaoData hongBao = new()
                    {
                        Id = int.Parse(functionParameters!.Attributes!["itemId"]!.Value),
                        Count = short.Parse(functionParameters.Attributes["totalCount"]!.Value),
                        TotalUsers = byte.Parse(functionParameters.Attributes["totalUser"]!.Value),
                        Duration = int.Parse(functionParameters.Attributes["durationSec"]!.Value)
                    };
                    metadata.Function.HongBao = hongBao;
                    break;
                }
            case "SuperWorldChat":
                {
                    string[] parameters = function.parameter.Split(",");
                    metadata.Function.Id = int.Parse(parameters[0]); // only storing the first parameter. Not sure if the server uses the other 2. 
                    break;
                }
            case "OpenGachaBox":
                {
                    string[] parameters = function.parameter.Split(",");
                    metadata.Function.Id = int.Parse(parameters[0]); // only storing the first parameter. Unknown what the second parameter is used for.
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
                    metadata.Function.OpenCoupleEffectBox = box;
                    break;
                }
            case "InstallBillBoard":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");

                    if (ParserHelper.CheckForNull(functionParameters, "interactID", "durationSec", "model", "normal", "reactable"))
                    {
                        break;
                    }

                    InstallBillboard balloon = new()
                    {
                        InteractId = int.Parse(functionParameters!.Attributes!["interactID"]!.Value),
                        Duration = int.Parse(functionParameters.Attributes["durationSec"]!.Value),
                        Model = functionParameters.Attributes["model"]!.Value,
                        Asset = functionParameters.Attributes["asset"]?.Value ?? "",
                        NormalState = functionParameters.Attributes["normal"]!.Value,
                        Reactable = functionParameters.Attributes["reactable"]!.Value,
                        Scale = float.Parse(functionParameters.Attributes["scale"]?.Value ?? "0")
                    };
                    metadata.Function.InstallBillboard = balloon;
                    break;
                }
            case "SurvivalSkin":
                {
                    string rawParameter = function.parameter;
                    string decodedParameter = HttpUtility.HtmlDecode(rawParameter);

                    XmlDocument xmlParameter = new();
                    xmlParameter.LoadXml(decodedParameter);
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");
                    if (ParserHelper.CheckForNull(functionParameters, "type", "id"))
                    {
                        break;
                    }

                    MedalSlot medalSlot = functionParameters!.Attributes!["type"]!.Value switch
                    {
                        "effectTail" => MedalSlot.Tail,
                        "riding" => MedalSlot.GroundMount,
                        "gliding" => MedalSlot.Glider,
                        _ => throw new ArgumentException($"Unknown slot for: {functionParameters.Attributes["type"]!.Value}")
                    };
                    metadata.Function.SurvivalSkin = new()
                    {
                        Id = int.Parse(functionParameters.Attributes["id"]!.Value),
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
                    XmlNode? functionParameters = xmlParameter.SelectSingleNode("v");
                    if (ParserHelper.CheckForNull(functionParameters, "SurvivalExp"))
                    {
                        break;
                    }

                    metadata.Function.SurvivalLevelExp = new()
                    {
                        SurvivalExp = int.Parse(functionParameters!.Attributes!["SurvivalExp"]!.Value)
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
            case "ItemSocketScroll":
            case "EnchantScroll":
                metadata.Function.Id = int.Parse(function.parameter);
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
            XmlNodeList? nodes = innerDocument.SelectNodes("/ms2/key");
            if (nodes is null)
            {
                continue;
            }

            foreach (XmlNode node in nodes)
            {
                if (ParserHelper.CheckForNull(node, "id", "grade"))
                {
                    continue;
                }

                int itemId = int.Parse(node.Attributes!["id"]!.Value);
                int rarity = int.Parse(node.Attributes["grade"]!.Value);
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
            XmlNodeList? individualItems = innerDocument.SelectNodes("/ms2/item");
            if (individualItems is null)
            {
                continue;
            }

            foreach (XmlNode nodes in individualItems)
            {
                string locale = nodes.Attributes?["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(nodes, "ItemId"))
                {
                    continue;
                }

                int itemID = int.Parse(nodes.Attributes!["ItemID"]!.Value);
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
        Slot.Scale? scaleNode = slot.scale?.FirstOrDefault();

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

                        metadata.Customize.HairPresets.Add(hairPresets);
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

                        metadata.Customize.HairPresets.Add(hairPresets);
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

                    metadata.Customize.HairPresets.Add(hairPresets);
                    break;
                }
        }
    }

    // This is an approximation and may not be 100% correct
    private static InventoryTab GetTab(int type, int subType, bool skin = false, bool survival = false)
    {
        if (skin)
        {
            return InventoryTab.Outfit;
        }

        if (survival)
        {
            //return InventoryTab.Survival;
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
