using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using NLog;

namespace MapleServer2.Types;

public static class RegionSkillHandler
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void HandleEffect(GameSession session, SkillCast skillCast)
    {
        Player player = session.Player;
        IFieldActor<Player> fieldPlayer = player.FieldPlayer;

        if (skillCast.SkillAttack is null)
        {
            return;
        }

        skillCast.EffectCoords = GetEffectCoords(skillCast.SkillAttack, fieldPlayer.Coord, player.MapId, fieldPlayer.LookDirection);

        session.FieldManager.AddRegionSkillEffect(skillCast);

        Task removeEffectTask = RemoveEffects(session, skillCast);

        // TODO: Vibrate objects around skill
        if (skillCast.Interval <= 0)
        {
            HandleRegionSkill(session, skillCast);
            return;
        }

        // Task to loop trough all entities in range to do damage/heal
        Task.Run(async () =>
        {
            while (!removeEffectTask.IsCompleted)
            {
                HandleRegionSkill(session, skillCast);

                // TODO: Find the correct delay for the skill
                await Task.Delay(skillCast.Interval);
            }
        });
    }

    private static Task RemoveEffects(GameSession session, SkillCast skillCast)
    {
        return Task.Run(async () =>
        {
            // TODO: Get the correct Region Skill Duration when calling chain Skills
            await Task.Delay(skillCast.Duration);
            if (!session.FieldManager.RemoveRegionSkillEffect(skillCast))
            {
                Logger.Error("Failed to remove Region Skill");
            }
        });
    }

    /// <summary>
    /// Get the coordinates of the skill's effect, if needed change the offset to match the direction of the player.
    /// For skills that paint the ground, match the correct height.
    /// </summary>
    private static List<CoordF> GetEffectCoords(SkillAttack skillAttack, CoordF effectCoord, int mapId, int lookDirection)
    {
        List<MagicPathMove> cubeSkillMoves = new();
        if (skillAttack.CubeMagicPathId != 0)
        {
            cubeSkillMoves.AddRange(MagicPathMetadataStorage.GetMagicPath(skillAttack.CubeMagicPathId)?.MagicPathMoves ?? new());
        }

        if (skillAttack.MagicPathId != 0)
        {
            cubeSkillMoves.AddRange(MagicPathMetadataStorage.GetMagicPath(skillAttack.MagicPathId)?.MagicPathMoves ?? new());
        }

        int skillMovesCount = cubeSkillMoves.Count;

        List<CoordF> effectCoords = new();
        if (skillMovesCount <= 0)
        {
            return effectCoords;
        }

        foreach (MagicPathMove magicPathMove in cubeSkillMoves)
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

    private static void HandleRegionSkill(GameSession session, SkillCast skillCast)
    {
        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            foreach (SkillAttack skillAttack in skillMotion.SkillAttacks)
            {
                if (skillAttack.SkillConditions?.Count > 0)
                {
                    foreach (SkillCondition skillCondition in skillAttack.SkillConditions)
                    {
                        SkillCast splashSkill = new(skillCondition.SkillId, skillCondition.SkillLevel, GuidGenerator.Long(), session.ServerTick, skillCast)
                        {
                            CasterObjectId = session.Player.FieldPlayer.ObjectId,
                            SkillAttack = skillAttack,
                            EffectCoords = skillCast.EffectCoords,
                            SkillObjectId = skillCast.SkillObjectId
                        };

                        if (!skillCondition.ImmediateActive)
                        {
                            if (splashSkill.IsRecoveryFromBuff())
                            {
                                HandleRegionHeal(session, splashSkill);
                                continue;
                            }

                            HandleRegionDamage(session, splashSkill);
                            continue;
                        }

                        // go to another skill condition?? might cause infinite loop
                        // HandleRegionSkill(session, splashSkill);
                    }

                    continue;
                }

                skillCast.SkillAttack = skillAttack;

                if (skillCast.IsRecoveryFromBuff())
                {
                    HandleRegionHeal(session, skillCast);
                    continue;
                }

                HandleRegionDamage(session, skillCast);
            }
        }
    }

    private static void HandleRegionHeal(GameSession session, SkillCast skillCast)
    {
        foreach (IFieldActor<Player> player in session.FieldManager.State.Players.Values)
        {
            foreach (CoordF effectCoord in skillCast.EffectCoords)
            {
                if ((player.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
                {
                    continue;
                }

                // Use RecoveryRate from skillcast.SkillAttack
                Status status = new(skillCast, player.ObjectId, session.Player.FieldPlayer.ObjectId, 1);
                player.Heal(player.Value.Session, status, 50);
            }
        }
    }

    private static void HandleRegionDamage(GameSession session, SkillCast skillCast)
    {
        List<DamageHandler> damages = new();
        bool isCrit = DamageHandler.RollCrit(session.Player.Stats[StatId.CritRate].Total);

        foreach (IFieldActor<NpcMetadata> mob in session.FieldManager.State.Mobs.Values)
        {
            foreach (CoordF effectCoord in skillCast.EffectCoords)
            {
                if ((mob.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
                {
                    continue;
                }

                DamageHandler damage = DamageHandler.CalculateDamage(skillCast, session.Player.FieldPlayer, mob, isCrit);
                mob.Damage(damage, session);

                damages.Add(damage);
            }
        }

        if (damages.Count <= 0)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, damages));
    }
}
