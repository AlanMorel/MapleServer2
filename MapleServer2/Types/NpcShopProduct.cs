using MaplePacketLib2.Tools;

namespace MapleServer2.Types
{
    public class NpcShopProduct
    {
        public int UniqueId { get; set; }
        public int ItemId { get; set; }
        /*
         * Currency type
         */
        public byte TokenType { get; set; }
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
        public byte Flag { get; set; }
        /*
         * Required faction type
         */
        public short RequiredQuestAlliance { get; set; }
        /*
         * Required reputation for the above faction type
         */
        public int RequiredFameGrade { get; set; }
        public bool Period { get; set; }

        public NpcShopProduct()
        {
            
        }

        public void Encode(PacketWriter pWriter) {
            pWriter.WriteInt(UniqueId);
            pWriter.WriteInt(ItemId);
            pWriter.WriteByte(TokenType);
            pWriter.WriteInt(RequiredItemId);
            pWriter.WriteInt();
            pWriter.WriteInt(Price);
            pWriter.WriteInt(SalePrice);
            pWriter.WriteByte(ItemRank);
            pWriter.WriteInt();
            pWriter.WriteInt(StockCount);
            pWriter.WriteInt(StockPurchased);
            pWriter.WriteInt(GuildTrophy);
            pWriter.WriteString(Category);
            pWriter.WriteInt(RequiredAchievementId);
            pWriter.WriteInt(RequiredAchievementGrade);
            pWriter.WriteByte(RequiredChampionshipGrade);
            pWriter.WriteShort(RequiredChampionshipJoinCount);
            pWriter.WriteByte(RequiredGuildMerchantType);
            pWriter.WriteShort(RequiredGuildMerchantLevel);
            pWriter.WriteBool(false);
            pWriter.WriteShort(Quantity);
            pWriter.WriteByte(1);
            pWriter.WriteByte(Flag);
            pWriter.WriteString("uwu");
            pWriter.WriteShort(RequiredQuestAlliance);
            pWriter.WriteInt(RequiredFameGrade);
            pWriter.WriteBool(false);
            // pWriter.WriteBool(Period != null);
            // if (Period != null) {
            //     Period.Write(pWriter);
            // }
        }
    }
}
