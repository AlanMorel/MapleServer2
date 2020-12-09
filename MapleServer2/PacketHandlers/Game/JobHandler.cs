using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class JobHandler : GamePacketHandler
    {
        public override ushort OpCode => RecvOp.JOB;

        public JobHandler(ILogger<JobHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 8: // Close Skill Tree
                    HandleCloseSkillTree(session, packet);
                    break;
                case 9: // Save Skill Tree
                    HandleSaveSkillTree(session, packet);
                    break;
            }
        }

        public void HandleCloseSkillTree(GameSession session, PacketReader packet)
        {
            session.Send(JobPacket.Close());
        }

        public void HandleSaveSkillTree(GameSession session, PacketReader packet)
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
                skillTab.AddOrUpdate(Skill.skill(id, level, learned));
            }

            // Send JOB packet that contains all skills then send KEY_TABLE packet to update hotbars
            session.Send(JobPacket.Save(session.Player, session.FieldPlayer.ObjectId));
            session.Send(KeyTablePacket.SendHotbars(session.Player.GameOptions));
        }
    }
}