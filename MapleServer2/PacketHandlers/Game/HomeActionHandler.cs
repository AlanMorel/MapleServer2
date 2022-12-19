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

public class HomeActionHandler : GamePacketHandler<HomeActionHandler>
{
    public override RecvOp OpCode => RecvOp.HomeAction;

    private enum Mode : byte
    {
        Smite = 0x01,
        Kick = 0x02,
        Survey = 0x05,
        ChangePortalSettings = 0x06,
        UpdateBallCoord = 0x07,
        SendPortalSettings = 0x0D
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Kick:
                HandleKick(packet);
                break;
            case Mode.Survey:
                HandleRespondSurvey(session, packet);
                break;
            case Mode.ChangePortalSettings:
                HandleChangePortalSettings(session, packet);
                break;
            case Mode.UpdateBallCoord:
                HandleUpdateBallCoord(session, packet);
                break;
            case Mode.SendPortalSettings:
                HandleSendPortalSettings(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleRespondSurvey(GameSession session, PacketReader packet)
    {
        packet.ReadByte();
        packet.ReadLong(); // character id
        long surveyId = packet.ReadLong();
        byte responseIndex = packet.ReadByte();

        Player player = session.Player;

        Home? home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home is null)
        {
            return;
        }

        HomeSurvey homeSurvey = home.Survey;

        string option = homeSurvey.Options.Keys.ToList()[responseIndex];
        if (!homeSurvey.Started || homeSurvey.Ended || homeSurvey.Id != surveyId || homeSurvey.Options[option].Contains(player.Name) || !homeSurvey.AvailableCharacters.Contains(player.Name))
        {
            return;
        }

        homeSurvey.AvailableCharacters.Remove(player.Name);
        homeSurvey.Options[option].Add(player.Name);
        session.Send(HomeActionPacket.SurveyAnswer(player.Name));

        homeSurvey.Answers++;

        if (homeSurvey.Answers < homeSurvey.MaxAnswers)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(HomeActionPacket.SurveyEnd(homeSurvey));
        homeSurvey.End();
    }

    private static void HandleKick(PacketReader packet)
    {
        string characterName = packet.ReadUnicodeString();
        Player? target = GameServer.PlayerManager.GetPlayerByName(characterName);
        if (target is null)
        {
            return;
        }

        target.Warp(target.ReturnMapId, target.ReturnCoord);
        target.ReturnMapId = 0;
        target.VisitingHomeId = 0;
    }

    private static void HandleChangePortalSettings(GameSession session, PacketReader packet)
    {
        packet.ReadByte();
        CoordB coordB = packet.Read<CoordB>();
        packet.ReadByte();

        IFieldObject<Cube>? fieldCube = session.FieldManager.State.Cubes.Values.FirstOrDefault(x => x.Coord == coordB.ToFloat());
        Cube? cube = fieldCube?.Value;

        if (cube?.PortalSettings == null)
        {
            return;
        }

        cube.PortalSettings.PortalName = packet.ReadUnicodeString();
        cube.PortalSettings.Method = (UgcPortalMethod) packet.ReadByte();
        cube.PortalSettings.Destination = (UgcPortalDestination) packet.ReadByte();
        cube.PortalSettings.DestinationTarget = packet.ReadUnicodeString();

        DatabaseManager.Cubes.Update(cube);

        UpdateAllPortals(session);
    }

    private static void HandleUpdateBallCoord(GameSession session, PacketReader packet)
    {
        byte mode = packet.ReadByte(); // 2 move, 3 hit ball
        int objectId = packet.ReadInt();
        CoordF coord = packet.Read<CoordF>();
        CoordF velocity1 = packet.Read<CoordF>();

        if (!session.FieldManager.State.Guide.TryGetValue(objectId, out IFieldObject<GuideObject>? ball))
        {
            return;
        }

        ball.Coord = coord;

        switch (mode)
        {
            case 2:
                CoordF velocity2 = packet.Read<CoordF>();

                session.FieldManager.BroadcastPacket(HomeActionPacket.UpdateBall(ball, velocity1, velocity2), session);
                break;
            case 3:
                session.FieldManager.BroadcastPacket(HomeActionPacket.HitBall(ball, velocity1));
                break;
        }
    }

    private static void HandleSendPortalSettings(GameSession session, PacketReader packet)
    {
        CoordB coordB = packet.Read<CoordB>();

        // 50400158 = Portal Cube
        IFieldObject<Cube>? cube = session.FieldManager.State.Cubes.Values
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
            if (cubePortal.PortalSettings is null)
            {
                continue;
            }

            Portal portal = new(GuidGenerator.Int())
            {
                IsVisible = true,
                IsEnabled = true,
                IsMinimapVisible = false,
                Rotation = cubePortal.Rotation,
                PortalType = PortalTypes.Home,
                UgcPortalMethod = cubePortal.PortalSettings.Method
            };

            IFieldObject<Portal> fieldPortal = session.FieldManager.RequestFieldObject(portal);
            fieldPortal.Coord = cubePortal.CoordF;
            if (!string.IsNullOrEmpty(cubePortal.PortalSettings.DestinationTarget))
            {
                switch (cubePortal.PortalSettings.Destination)
                {
                    case UgcPortalDestination.PortalInHome:
                        fieldPortal.Value.TargetMapId = (int) Map.PrivateResidence;
                        break;
                    case UgcPortalDestination.SelectedMap:
                        fieldPortal.Value.TargetMapId = int.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                    case UgcPortalDestination.FriendHome:
                        fieldPortal.Value.TargetHomeAccountId = long.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                }
            }
            cubePortal.PortalSettings.PortalObjectId = fieldPortal.ObjectId;
            session.FieldManager.AddPortal(fieldPortal);
        }
    }
}
