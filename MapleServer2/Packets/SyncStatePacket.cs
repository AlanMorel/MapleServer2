using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SyncStatePacket
    {
        public static Packet UserSync(IFieldObject<Player> player, params SyncState[] syncStates)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_SYNC);
            pWriter.WriteInt(player.ObjectId);
            SyncStateHelper.WriteSyncStates(pWriter, syncStates);

            return pWriter;
        }

        public static Packet RideSync(IFieldObject<Player> player, params SyncState[] syncStates)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.RIDE_SYNC);
            pWriter.WriteInt(player.ObjectId);
            SyncStateHelper.WriteSyncStates(pWriter, syncStates);

            return pWriter;
        }
    }
}
