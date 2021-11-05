using Maple2Storage.Types;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class GotoMapCommand : InGameCommand
{
    public GotoMapCommand()
    {
        Aliases = new()
        {
            "map"
        };
        Description = "Give informations about a map";
        Parameters = new()
        {
            new Parameter<int>("id", "The map id."),
            new Parameter<int>("instance", "The instance id.")
        };
        Usage = "/map [id] [instance]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        int mapId = trigger.Get<int>("id");
        int instanceId = trigger.Get<int>("instance");

        if (MapMetadataStorage.GetMetadata(mapId) == null)
        {
            trigger.Session.SendNotice($"Current map id:{trigger.Session.Player.MapId} instance: {trigger.Session.Player.InstanceId}");
            return;
        }
        if (trigger.Session.Player.MapId == mapId && trigger.Session.Player.InstanceId == instanceId)
        {
            trigger.Session.SendNotice("You are already on that map.");
            return;
        }
        trigger.Session.Player.Warp(mapId, instanceId: instanceId);
    }
}
public class GotoPlayerCommand : InGameCommand
{
    public GotoPlayerCommand()
    {
        Aliases = new()
        {
            "goto"
        };
        Description = "Go to a player location.";
        Parameters = new()
        {
            new Parameter<string>("name", "Name of the target player.", string.Empty)
        };
        Usage = "/goto [player name]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string name = trigger.Get<string>("name");
        Player target = GameServer.PlayerManager.GetPlayerByName(name);

        if (target is null)
        {
            trigger.Session.SendNotice($"Couldn't find player with name: {name}!");
            return;
        }
        IFieldObject<Player> fieldPlayer = target.Session.FieldPlayer;

        if (target.MapId == trigger.Session.Player.MapId && target.InstanceId == trigger.Session.Player.InstanceId)
        {
            trigger.Session.Send(UserMoveByPortalPacket.Move(trigger.Session.FieldPlayer, fieldPlayer.Coord, fieldPlayer.Rotation));
            return;
        }
        trigger.Session.Player.Warp(target.MapId, fieldPlayer.Coord, instanceId: target.InstanceId);
    }
}
public class GotoCoordCommand : InGameCommand
{
    public GotoCoordCommand()
    {
        Aliases = new()
        {
            "coord"
        };
        Description = "Get the current coord of the player.";
        Parameters = new()
        {
            new Parameter<CoordF>("pos", "The position in map.", CoordF.From(0, 0, 0))
        };
        Usage = "/coord [x], [y], [z]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        CoordF coordF = trigger.Get<CoordF>("pos");

        if (coordF == default)
        {
            trigger.Session.SendNotice(trigger.Session.FieldPlayer.Coord.ToString());
            return;
        }
        trigger.Session.Player.Coord = coordF;
        trigger.Session.Send(UserMoveByPortalPacket.Move(trigger.Session.FieldPlayer, coordF, trigger.Session.Player.Rotation));
    }
}
