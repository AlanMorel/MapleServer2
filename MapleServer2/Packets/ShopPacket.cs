using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ShopPacket
{
    private enum Mode : byte
    {
        Open = 0,
        LoadProducts = 1,
        Buyback = 3,
        Buy = 4,
        Sell = 5,
        EndLoad = 6,
        AddRepurchase = 7,
        RemoveRepurchase = 8,
        Refresh = 10
    }

    public static PacketWriter Open(Shop shop, int npcId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.Open);
        pWriter.WriteInt(npcId);
        pWriter.WriteInt(shop.Id);
        pWriter.WriteLong(shop.NextRestock);
        pWriter.WriteInt();
        pWriter.WriteShort(4); // item count
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
        if (shop.CanRestock)
        {
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt(); // restock cost
            pWriter.WriteBool(false);
            pWriter.WriteInt();
            pWriter.WriteByte(); // shop type again?
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
        }
        return pWriter;
    }

    public static PacketWriter EndLoad()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Shop);
        pWriter.Write(Mode.EndLoad);
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
        pWriter.Write(Mode.Buy);
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
        pWriter.Write(Mode.Sell);
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
        pWriter.Write(Mode.LoadProducts);
        pWriter.WriteByte(1); // quantity of shop items. GMS2 loads one item at a time, while KMS2 does all.
        pWriter.WriteClass(product);
        return pWriter;
    }
}
