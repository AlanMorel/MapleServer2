namespace MapleServer2.Database.Types
{
    public class UGCMapExtensionSaleEvent
    {
        public int Id { get; set; }
        public int DiscountAmount { get; set; }

        public UGCMapExtensionSaleEvent() { }

        public UGCMapExtensionSaleEvent(int id, int discountAmount)
        {
            Id = id;
            DiscountAmount = discountAmount;
        }
    }
}
