using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SyncStatePacket
{
    public static PacketWriter UserSync(IFieldObject<Player> player, params SyncState[] syncStates)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserSync);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteSyncStates(syncStates);

        return pWriter;
    }

    public static PacketWriter RideSync(IFieldObject<Player> player, params SyncState[] syncStates)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RideSync);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteSyncStates(syncStates);

        return pWriter;
    }
}
