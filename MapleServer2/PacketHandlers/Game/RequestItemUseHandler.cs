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
            long boxUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(boxUid))
            {
                return;
            }

            // Get the box item
            Item box = session.Player.Inventory.Items[boxUid];

            // Normally would look up which item to create, instead always create poisonous mushroom
            Item item = new Item(30001001)
            {
                Amount = 1,
                Uid = GuidGenerator.Long()
            };

            // Remove box if amount is 1 or less
            if (box.Amount <= 1)
            {
                session.Player.Inventory.Remove(boxUid, out Item removed);
                session.Send(ItemInventoryPacket.Remove(boxUid));
            }
            // Update box amount to be -1 otherwise
            else
            {
                box.Amount -= 1;
                session.Send(ItemInventoryPacket.Update(boxUid, box.Amount));
            }

            // Add the opened item
            InventoryController.Add(session, item);
        }
    }
}