using Maple2Storage.Enums;
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

    public static void HandleEffect(FieldManager field, SkillCast skillCast, IFieldActor? target = null)
    {
        if (skillCast.Owner is not null && skillCast.Caster is not null)
        {
            skillCast.Caster.SkillTriggerHandler.FireEvents(new(skillCast.Owner, target, skillCast.Caster), EffectEvent.OnSkillCasted,
                skillCast.SkillId);
        }

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
            effectCoords.Add(skillCast.Position);

            return effectCoords;
        }

        // TODO: Handle case where magicPathMoves and cubeMagicPathMoves counts are > 0
        // Basically do the next if, with the later for loop

        if (magicPathMoves.Count > 0)
        {
            MagicPathMove magicPathMove = magicPathMoves[attackIndex];

            IFieldActor? parentSkillTarget = skillCast.ParentSkill?.Target;
            if (parentSkillTarget is not null)
            {
                effectCoords.Add(parentSkillTarget.Coord);

                return effectCoords;
            }

            // Rotate the offset coord and distance based on the look direction
            CoordF rotatedOffset = CoordF.Rotate(magicPathMove.FireOffsetPosition, skillCast.LookDirection);
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
                CoordF rotatedOffset = -CoordF.Rotate(offSetCoord, skillCast.Caster.LookDirection);

                // Create new effect coord based on offset rotation and source coord
                effectCoords.Add(rotatedOffset + skillCast.Position);
                continue;
            }

            offSetCoord = -CoordF.Rotate(offSetCoord, skillCast.LookDirection);

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
        List<DamageHandler> damages = new();

        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            foreach (SkillAttack skillAttack in skillMotion.SkillAttacks)
            {
                int hitsRemaining = skillAttack.TargetCount;

                skillCast.SkillAttack = skillAttack;

                switch (skillAttack.RangeProperty.ApplyTarget)
                {
                    case ApplyTarget.None:
                        break;
                    case ApplyTarget.Enemy:
                        RegionHitEnemy(field, skillCast, ref hitsRemaining, damages);

                        break;
                    case ApplyTarget.Ally:
                        RegionHitAlly(field, skillCast, ref hitsRemaining, damages);

                        break;
                    case ApplyTarget.Player1:
                    case ApplyTarget.Player2:
                    case ApplyTarget.Player3:
                    case ApplyTarget.Player4:
                        RegionHitPlayer(field, skillCast, ref hitsRemaining, damages);

                        break;
                    case ApplyTarget.HungryMobs:
                        RegionHitHungryMobs(field, skillCast, ref hitsRemaining, damages);

                        break;
                }
            }
        }

        field.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, damages));
    }

    private static void RegionHitAlly(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages)
    {
        foreach (IFieldActor<Player> player in field.State.Players.Values)
        {
            if (hitsRemaining == 0)
            {
                return;
            }

            RegionHitTarget(field, skillCast, ref hitsRemaining, damages, player, false);
        }
    }

    private static void RegionHitEnemy(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages)
    {
        foreach (IFieldActor<NpcMetadata> mob in field.State.Mobs.Values)
        {
            if (hitsRemaining == 0)
            {
                return;
            }

            RegionHitTarget(field, skillCast, ref hitsRemaining, damages, mob, true);
        }
    }

    private static void RegionHitPlayer(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages)
    {
        foreach (IFieldActor<Player> player in field.State.Players.Values)
        {
            if (hitsRemaining == 0)
            {
                return;
            }

            RegionHitTarget(field, skillCast, ref hitsRemaining, damages, player, true);
        }
    }

    private static void RegionHitHungryMobs(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages) { }

    private static void RegionHitTarget(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages, IFieldActor target,
        bool damaging)
    {
        if (hitsRemaining == 0)
        {
            return;
        }

        Servers.Game.GameSession session = null;
        IFieldActor caster = skillCast.Caster;

        if (caster is Character character)
        {
            session = character.Value.Session;
        }

        foreach (CoordF effectCoord in skillCast.EffectCoords)
        {
            if ((target.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
            {
                continue;
            }

            ConditionSkillTarget castInfo = new(caster, target, caster);
            bool hitTarget = false;
            bool hitCrit = false;
            bool hitMissed = false;

            if (skillCast.SkillAttack.DamageProperty.DamageRate != 0)
            {
                for (int i = 0; i < skillCast.SkillAttack.DamageProperty.Count; ++i)
                {
                    DamageHandler damage = DamageHandler.CalculateDamage(skillCast, caster, target);
                    target.Damage(damage, session);

                    damages.Add(damage);

                    hitCrit |= damage.HitType == Enums.HitType.Critical;
                    hitMissed |= damage.HitType == Enums.HitType.Miss;
                    hitTarget |= damage.HitType != Enums.HitType.Miss;
                }
            }

            if (damaging)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(10);

                    if (hitCrit)
                    {
                        caster.SkillTriggerHandler.FireEvents(castInfo, EffectEvent.OnOwnerAttackCrit, skillCast.SkillId);
                    }

                    if (hitMissed)
                    {
                        caster.SkillTriggerHandler.FireEvents(castInfo, EffectEvent.OnAttackMiss, skillCast.SkillId);
                        target.SkillTriggerHandler.FireEvents(new(target, caster, target), EffectEvent.OnEvade, skillCast.SkillId);
                    }

                    caster.SkillTriggerHandler.FireEvents(castInfo, EffectEvent.OnOwnerAttackHit, skillCast.SkillId);
                    target.SkillTriggerHandler.FireEvents(new(target, caster, target, caster), EffectEvent.OnAttacked, skillCast.SkillId);
                });
            }

            castInfo.Owner.SkillTriggerHandler.FireTriggerSkills(skillCast.SkillAttack.SkillConditions, skillCast, castInfo, -1, hitTarget);

            hitsRemaining--;

            return;
        }
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
