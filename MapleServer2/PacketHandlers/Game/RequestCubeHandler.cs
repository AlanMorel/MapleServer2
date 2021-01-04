using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestCubeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_CUBE;

        public RequestCubeHandler(ILogger<RequestCubeHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();

            switch (mode)
            {
                case 0x11: // Pickup item
                    HandlePickup(session, packet);
                    break;
                case 0x12: // Drop item
                    HandleDrop(session);
                    break;
            }
        }

        private void HandlePickup(GameSession session, PacketReader packet)
        {
            byte[] coords = packet.Read(3);

            // Pickup item then set battle state to true
            session.Send(ResponseCubePacket.Pickup(session, coords));
            session.Send(UserBattlePacket.UserBattle(session.FieldPlayer, true));
        }

        private void HandleDrop(GameSession session)
        {
            // Drop item then set battle state to false
            session.Send(ResponseCubePacket.Drop(session.FieldPlayer));
            session.Send(UserBattlePacket.UserBattle(session.FieldPlayer, false));
        }
    }
}
