using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace MapleServer2.PacketHandlers.Game {
    public class PartyHandler : GamePacketHandler {
        public override RecvOp OpCode => RecvOp.PARTY;

        public PartyHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet) {
            int mode = (int) packet.ReadByte(); //Type / Mode

            switch(mode)
            {
                //Party invite
                case 1:
                    HandleInvite(session, packet);
                    break;
                //Party join
                case 2:
                    HandleJoin(session, packet);
                    break;
                //Party leave
                case 3:
                    HandleLeave(session, packet);
                    break;
                //Kick player
                case 4:
                    HandleKick(session, packet);
                    break;
                //Set party leader
                case 17:
                    HandleSetLeader(session, packet);
                    break;

            }
        }
        private void HandleInvite(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            //TODO: Check if invited player already in a party.
            Player other = GameServer.Storage.GetPlayerByName(target);
            if (other != null)
            {
                other.Session.Send(PartyPacket.SendInvite(session.Player));
                session.Send(PartyPacket.Create(session.Player));
                //pSession.Send(ChatPacket.Send(session.Player, "You were invited to a party by " + session.Player.Name, ChatType.NoticeAlert));
            }

        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            int accept = packet.ReadByte();
            int unknown = packet.ReadInt();
            if(accept == 1)
            {
                //TODO:
                // Check if party already exists with leader (other)
                // If already exists, join that party and send this player joining to all other party members
                // If it doesn't exists, create party and add other and this session


                //Create party object and append to parties master list somewhere
                Player other = GameServer.Storage.GetPlayerByName(target);
                if (other != null)
                {
                    GameSession pSession = other.Session;
                    pSession.Send(PartyPacket.Join(session.Player));
                    pSession.Send(PartyPacket.UpdatePlayer(session.Player));
                    pSession.Send(PartyPacket.UpdateHitpoints(other));
                    pSession.Send(PartyPacket.UpdateHitpoints(session.Player));

                    //Need a special JoinParty that sends all players in party to newly joining player.
                    session.Send(PartyPacket.CreateExisting(other, session.Player));
                    session.Send(PartyPacket.UpdatePlayer(other));
                    session.Send(PartyPacket.Join(session.Player));

                    session.Send(PartyPacket.UpdateHitpoints(other));
                    session.Send(PartyPacket.UpdateHitpoints(session.Player));
                }
            }
            else
            {
                //Send Decline message to inviting player?
            }
        }

        private void HandleSetLeader(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            Player newLeader = GameServer.Storage.GetPlayerByName(target);
            if (newLeader != null)
            {
                //TODO: Send to all players in party
                MapleServer.BroadcastPacketAll(PartyPacket.SetLeader(newLeader));
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            //TODO: Send to all players in party
            MapleServer.BroadcastPacketAll(PartyPacket.Leave(session.Player));
        }

        private void HandleKick(GameSession session, PacketReader packet)
        {
            long char_id = packet.ReadLong();
            Player kickedPlayer = GameServer.Storage.GetPlayerById(char_id);
            //TODO: Send to all players in party
            session.Send(PartyPacket.Kick(kickedPlayer));
            kickedPlayer.Session.Send(PartyPacket.Kick(kickedPlayer));

            //TODO: When kicking a player, if there is only 1 player left in the party we need to force them to leave.
            session.Send(PartyPacket.Leave(session.Player));
        }
    }
}