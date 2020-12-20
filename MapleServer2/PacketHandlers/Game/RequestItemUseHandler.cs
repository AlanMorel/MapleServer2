using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using MapleServer2.Types;
using MapleServer2.Tools;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE;

        public RequestItemUseHandler(ILogger<RequestItemUseHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }
            Item box = session.Player.Inventory.Items[itemUid];
            Item item = new Item(30001001)
            {
                Amount = 1,
                Uid = GuidGenerator.Long()
            };
            if (box.Amount <= 1)
            {
                session.Player.Inventory.Remove(itemUid, out Item removed);
                session.Send(ItemInventoryPacket.Remove(itemUid));
                InventoryController.Add(session, item);
            }
            else
            {
                box.Amount -= 1;
                session.Send(ItemInventoryPacket.Update(itemUid, box.Amount));
                InventoryController.Add(session, item);
            }
        }
    }
}