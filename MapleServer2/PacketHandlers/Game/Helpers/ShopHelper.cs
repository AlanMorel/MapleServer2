using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MySqlX.XDevAPI;
using Serilog;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ShopHelper
{
    public static void OpenSystemShop(GameSession session, int shopId, int npcId)
    {
        Shop shop = DatabaseManager.Shops.FindById(shopId);
        if (shop is null)
        {
            Log.Logger.ForContext(typeof(ShopHelper)).Warning("Unknown shop ID: {shopID}", shopId);
            return;
        }

        session.Send(ShopPacket.Open(shop));
        foreach (ShopItem shopItem in shop.Items)
        {
            session.Send(ShopPacket.LoadProducts(new(){shopItem}));
        }

        //session.Send(ShopPacket.LoadBuybackItemCount((short) session.Player.BuyBackItems.Count));
        session.Player.ShopId = shopId;
        session.Send(SystemShopPacket.Open());
    }

    public static Shop GetShopInstance(Shop serverShop, Player player)
    {
        if (!player.Shops.ContainsKey(serverShop.Id))
        {
            if (!player.ShopLogs.TryGetValue(serverShop.Id, out PlayerShopLog? log))
            {
                log = new(serverShop, player);
                player.ShopLogs.Add(serverShop.Id, log);
            }
            
            player.Shops[serverShop.Id] = new(serverShop, player, log);
            player.Shops[serverShop.Id].Items = GetShopItems(serverShop, player);
        }
        
        // if expired, delete old entries and create new ones
        if (player.Shops[serverShop.Id].RestockTime <= TimeInfo.Now())
        {
            _ = RecreateShop(player, serverShop);
        }

        return player.Shops[serverShop.Id];
    }

    public static Shop RecreateShop(Player player, Shop serverShop)
    {
        DatabaseManager.ShopLogs.Delete(player.ShopLogs[serverShop.Id].Uid);
        foreach (ShopItem shopItem in player.Shops[serverShop.Id].Items)
        {
            if (player.ShopItemLogs.ContainsKey(shopItem.ShopItemUid))
            {
                DatabaseManager.ShopItemLogs.Delete(player.ShopItemLogs[shopItem.ShopItemUid].Uid);
                DatabaseManager.Items.Delete(player.ShopItemLogs[shopItem.ShopItemUid].ItemUid);
                player.ShopItemLogs.Remove(shopItem.ShopItemUid);
            }
        }
        player.ShopLogs.Remove(serverShop.Id);
        // create new shop instance
        player.ShopLogs[serverShop.Id] = new(serverShop, player);
        player.Shops[serverShop.Id] = new(serverShop, player, player.ShopLogs[serverShop.Id]);
        player.Shops[serverShop.Id].Items = GetShopItems(serverShop, player);
        //player.Shops[serverShop.Id].RestockTime += 60;
        Console.WriteLine($"Shop expiration: {player.Shops[serverShop.Id].RestockTime}, Time Now: {TimeInfo.Now()}");
        player.Session.SendNotice($"Shop expiration: {player.Shops[serverShop.Id].RestockTime}, Time Now: {TimeInfo.Now()}");
        return player.Shops[serverShop.Id];
    }

    private static List<ShopItem> GetShopItems(Shop serverShop, Player player)
    {
        List<ShopItem> shopItems = serverShop.Items;
        if (player.Shops[serverShop.Id].Items.Count > 0)
        {
            return player.Shops[serverShop.Id].Items;
        }

        List<PlayerShopItemLog> itemLogs = player.ShopItemLogs.Values.Where(x => x.ShopId == serverShop.Id).ToList();
        List<ShopItem> instanceShopItems = new();
        if (itemLogs.Count > 1)
        {
            foreach (PlayerShopItemLog itemLog in itemLogs)
            {
                ShopItem shopItem = shopItems.FirstOrDefault(x => x.ShopItemUid == itemLog.ShopItemUid);
                if (shopItem is null)
                {
                    continue;
                }

                shopItem.StockPurchased = itemLog.StockPurchased;
                shopItem.Item = DatabaseManager.Items.FindByUid(itemLog.ItemUid);
                instanceShopItems.Add(shopItem);
            }
            return instanceShopItems;
        }

        // ?? huh above is good but cant be in the same functino with below. 
        
        if (!serverShop.PersistantInventory)
        {
            shopItems = shopItems.OrderBy(x => Random.Shared.Next()).Take(12).ToList();
        }
        
        foreach (ShopItem shopItem in shopItems)
        {
            Item item = null;
            if (!player.ShopItemLogs.TryGetValue(shopItem.ShopItemUid, out PlayerShopItemLog itemLog))
            {
                itemLog = new(shopItem, player, serverShop, out item);

            }
            shopItem.Item = item;
            shopItem.StockPurchased = player.ShopItemLogs[shopItem.ShopItemUid].StockPurchased;
        }
        return shopItems;
    }
}
