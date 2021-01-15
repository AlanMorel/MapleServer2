using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

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

            // Remove box if amount is 1 or less
            // TODO remove these inventory packets
            if (box.Amount <= 1)
            {
                InventoryController.Remove(session, boxUid, out Item removed);
            }
            // Decrement box amount to otherwise
            else
            {
                box.Amount -= 1;
                InventoryController.Update(session, boxUid, box.Amount);
            }

            // Normally would look up which item to create, instead always add poisonous mushroom
            Item item = new Item(30001001);
            InventoryController.Add(session, item, true);
        }
    }
}
