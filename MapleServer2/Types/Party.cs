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
        public int RecruitMemberCount { get; set; }
        public List<Player> ReadyCheck { get; set; }
        public int RemainingMembers { get; set; } //# of members left to reply to ready check
        public int DungeonSessionId = -1;

        //List of players and their session.
        public List<Player> Members { get; }

        public Party(Player partyLeader)
        {
            Id = GuidGenerator.Int();
            Leader = partyLeader;
            ReadyCheck = new List<Player>() { };
            Members = new List<Player>() { };
            PartyFinderId = 0;
            Approval = true;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;

            AddMember(partyLeader);
        }

        public Party(string pName, bool pApproval, Player player, int recruitMemberCount)
        {
            Id = GuidGenerator.Int();
            Name = pName;
            ReadyCheck = new List<Player>() { };
            Members = new List<Player>() { };
            Approval = pApproval;
            PartyFinderId = GuidGenerator.Long();
            Leader = player;
            RecruitMemberCount = recruitMemberCount;
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;

            AddMember(player);
        }

        public void AddMember(Player player)
        {
            Members.Add(player);
            player.Party = this;
        }

        public void RemoveMember(Player player)
        {
            Members.Remove(player);
            player.Party = null;

            if (Leader.CharacterId == player.CharacterId && Members.Count > 2)
            {
                FindNewLeader();
            }

            CheckDisband();
        }

        public void FindNewLeader()
        {
            Player newLeader = Members.First(x => x.CharacterId != Leader.CharacterId);
            BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
            Leader = newLeader;
        }

        public void CheckDisband()
        {
            if (Members.Count >= 2)
            {
                return;
            }

            BroadcastParty(session =>
            {
                session.Player.Party = null;
                session.Send(PartyPacket.Disband());
            });
            GameServer.PartyManager.RemoveParty(this);
        }

        public void CheckOffineParty(Player player)
        {
            List<GameSession> sessions = GetSessions();
            if (sessions.Count == 0)
            {
                GameServer.PartyManager.RemoveParty(this);
                return;
            }
            BroadcastPacketParty(PartyPacket.LogoutNotice(player.CharacterId));
            if (Leader == player)
            {
                FindNewLeader();
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

        public List<GameSession> GetSessions()
        {
            List<GameSession> sessions = new List<GameSession>();
            foreach (Player member in Members)
            {
                GameSession playerSession = GameServer.Storage.GetPlayerById(member.CharacterId)?.Session;
                if (playerSession != null)
                {
                    sessions.Add(playerSession);
                }
            }
            return sessions;
        }

        public Task StartReadyCheck()
        {
            ReadyCheck = new List<Player>() { Leader };
            BroadcastPacketParty(PartyPacket.StartReadyCheck(Leader, Members, ReadyCheck.Count));
            return Task.Run(async () =>
            {
                await Task.Delay(20000);
                if (Members.Count == ReadyCheck.Count || ReadyCheck.Count == 0) // Cancel this. Ready check was successfully responded by each player
                {
                    return;
                }

                foreach (Player member in Members)
                {
                    if (!ReadyCheck.Contains(member))
                    {
                        BroadcastPacketParty(PartyPacket.ReadyCheck(member, 0)); // Force player who did not respond to respond with 'not ready'
                    }
                }

                BroadcastPacketParty(PartyPacket.EndReadyCheck());
                ReadyCheck.Clear();
            });
        }
    }
}
