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
        public static Packet SetSessionServerTick(int key)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteByte(0x00); // Response
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt(key);

            return pWriter;
        }

        // Request client to make a request
        public static Packet Request()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteByte(0x02); // 1 and 2
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet SetInitial1()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteByte(0x01); // 1 and 2
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet SetInitial2()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteByte(0x03);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return pWriter;
        }
    }
}
