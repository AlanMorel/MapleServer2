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
            Join = 0x3, //for creating
            Invite = 0x6,
            InviteResponse = 0x8,
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
                case ClubMode.Invite:
                    HandleInvite(session, packet);
                    break;
                case ClubMode.InviteResponse:
                    HandleInviteResponse(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleCreate(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }

            string clubName = packet.ReadUnicodeString();

            Club club = new Club(party, clubName);
            GameServer.ClubManager.AddClub(club);

            party.BroadcastPacketParty(ClubPacket.CreateClub(party, club));


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
                    party.BroadcastPacketParty(ClubPacket.UpdatePlayerClubList(member, club.Id));
                }
            }
            else
            {
                // you are not leader but have responded to the invitation
                int response = packet.ReadInt(); // 0 = accept, 76 (0x4C) = reject

                if (response == 0)
                {
                    party.BroadcastPacketParty(ClubPacket.ClubCreated(club.Id));
                    club.Leader.Session.Send(ClubPacket.Join(session.Player, club));
                    club.Leader.Session.Send(ClubPacket.EstablishClub(club));
                }
                else
                {
                    party.Leader.Session.Send(ChatPacket.Send(party.Leader, session.Player.Name + " declined the invitation.", ChatType.NoticeAlert2));
                }
            }

            // TODO only send after invite accepted, broadcast to entire party?
        }

        private void HandleInvite(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string invitee = packet.ReadUnicodeString();
            session.Send(ClubPacket.InviteSentReceipt(clubId, invitee));
        }

        private void HandleInviteResponse(GameSession session, PacketReader packet)
        {
            long clubId = packet.ReadLong();
            string clubName = packet.ReadUnicodeString();
            string clubLeader = packet.ReadUnicodeString();
            packet.ReadUnicodeString(); //playerName
            byte response = packet.ReadByte(); // 0 = accept
            session.Send(ClubPacket.InviteResponse(clubId, clubName, clubLeader, session.Player, response));
            //TODO handle response
        }
    }
}