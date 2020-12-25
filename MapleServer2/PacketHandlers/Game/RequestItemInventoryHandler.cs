using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemInventoryHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_INVENTORY;

        public RequestItemInventoryHandler(ILogger<RequestItemInventoryHandler> logger) : base(logger) { }

        private enum RequestItemInventoryMode : byte
        {
            Move = 0x3,
            Drop = 0x4,
            DropBound = 0x5,
            Sort = 0xA
        };

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestItemInventoryMode mode = (RequestItemInventoryMode)packet.ReadByte();
            Inventory inventory = session.Player.Inventory;

            switch (mode)
            {
                case RequestItemInventoryMode.Move:
                    HandleMove(session, packet, inventory);
                    break;

                case RequestItemInventoryMode.Drop:
                    HandleDrop(session, packet, inventory);
                    break;
                case RequestItemInventoryMode.DropBound:
                    HandleDropBound(session, packet, inventory);
                    break;
                case RequestItemInventoryMode.Sort:
                    HandleSort(session, packet, inventory);
                    break;
                default:
                    logger.LogDebug($"Unknown RequestItemInventoryMode: " + mode);
                    break;
            }
        }

        private void HandleMove(GameSession session, PacketReader packet, Inventory inventory)
        {
            long uid = packet.ReadLong();
            short dstSlot = packet.ReadShort();

            Tuple<long, short> result = inventory.Move(uid, dstSlot);
            if (result == null)
            {
                return;
            }

            session.Send(ItemInventoryPacket.Move(result.Item1, result.Item2, uid, dstSlot));
        }

        private void HandleDrop(GameSession session, PacketReader packet, Inventory inventory)
        {
            // TODO: Make sure items are tradable?
            long uid = packet.ReadLong();
            int amount = packet.ReadInt();
            int remaining = inventory.Remove(uid, out Item droppedItem, amount);

            if (remaining < 0)
            {
                return; // Removal failed
            }

            if (remaining > 0)
            {
                session.Send(ItemInventoryPacket.Update(uid, remaining));
            }
            else
            {
                session.Send(ItemInventoryPacket.Remove(uid));
            }

            session.FieldManager.AddItem(session, droppedItem);
        }

        private void HandleDropBound(GameSession session, PacketReader packet, Inventory inventory)
        {
            long uid = packet.ReadLong();

            if (inventory.Remove(uid, out Item droppedItem) != 0)
            {
                return; // Removal from inventory failed
            }

            session.Send(ItemInventoryPacket.Remove(uid));

            // Allow dropping bound items for now
            session.FieldManager.AddItem(session, droppedItem);
        }

        private void HandleSort(GameSession session, PacketReader packet, Inventory inventory)
        {
            var tab = (InventoryType)packet.ReadShort();
            inventory.Sort(tab);

            session.Send(ItemInventoryPacket.ResetTab(tab));
            session.Send(ItemInventoryPacket.LoadItemsToTab(tab, inventory.GetItems(tab)));
        }
    }
}