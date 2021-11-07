using System.Drawing;
using System.Text;
using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class InfoCommands : InGameCommand
{
    public InfoCommands()
    {
        Aliases = new()
        {
            "commands"
        };
        Description = "Give informations about all commands";
        Usage = "/commands";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        List<IGrouping<string, CommandBase>> commandList = CommandManager.CommandsByAlias.Values.GroupBy(x => x.Aliases[0]).ToList();
        StringBuilder stringBuilder = new("Commands:\n");

        foreach (IGrouping<string, CommandBase> commandGroup in commandList)
        {
            string aliases = string.Empty;
            CommandBase commandBase = commandGroup.First();
            if (commandBase.Aliases.Skip(1).Any())
            {
                aliases = $" [{string.Join(", ", commandBase.Aliases.Skip(1))}]";
            }
            stringBuilder.Append((commandBase.Aliases.First() + aliases).Color(Color.DarkOrange).Bold() + " ");

            stringBuilder.Append($"{commandBase.Usage.Color(Color.Wheat)}\n");
        }
        trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
    }
}
public class InfoCommand : InGameCommand
{
    public InfoCommand()
    {
        Aliases = new()
        {
            "info"
        };
        Description = "Give informations about an command";
        Parameters = new()
        {
            new Parameter<string>("command", "The command to get help about", string.Empty)
        };
        Usage = "/info [command]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string command = trigger.Get<string>("command");
        if (string.IsNullOrEmpty(command))
        {
            trigger.Session.Send(NoticePacket.Notice("You must specify a command to get help about", NoticeType.Chat));
            return;
        }


        if (!CommandManager.CommandsByAlias.TryGetValue(command, out CommandBase commandBase))
        {
            trigger.Session.Send(NoticePacket.Notice($"Command {command} not found", NoticeType.Chat));
            return;
        }

        StringBuilder stringBuilder = new();
        string aliases = string.Empty;
        if (commandBase.Aliases.Skip(1).Any())
        {
            aliases = $" [{string.Join(", ", commandBase.Aliases.Skip(1))}]";
        }
        stringBuilder.Append((commandBase.Aliases.First() + aliases).Color(Color.DarkOrange).Bold() + " ");

        stringBuilder.Append($"{commandBase.Description.Color(Color.Wheat)}\n");
        stringBuilder.Append($"{commandBase.Usage.Color(Color.Wheat)}\n");

        foreach (IParameter param in commandBase.Parameters)
        {
            stringBuilder.Append($"    -{param.Name.Color(Color.Aquamarine)} {param.Description.Color(Color.Wheat)}");
            if (commandBase.Parameters.Last() != param)
            {
                stringBuilder.Append('\n');
            }
        }
        trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
    }

    public class SendNoticeCommand : InGameCommand
    {
        public SendNoticeCommand()
        {
            Aliases = new()
            {
                "notice"
            };
            Description = "Send a server message.";
            Parameters = new()
            {
                new Parameter<string[]>("message", "Message to send on the server.")
            };
            Usage = "/notice <message>";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            string[] args = trigger.Get<string[]>("message");

            if (args == null || args.Length <= 1)
            {
                trigger.Session.SendNotice("No message provided.");
                return;
            }

            string message = CommandHelpers.BuildString(args, trigger.Session.Player.Name);
            MapleServer.BroadcastPacketAll(NoticePacket.Notice(message));
        }
    }

    public class OnlineCommand : InGameCommand
    {
        public OnlineCommand()
        {
            Aliases = new()
            {
                "online"
            };
            Description = "See all online players.";
            Usage = "/online";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            List<Player> players = GameServer.PlayerManager.GetAllPlayers();
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"Online players:".Color(Color.DarkOrange).Bold() + "\n");
            stringBuilder.Append(string.Join(", ", players.Select(p => p.Name)));

            trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
        }
    }
}
