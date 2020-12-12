using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class RequestItemPickupHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_PICKUP;

        public RequestItemPickupHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            int objectId = packet.ReadInt();
            packet.ReadByte();

            // TODO: This will be bugged when you have a full inventory, check inventory before looting
            // Remove objectId from Field, make sure item still exists (multiple looters)
            if (!session.FieldManager.RemoveItem(objectId, out Item item)) {
                return;
            }

            session.Inventory.Add(item);
            session.Send(ItemInventoryPacket.Add(item));
            session.Send(ItemInventoryPacket.MarkItemNew(item));
        }
    }
}