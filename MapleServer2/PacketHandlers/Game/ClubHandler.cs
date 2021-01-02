using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ClubHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CLUB;

        public ClubHandler(ILogger<ClubHandler> logger) : base(logger) { }

        private enum ClubMode : byte
        {
            Create = 0x1,
            Join = 0x3,
            SendInvite = 0x6,
            InviteResponse = 0x8,
            Leave = 0xA,
            Buff = 0xD,
            Rename = 0xE,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ClubMode mode = (ClubMode)packet.ReadByte();

            switch (mode)
            {
                case ClubMode.Create:
                    HandleCreate(session, packet);
                    break;
                case ClubMode.Join:
                    HandleJoin(session, packet);
                    break;
                case ClubMode.SendInvite:
                    HandleSendInvite(session, packet);
                    break;
                case ClubMode.InviteResponse:
                    HandleInviteResponse(session, packet);
                    break;
                case ClubMode.Leave:
                    HandleLeave(session, packet);
                    break;
                case ClubMode.Buff:
                    HandleBuff(session, packet);
                    break;
                case ClubMode.Rename:
                    HandleRename(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleCreate(GameSession session, PacketReader packet)
        {
            // TODO fix creating for a party of more than 2. Currently if a member does not respond, despite atleast one other member accepting, it does not get created.
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }

            string clubName = packet.ReadUnicodeString();

            Club club = new Club(party, clubName);
            GameServer.ClubManager.AddClub(club);

            party.BroadcastPacketParty(ClubPacket.Create(party, club));
        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party == null)
            {
                return;
            }

            long clubId = packet.ReadLong();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            if (club.Leader.CharacterId == session.Player.CharacterId)
            {
                foreach (Player member in party.Members)
                {
                    party.BroadcastPacketParty(ClubPacket.UpdatePlayerClubList(session.Player, club));
                }
            }
            else
            {
                int response = packet.ReadInt(); // 0 = accept, 76 (0x4C) = reject

                if (response == 0)
                {
                    party.BroadcastPacketParty(ClubPacket.ConfirmCreate(club.Id));
                    club.Leader.Session.Send(ClubPacket.Join(session.Player, club));
                    club.Leader.Session.Send(ClubPacket.Establish(club));
                }
                else
                {
                    party.Leader.Session.Send(ChatPacket.Send(party.Leader, session.Player.Name + " declined the invitation.", ChatType.NoticeAlert2));
                }
            }
        }

        private void HandleSendInvite(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string invitee = packet.ReadUnicodeString();

            Player other = GameServer.Storage.GetPlayerByName(invitee);
            if (other == null)
            {
                return;
            }

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            session.Send(ClubPacket.InviteSentReceipt(clubId, other.Session.Player));
            other.Session.Send(ClubPacket.Invite(club, other.Session.Player));
        }

        private void HandleInviteResponse(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string clubName = packet.ReadUnicodeString();
            string clubLeader = packet.ReadUnicodeString();
            string invitee = packet.ReadUnicodeString(); //playerName. TODO: verify player name
            byte response = packet.ReadByte(); // 1 = accept

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            Player other = GameServer.Storage.GetPlayerByName(invitee);
            if (other == null)
            {
                return;
            }

            if (response == 1)
            {
                club.Leader.Session.Send(ClubPacket.LeaderInviteResponse(clubId, invitee, response));
                club.BroadcastPacketClub(ClubPacket.ConfirmInvite(clubId, other.Session.Player, response, clubLeader));
                other.Session.Send(ClubPacket.InviteResponse(clubId, clubName, clubLeader, session.Player, response));
                other.Session.Send(ClubPacket.Join(session.Player, club));
                other.Session.Send(ClubPacket.UpdateClub(club, invitee));
                club.BroadcastPacketClub(ClubPacket.UpdatePlayerClubList(session.Player, club));
            }
            else
            {
                club.Leader.Session.Send(ClubPacket.LeaderInviteResponse(clubId, invitee, response));
                other.Session.Send(ClubPacket.InviteResponse(clubId, clubName, clubLeader, session.Player, response));
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            if (session.Player.CharacterId == club.Leader.CharacterId)
            {
                if (club.Members.Count < 2)
                {
                    // TODO fix disbanding
                    club.BroadcastPacketClub(ClubPacket.Disband(club));
                    club.BroadcastPacketClub(ClubPacket.UpdatePlayerClubList(session.Player, club));
                }
                else
                {
                    // TODO fix reassigning leader
                    session.Send(ClubPacket.LeaveClub(club));
                    club.BroadcastPacketClub(ClubPacket.LeaveNotice(club, session.Player));
                    club.BroadcastPacketClub(ClubPacket.AssignNewLeader(session.Player, club));
                }
            }
            else
            {
                session.Send(ClubPacket.LeaveClub(club));
                club.BroadcastPacketClub(ClubPacket.LeaveNotice(club, session.Player));
            }
        }

        private void HandleBuff(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            int buffId = packet.ReadInt();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            club.Leader.Session.Send(ClubPacket.ChangeBuffReceipt(club, buffId));
            club.BroadcastPacketClub(ClubPacket.ChangeBuff(club, buffId));
        }

        private void HandleRename(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string clubNewName = packet.ReadUnicodeString();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            club.BroadcastPacketClub(ClubPacket.Rename(club, clubNewName));
        }
    }
}