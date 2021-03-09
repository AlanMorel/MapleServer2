using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets
{
    public static class NoticePacket
    {
        private enum NoticePacketMode : byte
        {
            Send = 0x04
        }

        public static Packet Notice(string message, NoticeType type = NoticeType.Mint)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NOTICE);
            pWriter.WriteEnum(NoticePacketMode.Send);
            pWriter.WriteShort((short) type);
            pWriter.WriteByte(0x0);
            pWriter.WriteInt();
            pWriter.WriteUnicodeString(message);
            pWriter.WriteShort();
            return pWriter;
        }

        public static Packet Notice(SystemNotice notice, NoticeType type = NoticeType.Mint)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.NOTICE);
            pWriter.WriteEnum(NoticePacketMode.Send);
            pWriter.WriteShort((short) type);
            pWriter.WriteByte(0x1);
            pWriter.WriteInt(0x1);
            pWriter.WriteInt((int) notice);
            pWriter.WriteInt();
            return pWriter;
        }
    }
}
