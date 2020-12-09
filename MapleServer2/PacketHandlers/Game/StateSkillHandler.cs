using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class StateSkillHandler : GamePacketHandler
    {
        public override ushort OpCode => RecvOp.STATE_SKILL;

        public StateSkillHandler(ILogger<StateSkillHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            if (function == 0)
            {
                // This count seems to increase for each skill used
                packet.ReadInt();   // Counter
                // objectId for climb, 13641 (0x3549 for swim dash)
                packet.ReadInt();   // Object Id
                packet.ReadInt();   // Client Time
                packet.ReadInt();   // Skill Id
                packet.ReadShort(); // 1
                session.Player.Animation = (byte)packet.ReadInt(); // Animation
                packet.ReadInt();   // Client Tick
                packet.ReadLong();  // 0

                // TODO: Broadcast this to all field players
            }
        }
    }
}