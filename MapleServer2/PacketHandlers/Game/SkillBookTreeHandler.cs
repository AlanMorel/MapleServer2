using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class SkillBookTreeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_SKILL_BOOK_TREE;

        public SkillBookTreeHandler(ILogger<SkillBookTreeHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 0: // Open skill tree
                    HandleOpen(session);
                    break;
                case 1: // Save skill tree
                    HandleSave(session);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            session.Send(SkillBookTreePacket.Open(session.Player));
        }

        private static void HandleSave(GameSession session)
        {
            session.Send(SkillBookTreePacket.Save(session.Player));
        }
    }
}
