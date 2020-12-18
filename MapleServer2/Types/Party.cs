using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace MapleServer2.Types {
    public class Party {

        public int Uid { get; private set; }
        public int MaxMembers { get; set; }
        public long Leader { get; private set; }
        public HashSet<Player> Players { get; private set; }
        public long CreatedTimestamp { get; private set; }

        public Party(int pUid, int pMaxMembers, long pLeader, HashSet<Player> pPlayers, long pCreateTimestamp)
        {
            Uid = pUid;
            MaxMembers = pMaxMembers;
            Leader = pLeader;
            Players = pPlayers;
            CreatedTimestamp = pCreateTimestamp;
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
