using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets;

public static class NoticePacket
{
    private enum NoticePacketMode : byte
    {
        Send = 0x04
    }

    public static PacketWriter Notice(string message, NoticeType type = NoticeType.Mint)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.NOTICE);
        pWriter.Write(NoticePacketMode.Send);
        pWriter.WriteShort((short) type);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(message);
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter Notice(SystemNotice notice, NoticeType type = NoticeType.Mint, List<string> parameters = null)
    {
        parameters ??= new();
        PacketWriter pWriter = PacketWriter.Of(SendOp.NOTICE);
        pWriter.Write(NoticePacketMode.Send);
        pWriter.WriteShort((short) type);
        pWriter.WriteByte(0x1);
        pWriter.WriteInt(0x1);
        pWriter.Write(notice);
        pWriter.WriteInt(parameters.Count);
        foreach (string parameter in parameters)
        {
            pWriter.WriteUnicodeString(parameter);
        }
        pWriter.WriteShort();
        return pWriter;
    }
}
