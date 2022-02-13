namespace MapleServer2.Database.Types;

public class UgcMapExtensionSaleEvent
{
    public int Id { get; set; }
    public int DiscountAmount { get; set; }

    public UgcMapExtensionSaleEvent() { }

    public UgcMapExtensionSaleEvent(int id, int discountAmount)
    {
        Id = id;
        DiscountAmount = discountAmount;
    }
}
