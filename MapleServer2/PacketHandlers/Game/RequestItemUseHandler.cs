using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE;

        public RequestItemUseHandler(ILogger<RequestItemUseHandler> logger) : base(logger) { }

        private enum BoxType : byte
        {
            OPEN = 0x00,
            SELECT = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long boxUid = packet.ReadLong();
            BoxType boxType = (BoxType) packet.ReadShort();

            int index = 0;
            if (boxType == BoxType.SELECT)
            {
                index = packet.ReadShort() - 0x30; // Starts at 0x30 for some reason
                if (index < 0)
                {
                    return;
                }
            }

            if (!session.Player.Inventory.Items.ContainsKey(boxUid))
            {
                return;
            }

            // Get the box item
            Item box = session.Player.Inventory.Items[boxUid];

            // Do nothing if box has no data stored
            if (box.Content.Count <= 0)
            {
                return;
            }

            // Remove box if amount is 1 or less
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

            // Handle selection box
            if (boxType == BoxType.SELECT)
            {
                if (index < box.Content.Count)
                {
                    ItemUseHelper.GiveItem(session, box.Content[index]);
                }
                return;
            }

            ItemUseHelper.OpenBox(session, box.Content);
        }
    }
}
