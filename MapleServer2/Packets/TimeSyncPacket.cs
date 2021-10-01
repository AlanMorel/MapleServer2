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
        private enum TimeSyncPacketMode : byte
        {
            SetSessionServerTick = 0x0,
            SetInitial1 = 0x1,
            Request = 0x2,
            SetInitial2 = 0x3,
        }

        public static Packet SetSessionServerTick(int key)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteEnum(TimeSyncPacketMode.SetSessionServerTick);
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt(key);

            return pWriter;
        }

        public static Packet SetInitial1()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteEnum(TimeSyncPacketMode.SetInitial1);
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet Request()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteEnum(TimeSyncPacketMode.Request);
            pWriter.WriteInt(Environment.TickCount);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            pWriter.WriteByte();
            pWriter.WriteInt();

            return pWriter;
        }

        public static Packet SetInitial2()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_TIME_SYNC);
            pWriter.WriteEnum(TimeSyncPacketMode.SetInitial2);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            return pWriter;
        }
    }
}
