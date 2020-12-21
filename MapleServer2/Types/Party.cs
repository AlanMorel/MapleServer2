using MaplePacketLib2.Tools;
using MapleServer2.Packets;
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
        public int ReadyChecks { get; set; }
        public int RemainingMembers { get; set; } //# of members left to reply to ready check

        //List of players and their session.
        public List<Player> Members { get; private set; }

        public Party(int pId, int pMaxMembers, List<Player> pPlayers)
        {
            Id = pId;
            MaxMembers = pMaxMembers;
            Leader = pPlayers.First();
            Members = pPlayers;
            ReadyChecks = 0;
        }

        public void AddMember(Player player)
        {
            Members.Add(player);
        }

        public void RemoveMember(Player player)
        {
            Members.Remove(player);
        }

        public void FindNewLeader()
        {
            Player newLeader = Members.First();
            BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
            Leader = newLeader;
            Members.Remove(newLeader);
            Members.Insert(0, newLeader);
        }

        public void CheckDisband()
        {
            if (Members.Count < 2)
            {
                BroadcastParty(session =>
                {
                    session.Player.PartyId = 0;
                    session.Send(PartyPacket.Disband());
                });
                GameServer.PartyManager.RemoveParty(this);
            }
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
                if (partyMember.Session.Connected())
                {
                    sessions.Add(partyMember.Session);
                }
            }
            return sessions;
        }
    }
}
