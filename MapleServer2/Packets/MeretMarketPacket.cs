using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MeretMarketPacket
{
    private enum MeretMarketMode : byte
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
        LoadCart = 0x6B
    }

    public static PacketWriter LoadPersonalListings(List<UGCMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.LoadPersonalListings);
        pWriter.WriteLong();
        pWriter.WriteInt(items.Count);
        foreach (UGCMarketItem item in items)
        {
            pWriter.WriteByte(1);
            WriteUGCMarketItem(pWriter, item);
        }
        return pWriter;
    }

    public static PacketWriter LoadSales(List<UGCMarketSale> sales)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.LoadSales);
        pWriter.WriteInt(sales.Count); // count

        foreach (UGCMarketSale sale in sales)
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

    public static PacketWriter ListItem(UGCMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.ListItem);
        WriteUGCMarketItem(pWriter, item);
        return pWriter;
    }

    public static PacketWriter RemoveListing(long id)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.RemoveListing);
        pWriter.WriteInt();
        pWriter.WriteLong(id);
        pWriter.WriteLong(id);
        return pWriter;
    }

    public static PacketWriter RelistItem(UGCMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.RelistItem);
        WriteUGCMarketItem(pWriter, item);
        return pWriter;
    }

    public static PacketWriter CollectProfit(UGCMarketSale sale)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.CollectProfit);
        pWriter.WriteLong(sale.Id);
        pWriter.WriteInt();
        pWriter.WriteLong(sale.Id);
        return pWriter;
    }


    public static PacketWriter UpdateExpiration(UGCMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.UpdateExpiration);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.WriteLong(item.Id);
        pWriter.Write(item.Status);
        pWriter.WriteByte();
        pWriter.WriteLong(item.ListingExpirationTimestamp);
        return pWriter;
    }

    public static PacketWriter UpdateProfit(long saleId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.UpdateProfit);
        pWriter.WriteLong(saleId);
        pWriter.WriteLong(saleId);
        return pWriter;
    }

    public static PacketWriter Initialize()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Initialize);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter LoadPremiumShopCategory(List<MeretMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.LoadShopCategory);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(1);
        foreach (MeretMarketItem item in items)
        {
            pWriter.WriteByte(1);
            pWriter.WriteByte(); // bool for ugc item or not
            pWriter.WriteInt(item.MarketId);
            pWriter.WriteLong();
            WriteMeretMarketItem(pWriter, item);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
            foreach (MeretMarketItem additionalItem in item.AdditionalQuantities)
            {
                pWriter.WriteByte(1);
                WriteMeretMarketItem(pWriter, additionalItem);
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteByte();
            }
        }
        return pWriter;
    }

    public static PacketWriter LoadUGCShopCategory(List<UGCMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.LoadShopCategory);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(1);
        foreach (UGCMarketItem item in items)
        {
            pWriter.WriteByte(1);
            WriteUGCMarketItem(pWriter, item);
        }
        return pWriter;
    }

    public static PacketWriter Purchase(int premiumMarketId, long ugcMarketItemId, long price, int totalQuantity, int itemIndex = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Purchase);
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
        pWriter.WriteUnicodeString("");
        pWriter.WriteUnicodeString("");
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
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Home);
        pWriter.WriteByte();
        pWriter.WriteByte(10);
        pWriter.WriteByte(1);
        pWriter.WriteByte(14);
        pWriter.WriteByte();
        pWriter.WriteByte();

        foreach (MeretMarketItem item in items)
        {
            pWriter.WriteByte(1);
            pWriter.WriteByte();
            pWriter.WriteInt(item.MarketId);
            pWriter.WriteLong();
            WriteMeretMarketItem(pWriter, item);

            pWriter.WriteByte(1);
            pWriter.WriteString(item.PromoName);
            pWriter.WriteLong(item.PromoBannerBeginTime);
            pWriter.WriteLong(item.PromoBannerEndTime);
            pWriter.WriteByte();
            pWriter.WriteBool(item.ShowSaleTime);
            pWriter.WriteInt();
            pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
            foreach (MeretMarketItem additionalItem in item.AdditionalQuantities)
            {
                pWriter.WriteByte(1);
                WriteMeretMarketItem(pWriter, additionalItem);
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteByte();
            }
        }

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

    public static PacketWriter OpenDesignShop(List<UGCMarketItem> promoItems, List<UGCMarketItem> newItems)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.OpenDesignShop);
        pWriter.WriteInt(promoItems.Count + newItems.Count);
        foreach (UGCMarketItem item in promoItems)
        {
            pWriter.WriteByte(1);
            WriteUGCMarketItem(pWriter, item, UGCMarketItemHomeCategory.Promoted);
        }
        foreach (UGCMarketItem item in newItems)
        {
            pWriter.WriteByte(1);
            WriteUGCMarketItem(pWriter, item, UGCMarketItemHomeCategory.New);
        }
        return pWriter;
    }

    public static PacketWriter LoadCart()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.LoadCart);
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteInt(3);
        pWriter.WriteInt(10);
        return pWriter;
    }

    public static void WriteMeretMarketItem(PacketWriter pWriter, MeretMarketItem item)
    {
        pWriter.WriteInt(item.MarketId);
        pWriter.WriteByte(2);
        pWriter.WriteUnicodeString(item.ItemName);
        pWriter.WriteByte(1);
        pWriter.WriteInt(item.ParentMarketId);
        pWriter.WriteInt(254);
        pWriter.WriteInt(); // promo bool
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
        string bannerName = "";
        if (item.Banner != null)
        {
            bannerName = item.Banner.Name;
        }
        pWriter.WriteString(bannerName);
        pWriter.WriteString("");
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

    public static void WriteUGCMarketItem(PacketWriter pWriter, UGCMarketItem item, UGCMarketItemHomeCategory category = UGCMarketItemHomeCategory.None)
    {
        pWriter.WriteByte(1);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.Write(item.Status);
        pWriter.WriteInt(item.Item.Id);
        pWriter.WriteInt(35);
        pWriter.WriteByte(0);
        pWriter.WriteInt(0);
        pWriter.WriteLong(item.Price);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(item.SalesCount);
        pWriter.WriteInt();
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteLong(item.ListingExpirationTimestamp); // TODO: Change name
        pWriter.WriteInt();
        pWriter.WriteLong(item.PromotionExpirationTimestamp);
        pWriter.WriteLong();
        pWriter.WriteLong(item.CreationTimestamp);
        pWriter.WriteInt(2);
        pWriter.WriteLong(item.SellerAccountId);
        pWriter.WriteLong(item.SellerCharacterId);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(item.SellerCharacterName);
        pWriter.WriteUnicodeString(string.Join(",", item.Tags.ToArray()) + ", " + item.Item.UGC.Name);
        pWriter.WriteUnicodeString(item.Description);
        pWriter.WriteUnicodeString(item.Item.UGC.CharacterName);
        pWriter.WriteLong(item.Item.UGC.Uid);
        pWriter.WriteUnicodeString(item.Item.UGC.Guid.ToString());
        pWriter.WriteUnicodeString(item.Item.UGC.Name);
        pWriter.WriteByte(1);
        pWriter.WriteInt(2);
        pWriter.WriteLong(item.Item.UGC.AccountId);
        pWriter.WriteLong(item.Item.UGC.CharacterId);
        pWriter.WriteUnicodeString(item.Item.UGC.CharacterName);
        pWriter.WriteLong(item.Item.UGC.CreationTime);
        pWriter.WriteUnicodeString(item.Item.UGC.Url);
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
