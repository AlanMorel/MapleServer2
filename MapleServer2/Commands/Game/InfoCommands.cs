using System.Drawing;
using System.Text;
using Maple2Storage.Types.Metadata;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
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
        stringBuilder.Append("Online players:".Color(Color.DarkOrange).Bold() + "\n");
        stringBuilder.Append(string.Join(", ", players.Select(p => p.Name)));

        trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
    }
}

public class FindCommand : InGameCommand
{
    private readonly string[] Options =
    {
        "item", "map", "npc", "mob", "trophy", "quest"
    };

    public FindCommand()
    {
        Aliases = new()
        {
            "find"
        };
        Description = $"Find id by {string.Join("/", Options)} name.";
        Parameters = new()
        {
            new Parameter<string>("type", "The search type."),
            new Parameter<string[]>("name", "The search name.")
        };
        Usage = $"/find [{string.Join("/", Options)}] [name]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string type = trigger.Get<string>("type");
        string[] command = trigger.Get<string[]>("name");
        if (string.IsNullOrEmpty(type) || !Options.Contains(type) || command is null)
        {
            trigger.Session.SendNotice($"Usage: /find [{string.Join("/", Options)}] [name]");
            return;
        }

        string[] args = command[1..];
        if (args.Length == 0)
        {
            trigger.Session.SendNotice($"Usage: /find [{string.Join("/", Options)}] [name]");
            return;
        }

        string name = string.Join(" ", args);
        if (string.IsNullOrEmpty(name))
        {
            trigger.Session.SendNotice("No names provided.");
            return;
        }

        StringBuilder stringBuilder = new();
        switch (type)
        {
            case "item":
                List<ItemMetadata> itemMetadatas = ItemMetadataStorage.GetAll()
                    .Where(x => x.Name is not null && x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                switch (itemMetadatas.Count)
                {
                    case 0:
                        trigger.Session.SendNotice($"Item '{name}' not found.");
                        return;
                    case > 50:
                        trigger.Session.SendNotice($"Too many results for '{name}', found {itemMetadatas.Count}, limit it to 50.");
                        return;
                }

                stringBuilder.Append($"Found {itemMetadatas.Count} items with name '{name}':".Color(Color.DarkOrange).Bold() + "\n");
                foreach (ItemMetadata item in itemMetadatas)
                {
                    stringBuilder.Append($"{item.Name} - {item.Id}\n".Color(Color.Wheat));
                }

                break;
            case "map":
                List<MapMetadata> mapMetadatas = MapMetadataStorage.GetAll()
                    .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                switch (mapMetadatas.Count)
                {
                    case 0:
                        trigger.Session.SendNotice($"Map '{name}' not found.");
                        return;
                    case > 50:
                        trigger.Session.SendNotice($"Too many results for '{name}', found {mapMetadatas.Count}, limit it to 50.");
                        return;
                }

                stringBuilder.Append($"Found {mapMetadatas.Count} maps with name '{name}':".Color(Color.DarkOrange).Bold() + "\n");
                foreach (MapMetadata map in mapMetadatas)
                {
                    stringBuilder.Append($"{map.Name} - {map.Id}\n".Color(Color.Wheat));
                }

                break;
            case "mob":
            case "npc":
                List<NpcMetadata> npcMetadatas = NpcMetadataStorage.GetAll()
                    .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                switch (npcMetadatas.Count)
                {
                    case 0:
                        trigger.Session.SendNotice($"Npc '{name}' not found.");
                        return;
                    case > 50:
                        trigger.Session.SendNotice($"Too many results for '{name}', found {npcMetadatas.Count}, limit it to 50.");
                        return;
                }

                stringBuilder.Append($"Found {npcMetadatas.Count} npcs/mobs with name '{name}':".Color(Color.DarkOrange).Bold() + "\n");
                foreach (NpcMetadata npc in npcMetadatas)
                {
                    stringBuilder.Append($"{npc.Name} - {npc.Id}\n".Color(Color.Wheat));
                }

                break;
            case "trophy":
                List<TrophyMetadata> trophyMetadatas = TrophyMetadataStorage.GetAll()
                    .Where(trophy => trophy.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
                switch (trophyMetadatas.Count)
                {
                    case 0:
                        trigger.Session.SendNotice($"Trophy '{name}' not found.");
                        return;
                    case > 50:
                        trigger.Session.SendNotice($"Too many results for '{name}', found {trophyMetadatas.Count}, limit it to 50.");
                        return;
                }

                stringBuilder.Append($"Found {trophyMetadatas.Count} trophies with name '{name}':".Color(Color.DarkOrange).Bold() + "\n");
                foreach (TrophyMetadata trophy in trophyMetadatas)
                {
                    stringBuilder.Append($"{trophy.Name} - {trophy.Id}\n".Color(Color.Wheat));
                }

                break;
            case "quest":
                List<QuestMetadata> questMetadata = QuestMetadataStorage.GetAllQuests().Values
                    .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();

                switch (questMetadata.Count)
                {
                    case 0:
                        trigger.Session.SendNotice($"Quest '{name}' not found.");
                        return;
                    case > 50:
                        trigger.Session.SendNotice($"Too many results for '{name}', found {questMetadata.Count}, limit it to 50.");
                        return;
                }

                stringBuilder.Append($"Found {questMetadata.Count} quests with name '{name}':".Color(Color.DarkOrange).Bold() + "\n");
                foreach (QuestMetadata quest in questMetadata)
                {
                    stringBuilder.Append($"{quest.Name} - {quest.Basic.Id}\n".Color(Color.Wheat));
                }

                break;
        }

        trigger.Session.Send(NoticePacket.Notice(stringBuilder.ToString(), NoticeType.Chat));
    }
}
