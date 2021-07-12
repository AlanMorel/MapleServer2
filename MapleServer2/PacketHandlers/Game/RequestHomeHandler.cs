using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestHomeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_HOME;

        public RequestHomeHandler(ILogger<RequestHomeHandler> logger) : base(logger) { }

        private enum RequestHomeMode : byte
        {
            MoveToHome = 0x03
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestHomeMode mode = (RequestHomeMode) packet.ReadByte();
            switch (mode)
            {
                case RequestHomeMode.MoveToHome:
                    HandleMoveToHome(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        // The same mode also handles creation of new homes.
        private static void HandleMoveToHome(GameSession session, PacketReader packet)
        {
            int houseTemplate = packet.ReadInt();
            Player player = session.Player;
            if (player.Account.Home == null)
            {
                player.Account.Home = new Home(player.Account, player.Name);
            }

            player.ReturnMapId = player.MapId;
            player.ReturnCoord = player.SafeBlock;
            player.VisitingHomeId = player.Account.Home.Id;
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));

            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(player.Account.Home.MapId);
            player.Warp(spawn.Coord.ToFloat(), spawn.Rotation.ToFloat(), player.Account.Home.MapId, instanceId: player.Account.Home.Id);
        }
    }
}
