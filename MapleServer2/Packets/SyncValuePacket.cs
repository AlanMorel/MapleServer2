using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class SyncValuePacket
{
    public static PacketWriter SetSyncValue(int value)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.SYNC_VALUE);
        pWriter.WriteInt(value);

        return pWriter;
    }
}
