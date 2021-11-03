using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class RequestPacket
    {
        public static PacketWriter Login()
        {
            return PacketWriter.Of(SendOp.REQUEST_LOGIN);
        }

        public static PacketWriter Key()
        {
            return PacketWriter.Of(SendOp.REQUEST_KEY);
        }

        public static PacketWriter Heartbeat(int key)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_HEARTBEAT);
            pWriter.WriteInt(key);

            return pWriter;
        }

        public static PacketWriter TickSync()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_CLIENTTICK_SYNC);
            pWriter.WriteInt(Environment.TickCount);

            return pWriter;
        }
    }
}
