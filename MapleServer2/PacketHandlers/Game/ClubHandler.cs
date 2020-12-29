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


        public override void Handle(GameSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte(); //Mode

            switch (mode)
            {
                //Club Create
                case 0x1:
                    HandleCreate(session, packet);
                    break;
                //Club Join
                case 0x3:
                    HandleJoin(session, packet);
                    break;
            }
        }
        private void HandleCreate(GameSession session, PacketReader packet)
        {
            string clubName = packet.ReadUnicodeString();
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }
            List<Player> members = party.Members;
            party.BroadcastPacketParty(ClubPacket.CreateClub(session.Player, party.Leader, clubName, members));
            party.BroadcastPacketParty(ClubPacket.UpdateClubs(session.Player));
        }
        private void HandleJoin(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }
            List<Player> members = party.Members;
            long clubId = packet.ReadLong();
            party.BroadcastPacketParty(ClubPacket.Invite());
            session.Send(ClubPacket.AssignLeader(session.Player));
            session.Send(ClubPacket.EstablishClub(session.Player));
        }
    }

}