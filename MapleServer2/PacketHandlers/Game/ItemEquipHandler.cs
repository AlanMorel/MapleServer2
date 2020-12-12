using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class ItemEquipHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.ITEM_EQUIP;

        public ItemEquipHandler(ILogger<ItemEquipHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();

            switch (function) {
                case 0:
                    HandleEquipItem(session, packet);
                    break;
                case 1:
                    HandleUnequipItem(session, packet);
                    break;
            }
        }

        private void HandleEquipItem(GameSession session, PacketReader packet) {
            long itemUid = packet.ReadLong();
            string equipSlotStr = packet.ReadUnicodeString();
            if (!Enum.TryParse(equipSlotStr, out ItemSlot equipSlot)) {
                logger.Warning("Unknown equip slot: " + equipSlotStr);
                return;
            }

            // Remove the item from the users inventory
            session.Inventory.Remove(itemUid, out Item item);
            session.Send(ItemInventoryPacket.Remove(itemUid));

            // TODO: Move unequipped item into the correct slot
            // Move previously equipped item back to inventory
            if (session.Player.Equips.Remove(equipSlot, out Item prevItem)) {
                session.Inventory.Add(prevItem);
                session.Send(ItemInventoryPacket.Add(prevItem));
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, prevItem));
            }

            // Equip new item
            session.Player.Equips[equipSlot] = item;
            session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, item, equipSlot));

            // TODO - Increase stats based on the item stats itself
            session.Player.Stats.CritRate.Max += 12;
            session.Player.Stats.CritRate.Total += 12;

            session.Player.Stats.MinAtk.Max += 15;
            session.Player.Stats.MinAtk.Total += 15;

            session.Player.Stats.MaxAtk.Max += 17;
            session.Player.Stats.MaxAtk.Total += 17;

            session.Send(FieldObjectPacket.SetStats(session.FieldPlayer));
        }

        private void HandleUnequipItem(GameSession session, PacketReader packet) {
            long itemUid = packet.ReadLong();

            foreach ((ItemSlot slot, Item item) in session.Player.Equips) {
                if (itemUid != item.Uid) continue;
                if (session.Player.Equips.Remove(slot, out Item unequipItem)) {
                    session.Inventory.Add(unequipItem);
                    session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, unequipItem));
                    session.Send(ItemInventoryPacket.Add(unequipItem));
                    break;
                }
            }
        }
    }
}