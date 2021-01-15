using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Maple2Storage.Types.Metadata;
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
            }

            if (!session.Player.Inventory.Items.ContainsKey(boxUid))
            {
                return;
            }

            // Get the box item
            Item box = session.Player.Inventory.Items[boxUid];

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
                if (index >= box.Content.Count)
                {
                    return;
                }

                OpenBox(session, box.Content[index]);
                return;
            }

            // Handle open box
            foreach (ItemContent content in box.Content)
            {
                OpenBox(session, content);
            }
        }

        private void OpenBox(GameSession session, ItemContent content)
        {
            // Currency
            if (content.Id.ToString().StartsWith("9"))
            {
                switch (content.Id)
                {
                    case 90000001: // Meso
                        session.Player.Wallet.Meso.Modify(content.Amount);
                        break;
                    case 90000004: // Meret
                    case 90000011: // Meret
                    case 90000015: // Meret
                    case 90000016: // Meret
                        session.Player.Wallet.Meret.Modify(content.Amount);
                        break;
                }
            }
            // Items
            else
            {
                Item item = new Item(content.Id)
                {
                    Amount = content.Amount
                };
                InventoryController.Add(session, item, true);
            }
        }
    }
}
