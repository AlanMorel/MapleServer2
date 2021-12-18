using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MeretMarketPacket
{
    private enum MeretMarketMode : byte
    {
        LoadPersonalListings = 0xB,
        LoadSales = 0xC,
        ListItem = 0xD,
        UnlistItem = 0xF,
        SetExpiration = 0x15,
        Premium = 0x1B,
        Purchase = 0x1E,
        Initialize = 016,
        Home = 0x65,
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

        foreach(UGCMarketSale sale in sales)
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

    public static PacketWriter SetExpiration(UGCMarketItem item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.SetExpiration);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.WriteLong(item.Id);
        pWriter.Write(item.Status);
        pWriter.WriteByte();
        pWriter.WriteLong(item.CreationTimestamp * 2);
        return pWriter;
    }

    public static PacketWriter Initialize()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Initialize);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter Premium(List<MeretMarketItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Premium);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(items.Count);
        pWriter.WriteInt(1);
        foreach (MeretMarketItem item in items)
        {
            pWriter.WriteByte(1);
            pWriter.WriteByte();
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

    public static PacketWriter Purchase(MeretMarketItem item, int itemIndex, int totalQuantity)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
        pWriter.Write(MeretMarketMode.Purchase);
        pWriter.WriteByte((byte) totalQuantity);
        pWriter.WriteInt(item.MarketId);
        pWriter.WriteLong();
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteInt(itemIndex);
        pWriter.WriteInt(totalQuantity);
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteUnicodeString("");
        pWriter.WriteUnicodeString("");
        pWriter.WriteLong(item.SalePrice);
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

    public static void WriteUGCMarketItem(PacketWriter pWriter, UGCMarketItem item)
    {
        pWriter.WriteByte(1);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.WriteInt();
        pWriter.WriteLong(item.Id);
        pWriter.Write(item.Status);
        pWriter.WriteInt(item.Item.Id);
        pWriter.WriteInt(35);
        pWriter.WriteByte();
        pWriter.WriteInt(5);
        pWriter.WriteLong(item.Price);
        pWriter.WriteInt(10);
        pWriter.WriteInt(15);
        pWriter.WriteInt(20); // amount sold
        pWriter.WriteInt(25);
        pWriter.WriteLong(item.CreationTimestamp); // listed time
        pWriter.WriteLong(item.CreationTimestamp); // listed time
        pWriter.WriteLong(item.CreationTimestamp * 2); // expiration time
        pWriter.WriteInt(30);
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteLong(item.CreationTimestamp); // submitted time
        pWriter.WriteInt(2);
        pWriter.WriteLong(item.SellerAccountId);
        pWriter.WriteLong(item.SellerCharacterId);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(item.SellerCharacterName);
        pWriter.WriteUnicodeString(string.Join(",", item.Tags.ToArray()) + ", " + item.Item.UGC.Name);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString(item.Item.UGC.Name);
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
        pWriter.WriteByte();
    }
}
