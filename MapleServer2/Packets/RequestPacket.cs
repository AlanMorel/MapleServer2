using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class RequestPacket
    {
        public static Packet Login()
        {
            return PacketWriter.Of(SendOp.REQUEST_LOGIN);
        }

        public static Packet Key()
        {
            return PacketWriter.Of(SendOp.REQUEST_KEY);
        }

        public static Packet Heartbeat(int key)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_HEARTBEAT);
            pWriter.WriteInt(key);

            return pWriter;
        }

        public static Packet TickSync()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_CLIENTTICK_SYNC);
            pWriter.WriteInt(Environment.TickCount);

            return pWriter;
        }
    }
}
