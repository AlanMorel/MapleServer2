using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
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
            RequestItemInventoryMode mode = (RequestItemInventoryMode) packet.ReadByte();
            Inventory inventory = session.Player.Inventory;

            switch (mode)
            {
                case RequestItemInventoryMode.Move:
                    HandleMove(session, packet);
                    break;
                case RequestItemInventoryMode.Drop:
                    HandleDrop(session, packet);
                    break;
                case RequestItemInventoryMode.DropBound:
                    HandleDropBound(session, packet);
                    break;
                case RequestItemInventoryMode.Sort:
                    HandleSort(session, packet, inventory);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleMove(GameSession session, PacketReader packet)
        {
            long uid = packet.ReadLong(); // Grabs incoming item packet uid
            short dstSlot = packet.ReadShort(); // Grabs incoming item packet slot
            InventoryController.MoveItem(session, uid, dstSlot);
        }

        private static void HandleDrop(GameSession session, PacketReader packet)
        {
            // TODO: Make sure items are tradable?
            long uid = packet.ReadLong();
            int amount = packet.ReadInt(); // Grabs incoming item packet amount
            InventoryController.DropItem(session, uid, amount, false);
        }

        private static void HandleDropBound(GameSession session, PacketReader packet)
        {
            long uid = packet.ReadLong();
            InventoryController.DropItem(session, uid, 0, true);
        }

        private static void HandleSort(GameSession session, PacketReader packet, Inventory inventory)
        {
            InventoryTab tab = (InventoryTab) packet.ReadShort();
            InventoryController.SortInventory(session, inventory, tab);
        }
    }
}
