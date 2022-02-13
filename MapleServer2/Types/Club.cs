using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Club
{
    public long Id;
    public string Name;
    public long LeaderAccountId;
    public long LeaderCharacterId;
    public string LeaderName;
    public List<ClubMember> Members;
    public long CreationTimestamp;
    public long LastNameChangeTimestamp;
    public bool IsEstablished;

    public Club() { }

    public Club(Party party, string name)
    {
        Name = name;
        LeaderAccountId = party.Leader.AccountId;
        LeaderCharacterId = party.Leader.CharacterId;
        LeaderName = party.Leader.Name;
        CreationTimestamp = TimeInfo.Now();
        LastNameChangeTimestamp = 0;
        IsEstablished = false;
        Members = new();
        Id = DatabaseManager.Clubs.Insert(this);

        foreach (Player partyMember in party.Members)
        {
            if (partyMember.Session.Connected())
            {
                AddMember(partyMember, out ClubMember clubMember);
            }
        }
    }

    public void InvitedPlayerResponse(Player player, ClubInviteResponse inviteResponse)
    {
        if (!Members.Any(x => x.Player.CharacterId == player.CharacterId))
        {
            return;
        }

        ClubMember member = player.ClubMembers.FirstOrDefault(x => x.ClubId == Id);
        member.InviteResponse = inviteResponse;

        if (inviteResponse == ClubInviteResponse.Reject)
        {
            List<GameSession> memberSessions = GetSessions();
            foreach (GameSession session in memberSessions)
            {
                session.Send(ClubPacket.ClubProposalInviteResponse(Id, inviteResponse, player.Name));
                session.Send(ClubPacket.DeleteUnestablishedClub(Id));
                ClubMember membership = session.Player.ClubMembers.First(x => x.ClubId == Id);
                session.Player.ClubMembers.Remove(membership);
                session.Player.Clubs.Remove(this);
            }

            GameServer.ClubManager.RemoveClub(this);
            return;
        }
        EstablishClub(player);
    }

    private void EstablishClub(Player player)
    {
        if (IsEstablished)
        {
            // player joins normally
            AcceptInvite(player);
            return;
        }

        if (Members.Any(x => x.InviteResponse == ClubInviteResponse.Pending))
        {
            return;
        }

        IsEstablished = true;
        DatabaseManager.Clubs.Update(this);

        foreach (ClubMember member in Members)
        {
            if (member.InviteResponse != ClubInviteResponse.Accept)
            {
                continue;
            }

            if (member.Player.CharacterId == LeaderCharacterId)
            {
                member?.Player.Session?.Send(ClubPacket.Join(member.Player.Name, this));
                member?.Player.Session?.Send(ClubPacket.Establish(this));
            }
            member?.Player.Session?.Send(ClubPacket.ClubProposalInviteResponse(Id, ClubInviteResponse.Accept, member.Player.Name));
        }
    }

    public void Disband()
    {
        BroadcastPacketClub(ClubPacket.Disband(this));
        List<GameSession> memberSessions = GetSessions();
        foreach (GameSession session in memberSessions)
        {
            session.Player.Clubs.Remove(this);
            ClubMember membership = session.Player.ClubMembers.FirstOrDefault(x => x.ClubId == Id);
            session.Player.ClubMembers.Remove(membership);
            DatabaseManager.ClubMembers.Delete(Id, session.Player.CharacterId);
        }
        GameServer.ClubManager.RemoveClub(this);
    }

    public void AddMember(Player player, out ClubMember member)
    {
        member = new ClubMember(player, Id);
        player.Clubs.Add(this);
        player.ClubMembers.Add(member);
        Members.Add(member);
        player.UpdateSocials();
    }

    public void AcceptInvite(Player player)
    {
        AddMember(player, out ClubMember member);
        BroadcastPacketClub(ClubPacket.ConfirmInvite(this, member));
        player.Session.Send(ClubPacket.Join(player.Name, this));
        player.Session.Send(ClubPacket.UpdateClub(this));
        BroadcastPacketClub(ClubPacket.UpdatePlayer(member, this));
    }

    public void SetName(string newName)
    {
        Name = newName;
        LastNameChangeTimestamp = TimeInfo.Now();
        DatabaseManager.Clubs.Update(this);
        BroadcastPacketClub(ClubPacket.Rename(this));
    }

    public void RemoveMember(Player player)
    {
        player.Session.Send(ClubPacket.LeaveClub(this));
        player.Clubs.Remove(this);
        BroadcastPacketClub(ClubPacket.LeaveNotice(this, player));
        ClubMember member = player.ClubMembers.First(x => x.ClubId == Id);
        Members.Remove(member);
        player.ClubMembers.Remove(member);
        DatabaseManager.ClubMembers.Delete(Id, player.CharacterId);
        DatabaseManager.Clubs.Update(this);

    }

    public void AssignNewLeader(Player oldLeader)
    {
        if (Members.Count <= 2)
        {
            Disband();
            return;
        }

        Player player = Members.OrderBy(x => x.JoinTimestamp)
            .First(x => x.Player != oldLeader).Player;

        LeaderAccountId = player.AccountId;
        LeaderCharacterId = player.CharacterId;
        LeaderName = player.Name;
        BroadcastPacketClub(ClubPacket.AssignNewLeader(oldLeader, this));
        DatabaseManager.Clubs.Update(this);
    }

    public void BroadcastPacketClub(PacketWriter packet, GameSession sender = null)
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
        List<GameSession> sessions = new();
        foreach (ClubMember clubMember in Members)
        {
            Player player = GameServer.PlayerManager.GetPlayerById(clubMember.Player.CharacterId);
            if (player == null)
            {
                continue;
            }
            sessions.Add(player.Session);
        }

        return sessions;
    }
}

public enum ClubInviteResponse
{
    Accept = 0,
    Reject = 76,
    Pending = 100
}
