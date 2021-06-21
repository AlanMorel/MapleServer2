using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class VibrateHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.VIBRATE;

        public VibrateHandler(ILogger<VibrateHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            string entityId = packet.ReadMapleString(); //Object hash
            long someId = packet.ReadLong();
            int objectId = packet.ReadInt(); //Object id??

            int flag = packet.ReadInt();

            int empty = packet.ReadInt(); //Empty

            int unk1 = packet.ReadInt();
            int unk2 = packet.ReadInt();
            int unk3 = packet.ReadInt();

            session.FieldManager.BroadcastPacket(VibratePacket.Vibrate(entityId, someId, objectId, flag, session.FieldPlayer, session.ServerTick));
        }
    }
}
