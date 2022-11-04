using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Guild
{
    public long Id { get; set; }
    public string Name { get; set; }
    public long CreationTimestamp { get; set; }
    public long LeaderAccountId { get; set; }
    public long LeaderCharacterId { get; set; }
    public string LeaderName { get; set; }
    public byte Capacity { get; set; }
    public List<GuildMember> Members = new();
    public GuildRank[] Ranks;
    public List<GuildBuff> Buffs = new();
    public List<GuildService> Services = new();
    public List<Item> GiftBank = new();
    public List<GuildApplication> Applications = new();
    public int Funds { get; set; }
    public int Exp { get; set; }
    public bool Searchable { get; set; }
    public string Notice;
    public string Emblem;
    public List<UGC> Banners;
    public int FocusAttributes;
    public int HouseRank;
    public int HouseTheme;

    public Guild() { }

    public Guild(string name, Player leader)
    {
        GuildPropertyMetadata property = GuildPropertyMetadataStorage.GetMetadata(0);
        Name = name;
        LeaderAccountId = leader.AccountId;
        LeaderCharacterId = leader.CharacterId;
        LeaderName = leader.Name;
        Capacity = (byte) property.Capacity;
        Exp = 0;
        Funds = 0;
        Emblem = "";
        Notice = "";
        Banners = new();
        Searchable = true;
        HouseRank = 1;
        HouseTheme = 1;
        Ranks = new GuildRank[6]
        {
            new("Master", 4095), new("Jr. Master"), new("Member 1"), new("Member 2"), new("New Member 1"), new("New Member 2")
        };
        CreationTimestamp = TimeInfo.Now();

        List<int> buffIds = GuildBuffMetadataStorage.GetBuffList();
        foreach (int buffId in buffIds)
        {
            Buffs.Add(new(buffId));
        }
        Id = DatabaseManager.Guilds.Insert(this);

        GuildMember guildMemberLeader = new(leader, 0, Id);
        Members.Add(guildMemberLeader);

        leader.Guild = this;
        leader.GuildMember = guildMemberLeader;
        DatabaseManager.Characters.Update(leader);
    }

    public void AddMember(Player player)
    {
        GuildMember member = new(player, 5, Id);
        Members.Add(member);

        player.Guild = this;
        player.GuildMember = member;

        DatabaseManager.GuildMembers.Update(member);
        DatabaseManager.Guilds.Update(this);
        DatabaseManager.Characters.Update(player);
    }

    public void RemoveMember(Player player)
    {
        GuildMember member = Members.First(x => x.Player.CharacterId == player.CharacterId);
        Members.Remove(member);
        player.Guild = null;
        player.GuildMember = null;

        DatabaseManager.Guilds.Update(this);
        DatabaseManager.Characters.Update(player);
        DatabaseManager.GuildMembers.Delete(member.Id);
    }

    public void AssignNewLeader(Player oldLeader, Player newLeader)
    {
        GuildMember newLeadMember = Members.First(x => x.Player.CharacterId == newLeader.CharacterId);
        GuildMember oldLeadMember = Members.First(x => x.Player.CharacterId == oldLeader.CharacterId);

        Members.Remove(newLeadMember);
        Members.Remove(oldLeadMember);
        Members.Insert(0, newLeadMember);
        Members.Add(oldLeadMember);

        DatabaseManager.Guilds.Update(this);
        DatabaseManager.GuildMembers.Update(newLeadMember);
        DatabaseManager.GuildMembers.Update(oldLeadMember);
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
        DatabaseManager.Guilds.Update(this);
    }

    public void AddExp(GameSession session, int expGain)
    {
        Exp += expGain;
        BroadcastPacketGuild(GuildPacket.UpdateGuildExp(Exp));
        session.Send(GuildPacket.UpdateGuildStatsNotice(expGain, 0));
        DatabaseManager.Guilds.Update(this);
    }

    public void BroadcastPacketGuild(PacketWriter packet, GameSession sender = null)
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

    private IEnumerable<GameSession> GetSessions()
    {
        List<GameSession> sessions = new();
        foreach (GuildMember guildMember in Members)
        {
            Player player = GameServer.PlayerManager.GetPlayerById(guildMember.Player.CharacterId);
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
