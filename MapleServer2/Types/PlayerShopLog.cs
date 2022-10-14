using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Database.Types;

namespace MapleServer2.Types;

public class PlayerShopLog
{
    public int ShopId;
    public long CreateTime;
    public List<ShopItem> ShopItems;

    public PlayerShopLog() { }

    public PlayerShopLog(int shopId, int createTime, List<ShopItem> items)
    {
        ShopId = shopId;
        CreateTime = createTime;
        ShopItems = items;
    }
}
