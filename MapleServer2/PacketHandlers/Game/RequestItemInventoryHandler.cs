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
                    HandleMove(session, packet);
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
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleMove(GameSession session, PacketReader packet)
        {
            InventoryController.MoveItem(session, packet);
        }

        private void HandleDrop(GameSession session, PacketReader packet, Inventory inventory)
        {
            // TODO: Make sure items are tradable?
            InventoryController.DropItem(session, packet, false);
        }

        private void HandleDropBound(GameSession session, PacketReader packet, Inventory inventory)
        {
            InventoryController.DropItem(session, packet, true);
        }

        private void HandleSort(GameSession session, PacketReader packet, Inventory inventory)
        {
            var tab = (InventoryType)packet.ReadShort();
            InventoryController.SortInventory(session, inventory, tab);
        }
    }
}