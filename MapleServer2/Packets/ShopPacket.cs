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

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteShort(0);
            return pWriter;
        }
    }
}
