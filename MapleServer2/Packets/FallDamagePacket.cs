using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

internal class FallDamagePacket
{
    public static PacketWriter FallDamage(int objectId, int hpLost)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StateFallDmg);
        pWriter.WriteInt(objectId);
        pWriter.WriteInt(hpLost);

        return pWriter;
    }
}
