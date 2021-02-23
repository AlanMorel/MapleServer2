using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;
using Renci.SshNet;

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
        
        public static Packet Open(int npcTemplateId, int shopId, int shopCategory, string shopName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte();
            pWriter.WriteInt(npcTemplateId);
            pWriter.WriteInt(shopId);
            pWriter.WriteLong(1614265200); // timestamp for next restock
            pWriter.WriteInt();
            pWriter.WriteShort(1);
            pWriter.WriteInt(1); // ShopCategory (916 = Juice)
            pWriter.WriteBool(false); //
            pWriter.WriteBool(true); // restrict sales (default: false)
            pWriter.WriteBool(false); // shop can be restocked
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false); // show buyback tab (default: true)
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteMapleString("shop"); // shopName
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

        public static Packet LoadProducts(Player owner, List<NpcShopItem> products)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP); 
            pWriter.WriteByte((byte) ShopMode.LoadProducts);
            pWriter.WriteByte((byte) products.Count);
            foreach (NpcShopItem product in products)
            {
                product.Encode(pWriter);
            }
            
            return pWriter;
        }
    }
}
