using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class CharacterAbilityPacket
{
    public static PacketWriter Send()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharAbility);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteInt();

        return pWriter;
    }
}
