using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public class RevivalConfirmPacket
{
    public static PacketWriter Send(int objectId, long unk)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.RevivalConfirm);

        pWriter.WriteInt(objectId);
        pWriter.WriteLong(unk);

        return pWriter;
    }
}
