﻿using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
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

            switch (item.Function.Name)
            {
                case "CallAirTaxi":
                    HandleCallAirTaxi(session, packet, item);
                    break;
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
                case "ItemRemakeScroll":
                    HandleItemRemakeScroll(session, itemUid);
                    break;
                case "OpenGachaBox": // Gacha capsules
                    HandleOpenGachaBox(session, packet, item);
                    break;
                case "OpenCoupleEffectBox": // Buddy badges
                    HandleOpenCoupleEffectBox(session, packet, item);
                    break;
                case "PetExtraction": // Pet skin scroll
                    HandlePetExtraction(session, packet, item);
                    break;
                case "InstallBillBoard": // ad balloons
                    HandleInstallBillBoard(session, packet, item);
                    break;
                default:
                    Console.WriteLine("Unhandled item function: " + item.Function.Name);
                    break;
            }
        }

        private static void HandleItemRemakeScroll(GameSession session, long itemUid)
        {
            session.Send(ChangeAttributesScrollPacket.Open(itemUid));
        }

        private static void HandleChatEmoticonAdd(GameSession session, Item item)
        {
            long expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + item.Function.Duration + Environment.TickCount;

            if (item.Function.Duration == 0) // if no duration was set, set it to not expire
            {
                expiration = 9223372036854775807;
            }

            if (session.Player.ChatSticker.Any(p => p.GroupId == item.Function.Id))
            {
                // TODO: Find reject packet
                return;
            }

            session.Send(ChatStickerPacket.AddSticker(item.Id, item.Function.Id, expiration));
            session.Player.ChatSticker.Add(new((byte) item.Function.Id, expiration));
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
            int duration = item.Function.Duration + Environment.TickCount;
            CoordF portalCoord = session.Player.Coord;
            CoordF portalRotation = session.Player.Rotation;

            session.FieldManager.BroadcastPacket(PlayerHostPacket.StartMinigame(session.Player, item.Function.FieldId));
            //  session.FieldManager.BroadcastPacket(FieldPacket.AddPortal()
            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleLevelPotion(GameSession session, Item item)
        {
            if (session.Player.Levels.Level >= item.Function.TargetLevel)
            {
                return;
            }

            session.Player.Levels.SetLevel(item.Function.TargetLevel);

            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleTitleScroll(GameSession session, Item item)
        {
            if (session.Player.Titles.Contains(item.Function.Id))
            {
                return;
            }

            session.Player.Titles.Add(item.Function.Id);

            session.Send(UserEnvPacket.AddTitle(item.Function.Id));

            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleOpenInstrument(Item item)
        {
            if (!InstrumentCategoryInfoMetadataStorage.IsValid(item.Function.Id))
            {
                return;
            }
        }

        private static void HandleVIPCoupon(GameSession session, Item item)
        {
            long vipTime = item.Function.Duration * 3600;

            PremiumClubHandler.ActivatePremium(session, vipTime);
            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleStoryBook(GameSession session, Item item)
        {
            session.Send(StoryBookPacket.Open(item.Function.Id));
        }

        private static void HandleHongBao(GameSession session, Item item)
        {
            HongBao newHongBao = new(session.Player, item.Function.TotalUser, item.Id, item.Function.Id, item.Function.Count, item.Function.Duration);
            GameServer.HongBaoManager.AddHongBao(newHongBao);

            session.FieldManager.BroadcastPacket(PlayerHostPacket.OpenHongbao(session.Player, newHongBao));
            InventoryController.Consume(session, item.Uid, 1);
        }

        private static void HandleOpenGachaBox(GameSession session, PacketReader packet, Item capsule)
        {
            string amount = packet.ReadUnicodeString();
            int rollCount = 0;

            GachaMetadata gacha = GachaMetadataStorage.GetMetadata(capsule.Function.Id);

            Random random = new Random();
            List<Item> items = new List<Item>() { };

            if (amount == "single")
            {
                rollCount = 1;
            }
            else if (amount == "multi")
            {
                rollCount = 10;
            }

            for (int i = 0; i < rollCount; i++)
            {
                GachaContent contents = HandleSmartGender(gacha, session.Player.Gender);

                int itemAmount = random.Next(contents.MinAmount, contents.MaxAmount);

                Item gachaItem = new Item(contents.ItemId)
                {
                    Rarity = contents.Rarity,
                    Amount = itemAmount,
                    GachaDismantleId = gacha.GachaId
                };
                items.Add(gachaItem);
                InventoryController.Consume(session, capsule.Uid, 1);
            }

            session.Send(FireWorksPacket.Gacha(items));

            foreach (Item item in items)
            {
                InventoryController.Add(session, item, true);
            }
        }

        private static GachaContent HandleSmartGender(GachaMetadata gacha, byte playerGender)
        {
            Random random = new Random();
            int index = random.Next(gacha.Contents.Count);

            GachaContent contents = gacha.Contents[index];
            if (!contents.SmartGender)
            {
                return contents;
            }

            byte itemGender = ItemMetadataStorage.GetGender(contents.ItemId);
            if (playerGender != itemGender || itemGender != 2)  // if it's not the same gender or unisex, roll again
            {
                bool sameGender = false;
                do
                {
                    int indexReroll = random.Next(gacha.Contents.Count);

                    GachaContent rerollContents = gacha.Contents[indexReroll];
                    byte rerollContentsGender = ItemMetadataStorage.GetGender(rerollContents.ItemId);

                    if (rerollContentsGender == playerGender || rerollContentsGender == 2)
                    {
                        return rerollContents;
                    }
                } while (!sameGender);
            }
            return contents;
        }

        public static void HandleOpenCoupleEffectBox(GameSession session, PacketReader packet, Item item)
        {
            string targetUser = packet.ReadUnicodeString();

            Player otherPlayer = GameServer.Storage.GetPlayerByName(targetUser);
            if (otherPlayer == null)
            {
                session.Send(NoticePacket.Notice(SystemNotice.CharacterNotFound, NoticeType.Popup));
                return;
            }

            Item badge = new Item(item.Function.Id)
            {
                Rarity = item.Function.Rarity,
                PairedCharacterId = otherPlayer.CharacterId,
                PairedCharacterName = otherPlayer.Name
            };

            Item otherUserBadge = new Item(item.Function.Id)
            {
                Rarity = item.Function.Rarity,
                PairedCharacterId = session.Player.CharacterId,
                PairedCharacterName = session.Player.Name
            };

            //InventoryController.Consume(session, item.Uid, 1);
            InventoryController.Add(session, badge, true);
            //session.Send(NoticePacket.Notice(SystemNotice.BuddyBadgeMailedToUser, otherPlayer.Name, NoticeType.ChatAndFastText));

            //otherPlayer.Session.Send(MailPacket.Notify(otherPlayer.Session));
            // TODO: Mail the badge to the other user
        }

        public static void HandlePetExtraction(GameSession session, PacketReader packet, Item item)
        {
            long petUid = long.Parse(packet.ReadUnicodeString());
            if (!session.Player.Inventory.Items.ContainsKey(petUid))
            {
                return;
            }

            Item pet = session.Player.Inventory.Items[petUid];

            Item badge = new Item(70100000)
            {
                PetSkinBadgeId = pet.Id,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount
            };

            InventoryController.Consume(session, item.Uid, 1);
            InventoryController.Add(session, badge, true);
            session.Send(PetSkinPacket.Extract(petUid, badge));
        }

        public static void HandleCallAirTaxi(GameSession session, PacketReader packet, Item item)
        {
            int fieldID = int.Parse(packet.ReadUnicodeString());
            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(fieldID);
            if (spawn != null)
            {
                InventoryController.Consume(session, item.Uid, 1);
                session.Player.Warp(spawn, fieldID);
            }
        }

        public static void HandleInstallBillBoard(GameSession session, PacketReader packet, Item item)
        {
            string[] parameters = packet.ReadUnicodeString().Split("'");
            string title = parameters[0];
            string description = parameters[1];
            bool publicHouse = parameters[2].Equals("1");

            int balloonUid = GuidGenerator.Int();
            string uuid = "AdBalloon_" + balloonUid.ToString();
            InteractObject balloon = new InteractObject(uuid, uuid, Maple2Storage.Enums.InteractObjectType.AdBalloon);
            balloon.Balloon = new AdBalloon(session.Player, item, title, description, publicHouse);
            IFieldObject<InteractObject> fieldBalloon = session.FieldManager.RequestFieldObject(balloon);
            fieldBalloon.Coord = session.FieldPlayer.Coord;
            fieldBalloon.Rotation = session.FieldPlayer.Rotation;
            session.FieldManager.AddBalloon(fieldBalloon);

            session.Send(PlayerHostPacket.AdBalloonPlace());
            InventoryController.Consume(session, item.Uid, 1);
        }
    }
}
