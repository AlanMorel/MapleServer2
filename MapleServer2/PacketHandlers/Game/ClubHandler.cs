using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ClubHandler : GamePacketHandler<ClubHandler>
{
    public override RecvOp OpCode => RecvOp.Club;

    private enum Mode : byte
    {
        Create = 0x1,
        NewClubInvite = 0x3,
        SendInvite = 0x6,
        InviteResponse = 0x8,
        Leave = 0xA,
        Buff = 0xD,
        Rename = 0xE
    }

    private enum ClubErrorNotice
    {
        CannotInviteIntoClub = 0xC,
        OnlyLeaderCanPerformAction = 0x33,
        NotAClubMember = 0x34,
        LeaderCannotLeaveOwnClub = 0x35,
        FailedToDeliverInvite = 0x36,
        FailedToInviteMember = 0x37,
        ClubIsFull = 0x39,
        PlayerCannotJoinMoreClubs = 0x3A,
        ClubWithNameAlreadyExists = 0x3C,
        ContainsForbiddenWord = 0x3D,
        CannotDisbandIfMembersArePresent = 0x3E,
        AlreadyInClub = 0x3F,
        SomePartyMembersCannotBeInvited = 0x43,
        CanOnlyBeRenamedOnceAnHour = 0x48,
        NewNameIsCurrentName = 0x49,
        NoSpacesInName = 0x4A
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Create:
                HandleCreate(session, packet);
                break;
            case Mode.NewClubInvite:
                HandleNewClubInvite(session, packet);
                break;
            case Mode.SendInvite:
                HandleSendInvite(session, packet);
                break;
            case Mode.InviteResponse:
                HandleInviteResponse(session, packet);
                break;
            case Mode.Leave:
                HandleLeave(session, packet);
                break;
            case Mode.Buff:
                HandleBuff(packet);
                break;
            case Mode.Rename:
                HandleRename(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleCreate(GameSession session, PacketReader packet)
    {
        Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party is null || party.Leader.CharacterId != session.Player.CharacterId)
        {
            return;
        }

        int maxClubCount = int.Parse(ConstantsMetadataStorage.GetConstant("ClubMaxCount"));
        // Fail if a party member is offline or if member has joined max amount of clubs
        if (party.Members.Any(x => !x.Session.Connected()) || party.Members.Any(x => x.Clubs.Count >= maxClubCount))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.SomePartyMembersCannotBeInvited));
            return;
        }

        string clubName = packet.ReadUnicodeString();
        if (!ValidClubName(session, clubName))
        {
            return;
        }


        Club club = new(party, clubName);
        GameServer.ClubManager.AddClub(club);

        party.BroadcastPacketParty(ClubPacket.Create(party, club));
    }

    private static bool ValidClubName(GameSession session, string clubName)
    {
        if (DatabaseManager.Clubs.NameExists(clubName))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.ClubWithNameAlreadyExists));
            return false;
        }

        if (clubName.Contains(' '))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.NoSpacesInName));
            return false;
        }

        return true;
    }

    private static void HandleNewClubInvite(GameSession session, PacketReader packet)
    {
        if (session.Player.Clubs.Count > int.Parse(ConstantsMetadataStorage.GetConstant("ClubMaxCount")))
        {
            return;
        }

        long clubId = packet.ReadLong();

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club == null)
        {
            return;
        }

        ClubInviteResponse response = (ClubInviteResponse) packet.ReadInt();
        club.InvitedPlayerResponse(session.Player, response);
    }

    private static void HandleSendInvite(GameSession session, PacketReader packet)
    {
        long clubId = packet.ReadLong();
        string invitee = packet.ReadUnicodeString();

        Player other = GameServer.PlayerManager.GetPlayerByName(invitee);
        if (other == null)
        {
            return;
        }

        if (other.Clubs.Any(x => x.Id == clubId))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.AlreadyInClub));
            return;
        }

        if (other.Clubs.Count >= int.Parse(ConstantsMetadataStorage.GetConstant("ClubMaxCount")))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.PlayerCannotJoinMoreClubs));
            return;
        }

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club == null)
        {
            return;
        }

        if (club.Members.Count >= int.Parse(ConstantsMetadataStorage.GetConstant("ClubMaxMembers")))
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.ClubIsFull));
            return;
        }

        session.Send(ClubPacket.InviteSentReceipt(clubId, other));
        other.Session.Send(ClubPacket.Invite(club, other));
    }

    private static void HandleInviteResponse(GameSession session, PacketReader packet)
    {
        long clubId = packet.ReadLong();
        string clubName = packet.ReadUnicodeString();
        string clubLeader = packet.ReadUnicodeString();
        string invitee = packet.ReadUnicodeString();
        bool accept = packet.ReadBool();

        if (invitee != session.Player.Name)
        {
            return;
        }

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club == null)
        {
            return;
        }

        Player leader = GameServer.PlayerManager.GetPlayerById(club.LeaderCharacterId);

        leader?.Session.Send(ClubPacket.LeaderInviteResponse(club, invitee, accept));
        session.Send(ClubPacket.InviteResponse(club, session.Player));

        if (accept)
        {
            club.AcceptInvite(session.Player);
        }
    }

    private static void HandleLeave(GameSession session, PacketReader packet)
    {
        long clubId = packet.ReadLong();

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club is null)
        {
            return;
        }

        if (!session.Player.Clubs.Contains(club))
        {
            return;
        }

        if (club.Members.Count <= 2)
        {
            club.Disband();
            return;
        }

        if (session.Player.CharacterId == club.LeaderCharacterId)
        {
            club.AssignNewLeader(session.Player);
        }
        club.RemoveMember(session.Player);
        session.Player.UpdateSocials();
    }

    private static void HandleBuff(PacketReader packet)
    {
        long clubId = packet.ReadLong();
        int buffId = packet.ReadInt();

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club is null)
        {
            return;
        }

        // TODO add buff effect packet
        Player leader = GameServer.PlayerManager.GetPlayerById(club.LeaderCharacterId);
        leader?.Session.Send(ClubPacket.ChangeBuffReceipt(club, buffId));
        club.BroadcastPacketClub(ClubPacket.ChangeBuff(club, buffId));
    }

    private static void HandleRename(GameSession session, PacketReader packet)
    {
        long clubId = packet.ReadLong();
        string clubNewName = packet.ReadUnicodeString();

        Club club = GameServer.ClubManager.GetClubById(clubId);
        if (club is null)
        {
            return;
        }

        if (club.LastNameChangeTimestamp > TimeInfo.Now() + 3600) // 1 hour restriction to change names
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.CanOnlyBeRenamedOnceAnHour));
            return;
        }

        if (!ValidClubName(session, clubNewName))
        {
            return;
        }

        if (clubNewName == club.Name)
        {
            session.Send(ClubPacket.ErrorNotice((int) ClubErrorNotice.NewNameIsCurrentName));
            return;
        }

        club.SetName(clubNewName);
    }
}
