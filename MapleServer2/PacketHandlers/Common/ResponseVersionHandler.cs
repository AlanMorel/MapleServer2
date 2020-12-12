using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Common {
    public class ResponseVersionHandler : CommonPacketHandler {
        public override RecvOp OpCode => RecvOp.RESPONSE_VERSION;

        public ResponseVersionHandler(ILogger<ResponseVersionHandler> logger) : base(logger) { }

        public override void Handle(LoginSession session, PacketReader packet) {
            HandleCommon(session, packet);

            session.Send(RequestPacket.Login());
        }

        public override void Handle(GameSession session, PacketReader packet) {
            HandleCommon(session, packet);

            // No idea what this is, but server sends it when logging into game server
            session.Send(PacketWriter.Of(0x132)
                .WriteByte()
                .WriteInt(Environment.TickCount));
            session.Send(RequestPacket.Key());
        }

        protected override void HandleCommon(Session session, PacketReader packet) {
            uint version = packet.ReadUInt();
            // +4 Bytes CONST(2F 00 02 00)

            if (version != Session.VERSION) {
                session.Disconnect();
            }
        }
    }
}