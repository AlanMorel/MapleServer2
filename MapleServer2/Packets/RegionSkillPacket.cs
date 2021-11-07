using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
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

    public static PacketWriter Send(int sourceObjectId, CoordS effectCoord, SkillCast skill)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.REGION_SKILL);
        SkillCast parentSkill = skill.ParentSkill;
        List<MagicPathMove> skillMoves = parentSkill?.GetMagicPaths().MagicPathMoves ?? null;
        byte tileCount = (byte) (skillMoves != null ? skillMoves.Count : 1);

        pWriter.Write(RegionSkillMode.Add);
        pWriter.WriteInt(sourceObjectId);
        pWriter.WriteInt(sourceObjectId);
        pWriter.WriteInt();
        pWriter.WriteByte(tileCount);
        if (tileCount == 0)
        {
            return pWriter;
        }

        for (int i = 0; i < tileCount; i++)
        {
            CoordF currentSkillCoord = skillMoves != null ? skillMoves[i].FireOffsetPosition : CoordF.From(0, 0, 0);
            CoordF castPosition = Block.ClosestBlock(currentSkillCoord + effectCoord.ToFloat());

            pWriter.Write(castPosition);
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
