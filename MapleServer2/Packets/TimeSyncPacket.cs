using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    // Whenever server sends 02, client makes a request
    //
    // 01 and 03 seem to be the first ones sent after entering game
    // perhaps setting some initial state?
    public static class TimeSyncPacket
    {
        public static Packet Response(int key)
        {
            return PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC)
            .WriteByte(0x00) // Response
            .WriteInt(Environment.TickCount)
            .WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .WriteByte()
            .WriteInt()
            .WriteInt(key);
        }

        // Request client to make a request
        public static Packet Request()
        {
            return PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC)
            .WriteByte(0x02) // 1 and 2
            .WriteInt(Environment.TickCount)
            .WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .WriteByte()
            .WriteInt();
        }

        public static Packet SetInitial1()
        {
            return PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC)
            .WriteByte(0x01) // 1 and 2
            .WriteInt(Environment.TickCount)
            .WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            .WriteByte()
            .WriteInt();

        }

        public static Packet SetInitial2()
        {
            return PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC)
            .WriteByte(0x03)
            .WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
    }
}