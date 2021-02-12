using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class LoginResultPacket
    {
        public static Packet InitLogin(long accountId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_RESULT);
            pWriter.WriteByte(); // Login State
            pWriter.WriteInt(); // Const
            pWriter.WriteUnicodeString(""); // Ban reason
            pWriter.WriteLong(accountId);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // SyncTime
            pWriter.WriteInt(Environment.TickCount); // SyncTicks
            pWriter.WriteByte(); // TimeZone
            pWriter.WriteByte(); // BlockType
            pWriter.WriteInt(); // Const
            pWriter.WriteLong(); // Const
            pWriter.WriteInt(2); // Const

            return pWriter;
        }

        // Set 0 for all values in login packet
        public static Packet Timeout(long accountId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_RESULT);
            pWriter.WriteByte(0x19); // Login State
            pWriter.WriteInt(); // Const
            pWriter.WriteUnicodeString(""); // Ban reason
            pWriter.WriteLong(accountId);
            pWriter.WriteLong(); // SyncTime
            pWriter.WriteInt(); // SyncTicks
            pWriter.WriteByte(); // TimeZone
            pWriter.WriteByte(); // BlockType
            pWriter.WriteInt(); // Const
            pWriter.WriteLong(); // Const
            pWriter.WriteInt(); // Const]

            return pWriter;
        }
    }
}
