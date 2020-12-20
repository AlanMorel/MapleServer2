using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseMultipleHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE_MULTIPLE;

        public RequestItemUseMultipleHandler(ILogger<RequestItemUseMultipleHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            int boxId = packet.ReadInt();
            packet.ReadShort(); // Unknown
            int amount = packet.ReadInt();

            short opened = 0; // Amount of opened boxes
            List<Item> items = new List<Item> (session.Player.Inventory.Items.Values); // Make copy of items in-case new item is added

            foreach (Item item in items)
            {
                // Continue over non-matching item ids
                if (item.Id != boxId)
                {
                    continue;
                }

                for (int i = opened; i < amount; i++)
                {
                    // Create new item from opening box
                    Item newItem = new Item(30001001);

                    // Remove box if there is only 1 left
                    if (item.Amount <= 1)
                    {
                        session.Player.Inventory.Remove(item.Uid, out Item removed);
                        session.Send(ItemInventoryPacket.Remove(item.Uid));
                        InventoryController.Add(session, newItem);

                        opened++;

                        break; // Break out of the amount loop because this stack of boxes is empty, look for next stack
                    }

                    // Update box amount if there is more than 1
                    item.Amount -= 1;
                    session.Send(ItemInventoryPacket.Update(item.Uid, item.Amount));
                    InventoryController.Add(session, newItem);

                    opened++;
                }
            }

            session.Send(ItemUsePacket.Use(session, boxId, amount));

            // Need to handle opening boxes, probably serialize the item xml
        }
    }
}