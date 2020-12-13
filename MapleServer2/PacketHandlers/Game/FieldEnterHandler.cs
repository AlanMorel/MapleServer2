using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class FieldEnterHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.RESPONSE_FIELD_ENTER;

        public FieldEnterHandler(ILogger<FieldEnterHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
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

            if (session.Player.GameOptions.TryGetHotbar(0, out Hotbar mainHotbar)) {
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

            //Adds first set of item test case
            foreach(int catalyst in catalysts)
            {
                var item = new Item(catalyst) { Amount = 4, Uid = catalyst };

                session.Inventory.Add(item);
                session.Send(ItemInventoryPacket.Add(item));
            }
            //session.Inventory.print();

            //Adds 2nd set of same items to test stacking
            foreach (int catalyst in catalysts) {
                var item = new Item(catalyst) { Amount = 4, Uid = catalyst };

                if (!session.Inventory.Add(item))
                {
                    session.Inventory.Update(item, (item.Amount + item.Amount));
                    session.Send(ItemInventoryPacket.Update(item.Uid, (item.Amount + item.Amount)));
                }
                else
                {
                    session.Send(ItemInventoryPacket.Add(item));
                }
            //session.Inventory.print();
            }
        }
    }
}