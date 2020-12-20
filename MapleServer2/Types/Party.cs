using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using System;
using System.Collections.Generic;

namespace MapleServer2.Types
{
    public class Party
    {

        public int Uid { get; private set; }
        public int MaxMembers { get; set; }
        public Player Leader { get; set; }

        //List of players and their session.
        public HashSet<Player> Players { get; private set; }

        public Party(int pUid, int pMaxMembers, Player pLeader, HashSet<Player> pPlayers)
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

        public void BroadcastPacketParty(Packet packet, GameSession sender = null)
        {
            BroadcastParty(session =>
            {
                if (session == sender)
                {
                    return;
                }
                session.Send(packet);
            });
        }

        public void BroadcastParty(Action<GameSession> action)
        {
            IEnumerable<GameSession> sessions = GetSessions();
            lock (sessions)
            {
                foreach (GameSession session in sessions)
                {
                    action?.Invoke(session);
                }
            }
        }

        private List<GameSession> GetSessions()
        {
            List<GameSession> sessions = new List<GameSession>();

            foreach (Player partyMember in Players)
            {
                sessions.Add(partyMember.Session);
            }
            return sessions;
        }
    }
}
