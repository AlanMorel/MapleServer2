using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
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
        
        public static Packet Open(NpcShop shop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte();
            pWriter.WriteInt(shop.TemplateId);
            pWriter.WriteInt(shop.Id);
            pWriter.WriteLong(shop.NextRestock); // timestamp for next restock
            pWriter.WriteInt();
            pWriter.WriteShort(15);
            pWriter.WriteInt(shop.Category); // ShopCategory (916 = Juice)
            pWriter.WriteBool(false); //
            pWriter.WriteBool(shop.RestrictSales); // restrict sales (default: false)
            pWriter.WriteBool(false); // shop can be restocked
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(shop.AllowBuyback); // show buyback tab (default: true)
            pWriter.WriteBool(false);
            pWriter.WriteBool(true); // unknown
            pWriter.WriteBool(false);
            pWriter.WriteMapleString(shop.Name); // shopName
            return pWriter;
        }

        public static Packet Reload()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.Reload);
            pWriter.WriteByte();
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet Buy(int itemId, int quantity, int price, CurrencyType currencyType)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.Buy);
            pWriter.WriteInt(itemId); // Item ID
            pWriter.WriteInt(quantity); // Quantity
            pWriter.WriteInt(price * quantity); // Total price
            pWriter.WriteShort((short) currencyType); // Currency type
            return pWriter;
        }

        public static Packet Sell(long itemUid, int quantity, int price)
        {
            // TODO: Implement selling item to shop
            // PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            Console.WriteLine($"Selling {quantity}x {itemUid} for {price * quantity} mesos");
            // return pWriter;
            return null;
        }

        public static Packet LoadProducts(List<NpcShopItem> products)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP); 
            pWriter.WriteByte((byte) ShopMode.LoadProducts);
            pWriter.WriteByte((byte) products.Count);
            foreach (NpcShopItem product in products)
            {
                product.Write(pWriter);
            }
            
            return pWriter;
        }
    }
}
