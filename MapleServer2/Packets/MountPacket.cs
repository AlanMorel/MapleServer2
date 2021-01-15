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
            return PacketWriter.Of(SendOp.RESPONSE_RIDE)
                .WriteByte(0x00)
                .WriteInt(player.ObjectId)
                .WriteMount(player.Value.Mount);
        }

        public static Packet StopRide(IFieldObject<Player> player, bool forced = false)
        {
            return PacketWriter.Of(SendOp.RESPONSE_RIDE)
                .WriteByte(0x01)
                .WriteInt(player.ObjectId)
                .WriteByte()
                .WriteBool(forced);
        }

        public static Packet ChangeRide(int playerObjectId, int mountId, long mountUid)
        {
            return PacketWriter.Of(SendOp.RESPONSE_RIDE)
                .WriteByte(0x02)
                .WriteInt(playerObjectId)
                .WriteInt(mountId)
                .WriteLong(mountUid);
        }
    }
}
