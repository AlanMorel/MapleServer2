using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
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
            UnequipItem = 0x01,
            Transprency = 0x03
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
                case RequestGemEquipmentMode.Transprency:
                    HandleTransparency(session, packet);
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
            List<Item> badges = session.Player.Inventory.Badges;
            int index = badges.FindIndex(i => i.GemSlot == item.GemSlot);
            if (index >= 0)
            {
                // Add to inventory
                badges[index].IsEquipped = false;
                InventoryController.Add(session, badges[index], false);

                // Unequip
                badges.RemoveAt(index);
                session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session, (byte) item.GemSlot));
            }

            // Equip
            item.IsEquipped = true;
            badges.Add(item);
            session.FieldManager.BroadcastPacket(GemPacket.EquipItem(session, item));
        }

        private static void HandleUnequipItem(GameSession session, PacketReader packet)
        {
            byte index = packet.ReadByte();

            List<Item> badges = session.Player.Inventory.Badges;
            if (badges.Count < index + 1)
            {
                return;
            }

            Item item = badges[index];

            // Add to inventory
            item.IsEquipped = false;
            InventoryController.Add(session, item, false);

            // Unequip
            bool removed = badges.Remove(item);
            if (removed)
            {
                session.FieldManager.BroadcastPacket(GemPacket.UnequipItem(session, (byte) item.GemSlot));
            }
        }

        private static void HandleTransparency(GameSession session, PacketReader packet)
        {
            byte slot = packet.ReadByte();
            byte[] transparencyBools = packet.Read(10);

            Item item = session.Player.Inventory.Badges[slot];

            item.TransparencyBadgeBools = transparencyBools;

            session.FieldManager.BroadcastPacket(GemPacket.EquipItem(session, item));
        }
    }
}
