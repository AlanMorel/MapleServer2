using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ShopPacket
{
    private enum Mode : byte
    {
        Open = 0,
        LoadProducts = 1,
        UpdateProduct = 2,
        Buy = 4,
        LoadBuyBackItemCount = 6,
        AddBuyBackItem = 7,
        RemoveBuyBackItem = 8,
        InstantRestock = 9,
        LoadNew = 14,
        Notice = 15
    }

    public static PacketWriter Open(Shop shop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.Open);
        pWriter.WriteClass(shop);
        return pWriter;
    }

    public static PacketWriter UpdateProduct(ShopItem item, int totalQuantityPurchased)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.UpdateProduct);
        pWriter.WriteInt(item.ShopItemUid);
        pWriter.WriteInt(totalQuantityPurchased);
        return pWriter;
    }

    public static PacketWriter LoadBuybackItemCount(short itemCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.LoadBuyBackItemCount);
        pWriter.WriteShort(itemCount);
        return pWriter;
    }

    public static PacketWriter RemoveBuyBackItem(int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.RemoveBuyBackItem);
        pWriter.WriteInt(index);
        return pWriter;
    }

    public static PacketWriter InstantRestock()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.InstantRestock);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter Buy(int itemId, int quantity, int price, byte rarity, bool toGuildStorage = false)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.Buy);
        pWriter.WriteInt(itemId);
        pWriter.WriteInt(quantity);
        pWriter.WriteInt(price * quantity);
        pWriter.WriteByte(rarity);
        pWriter.WriteBool(toGuildStorage);
        return pWriter;
    }

    public static PacketWriter AddBuyBackItem(BuyBackItem?[] buyBackItems, short itemCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.AddBuyBackItem);
        pWriter.WriteShort(itemCount);
        for (int i = 0; i < itemCount; i++)
        {
            if (buyBackItems[i] is not null)
            {
                pWriter.WriteInt(i);
                pWriter.WriteInt(buyBackItems[i].Item.Id);
                pWriter.WriteByte((byte) buyBackItems[i].Item.Rarity);
                pWriter.WriteLong(buyBackItems[i].Price);
                pWriter.WriteItem(buyBackItems[i].Item);
            }
        }
        return pWriter;
    }

    public static PacketWriter AddBuyBackItem(BuyBackItem buyBackItem, int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.AddBuyBackItem);
        pWriter.WriteShort(1);
        pWriter.WriteInt(index);
        pWriter.WriteInt(buyBackItem.Item.Id);
        pWriter.WriteByte((byte) buyBackItem.Item.Rarity);
        pWriter.WriteLong(buyBackItem.Price);
        pWriter.WriteItem(buyBackItem.Item);
        return pWriter;
    }

    public static PacketWriter LoadProducts(List<ShopItem> products)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.LoadProducts);
        pWriter.WriteByte((byte) products.Count);
        foreach (ShopItem product in products)
        {
            pWriter.WriteClass(product);
        }
        return pWriter;
    }

    public static PacketWriter LoadNew(List<ShopItem> products)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.LoadNew);
        pWriter.WriteByte((byte) products.Count);
        foreach (ShopItem product in products)
        {
            pWriter.WriteInt(product.Item.Id);
            pWriter.WriteBool(false);
            pWriter.WriteByte((byte) product.Item.Rarity);
            pWriter.WriteString(product.Item.Name);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteBool(false); // buy period
            pWriter.WriteItem(product.Item);
        }
        return pWriter;
    }

    public static PacketWriter Notice(ShopNotice notice, int stringId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.Notice);
        pWriter.Write(notice);
        pWriter.WriteByte();
        pWriter.WriteInt(stringId);
        return pWriter;
    }
}
