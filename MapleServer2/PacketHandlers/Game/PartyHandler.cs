using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

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
                default:
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
                //Create party object and append to parties master list somewhere
                MapleServer.BroadcastAll(pSession => {
                    if (pSession.Player.Name == target)
                    {
                        pSession.Send(PartyPacket.JoinParty(pSession.Player, session.Player));
                        pSession.Send(PartyPacket.JoinParty2(pSession.Player, session.Player));
                        pSession.Send(PartyPacket.JoinParty(pSession.Player, session.Player));
                        pSession.Send(PartyPacket.JoinParty3(pSession.Player, session.Player));
                        session.Send(PartyPacket.JoinParty(session.Player, session.Player));
                        session.Send(PartyPacket.JoinParty2(pSession.Player, session.Player));
                        session.Send(PartyPacket.JoinParty(pSession.Player, session.Player));
                        session.Send(PartyPacket.JoinParty3(session.Player, session.Player));
                    }
                });
                
            }
            else
            {
                //Send Decline message to inviting player?
            }
        }
    }
}