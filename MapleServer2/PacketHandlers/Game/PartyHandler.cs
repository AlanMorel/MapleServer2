using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PartyHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.PARTY;
    public PartyHandler() : base() { }

    private enum PartyMode : byte
    {
        Invite = 0x1,
        Join = 0x2,
        Leave = 0x3,
        Kick = 0x4,
        SetLeader = 0x11,
        FinderJoin = 0x17,
        SummonParty = 0x1D,
        VoteKick = 0x2D,
        ReadyCheck = 0x2E,
        FindDungeonParty = 0x21,
        CancelFindDungeonParty = 0x22,
        ReadyCheckUpdate = 0x30
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        PartyMode mode = (PartyMode) packet.ReadByte(); //Mode

        switch (mode)
        {
            case PartyMode.Invite:
                HandleInvite(session, packet);
                break;
            case PartyMode.Join:
                HandleJoin(session, packet);
                break;
            case PartyMode.Leave:
                HandleLeave(session);
                break;
            case PartyMode.Kick:
                HandleKick(session, packet);
                break;
            case PartyMode.SetLeader:
                HandleSetLeader(session, packet);
                break;
            case PartyMode.FinderJoin:
                HandleFinderJoin(session, packet);
                break;
            case PartyMode.SummonParty:
                HandleSummonParty();
                break;
            case PartyMode.VoteKick:
                HandleVoteKick(session, packet);
                break;
            case PartyMode.ReadyCheck:
                HandleStartReadyCheck(session);
                break;
            case PartyMode.FindDungeonParty:
                HandleFindDungeonParty(session, packet);
                break;
            case PartyMode.ReadyCheckUpdate:
                HandleReadyCheckUpdate(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleInvite(GameSession session, PacketReader packet)
    {
        string target = packet.ReadUnicodeString();

        Player other = GameServer.PlayerManager.GetPlayerByName(target);
        if (other == null)
        {
            return;
        }

        if (session.Player.Party != null)
        {
            Party party = session.Player.Party;

            if (party.Leader != session.Player)
            {
                session.Send(PartyPacket.Notice(session.Player, PartyNotice.NotLeader));
                return;
            }

            if (other == session.Player)
            {
                session.Send(PartyPacket.Notice(session.Player, PartyNotice.InviteSelf));
                return;
            }

            if (other.Party != null)
            {
                Party otherParty = other.Party;

                if (otherParty.Members.Count > 1)
                {
                    session.Send(PartyPacket.Notice(session.Player, PartyNotice.UnableToInvite));
                    return;
                }
            }

            other.Session.Send(PartyPacket.SendInvite(session.Player, party));
        }
        else
        {
            if (other.Party != null)
            {
                Party otherParty = other.Party;

                if (otherParty.Members.Count == 1)
                {
                    Party newParty = new(session.Player);
                    GameServer.PartyManager.AddParty(newParty);

                    session.Send(PartyPacket.Create(newParty, true));
                    other.Session.Send(PartyPacket.SendInvite(session.Player, newParty));
                    return;
                }

                session.Send(PartyPacket.Notice(other, PartyNotice.RequestToJoin));
                otherParty.Leader.Session.Send(PartyPacket.JoinRequest(session.Player));
                return;
            }
            else
            {
                // create party
                Party newParty = new(session.Player);
                GameServer.PartyManager.AddParty(newParty);
                session.Send(PartyPacket.Create(newParty, true));
                other.Session.Send(PartyPacket.SendInvite(session.Player, newParty));
            }
        }
    }

    private static void HandleJoin(GameSession session, PacketReader packet)
    {
        string target = packet.ReadUnicodeString();
        PartyNotice response = (PartyNotice) packet.ReadByte();
        int partyId = packet.ReadInt();

        JoinParty(session, response, partyId);
    }

    private static void JoinParty(GameSession session, PartyNotice response, int partyId)
    {
        Party party = GameServer.PartyManager.GetPartyById(partyId);
        if (party == null)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.PartyNotFound));
            return;
        }

        if (party.Members.Contains(session.Player) || party.Leader == session.Player)
        {
            return;
        }

        if (response != PartyNotice.AcceptedInvite)
        {
            party.Leader.Session.Send(PartyPacket.Notice(session.Player, response));
            return;
        }

        if (party.Members.Count >= 10)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.FullParty));
            return;
        }

        if (session.Player.Party != null)
        {
            Party currentParty = session.Player.Party;
            if (currentParty.Members.Count == 1)
            {
                currentParty.RemoveMember(session.Player);
            }
        }

        if (party.Members.Count == 1)
        {
            //establish party.
            party.BroadcastPacketParty(PartyPacket.Join(session.Player));
            party.AddMember(session.Player);
            session.Send(PartyPacket.Create(party, true));
            party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(party.Leader));
            party.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));
            return;
        }

        party.BroadcastPacketParty(PartyPacket.Join(session.Player));
        party.AddMember(session.Player);
        session.Send(PartyPacket.Create(party, true));
        party.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));

        foreach (Player member in party.Members)
        {
            if (member != session.Player)
            {
                party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(member));
            }
        }
    }

    private static void HandleLeave(GameSession session)
    {
        Party party = session.Player.Party;

        session.Send(PartyPacket.Leave(session.Player, 1)); //1 = You're the player leaving
        party?.RemoveMember(session.Player);

        if (party != null)
        {
            party.BroadcastPacketParty(PartyPacket.Leave(session.Player, 0));
        }
    }

    private static void HandleSetLeader(GameSession session, PacketReader packet)
    {
        string target = packet.ReadUnicodeString();

        Player newLeader = GameServer.PlayerManager.GetPlayerByName(target);
        if (newLeader == null)
        {
            return;
        }

        Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party == null)
        {
            return;
        }

        party.BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
        party.Leader = newLeader;
        party.Members.Remove(newLeader);
        party.Members.Insert(0, newLeader);
    }

    private static void HandleFinderJoin(GameSession session, PacketReader packet)
    {
        int partyId = packet.ReadInt();
        string leaderName = packet.ReadUnicodeString();

        Party party = GameServer.PartyManager.GetPartyById(partyId);
        if (party == null)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.OutdatedRecruitmentListing));
            return;
        }

        if (party.PartyFinderId == 0)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.RecruitmentListingDeleted));
            return;
        }

        if (session.Player.Party == null)
        {
            return;
        }

        //Join party
        JoinParty(session, PartyNotice.AcceptedInvite, partyId);
    }

    private static void HandleKick(GameSession session, PacketReader packet)
    {
        long charId = packet.ReadLong();

        Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party == null)
        {
            return;
        }

        Player kickedPlayer = GameServer.PlayerManager.GetPlayerById(charId);
        if (kickedPlayer == null)
        {
            return;
        }

        party.BroadcastPacketParty(PartyPacket.Kick(kickedPlayer));
        party.RemoveMember(kickedPlayer);
    }

    private static void HandleVoteKick(GameSession session, PacketReader packet)
    {
        long charId = packet.ReadLong();

        Party party = session.Player.Party;
        if (party == null)
        {
            return;
        }

        Player kickedPlayer = GameServer.PlayerManager.GetPlayerById(charId);
        if (kickedPlayer == null)
        {
            return;
        }

        if (party.Members.Count < 4)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.InsufficientMemberCountForKickVote));
        }

        //TODO: Keep a counter of vote kicks for a player?
    }

    public static void HandleSummonParty()
    {
        //TODO: implement Summon Party Button
        return;
    }
    private static void HandleStartReadyCheck(GameSession session)
    {
        Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party == null)
        {
            return;
        }

        if (party.ReadyCheck.Count > 0) // a ready check is already in progress
        {
            return;
        }

        party.StartReadyCheck();
    }

    private static void HandleFindDungeonParty(GameSession session, PacketReader packet)
    {
        int dungeonId = packet.ReadInt();

        if (session.Player.Party == null)
        {
            Party newParty = new(session.Player);
            GameServer.PartyManager.AddParty(newParty);
            session.Send(PartyPacket.Create(newParty, true));
        }

        Party party = session.Player.Party;

        // TODO: Party pairing system

        session.Send(PartyPacket.DungeonFindParty());
    }

    private static void CancelFindDungeonParty(GameSession session)
    {
        Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party == null)
        {
            return;
        }

        if (party.Members.Count <= 1)
        {
            party.RemoveMember(session.Player);
        }

        // TODO: Remove party from pairing system

        session.Send(PartyPacket.DungeonFindParty());
    }

    private static void HandleReadyCheckUpdate(GameSession session, PacketReader packet)
    {
        int checkNum = packet.ReadInt() + 1; //+ 1 is because the ReadyChecks variable is always 1 ahead
        byte response = packet.ReadByte();

        Party party = session.Player.Party;
        if (party == null)
        {
            return;
        }

        party.BroadcastPacketParty(PartyPacket.ReadyCheck(session.Player, response));

        party.ReadyCheck.Add(session.Player);
        if (party.ReadyCheck.Count == party.Members.Count)
        {
            party.BroadcastPacketParty(PartyPacket.EndReadyCheck());
            party.ReadyCheck.Clear();
        }
    }
}
