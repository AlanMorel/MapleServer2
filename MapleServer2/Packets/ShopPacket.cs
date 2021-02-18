using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class ShopPacket
    {
        private enum ShopMode : byte
        {
            Open = 0x0,
            GetProducts = 0x1,
            Buy = 0x4,
            Sell = 0x5,
            Close = 0x6
        }
        
        public static Packet Open(int npcTemplateId, int shopCategory, string shopName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);

            // Currently unknown
            bool idk = false;
            
            pWriter.WriteInt(npcTemplateId);
            pWriter.WriteInt(128);
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds()); // current date/time in seconds
            pWriter.WriteInt(0);
            pWriter.WriteShort(3);
            pWriter.WriteInt(shopCategory); // ShopCategory (916 = Juice)
            pWriter.WriteByte(0);
            pWriter.WriteBool(false);
            pWriter.WriteBool(idk);
            pWriter.WriteBool(false);
            pWriter.WriteByte(0);
            pWriter.WriteBool(true);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteUnicodeString(shopName);
            
            if (idk) {
                pWriter.WriteByte(0);
                pWriter.WriteByte(0);
                pWriter.WriteInt(0);
                pWriter.WriteInt(0);
                pWriter.WriteBool(false);
                pWriter.WriteInt(0);
                pWriter.WriteByte(0);
                pWriter.WriteBool(false);
                pWriter.WriteBool(false);
            }

            return pWriter;
        }

        // public static Packet LoadProducts(List<NpcShopProduct> products)
        // {
        //     PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
        //     pWriter.WriteByte(products.size());
        //     foreach (NpcShopProduct product in products)
        //     {
        //         pWriter.Write(product);
        //     }
        // }
        
        // private void onSendShop(InPacket packet) {
        //     //[SendShop] 28 00 04 24 2D 31 01 05 00 00 00 - 5 is    qty
        //     int mode = packet.decodeByte();
        //     if (mode == 4) { //buy from shop
        //         int id = packet.decodeInt();
        //         int quantity = packet.decodeInt();
        //         ShopHandler.handleBuy(this, id, quantity);
        //     } else if (mode == 5) { //sell to shop
        //         long itemSN = packet.decodeLong();
        //         int quantity = packet.decodeInt();
        //         ShopHandler.handleSell(this, itemSN, quantity);
        //     }
        // }

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteShort(0);
            return pWriter;
        }
    }
}
