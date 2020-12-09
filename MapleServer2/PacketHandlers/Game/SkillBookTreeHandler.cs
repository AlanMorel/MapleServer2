using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;

namespace MapleServer2.PacketHandlers.Game
{
    public class SkillBookTreeHandler : GamePacketHandler
    {
        public override ushort OpCode => RecvOp.REQUEST_SKILL_BOOK_TREE;

        public SkillBookTreeHandler(ILogger<SkillBookTreeHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 0: // Open skill tree
                    HandleOpen(session, packet);
                    break;
                case 1: // Save skill tree
                    HandleSave(session, packet);
                    break;
            }
        }

        public void HandleOpen(GameSession session, PacketReader packet)
        {
            session.Send(SkillBookTreePacket.Open(session.Player));
        }

        public void HandleSave(GameSession session, PacketReader packet)
        {
            // Reads first 20 bytes, some kind of identifier info, then sends the same 20 bytes back
            byte[] header = packet.Read(20);
            session.Send(SkillBookTreePacket.Save(header));
        }
    }
}