using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseShopItem : DatabaseTable
    {
        public DatabaseShopItem() : base("shop_items") { }

        public ShopItem FindByUid(long uid) => ReadShopItem(QueryFactory.Query(TableName).Where("uid", uid).Get().FirstOrDefault());

        public List<ShopItem> FindAllByShopUid(long shopUid)
        {
            List<ShopItem> items = new List<ShopItem>();
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("shop_uid", shopUid).Get();
            foreach (dynamic result in results)
            {
                items.Add(ReadShopItem(result));
            }
            return items;
        }

        private static ShopItem ReadShopItem(dynamic data)
        {
            return new ShopItem(
                data.uid,
                data.auto_preview_equip,
                data.category,
                data.flag,
                data.guild_trophy,
                data.item_id,
                data.item_rank,
                data.price,
                data.quantity,
                data.required_achievement_grade,
                data.required_achievement_id,
                data.required_championship_grade,
                data.required_championship_join_count,
                data.required_fame_grade,
                data.required_guild_merchant_level,
                data.required_guild_merchant_type,
                data.required_item_id,
                data.required_quest_alliance,
                data.sale_price,
                data.stock_count,
                data.stock_purchased,
                data.template_name,
                data.token_type);
        }
    }
}
