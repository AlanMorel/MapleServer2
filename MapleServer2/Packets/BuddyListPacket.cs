using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public class BuddyListPacket
    {
        public static Packet AddEntry()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);

            pWriter.WriteByte(0x01);
            pWriter.WriteInt(); // Count
            // ...

            return pWriter;
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteByte(0x0F);

            return pWriter;
        }

        public static Packet EndList(int count = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BUDDY);
            pWriter.WriteByte(0x13);
            pWriter.WriteInt(count);

            return pWriter;
        }
    }
}
