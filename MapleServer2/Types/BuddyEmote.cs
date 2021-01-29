using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class BuddyEmote
    {
        public List<Player> Buddies { get; }

        public BuddyEmote(List<Player> buddyPlayers)
        {
            Buddies = buddyPlayers;
        }

        public void AddBuddy(Player player)
        {
            Buddies.Add(player);
        }

        public void RemoveMember(Player player)
        {
            Buddies.Remove(player);
        }
    }
}
