using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class UGCMarketSale
{
    public long Id;
    public long Price;
    public long Profit;
    public string ItemName;
    public long SoldTimestamp;
    public long SellerCharacterId;

    public UGCMarketSale() { }

    public UGCMarketSale(long id, long price, long profit, string itemName, long soldTimestamp, long sellerCharacterId)
    {
        Id = id;
        Price = price;
        Profit = profit;
        ItemName = itemName;
        SoldTimestamp = soldTimestamp;
        SellerCharacterId = sellerCharacterId;
    }
}
