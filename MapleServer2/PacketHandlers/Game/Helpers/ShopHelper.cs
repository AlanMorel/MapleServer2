using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
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

        session.Send(ShopPacket.Open(shop, npcId));
        foreach (ShopItem shopItem in shop.Items)
        {
            session.Send(ShopPacket.LoadProducts(shopItem));
        }

        session.Send(ShopPacket.EndLoad());
        session.Send(SystemShopPacket.Open());
    }
}
