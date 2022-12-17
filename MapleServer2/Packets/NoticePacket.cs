using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;

namespace MapleServer2.Packets;

public static class NoticePacket
{
    private enum Mode : byte
    {
        Send = 0x04,
        Quit = 0x05
    }

    public static PacketWriter Notice(string message, NoticeType type = NoticeType.Mint, short durationSec = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Notice);
        pWriter.Write(Mode.Send);
        pWriter.WriteShort((short) type);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteUnicodeString(message);
        if (type.HasFlag(NoticeType.Mint))
        {
            pWriter.WriteShort(durationSec);
        }

        return pWriter;
    }

    public static PacketWriter Notice(SystemNotice notice, NoticeType type = NoticeType.Mint, List<string>? parameters = null, short durationSec = 0)
    {
        parameters ??= new();
        PacketWriter pWriter = PacketWriter.Of(SendOp.Notice);
        pWriter.Write(Mode.Send);
        WriteNotice(pWriter, notice, parameters, type, durationSec);
        return pWriter;
    }

    public static PacketWriter QuitNotice(SystemNotice notice, NoticeType type = NoticeType.Mint, List<string>? parameters = null, short durationSec = 0)
    {
        parameters ??= new();
        PacketWriter pWriter = PacketWriter.Of(SendOp.Notice);
        pWriter.Write(Mode.Quit);
        WriteNotice(pWriter, notice, parameters, type, durationSec);
        return pWriter;
    }

    private static void WriteNotice(PacketWriter pWriter, SystemNotice notice, List<string> parameters, NoticeType type = NoticeType.Mint,
        short durationSec = 0)
    {
        pWriter.WriteShort((short) type);
        pWriter.WriteByte(0x1);
        pWriter.WriteInt(0x1);
        pWriter.Write(notice);
        pWriter.WriteInt(parameters.Count);
        foreach (string parameter in parameters)
        {
            pWriter.WriteUnicodeString(parameter);
        }

        if (type.HasFlag(NoticeType.Mint))
        {
            pWriter.WriteShort(durationSec);
        }
    }
}
