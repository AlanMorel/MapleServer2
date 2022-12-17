using MapleServer2.Database;
using MapleServer2.Types;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

namespace MapleServer2.Managers;

public class ShopManager
{
    private readonly Dictionary<int, Shop> ShopList;

    public ShopManager()
    {
        ShopList = new();
    }

    public void AddShop(Shop shop) => ShopList.Add(shop.Id, shop);

    public void UpdateShops()
    {
        foreach (Shop shop in ShopList.Values)
        {
            if (!shop.CanRestock)
            {
                continue;
            }

            while (TimeInfo.Now() > shop.RestockTime)
            {
                shop.RestockTime += shop.RestockMinInterval * 60; // convert to seconds
            }
            DatabaseManager.Shops.UpdateRestockTime(shop);

            TaskScheduler.Instance.ScheduleTask(shop.RestockTime, shop.RestockMinInterval, shop.Update);
        }
    }
}
