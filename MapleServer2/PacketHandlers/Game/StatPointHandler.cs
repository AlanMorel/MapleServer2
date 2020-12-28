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
                    // follow up SendStat packet to refresh updated stats
                    break;
                case 3:
                // Reset attribute distribution
                    HandleResetStatDistribtuion(session, packet);
                    // follow up SendStat packet to refresh updated stats
                    break;
            }
        }

        private void HandleStatIncrement(GameSession session, PacketReader packet)
        {
            byte statTypeIndex = packet.ReadByte();
            Console.WriteLine("statIndex value: " + statTypeIndex);
            StatPointPacket.AddStatPoint(session.Player, statTypeIndex);
            session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        }
        private void HandleResetStatDistribtuion(GameSession session, PacketReader packet)
        {
            StatPointPacket.ResetStatPoints(session.Player);
            session.Send(StatPointPacket.WriteStatPointDistribution(session.Player));
        }
    }
}