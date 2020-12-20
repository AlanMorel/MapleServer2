using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MapleServer2.Types
{
    public class Party
    {
        public int Id { get; private set; }
        public int MaxMembers { get; set; }
        public Player Leader { get; set; }

        //List of players and their session.
        public List<Player> Members { get; private set; }

        public Party(int pUid, int pMaxMembers, List<Player> pPlayers)
        {
            Id = pUid;
            MaxMembers = pMaxMembers;
            Leader = pPlayers.First();
            Members = pPlayers;
        }

        public void AddMember(Player p)
        {
            Members.Add(p);
        }

        public void RemoveMember(Player p)
        {
            Members.Remove(p);
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

            foreach (Player partyMember in Members)
            {
                sessions.Add(partyMember.Session);
            }
            return sessions;
        }
    }
}
