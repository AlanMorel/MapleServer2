using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class VibratePacket
    {
        public static Packet Vibrate(string objectHash, long someId, int objectId, int flag, Player player, int clientTicks)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIBRATE)
                .WriteByte(1)
                .WriteMapleString(objectHash)
                .WriteLong(someId)
                .WriteInt(objectId)
                .WriteInt(flag)
                .Write(player.Coord.ToShort())
                .WriteInt(clientTicks)
                .WriteMapleString("")
                .WriteByte();
            return pWriter;
        }
    }
}
