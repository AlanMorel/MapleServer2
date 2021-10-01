using Maple2Storage.Types.Metadata;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game
{
    public class GuildExpCommand : InGameCommand
    {
        public GuildExpCommand()
        {
            Aliases = new()
            {
                "setguildexp"
            };
            Description = "Set the experience of the current player guild.";
            Parameters = new()
            {
                new Parameter<int>("exp", "Amount of experience.")
            };
            Usage = "/setguildexp [exp]";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(trigger.Session.Player.Guild.Id);

            if (guild == null)
            {
                trigger.Session.SendNotice("Player is not in a guild to add exp. Make sure you join one.");
                return;
            }
            int guildExp = trigger.Get<int>("exp");

            if (guildExp <= 0)
            {
                trigger.Session.SendNotice("Amount must be more than 0 to add.");
                return;
            }
            guild.Exp = guildExp;
            guild.BroadcastPacketGuild(GuildPacket.UpdateGuildExp(guild.Exp));
            GuildPropertyMetadata data = GuildPropertyMetadataStorage.GetMetadata(guild.Exp);
            DatabaseManager.Guilds.Update(guild);
        }
    }

    public class GuildFundCommand : InGameCommand
    {
        public GuildFundCommand()
        {
            Aliases = new()
            {
                "setguildfund"
            };
            Description = "Set the Funds of the current player guild.";
            Parameters = new()
            {
                new Parameter<int>("amount", "Amount of Funds.")
            };
            Usage = "/setguildfund [amount]";
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(trigger.Session.Player.Guild.Id);

            if (guild == null)
            {
                trigger.Session.SendNotice("Player is not in a guild to add funds. Make sure you join one.");
                return;
            }
            int guildFunds = trigger.Get<int>("amount");

            if (guildFunds <= 0)
            {
                trigger.Session.SendNotice("Amount must be more than 0 to add.");
                return;
            }
            guild.Funds = guildFunds;
            guild.BroadcastPacketGuild(GuildPacket.UpdateGuildFunds(guild.Funds));
            DatabaseManager.Guilds.Update(guild);
        }
    }
}
