using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseMultipleHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE_MULTIPLE;

        public RequestItemUseMultipleHandler(ILogger<RequestItemUseMultipleHandler> logger) : base(logger) { }

        private enum BoxType : byte
        {
            OPEN = 0x00,
            SELECT = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            int boxId = packet.ReadInt();
            packet.ReadShort(); // Unknown
            int amount = packet.ReadInt();
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

            short opened = 0; // Amount of opened boxes
            List<Item> items = new List<Item>(session.Player.Inventory.Items.Values); // Make copy of items in-case new item is added

            foreach (Item item in items)
            {
                // Continue over non-matching item ids
                if (item.Id != boxId)
                {
                    continue;
                }

                // Do nothing if box has no data stored
                if (item.Content.Count <= 0)
                {
                    break;
                }

                for (int i = opened; i < amount; i++)
                {
                    bool breakOut = false; // Needed to remove box before adding item to prevent item duping

                    // Remove box if there is only 1 left
                    if (item.Amount <= 1)
                    {
                        InventoryController.Remove(session, item.Uid, out Item removed);
                        opened++;

                        breakOut = true; // Break out of the amount loop because this stack of boxes is empty, look for next stack
                    }
                    else
                    {
                        // Update box amount if there is more than 1
                        item.Amount -= 1;
                        InventoryController.Update(session, item.Uid, item.Amount);

                        opened++;
                    }

                    // Handle selection box
                    if (boxType == BoxType.SELECT)
                    {
                        if (index < item.Content.Count)
                        {
                            ItemUseHelper.GiveItem(session, item.Content[index]);
                        }
                    }

                    // Handle open box
                    else if (boxType == BoxType.OPEN)
                    {
                        ItemUseHelper.OpenBox(session, item.Content);
                    }

                    if (breakOut)
                    {
                        break;
                    }
                }
            }

            session.Send(ItemUsePacket.Use(boxId, amount));
        }
    }
}
