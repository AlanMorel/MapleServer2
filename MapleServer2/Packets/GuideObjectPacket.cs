using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class GuideObjectPacket
{
    private enum GuideObjectPacketMode : byte
    {
        Add = 0x0,
        Remove = 0x1,
        Sync = 0x2
    }

    public static PacketWriter Add(IFieldObject<GuideObject> guide)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
        pWriter.Write(GuideObjectPacketMode.Add);
        pWriter.WriteShort(guide.Value.Type);
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteLong(guide.Value.BoundCharacterId);
        pWriter.Write(guide.Coord);
        pWriter.Write(guide.Rotation);
        if (guide.Value.Type == 0)
        {
            pWriter.WriteLong();
        }

        return pWriter;
    }

    public static PacketWriter Remove(IFieldObject<GuideObject> guide)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
        pWriter.Write(GuideObjectPacketMode.Remove);
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteLong(guide.Value.BoundCharacterId);

        return pWriter;
    }

    public static PacketWriter Sync(IFieldObject<GuideObject> guide, byte unk2, byte unk3, byte unk4, byte unk5, CoordS unkCoord, short unk6, int unk7)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GUIDE_OBJECT);
        pWriter.Write(GuideObjectPacketMode.Sync);
        pWriter.WriteInt(guide.ObjectId);
        pWriter.WriteByte(unk2);
        pWriter.WriteByte(unk3);
        pWriter.WriteByte(unk4);
        pWriter.WriteByte(unk5);
        pWriter.Write(guide.Coord.ToShort());
        pWriter.Write(unkCoord);
        pWriter.Write(guide.Rotation.ToShort());
        pWriter.WriteShort(unk6);
        pWriter.WriteInt(unk7);

        return pWriter;
    }
}
