using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MountPacket
    {
        public static Packet StartRide(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteByte(0x00);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteMount(player.Value.Mount);

            return pWriter;
        }

        public static Packet StopRide(IFieldObject<Player> player, bool forced = false)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteByte(0x01);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteBool(forced);

            return pWriter;
        }

        public static Packet ChangeRide(int playerObjectId, int mountId, long mountUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteByte(0x02);
            pWriter.WriteInt(playerObjectId);
            pWriter.WriteInt(mountId);
            pWriter.WriteLong(mountUid);

            return pWriter;
        }
    }
}
