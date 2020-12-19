using MapleServer2.Servers.Game;
using System.Collections.Generic;

namespace MapleServer2.Types {
    public class Party {

        public int Uid { get; private set; }
        public int MaxMembers { get; set; }
        public long Leader { get; set; }

        //List of players and their session.
        public HashSet<Player> Players { get; private set; }

        public Party(int pUid, int pMaxMembers, long pLeader, HashSet<Player> pPlayers)
        {
            Uid = pUid;
            MaxMembers = pMaxMembers;
            Leader = pLeader;
            Players = pPlayers;
        }

        public void AddPlayer(Player p)
        {
            Players.Add(p);
        }

        public void RemovePlayer(Player p)
        {
            Players.Remove(p);
        }
    }
}
