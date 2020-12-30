using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
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

            long clubId = 0; // TODO generate unique club id

            party.BroadcastPacketParty(ClubPacket.CreateClub(party, clubName, clubId));

            foreach (Player member in party.Members) {
                party.BroadcastPacketParty(ClubPacket.UpdateClub(member, clubId));
            }
        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party == null)
            {
                return;
            }

            long clubId = packet.ReadLong();
            int response = packet.ReadInt(); // 0 = accept, 76 (0x4C) = reject??? lol

            string clubName = ""; // TODO get club name from club

            // TODO handle rejections

            if (session.Player.CharacterId == party.Leader.CharacterId) {
                session.Send(ClubPacket.AssignLeader(session.Player, clubId, clubName));
                session.Send(ClubPacket.EstablishClub(clubId, clubName));
            } 

            session.Send(ClubPacket.ClubCreated(clubId)); // TODO only send after invite accepted, broadcast to entire party?
        }
    }
}