using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class JobHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.JOB;

        public JobHandler(ILogger<JobHandler> logger) : base(logger) { }

        private enum JobMode : byte
        {
            Close = 0x08,
            Save = 0x09,
            Reset = 0x0A,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            JobMode mode = (JobMode) packet.ReadByte();
            switch (mode)
            {
                case JobMode.Close: // Close Skill Tree
                    HandleCloseSkillTree(session);
                    break;
                case JobMode.Save: // Save Skill Tree
                    HandleSaveSkillTree(session, packet);
                    break;
                case JobMode.Reset:
                    HandleResetSkillTree(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleCloseSkillTree(GameSession session)
        {
            session.Send(JobPacket.Close());
        }

        private static void HandleSaveSkillTree(GameSession session, PacketReader packet)
        {
            // Get skill tab to update
            SkillTab skillTab = session.Player.SkillTabs[0]; // Get first skill tab only for now, uncertain of how to have multiple skill tabs

            // Read skills
            int count = packet.ReadInt(); // Number of skills
            for (int i = 0; i < count; i++)
            {
                // Read skill info
                int id = packet.ReadInt(); // Skill id
                short level = packet.ReadShort(); // Skill level
                byte learned = packet.ReadByte(); // 00 if unlearned 01 if learned

                // Update current character skill tree data with new skill
                skillTab.AddOrUpdate(id, level, learned > 0);
            }

            // Send JOB packet that contains all skills then send KEY_TABLE packet to update hotbars
            session.Send(JobPacket.Save(session.Player, session.FieldPlayer.ObjectId));
            session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
        }

        private static void HandleResetSkillTree(GameSession session, PacketReader packet)
        {
            int unknown = packet.ReadInt();
            session.Player.SkillTabs[0] = new SkillTab(session.Player.Job);
            session.Send(JobPacket.Save(session.Player, session.FieldPlayer.ObjectId));
        }
    }
}
