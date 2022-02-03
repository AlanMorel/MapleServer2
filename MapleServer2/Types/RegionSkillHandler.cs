using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public static class RegionSkillHandler
{
    public static void Handle(GameSession session, int sourceId, CoordF coords, SkillCast skillCast)
    {
        List<MagicPathMove> skillMoves = skillCast.ParentSkill?.GetMagicPaths().MagicPathMoves;
        byte tileCount = (byte) (skillMoves?.Count ?? 0);

        List<CoordF> effectCoords = new();
        if (tileCount > 0)
        {
            Player player = session.Player;

            effectCoords = GetEffectCoords(skillMoves, player.FieldPlayer.LookDirection, coords, player.MapId);
            tileCount = (byte) effectCoords.Count;
        }

        session.FieldManager.BroadcastPacket(RegionSkillPacket.Send(sourceId, skillCast, tileCount, effectCoords));
        Remove(session, skillCast, sourceId);
    }

    private static void Remove(GameSession session, SkillCast skillCast, int sourceId)
    {
        Task.Run(async () =>
        {
            // TODO: Get the correct Region Skill Duration when calling chain Skills
            await Task.Delay(skillCast.DurationTick() + 5000);
            session.FieldManager.BroadcastPacket(RegionSkillPacket.Remove(sourceId));
        });
    }

    /// <summary>
    /// Get the coordinates of the skill's effect, if needed change the offset to match the direction of the player.
    /// For skills that paint the ground, match the correct height.
    /// </summary>
    private static List<CoordF> GetEffectCoords(List<MagicPathMove> skillMoves, short lookDirection, CoordF effectCoord, int mapId)
    {
        List<CoordF> effectCoords = new();
        foreach (MagicPathMove magicPathMove in skillMoves)
        {
            CoordF offSetCoord = magicPathMove.FireOffsetPosition;

            // If false, rotate the offset based on the look direction. Example: Wizard's Tornado
            if (!magicPathMove.IgnoreAdjust)
            {
                // Rotate the offset coord based on the look direction
                CoordF rotatedOffset = CoordF.From(offSetCoord.Length(), lookDirection);

                // Add the effect coord to the rotated coord
                offSetCoord = rotatedOffset + effectCoord;
                effectCoords.Add(offSetCoord);
                continue;
            }

            offSetCoord += Block.ClosestBlock(effectCoord);

            CoordS tempBlockCoord = offSetCoord.ToShort();

            // Set the height to the max allowed, which is one block above the cast coord.
            tempBlockCoord.Z += Block.BLOCK_SIZE * 2;

            // Find the first block below the effect coord
            int distanceToNextBlockBelow = MapMetadataStorage.GetDistanceToNextBlockBelow(mapId, offSetCoord.ToShort(), out MapBlock blockBelow);

            // If the block is null or the distance from the cast effect Z height is greater than two blocks, continue
            if (blockBelow is null || distanceToNextBlockBelow > Block.BLOCK_SIZE * 2)
            {
                continue;
            }

            // If there is a block above, continue
            if (MapMetadataStorage.BlockAboveExists(mapId, blockBelow.Coord))
            {
                continue;
            }

            // If block is liquid, continue
            if (MapMetadataStorage.IsLiquidBlock(blockBelow))
            {
                continue;
            }

            // Since this is the block below, add 150 units to the Z coord so the effect is above the block
            offSetCoord = blockBelow.Coord.ToFloat();
            offSetCoord.Z += Block.BLOCK_SIZE;

            effectCoords.Add(offSetCoord);
        }

        return effectCoords;
    }
}
