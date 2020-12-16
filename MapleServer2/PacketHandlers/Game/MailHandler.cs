using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;

namespace MapleServer2.PacketHandlers.Game
{
    public class MailHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.MAIL;

        public MailHandler(ILogger<SkillBookTreeHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();

            switch (mode)
            {
                case 0: // Open
                    HandleOpen(session, packet);
                    break;
                case 1: // Send
                    HandleSend(session, packet);
                    break;
                case 2: // Read
                    HandleRead(session, packet);
                    break;
                case 11: // Collect
                    HandleCollect(session, packet);
                    break;
                case 18: // Read batch
                    HandleReadBatch(session, packet);
                    break;
                case 19: // Collect batch
                    HandleCollectBatch(session, packet);
                    break;
            }
        }

        private void HandleOpen(GameSession session, PacketReader packet)
        {
            MailPacket.Open(session);
        }

        private void HandleSend(GameSession session, PacketReader packet)
        {
            string recipient = packet.ReadUnicodeString();
            string title = packet.ReadUnicodeString();
            string body = packet.ReadUnicodeString();

            session.Send(MailPacket.Send(session, recipient, title, body));
        }

        private void HandleRead(GameSession session, PacketReader packet)
        {
            int id = packet.ReadInt();

            session.Send(MailPacket.Read(session, id));
        }

        private void HandleCollect(GameSession session, PacketReader packet)
        {
            int id = packet.ReadInt();

            MailPacket.Collect(session, id);
        }

        private void HandleReadBatch(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();

            int id = 0;
            for (int i = 0; i < count; i++)
            {
                id = packet.ReadInt();
                packet.ReadInt();

                session.Send(MailPacket.Read(session, id));
            }
        }

        private void HandleCollectBatch(GameSession session, PacketReader packet)
        {
            int count = packet.ReadInt();

            int id = 0;
            for (int i = 0; i < count; i++)
            {
                id = packet.ReadInt();
                packet.ReadInt();

                session.Send(MailPacket.Read(session, id));
                MailPacket.Collect(session, id);
            }
        }
    }
}