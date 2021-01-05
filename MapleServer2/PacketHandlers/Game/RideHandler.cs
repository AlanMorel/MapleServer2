using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RideHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_RIDE;

        public RideHandler(ILogger<RideHandler> logger) : base(logger) { }

        // Test Ids: 50600145, 50600155
        public override void Handle(GameSession session, PacketReader packet)
        {
            byte function = packet.ReadByte();
            switch (function)
            {
                case 0:
                    HandleStartRide(session, packet);
                    break;
                case 1:
                    HandleStopRide(session, packet);
                    break;
                case 2:
                    HandleChangeRide(session, packet);
                    break;
            }
        }

        private static void HandleStartRide(GameSession session, PacketReader packet)
        {
            RideType type = (RideType) packet.ReadByte();
            int mountId = packet.ReadInt();
            packet.ReadLong();
            long mountUid = packet.ReadLong();
            // [46-0s] (UgcPacketHelper.Ugc()) but client doesn't set this data?

            IFieldObject<Mount> fieldMount =
                session.FieldManager.RequestFieldObject(new Mount { Type = type, Id = mountId, Uid = mountUid });
            session.Player.Mount = fieldMount;

            Packet startPacket = MountPacket.StartRide(session.FieldPlayer);
            session.FieldManager.BroadcastPacket(startPacket);
        }

        private static void HandleStopRide(GameSession session, PacketReader packet)
        {
            packet.ReadByte();
            bool forced = packet.ReadBool(); // Going into water without amphibious riding

            session.Player.Mount = null; // Remove mount from player
            Packet stopPacket = MountPacket.StopRide(session.FieldPlayer, forced);
            session.FieldManager.BroadcastPacket(stopPacket);
        }

        private static void HandleChangeRide(GameSession session, PacketReader packet)
        {
            int mountId = packet.ReadInt();
            long mountUid = packet.ReadLong();

            Packet changePacket = MountPacket.ChangeRide(session.FieldPlayer.ObjectId, mountId, mountUid);
            session.FieldManager.BroadcastPacket(changePacket);
        }
    }
}
