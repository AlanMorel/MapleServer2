using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GroupChatPacket
    {
        private enum GroupChatPacketMode : byte
        {
            Update = 0x0,
            Create = 0x1,
            Invite = 0x2,
            Join = 0x3,
            Leave = 0x4,
            UpdateGroupMembers = 0x6,
            LeaveNotice = 0x7,
            LoginNotice = 0x8,
            LogoutNotice = 0x9,
            Chat = 0xA,
            Error = 0xD,
        }

        public static Packet Update(GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Update);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteByte((byte) groupChat.Members.Count);
            foreach (Player member in groupChat.Members)
            {
                pWriter.WriteByte(0x1);
                CharacterListPacket.WriteCharacter(member, pWriter);
            }
            return pWriter;
        }

        public static Packet Create(GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Create);
            pWriter.WriteInt(groupChat.Id);
            return pWriter;
        }

        public static Packet Invite(Player member, Player invitee, GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Invite);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteUnicodeString(invitee.Name);
            pWriter.WriteInt(groupChat.Id);
            return pWriter;
        }

        public static Packet Join(Player member, Player invitee, GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Join);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteUnicodeString(invitee.Name);
            pWriter.WriteInt(groupChat.Id);
            return pWriter;
        }

        public static Packet Leave(GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Leave);
            pWriter.WriteInt(groupChat.Id);
            return pWriter;
        }

        public static Packet UpdateGroupMembers(Player member, Player invitee, GroupChat groupChat)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.UpdateGroupMembers);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteUnicodeString(member.Name);
            pWriter.WriteByte(0x1);
            CharacterListPacket.WriteCharacter(invitee, pWriter);
            return pWriter;
        }

        public static Packet LeaveNotice(GroupChat groupChat, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.LeaveNotice);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet LoginNotice(GroupChat groupChat, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.LoginNotice);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet LogoutNotice(GroupChat groupChat, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.LogoutNotice);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteUnicodeString(player.Name);
            return pWriter;
        }

        public static Packet Chat(GroupChat groupChat, Player player, string message)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Chat);
            pWriter.WriteInt(groupChat.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(message);
            return pWriter;
        }

        public static Packet Error(Player player, string other, int error)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GROUP_CHAT);
            pWriter.WriteEnum(GroupChatPacketMode.Error);
            pWriter.WriteByte(0x2);
            pWriter.WriteInt(error);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteUnicodeString(other);
            return pWriter;
        }


    }
}
