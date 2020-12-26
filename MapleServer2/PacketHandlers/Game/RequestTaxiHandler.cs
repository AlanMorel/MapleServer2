using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using Microsoft.Extensions.Logging;
using Maple2Storage.Types;
using MapleServer2.Data.Static;
using System;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.PacketHandlers.Game
{
    class RequestTaxiHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_TAXI;

        public RequestTaxiHandler(ILogger<RequestTaxiHandler> logger) : base(logger) { }

        private enum RequestTaxiMode : byte
        {
            Car = 0x1,
            RotorsMeso = 0x3,
            RotorsMeret = 0x4,
            DiscoverTaxi = 0x5
        };

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestTaxiMode mode = (RequestTaxiMode)packet.ReadByte();

            int mapId = 0;
            long mesoPrice = 60000;

            if (mode != RequestTaxiMode.DiscoverTaxi)
            {
                mapId = packet.ReadInt();
            }

            switch (mode)
            {
                case RequestTaxiMode.Car:
                    mesoPrice = 5000; //For now make all car taxis cost 5k, as we don't know the formula to calculate it yet.
                    goto case RequestTaxiMode.RotorsMeso;
                case RequestTaxiMode.RotorsMeso:
                    if (session.Player.Mesos >= mesoPrice)
                    {
                        session.Player.Mesos -= mesoPrice;
                        session.Send(MesosPacket.UpdateMesos(session));
                        HandleTeleport(session, mapId);
                    }
                    break;
                case RequestTaxiMode.RotorsMeret:
                    if (session.Player.Merets >= 15)
                    {
                        session.Player.Merets -= 15;
                        session.Send(MeretsPacket.UpdateMerets(session));
                        HandleTeleport(session, mapId);
                    }
                    break;
                case RequestTaxiMode.DiscoverTaxi:
                    //TODO: Save somewhere and load somewhere? Perhaps on login.
                    session.Send(TaxiPacket.DiscoverTaxi(session.Player.MapId));
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleTeleport(GameSession session, int mapId)
        {
            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);
            if (spawn != null)
            {
                session.Player.Warp(spawn, mapId);
            }
        }
    }
}
