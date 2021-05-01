﻿using System.Collections.Generic;
using System.Linq;
using Autofac;
using MapleServer2.Database;
using MapleServer2.Network;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Game
{
    public class GameServer : Server<GameSession>
    {
        public const int PORT = 21001;
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
            List<Guild> guilds = new List<Guild>();
            using (DatabaseContext context = new DatabaseContext())
            {
                guilds = context.Guilds
                .Include(p => p.Members).ThenInclude(p => p.Player).ThenInclude(p => p.Levels)
                .Include(p => p.Leader).ToList();
            }
            foreach (Guild guild in guilds)
            {
                GuildManager.AddGuild(guild);
            }
            Start(PORT);
        }
    }
}
