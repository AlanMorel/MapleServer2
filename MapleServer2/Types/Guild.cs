using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Guild
    {
        // TODO: Add ranks, rank names, Member contribution
        public long Id { get; }
        public string Name { get; set; }
        public bool Approval { get; set; } //Require approval before someone can join
        public long CreationTimestamp { get; set; }
        public Player Leader { get; set; }
        public int MaxMembers { get; set; }
        public List<Player> Members { get; }
        public int Funds { get; set; }
        public int Exp { get; set; }
        public bool Searchable { get; set; }

        public Guild(string name, List<Player> gPlayers)
        {
            Id = GuidGenerator.Long();
            Name = name;
            MaxMembers = 60;
            Leader = gPlayers.First();
            Members = gPlayers;
            Approval = false;
            Exp = 0;
            Funds = 0;
            Searchable = true;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;
        }

        public void AddMember(Player player)
        {
            Members.Add(player);
        }

        public void RemoveMember(Player player)
        {
            Members.Remove(player);
            player.GuildId = 0;
        }


        public void BroadcastPacketGuild(Packet packet, GameSession sender = null)
        {
            BroadcastGuild(session =>
            {
                if (session == sender)
                {
                    return;
                }

                session.Send(packet);
            });
        }

        public void BroadcastGuild(Action<GameSession> action)
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
