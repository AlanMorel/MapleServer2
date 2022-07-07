using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MeretMarketPacket
{
    private enum Mode : byte
    {
        LoadPersonalListings = 0xB,
        LoadSales = 0xC,
        ListItem = 0xD,
        RemoveListing = 0xE,
        UnlistItem = 0xF,
        RelistItem = 0x12,
        CollectProfit = 0x14,
        UpdateExpiration = 0x15,
        UpdateProfit = 0x1A, // ?? maybe?
        LoadShopCategory = 0x1B,
        Purchase = 0x1E,
        Initialize = 016,
        Home = 0x65,
        OpenDesignShop = 0x66,
        LoadCart = 0x6B,
        ModeC9 = 0xC9
    }

    public static PacketWriter LoadPersonalListings(List<UgcMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.LoadPersonalListings);
        pWriter.WriteLong();
        pWriter.WriteInt(items.Count);
        foreach (UgcMarketItem item in items)
        {
            pWriter.WriteByte(1);
            WriteUgcMarketItem(pWriter, item);
        }
        return pWriter;
    }

    public static PacketWriter LoadSales(List<UgcMarketSale> sales)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.LoadSales);
        pWriter.WriteInt(sales.Count); // count

        foreach (UgcMarketSale sale in sales)
        {
            pWriter.WriteLong(sale.Id);
            pWriter.WriteLong(2); // item id? or listing id?
            pWriter.WriteUnicodeString(sale.ItemName);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString();
            pWriter.WriteUnicodeString();
            pWriter.WriteInt();
            pWriter.WriteLong(sale.Price);
            pWriter.WriteLong(sale.SoldTimestamp);
            pWriter.WriteLong(sale.Profit);
        }

        return pWriter;
    }

    public static PacketWriter ListItem(UgcMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.ListItem);
        WriteUgcMarketItem(pWriter, item);
        return pWriter;
    }

    public static PacketWriter RemoveListing(long id)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.RemoveListing);
        pWriter.WriteInt();
        pWriter.WriteLong(id);
        pWriter.WriteLong(id);
        return pWriter;
    }

    public static PacketWriter RelistItem(UgcMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.RelistItem);
        WriteUgcMarketItem(pWriter, item);
        return pWriter;
    }

    public static PacketWriter CollectProfit(UgcMarketSale sale)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.CollectProfit);
        pWriter.WriteLong(sale.Id);
        pWriter.WriteInt();
        pWriter.WriteLong(sale.Id);
        return pWriter;
    }


    public static PacketWriter UpdateExpiration(UgcMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.UpdateExpiration);
        pWriter.WriteInt();
        pWriter.WriteLong(item.MarketId);
        pWriter.WriteLong(item.MarketId);
        pWriter.Write(item.Status);
        pWriter.WriteByte();
        pWriter.WriteLong(item.ListingExpirationTimestamp);
        return pWriter;
    }

    public static PacketWriter UpdateProfit(long saleId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.UpdateProfit);
        pWriter.WriteLong(saleId);
        pWriter.WriteLong(saleId);
        return pWriter;
    }

    public static PacketWriter Initialize()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.Initialize);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LoadShopCategory(List<MeretMarketItem> items, int totalItems)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.LoadShopCategory);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(totalItems);
        pWriter.WriteInt(1);
        WriteMarketItemLoop(pWriter, items);
        return pWriter;
    }

    public static PacketWriter Purchase(int premiumMarketId, long ugcMarketItemId, long price, int totalQuantity, int itemIndex = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.Purchase);
        pWriter.WriteByte((byte) totalQuantity);
        pWriter.WriteInt(premiumMarketId);
        pWriter.WriteLong(ugcMarketItemId);
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteInt(itemIndex);
        pWriter.WriteInt(totalQuantity);
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();
        pWriter.WriteLong(price);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter Promos(List<MeretMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.Home);
        pWriter.WriteByte();
        pWriter.WriteByte(10);
        pWriter.WriteByte(1);
        pWriter.WriteByte(14);
        pWriter.WriteByte();
        pWriter.WriteByte();

        WriteMarketItemLoop(pWriter, items);

        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter OpenDesignShop(List<UgcMarketItem> promoItems, List<UgcMarketItem> newItems)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.OpenDesignShop);
        pWriter.WriteInt(promoItems.Count + newItems.Count);
        foreach (UgcMarketItem item in promoItems)
        {
            pWriter.WriteByte(1);
            WriteUgcMarketItem(pWriter, item, UgcMarketItemHomeCategory.Promoted);
        }
        foreach (UgcMarketItem item in newItems)
        {
            pWriter.WriteByte(1);
            WriteUgcMarketItem(pWriter, item, UgcMarketItemHomeCategory.New);
        }
        return pWriter;
    }

    public static PacketWriter LoadCart()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.LoadCart);
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteInt(3);
        pWriter.WriteInt(10);
        return pWriter;
    }

    public static PacketWriter ModeC9()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MeretMarket);
        pWriter.Write(Mode.ModeC9);
        pWriter.WriteInt();

        return pWriter;
    }

    private static void WriteMarketItemLoop(PacketWriter pWriter, List<MeretMarketItem> items)
    {
        foreach (MeretMarketItem item in items)
        {
            if (item is PremiumMarketItem)
            {
                pWriter.WriteByte(1);
            }
            pWriter.WriteBool(item is UgcMarketItem);
            switch (item)
            {
                case UgcMarketItem ugc:
                    WriteUgcMarketItem(pWriter, ugc);
                    break;
                case PremiumMarketItem premium:
                    {
                        pWriter.WriteInt((int) premium.MarketId);
                        pWriter.WriteLong();
                        WriteMeretMarketItem(pWriter, premium);
                        pWriter.WriteBool(premium.IsPromo);
                        if (premium.IsPromo)
                        {
                            pWriter.WriteString(premium.PromoName);
                            pWriter.WriteLong(premium.PromoBannerBeginTime);
                            pWriter.WriteLong(premium.PromoBannerEndTime);
                        }
                        pWriter.WriteByte();
                        pWriter.WriteBool(premium.ShowSaleTime);
                        pWriter.WriteInt();
                        pWriter.WriteByte((byte) premium.AdditionalQuantities.Count);
                        foreach (PremiumMarketItem additionalItem in premium.AdditionalQuantities)
                        {
                            pWriter.WriteByte(1);
                            WriteMeretMarketItem(pWriter, additionalItem);
                            pWriter.WriteByte();
                            pWriter.WriteByte();
                            pWriter.WriteByte();
                        }
                        break;
                    }
            }
        }
    }

    private static void WriteMeretMarketItem(PacketWriter pWriter, PremiumMarketItem item)
    {
        pWriter.WriteInt((int) item.MarketId);
        pWriter.WriteByte(2);
        pWriter.WriteUnicodeString(item.ItemName);
        pWriter.WriteByte(1);
        pWriter.WriteInt(item.ParentMarketId);
        pWriter.WriteInt(254);
        pWriter.WriteInt();
        pWriter.WriteByte(2);
        pWriter.Write(item.Flag);
        pWriter.Write(item.TokenType);
        pWriter.WriteLong(item.Price);
        pWriter.WriteLong(item.SalePrice);
        pWriter.WriteByte(1);
        pWriter.WriteLong(item.SellBeginTime);
        pWriter.WriteLong(item.SellEndTime);
        pWriter.Write(item.JobRequirement);
        pWriter.WriteInt(3);
        pWriter.WriteBool(item.RestockUnavailable);
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteShort(item.MinLevelRequirement);
        pWriter.WriteShort(item.MaxLevelRequirement);
        pWriter.Write(item.JobRequirement);
        pWriter.WriteInt(item.ItemId);
        pWriter.WriteByte(item.Rarity);
        pWriter.WriteInt(item.Quantity);
        pWriter.WriteInt(item.Duration);
        pWriter.WriteInt(item.BonusQuantity);
        pWriter.WriteInt(40300);
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.Write(item.PromoFlag);
        pWriter.WriteString(item.Banner?.Name ?? "");
        pWriter.WriteString();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteInt(item.RequiredAchievementId);
        pWriter.WriteInt(item.RequiredAchievementGrade);
        pWriter.WriteInt();
        pWriter.WriteBool(item.PCCafe);
        pWriter.WriteByte();
        pWriter.WriteInt();
    }

    private static void WriteUgcMarketItem(PacketWriter pWriter, UgcMarketItem item, UgcMarketItemHomeCategory category = UgcMarketItemHomeCategory.None)
    {
        pWriter.WriteByte(1);
        pWriter.WriteInt();
        pWriter.WriteLong(item.MarketId);
        pWriter.WriteInt();
        pWriter.WriteLong(item.MarketId);
        pWriter.Write(item.Status);
        pWriter.WriteInt(item.Item.Id);
        pWriter.WriteInt(35);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteLong(item.Price);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(item.SalesCount);
        pWriter.WriteInt();
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteLong(item.ListingExpirationTimestamp);
        pWriter.WriteInt();
        pWriter.WriteLong(item.PromotionExpirationTimestamp);
        pWriter.WriteLong();
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteInt(2);
        pWriter.WriteLong(item.SellerAccountId);
        pWriter.WriteLong(item.SellerCharacterId);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(item.SellerCharacterName);
        pWriter.WriteUnicodeString(string.Join(",", item.Tags.ToArray()) + ", " + item.Item.Ugc.Name);
        pWriter.WriteUnicodeString(item.Description);
        pWriter.WriteUnicodeString(item.Item.Ugc.CharacterName);
        pWriter.WriteLong(item.Item.Ugc.Uid);
        pWriter.WriteUnicodeString(item.Item.Ugc.Guid.ToString());
        pWriter.WriteUnicodeString(item.Item.Ugc.Name);
        pWriter.WriteByte(1);
        pWriter.WriteInt(2);
        pWriter.WriteLong(item.Item.Ugc.AccountId);
        pWriter.WriteLong(item.Item.Ugc.CharacterId);
        pWriter.WriteUnicodeString(item.Item.Ugc.CharacterName);
        pWriter.WriteLong(item.Item.Ugc.CreationTime);
        pWriter.WriteUnicodeString(item.Item.Ugc.Url);
        pWriter.WriteByte();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();
        pWriter.WriteString();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteUnicodeString();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteUnicodeString();
        pWriter.Write(category);
    }
}
