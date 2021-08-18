using System.Drawing;
using System.Text;
using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Tools;

namespace MapleServer2.Commands.Game
{
    public class InfoCommands : InGameCommand
    {
        public InfoCommands()
        {
            Aliases = new[]
            {
                "commands"
            };
            Description = "Give informations about all commands";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            List<CommandBase> commandList = CommandManager.CommandsByAlias.Values.ToList();
            StringBuilder stringBuilder = new();

            foreach (CommandBase command in commandList)
            {
                foreach (string alias in command.Aliases)
                {
                    stringBuilder.Append(CommandHelpers.Color(CommandHelpers.Bold(alias), Color.DarkOrange));
                }

                stringBuilder.Append($"{CommandHelpers.Color(command.Description, Color.Wheat)}\n");
                foreach (IParameter param in command.Parameters)
                {
                    stringBuilder.Append($" - {CommandHelpers.Color(param.Name, Color.Aquamarine)} {CommandHelpers.Color(param.Description, Color.Wheat)}\n");
                }
            }
            trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
        }
    }

    public class SendNoticeCommand : InGameCommand
    {
        public SendNoticeCommand()
        {
            Aliases = new[]
            {
                "notice"
            };
            Description = "Send a server message.";
            AddParameter<string[]>("message", "Message to send on the server.");
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
}
