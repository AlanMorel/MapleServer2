using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class HomeActionHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.HOME_ACTION;

    public HomeActionHandler() : base() { }

    private enum HomeActionMode : byte
    {
        Smite = 0x01,
        Kick = 0x02,
        ChangePortalSettings = 0x06,
        SendPortalSettings = 0x0D
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        HomeActionMode mode = (HomeActionMode) packet.ReadByte();

        switch (mode)
        {
            case HomeActionMode.Kick:
                HandleKick(packet);
                break;
            case HomeActionMode.ChangePortalSettings:
                HandleChangePortalSettings(session, packet);
                break;
            case HomeActionMode.SendPortalSettings:
                HandleSendPortalSettings(session, packet);
                break;
        }
    }

    private static void HandleKick(PacketReader packet)
    {
        string characterName = packet.ReadUnicodeString();
        Player target = GameServer.PlayerManager.GetPlayerByName(characterName);
        if (target == null)
        {
            return;
        }

        target.Warp(target.ReturnMapId, target.ReturnCoord, target.Session.FieldPlayer.Rotation);
        target.ReturnMapId = 0;
        target.VisitingHomeId = 0;
    }

    private static void HandleChangePortalSettings(GameSession session, PacketReader packet)
    {
        packet.ReadByte();
        CoordB coordB = packet.Read<CoordB>();
        packet.ReadByte();

        IFieldObject<Cube> fieldCube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coordB.ToFloat());
        if (fieldCube is null)
        {
            return;
        }
        Cube cube = fieldCube.Value;

        cube.PortalSettings.PortalName = packet.ReadUnicodeString();
        cube.PortalSettings.Method = (UGCPortalMethod) packet.ReadByte();
        cube.PortalSettings.Destination = (UGCPortalDestination) packet.ReadByte();
        cube.PortalSettings.DestinationTarget = packet.ReadUnicodeString();

        DatabaseManager.Cubes.Update(cube);

        UpdateAllPortals(session);
    }

    private static void HandleSendPortalSettings(GameSession session, PacketReader packet)
    {
        CoordB coordB = packet.Read<CoordB>();

        // 50400158 = Portal Cube
        IFieldObject<Cube> cube = session.FieldManager.State.Cubes.Values
            .FirstOrDefault(x => x.Coord == coordB.ToFloat() && x.Value.Item.Id == 50400158);
        if (cube is null)
        {
            return;
        }

        List<Cube> otherPortals = session.FieldManager.State.Cubes.Values
            .Where(x => x.Value.Item.Id == 50400158 && x.Value.Uid != cube.Value.Uid)
            .Select(x => x.Value).ToList();
        session.Send(HomeActionPacket.SendCubePortalSettings(cube.Value, otherPortals));
    }

    private static void UpdateAllPortals(GameSession session)
    {
        foreach (IFieldObject<Portal> fieldPortal in session.FieldManager.State.Portals.Values)
        {
            session.FieldManager.RemovePortal(fieldPortal);
        }

        // Re-add cube portals in map
        IEnumerable<IFieldObject<Cube>> fieldCubePortals = session.FieldManager.State.Cubes.Values.Where(x => x.Value.Item.Id == 50400158);
        foreach (IFieldObject<Cube> fieldCubePortal in fieldCubePortals)
        {
            Cube cubePortal = fieldCubePortal.Value;
            Portal portal = new(GuidGenerator.Int())
            {
                IsVisible = true,
                IsEnabled = true,
                IsMinimapVisible = false,
                Rotation = cubePortal.Rotation,
                PortalType = PortalTypes.Home,
                UGCPortalMethod = cubePortal.PortalSettings.Method
            };

            IFieldObject<Portal> fieldPortal = session.FieldManager.RequestFieldObject(portal);
            fieldPortal.Coord = cubePortal.CoordF;
            if (!string.IsNullOrEmpty(cubePortal.PortalSettings.DestinationTarget))
            {
                switch (cubePortal.PortalSettings.Destination)
                {
                    case UGCPortalDestination.PortalInHome:
                        fieldPortal.Value.TargetMapId = (int) Map.PrivateResidence;
                        break;
                    case UGCPortalDestination.SelectedMap:
                        fieldPortal.Value.TargetMapId = int.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                    case UGCPortalDestination.FriendHome:
                        fieldPortal.Value.TargetHomeAccountId = long.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                }
            }
            cubePortal.PortalSettings.PortalObjectId = fieldPortal.ObjectId;
            session.FieldManager.AddPortal(fieldPortal);
        }
    }
}
