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
            return PacketWriter.Of(SendOp.REQUEST_HEARTBEAT)
                .WriteInt(key);
        }

        public static Packet TickSync(int serverTick)
        {
            return PacketWriter.Of(SendOp.REQUEST_CLIENTTICK_SYNC)
                .WriteInt(serverTick);
        }
    }
}