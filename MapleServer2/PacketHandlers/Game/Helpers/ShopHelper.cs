using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ShopHelper
{
    public static void OpenShop(GameSession session, int shopId, int npcId)
    {
        Shop shop = DatabaseManager.Shops.FindById(shopId);
        if (shop == null)
        {
            //Logger.Warning("Unknown shop ID: {shopId}", shopId);
            return;
        }
        shop.NpcId = npcId;

        LoadShop(session, shop);

        short itemCount = (short) session.Player.BuyBackItems.Count(x => x != null);
        session.Send(ShopPacket.LoadBuybackItemCount(itemCount));
        if (!shop.DisableBuyback & itemCount > 0)
        {
            session.Send(ShopPacket.AddBuyBackItem(session.Player.BuyBackItems, itemCount));
        }
        session.Player.ShopId = shop.Id;
    }

    public static void LoadShop(GameSession session, Shop shop)
    {
        if (shop.CanRestock)
        {
            shop = GetShopInstance(shop, session.Player);
            session.Send(ShopPacket.Open(shop));
            session.Send(ShopPacket.LoadProducts(shop.Items));
            return;
        }

        session.Send(ShopPacket.Open(shop));
        foreach (ShopItem shopItem in shop.Items)
        {
            session.Send(ShopPacket.LoadProducts(new()
            {
                shopItem
            }));
        }
    }

    private static Shop GetShopInstance(Shop serverShop, Player player)
    {
        if (!player.Shops.ContainsKey(serverShop.Id))
        {
            if (!player.ShopLogs.TryGetValue(serverShop.Id, out PlayerShopLog? log))
            {
                log = new(serverShop, player);
                player.ShopLogs.Add(serverShop.Id, log);
            }

            player.Shops[serverShop.Id] = new(serverShop, log);
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
        player.Shops[serverShop.Id] = new(serverShop, player.ShopLogs[serverShop.Id]);
        player.Shops[serverShop.Id].Items = GetShopItems(serverShop, player);
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

        if (serverShop.PullCount > 0)
        {
            shopItems = shopItems.OrderBy(x => Random.Shared.Next()).Take(serverShop.PullCount).ToList();
        }

        foreach (ShopItem shopItem in shopItems)
        {
            Item item = null;
            if (!player.ShopItemLogs.TryGetValue(shopItem.ShopItemUid, out PlayerShopItemLog itemLog))
            {
                player.ShopItemLogs.Add(shopItem.ShopItemUid, new(shopItem, player, serverShop, out item));
            }
            shopItem.Item = item;
            shopItem.StockPurchased = player.ShopItemLogs[shopItem.ShopItemUid].StockPurchased;
        }
        return shopItems;
    }
}
