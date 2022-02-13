namespace MapleServer2.Database.Types;

public class UgcMapContractSaleEvent
{
    public int Id { get; set; }
    public int DiscountAmount { get; set; }

    public UgcMapContractSaleEvent() { }

    public UgcMapContractSaleEvent(dynamic id, dynamic discountAmount)
    {
        Id = id;
        DiscountAmount = discountAmount;
    }
}
