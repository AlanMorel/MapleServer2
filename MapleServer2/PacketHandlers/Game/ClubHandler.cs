using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class ClubHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CLUB;

        public ClubHandler() : base() { }

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
            ClubMode mode = (ClubMode) packet.ReadByte();

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
                    HandleBuff(packet);
                    break;
                case ClubMode.Rename:
                    HandleRename(packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleCreate(GameSession session, PacketReader packet)
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

        private static void HandleJoin(GameSession session, PacketReader packet)
        {
            Party party = session.Player.Party;
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
                party.BroadcastPacketParty(ClubPacket.UpdatePlayerClubList(session.Player, club));
            }
            else
            {
                int response = packet.ReadInt(); // 0 = accept, 76 (0x4C) = reject

                if (response == 0)
                {
                    party.BroadcastPacketParty(ClubPacket.ConfirmCreate(club.Id));
                    club.Leader.Session.Send(ClubPacket.Join(session.Player, club));
                    club.Leader.Session.Send(ClubPacket.Establish(club));
                    // TODO add member to club (club.AddMember(session.Player);)
                }
                else
                {
                    // TODO update to proper rejection packet
                    party.Leader.Session.Send(ChatPacket.Send(party.Leader, session.Player.Name + " declined the invitation.", ChatType.NoticeAlert2));
                }
            }
        }

        private static void HandleSendInvite(GameSession session, PacketReader packet)
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

            // TODO check that the club can fit more people, if it's at max members, return/leave error

            session.Send(ClubPacket.InviteSentReceipt(clubId, other));
            other.Session.Send(ClubPacket.Invite(club, other));
        }

        private static void HandleInviteResponse(GameSession session, PacketReader packet)
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
                club.Leader.Session.Send(ClubPacket.LeaderInviteResponse(club, invitee, response));
                club.BroadcastPacketClub(ClubPacket.ConfirmInvite(club, other.Session.Player));
                other.Session.Send(ClubPacket.InviteResponse(club, session.Player));
                other.Session.Send(ClubPacket.Join(session.Player, club));
                other.Session.Send(ClubPacket.UpdateClub(club));
                club.BroadcastPacketClub(ClubPacket.UpdatePlayerClubList(session.Player, club));
                // TODO add member to club (club.AddMember(other);)
            }
            else
            {
                club.Leader.Session.Send(ClubPacket.LeaderInviteResponse(club, invitee, response));
                other.Session.Send(ClubPacket.InviteResponse(club, session.Player));
            }
        }

        private static void HandleLeave(GameSession session, PacketReader packet)
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
                // TODO remove member from club (club.RemoveMember(session.Player);)
            }
        }

        private static void HandleBuff(PacketReader packet)
        {
            long clubId = packet.ReadLong();
            int buffId = packet.ReadInt();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            // TODO add buff effect packet
            club.Leader.Session.Send(ClubPacket.ChangeBuffReceipt(club, buffId));
            club.BroadcastPacketClub(ClubPacket.ChangeBuff(club, buffId));
        }

        private static void HandleRename(PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string clubNewName = packet.ReadUnicodeString();

            Club club = GameServer.ClubManager.GetClubById(clubId);
            if (club == null)
            {
                return;
            }

            club.BroadcastPacketClub(ClubPacket.Rename(club, clubNewName));
            // TODO rename club (club.SetName(clubNewName);)
        }
    }
}
