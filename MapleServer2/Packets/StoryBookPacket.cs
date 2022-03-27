using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class StoryBookPacket
{
    public static PacketWriter Open(int storyBookId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.StoryBook);
        pWriter.WriteInt(storyBookId);
        return pWriter;
    }
}
