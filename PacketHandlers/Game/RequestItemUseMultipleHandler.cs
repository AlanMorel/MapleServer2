using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Maple2Storage.Types.Metadata;
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
                            OpenBox(session, item.Content[index]);
                        }
                    }

                    // Handle open box
                    else if (boxType == BoxType.OPEN)
                    {
                        foreach (ItemContent content in item.Content)
                        {
                            OpenBox(session, content);
                        }
                    }

                    if (breakOut)
                    {
                        break;
                    }
                }
            }

            session.Send(ItemUsePacket.Use(boxId, amount));
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
