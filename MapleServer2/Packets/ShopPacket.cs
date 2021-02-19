using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ShopPacket
    {
        public static Packet Open(int npcTemplateId, int shopCategory, string shopName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);

            // Currently unknown
            const bool idk = false;

            pWriter.WriteInt(npcTemplateId);
            pWriter.WriteInt(128);
            pWriter.WriteLong(DateTimeOffset.Now.ToUnixTimeSeconds()); // current date/time in seconds
            pWriter.WriteInt();
            pWriter.WriteShort(3);
            pWriter.WriteInt(shopCategory); // ShopCategory (916 = Juice)
            pWriter.WriteByte();
            pWriter.WriteBool(false);
            pWriter.WriteBool(idk);
            pWriter.WriteBool(false);
            pWriter.WriteByte();
            pWriter.WriteBool(true);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteUnicodeString(shopName);

            // if (idk) {
            //     pWriter.WriteByte(0);
            //     pWriter.WriteByte(0);
            //     pWriter.WriteInt(0);
            //     pWriter.WriteInt(0);
            //     pWriter.WriteBool(false);
            //     pWriter.WriteInt(0);
            //     pWriter.WriteByte(0);
            //     pWriter.WriteBool(false);
            //     pWriter.WriteBool(false);
            // }

            return pWriter;
        }

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet Buy(int itemId, int quantity)
        {
            // TODO: Implement buying item from shop
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            Console.WriteLine($"Buying {quantity}x {itemId}");
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

        public static Packet LoadProducts(List<NpcShopProduct> products)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) products.Count);
            foreach (NpcShopProduct product in products)
            {
                product.Encode(pWriter);
            }
            return pWriter;
        }
    }
}
