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

            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleSelectItemBox(GameSession session, PacketReader packet, Item item)
        {
            short boxType = packet.ReadShort();
            int index = packet.ReadShort() - 0x30;

            if (item.Content.Count <= 0)
            {
                return;
            }

            InventoryController.Consume(session, item.Uid, 1);

            if (index < item.Content.Count)
            {
                ItemUseHelper.GiveItem(session, item.Content[index]);
            }
        }

        private static void HandleOpenItemBox(GameSession session, PacketReader packet, Item item)
        {
            short boxType = packet.ReadShort();

            InventoryController.Consume(session, item.Uid, 1);
            ItemUseHelper.OpenBox(session, item.Content);
        }
    }
}
