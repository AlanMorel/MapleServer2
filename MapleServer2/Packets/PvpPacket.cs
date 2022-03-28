using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class PvpPacket
{
    private enum PvpMode : byte
    {
        Mode16 = 0x16,
        Mode17 = 0x17,
        Mode0C = 0x0C
    }

    public static PacketWriter Mode16()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PVP);
        pWriter.Write(PvpMode.Mode16);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Mode17()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PVP);
        pWriter.Write(PvpMode.Mode17);
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Mode0C()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PVP);
        pWriter.Write(PvpMode.Mode0C);
        pWriter.WriteByte();

        return pWriter;
    }
}
