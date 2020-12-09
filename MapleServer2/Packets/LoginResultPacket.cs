using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class LoginResultPacket
    {
        public static Packet InitLogin(long accountId)
        {
            return PacketWriter.Of(SendOp.LOGIN_RESULT)
                .WriteByte() // Login State
                .WriteInt() // Const
                .WriteUnicodeString("") // Ban reason
                .WriteLong(accountId)
                .WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()) // SyncTime
                .WriteInt(Environment.TickCount) // SyncTicks
                .WriteByte() // TimeZone
                .WriteByte() // BlockType
                .WriteInt() // Const
                .WriteLong() // Const
                .WriteInt(2); // Const
        }

        // Set 0 for all values in login packet
        public static Packet Timeout(long accountId)
        {
            return PacketWriter.Of(SendOp.LOGIN_RESULT)
                .WriteByte(0x19) // Login State
                .WriteInt() // Const
                .WriteUnicodeString("") // Ban reason
                .WriteLong(accountId)
                .WriteLong() // SyncTime
                .WriteInt() // SyncTicks
                .WriteByte() // TimeZone
                .WriteByte() // BlockType
                .WriteInt() // Const
                .WriteLong() // Const
                .WriteInt(); // Const
        }
    }
}