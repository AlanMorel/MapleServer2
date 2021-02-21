using System;
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

            Item item = session.Player.Inventory.Items[itemUid];

            switch (item.FunctionName)
            {
                case "ChatEmoticonAdd":
                    HandleChatEmoticonAdd(session, packet, item);
                    break;
                case "SelectItemBox":
                    HandleSelectItemBox(session, packet, item);
                    break;
                case "OpenItemBox":
                    HandleOpenItemBox(session, packet, item);
                    break;
                default:
                    return;
            }
        }

        private static void HandleChatEmoticonAdd(GameSession session, PacketReader packet, Item item)
        {

            session.Send(ChatStickerPacket.AddSticker(item.Id, item.FunctionParameter));
            session.Player.Stickers.Add((short) item.FunctionParameter);

            if (item.Amount <= 1)
            {
                InventoryController.Remove(session, item.Uid, out Item removed);
            }
            else
            {
                item.Amount -= 1;
                InventoryController.Update(session, item.Uid, item.Amount);
            }
        }

        private static void HandleSelectItemBox(GameSession session, PacketReader packet, Item item)
        {
            short boxType = packet.ReadShort();
            int index = packet.ReadShort() - 0x30;

            Console.WriteLine(index);

            if (item.Content.Count <= 0)
            {
                return;
            }

            // Remove box if amount is 1 or less
            if (item.Amount <= 1)
            {
                InventoryController.Remove(session, item.Uid, out Item removed);
            }
            // Decrement box amount to otherwise
            else
            {
                item.Amount -= 1;
                InventoryController.Update(session, item.Uid, item.Amount);
            }

            if (index < item.Content.Count)
            {
                ItemUseHelper.GiveItem(session, item.Content[index]);
            }
        }

        private static void HandleOpenItemBox(GameSession session, PacketReader packet, Item item)
        {
            short boxType = packet.ReadShort();

            if (item.Amount <= 1)
            {
                InventoryController.Remove(session, item.Uid, out Item removed);
            }
            else
            {
                item.Amount -= 1;
                InventoryController.Update(session, item.Uid, item.Amount);
            }

            ItemUseHelper.OpenBox(session, item.Content);
        }

    }
}
