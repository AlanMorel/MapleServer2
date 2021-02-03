using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestUserEnvHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_USER_ENV;

        public RequestUserEnvHandler(ILogger<RequestUserEnvHandler> logger) : base(logger) { }

        private enum UserEnvMode : byte
        {
            Change = 0x1,
            Achieve = 0x3,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            UserEnvMode mode = (UserEnvMode) packet.ReadByte();

            switch (mode)
            {
                case UserEnvMode.Change:
                    HandleTitleChange(session, packet);
                    break;
                case UserEnvMode.Achieve:
                    HandleAchieve(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleTitleChange(GameSession session, PacketReader packet)
        {
            int titleID = packet.ReadInt();

            if (titleID < 0)
            {
                return;
            }

            session.Player.TitleId = titleID;
            session.FieldManager.BroadcastPacket(UserEnvPacket.UpdateTitle(session, titleID));
        }

        private static void HandleAchieve(GameSession session)
        {
            // seems unchanged before and after gaining trophy
            // TODO: figure out what the bytes mean
            byte[] toSend = { 0x03, 0x03, 0x00, 0x00, 0x00, 0x17, 0xC9, 0xC9, 0x01, 0x01, 0x19, 0xC9, 0xC9, 0x01, 0x01, 0xDA, 0xC9, 0xA2, 0x03, 0x01 };
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_ENV);
            pWriter.Write(toSend);
            session.Send(pWriter);
        }
    }
}
