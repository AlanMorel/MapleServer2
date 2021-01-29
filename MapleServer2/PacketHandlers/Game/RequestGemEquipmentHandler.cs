using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestGemEquipmentHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_GEM_EQUIPMENT;

        public RequestGemEquipmentHandler(ILogger<RequestGemEquipmentHandler> logger) : base(logger) { }

        private enum RequestGemEquipmentMode : byte
        {
            EquipItem = 0x00,
            UnequipItem = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestGemEquipmentMode mode = (RequestGemEquipmentMode) packet.ReadByte();

            switch (mode)
            {
                case RequestGemEquipmentMode.EquipItem:
                    HandleEquipItem(session, packet);
                    break;
                case RequestGemEquipmentMode.UnequipItem:
                    HandleUnequipItem(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleEquipItem(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            // Remove from inventory
            bool success = InventoryController.Remove(session, itemUid, out Item item);

            if (!success)
            {
                return;
            }

            // Unequip existing item in slot
            int index = session.Player.Badges.FindIndex(i => i.GemSlot == item.GemSlot);
            if (index >= 0)
            {
                // Add to inventory
                InventoryController.Add(session, session.Player.Badges[index], false);

                // Unequip
                session.Player.Badges.RemoveAt(index);
                session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session, (byte) item.GemSlot));
            }

            // Equip
            session.Player.Badges.Add(item);
            session.FieldManager.BroadcastPacket(GemPacket.EquipItem(session, item));
        }

        private static void HandleUnequipItem(GameSession session, PacketReader packet)
        {
            byte index = packet.ReadByte();

            if (session.Player.Badges.Count < index + 1)
            {
                return;
            }

            Item item = session.Player.Badges[index];

            // Add to inventory
            InventoryController.Add(session, item, false);

            // Unequip
            bool removed = session.Player.Badges.Remove(item);
            if (removed)
            {
                session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session, (byte) item.GemSlot));
            }
        }
    }
}
