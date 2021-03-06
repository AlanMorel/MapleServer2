using System;
using System.Linq;
using System.Threading.Tasks;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Data.Static;
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
                    HandleChatEmoticonAdd(session, item);
                    break;
                case "SelectItemBox": // Item box selection reward
                    HandleSelectItemBox(session, packet, item);
                    break;
                case "OpenItemBox": // Item box random/fixed reward
                    HandleOpenItemBox(session, packet, item);
                    break;
                case "OpenMassive": // Player hosted mini game
                    HandleOpenMassive(session, packet, item);
                    break;
                case "LevelPotion":
                    HandleLevelPotion(session, item);
                    break;
                case "TitleScroll":
                    HandleTitleScroll(session, item);
                    break;
                case "OpenInstrument":
                    HandleOpenInstrument(item);
                    break;
                case "VIPCoupon":
                    HandleVIPCoupon(session, item);
                    break;
                case "StoryBook":
                    HandleStoryBook(session, item);
                    break;
                case "HongBao":
                    HandleHongBao(session, item);
                    break;
                default:
                    return;
            }
        }

        private static void HandleChatEmoticonAdd(GameSession session, Item item)
        {
            long expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + item.FunctionDuration + AccountStorage.TickCount;

            if (item.FunctionDuration == 0) // if no duration was set, set it to not expire
            {
                expiration = 9223372036854775807;
            }

            if (session.Player.ChatSticker.Any(p => p.GroupId == item.FunctionId))
            {
                // TODO: Find reject packet
                return;
            }

            session.Send(ChatStickerPacket.AddSticker(item.Id, item.FunctionId, expiration));
            session.Player.ChatSticker.Add(new((byte) item.FunctionId, expiration));
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

        private static void HandleOpenMassive(GameSession session, PacketReader packet, Item item)
        {
            // Major WIP

            string password = packet.ReadUnicodeString();
            int duration = item.FunctionDuration + AccountStorage.TickCount;
            CoordF portalCoord = session.Player.Coord;
            CoordF portalRotation = session.Player.Rotation;

            session.FieldManager.BroadcastPacket(PlayerHostPacket.StartMinigame(session.Player, item.FunctionFieldId));
            //  session.FieldManager.BroadcastPacket(FieldPacket.AddPortal()
            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleLevelPotion(GameSession session, Item item)
        {
            if (session.Player.Levels.Level >= item.FunctionTargetLevel)
            {
                return;
            }

            session.Player.Levels.SetLevel(item.FunctionTargetLevel);

            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleTitleScroll(GameSession session, Item item)
        {
            if (session.Player.Titles.Contains(item.FunctionId))
            {
                return;
            }

            session.Player.Titles.Add(item.FunctionId);

            session.Send(UserEnvPacket.AddTitle(item.FunctionId));

            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleOpenInstrument(Item item)
        {
            if (!InstrumentCategoryInfoMetadataStorage.IsValid(item.FunctionId))
            {
                return;
            }
        }

        private static void HandleVIPCoupon(GameSession session, Item item)
        {
            long vipTime = item.FunctionDuration * 3600;

            PremiumClubHandler.ActivatePremium(session, vipTime);
            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleStoryBook(GameSession session, Item item)
        {
            session.Send(StoryBookPacket.Open(item.FunctionId));
        }

        private static void HandleHongBao(GameSession session, Item item)
        {
            HongBao newHongBao = new(session.Player, item.FunctionTotalUser, item.Id, item.FunctionId, item.FunctionCount, item.FunctionDuration);
            GameServer.HongBaoManager.AddHongBao(newHongBao);

            session.FieldManager.BroadcastPacket(PlayerHostPacket.OpenHongbao(session.Player, newHongBao));
            InventoryController.Consume(session, item.Uid, 1);
            StartHongBao(newHongBao);
        }

        private static async Task StartHongBao(HongBao hongBao)
        {
            await Task.Delay(hongBao.Duration * 1000);
            hongBao.DistributeReward();
        }
    }
}
