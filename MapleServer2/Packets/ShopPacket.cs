using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

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

        public static Packet Open(ShopMetadata shop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.Open);
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

        public static Packet Buy(int itemId, int quantity, int price, byte currencyType)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.Buy);
            pWriter.WriteInt(itemId); // Item ID
            pWriter.WriteInt(quantity); // Quantity
            pWriter.WriteInt(price * quantity); // Total price
            pWriter.WriteShort(currencyType); // Currency type
            
            return pWriter;
        }

        public static Packet Sell(long itemUid, int itemId, int quantity)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.Sell);
            pWriter.WriteInt(quantity);
            pWriter.WriteShort();
            pWriter.WriteInt(itemId);
            pWriter.WriteByte(1);
            pWriter.WriteByte(1);
            pWriter.WriteByte();
            pWriter.WriteInt();
            
            // Write item data
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt(1); // Unknown (amount?)
            pWriter.WriteInt(-1);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // Item creation time
            pWriter.WriteZero(56);
            pWriter.WriteInt(-1);
            pWriter.WriteZero(69);
            pWriter.WriteInt(1);
            pWriter.WriteZero(14);
            pWriter.WriteInt(6);
            pWriter.WriteZero(10);
            pWriter.WriteInt(1);
            pWriter.WriteZero(18);

            return pWriter;
        }

        public static Packet LoadProducts(List<ShopItem> products)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteByte((byte) ShopMode.LoadProducts);
            pWriter.WriteByte((byte) products.Count);
            foreach (ShopItem product in products)
            {
                pWriter.WriteInt(product.UniqueId); // 968620
                pWriter.WriteInt(product.ItemId);
                pWriter.WriteByte(product.TokenType); // Currency Type
                pWriter.WriteInt(product.RequiredItemId); // Only used when tokenType is type Capsule (1)
                pWriter.WriteInt();
                pWriter.WriteInt(product.Price); // Current Price (e.g discounted price)
                pWriter.WriteInt(product.SalePrice); // Original Price
                pWriter.WriteByte(product.ItemRank); // Stars
                pWriter.WriteUInt(0xEFDA5D2D);
                pWriter.WriteInt(product.StockCount); // Total Stock
                pWriter.WriteInt(product.StockPurchased); // Purchased Stock (remaining stock will show Total - Purchased)
                pWriter.WriteInt(product.GuildTrophy); // "More than x guild trophies"
                pWriter.WriteMapleString(product.Category); // Item Category (found in the item's xml)
                pWriter.WriteInt(product.RequiredAchievementId); 
                pWriter.WriteInt(product.RequiredAchievementGrade); // When > 0: "You must have the "X. TrophyNameHere" trophy to do this."
                pWriter.WriteByte(product.RequiredChampionshipGrade); // Guild ranking above X
                pWriter.WriteShort(product.RequiredChampionshipJoinCount); // Must participate more than X times
                pWriter.WriteByte(product.RequiredGuildMerchantType); // 2 = "Guild Supply Merchant" 3 = "Guild Gemstone Merchant"
                pWriter.WriteShort(product.RequiredGuildMerchantLevel); // The guild <type> merchant must be above level X
                pWriter.WriteBool(false);
                pWriter.WriteShort(product.Quantity); // Bundle Quantity
                pWriter.WriteByte(1);
                pWriter.WriteByte(product.Flag); // New, Sale, Event, Hot, etc.
                pWriter.WriteByte();
                pWriter.WriteShort(product.RequiredQuestAlliance); // Required faction
                pWriter.WriteInt(product.RequiredFameGrade); // Required reputation for the above faction type
                pWriter.WriteBool(false);
                
                // Write item data
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteInt(1); // Unknown (amount?)
                pWriter.WriteInt();
                pWriter.WriteInt(-1);
                pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // Item creation time
                pWriter.WriteZero(52);
                pWriter.WriteInt(-1);
                pWriter.WriteZero(102);
                pWriter.WriteInt(1);
                pWriter.WriteZero(28);
                pWriter.WriteLong(1); // Item owner character id (shop items have no owner)
                pWriter.WriteShort(); // Item owner name (shop items have no owner)
                pWriter.WriteZero(12);
            }

            return pWriter;
        }
    }
}
