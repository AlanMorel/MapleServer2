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
        return new MeretMarketItem(
            data.market_id,
            data.banner_id ?? 0,
            data.bonus_quantity,
            data.category,
            data.duration,
            data.flag,
            data.item_id,
            data.item_name,
            data.job_requirement,
            data.max_level_requirement,
            data.min_level_requirement,
            data.pc_cafe,
            data.parent_market_id,
            data.price,
            data.promo_banner_begin_time,
            data.promo_banner_end_time,
            data.promo_flag,
            data.promo_name,
            data.quantity,
            data.rarity,
            data.required_achievement_grade,
            data.required_achievement_id,
            data.restock_unavailable,
            data.sale_price,
            data.sell_begin_time,
            data.sell_end_time,
            data.show_sale_time,
            data.token_type);
    }
}
