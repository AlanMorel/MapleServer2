using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using Microsoft.Extensions.Logging;
using Maple2Storage.Types;
using MapleServer2.Data.Static;

namespace MapleServer2.PacketHandlers.Game
{
    class RequestTaxiHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_TAXI;

        public RequestTaxiHandler(ILogger<RequestTaxiHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            int mapId = packet.ReadInt();

            switch (mode)
            {
                case 3: // rotors using mesos?
                    break;
            }

            //TODO: figure out when to pay which currency Merits/Mesos?
            //TODO: get correct player spawn coordinates
            MapPortal dstPortal = MapEntityStorage.GetFirstPortal(mapId);

            if (dstPortal != null)
            {
                session.Player.MapId = mapId;
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }
    }
}
