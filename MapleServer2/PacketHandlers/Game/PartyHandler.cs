using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PartyHandler : GamePacketHandler<PartyHandler>
{
    public override RecvOp OpCode => RecvOp.Party;

    private enum Mode : byte
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
        Mode mode = (Mode) packet.ReadByte(); //Mode

        switch (mode)
        {
            case Mode.Invite:
                HandleInvite(session, packet);
                break;
            case Mode.Join:
                HandleJoin(session, packet);
                break;
            case Mode.Leave:
                HandleLeave(session);
                break;
            case Mode.Kick:
                HandleKick(session, packet);
                break;
            case Mode.SetLeader:
                HandleSetLeader(session, packet);
                break;
            case Mode.FinderJoin:
                HandleFinderJoin(session, packet);
                break;
            case Mode.SummonParty:
                HandleSummonParty();
                break;
            case Mode.VoteKick:
                HandleVoteKick(session, packet);
                break;
            case Mode.ReadyCheck:
                HandleStartReadyCheck(session);
                break;
            case Mode.FindDungeonParty:
                HandleFindDungeonParty(session, packet);
                break;
            case Mode.ReadyCheckUpdate:
                HandleReadyCheckUpdate(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleInvite(GameSession session, PacketReader packet)
    {
        string target = packet.ReadUnicodeString();

        Player? other = GameServer.PlayerManager.GetPlayerByName(target);
        if (other == null)
        {
            return;
        }

        Party? currentParty = session.Player.Party;
        Party? otherParty = other.Party;

        if (currentParty == null)
        {
            // Join Request
            if (otherParty != null && otherParty.Members.Count > 1)
            {
                session.Send(PartyPacket.Notice(other, PartyNotice.RequestToJoin));
                otherParty.Leader.Session?.Send(PartyPacket.JoinRequest(session.Player));
                return;
            }

            // create party
            Party newParty = new(session.Player);
            GameServer.PartyManager.AddParty(newParty);
            session.Send(PartyPacket.Create(newParty, true));
            other.Session?.Send(PartyPacket.SendInvite(session.Player, newParty));
            return;
        }

        if (currentParty.Leader.CharacterId != session.Player.CharacterId)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.NotLeader));
            return;
        }

        if (other.CharacterId == session.Player.CharacterId)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.InviteSelf));
            return;
        }

        if (otherParty != null && otherParty.Members.Count > 1)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.UnableToInvite));
            return;
        }

        other.Session?.Send(PartyPacket.SendInvite(session.Player, currentParty));
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
        Party? party = GameServer.PartyManager.GetPartyById(partyId);
        if (party == null)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.PartyNotFound));
            return;
        }

        if (party.Members.FindAll(m => m.Session?.Player.CharacterId == session.Player.CharacterId).Count > 0 || party.Leader.CharacterId == session.Player.CharacterId)
        {
            return;
        }

        if (response != PartyNotice.AcceptedInvite)
        {
            party.Leader.Session?.Send(PartyPacket.Notice(session.Player, response));
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

        party.BroadcastPacketParty(PartyPacket.Join(session.Player));
        party.AddMember(session.Player);
        session.Send(PartyPacket.Create(party, true));
    }

    private static void HandleLeave(GameSession session)
    {
        Party? party = session.Player.Party;

        if (party is not null && party.DungeonSessionId != -1)
        {
            DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);

            if (dungeonSession is not null && dungeonSession.IsDungeonReservedField(session.Player.MapId, (int) session.Player.InstanceId))
            {
                session.Player.Warp(session.Player.ReturnMapId, session.Player.ReturnCoord, instanceId: 1);
            }
        }

        session.Send(PartyPacket.Leave(session.Player, 1)); //1 = You're the player leaving
        party?.RemoveMember(session.Player);

        party?.BroadcastPacketParty(PartyPacket.Leave(session.Player, 0));
    }

    private static void HandleSetLeader(GameSession session, PacketReader packet)
    {
        string target = packet.ReadUnicodeString();

        Player? newLeader = GameServer.PlayerManager.GetPlayerByName(target);
        if (newLeader == null)
        {
            return;
        }

        Party? party = GameServer.PartyManager.GetPartyByLeader(session.Player);
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

        Party? party = GameServer.PartyManager.GetPartyById(partyId);
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

        //Join party
        JoinParty(session, PartyNotice.AcceptedInvite, partyId);
    }

    private static void HandleKick(GameSession session, PacketReader packet)
    {
        long playerId = packet.ReadLong();

        Player? kickedPlayer = GameServer.PlayerManager.GetPlayerById(playerId);

        Party? party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party is null || kickedPlayer is null)
        {
            return;
        }

        if (party.DungeonSessionId != -1)
        {
            DungeonSession dungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);

            if (dungeonSession.IsDungeonReservedField(kickedPlayer.MapId, (int) kickedPlayer.InstanceId))
            {
                session.Send(PartyPacket.Notice(session.Player, PartyNotice.UnableToKickInDungeonBoss));
                return;
            }
        }

        kickedPlayer.CharacterId = playerId;
        party.BroadcastPacketParty(PartyPacket.Kick(kickedPlayer));
        party.RemoveMember(kickedPlayer);
    }

    private static void HandleVoteKick(GameSession session, PacketReader packet)
    {
        long playerId = packet.ReadLong();

        Party? party = session.Player.Party;
        if (party == null)
        {
            return;
        }

        Player? kickedPlayer = GameServer.PlayerManager.GetPlayerById(playerId);
        if (kickedPlayer == null)
        {
            return;
        }

        if (party.Members.Count < 4)
        {
            session.Send(PartyPacket.Notice(session.Player, PartyNotice.InsufficientMemberCountForKickVote));
        }

        //TODO: gather votes and kick player

        //if kicked player is in a dungeon session
        //that is party has a dungeon Session id != -1
        if (party.DungeonSessionId == -1)
        {
            return;
        }

        DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);

        //if player is in a dungeon session map, warp them to last safe place
        if (dungeonSession is not null && dungeonSession.IsDungeonReservedField(kickedPlayer.MapId, instanceId: (int) kickedPlayer.InstanceId))
        {
            kickedPlayer.Warp(kickedPlayer.ReturnMapId, kickedPlayer.ReturnCoord, instanceId: 1);
        }
    }

    public static void HandleSummonParty()
    {
        //TODO: implement Summon Party Button
    }
    private static void HandleStartReadyCheck(GameSession session)
    {
        Party? party = GameServer.PartyManager.GetPartyByLeader(session.Player);
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

        Party? party = session.Player.Party;
        if (party == null)
        {
            Party newParty = new(session.Player);
            GameServer.PartyManager.AddParty(newParty);
            session.Send(PartyPacket.Create(newParty, true));
            party = newParty;
        }

        // TODO: Party pairing system

        session.Send(PartyPacket.DungeonFindParty(0, true));
    }

    private static void CancelFindDungeonParty(GameSession session)
    {
        Party? party = GameServer.PartyManager.GetPartyByLeader(session.Player);
        if (party == null)
        {
            return;
        }

        if (party.Members.Count <= 1)
        {
            party.RemoveMember(session.Player);
        }

        // TODO: Remove party from pairing system

        session.Send(PartyPacket.DungeonFindParty(0, false));
    }

    private static void HandleReadyCheckUpdate(GameSession session, PacketReader packet)
    {
        int checkNum = packet.ReadInt() + 1; //+ 1 is because the ReadyChecks variable is always 1 ahead
        byte response = packet.ReadByte();

        Party? party = session.Player.Party;
        if (party == null)
        {
            return;
        }

        party.BroadcastPacketParty(PartyPacket.ReadyCheck(session.Player, response));

        party.ReadyCheck.Add(session.Player);
        if (party.ReadyCheck.Count != party.Members.Count)
        {
            return;
        }

        party.BroadcastPacketParty(PartyPacket.EndReadyCheck());
        party.ReadyCheck.Clear();
    }
}
