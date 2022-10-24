namespace MapleServer2.Types;

public class BuyBackItem
{
    public readonly long AddedTimestamp;
    public readonly long Price;
    public readonly Item Item;

    public BuyBackItem(Item item, long price)
    {
        AddedTimestamp = TimeInfo.Now();
        Price = price;
        Item = item;
    }
}
