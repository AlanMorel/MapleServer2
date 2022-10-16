using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Database.Types;

namespace MapleServer2.Types;

public class PlayerShopLog
{
    public int ShopId;
    public long NextRestockTime;
    public List<ShopItem> ShopItems;

    public PlayerShopLog() { }

    public PlayerShopLog(int shopId, long nextRestockTime)
    {
        ShopId = shopId;
        NextRestockTime = nextRestockTime;
    }
}
