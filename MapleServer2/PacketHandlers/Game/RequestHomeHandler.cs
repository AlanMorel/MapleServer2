using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestHomeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_HOME;

        public RequestHomeHandler() : base() { }

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
            if (session.Player.Account.Home == null)
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
                player.Account.Home = new Home(player.Account.Id, player.Name, homeTemplate);
                GameServer.HomeManager.AddHome(player.Account.Home);

                // Send inventories
                session.Send(WarehouseInventoryPacket.StartList());
                int counter = 0;
                foreach (KeyValuePair<long, Item> kvp in player.Account.Home.WarehouseInventory)
                {
                    session.Send(WarehouseInventoryPacket.Load(kvp.Value, ++counter));
                }
                session.Send(WarehouseInventoryPacket.EndList());

                session.Send(FurnishingInventoryPacket.StartList());
                foreach (Cube cube in player.Account.Home.FurnishingInventory.Values.Where(x => x.Item != null))
                {
                    session.Send(FurnishingInventoryPacket.Load(cube));
                }
                session.Send(FurnishingInventoryPacket.EndList());
            }
            Home home = GameServer.HomeManager.GetHomeById(player.Account.Home.Id);

            player.VisitingHomeId = player.Account.Home.Id;
            player.Guide = null;
            session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer));

            player.Warp(home.MapId, player.Coord, player.Rotation, instanceId: home.InstanceId);
        }
    }
}
