using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldPortalPacket
{
    private enum PortalType : byte
    {
        AddPortal = 0x00,
        RemovePortal = 0x01,
        UpdatePortal = 0x02
    }

    public static PacketWriter AddPortal(IFieldObject<Portal> fieldPortal)
    {
        Portal portal = fieldPortal.Value;
        CoordF coord = fieldPortal.Coord;
        coord.Z -= 75; // Looks like every portal coord is offset by 75

        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.AddPortal);
        pWriter.WriteInt(portal.Id);
        pWriter.WriteBool(portal.IsVisible);
        pWriter.WriteBool(portal.IsEnabled);
        pWriter.Write(coord);
        pWriter.Write(portal.Rotation);
        pWriter.Write(CoordF.From(150, 150, 150)); // not sure (200,200,250) was used a lot
        pWriter.WriteUnicodeString();
        pWriter.WriteInt(portal.TargetMapId);
        pWriter.WriteInt(fieldPortal.ObjectId);
        pWriter.WriteInt((int) portal.UGCPortalMethod);
        pWriter.WriteBool(portal.IsMinimapVisible);
        pWriter.WriteLong(portal.TargetHomeAccountId);
        pWriter.Write(portal.PortalType);
        pWriter.WriteInt(portal.Duration);
        pWriter.WriteShort();
        pWriter.WriteInt();
        pWriter.WriteBool(portal.IsPassEnabled);
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();
        pWriter.WriteUnicodeString();

        return pWriter;
    }

    public static PacketWriter RemovePortal(Portal portal)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.RemovePortal);
        pWriter.WriteInt(portal.Id);

        return pWriter;
    }

    public static PacketWriter UpdatePortal(IFieldObject<Portal> portal)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.UpdatePortal);
        pWriter.WriteInt(portal.Value.Id);
        pWriter.WriteBool(portal.Value.IsVisible);
        pWriter.WriteBool(portal.Value.IsEnabled);
        pWriter.WriteBool(portal.Value.IsMinimapVisible);
        pWriter.WriteBool(false);
        pWriter.WriteBool(false);
        return pWriter;
    }
}
