using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class UserChatHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.USER_CHAT;

        public UserChatHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ChatType type = (ChatType)packet.ReadInt();
            string message = packet.ReadUnicodeString();
            string recipient = packet.ReadUnicodeString();
            packet.ReadLong();

            GameCommandActions.Process(session, message);

            switch(type)
            {
                case ChatType.Channel:
                    //TODO: Send to all players on current channel
                case ChatType.World:
                    //Send to all players online
                    MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
                    break;
                case ChatType.GuildNotice:
                case ChatType.Guild:
                    //TODO: Send to all in guild
                    break;
                case ChatType.Party:
                    //TODO: Send to all in party
                    break;
                case ChatType.WhisperTo:
                    Types.Player recipientPlayer = null;
                    MapleServer.BroadcastAll(pSession => {
                        if (pSession.Player.Name == recipient)
                        {
                            pSession.Send(ChatPacket.Send(session.Player, message, ChatType.WhisperFrom));
                            recipientPlayer = pSession.Player;
                        }
                    });
                    if (recipientPlayer != null)
                    {
                        session.Send(ChatPacket.Send(recipientPlayer, message, type));
                        break;
                    }
                    goto case ChatType.WhisperFail;
                case ChatType.WhisperFail:
                    session.Send(ChatPacket.Send(session.Player, "Player not found or they are not online.", ChatType.WhisperFail));
                    break;
                default:
                    session.FieldManager.SendChat(session.Player, message, type);
                    break;
            }
        }
    }
}
// Party invite
// 01 09 00 42 00 75 00 62 00 62 00 6C 00 65 00 47 00 75 00 6E 00