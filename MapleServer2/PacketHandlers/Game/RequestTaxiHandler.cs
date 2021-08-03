using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

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
            RequestTaxiMode mode = (RequestTaxiMode) packet.ReadByte();

            int mapId = 0;
            long mesoPrice = 30000;
            long meretPrice = 15;

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
                    HandleRotorMeso(session, mapId, mesoPrice);
                    break;
                case RequestTaxiMode.RotorsMeret:
                    HandleRotorMeret(session, mapId, meretPrice);
                    break;
                case RequestTaxiMode.DiscoverTaxi:
                    HandleDiscoverTaxi(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleRotorMeso(GameSession session, int mapId, long mesoPrice)
        {
            if (!session.Player.Wallet.Meso.Modify(session, -mesoPrice))
            {
                return;
            }

            HandleTeleport(session, mapId);
        }

        private static void HandleRotorMeret(GameSession session, int mapId, long meretPrice)
        {
            if (!session.Player.Account.RemoveMerets(session, meretPrice))
            {
                return;
            }

            HandleTeleport(session, mapId);
        }

        private static void HandleDiscoverTaxi(GameSession session)
        {
            List<int> unlockedTaxis = session.Player.UnlockedTaxis;
            int mapId = session.Player.MapId;
            if (!unlockedTaxis.Contains(mapId))
            {
                unlockedTaxis.Add(mapId);
            }
            session.Send(TaxiPacket.DiscoverTaxi(mapId));
        }

        private static void HandleTeleport(GameSession session, int mapId)
        {
            session.Player.Warp(mapId);
        }
    }
}
