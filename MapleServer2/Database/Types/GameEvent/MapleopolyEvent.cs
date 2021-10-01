namespace MapleServer2.Database.Types
{
    public class MapleopolyEvent
    {
        public int Id { get; set; }
        public int TripAmount { get; set; }
        public int ItemId { get; set; }
        public byte ItemRarity { get; set; }
        public int ItemAmount { get; set; }

        public MapleopolyEvent() { }

        public MapleopolyEvent(dynamic id, dynamic tripAmount, dynamic itemId, dynamic itemRarity, dynamic itemAmount)
        {
            Id = id;
            TripAmount = tripAmount;
            ItemId = itemId;
            ItemRarity = itemRarity;
            ItemAmount = itemAmount;
        }
    }
}
