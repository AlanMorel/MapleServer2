using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
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

            switch (type)
            {

                case ChatType.Channel: //TODO: Send to all players on current channel
                    break;
                case ChatType.Super:
                case ChatType.World:
                    //Send to all players online
                    MapleServer.BroadcastPacketAll(ChatPacket.Send(session.Player, message, type));
                    break;
                case ChatType.GuildNotice:
                case ChatType.Guild:
                    //TODO: Send to all in guild
                    break;
                case ChatType.Party:
                    Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
                    if (party != null)
                    {
                        party.BroadcastPacketParty(ChatPacket.Send(session.Player, message, type));
                    }
                    break;
                case ChatType.WhisperTo:
                    Player recipientPlayer = GameServer.Storage.GetPlayerByName(recipient);

                    if (recipientPlayer != null)
                    {
                        recipientPlayer.Session.Send(ChatPacket.Send(session.Player, message, ChatType.WhisperFrom));
                        session.Send(ChatPacket.Send(recipientPlayer, message, ChatType.WhisperTo));
                    }
                    else
                    {
                        session.Send(ChatPacket.Send(session.Player, "Player not found or they are not online.", ChatType.WhisperFail));
                    }
                    break;
                default:
                    session.FieldManager.SendChat(session.Player, message, type);
                    break;
            }
        }
    }
}
