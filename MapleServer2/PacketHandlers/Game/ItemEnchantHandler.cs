using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ItemEnchantHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_ENCHANT;

        public ItemEnchantHandler(ILogger<ItemEnchantHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();

            switch (function)
            {
                case 0: // Sent when opening up enchant ui
                    break;
                case 1:
                    HandleBeginEnchant(session, packet);
                    break;
                case 4:
                    HandleOpheliaEnchant(session, packet);
                    break;
                case 6:
                    HandlePeachyEnchant(session, packet);
                    break;
            }
        }

        private static void HandleBeginEnchant(GameSession session, PacketReader packet)
        {
            byte type = packet.ReadByte();
            long itemUid = packet.ReadLong();

            if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
            {
                session.Send(ItemEnchantPacket.BeginEnchant(type, item));
            }
        }

        private static void HandleOpheliaEnchant(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
            {
                item.Enchants += 5;
                item.Charges += 10;
                session.Send(ItemEnchantPacket.EnchantResult(item));
            }
        }

        private static void HandlePeachyEnchant(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            if (session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
            {
                item.EnchantExp += 5000;
                if (item.EnchantExp >= 10000)
                {
                    item.EnchantExp %= 10000;
                    item.Enchants++;
                }
                session.Send(ItemEnchantPacket.EnchantResult(item));
                session.Send(ItemEnchantPacket.UpdateCharges(item));
            }
        }
    }
}
