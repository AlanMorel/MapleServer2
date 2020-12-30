using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using System;

namespace MapleServer2.PacketHandlers.Game
{
    public class StatPointHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.STAT_POINT;

        public StatPointHandler(ILogger<StatPointHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();

            switch (mode)
            {
                case 2: 
                    // Increment attribute point
                    HandleStatIncrement(session, packet);
                    break;
                case 3:
                    // Reset attribute distribution
                    HandleResetStatDistribtuion(session, packet);
                    break;
            }
        }

        private void HandleStatIncrement(GameSession session, PacketReader packet)
        {
            byte statTypeIndex = packet.ReadByte();
            StatPointPacket.AddStatPoint(session.Player, statTypeIndex);
            session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
            session.Send(SendStatPacket.WriteCharacterStats(session.Player));
        }

        private void HandleResetStatDistribtuion(GameSession session, PacketReader packet)
        {
            StatPointPacket.ResetStatPoints(session.Player);
            session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
            session.Send(SendStatPacket.WriteCharacterStats(session.Player));
        }
    }
}