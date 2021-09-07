using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class StateSkillHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STATE_SKILL;

        public StateSkillHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            if (function == 0)
            {
                // This count seems to increase for each skill used
                int counter = packet.ReadInt();
                // objectId for climb, 13641 (0x3549 for swim dash)
                int objectId = packet.ReadInt();
                int clientTime = packet.ReadInt();
                int skillId = packet.ReadInt();
                packet.ReadShort(); // 1
                session.Player.Animation = (byte) packet.ReadInt(); // Animation
                int clientTick = packet.ReadInt();
                packet.ReadLong(); // 0

                if (SkillMetadataStorage.GetSkill(skillId).State == "gosGlide")
                {
                    session.Player.OnAirMount = true;
                }

                // TODO: Broadcast this to all field players
            }
        }
    }
}
