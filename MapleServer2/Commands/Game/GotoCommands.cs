using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
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

        if (MapMetadataStorage.GetMetadata(mapId) is null)
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

public class MapCommand : InGameCommand
{
    // Maybe merge this command with /map, but I don't know if it's easily possible.
    public MapCommand()
    {
        Aliases = new()
        {
            "m"
        };
        Description = "Move to map";
        Parameters = new()
        {
            new Parameter<string[]>("map", "The map id or map name.")
        };
        Usage = "/m [id / map name]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string[] command = trigger.Get<string[]>("map");
        if (command is null)
        {
            return;
        }

        string[] map = command[1..];
        if (map.Length == 0)
        {
            trigger.Session.SendNotice($"Current map id:{trigger.Session.Player.MapId} instance: {trigger.Session.Player.InstanceId}");
            return;
        }

        string mapName = string.Join(" ", map).Trim();

        if (!int.TryParse(mapName, out int mapId))
        {
            MapMetadata mapMetadata =
                MapMetadataStorage.GetAll().FirstOrDefault(x => string.Equals(x.Name, mapName, StringComparison.CurrentCultureIgnoreCase));
            if (mapMetadata is null)
            {
                trigger.Session.SendNotice($"Map '{mapName}' not found.");
                return;
            }

            mapId = mapMetadata.Id;
        }

        if (MapMetadataStorage.GetMetadata(mapId) is null)
        {
            trigger.Session.SendNotice("Map doesn't exists.");
            return;
        }

        if (trigger.Session.Player.MapId == mapId)
        {
            trigger.Session.SendNotice("You are already on that map.");
            return;
        }

        trigger.Session.Player.Warp(mapId);
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

        IFieldObject<Player> targetFieldPlayer = target.Session.Player.FieldPlayer;

        Player player = trigger.Session.Player;
        if (target.MapId == player.MapId && target.InstanceId == player.InstanceId)
        {
            player.Move(targetFieldPlayer.Coord, targetFieldPlayer.Rotation);
            return;
        }

        if (player.ChannelId != target.ChannelId)
        {
            player.ChannelId = target.ChannelId;
            player.WarpGameToGame(target.MapId, target.InstanceId, targetFieldPlayer.Coord);
            return;
        }

        player.Warp(target.MapId, targetFieldPlayer.Coord, instanceId: target.InstanceId);
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

        Player player = trigger.Session.Player;
        IFieldActor<Player> fieldPlayer = player.FieldPlayer;
        if (coordF == default)
        {
            trigger.Session.SendNotice($"Coord: {fieldPlayer.Coord}");
            trigger.Session.SendNotice($"Closest Block: {Block.ClosestBlock(fieldPlayer.Coord)}");
            return;
        }

        player.Move(coordF, fieldPlayer.Rotation);
    }
}
