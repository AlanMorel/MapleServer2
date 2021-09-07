using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;

namespace MapleServer2.PacketHandlers.Common
{
    public class ResponseVersionHandler : CommonPacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_VERSION;

        public ResponseVersionHandler() : base() { }

        public override void Handle(LoginSession session, PacketReader packet)
        {
            HandleCommon(session, packet);

            session.Send(RequestPacket.Login());
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            HandleCommon(session, packet);

            // No idea what this is, but server sends it when logging into game server
            PacketWriter pWriter = PacketWriter.Of(SendOp.UNKNOWN_SYNC);
            pWriter.WriteByte();
            pWriter.WriteInt(session.ClientTick);

            session.Send(pWriter);
            session.Send(RequestPacket.Key());
        }

        protected override void HandleCommon(Session session, PacketReader packet)
        {
            uint version = packet.ReadUInt();
            // +4 Bytes CONST(2F 00 02 00)

            if (version != Session.VERSION)
            {
                session.Disconnect();
            }
        }
    }
}
