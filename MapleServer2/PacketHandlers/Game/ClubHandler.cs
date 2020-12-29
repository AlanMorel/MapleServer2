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
            party.BroadcastPacketParty(ClubPacket.CreateClub(session.Player, party.Leader, clubName, party.Members));
            party.BroadcastPacketParty(ClubPacket.UpdateClubs(session.Player));
        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }
            long clubId = packet.ReadLong();
            party.BroadcastPacketParty(ClubPacket.Invite());
            session.Send(ClubPacket.AssignLeader(session.Player));
            session.Send(ClubPacket.EstablishClub(session.Player));
        }
    }

}