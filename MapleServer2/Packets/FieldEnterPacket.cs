using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldEnterPacket
{
    public static PacketWriter RequestEnter(IFieldActor<Player> fieldPlayer)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RequestFieldEnter);
        pWriter.WriteByte();
        pWriter.WriteInt(fieldPlayer.Value.MapId);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.Write(fieldPlayer.Coord);
        pWriter.Write(fieldPlayer.Rotation);
        pWriter.WriteInt(); // Whatever is here seems to be repeated by client in FIELD_ENTER response.

        return pWriter;
    }
}
