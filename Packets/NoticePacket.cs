using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class NoticePacket
    {
        private enum NoticePacketMode : byte
        {
            Red = 0x04
        }

        public static Packet Notice(string message, byte type = 0x10)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NOTICE);

            pWriter.WriteMode(NoticePacketMode.Red); // Only known mode atm
            /* Known types
            00 chat
            01 chat
            05 chat and fast text
            10 mint
            11 chat and mint
            14 fast text
            15 chat and fast text
            */
            pWriter.WriteByte(type);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(message);
            pWriter.WriteShort();

            return pWriter;
        }
    }
}
