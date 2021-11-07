using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MountPacket
{
    private enum MountPacketMode : byte
    {
        StartRide = 0x0,
        StopRide = 0x1,
        ChangeRide = 0x2,
        StartTwoPersonRide = 0x3,
        StopTwoPersonRide = 0x4
    }

    public static PacketWriter StartRide(IFieldObject<Player> player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
        pWriter.Write(MountPacketMode.StartRide);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteMount(player.Value.Mount);
        return pWriter;
    }

    public static PacketWriter StopRide(IFieldObject<Player> player, bool forced = false)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
        pWriter.Write(MountPacketMode.StopRide);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteByte();
        pWriter.WriteBool(forced);
        return pWriter;
    }

    public static PacketWriter ChangeRide(int playerObjectId, int mountId, long mountUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
        pWriter.Write(MountPacketMode.ChangeRide);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteInt(mountId);
        pWriter.WriteLong(mountUid);
        return pWriter;
    }

    public static PacketWriter StartTwoPersonRide(int otherPlayerObjectId, int playerObjectId, byte index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
        pWriter.Write(MountPacketMode.StartTwoPersonRide);
        pWriter.WriteInt(otherPlayerObjectId);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteByte(index);
        return pWriter;
    }

    public static PacketWriter StopTwoPersonRide(int otherPlayerObjectId, int playerObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_RIDE);
        pWriter.Write(MountPacketMode.StopTwoPersonRide);
        pWriter.WriteInt(otherPlayerObjectId);
        pWriter.WriteInt(playerObjectId);
        return pWriter;
    }
}
