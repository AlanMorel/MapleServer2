using Maple2Storage.Types;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game.HomeCommands;

public class BallCommand : InGameCommand
{
    public BallCommand()
    {
        Aliases = new()
        {
            "hostball"
        };
        Usage = "hostball [value]";
        Description = "Changes the gravity of the map.";
        Parameters = new()
        {
            new Parameter<int>("size", "The size of the ball.", 1)
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
        IFieldActor<Player> fieldPlayer = player.FieldPlayer;
        FieldManager fieldManager = trigger.Session.FieldManager;

        bool mapIsHome = player.MapId == (int) Map.PrivateResidence;

        if (!mapIsHome)
        {
            return;
        }

        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home.AccountId != player.AccountId)
        {
            return;
        }

        IFieldObject<GuideObject> ballObject = fieldManager.State.Guide.Values.FirstOrDefault(x => x.Value.IsBall);
        if (ballObject is not null)
        {
            fieldManager.RemoveGuide(ballObject);
            fieldManager.BroadcastPacket(HomeActionPacket.RemoveBall(ballObject));
            return;
        }

        int size = trigger.Get<int>("size");

        size = Math.Min(30 + size * 30, 330);
        if (size < 0)
        {
            size = 60;
        }

        GuideObject ball = new(0, player.CharacterId)
        {
            IsBall = true
        };
        IFieldObject<GuideObject> fieldObject = fieldManager.RequestFieldObject(ball);

        fieldObject.Coord = CoordF.From(fieldPlayer.Coord.X, fieldPlayer.Coord.Y, fieldPlayer.Coord.Z + Block.BLOCK_SIZE * 2);
        fieldObject.Rotation = CoordF.From(0, 0, size);

        fieldManager.AddGuide(fieldObject);

        fieldManager.BroadcastPacket(HomeActionPacket.AddBall(fieldObject));
    }
}
