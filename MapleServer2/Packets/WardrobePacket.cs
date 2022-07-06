using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class WardrobePacket
{
    private enum Mode : byte
    {
        Load = 0x5
    }

    public static PacketWriter Load(Wardrobe wardrobe)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Wardrobe);
        pWriter.Write(Mode.Load);
        pWriter.WriteClass(wardrobe);
        return pWriter;
    }
}
