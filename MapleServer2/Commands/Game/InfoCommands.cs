﻿using System.Drawing;
using System.Text;
using MapleServer2.Commands.Core;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class InfoCommands : InGameCommand
    {
        public InfoCommands()
        {
            Aliases = new()
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
                    stringBuilder.Append(CommandHelpers.Color(CommandHelpers.Bold(alias), Color.DarkOrange) + " ");
                }

                stringBuilder.Append($"{CommandHelpers.Color(command.Description, Color.Wheat)}\n");
                foreach (IParameter param in command.Parameters)
                {
                    stringBuilder.Append($"    -{CommandHelpers.Color(param.Name, Color.Aquamarine)} {CommandHelpers.Color(param.Description, Color.Wheat)}");
                    if (command.Parameters.IndexOf(param) != command.Parameters.Count - 1 || command != commandList.Last())
                    {
                        stringBuilder.Append('\n');
                    }
                }
            }
            trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
        }
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

    public class OnlineCommand : InGameCommand
    {
        public OnlineCommand()
        {
            Aliases = new() { "online" };
            Description = "See all online players.";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            List<Player> players = GameServer.Storage.GetAllPlayers();
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"{CommandHelpers.Color(CommandHelpers.Bold("Online players:"), Color.DarkOrange)}\n");
            stringBuilder.Append(string.Join(", ", players.Select(p => p.Name)));

            trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
        }
    }
}
