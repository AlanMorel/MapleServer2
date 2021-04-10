using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class GuideObjectPacket
    {
        private enum GuideObjectPacketMode : byte
        {
            Add = 0x0,
            Remove = 0x1,
            Sync = 0x2,
        }

        public static Packet Add(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
            pWriter.WriteEnum(GuideObjectPacketMode.Add);
            pWriter.WriteShort(player.Value.Guide.Value.Type);
            pWriter.WriteInt(player.Value.Guide.ObjectId);
            pWriter.WriteLong(player.Value.CharacterId);
            pWriter.Write(player.Value.Guide.Coord);
            pWriter.Write(player.Rotation);
            if (player.Value.Guide.Value.Type == 0)
            {
                pWriter.WriteLong();
            }

            return pWriter;
        }

        public static Packet Remove(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
            pWriter.WriteEnum(GuideObjectPacketMode.Remove);
            pWriter.WriteInt(player.Value.Guide.ObjectId);
            pWriter.WriteLong(player.Value.CharacterId);

            return pWriter;
        }

        public static Packet Sync(IFieldObject<GuideObject> guide, int unk2, CoordS unkCoord)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
            pWriter.WriteEnum(GuideObjectPacketMode.Sync);
            pWriter.WriteInt(guide.ObjectId);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.Write(guide.Coord);
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteByte();
            pWriter.Write(guide.Rotation);
            pWriter.WriteByte();

            return pWriter;
        }
    }
}
