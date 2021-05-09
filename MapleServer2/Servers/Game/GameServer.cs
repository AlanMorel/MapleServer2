using System;
using System.Collections.Generic;
using Autofac;
using MapleServer2.Config;
using MapleServer2.Database;
using MapleServer2.Network;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Game
{
    public class GameServer : Server<GameSession>
    {
        public static readonly PlayerStorage Storage = new();
        public static readonly PartyManager PartyManager = new();
        public static readonly ClubManager ClubManager = new();
        public static readonly GuildManager GuildManager = new();
        public static readonly GroupChatManager GroupChatManager = new();
        public static readonly HongBaoManager HongBaoManager = new();
        public static readonly BuddyManager BuddyManager = new();

        public GameServer(PacketRouter<GameSession> router, ILogger<GameServer> logger, IComponentContext context) :
            base(router, logger, context)
        { }

        public void Start()
        {
            List<Guild> guilds = DatabaseManager.GetGuilds();
            foreach (Guild guild in guilds)
            {
                GuildManager.AddGuild(guild);
            }
            Start(Convert.ToUInt16(ConfigHandler.Data["Server"]["GamePort"]));
        }
    }
}
