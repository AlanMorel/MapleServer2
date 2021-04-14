using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SendCubesPacket
    {
        private enum SendCubesPacketMode : byte
        {
            Load = 0x1,
        }

        public static Packet Load()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
            pWriter.WriteEnum(SendCubesPacketMode.Load);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteByte();
            return pWriter;
        }
    }
}
