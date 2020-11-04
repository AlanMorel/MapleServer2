using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;
using MapleServer2.Types.FieldObjects;
using Maple2.Data.Types;

namespace MapleServer2.Packets {
    public static class SyncStatePacket {
        public static Packet UserSync(IFieldObject<Player> player, params SyncState[] syncStates) {
            return PacketWriter.Of(SendOp.USER_SYNC)
                .WriteInt(player.ObjectId)
                .WriteSyncStates(syncStates);
        }

        public static Packet RideSync(IFieldObject<Player> player, params SyncState[] syncStates) {
            return PacketWriter.Of(SendOp.RIDE_SYNC)
                .WriteInt(player.ObjectId)
                .WriteSyncStates(syncStates);
        }
    }
}