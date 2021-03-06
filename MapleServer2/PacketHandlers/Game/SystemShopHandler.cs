using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
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
            Close = 0x0,
            Open = 0x1,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte unk = packet.ReadByte(); // Looks like it's always 0x0A
            ShopMode mode = (ShopMode) packet.ReadByte();

            switch (mode)
            {
                case ShopMode.Open:
                    HandleOpen(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session, PacketReader packet)
        {
            int itemId = packet.ReadInt();

            List<Item> playerInventory = new(session.Player.Inventory.Items.Values);

            Item item = playerInventory.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
            {
                return;
            }

            ShopMetadata shop = ShopMetadataStorage.GetShop(item.ShopID);
            session.Send(ShopPacket.Open(shop));
            session.Send(ShopPacket.LoadProducts(shop.Items));
            session.Send(ShopPacket.Reload());
            session.Send(SystemShopPacket.Open());
        }
    }
}
