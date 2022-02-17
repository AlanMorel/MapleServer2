using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseMeretMarket : DatabaseTable
{
    public DatabaseMeretMarket() : base("meret_market_items") { }

    public List<PremiumMarketItem> FindAllByCategory(MeretMarketSection section, MeretMarketCategory category, GenderFlag gender, JobFlag jobFlag, string searchString)
    {
        List<PremiumMarketItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Get();
        if (section != MeretMarketSection.All)
        {
            results = QueryFactory.Query(TableName).Where("section", (int) section).Get();
        }

        foreach (dynamic data in results)
        {
            if (category != MeretMarketCategory.None && (MeretMarketCategory) data.category != category)
            {
                continue;
            }

            if (data.parent_market_id != 0)
            {
                continue;
            }
            PremiumMarketItem meretMarketItem = ReadMeretMarketItem(data);

            if (!meretMarketItem.ItemName.ToLower().Contains(searchString.ToLower()))
            {
                continue;
            }

            List<Job> jobs = ItemMetadataStorage.GetRecommendJobs(meretMarketItem.ItemId);
            if (!JobHelper.CheckJobFlagForJob(jobs, jobFlag))
            {
                continue;
            }

            if (!MeretMarketHelper.CheckGender(gender, meretMarketItem.ItemId))
            {
                continue;
            }

            if (meretMarketItem.BannerId != 0)
            {
                meretMarketItem.Banner = DatabaseManager.Banners.FindById(meretMarketItem.BannerId);
            }

            items.Add(meretMarketItem);
        }

        return items;
    }

    public PremiumMarketItem FindById(int id)
    {
        return ReadMeretMarketItem(QueryFactory.Query(TableName).Where("market_id", id).Get().FirstOrDefault());
    }

    private MeretMarketItem ReadMeretMarketItem(dynamic data)
    {
        PremiumMarketItem item = new(data);

        //Find additional quantities
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("parent_market_id", item.MarketId).Get();
        foreach (dynamic result in results)
        {
            PremiumMarketItem meretMarketItem = ReadAdditionalQuantityMarketItem(result);
            item.AdditionalQuantities.Add(meretMarketItem);
        }

        return item;
    }

    private static PremiumMarketItem ReadAdditionalQuantityMarketItem(dynamic data)
    {
        return new(data);
    }
}
