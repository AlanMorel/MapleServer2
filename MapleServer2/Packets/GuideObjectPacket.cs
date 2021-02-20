using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GuideObjectPacket
    {
        public static Packet Bracket(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
            pWriter.WriteByte(0x00);
            pWriter.WriteShort(1); // Type?
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(player.Value.CharacterId);
            pWriter.Write(Block.ClosestBlock(player.Coord));
            pWriter.Write<CoordF>(default); // Unknown

            return pWriter;
        }

        public static Packet Remove(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
            pWriter.WriteByte(0x01);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(player.Value.CharacterId);

            return pWriter;
        }
    }
}
