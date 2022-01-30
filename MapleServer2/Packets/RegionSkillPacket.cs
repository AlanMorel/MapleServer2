using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class RegionSkillPacket
{
    private enum RegionSkillMode : byte
    {
        Add = 0x0,
        Remove = 0x1
    }

    public static PacketWriter Send(int sourceObjectId, SkillCast skill, byte tileCount, List<CoordF> effectCoords)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.REGION_SKILL);

        pWriter.Write(RegionSkillMode.Add);
        pWriter.WriteInt(sourceObjectId);
        pWriter.WriteInt(sourceObjectId);
        pWriter.WriteInt();
        pWriter.WriteByte(tileCount);
        if (tileCount == 0)
        {
            return pWriter;
        }

        foreach (CoordF effectCoord in effectCoords)
        {
            pWriter.Write(effectCoord);
        }

        pWriter.WriteInt(skill.SkillId);
        pWriter.WriteShort(skill.SkillLevel);
        pWriter.WriteLong();

        return pWriter;
    }

    public static PacketWriter Remove(int sourceObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.REGION_SKILL);
        pWriter.Write(RegionSkillMode.Remove);
        pWriter.WriteInt(sourceObjectId); // Uid regionEffect
        return pWriter;
    }
}
