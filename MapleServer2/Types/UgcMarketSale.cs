using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class UgcMarketSale
{
    public long Id;
    public long Price;
    public long Profit;
    public string ItemName;
    public long SoldTimestamp;
    public long SellerCharacterId;

    public UgcMarketSale() { }

    public UgcMarketSale(long price, string itemName, long sellerCharacterId)
    {
        Price = price;
        ItemName = itemName;
        SellerCharacterId = sellerCharacterId;
        float fee = float.Parse(ConstantsMetadataStorage.GetConstant("UGCShopProfitFee")) * price;
        Profit = (long) (price - fee);
        SoldTimestamp = TimeInfo.Now();
        Id = DatabaseManager.UgcMarketSales.Insert(this);
        GameServer.UgcMarketManager.AddSale(this);
    }

    public UgcMarketSale(long id, long price, long profit, string itemName, long soldTimestamp, long sellerCharacterId)
    {
        Id = id;
        Price = price;
        Profit = profit;
        ItemName = itemName;
        SoldTimestamp = soldTimestamp;
        SellerCharacterId = sellerCharacterId;
    }
}
