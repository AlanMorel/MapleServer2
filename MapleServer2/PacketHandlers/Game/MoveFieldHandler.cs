using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MoveFieldHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_MOVE_FIELD;

    public MoveFieldHandler() : base() { }

    private enum RequestMoveFieldMode : byte
    {
        Move = 0x0,
        LeaveInstance = 0x1,
        VisitHouse = 0x02,
        ReturnMap = 0x03,
        EnterDecorPlaner = 0x04
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        RequestMoveFieldMode mode = (RequestMoveFieldMode) packet.ReadByte();

        switch (mode)
        {
            case RequestMoveFieldMode.Move:
                HandleMove(session, packet);
                break;
            case RequestMoveFieldMode.LeaveInstance:
                HandleLeaveInstance(session);
                break;
            case RequestMoveFieldMode.VisitHouse:
                HandleVisitHouse(session, packet);
                break;
            case RequestMoveFieldMode.ReturnMap:
                HandleReturnMap(session);
                break;
            case RequestMoveFieldMode.EnterDecorPlaner:
                HandleEnterDecorPlaner(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleMove(GameSession session, PacketReader packet)
    {
        int srcMapId = packet.ReadInt();
        if (srcMapId != session.FieldManager.MapId)
        {
            return;
        }

        int portalId = packet.ReadInt();
        IFieldObject<Portal> fieldPortal = session.FieldManager.State.Portals.Values.FirstOrDefault(x => x.Value.Id == portalId);
        if (fieldPortal == default)
        {
            Logger.Warn($"Unable to find portal:{portalId} in map:{srcMapId}");
            return;
        }

        Portal srcPortal = fieldPortal.Value;
        switch (srcPortal.PortalType)
        {
            case PortalTypes.Field:
                break;
            case PortalTypes.DungeonReturnToLobby:
                DungeonSession dungeonSession = GameServer.DungeonManager.GetDungeonSessionByInstanceId(session.Player.InstanceId);
                if (dungeonSession == null)
                {
                    return;
                }
                session.Player.Warp(dungeonSession.DungeonLobbyId, instanceId: dungeonSession.DungeonInstanceId);
                return;
            case PortalTypes.LeaveDungeon:
                HandleLeaveInstance(session);
                return;
            case PortalTypes.Home:
                HandleHomePortal(session, fieldPortal);
                return;
            default:
                Logger.Warn($"unknown portal type id: {srcPortal.PortalType}");
                break;
        }

        if (!MapEntityStorage.HasSafePortal(srcMapId) || srcPortal.TargetMapId == 0) // map is instance only
        {
            HandleLeaveInstance(session);
            return;
        }

        MapPortal dstPortal = MapEntityStorage.GetPortals(srcPortal.TargetMapId)
            .FirstOrDefault(portal => portal.Id == srcPortal.TargetPortalId); // target map's portal id == source portal's targetPortalId
        if (dstPortal == default)
        {
            session.Player.Warp(srcPortal.TargetMapId);
            return;
        }

        CoordF coord = dstPortal.Coord.ToFloat();

        if (dstPortal.Name == "Portal_cube") // spawn on the next block if portal is a cube
        {
            if (dstPortal.Rotation.Z == Direction.SOUTH_EAST)
            {
                coord.Y -= Block.BLOCK_SIZE;
            }
            else if (dstPortal.Rotation.Z == Direction.NORTH_EAST)
            {
                coord.X += Block.BLOCK_SIZE;
            }
            else if (dstPortal.Rotation.Z == Direction.NORTH_WEST)
            {
                coord.Y += Block.BLOCK_SIZE;
            }
            else if (dstPortal.Rotation.Z == Direction.SOUTH_WEST)
            {
                coord.X -= Block.BLOCK_SIZE;
            }
        }

        session.Player.Warp(srcPortal.TargetMapId, coord, dstPortal.Rotation.ToFloat());
    }

    private static void HandleHomePortal(GameSession session, IFieldObject<Portal> fieldPortal)
    {
        IFieldObject<Cube> srcCube = session.FieldManager.State.Cubes.Values
            .FirstOrDefault(x => x.Value.PortalSettings is not null
                                && x.Value.PortalSettings.PortalObjectId == fieldPortal.ObjectId);
        if (srcCube is null)
        {
            return;
        }

        string destinationTarget = srcCube.Value.PortalSettings.DestinationTarget;
        if (string.IsNullOrEmpty(destinationTarget))
        {
            return;
        }

        switch (srcCube.Value.PortalSettings.Destination)
        {
            case UGCPortalDestination.PortalInHome:
                IFieldObject<Cube> destinationCube = session.FieldManager.State.Cubes.Values
                    .FirstOrDefault(x => x.Value.PortalSettings is not null
                                        && x.Value.PortalSettings.PortalName == destinationTarget);
                if (destinationCube is null)
                {
                    return;
                }
                session.Player.Coord = destinationCube.Coord;
                CoordF coordF = destinationCube.Coord;
                coordF.Z += 25; // Without this the player falls through the ground.
                session.Send(UserMoveByPortalPacket.Move(session.FieldPlayer, coordF, session.Player.Rotation));
                break;
            case UGCPortalDestination.SelectedMap:
                session.Player.Warp(int.Parse(destinationTarget));
                break;
            case UGCPortalDestination.FriendHome:
                long friendAccountId = long.Parse(destinationTarget);
                Home home = GameServer.HomeManager.GetHomeById(friendAccountId);
                if (home is null)
                {
                    return;
                }
                session.Player.WarpGameToGame((int) Map.PrivateResidence, instanceId: home.InstanceId);
                break;
        }
    }

    private static void HandleLeaveInstance(GameSession session)
    {
        Player player = session.Player;
        player.Warp(player.ReturnMapId, player.ReturnCoord, player.Rotation);
    }

    private static void HandleVisitHouse(GameSession session, PacketReader packet)
    {
        int returnMapId = packet.ReadInt();
        packet.Skip(8);
        long accountId = packet.ReadLong();
        string password = packet.ReadUnicodeString();

        Player target = GameServer.PlayerManager.GetPlayerByAccountId(accountId);
        if (target is null)
        {
            target = DatabaseManager.Characters.FindPartialPlayerById(accountId);
            if (target is null)
            {
                return;
            }
        }
        Player player = session.Player;

        Home home = GameServer.HomeManager.GetHomeByAccountId(accountId);
        if (home == null)
        {
            session.SendNotice("This player does not have a home!");
            return;
        }

        if (player.VisitingHomeId == home.Id && player.MapId == (int) Map.PrivateResidence)
        {
            session.SendNotice($"You are already at {target.Name}'s home!");
            return;
        }

        if (home.IsPrivate)
        {
            if (password == "")
            {
                session.Send(EnterUGCMapPacket.RequestPassword(accountId));
                return;
            }

            if (home.Password != password)
            {
                session.Send(EnterUGCMapPacket.WrongPassword(accountId));
                return;
            }
        }

        player.VisitingHomeId = home.Id;
        session.Send(ResponseCubePacket.LoadHome(session.FieldPlayer.ObjectId, home));

        player.WarpGameToGame(home.MapId, home.InstanceId, player.Coord, player.Rotation);
    }

    // This also leaves decor planning
    private static void HandleReturnMap(GameSession session)
    {
        Player player = session.Player;
        if (player.IsInDecorPlanner)
        {
            player.IsInDecorPlanner = false;
            player.Guide = null;
            player.WarpGameToGame((int) Map.PrivateResidence, instanceId: --player.InstanceId);
            return;
        }

        CoordF returnCoord = player.ReturnCoord;
        returnCoord.Z += Block.BLOCK_SIZE;
        player.WarpGameToGame(player.ReturnMapId, 1, returnCoord, player.Rotation);
        player.ReturnMapId = 0;
        player.VisitingHomeId = 0;
    }

    private static void HandleEnterDecorPlaner(GameSession session)
    {
        Player player = session.Player;
        if (player.IsInDecorPlanner)
        {
            return;
        }

        player.IsInDecorPlanner = true;
        player.Guide = null;
        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        home.DecorPlannerHeight = home.Height;
        home.DecorPlannerSize = home.Size;
        home.DecorPlannerInventory = new();
        player.WarpGameToGame((int) Map.PrivateResidence, instanceId: ++player.InstanceId);
    }
}
