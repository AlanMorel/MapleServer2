using System.Collections.Generic;
using System.Text;
using System.Linq;
using MapleServer2.Commands.Core;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Enums;

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
                    stringBuilder.Append($"<font color='#d68a11'>/{alias}</font>");
                }

                stringBuilder.Append($"<font color='#ffefad'> {command.Description}</font>\n");
                foreach (IParameter param in command.Parameters)
                {
                    stringBuilder.Append($"<font color='#93f5eb'> - {param.Name}</font> <font color='#ffefad'>{param.Description}</font>\n");
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

            if (args.Length <= 1)
            {
                trigger.Session.SendNotice("No message provided.");
                return;
            }
            string message = CommandHelpers.BuildString(args);
            MapleServer.BroadcastPacketAll(NoticePacket.Notice(message));
        }
    }
}
