using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Club
    {
        public long Id { get; }
        public string Name { get; set; }
        public bool Approval { get; set; } // require approval before someone can join
        public Player Leader { get; set; }
        public int MaxMembers { get; set; }
        public List<Player> Members { get; }

        public Club(Party party, string name)
        {
            Id = GuidGenerator.Long();
            Name = name;
            Approval = false;
            Leader = party.Leader;
            MaxMembers = 10;
            Members = new List<Player> { Leader };
            Leader.ClubId = Id;
        }

        public void BroadcastPacketClub(Packet packet, GameSession sender = null)
        {
            BroadcastClub(session =>
            {
                if (session == sender)
                {
                    return;
                }

                session.Send(packet);
            });
        }

        public void BroadcastClub(Action<GameSession> action)
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
