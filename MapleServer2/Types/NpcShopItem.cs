using System;
using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Packets;

namespace MapleServer2.Types
{
    public class NpcShopItem
    {
        public int UniqueId { get; set; }

        public int ItemId { get; set; }

        /*
         * Currency type
         */
        public CurrencyType TokenType { get; set; }

        /*
         * Only used when tokenType is type Capsule (1)
         */
        public int RequiredItemId { get; set; }

        /**
         * Current Price (e.g discounted price)
         */
        public int Price { get; set; }

        /*
         * Original Price
         */
        public int SalePrice { get; set; }

        /*
         * Stars
         */
        public byte ItemRank { get; set; }

        /*
         * Total Stock
         */
        public int StockCount { get; set; }

        /*
         * Purchased Stock (remaining stock will show Total - Purchased)
         */
        public int StockPurchased { get; set; }

        /*
         * Must have more than X guild trophies
         */
        public int GuildTrophy { get; set; }

        /*
         * Item Category (found in the item's xml)
         */
        public string Category { get; set; }

        public int RequiredAchievementId { get; set; }

        /*
         * When > 0: "You must have the "X. TrophyNameHere" trophy to do this."
         */
        public int RequiredAchievementGrade { get; set; }

        /*
         * Guild ranking above X
         */
        public byte RequiredChampionshipGrade { get; set; }

        /*
         * Must participate more than X times
         */
        public short RequiredChampionshipJoinCount { get; set; }

        /*
         * 2 = Guild Supply Merchant
         * 3 = Guild Gemstone Merchant
         */
        public byte RequiredGuildMerchantType { get; set; }

        /*
         * The guild <type> merchant must be above level X
         */
        public short RequiredGuildMerchantLevel { get; set; }

        /*
         * Bundle Quantity
         */
        public short Quantity { get; set; }

        /*
         * New, Sale, Event, Hot, etc.
         */
        public ShopItemFlag Flag { get; set; }

        /*
         * Required faction type
         */
        public short RequiredQuestAlliance { get; set; }

        /*
         * Required reputation for the above faction type
         */
        public int RequiredFameGrade { get; set; }

        public NpcShopItem()
        {
        }

        public void Write(PacketWriter pWriter)
        {
            pWriter.WriteInt(UniqueId); // 968620
            pWriter.WriteInt(ItemId);
            pWriter.WriteByte((byte) TokenType); // Currency Type
            pWriter.WriteInt(RequiredItemId); // Only used when tokenType is type Capsule (1)
            pWriter.WriteInt();
            pWriter.WriteInt(Price); // Current Price (e.g discounted price)
            pWriter.WriteInt(SalePrice); // Original Price
            pWriter.WriteByte(ItemRank); // Stars
            pWriter.WriteUInt(0xEFDA5D2D);
            pWriter.WriteInt(StockCount); // Total Stock
            pWriter.WriteInt(StockPurchased); // Purchased Stock (remaining stock will show Total - Purchased)
            pWriter.WriteInt(GuildTrophy); // "More than x guild trophies"
            pWriter.WriteMapleString(Category); // Item Category (found in the item's xml)
            pWriter.WriteInt(RequiredAchievementId);
            pWriter.WriteInt(RequiredAchievementGrade); // When > 0: "You must have the "X. TrophyNameHere" trophy to do this."
            pWriter.WriteByte(RequiredChampionshipGrade); // Guild ranking above X
            pWriter.WriteShort(RequiredChampionshipJoinCount); // Must participate more than X times
            pWriter.WriteByte(RequiredGuildMerchantType); // 2 = "Guild Supply Merchant" 3 = "Guild Gemstone Merchant"
            pWriter.WriteShort(RequiredGuildMerchantLevel); // The guild <type> merchant must be above level X
            pWriter.WriteBool(false);
            pWriter.WriteShort(Quantity); // Bundle Quantity
            pWriter.WriteByte(1);
            pWriter.WriteByte((byte) Flag); // New, Sale, Event, Hot, etc.
            pWriter.WriteByte();
            pWriter.WriteShort(RequiredQuestAlliance); // Required faction
            pWriter.WriteInt(RequiredFameGrade); // Required reputation for the above faction type
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
    }
}
