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
                //Send party invite
                case 1:
                    HandleInvite(session, packet);
                    break;
                //Party join?
                case 2:
                    HandleJoin(session, packet);
                    break;
                case 3:
                    HandleLeave(session, packet);
                    break;

            }
        }


        private void HandleInvite(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            MapleServer.BroadcastAll(pSession => {
                if (pSession.Player.Name == target)
                {
                    pSession.Send(PartyPacket.SendInvite(pSession.Player, session.Player));
                    pSession.Send(ChatPacket.Send(session.Player, "You were invited to a party by " + session.Player.Name, ChatType.NoticeAlert));
                    session.Send(PartyPacket.CreateParty(session.Player));
                }
            });
        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            int accept = packet.ReadByte();
            short unk = packet.ReadShort();
            byte unk2 = packet.ReadByte();
            byte unk3 = packet.ReadByte();
            if(accept == 1)
            {
                Player other = null;
                //Create party object and append to parties master list somewhere
                MapleServer.BroadcastAll(pSession => {
                    if (pSession.Player.Name == target)
                    {
                        other = pSession.Player;
                        

                        pSession.Send(PartyPacket.JoinParty(session.Player));
                        pSession.Send(PartyPacket.JoinParty2(session.Player));

                        pSession.Send(PartyPacket.UpdateHitpoints(pSession.Player));
                        pSession.Send(PartyPacket.UpdateHitpoints(session.Player));
                        

                    }
                });
                //Need a special JoinParty that sends all players in party to newly joining player.
                session.Send(PartyPacket.JoinParty3(other, session.Player));
                session.Send(PartyPacket.JoinParty2(other));
                session.Send(PartyPacket.JoinParty(session.Player));

                session.Send(PartyPacket.UpdateHitpoints(other));
                session.Send(PartyPacket.UpdateHitpoints(session.Player));
            }
            else
            {
                //Send Decline message to inviting player?
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            session.Send(PartyPacket.LeaveParty(session.Player));
        }

        private void HandleKick(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
        }
    }
}