using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class SkinColorPacket
    {
        public static Packet Update(IFieldObject<Player> player, SkinColor skinColor)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_SKIN_COLOR);
            pWriter.WriteInt(player.ObjectId);
            pWriter.Write(skinColor);
            return pWriter;
        }
    }
}
