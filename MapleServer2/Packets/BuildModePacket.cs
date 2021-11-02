using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class BuildModePacket
    {
        public static PacketWriter Use(IFieldObject<Player> fieldPlayer, bool start, int itemId = 0, long itemUid = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SET_BUILD_MODE);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            pWriter.WriteBool(start);
            if (start)
            {
                pWriter.WriteInt(itemId);
                pWriter.WriteLong(itemUid);
                pWriter.WriteLong();
                pWriter.WriteByte();
                pWriter.WriteInt();
            }
            return pWriter;
        }
    }
}
