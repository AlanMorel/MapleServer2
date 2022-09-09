using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Tools;
using Serilog;

namespace MapleServer2.Types;

public static class RegionSkillHandler
{
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(RegionSkillHandler));

    public static void HandleEffect(FieldManager field, SkillCast skillCast)
    {
        skillCast.EffectCoords = GetEffectCoords(skillCast, field.MapId, 0, field);

        field.AddRegionSkillEffect(skillCast);

        Task removeEffectTask = RemoveEffects(field, skillCast);

        int interval = skillCast.Interval;

        if (interval <= 0)
        {
            HandleRegionSkill(field, skillCast);
            VibrateObjects(field, skillCast);
            return;
        }

        // Task to loop trough all entities in range to do damage/heal
        Task.Run(async () =>
        {
            while (!removeEffectTask.IsCompleted)
            {
                HandleRegionSkill(field, skillCast);
                VibrateObjects(field, skillCast);

                // TODO: Find the correct delay for the skill
                await Task.Delay(interval);
            }
        });
    }

    private static Task RemoveEffects(FieldManager field, SkillCast skillCast)
    {
        return Task.Run(async () =>
        {
            // TODO: Get the correct Region Skill Duration when calling chain Skills
            await Task.Delay(Math.Max(skillCast.Duration, 10));
            if (!field.RemoveRegionSkillEffect(skillCast))
            {
                Logger.Error("Failed to remove Region Skill");
            }
        });
    }

    /// <summary>
    /// Get the coordinates of the skill's effect, if needed change the offset to match the direction of the player.
    /// For skills that paint the ground, match the correct height.
    /// </summary>
    private static List<CoordF> GetEffectCoords(SkillCast skillCast, int mapId, int attackIndex, FieldManager field)
    {
        SkillAttack skillAttack = skillCast.SkillAttack;
        List<MagicPathMove> cubeMagicPathMoves = new();
        List<MagicPathMove> magicPathMoves = new();

        if (skillAttack.CubeMagicPathId != 0)
        {
            cubeMagicPathMoves.AddRange(MagicPathMetadataStorage.GetMagicPath(skillAttack.CubeMagicPathId)?.MagicPathMoves ?? new());
        }

        if (skillAttack.MagicPathId != 0)
        {
            magicPathMoves.AddRange(MagicPathMetadataStorage.GetMagicPath(skillAttack.MagicPathId)?.MagicPathMoves ?? new());
        }

        int skillMovesCount = cubeMagicPathMoves.Count + magicPathMoves.Count;

        List<CoordF> effectCoords = new();
        if (skillMovesCount <= 0)
        {
            MapMetadataStorage.GetDistanceToNextBlockBelow(mapId, Block.ClosestBlock(skillCast.Position), out MapBlock block);
            if (block is null)
            {
                effectCoords.Add(skillCast.Position);
            }
            else
            {
                effectCoords.Add(block.Coord);
            }

            return effectCoords;
        }

        // TODO: Handle case where magicPathMoves and cubeMagicPathMoves counts are > 0
        // Basically do the next if, with the later for loop

        if (magicPathMoves.Count > 0)
        {
            MagicPathMove magicPathMove = magicPathMoves[attackIndex];

            IFieldActor<NpcMetadata> parentSkillTarget = skillCast.ParentSkill?.Target;
            if (parentSkillTarget is not null)
            {
                effectCoords.Add(parentSkillTarget.Coord);

                return effectCoords;
            }

            // Rotate the offset coord and distance based on the look direction
            CoordF rotatedOffset = CoordF.From(magicPathMove.FireOffsetPosition.Length(), skillCast.LookDirection);
            CoordF distance = CoordF.From(magicPathMove.Distance, skillCast.LookDirection);

            // Create new effect coord based on offset rotation and distance
            effectCoords.Add(rotatedOffset + distance + skillCast.Position);

            return effectCoords;
        }

        // Adjust the effect on the destination/cube
        foreach (MagicPathMove cubeMagicPathMove in cubeMagicPathMoves)
        {
            CoordF offSetCoord = cubeMagicPathMove.FireOffsetPosition;

            // If false, rotate the offset based on the look direction. Example: Wizard's Tornado
            if (!cubeMagicPathMove.IgnoreAdjust)
            {
                // Rotate the offset coord based on the look direction
                CoordF rotatedOffset = CoordF.From(offSetCoord.Length(), skillCast.LookDirection);

                // Create new effect coord based on offset rotation and source coord
                effectCoords.Add(rotatedOffset + skillCast.Position);
                continue;
            }

            offSetCoord += Block.ClosestBlock(skillCast.Position);

            CoordS tempBlockCoord = offSetCoord.ToShort();

            // Set the height to the max allowed, which is one block above the cast coord.
            tempBlockCoord.Z += Block.BLOCK_SIZE * 2;

            // Find the first block below the effect coord
            bool foundBlock = field.Navigator.FindFirstCoordSBelow(offSetCoord.ToShort(), out CoordS resultCoord);

            if (!foundBlock || offSetCoord.Z - resultCoord.Z > Block.BLOCK_SIZE * 2)
            {
                continue;
            }

            MapBlock blockBelow = MapMetadataStorage.GetMapBlock(mapId, Block.ClosestBlock(resultCoord));
            int distanceToNextBlockBelow = (int) (offSetCoord.Z - resultCoord.Z);

            //// If the block is null or the distance from the cast effect Z height is greater than two blocks, continue
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
            offSetCoord.Z += Block.BLOCK_SIZE + 1;

            effectCoords.Add(offSetCoord);
        }

        return effectCoords;
    }

    private static void HandleRegionSkill(FieldManager field, SkillCast skillCast)
    {
        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            foreach (SkillAttack skillAttack in skillMotion.SkillAttacks)
            {
                if (skillAttack.SkillConditions?.Count > 0)
                {
                    foreach (SkillCondition skillCondition in skillAttack.SkillConditions)
                    {
                        for (int i = 0; i < skillCondition.SkillId.Length; ++i)
                        {
                            SkillCast splashSkill = new(skillCondition.SkillId[i], skillCondition.SkillLevel[i], GuidGenerator.Long(), skillCast.ServerTick, skillCast)
                            {
                                SkillAttack = skillAttack,
                                EffectCoords = skillCast.EffectCoords,
                                SkillObjectId = skillCast.SkillObjectId,
                                Owner = skillCast.Owner,
                            };
                            if (!splashSkill.MetadataExists)
                            {
                                return;
                            }

                            if (!skillCondition.ImmediateActive)
                            {
                                if (splashSkill.IsRecoveryFromBuff())
                                {
                                    HandleRegionHeal(field, splashSkill);
                                    continue;
                                }

                                HandleRegionDamage(field, splashSkill, skillCondition);
                                continue;
                            }
                        }
                        // go to another skill condition?? might cause infinite loop
                        // HandleRegionSkill(session, splashSkill);
                    }

                    continue;
                }

                skillCast.SkillAttack = skillAttack;

                if (!skillCast.MetadataExists)
                {
                    return;
                }

                if (skillCast.IsRecoveryFromBuff())
                {
                    HandleRegionHeal(field, skillCast);
                    continue;
                }

                HandleRegionDamage(field, skillCast, null);
            }
        }
    }

    private static void HandleRegionHeal(FieldManager field, SkillCast skillCast)
    {
        foreach (IFieldActor<Player> player in field.State.Players.Values)
        {
            foreach (CoordF effectCoord in skillCast.EffectCoords)
            {
                if ((player.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
                {
                    continue;
                }

                // Use RecoveryRate from skillcast.SkillAttack
                Status status = new(skillCast, player.ObjectId, skillCast.Caster.ObjectId, 1);
                player.Heal(player.Value.Session, status, 50);
            }
        }
    }

    private static void HandleRegionDamage(FieldManager field, SkillCast skillCast, SkillCondition condition)
    {
        if (skillCast.Caster is not Character caster)
        {
            // TODO: Handle NPCs/Triggers sending skills
            field.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, new()));
            return;
        }

        List<DamageHandler> damages = new();

        foreach (IFieldActor<NpcMetadata> mob in field.State.Mobs.Values)
        {
            foreach (CoordF effectCoord in skillCast.EffectCoords)
            {
                if ((mob.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
                {
                    continue;
                }

                DamageHandler damage = DamageHandler.CalculateDamage(skillCast, caster, mob);
                mob.Damage(damage, caster.Value.Session);

                damages.Add(damage);

                EffectTriggers parameters = new()
                {
                    Caster = skillCast.Caster,
                    Owner = skillCast.Owner,
                    Target = mob
                };

                skillCast.Owner?.SkillTriggerHandler?.FireTriggerSkill(condition, skillCast.ParentSkill, parameters);
                parameters.Caster.SkillTriggerHandler.SkillTrigger(mob, skillCast);
            }
        }

        if (damages.Count <= 0)
        {
            return;
        }

        field.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, damages));
    }

    private static void VibrateObjects(FieldManager field, SkillCast skillCast)
    {
        RangeProperty rangeProperty = skillCast.SkillAttack.RangeProperty;
        foreach ((string objectId, MapVibrateObject metadata) in field.State.VibrateObjects)
        {
            foreach (CoordF effectCoord in skillCast.EffectCoords)
            {
                if ((metadata.Position - effectCoord).Length() > rangeProperty.Distance)
                {
                    continue;
                }

                field.BroadcastPacket(VibratePacket.Vibrate(objectId, skillCast));
            }
        }
    }
}
