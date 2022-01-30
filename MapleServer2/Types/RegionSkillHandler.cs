using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
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
            effectCoords = GetEffectCoords(skillMoves, session.Player.FieldPlayer.LookDirection, coords);
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
    /// For skills that paint the ground, check if the block exists.
    /// </summary>
    /// <param name="skillMoves">List of MagicPathMove</param>
    /// <param name="lookDirection">Look direction of character</param>
    /// <param name="effectCoord">Cast location</param>
    private static List<CoordF> GetEffectCoords(List<MagicPathMove> skillMoves, short lookDirection, CoordF effectCoord)
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
            }
            else
            {
                offSetCoord += Block.ClosestBlock(effectCoord);
            }

            effectCoords.Add(offSetCoord);
        }

        return effectCoords;
    }
}
