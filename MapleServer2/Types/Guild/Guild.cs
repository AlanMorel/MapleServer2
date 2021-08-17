using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class Guild
    {
        public long Id { get; }
        public string Name { get; set; }
        public long CreationTimestamp { get; set; }
        public Player Leader { get; set; }
        public byte Capacity { get; set; }
        public List<GuildMember> Members = new List<GuildMember>();
        public GuildRank[] Ranks;
        public List<GuildBuff> Buffs = new List<GuildBuff>();
        public List<GuildService> Services = new List<GuildService>();
        public List<Item> GiftBank = new List<Item>();
        public List<GuildApplication> Applications = new List<GuildApplication>();
        public int Funds { get; set; }
        public int Exp { get; set; }
        public bool Searchable { get; set; }
        public string Notice;
        public string Emblem;
        public int FocusAttributes;
        public int HouseRank;
        public int HouseTheme;

        public Guild() { }

        public Guild(string name, Player leader)
        {
            GuildPropertyMetadata property = GuildPropertyMetadataStorage.GetMetadata(0);
            GuildMember guildMemberLeader = new GuildMember(leader, 0);

            Name = name;
            Leader = leader;
            Members.Add(guildMemberLeader);
            Capacity = (byte) property.Capacity;
            Exp = 0;
            Funds = 0;
            Emblem = "";
            Notice = "";
            Searchable = true;
            HouseRank = 1;
            HouseTheme = 1;
            Ranks = new GuildRank[6] {
                new GuildRank("Master", 4095),
                new GuildRank("Jr. Master"),
                new GuildRank("Member 1"),
                new GuildRank("Member 2"),
                new GuildRank("New Member 1"),
                new GuildRank("New Member 2") };
            CreationTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount;

            List<int> buffIds = GuildBuffMetadataStorage.GetBuffList();
            foreach (int buffId in buffIds)
            {
                Buffs.Add(new GuildBuff(buffId));
            }
            Id = DatabaseManager.CreateGuild(this);
            Leader.Guild = this;
            DatabaseManager.UpdateCharacter(leader);
        }

        public void AddMember(Player player)
        {
            GuildMember member = new GuildMember(player, 5);
            Members.Add(member);

            player.Guild = this;
            player.GuildMember = member;

            DatabaseManager.Update(member);
            DatabaseManager.UpdateGuild(this);
            DatabaseManager.UpdateCharacter(player);
        }

        public void RemoveMember(Player player)
        {
            GuildMember member = Members.First(x => x.Player.CharacterId == player.CharacterId);
            Members.Remove(member);
            player.Guild = null;
            player.GuildMember = null;

            DatabaseManager.UpdateGuild(this);
            DatabaseManager.UpdateCharacter(player);
            DatabaseManager.Delete(member);
        }

        public void AssignNewLeader(Player oldLeader, Player newLeader)
        {
            GuildMember newLeadMember = Members.First(x => x.Player.CharacterId == newLeader.CharacterId);
            GuildMember oldLeadMember = Members.First(x => x.Player.CharacterId == oldLeader.CharacterId);

            Members.Remove(newLeadMember);
            Members.Remove(oldLeadMember);
            Members.Insert(0, newLeadMember);
            Members.Add(oldLeadMember);

            DatabaseManager.UpdateGuild(this);
            DatabaseManager.Update(newLeadMember);
            DatabaseManager.Update(oldLeadMember);
        }

        public void ModifyFunds(GameSession session, GuildPropertyMetadata property, int funds)
        {
            if (funds > 0)
            {
                if (Funds >= property.FundMax)
                {
                    return;
                }
                Funds += funds;

                Funds = Math.Min(property.FundMax, funds);
            }
            else
            {
                Funds += funds;
            }

            BroadcastPacketGuild(GuildPacket.UpdateGuildFunds(Funds));
            session.Send(GuildPacket.UpdateGuildStatsNotice(0, funds));
            DatabaseManager.UpdateGuild(this);
        }

        public void AddExp(GameSession session, int expGain)
        {
            Exp += expGain;
            BroadcastPacketGuild(GuildPacket.UpdateGuildExp(Exp));
            session.Send(GuildPacket.UpdateGuildStatsNotice(expGain, 0));
            DatabaseManager.UpdateGuild(this);
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
            List<GameSession> sessions = new List<GameSession>();
            foreach (GuildMember guildMember in Members)
            {
                Player player = GameServer.Storage.GetPlayerById(guildMember.Player.CharacterId);
                if (player == null)
                {
                    continue;
                }
                sessions.Add(player.Session);
            }

            return sessions;
        }
    }

    [Flags]
    public enum GuildFocus
    {
        Social = 1,
        HuntingParties = 2,
        TrophyCollection = 4,
        Dungeons = 8,
        HomeDesign = 16,
        PvP = 32,
        WorkshopTemplates = 64,
        GuildArcade = 128,
        Weekdays = 256,
        Mornings = 512,
        Weekends = 1024,
        Evenings = 2048,
        Teens = 4096,
        Thirties = 8192,
        Twenties = 16384,
        Other = 32768
    }
}
