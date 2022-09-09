using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Tools;
using MapleServer2.Types;
using ProtoBuf;

namespace MapleServer2.Data.Static;

// This is an in-memory storage to help with determining some metadata of items
public static class ItemMetadataStorage
{
    private static readonly Dictionary<int, ItemMetadata> ItemMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Item);
        List<ItemMetadata> items = Serializer.Deserialize<List<ItemMetadata>>(stream);
        foreach (ItemMetadata item in items)
        {
            ItemMetadatas[item.Id] = item;
        }
    }

    public static bool IsValid(int itemId) => ItemMetadatas.ContainsKey(itemId);

    public static ItemMetadata? GetMetadata(int itemId) => ItemMetadatas.GetValueOrDefault(itemId);

    public static string? GetName(int itemId) => GetMetadata(itemId)?.Name;

    public static List<ItemSlot>? GetItemSlots(int itemId) => GetMetadata(itemId)?.Slots;

    public static GemSlot? GetGem(int itemId) => GetMetadata(itemId)?.Gem.Gem;

    public static MedalSlot? GetMedalSlot(int itemId) => GetMetadata(itemId)?.Medal;

    public static InventoryTab? GetTab(int itemId) => GetMetadata(itemId)?.Tab;

    public static int GetRarity(int itemId) => GetMetadata(itemId)?.Rarity ?? 1;

    public static ItemPropertyMetadata? GetPropertyMetadata(int itemId) => GetMetadata(itemId)?.Property;

    public static ItemLimitMetadata? GetLimitMetadata(int itemId) => GetMetadata(itemId)?.Limit;

    public static ItemInstallMetadata? GetInstallMetadata(int itemId) => GetMetadata(itemId)?.Install;

    public static ItemMusicMetadata? GetMusicMetadata(int itemId) => GetMetadata(itemId)?.Music;

    public static ItemHousingMetadata? GetHousingMetadata(int itemId) => GetMetadata(itemId)?.Housing;

    public static ItemFunctionMetadata? GetFunctionMetadata(int itemId) => GetMetadata(itemId)?.Function;

    public static ItemOptionMetadata? GetOptionMetadata(int itemId) => GetMetadata(itemId)?.Option;

    public static ItemSkillMetadata? GetSkillMetadata(int itemId) => GetMetadata(itemId)?.Skill;

    public static ItemAdditionalEffectMetadata? GetAdditionalEffects(int itemId) => GetMetadata(itemId)?.AdditionalEffect;

    public static bool GetIsUGC(int itemId) => !string.IsNullOrEmpty(GetMetadata(itemId)?.UGC.Mesh);

    public static int? GetShopID(int itemId) => GetMetadata(itemId)?.Shop.ShopId;

    public static bool IsTradeDisabledWithinAccount(int itemId) => GetMetadata(itemId)?.Property.DisableTradeWithinAccount ?? false;

    public static ItemTransferFlag GetTransferFlag(int itemId, int rarity)
    {
        TransferType? transferType = GetLimitMetadata(itemId)?.TransferType;
        ItemTransferFlag transferFlag = ItemTransferFlag.Untradeable;
        int? tradeLimitByRarity = GetMetadata(itemId)?.Limit.TradeLimitByRarity;
        int? tradeCount = GetPropertyMetadata(itemId)?.TradeableCount;
        bool tradeable = tradeCount > 0 || transferType == TransferType.Tradeable;

        switch (transferType)
        {
            case TransferType.Tradeable:
                if (rarity >= tradeLimitByRarity)
                {
                    transferFlag = tradeable ? ItemTransferFlag.Untradeable : ItemTransferFlag.LimitedTradeCount;
                    break;
                }

                transferFlag = ItemTransferFlag.Tradeable;
                break;
            case TransferType.Untradeable:
                transferFlag = tradeable ? ItemTransferFlag.Untradeable : ItemTransferFlag.LimitedTradeCount;
                break;
            case TransferType.BindOnLoot:
            case TransferType.BindOnEquip:
            case TransferType.BindOnUse:
            case TransferType.BindOnTrade:
            case TransferType.BindOnSummonEnchantOrReroll:
                transferFlag = ItemTransferFlag.Binds;
                if (tradeCount <= 0)
                {
                    if (rarity >= tradeLimitByRarity)
                    {
                        break;
                    }

                    transferFlag = ItemTransferFlag.Binds | ItemTransferFlag.Tradeable;
                    break;
                }

                transferFlag = ItemTransferFlag.Binds | ItemTransferFlag.LimitedTradeCount;
                break;
            case TransferType.TradeableOnBlackMarket:
                if (tradeCount > 0 && rarity >= tradeLimitByRarity)
                {
                    transferFlag = tradeable ? ItemTransferFlag.Untradeable : ItemTransferFlag.LimitedTradeCount;
                    break;
                }

                transferFlag = ItemTransferFlag.Tradeable;
                break;
        }

        if (rarity < tradeLimitByRarity && (transferFlag & ItemTransferFlag.Tradeable) != 0)
        {
            transferFlag |= ItemTransferFlag.Splitable;
        }

        return transferFlag;
    }

    public static bool? IsFusionable(int itemId) => GetMetadata(itemId)?.Fusion.Fusionable;

    public static List<Job> GetRecommendJobs(int itemId)
    {
        static Job Converter(int integer) => (Job) integer;

        return GetMetadata(itemId)?.Limit.JobRecommendations?.ConvertAll(Converter) ?? new List<Job>
        {
            Job.None
        };
    }

    public static List<Job> GetRequiredJobs(int itemId)
    {
        static Job Converter(int integer) => (Job) integer;

        return GetMetadata(itemId)?.Limit.JobRequirements?.ConvertAll(Converter) ?? new List<Job>
        {
            Job.None
        };
    }

    public static long GetSellPrice(int itemId)
    {
        // get random selling price from price points
        List<long>? pricePoints = GetMetadata(itemId)?.Property.Sell.SellPrice;
        if (pricePoints == null || !pricePoints.Any())
        {
            return 0;
        }

        int rand = Random.Shared.Next(0, pricePoints.Count);

        return pricePoints.ElementAt(rand);
    }

    public static long GetCustomSellPrice(int itemId)
    {
        // get random selling price from price points
        List<long>? pricePoints = GetMetadata(itemId)?.Property.Sell.SellPriceCustom;
        if (pricePoints == null || !pricePoints.Any())
        {
            return 0;
        }

        int rand = Random.Shared.Next(0, pricePoints.Count);

        return pricePoints.ElementAt(rand);
    }

    public static string? GetTag(int itemId) => GetMetadata(itemId)?.Basic.Tag;

    public static int? GetPetId(int itemId) => GetMetadata(itemId)?.Pet.PetId;

    public static EquipColor GetEquipColor(int itemId)
    {
        ItemMetadata? itemMetadata = GetMetadata(itemId);
        int colorPalette = itemMetadata?.Customize.ColorPalette ?? 0;
        int colorIndex = itemMetadata?.Customize.ColorIndex ?? 0;

        if (colorPalette == 0) // item has no color
        {
            return EquipColor.Custom(MixedColor.Custom(Color.Argb(0, 0, 0, 0), Color.Argb(0, 0, 0, 0), Color.Argb(0, 0, 0, 0)), colorIndex, colorPalette);
        }

        ColorPaletteMetadata? palette = ColorPaletteMetadataStorage.GetMetadata(colorPalette);
        if (palette is null)
        {
            return EquipColor.Custom(MixedColor.Custom(Color.Argb(0, 0, 0, 0), Color.Argb(0, 0, 0, 0), Color.Argb(0, 0, 0, 0)), colorIndex, colorPalette);
        }

        if (colorPalette > 0 && colorIndex == -1) // random color from color palette
        {
            Random random = Random.Shared;

            int index = random.Next(palette.DefaultColors.Count);

            return EquipColor.Argb(palette.DefaultColors[index], colorIndex, colorPalette);
        }

        return EquipColor.Argb(palette.DefaultColors[colorIndex], colorIndex, colorPalette);
    }

    public static List<ItemBreakReward>? GetBreakRewards(int itemId) => GetMetadata(itemId)?.BreakRewards;

    public static long GetExpiration(int itemId)
    {
        ItemLifeMetadata? life = GetMetadata(itemId)?.Life;
        if (life is null)
        {
            return 0;
        }

        long expirationTimestamp = 0;

        if (life.ExpirationTime != new DateTime(1, 1, 1, 0, 0, 0))
        {
            expirationTimestamp = ((DateTimeOffset) life.ExpirationTime.ToUniversalTime().Date).ToUnixTimeSeconds();
        }
        else if (life.DurationPeriod > 0)
        {
            expirationTimestamp = TimeInfo.Now() + life.DurationPeriod;
        }
        else if (life.ExpirationType != ItemExpirationType.None)
        {
            expirationTimestamp = life.ExpirationType switch
            {
                ItemExpirationType.Months => life.ExpirationTypeDuration * TimeInfo.SecondsInMonth + TimeInfo.Now(),
                ItemExpirationType.Weeks => life.ExpirationTypeDuration * TimeInfo.SecondsInWeek + TimeInfo.Now(),
                _ => expirationTimestamp
            };
        }

        return expirationTimestamp;
    }

    public static IEnumerable<ItemMetadata> GetAll() => ItemMetadatas.Values;
}
