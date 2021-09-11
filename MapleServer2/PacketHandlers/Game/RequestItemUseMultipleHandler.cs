using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseMultipleHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE_MULTIPLE;

        public RequestItemUseMultipleHandler() : base() { }

        private enum BoxType : byte
        {
            OPEN = 0x00,
            SELECT = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            //int boxId = packet.ReadInt();
            //packet.ReadShort(); // Unknown
            //int amount = packet.ReadInt();
            //BoxType boxType = (BoxType) packet.ReadShort();

            //int index = 0;
            //if (boxType == BoxType.SELECT)
            //{
            //    index = packet.ReadShort() - 0x30; // Starts at 0x30 for some reason
            //    if (index < 0)
            //    {
            //        return;
            //    }
            //}

            //int opened = 0;
            //Dictionary<long, Item> items = new Dictionary<long, Item>(session.Player.Inventory.Items.Where(x => x.Value.Id == boxId)); // Make copy of items in-case new item is added

            //foreach (KeyValuePair<long, Item> kvp in items)
            //{
            //    Item item = kvp.Value;
            //    // Do nothing if box has no data stored
            //    if (item.Content.Count <= 0)
            //    {
            //        break;
            //    }

            //    for (int i = opened; i < amount; i++)
            //    {
            //        bool breakOut = false; // Needed to remove box before adding item to prevent item duping

            //        if (item.Amount <= 1)
            //        {
            //            breakOut = true; // Break out of the amount loop because this stack of boxes is empty, look for next stack
            //        }

            //        opened++;
            //        InventoryController.Consume(session, item.Uid, 1);

            //        // Handle selection box
            //        if (boxType == BoxType.SELECT)
            //        {
            //            if (index < item.Content.Count)
            //            {
            //                ItemUseHelper.GiveItem(session, item.Content[index]);
            //            }
            //        }

            //        // Handle open box
            //        else if (boxType == BoxType.OPEN)
            //        {
            //            ItemUseHelper.OpenBox(session, item.Content);
            //        }

            //        if (breakOut)
            //        {
            //            break;
            //        }
            //    }
            //}

            //session.Send(ItemUsePacket.Use(boxId, amount));
        }
    }
}
