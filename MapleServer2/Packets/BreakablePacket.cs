using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class BreakablePacket
    {
        public static Packet Break(string objectHash, byte flag)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BREAKABLE);
            pWriter.WriteByte(1);
            pWriter.WriteMapleString(objectHash);
            pWriter.WriteByte(flag);
            pWriter.WriteInt();
            pWriter.WriteInt(); //Unk, trigger id maybe
            pWriter.WriteByte();

            return pWriter;
        }
    }
}
