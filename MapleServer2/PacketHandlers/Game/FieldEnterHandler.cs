using System;
using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class FieldEnterHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_FIELD_ENTER;

        public FieldEnterHandler(ILogger<FieldEnterHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            packet.ReadInt(); // ?

            // Liftable: 00 00 00 00 00
            // SendBreakable
            // Self
            session.EnterField(session.Player.MapId);
            session.Send(FieldObjectPacket.SetStats(session.FieldPlayer));
            session.Send(EmotePacket.LoadEmotes());

            // Normally skill layout would be loaded from a database
            QuickSlot arrowStream = QuickSlot.From(10500001);
            QuickSlot arrowBarrage = QuickSlot.From(10500011);
            QuickSlot eagleGlide = QuickSlot.From(10500151);
            QuickSlot testSkill = QuickSlot.From(10500153);

            if (session.Player.GameOptions.TryGetHotbar(0, out Hotbar mainHotbar))
            {
                /*
                mainHotbar.MoveQuickSlot(4, arrowStream);
                mainHotbar.MoveQuickSlot(5, arrowBarrage);
                mainHotbar.MoveQuickSlot(6, eagleGlide);
                mainHotbar.MoveQuickSlot(7, testSkill);
                */
                session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
            }

            // Add catalysts for testing

            int[] catalysts = { 40100001, 40100002, 40100003, 40100021, 40100023, 40100024, 40100026 };

            var item = new Item(40100001)
            {
                Amount = 99999,
                Uid = GuidGenerator.Long()
            };
            var item2 = new Item(40100001)
            {
                Amount = 90000,
                Uid = GuidGenerator.Long()
            };
            var item3 = new Item(40100001)
            {
                Amount = 10000,
                Uid = GuidGenerator.Long()
            };
            InventoryController.Add(session, item);
            InventoryController.Add(session, item2);
            InventoryController.Add(session, item3);

            //Add mail for testing
            //System mail without any item
           Mail sysMail = new Mail
           (
               101,
               GuidGenerator.Int(),
               session.Player.CharacterId,
               "50000002",
               "",
               "",
               0,
               DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
               null
           );

            // System mail with an item
            List<Item> items = new List<Item>
            {
                new Item(20302228)
                {
                    Uid = GuidGenerator.Long(),
                    CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                    Owner = session.Player,
                    Amount = 10
                }
            };
            Mail sysItemMail = new Mail
            (
                101,
                GuidGenerator.Int(),
                session.Player.CharacterId,
                "53000042",
                "",
                "",
                0,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                items
            );

            // Regular mail
            Mail regMail = new Mail
            (
                1,
                GuidGenerator.Int(),
                session.Player.CharacterId,
                session.Player.Name,
                "Test Title",
                "Test Body",
                0,
                DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                null
            );

            session.Player.Mailbox.AddOrUpdate(sysItemMail);
            session.Player.Mailbox.AddOrUpdate(sysMail);
            session.Player.Mailbox.AddOrUpdate(regMail);
        }
    }
}