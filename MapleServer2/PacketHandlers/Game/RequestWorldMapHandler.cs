using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestWorldMapHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_WORLD_MAP;

        public RequestWorldMapHandler(ILogger<RequestWorldMapHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 0: // open
                    HandleOpen(session, packet);
                    break;
            }
            packet.ReadByte(); // always 0?
            int tab = packet.ReadInt();
        }

        private void HandleOpen(GameSession session, PacketReader packet)
        {
            session.Send(WorldMapPacket.Open(session.Player));
        }
    }
}
