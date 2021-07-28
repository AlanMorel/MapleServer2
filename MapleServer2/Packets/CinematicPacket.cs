using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class CinematicPacket
    {
        private enum CinematicPacketMode : byte
        {
            HideUi = 0x1,
            View = 0x3,
        }

        public static Packet HideUi(bool hide)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
            pWriter.WriteEnum(CinematicPacketMode.HideUi);
            pWriter.WriteBool(hide);
            return pWriter;
        }

        public static Packet View(int type)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CINEMATIC);
            pWriter.WriteEnum(CinematicPacketMode.View);
            pWriter.WriteInt(type);
            pWriter.WriteInt();
            return pWriter;
        }
    }
}
