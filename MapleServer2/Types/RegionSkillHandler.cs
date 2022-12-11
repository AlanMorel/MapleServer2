using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.Network;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Serilog;

namespace MapleServer2.Types;

public static class RegionSkillHandler
{
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(RegionSkillHandler));

    public static void CastRegionSkill(FieldManager field, SkillCast skillCast, int fireCount, int removeDelay, int interval, IFieldActor? target = null)
    {
        SkillCast regionCast = new(skillCast.SkillId, skillCast.SkillLevel, skillCast.SkillSn, skillCast.ServerTick, skillCast)
        {
            Rotation = skillCast.Rotation,
            Direction = skillCast.Direction,
            LookDirection = skillCast.LookDirection,
            Duration = skillCast.Duration,
            SkillAttack = skillCast.SkillAttack,
            ParentSkill = skillCast
        };

        List<SkillMotion> motions = skillCast.GetSkillMotions();

        long magicPath = skillCast.MagicPath;

        if (magicPath == 0 && motions.Count > 0 && motions[0].SkillAttacks.Count > 0)
        {
            foreach (SkillAttack attack in motions[0].SkillAttacks)
            {
                if (attack.MagicPathId != 0)
                {
                    magicPath = attack.MagicPathId;
                }
            }
        }

        int lookAtType = 0;

        regionCast.EffectCoords = GetCoords(skillCast.Position, skillCast.LookDirection, magicPath, skillCast.SkillAttack?.CubeMagicPathId ?? 0, field, out lookAtType);

        if (lookAtType == 2)
        {
            regionCast.Rotation = CoordF.Rotate(CoordF.From(0, -1, 0), regionCast.LookDirection);
        }

        field.AddRegionSkillEffect(regionCast);

        IFieldActor? firstTarget = HandleRegionSkillChaining(field, regionCast);

        if (firstTarget is not null)
        {
            CoordF offset = firstTarget.Coord - regionCast.EffectCoords[0];

            regionCast.Rotation = offset / (float) Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y + offset.Z * offset.Z);
        }

        field.FieldTaskScheduler.QueueTask(new(Math.Max(removeDelay, 1))
        {
            Executions = 1
        }, (currentTick, task) => { CleanUpRegionSkill(field, regionCast); return -1; });

        if (skillCast.Owner is not null && skillCast.Caster is not null)
        {
            skillCast.Caster.SkillTriggerHandler.FireEvents(target, null, EffectEvent.OnSkillCasted, skillCast.SkillId);
        }

        HandleRegionSkill(field, regionCast);

        if (interval == 0)
        {
            for (int i = 0; i < fireCount - 1; ++i)
            {
                HandleRegionSkill(field, regionCast);
            }

            return;
        }

        if (fireCount == 1)
        {
            return;
        }

        field.FieldTaskScheduler.QueueTask(new(interval)
        {
            Executions = fireCount - 1
        }, (currentTick, task) => TickRegionSkill(field, regionCast));
    }

    private static long TickRegionSkill(FieldManager field, SkillCast regionCast)
    {
        HandleRegionSkill(field, regionCast);

        return 0;
    }

    private static void CleanUpRegionSkill(FieldManager field, SkillCast regionCast)
    {
        if (!field.RemoveRegionSkillEffect(regionCast))
        {
            Logger.Error("Failed to remove Region Skill");
        }
    }

    private static bool AdjustCoord(FieldManager field, MapMetadata mapMetadata, MagicPathMove move, CoordF position, short lookDirection, out CoordF adjusted)
    {
        adjusted = new();

        CoordF offSetCoord = move.FireOffsetPosition;

        // If false, rotate the offset based on the look direction. Example: Wizard's Tornado
        if (!move.IgnoreAdjust)
        {
            // Rotate the offset coord based on the look direction
            CoordF rotatedOffset = -CoordF.Rotate(offSetCoord, lookDirection);

            // Create new effect coord based on offset rotation and source coord
            adjusted = rotatedOffset + position;

            return true;
        }

        offSetCoord = -CoordF.Rotate(offSetCoord, lookDirection);

        offSetCoord += Block.ClosestBlock(position);

        CoordS tempBlockCoord = offSetCoord.ToShort();

        // Set the height to the max allowed, which is one block above the cast coord.
        tempBlockCoord.Z += Block.BLOCK_SIZE * 2;

        CoordS resultCoord = new();

        // Find the first block below the effect coord
        bool foundBlock = field?.Navigator?.FindFirstCoordSBelow(offSetCoord.ToShort(), out resultCoord) ?? false;

        if (!foundBlock || offSetCoord.Z - resultCoord.Z > Block.BLOCK_SIZE * 2)
        {
            return false;
        }

        CoordF closest = Block.ClosestBlock(resultCoord);

        if (!mapMetadata.Blocks.TryGetValue(closest, out MapBlock? blockBelow))
        {
            return false;
        }

        int distanceToNextBlockBelow = (int) (offSetCoord.Z - resultCoord.Z);

        //// If the block is null or the distance from the cast effect Z height is greater than two blocks, continue
        if (blockBelow is null || distanceToNextBlockBelow > Block.BLOCK_SIZE * 2)
        {
            return false;
        }

        // If there is a block above, continue
        if (mapMetadata.Blocks.ContainsKey(closest + CoordF.From(0, 0, Block.BLOCK_SIZE)))
        {
            return false;
        }

        // If block is liquid, continue
        if (MapMetadataStorage.IsLiquidBlock(blockBelow))
        {
            return false;
        }

        // Since this is the block below, add 150 units to the Z coord so the effect is above the block
        offSetCoord = blockBelow.Coord.ToFloat();
        offSetCoord.Z += Block.BLOCK_SIZE + 1;

        adjusted = offSetCoord;

        return true;
    }

    public static List<CoordF> GetCoords(CoordF position, short lookDirection, long magicPathId, long cubeMagicPathId, FieldManager field, out int lookAtType)
    {
        lookAtType = 0;

        List<CoordF> cubeEffectCoords = new();

        MapMetadata? mapMetadata = field.Metadata;

        if (mapMetadata is null)
        {
            return cubeEffectCoords;
        }

        if (cubeMagicPathId != 0)
        {
            List<MagicPathMove>? cubeMagicPath = MagicPathMetadataStorage.GetMagicPath(cubeMagicPathId)?.MagicPathMoves;

            if (cubeMagicPath is null)
            {
                return cubeEffectCoords;
            }

            // Adjust the effect on the destination/cube
            foreach (MagicPathMove cubeMagicPathMove in cubeMagicPath)
            {
                lookAtType = cubeMagicPathMove.LookAtType;

                if (AdjustCoord(field, mapMetadata, cubeMagicPathMove, position, lookDirection, out CoordF adjusted))
                {
                    cubeEffectCoords.Add(adjusted);
                }
            }

            if (magicPathId == 0 || cubeEffectCoords.Count == 0)
            {
                return cubeEffectCoords;
            }
        }
        else
        {
            cubeEffectCoords.Add(position);
        }

        if (magicPathId == 0)
        {
            return cubeEffectCoords;
        }

        List<CoordF> effectCoords = new();
        List<MagicPathMove>? magicPath = MagicPathMetadataStorage.GetMagicPath(magicPathId)?.MagicPathMoves;

        if (magicPath is null)
        {
            return effectCoords;
        }

        for (int i = 0; i < cubeEffectCoords.Count; ++i)
        {
            foreach (MagicPathMove magicPathMove in magicPath)
            {
                lookAtType = magicPathMove.LookAtType;

                if (AdjustCoord(field, mapMetadata, magicPathMove, cubeEffectCoords[i], lookDirection, out CoordF adjusted))
                {
                    effectCoords.Add(adjusted);
                }
            }
        }

        return effectCoords;
    }

    private static IFieldActor? HandleRegionSkillChaining(FieldManager field, SkillCast skillCast)
    {
        Character? character = skillCast.Caster as Character;

        if (character is null)
        {
            return null;
        }

        Session? session = character.Value?.Session;

        if (session is null)
        {
            return null;
        }

        List<IFieldActor> targets = new();

        List<long> uid = new();
        List<int> atkCount = new();
        List<int> sourceId = new();
        List<int> targetId = new();
        List<short> animation = new();

        bool hasChaining = false;
        SkillAttack? chainingAttack = null;
        int attackPoint = 0;

        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            for (int i = 0; i < skillMotion.SkillAttacks.Count; ++i)
            {
                SkillAttack skillAttack = skillMotion.SkillAttacks[i];

                if (skillCast.EffectCoords.Count > 0 && skillAttack.ArrowProperty.BounceType == BounceType.Unknown2)
                {
                    hasChaining = true;
                    attackPoint = i;
                    chainingAttack = skillAttack;

                    CoordF position = skillCast.EffectCoords[0];

                    for (int j = 0; j < skillAttack.ArrowProperty.BounceCount - 1 && targets.Count == j; ++j)
                    {
                        IFieldActor? closest = null;

                        ProcessApplyTarget(field, skillAttack.RangeProperty.ApplyTarget, (target) =>
                        {
                            ProcessChainTarget(field, skillAttack, targets, target, position, ref closest);

                            return targets.Count <= j;
                        });

                        if (closest is not null)
                        {
                            position = closest.Coord;
                            targets.Add(closest);
                        }
                    }
                }
            }
        }

        if (!hasChaining)
        {
            return null;
        }

        for (int i = 0; i < targets.Count; ++i)
        {
            uid.Add(i > 0 ? (((long) skillCast.SkillObjectId << 32) + 1 + i) : 0);
            atkCount.Add(2 + i);
            sourceId.Add(skillCast.SkillObjectId);
            targetId.Add(targets[i].ObjectId);
            animation.Add((short) (i << 0));
        }

        if (targets.Count == 0)
        {
            uid.Add(0);
            atkCount.Add(2);
            sourceId.Add(skillCast.SkillObjectId);
            targetId.Add(0);
            animation.Add(0);
        }

        field.BroadcastPacket(SkillDamagePacket.SyncDamage(skillCast, skillCast.EffectCoords[0], skillCast.Rotation, character, sourceId, (byte) atkCount.Count, atkCount,
            targetId, animation, true, uid));

        if (chainingAttack is null || targets.Count == 0)
        {
            return targets.Count != 0 ? targets[0] : null;
        }

        skillCast.Position = skillCast.EffectCoords[0];
        int currentTarget = 0;
        MagicPathMove pathMove = MagicPathMetadataStorage.GetMagicPath(chainingAttack.MagicPathId)?.MagicPathMoves[0] ?? new();

        field.FieldTaskScheduler.QueueTask(new(10)
        {
            Duration = 25000,
            Executions = -1
        }, (currentTick, task) =>
        {
            float distance = CoordF.Distance(skillCast.Position, targets[currentTarget].Coord);
            float nextTickDistance = pathMove.Velocity * 0.01f;
            skillCast.Rotation = (targets[currentTarget].Coord - skillCast.Position) / distance;

            if (distance < nextTickDistance)
            {
                skillCast.Position = targets[currentTarget].Coord;

                SkillHandler.HandleDamage(skillCast, targets[currentTarget], 1, attackPoint, currentTarget + 2, skillCast.Position, skillCast.Rotation);
                // do damage

                ++currentTarget;

                return currentTarget < targets.Count ? 0 : -1;
            }

            skillCast.Position += skillCast.Rotation * nextTickDistance;

            return 0;
        });

        return targets.Count != 0 ? targets[0] : null;
    }

    private static void HandleRegionSkill(FieldManager field, SkillCast skillCast)
    {
        VibrateObjects(field, skillCast);

        List<DamageHandler> damages = new();

        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            foreach (SkillAttack skillAttack in skillMotion.SkillAttacks)
            {
                int hitsRemaining = skillAttack.TargetCount;

                skillCast.SkillAttack = skillAttack;

                ProcessApplyTarget(field, skillAttack.RangeProperty.ApplyTarget, (target) =>
                {
                    ProcessAttackApplyTarget(field, skillCast, ref hitsRemaining, damages, target, true);
                    return hitsRemaining > 0;
                });
            }
        }

        if (damages.Count == 0)
        {
            return;
        }

        field.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, damages));
    }

    private static void ProcessApplyTarget(FieldManager field, ApplyTarget target, Func<IFieldActor, bool> callback)
    {
        switch (target)
        {
            case ApplyTarget.None:
                break;
            case ApplyTarget.Enemy:
                ProcessAttackTargetEnemy(field, callback);

                break;
            case ApplyTarget.Ally:
                ProcessAttackTargetAlly(field, callback);

                break;
            case ApplyTarget.Player1:
            case ApplyTarget.Player2:
            case ApplyTarget.Player3:
            case ApplyTarget.Player4:
                ProcessAttackTargetPlayer(field, callback);

                break;
            case ApplyTarget.HungryMobs:
                ProcessAttackTargetHungryMobs(field, callback);

                break;
        }
    }

    private static void ProcessAttackTargetAlly(FieldManager field, Func<IFieldActor, bool> callback)
    {
        foreach (IFieldActor<Player> player in field.State.Players.Values)
        {
            if (!callback(player))
            {
                return;
            }
        }
    }

    private static void ProcessAttackTargetEnemy(FieldManager field, Func<IFieldActor, bool> callback)
    {
        foreach (IFieldActor<NpcMetadata> mob in field.State.Mobs.Values)
        {
            if (!callback(mob))
            {
                return;
            }
        }
    }

    private static void ProcessAttackTargetPlayer(FieldManager field, Func<IFieldActor, bool> callback)
    {
        foreach (IFieldActor<Player> player in field.State.Players.Values)
        {
            if (!callback(player))
            {
                return;
            }
        }
    }

    private static void ProcessAttackTargetHungryMobs(FieldManager field, Func<IFieldActor, bool> callback) { }

    private static void ProcessChainTarget(FieldManager field, SkillAttack skillAttack, List<IFieldActor> targets, IFieldActor target, CoordF position, ref IFieldActor? closest)
    {
        int range = targets.Count != 0 ? skillAttack.ArrowProperty.BounceRadius : skillAttack.RangeProperty.Distance;

        if ((target.Coord - position).Length() > range)
        {
            return;
        }

        if (skillAttack.ArrowProperty.BounceOverlap)
        {
            if (targets.Count > 0 && target == targets.Last())
            {
                return;
            }
        }
        else
        {
            for (int i = 0; i < targets.Count; ++i)
            {
                if (targets[i] == target)
                {
                    return;
                }
            }
        }

        if (closest is null)
        {
            closest = target;

            return;
        }

        CoordF closestOffset = closest.Coord - position;
        float closestDistance = CoordF.Dot(closestOffset, closestOffset);
        CoordF offset = target.Coord - position;
        float distance = CoordF.Dot(offset, offset);

        if (distance < closestDistance)
        {
            closest = target;
        }
    }

    private static void ProcessAttackApplyTarget(FieldManager field, SkillCast skillCast, ref int hitsRemaining, List<DamageHandler> damages, IFieldActor target,
        bool damaging)
    {
        if (hitsRemaining == 0)
        {
            return;
        }

        GameSession? session = null;
        IFieldActor? caster = skillCast.Caster;

        if (caster is Character character)
        {
            session = character.Value.Session;
        }

        if (skillCast.SkillAttack is null)
        {
            return;
        }

        foreach (CoordF effectCoord in skillCast.EffectCoords)
        {
            if ((target.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
            {
                continue;
            }

            ConditionSkillTarget castInfo = new(caster, target, caster, caster);
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
                target.SkillTriggerHandler.OnAttacked(caster, skillCast.SkillId, hitTarget, hitCrit, hitMissed, false);
            }

            castInfo.Owner?.SkillTriggerHandler.FireTriggerSkills(skillCast.SkillAttack.SkillConditions, skillCast, castInfo, -1, hitTarget);

            hitsRemaining--;

            return;
        }
    }

    private static void VibrateObjects(FieldManager field, SkillCast skillCast)
    {
        if (skillCast.SkillAttack is null)
        {
            return;
        }

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
