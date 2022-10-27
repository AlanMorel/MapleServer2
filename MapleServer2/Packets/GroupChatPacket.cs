using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GroupChatPacket
{
    private enum Mode : byte
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
        Error = 0xD
    }

    public static PacketWriter Update(GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Update);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteByte((byte) groupChat.Members.Count);
        foreach (Player member in groupChat.Members)
        {
            pWriter.WriteByte(0x1);
            pWriter.WriteClass(member);
        }
        return pWriter;
    }

    public static PacketWriter Create(GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Create);
        pWriter.WriteInt(groupChat.Id);
        return pWriter;
    }

    public static PacketWriter Invite(Player member, Player invitee, GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Invite);
        pWriter.WriteUnicodeString(member.Name);
        pWriter.WriteUnicodeString(invitee.Name);
        pWriter.WriteInt(groupChat.Id);
        return pWriter;
    }

    public static PacketWriter Join(Player member, Player invitee, GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Join);
        pWriter.WriteUnicodeString(member.Name);
        pWriter.WriteUnicodeString(invitee.Name);
        pWriter.WriteInt(groupChat.Id);
        return pWriter;
    }

    public static PacketWriter Leave(GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Leave);
        pWriter.WriteInt(groupChat.Id);
        return pWriter;
    }

    public static PacketWriter UpdateGroupMembers(Player member, Player invitee, GroupChat groupChat)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.UpdateGroupMembers);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteUnicodeString(member.Name);
        pWriter.WriteByte(0x1);
        pWriter.WriteClass(invitee);
        return pWriter;
    }

    public static PacketWriter LeaveNotice(GroupChat groupChat, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.LeaveNotice);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter LoginNotice(GroupChat groupChat, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.LoginNotice);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter LogoutNotice(GroupChat groupChat, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.LogoutNotice);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteUnicodeString(player.Name);
        return pWriter;
    }

    public static PacketWriter Chat(GroupChat groupChat, Player player, string message)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Chat);
        pWriter.WriteInt(groupChat.Id);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(message);
        return pWriter;
    }

    public static PacketWriter Error(Player player, string other, int error)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GroupChat);
        pWriter.Write(Mode.Error);
        pWriter.WriteByte(0x2);
        pWriter.WriteInt(error);
        pWriter.WriteUnicodeString(player.Name);
        pWriter.WriteUnicodeString(other);
        return pWriter;
    }
}
