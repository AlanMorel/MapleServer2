using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    public class RequestTutorialItemHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.REQUEST_TUTORIAL_ITEM;

        public RequestTutorialItemHandler(ILogger<RequestTutorialItemHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            // No data passed in

            // TODO - Determine item from player's job
            Item tutorialBow = Item.TutorialBow(session.Player);

            // Add the item to the inventory
            session.Player.Inventory.Add(tutorialBow);

            // Send to client
            session.Send(ItemInventoryPacket.Add(tutorialBow));
            session.Send(ItemInventoryPacket.MarkItemNew(tutorialBow));

            // The below packet is sent with the inventory packets, but doesn't seem to be needed
            //session.Send(PacketWriter.Of(0x0105).WriteHexString("3F A6 36 E2 94 98 9B 2F 01 00 00 00 00 00 00 00 FF FF FF FF 89 18 84 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B3 BC BC FF 3D DA C3 FF BA B4 B0 FF 13 00 00 00 05 00 00 00 00 03 00 11 00 0C 00 00 00 00 00 00 00 1B 00 0F 00 00 00 00 00 00 00 1C 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 00 00 00 00 00 00 00 00 00 01 01 66 66 66 66 66 66 66 66 08 00 61 00 73 00 61 00 73 00 61 00 73 00 31 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            // The below two are kept for future reference in regards to the above
            //session.Send(PacketWriter.Of(0x16).WriteHexString("00 38 69 E6 00 3F A6 36 E2 94 98 9B 2F 00 00 01 00 00 00 00 00 01 00 00 00 00 00 00 00 FF FF FF FF 89 18 84 5E 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 B3 BC BC FF 3D DA C3 FF BA B4 B0 FF 13 00 00 00 05 00 00 00 00 03 00 11 00 0C 00 00 00 00 00 00 00 1B 00 0F 00 00 00 00 00 00 00 1C 00 11 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 01 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 0E 00 00 00 01 00 00 00 00 00 00 00 00 00 01 01 66 66 66 66 66 66 66 66 08 00 61 00 73 00 61 00 73 00 61 00 73 00 31 00 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00"));
            //session.Send(PacketWriter.Of(0x16).WriteHexString("08 3F A6 36 E2 94 98 9B 2F 01 00 00 00 00 00"));

        }
    }
}