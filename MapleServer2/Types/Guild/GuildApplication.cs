﻿using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuildApplication
    {
        public long Id { get; }
        public long GuildId { get; set; }
        public long CharacterId { get; set; }
        public long CreationTimestamp { get; }

        public GuildApplication(long player, long guild)
        {
            Id = GuidGenerator.Long();
            CharacterId = player;
            GuildId = guild;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
        }

        public void Add(Player player, Guild guild)
        {
            player.GuildApplications.Add(this);
            guild.Applications.Add(this);
        }

        public void Remove(Player player, Guild guild)
        {
            player.GuildApplications.Remove(this);
            guild.Applications.Remove(this);
        }
    }
}