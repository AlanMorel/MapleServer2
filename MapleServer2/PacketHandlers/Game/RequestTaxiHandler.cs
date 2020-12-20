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
            long price = 0;

            switch (mode)
            {
                case 3: // rotors using mesos
                    price = 60000;
                    if (session.Player.Mesos >= price) {
                        session.Player.Mesos -= price;
                        session.Send(MesosPacket.UpdateMesos(session));
                    }
                    break;
                case 4: // rotors using merets
                    price = 15;
                    if (session.Player.Merets >= price)
                    {
                        session.Player.Merets -= price;
                        session.Send(MeretsPacket.UpdateMerets(session));
                    }
                    break;
                case 5: // is sent after using rotors with meret, idk why..
                    return;
            }

            int mapId = packet.ReadInt();

            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);

            if (spawn != null)
            {
                session.Player.MapId = mapId;
                session.Player.Coord = spawn.Coord.ToFloat();
                session.Player.Rotation = spawn.Rotation.ToFloat();
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
        }
    }
}
