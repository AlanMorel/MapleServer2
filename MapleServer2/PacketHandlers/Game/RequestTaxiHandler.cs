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

            switch (mode)
            {
                case 3: // rotors using mesos
                    session.Player.Mesos -= 60000;
                    session.Send(MesosPacket.UpdateMesos(session));
                    break;
                case 4: // rotors using merets
                    session.Player.Merets -= 15;
                    session.Send(MeretsPacket.UpdateMerets(session));
                    break;
                case 5: // is sent after using rotors with meret, idk why..
                    return;
            }

            int mapId = packet.ReadInt();

            //TODO: get correct player spawn coordinates
            MapPortal dstPortal = MapEntityStorage.GetFirstPortal(mapId);

            if (dstPortal != null)
            {
                session.Player.MapId = mapId;
                session.Player.Coord = dstPortal.Coord.ToFloat();
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }
    }
}
