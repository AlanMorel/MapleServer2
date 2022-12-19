using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BuddyPacket
{
    private enum Mode : byte
    {
        LoadList = 0x1,
        Notice = 0x2,
        AcceptRequest = 0x3,
        DeclineRequest = 0x4,
        Block = 0x5,
        Unblock = 0x6,
        RemoveFromList = 0x7,
        UpdateBuddy = 0x8,
        AddToList = 0x9,
        EditBlockReason = 0xA,
        AcceptNotification = 0xB,
        BlockNotification = 0xC,
        LoginLogoutNotification = 0xE,
        Initialize = 0xF,
        CancelRequest = 0x11,
        EndList = 0x13
    }

    public static PacketWriter LoadList(List<Buddy> buddyList)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.LoadList);
        pWriter.WriteInt(buddyList.Count);
        foreach (Buddy buddy in buddyList)
        {
            WriteBuddy(buddy, pWriter);
        }
        return pWriter;
    }

    public static PacketWriter Notice(byte notice, string playerName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.Notice);
        pWriter.WriteByte(notice);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteUnicodeString();
        return pWriter;
    }

    public static PacketWriter AcceptRequest(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.AcceptRequest);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteLong(buddy.Friend.CharacterId);
        pWriter.WriteLong(buddy.Friend.AccountId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        return pWriter;
    }

    public static PacketWriter DeclineRequest(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.DeclineRequest);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        return pWriter;
    }

    public static PacketWriter Block(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.Block);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        pWriter.WriteUnicodeString(buddy.BlockReason);
        return pWriter;
    }

    public static PacketWriter Unblock(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.Unblock);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        return pWriter;
    }

    public static PacketWriter RemoveFromList(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.RemoveFromList);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteLong(buddy.Friend.AccountId);
        pWriter.WriteLong(buddy.Friend.CharacterId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        return pWriter;
    }

    public static PacketWriter UpdateBuddy(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.UpdateBuddy);
        WriteBuddy(buddy, pWriter);
        return pWriter;
    }

    public static PacketWriter AddToList(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.AddToList);
        WriteBuddy(buddy, pWriter);
        return pWriter;
    }

    public static PacketWriter EditBlockReason(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.EditBlockReason);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        pWriter.WriteUnicodeString(buddy.Message);
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        return pWriter;
    }

    public static PacketWriter AcceptNotification(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.AcceptNotification);
        pWriter.WriteLong(buddy.SharedId);
        return pWriter;
    }

    public static PacketWriter BlockNotification(string playerName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.BlockNotification);
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(playerName);
        return pWriter;
    }

    public static PacketWriter LoginLogoutNotification(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.LoginLogoutNotification);
        pWriter.WriteBool(!buddy.Friend.Session?.Connected() ?? true);
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        return pWriter;
    }

    public static PacketWriter Initialize()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.Initialize);
        return pWriter;
    }

    public static PacketWriter CancelRequest(Buddy buddy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.CancelRequest);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.SharedId);
        return pWriter;
    }

    public static PacketWriter EndList(int buddyCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Buddy);
        pWriter.Write(Mode.EndList);
        pWriter.WriteInt(buddyCount);
        return pWriter;
    }

    private static void WriteBuddy(Buddy buddy, PacketWriter pWriter)
    {
        pWriter.WriteLong(buddy.SharedId);
        pWriter.WriteLong(buddy.Friend.CharacterId);
        pWriter.WriteLong(buddy.Friend.AccountId);
        pWriter.WriteUnicodeString(buddy.Friend.Name);
        pWriter.WriteUnicodeString(buddy.Message);
        pWriter.WriteShort();
        pWriter.WriteInt(buddy.Friend.Account.Home?.MapId ?? 0);
        pWriter.Write(buddy.Friend.JobCode);
        pWriter.Write(buddy.Friend.SubJobCode);
        pWriter.WriteShort(buddy.Friend.Levels.Level);
        pWriter.WriteBool(buddy.IsFriendRequest);
        pWriter.WriteBool(buddy.IsPending);
        pWriter.WriteBool(buddy.Blocked);
        pWriter.WriteBool(buddy.Friend.Session?.Connected() ?? false);
        pWriter.WriteByte();
        pWriter.WriteLong(buddy.Timestamp);
        pWriter.WriteUnicodeString(buddy.Friend.ProfileUrl);
        pWriter.WriteUnicodeString(buddy.Friend.Motto);
        pWriter.WriteUnicodeString(buddy.BlockReason);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(buddy.Friend.Account.Home?.Name ?? "");
        pWriter.WriteLong();
        foreach (int trophyCount in buddy.Friend.TrophyCount)
        {
            pWriter.WriteInt(trophyCount);
        }
    }
}
