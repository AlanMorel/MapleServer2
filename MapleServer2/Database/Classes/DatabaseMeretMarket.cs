using Maple2Storage.Enums;
using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMeretMarket : DatabaseTable
{
    public DatabaseMeretMarket() : base("meret_market_items") { }

    public List<MeretMarketItem> FindAllByCategoryId(MeretMarketCategory category)
    {
        List<MeretMarketItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("category", (int) category).Get();
        foreach (dynamic data in results)
        {
            MeretMarketItem meretMarketItem = ReadMeretMarketItem(data);
            if (meretMarketItem.BannerId != 0)
            {
                meretMarketItem.Banner = DatabaseManager.Banners.FindById(meretMarketItem.BannerId);
            }

            items.Add(meretMarketItem);
        }

        return items;
    }

    public MeretMarketItem FindById(int id)
    {
        return ReadMeretMarketItem(QueryFactory.Query(TableName).Where("market_id", id).Get().FirstOrDefault());
    }

    private static MeretMarketItem ReadMeretMarketItem(dynamic data)
    {
        return new(data);
    }
}
