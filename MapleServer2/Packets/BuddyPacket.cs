using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class BuddyPacket
    {
        private enum BuddyPacketMode : byte
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
            LoginLogoutNotifcation = 0xE,
            Initialize = 0xF,
            CancelRequest = 0x11,
            EndList = 0x13,
        }

        public static Packet LoadList(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.LoadList);
            pWriter.WriteInt(player.BuddyList.Count);
            foreach (Buddy buddy in player.BuddyList)
            {
                WriteBuddy(buddy, pWriter);
            }
            return pWriter;
        }

        public static Packet Notice(byte notice, string playerName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.Notice);
            pWriter.WriteByte(notice);
            pWriter.WriteUnicodeString(playerName);
            pWriter.WriteUnicodeString("");
            return pWriter;
        }

        public static Packet AcceptRequest(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.AcceptRequest);
            pWriter.WriteByte(0x0);
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteLong(buddy.Friend.CharacterId);
            pWriter.WriteLong(buddy.Friend.AccountId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            return pWriter;
        }

        public static Packet DeclineRequest(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.DeclineRequest);
            pWriter.WriteByte(0x0);
            pWriter.WriteLong(buddy.SharedId);
            return pWriter;
        }

        public static Packet Block(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.Block);
            pWriter.WriteByte();
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            pWriter.WriteUnicodeString(buddy.BlockReason);
            return pWriter;
        }

        public static Packet Unblock(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.Unblock);
            pWriter.WriteByte(0x0);
            pWriter.WriteLong(buddy.SharedId);
            return pWriter;
        }

        public static Packet RemoveFromList(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.RemoveFromList);
            pWriter.WriteByte(0x0);
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteLong(buddy.Friend.AccountId);
            pWriter.WriteLong(buddy.Friend.CharacterId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            return pWriter;
        }

        public static Packet UpdateBuddy(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.UpdateBuddy);
            WriteBuddy(buddy, pWriter);
            return pWriter;
        }

        public static Packet AddToList(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.AddToList);
            WriteBuddy(buddy, pWriter);
            return pWriter;
        }

        public static Packet EditBlockReason(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.EditBlockReason);
            pWriter.WriteByte();
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            pWriter.WriteUnicodeString(buddy.Message);
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            return pWriter;
        }

        public static Packet AcceptNotification(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.AcceptNotification);
            pWriter.WriteLong(buddy.SharedId);
            return pWriter;
        }

        public static Packet BlockNotification(string playerName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.BlockNotification);
            pWriter.WriteByte();
            pWriter.WriteUnicodeString(playerName);
            return pWriter;
        }

        public static Packet LoginLogoutNotifcation(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.LoginLogoutNotifcation);
            pWriter.WriteBool(true); // TODO: Change to online check
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            return pWriter;
        }

        public static Packet Initialize()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.Initialize);
            return pWriter;
        }

        public static Packet CancelRequest(Buddy buddy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.CancelRequest);
            pWriter.WriteByte();
            pWriter.WriteLong(buddy.SharedId);
            return pWriter;
        }

        public static Packet EndList(int buddyCount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteEnum(BuddyPacketMode.EndList);
            pWriter.WriteInt(buddyCount);
            return pWriter;
        }

        public static void WriteBuddy(Buddy buddy, PacketWriter pWriter)
        {
            pWriter.WriteLong(buddy.SharedId);
            pWriter.WriteLong(buddy.Friend.CharacterId);
            pWriter.WriteLong(buddy.Friend.AccountId);
            pWriter.WriteUnicodeString(buddy.Friend.Name);
            pWriter.WriteUnicodeString(buddy.Message);
            pWriter.WriteShort();
            pWriter.WriteInt(); // player Home map ID
            pWriter.WriteEnum(buddy.Friend.Job);
            pWriter.WriteEnum(buddy.Friend.JobCode);
            pWriter.WriteShort(buddy.Friend.Levels.Level);
            pWriter.WriteBool(buddy.IsFriendRequest);
            pWriter.WriteBool(buddy.IsPending);
            pWriter.WriteBool(buddy.Blocked);
            pWriter.WriteBool(true); // TODO: Change to online check
            pWriter.WriteByte(0);
            pWriter.WriteLong(buddy.Timestamp);
            pWriter.WriteUnicodeString(buddy.Friend.ProfileUrl);
            pWriter.WriteUnicodeString(buddy.Friend.Motto);
            pWriter.WriteUnicodeString(buddy.BlockReason);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(buddy.Friend.HomeName);
            pWriter.WriteLong();
            foreach (int trophyCount in buddy.Friend.Trophy)
            {
                pWriter.WriteInt(trophyCount);
            }
        }
    }
}
