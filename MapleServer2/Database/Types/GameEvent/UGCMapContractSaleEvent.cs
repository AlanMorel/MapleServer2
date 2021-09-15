namespace MapleServer2.Database.Types
{
    public class UGCMapContractSaleEvent
    {
        public int Id { get; set; }
        public int DiscountAmount { get; set; }

        public UGCMapContractSaleEvent() { }

        public UGCMapContractSaleEvent(dynamic id, dynamic discountAmount)
        {
            Id = id;
            DiscountAmount = discountAmount;
        }
    }
}
