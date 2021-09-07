using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class MountPacket
    {
        private enum MountPacketMode : byte
        {
            StartRide = 0x0,
            StopRide = 0x1,
            ChangeRide = 0x2,
            StartTwoPersonRide = 0x3,
            StopTwoPersonRide = 0x4,
        }

        public static Packet StartRide(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteEnum(MountPacketMode.StartRide);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteMount(player.Value.Mount);
            return pWriter;
        }

        public static Packet StopRide(IFieldObject<Player> player, bool forced = false)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteEnum(MountPacketMode.StopRide);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteBool(forced);
            return pWriter;
        }

        public static Packet ChangeRide(int playerObjectId, int mountId, long mountUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteEnum(MountPacketMode.ChangeRide);
            pWriter.WriteInt(playerObjectId);
            pWriter.WriteInt(mountId);
            pWriter.WriteLong(mountUid);
            return pWriter;
        }

        public static Packet StartTwoPersonRide(int otherPlayerObjectId, int playerObjectId, byte index)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteEnum(MountPacketMode.StartTwoPersonRide);
            pWriter.WriteInt(otherPlayerObjectId);
            pWriter.WriteInt(playerObjectId);
            pWriter.WriteByte(index);
            return pWriter;
        }

        public static Packet StopTwoPersonRide(int otherPlayerObjectId, int playerObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
            pWriter.WriteEnum(MountPacketMode.StopTwoPersonRide);
            pWriter.WriteInt(otherPlayerObjectId);
            pWriter.WriteInt(playerObjectId);
            return pWriter;
        }
    }
}
