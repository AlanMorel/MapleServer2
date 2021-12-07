using System.Text;
using MapleServer2.Commands.Core;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game.HomeCommands;

public class AlarmCommand : InGameCommand
{
    public AlarmCommand()
    {
        Aliases = new()
        {
            "hostalarm"
        };
        Description = "Send message to all players in the same map.";
        Usage = "hostalarm <message>";
        Parameters = new()
        {
            new Parameter<string[]>("message", "Message")
        };
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        Player player = trigger.Session.Player;
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

        string[] args = trigger.Get<string[]>("message");

        if (args is null)
        {
            trigger.Session.FieldManager.BroadcastPacket(HomeActionPacket.HostAlarm(string.Empty));
            return;
        }

        StringBuilder message = new();
        foreach (string arg in args[1..])
        {
            message.Append($"{arg} ");
        }
        trigger.Session.FieldManager.BroadcastPacket(HomeActionPacket.HostAlarm(message.ToString().Trim()));
    }
}
