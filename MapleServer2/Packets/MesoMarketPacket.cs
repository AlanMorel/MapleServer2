using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class MesoMarketPacket
    {
        private enum MesoMarketPacketMode : byte
        {
            Unk1 = 0x1,
            Unk2 = 0x2,
            Unk4 = 0x4,
        }

        public static Packet Unk1()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MESO_MARKET);
            pWriter.WriteEnum(MesoMarketPacketMode.Unk1);
            pWriter.WriteFloat();
            pWriter.WriteFloat(0.2f);
            pWriter.WriteLong(100);
            pWriter.WriteInt(5);
            pWriter.WriteInt(5);
            pWriter.WriteInt(4);
            pWriter.WriteInt(2);
            pWriter.WriteInt(100);
            pWriter.WriteInt(100);
            pWriter.WriteInt(1000);
            return pWriter;
        }

        public static Packet Unk2()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MESO_MARKET);
            pWriter.WriteEnum(MesoMarketPacketMode.Unk2);
            pWriter.WriteInt(0);
            pWriter.WriteInt(0);
            return pWriter;

        }

        public static Packet Unk4()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MESO_MARKET);
            pWriter.WriteEnum(MesoMarketPacketMode.Unk4);
            pWriter.WriteInt(0);
            return pWriter;

        }
    }
}
