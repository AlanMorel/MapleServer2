using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class VibratePacket
    {
        public static Packet Vibrate(string objectHash, long someId, int objectId, int flag, Player player, int clientTicks)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIBRATE);
            pWriter.WriteByte(1);
            pWriter.WriteMapleString(objectHash);
            pWriter.WriteLong(someId);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(flag);
            pWriter.Write(player.Coord.ToShort());
            pWriter.WriteInt(clientTicks);
            pWriter.WriteMapleString("");
            pWriter.WriteByte();

            return pWriter;
        }
    }
}
