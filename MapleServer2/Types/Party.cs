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
        public long PartyFinderId { get; set; } //Show on party finder or not
        public long CreationTimestamp { get; set; }
        public string Name { get; set; }
        public bool Approval { get; set; } //Require approval before someone can join
        public Player Leader { get; set; }
        public int MaxMembers { get; set; }
        public int ReadyChecks { get; set; }
        public int RemainingMembers { get; set; } //# of members left to reply to ready check
        public int Dungeon { get; set; }

        //List of players and their session.
        public List<Player> Members { get; private set; }

        public Party(int pId, int pMaxMembers, List<Player> pPlayers)
        {
            Id = pId;
            MaxMembers = pMaxMembers;
            Leader = pPlayers.First();
            Members = pPlayers;
            ReadyChecks = 0;
            PartyFinderId = 0;
            Approval = true;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount; 
        }

        public Party(int pId, long pFinderId, int pMaxMembers, List<Player> pPlayers, string pName, bool pApproval) : this(pId, pMaxMembers, pPlayers)
        {
            Name = pName;
            Approval = pApproval;
            PartyFinderId = pFinderId;
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
            if (Members.Count < 2 && PartyFinderId == 0)
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

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
