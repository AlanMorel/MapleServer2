using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class RequestItemInventoryHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_INVENTORY;

        public RequestItemInventoryHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();

            Inventory inventory = session.Player.Inventory;
            switch (function) {
                case 0: {
                    break;
                }
                case 3: { // Move
                    InventoryController.MoveItem(session, packet);
                    break;
                }
                case 4: { // Drop
                          // TODO: Make sure items are tradable?
                    InventoryController.DropItem(session, packet, false);
                    break;
                }
                case 5: { // Drop Bound
                    InventoryController.DropItem(session, packet, true);
                    break;
                }
                case 10: { // Sort
                    var tab = (InventoryType) packet.ReadShort();
                    InventoryController.SortInventory(session, inventory, tab);
                    break;
                }
            }
        }
    }
}