using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Party
    {
        public int Id { get; }
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
        public List<Player> Members { get; }

        public Party(int pMaxMembers, List<Player> pPlayers)
        {
            Id = GuidGenerator.Int();
            MaxMembers = pMaxMembers;
            Leader = pPlayers.First();
            Members = pPlayers;
            ReadyChecks = 0;
            PartyFinderId = 0;
            Approval = true;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;

            Members.ForEach(member => member.PartyId = Id);
        }

        public Party(int pMaxMembers, List<Player> pPlayers, string pName, bool pApproval) : this(pMaxMembers, pPlayers)
        {
            Name = pName;
            Approval = pApproval;
            PartyFinderId = GuidGenerator.Long();
        }

        public void AddMember(Player player)
        {
            Members.Add(player);
        }

        public void RemoveMember(Player player)
        {
            Members.Remove(player);
            player.PartyId = 0;

            if (Leader.CharacterId == player.CharacterId)
            {
                FindNewLeader();
            }

            CheckDisband();
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
            if (Members.Count >= 2 || PartyFinderId != 0)
            {
                return;
            }

            BroadcastParty(session =>
            {
                session.Player.PartyId = 0;
                session.Send(PartyPacket.Disband());
            });
            GameServer.PartyManager.RemoveParty(this);
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
            return Members.Where(member => member.Session.Connected()).Select(member => member.Session).ToList();
        }
    }
}