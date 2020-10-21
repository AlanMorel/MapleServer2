using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game {
    // This is not working yet. for some reason it does not give the "start" prompt after casting line.
    public class FishingHandler : GamePacketHandler {
        public override ushort OpCode => RecvOp.FISHING;

        public FishingHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            byte function = packet.ReadByte();
            switch (function) {
                case 0:
                    long rodItemUid = packet.ReadLong(); // Testing RodItemId: 32000055
                    session.Send(PacketWriter.Of(SendOp.FISHING).WriteHexString("04 00 08 00 00 00 0C F7 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0B F8 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0E F6 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0D F7 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0C F8 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0F F6 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0E F7 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00 0D F8 10 00 81 96 98 00 19 00 00 00 98 3A 00 00 01 00"));
                    //session.Send(GuideObjectPacket.Bracket(session.FieldPlayer));
                    session.Send(FishingPacket.Start(rodItemUid));
                    break;
                case 1:
                    session.Send(FishingPacket.Stop());
                    //session.Send(GuideObjectPacket.Remove(session.FieldPlayer));
                    break;
                case 8: // Complete Fishing
                    // When fishing manually, 0 = success minigame, 1 = no minigame
                    // When auto-fishing, it seems to send 0, gets back failed fishing response
                    // Then it sends 1 and gets back 0x04 response before restarting fishing again
                    bool completed = packet.ReadBool(); // Completed without minigame
                    // Give fish!
                    session.Send(FishingPacket.CatchFish(10000001, 100, true));
                    break;
                case 10: // Failed minigame
                    break;
            }
        }
    }
}