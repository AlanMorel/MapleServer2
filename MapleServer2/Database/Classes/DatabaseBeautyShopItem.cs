using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseBeautyShopItem : DatabaseTable
{
    public DatabaseBeautyShopItem() : base("beauty_shop_items") { }

    public ShopItem FindByUid(long uid)
    {
        return ReadShopItem(QueryFactory.Query(TableName).Where("uid", uid).Get().FirstOrDefault());
    }

    public long Insert(BeautyItem beauty, int shopId)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            item_id = beauty.ItemId,
            shop_id = shopId,
            gender = beauty.Gender,
            flag = beauty.Flag,
            required_level = beauty.Flag,
            required_achievement_id = beauty.RequiredAchievementId,
            required_achievement_grade = beauty.RequiredAchievementGrade,
            currency_type = beauty.TokenType,
            currency_cost = beauty.TokenCost,
            required_item_id = beauty.RequiredItemId
        });
    }
    
    public List<BeautyShopItem> FindAllByShopId(int shopId)
    {
        List<BeautyShopItem> items = new();
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("shop_id", shopId).Get();
        foreach (dynamic result in results)
        {
            items.Add(ReadShopItem(result));
        }

        return items;
    }

    public BeautyShopItem FindByItemId(int itemId)
    {
        dynamic result = QueryFactory.Query(TableName).Where("item_id", itemId).FirstOrDefault();

        return result == null ? null : ReadShopItem(result);
    }

    public List<BeautyShopItem> FindAllByShopIdAndGender(int shopId, Gender gender)
    {
        return FindAllByShopId(shopId).Where(x => x.Gender == gender).ToList();
    }

    private static BeautyShopItem ReadShopItem(dynamic data)
    {
        return new BeautyShopItem(data);
    }
}
