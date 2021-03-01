﻿using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class GroupChatHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.GROUP_CHAT;

        public GroupChatHandler(ILogger<GroupChatHandler> logger) : base(logger) { }

        private enum GroupChatMode : byte
        {
            Create = 0x1,
            Invite = 0x2,
            Leave = 0x4,
            Chat = 0x0A,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            GroupChatMode mode = (GroupChatMode) packet.ReadByte();

            switch (mode)
            {
                case GroupChatMode.Create:
                    HandleCreate(session);
                    break;
                case GroupChatMode.Invite:
                    HandleInvite(session, packet);
                    break;
                case GroupChatMode.Leave:
                    HandleLeave(session, packet);
                    break;
                case GroupChatMode.Chat:
                    HandleChat(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private enum GroupChatError : int
        {
            OfflinePlayer = 0x3,
            MaxGroups = 0xA,
        }

        private static void HandleCreate(GameSession session)
        {
            GroupChat groupChat = new(session.Player);
            GameServer.GroupChatManager.AddGroupChat(groupChat);

            session.Send(GroupChatPacket.Update(groupChat));
            session.Send(GroupChatPacket.Create(groupChat));
        }

        private static void HandleInvite(GameSession session, PacketReader packet)
        {
            string targetPlayer = packet.ReadUnicodeString();
            int groupChatId = packet.ReadInt();

            GroupChat groupChat = GameServer.GroupChatManager.GetGroupChatById(groupChatId);
            if (groupChat == null)
            {
                return;
            }

            Player other = GameServer.Storage.GetPlayerByName(targetPlayer);
            if (other == null)
            {
                session.Send(GroupChatPacket.Error(session.Player, targetPlayer, (int) GroupChatError.OfflinePlayer));
                return;
            }

            int count = other.GroupChatId.Count(x => x != 0);

            if (count >= 3) // 3 is the max group chats a user can be at
            {
                session.Send(GroupChatPacket.Error(session.Player, targetPlayer, (int) GroupChatError.MaxGroups));

                return;
            }

            session.Send(GroupChatPacket.Invite(session.Player, other, groupChat));
            groupChat.BroadcastPacketGroupChat(GroupChatPacket.UpdateGroupMembers(session.Player, other, groupChat));

            groupChat.AddMember(other);

            other.Session.Send(GroupChatPacket.Update(groupChat));
            other.Session.Send(GroupChatPacket.Join(session.Player, other, groupChat));
        }

        private static void HandleLeave(GameSession session, PacketReader packet)
        {
            int groupChatId = packet.ReadInt();

            GroupChat groupChat = GameServer.GroupChatManager.GetGroupChatById(groupChatId);
            if (groupChat == null)
            {
                return;
            }

            groupChat.RemoveMember(session.Player);
            session.Send(GroupChatPacket.Leave(groupChat));
            groupChat.BroadcastPacketGroupChat(GroupChatPacket.LeaveNotice(groupChat, session.Player));
        }

        private static void HandleChat(GameSession session, PacketReader packet)
        {
            string message = packet.ReadUnicodeString();
            int groupChatId = packet.ReadInt();

            GroupChat groupChat = GameServer.GroupChatManager.GetGroupChatById(groupChatId);
            if (groupChat == null)
            {
                return;
            }

            groupChat.BroadcastPacketGroupChat(GroupChatPacket.Chat(groupChat, session.Player, message));
        }
    }
}
