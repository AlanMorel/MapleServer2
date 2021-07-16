using MaplePacketLib2.Tools;
using MapleServer2.Constants;
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
            InviteToHome = 0x01,
            MoveToHome = 0x03
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            RequestHomeMode mode = (RequestHomeMode) packet.ReadByte();
            switch (mode)
            {
                case RequestHomeMode.InviteToHome:
                    HandleInviteToHome(session, packet);
                    break;
                case RequestHomeMode.MoveToHome:
                    HandleMoveToHome(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleInviteToHome(GameSession session, PacketReader packet)
        {
            string characterName = packet.ReadUnicodeString();
            Player target = GameServer.Storage.GetPlayerByName(characterName);
            if (target == null)
            {
                return;
            }

            target.Session.Send(InviteToHomePacket.InviteToHome(session.Player));
        }

        // The same mode also handles creation of new homes.
        private static void HandleMoveToHome(GameSession session, PacketReader packet)
        {
            int homeTemplate = packet.ReadInt();
            Player player = session.Player;
            if (player.Account.Home == null)
            {
                player.Account.Home = new Home(player.Account, player.Name, homeTemplate);
                GameServer.HomeManager.AddHome(player.Account.Home);
            }

            if (player.MapId != (int) Map.PrivateResidence)
            {
                player.ReturnMapId = player.MapId;
                player.ReturnCoord = player.SafeBlock;
            }
            player.VisitingHomeId = player.Account.Home.Id;
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));

            player.Warp(player.Account.Home.MapId, instanceId: player.Account.Home.Id);
        }
    }
}
