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
            return PacketWriter.Of(SendOp.BUDDY)
                .WriteByte(0x0F);
        }

        public static Packet EndList(int count = 0)
        {
            return PacketWriter.Of(SendOp.BUDDY)
                .WriteByte(0x13)
                .WriteInt(count);
        }
    }
}
