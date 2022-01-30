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

    public static PacketWriter Send(int sourceObjectId, CoordS effectCoord, SkillCast skill, short lookDirection)
    {
        List<MagicPathMove> skillMoves = skill.ParentSkill?.GetMagicPaths().MagicPathMoves;
        byte tileCount = (byte) (skillMoves?.Count ?? 0);

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

        for (int i = 0; i < tileCount; i++)
        {
            MagicPathMove magicPathMove = skillMoves?[i];
            if (magicPathMove is null)
            {
                continue;
            }

            CoordF offSetCoord = magicPathMove.FireOffsetPosition;

            // If false, rotate the offset based on the look direction.
            if (!magicPathMove.IgnoreAdjust)
            {
                // Rotate the offset coord based on the look direction
                CoordF rotatedOffset = CoordF.From(offSetCoord.Length(), lookDirection);

                // Add the effect coord to the rotated coord
                offSetCoord = rotatedOffset + effectCoord.ToFloat();
            }
            else
            {
                offSetCoord += Block.ClosestBlock(effectCoord.ToFloat());
            }

            pWriter.Write(offSetCoord);
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
