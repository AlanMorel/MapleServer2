using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ShopPacket
{
    private enum ShopMode : byte
    {
        Open = 0x0,
        LoadProducts = 0x1,
        Buy = 0x4,
        Sell = 0x5,
        Reload = 0x6
    }

    public static PacketWriter Open(Shop shop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(ShopMode.Open);
        pWriter.WriteInt(shop.Uid);
        pWriter.WriteInt(shop.Id);
        pWriter.WriteLong(shop.NextRestock);
        pWriter.WriteInt();
        pWriter.WriteShort(15);
        pWriter.WriteInt(shop.Category);
        pWriter.WriteBool(false);
        pWriter.WriteBool(shop.RestrictSales);
        pWriter.WriteBool(shop.CanRestock);
        pWriter.WriteBool(false);
        pWriter.Write(shop.ShopType);
        pWriter.WriteBool(shop.AllowBuyback);
        pWriter.WriteBool(false);
        pWriter.WriteBool(false);
        pWriter.WriteBool(false);
        pWriter.WriteString(shop.Name);

        return pWriter;
    }

    public static PacketWriter Reload()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(ShopMode.Reload);
        pWriter.WriteByte();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter Close()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.WriteShort();

        return pWriter;
    }

    public static PacketWriter Buy(int itemId, int quantity, int price, ShopCurrencyType shopCurrencyType)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(ShopMode.Buy);
        pWriter.WriteInt(itemId);
        pWriter.WriteInt(quantity);
        pWriter.WriteInt(price * quantity);
        pWriter.Write(shopCurrencyType);
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter Sell(Item item, int quantity)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(ShopMode.Sell);
        pWriter.WriteInt(quantity);
        pWriter.WriteShort();
        pWriter.WriteInt(item.Id);
        pWriter.WriteByte(1);
        pWriter.WriteByte(1);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteItem(item);

        return pWriter;
    }

    public static PacketWriter LoadProducts(ShopItem product)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(ShopMode.LoadProducts);
        pWriter.WriteByte(1);
        pWriter.WriteInt(product.Uid);
        pWriter.WriteInt(product.ItemId);
        pWriter.Write(product.TokenType);
        pWriter.WriteInt(product.RequiredItemId);
        pWriter.WriteInt();
        pWriter.WriteInt(product.Price);
        pWriter.WriteInt(product.SalePrice);
        pWriter.WriteByte(product.ItemRank);
        pWriter.Write(0xEFDA5D2D);
        pWriter.WriteInt(product.StockCount);
        pWriter.WriteInt(product.StockPurchased);
        pWriter.WriteInt(product.GuildTrophy);
        pWriter.WriteString(product.Category);
        pWriter.WriteInt(product.RequiredAchievementId);
        pWriter.WriteInt(product.RequiredAchievementGrade);
        pWriter.WriteByte(product.RequiredChampionshipGrade);
        pWriter.WriteShort(product.RequiredChampionshipJoinCount);
        pWriter.WriteByte(product.RequiredGuildMerchantType);
        pWriter.WriteShort(product.RequiredGuildMerchantLevel);
        pWriter.WriteBool(false);
        pWriter.WriteShort(product.Quantity);
        pWriter.WriteByte(1);
        pWriter.Write(product.Flag);
        pWriter.WriteString(product.TemplateName);
        pWriter.WriteShort(product.RequiredQuestAlliance);
        pWriter.WriteInt(product.RequiredFameGrade);
        pWriter.WriteBool(product.AutoPreviewEquip);
        pWriter.WriteByte();
        pWriter.WriteItem(new(product.ItemId, product.Quantity, product.ItemRank, false));

        return pWriter;
    }
}
