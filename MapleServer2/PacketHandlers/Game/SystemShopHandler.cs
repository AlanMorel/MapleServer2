using System;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class SystemShopHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.SYSTEM_SHOP;

        public SystemShopHandler(ILogger<SystemShopHandler> logger) : base(logger) { }

        private enum ShopMode : byte
        {
            Arena = 0x03,
            Fishing = 0x04,
            ViaItem = 0x0A
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ShopMode mode = (ShopMode) packet.ReadByte();

            switch (mode)
            {
                case ShopMode.ViaItem:
                    HandleViaItem(session, packet);
                    break;
                case ShopMode.Fishing:
                    HandleFishingShop(session, packet);
                    break;
                case ShopMode.Arena:
                    HandleMapleArenaShop(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleViaItem(GameSession session, PacketReader packet)
        {
            bool openShop = packet.ReadBool();

            if (!openShop)
            {
                return;
            }

            int itemId = packet.ReadInt();

            Item item = session.Player.Inventory.Items.Values.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
            {
                return;
            }

            Shop shop = DatabaseManager.GetShop(item.ShopID);
            if (shop == null)
            {
                Console.WriteLine($"Unknown shop ID: {item.ShopID}");
                return;
            }

            session.Send(ShopPacket.Open(shop));
            foreach (ShopItem shopItem in shop.Items)
            {
                session.Send(ShopPacket.LoadProducts(shopItem));
            }
            session.Send(ShopPacket.Reload());
            session.Send(SystemShopPacket.Open());
        }
        private static void HandleFishingShop(GameSession session, PacketReader packet)
        {
            bool openShop = packet.ReadBool();

            if (!openShop)
            {
                return;
            }

            int shopId = 161;
            OpenSystemShop(session, shopId);
        }

        private static void HandleMapleArenaShop(GameSession session, PacketReader packet)
        {
            bool openShop = packet.ReadBool();

            if (!openShop)
            {
                return;
            }

            int shopId = 168;
            OpenSystemShop(session, shopId);
        }

        private static void OpenSystemShop(GameSession session, int shopId)
        {
            Shop shop = DatabaseManager.GetShop(shopId);

            session.Send(ShopPacket.Open(shop));
            foreach (ShopItem shopItem in shop.Items)
            {
                session.Send(ShopPacket.LoadProducts(shopItem));
            }
            session.Send(ShopPacket.Reload());
            session.Send(SystemShopPacket.Open());
        }
    }
}
