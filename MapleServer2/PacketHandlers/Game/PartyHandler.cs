using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
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

            Player other = GameServer.Storage.GetPlayerByName(target);
            if (other != null)
            {
                if (other.Party == null)
                {
                    other.Session.Send(PartyPacket.SendInvite(session.Player));
                    session.Send(PartyPacket.Create(session.Player));
                    //pSession.Send(ChatPacket.Send(session.Player, "You were invited to a party by " + session.Player.Name, ChatType.NoticeAlert));
                }
                else
                {
                    session.Send(ChatPacket.Send(session.Player, other.Session.Player.Name + " is already in a party.", ChatType.NoticeAlert2));
                }
            }

        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            int accept = packet.ReadByte();
            int unknown = packet.ReadInt();
            if(accept == 1)
            {
                Player other = GameServer.Storage.GetPlayerByName(target);
                if (other != null)
                {
                    if (other.Party != null)
                    {
                        //Load old
                        other.Party.AddPlayer(session.Player);
                        foreach(Player partyMember in other.Party.Players)
                        {
                            GameSession pSession = partyMember.Session;
                            pSession.Send(PartyPacket.Join(session.Player));
                            pSession.Send(PartyPacket.UpdatePlayer(session.Player));
                            pSession.Send(PartyPacket.UpdateHitpoints(session.Player));
                        }
                    }
                    else
                    {
                        //Create new
                        Party newParty = new Party(GuidGenerator.Int(), 10, other.CharacterId, new HashSet<Player> { other, session.Player });
                        other.Party = newParty;
                        GameSession pSession = other.Session;
                        pSession.Send(PartyPacket.Join(session.Player));
                        pSession.Send(PartyPacket.UpdatePlayer(session.Player));
                        pSession.Send(PartyPacket.UpdateHitpoints(session.Player));
                        pSession.Send(PartyPacket.UpdateHitpoints(other));
                    }
                    session.Player.Party = other.Party;
                    session.Send(PartyPacket.CreateExisting(other, other.Party.Players));
                    foreach (Player partyMember in other.Party.Players)
                    {
                        if (partyMember != session.Player)
                        {
                            session.Send(PartyPacket.UpdatePlayer(partyMember));
                            session.Send(PartyPacket.UpdateHitpoints(partyMember));
                        }
                    }
                    session.Send(PartyPacket.Join(session.Player));
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
                foreach (Player partyMember in session.Player.Party.Players)
                {
                    partyMember.Party.Leader = newLeader.CharacterId;
                    partyMember.Session.Send(PartyPacket.SetLeader(newLeader));
                }
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            session.Player.Party.RemovePlayer(session.Player);
            foreach (Player partyMember in session.Player.Party.Players)
            {
                partyMember.Party.RemovePlayer(session.Player);
                partyMember.Session.Send(PartyPacket.Leave(session.Player));
                if (partyMember.Party.Players.Count < 2)
                {
                    //Disband
                    partyMember.Party = null;
                }
            }
            session.Send(PartyPacket.Leave(session.Player));
            session.Player.Party = null;
        }

        private void HandleKick(GameSession session, PacketReader packet)
        {
            long char_id = packet.ReadLong();
            Player kickedPlayer = GameServer.Storage.GetPlayerById(char_id);



            //TODO: finish this










            //TODO: Send to all players in party
            session.Send(PartyPacket.Kick(kickedPlayer));
            kickedPlayer.Session.Send(PartyPacket.Kick(kickedPlayer));

            //TODO: When kicking a player, if there is only 1 player left in the party we need to force them to leave.
            session.Send(PartyPacket.Leave(session.Player));
        }
    }
}