using Maple2.PathEngine;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using MapleServer2.PacketHandlers.Game;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Serilog;

namespace MapleServer2.Types;

public static class RegionSkillHandler
{
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(RegionSkillHandler));

    public static void CastRegionSkill(FieldManager field, SkillCast skillCast, int fireCount, int delay, int removeDelay, int interval, IFieldActor? target = null, SkillCondition? trigger = null, bool isSensor = false)
    {
        SkillCast regionCast = new(skillCast.SkillId, skillCast.SkillLevel, skillCast.SkillSn, skillCast.ServerTick, skillCast)
        {
            Rotation = skillCast.UseDirection ? skillCast.Rotation : default,
            Direction = skillCast.UseDirection ? skillCast.Direction : default,
            LookDirection = skillCast.UseDirection ? skillCast.LookDirection : default,
            Duration = skillCast.Duration,
            SkillAttack = skillCast.SkillAttack,
            ParentSkill = skillCast,
            UseDirection = skillCast.UseDirection
        };

        List<SkillMotion> motions = skillCast.GetSkillMotions();

        long magicPath = skillCast.MagicPath;
        SkillAttack? skillAttack = null;

        if (magicPath == 0 && motions.Count > 0 && motions[0].SkillAttacks.Count > 0)
        {
            foreach (SkillAttack attack in motions[0].SkillAttacks)
            {
                skillAttack = attack;

                if (attack.MagicPathId != 0)
                {
                    magicPath = attack.MagicPathId;
                }
            }
        }

        if (motions.Count > 0 && motions[0].SplashLifeTick != 0)
        {
            removeDelay = motions[0].SplashLifeTick;
        }

        int lookAtType = 0;
        CoordF skillPosition = skillCast.Position;

        regionCast.EffectCoords = GetCoords(skillCast.Position, skillCast.LookDirection, skillCast.UseDirection, magicPath, skillCast.SkillAttack?.CubeMagicPathId ?? 0, field, out lookAtType, out skillPosition);

        if (lookAtType == 2)
        {
            regionCast.Rotation = CoordF.Rotate(CoordF.From(0, -1, 0), regionCast.LookDirection);
        }

        field.AddRegionSkillEffect(regionCast);

        if (isSensor)
        {
            HandleSensorSkill(field, regionCast, skillPosition, fireCount, delay, removeDelay, interval, target);

            return;
        }

        IFieldActor? firstTarget = HandleRegionSkillChaining(field, regionCast, out bool isChained);

        if (firstTarget is not null)
        {
            CoordF offset = firstTarget.Coord - regionCast.EffectCoords[0];

            regionCast.Rotation = offset / (float) Math.Sqrt(offset.X * offset.X + offset.Y * offset.Y + offset.Z * offset.Z);
        }

        bool hasLinkSkills = InitializeLinkSkills(field, regionCast, interval, target);
        bool dependsOnCasterState = skillCast.Caster is not null && (trigger?.DependOnCasterState ?? false);

        if (dependsOnCasterState && skillCast.Caster is not null)
        {
            Func<long, TriggerTask, long> tickCallback = (currentTick, task) =>
            {
                TickRegionSkill(field, regionCast);

                bool isSkillAnimation = skillCast.Caster.Animation == 16;
                SkillCast? currentSkill = skillCast.Caster.AnimationHandler.CurrentSkill;
                bool skillMatchesCurrent = currentSkill?.SkillId == skillCast.ParentSkill?.SkillId;

                return (isSkillAnimation && (skillMatchesCurrent || currentSkill is null)) || skillMatchesCurrent ? 0 : -1;
            };

            Action<long, TriggerTask> cleanCallback = (currentTick, task) => CleanUpRegionSkill(field, regionCast, hasLinkSkills);

            field.FieldTaskScheduler.QueueTask(new(interval)
            {
                Executions = fireCount - 1
            }, tickCallback, cleanCallback);
        }
        else
        {
            field.FieldTaskScheduler.QueueTask(new(Math.Max(removeDelay, delay + 10))
            {
                Executions = 1
            }, (currentTick, task) => { CleanUpRegionSkill(field, regionCast, hasLinkSkills); return -1; });
        }

        if (skillCast.Caster is not null)
        {
            skillCast.Caster.SkillTriggerHandler.FireEvents(target, null, EffectEvent.OnSkillCasted, skillCast.SkillId);
            skillCast.Caster.SkillTriggerHandler.LinkSkillCasted(skillCast.SkillId);
        }

        if (isChained)
        {
            return;
        }

        if (fireCount == 0)
        {
            return;
        }

        if (delay == 0)
        {
            HandleRegionSkillTicks(field, regionCast, fireCount, interval, dependsOnCasterState);

            return;
        };

        field.FieldTaskScheduler.QueueTask(new(delay)
        {
            Executions = 1
        }, (currentTick, task) => HandleRegionSkillTicks(field, regionCast, fireCount, interval, dependsOnCasterState));
    }

    private static long HandleRegionSkillTicks(FieldManager field, SkillCast regionCast, int fireCount, int interval, bool dependsOnCasterState)
    {
        HandleRegionSkill(field, regionCast);

        if (fireCount == 1 || dependsOnCasterState)
        {
            return 0;
        }

        field.FieldTaskScheduler.QueueTask(new(interval)
        {
            Executions = fireCount - 1
        }, (currentTick, task) => TickRegionSkill(field, regionCast));

        return 0;
    }

    private static long TickRegionSkill(FieldManager field, SkillCast regionCast)
    {
        HandleRegionSkill(field, regionCast);

        return 0;
    }

    private static void CleanUpRegionSkill(FieldManager field, SkillCast regionCast, bool isLinked)
    {
        if (isLinked)
        {
            regionCast.Caster?.SkillTriggerHandler.RemoveListeningRegionLinkSkill(regionCast);
        }

        if (!field.RemoveRegionSkillEffect(regionCast))
        {
            Logger.Error("Failed to remove Region Skill");
        }
    }

    private static bool AdjustCoord(FieldManager field, MapMetadata mapMetadata, MagicPathMove move, CoordF position, short lookDirection, out CoordF adjusted)
    {
        adjusted = new();

        CoordF offSetCoord = move.FireOffsetPosition;

        bool foundBlock;

        // If false, rotate the offset based on the look direction. Example: Wizard's Tornado
        if (!move.IgnoreAdjust || !move.Align)
        {
            // Rotate the offset coord based on the look direction
            CoordF rotatedOffset = -CoordF.Rotate(offSetCoord, lookDirection);

            // Create new effect coord based on offset rotation and source coord
            adjusted = rotatedOffset + position;

            if (!move.Align)
            {
                CoordS closestBlockPosShort = new();

                foundBlock = field?.Navigator?.FindFirstCoordSBelow(adjusted.ToShort(), out closestBlockPosShort) ?? false;

                CoordF closestBlockPos = Block.ClosestBlock(closestBlockPosShort);

                adjusted.Z = closestBlockPos.Z + Block.BLOCK_SIZE + 1;
            }

            return true;
        }

        offSetCoord += Block.ClosestBlock(position);

        CoordS tempBlockCoord = offSetCoord.ToShort();

        // Set the height to the max allowed, which is one block above the cast coord.
        tempBlockCoord.Z += Block.BLOCK_SIZE * 2;

        CoordS resultCoord = new();

        // Find the first block below the effect coord
        foundBlock = field?.Navigator?.FindFirstCoordSBelow(offSetCoord.ToShort(), out resultCoord) ?? false;

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

    public static List<CoordF> GetCoords(CoordF position, short lookDirection, bool useDirection, long magicPathId, long cubeMagicPathId, FieldManager field, out int lookAtType, out CoordF skillPosition)
    {
        lookAtType = 0;
        skillPosition = position;

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

            if (cubeEffectCoords.Count == 1)
            {
                skillPosition = cubeEffectCoords[0];
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

                if (AdjustCoord(field, mapMetadata, magicPathMove, cubeEffectCoords[i], useDirection ? lookDirection : default, out CoordF adjusted))
                {
                    effectCoords.Add(adjusted);
                }
            }
        }

        return effectCoords;
    }

    private static bool CheckForEffect(int effectId, IFieldActor target, IFieldActor? caster, bool defaultValue)
    {
        if (effectId == 0)
        {
            return defaultValue;
        }

        if (target.AdditionalEffects.HasEffect(effectId))
        {
            return true;
        }

        if (caster is null)
        {
            return false;
        }

        return caster.AdditionalEffects.HasEffect(effectId, 0, ConditionOperator.GreaterEquals, 0, caster);
    }

    private static void HandleSensorSkill(FieldManager field, SkillCast skillCast, CoordF position, int fireCount, int delay, int removeDelay, int interval, IFieldActor? target)
    {
        RangeProperty sensorProperty = skillCast.GetCurrentLevel().SensorProperty;

        long startTick = field.FieldTaskScheduler.CurrentTick;
        int lastInterval = 0;
        const int delta = 10;

        //if (sensorProperty.TargetHasBuffOwner)
        //{
        //    target = skillCast.Caster;
        //}

        field.FieldTaskScheduler.QueueTask(new(delta)
        {
            Executions = -1
        }, (currentTick, task) =>
        {
            lastInterval += delta;
            removeDelay -= delta;

            bool senseTargetTouch = !sensorProperty.SensorForceInvokeByInterval;
            bool intervalActivated = sensorProperty.SensorForceInvokeByInterval && lastInterval >= interval;
            bool targetBuffConditionPassed = false;

            if (sensorProperty.TargetSelectType == 2 && target is not null)
            {
                senseTargetTouch = false;
                targetBuffConditionPassed = CheckForEffect(sensorProperty.TargetHasBuffID, target, skillCast.Caster, true);
                targetBuffConditionPassed &= !CheckForEffect(sensorProperty.TargetHasNotBuffID, target, skillCast.Caster, false);
            }

            bool activated = intervalActivated || targetBuffConditionPassed || (senseTargetTouch && ShouldSensorActivate(field, skillCast, position, sensorProperty));

            if ((intervalActivated && sensorProperty.SensorForceInvokeByInterval) || activated)
            {
                lastInterval = 0;
                fireCount--;
            }

            if (activated)
            {
                HandleRegionSkill(field, skillCast);
            }

            return fireCount > 0 && removeDelay > 0 ? 0 : -1;
        }, (currentTick, task) =>
        {
            CleanUpRegionSkill(field, skillCast, false);
        });
    }

    private static bool ShouldSensorActivate(FieldManager field, SkillCast skillCast, CoordF position, RangeProperty sensorProperty)
    {
        bool sensorActivated = false;

        ProcessApplyTarget(field, sensorProperty.ApplyTarget, (target) =>
        {
            sensorActivated = ProcessSensorTarget(field, skillCast, position, sensorProperty, target);

            return !sensorActivated;
        });

        return sensorActivated;
    }

    private static bool ProcessSensorTarget(FieldManager field, SkillCast skillCast, CoordF position, RangeProperty sensorProperty, IFieldActor target)
    {
        return (target.Coord - position).Length() <= sensorProperty.Distance;
    }

    private static IFieldActor? HandleRegionSkillChaining(FieldManager field, SkillCast skillCast, out bool isChained)
    {
        Character? character = skillCast.Caster as Character;

        isChained = false;

        if (character is null)
        {
            return null;
        }

        GameSession? session = character.Value?.Session;

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

        SkillAttack? chainingAttack = null;
        int attackPoint = 0;
        MagicPathMove? pathMove = null;

        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            for (int i = 0; i < skillMotion.SkillAttacks.Count; ++i)
            {
                SkillAttack skillAttack = skillMotion.SkillAttacks[i];

                MagicPathMove? path = MagicPathMetadataStorage.GetMagicPath(skillAttack.MagicPathId)?.MagicPathMoves[0];

                if (skillCast.EffectCoords.Count > 0 && path?.Velocity > 0)
                {
                    isChained = true;
                    attackPoint = i;
                    chainingAttack = skillAttack;
                    pathMove = path;

                    CoordF position = skillCast.EffectCoords[0];

                    for (int j = 0; j < skillAttack.ArrowProperty.BounceCount + 1 && targets.Count == j; ++j)
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

        if (!isChained)
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
            targetId, animation, isChained, uid));

        if (chainingAttack is null || targets.Count == 0 || pathMove is null)
        {
            return targets.Count != 0 ? targets[0] : null;
        }

        skillCast.Position = skillCast.EffectCoords[0];
        int currentTarget = 0;

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

                SkillHandler.HandleDamage(skillCast, targets[currentTarget], attackPoint, currentTarget + 2, skillCast.Position, skillCast.Rotation);
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
                    ProcessAttackApplyTarget(field, skillCast, ref hitsRemaining, damages, target, skillAttack.RangeProperty.ApplyTarget != ApplyTarget.Ally);
                    return hitsRemaining > 0;
                });

                ProcessFieldTarget(field, skillCast);
            }
        }

        field.BroadcastPacket(SkillDamagePacket.RegionDamage(skillCast, damages));
    }

    private static bool InitializeLinkSkills(FieldManager field, SkillCast skillCast, long cooldown, IFieldActor? owner)
    {
        bool hasLinkSkills = false;

        foreach (SkillMotion skillMotion in skillCast.GetSkillMotions())
        {
            long motionCooldown = skillMotion.SplashInvokeCoolTick != 0 ? skillMotion.SplashInvokeCoolTick : cooldown;

            foreach (SkillAttack skillAttack in skillMotion.SkillAttacks)
            {
                if (skillAttack.SkillConditions is null)
                {
                    continue;
                }

                foreach (SkillCondition trigger in skillAttack.SkillConditions)
                {
                    if (trigger.LinkSkillId == 0)
                    {
                        continue;
                    }

                    hasLinkSkills = true;

                    long lastTick = 0;

                    skillCast.Caster?.SkillTriggerHandler.AddListeningRegionLinkSkill(skillCast, trigger.LinkSkillId, () =>
                    {
                        long currentTick = field.TickCount64;

                        if (currentTick - lastTick < motionCooldown)
                        {
                            return;
                        }

                        lastTick = currentTick;

                        int hitsRemaining = skillAttack.TargetCount;

                        skillCast.SkillAttack = skillAttack;

                        ProcessApplyTarget(field, skillAttack.RangeProperty.ApplyTarget, (target) =>
                        {
                            for (int i = 0; i < skillCast.EffectCoords.Count; ++i)
                            {
                                CoordF effectCoord = skillCast.EffectCoords[i];

                                if ((target.Coord - effectCoord).Length() > skillAttack.RangeProperty.Distance)
                                {
                                    continue;
                                }

                                --hitsRemaining;

                                skillCast.Caster?.SkillTriggerHandler.FireLinkSkill(trigger, skillCast, new(skillCast.Caster, target, skillCast.Caster, skillCast.Caster), trigger.LinkSkillId);

                                break;
                            }

                            return hitsRemaining > 0;
                        });
                    });
                }
            }
        }

        return hasLinkSkills;
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
        if (hitsRemaining == 0 || skillCast.SkillAttack is null)
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

        if (caster == target && !skillCast.SkillAttack.RangeProperty.IncludeCaster)
        {
            return;
        }

        bool targetPassedBuffCheck = CheckForEffect(skillCast.SkillAttack.RangeProperty.TargetHasBuffID, target, skillCast.Caster, true);
        targetPassedBuffCheck &= !CheckForEffect(skillCast.SkillAttack.RangeProperty.TargetHasNotBuffID, target, skillCast.Caster, false);

        if (!targetPassedBuffCheck)
        {
            return;
        }

        for (int i = 0; i < skillCast.EffectCoords.Count; ++i)
        {
            CoordF effectCoord = skillCast.EffectCoords[i];

            if ((target.Coord - effectCoord).Length() > skillCast.SkillAttack.RangeProperty.Distance)
            {
                continue;
            }

            ConditionSkillTarget castInfo = new(caster, target, caster, caster);

            bool hitTarget = false;
            bool hitCrit = false;
            bool hitMissed = false;
            bool blocked = false;

            AdditionalEffect? activeShield = target.AdditionalEffects.ActiveShield;
            bool allowHit = true;

            if (activeShield is not null)
            {
                int[]? allowedSkills = activeShield.LevelMetadata?.Basic?.AllowedSkillAttacks;
                int[]? allowedDotEffects = activeShield.LevelMetadata?.Basic?.AllowedDotEffectAttacks;

                if ((allowedSkills?.Length > 0 || allowedDotEffects?.Length > 0) && allowedSkills?.Contains(skillCast.SkillId) != true)
                {
                    allowHit = false;
                }
            }

            if ((skillCast.SkillAttack.DamageProperty.DamageRate != 0 || skillCast.SkillAttack.DamageProperty.DamageValue != 0) && allowHit)
            {
                for (int j = 0; j < skillCast.SkillAttack.DamageProperty.Count; ++j)
                {
                    DamageHandler damage = DamageHandler.CalculateDamage(skillCast, caster, target);

                    if (activeShield is not null)
                    {
                        activeShield.DamageShield(target, (long) damage.Damage);
                    }
                    else
                    {
                        target.Damage(damage, session);

                        damages.Add(damage);
                    }

                    hitCrit |= damage.HitType == HitType.Critical;
                    hitMissed |= damage.HitType == HitType.Miss;
                    hitTarget |= damage.HitType != HitType.Miss;
                    blocked |= damage.HitType == HitType.Block;
                }
            }

            if (damaging)
            {
                target.SkillTriggerHandler.OnAttacked(caster, skillCast.SkillId, hitTarget, hitCrit, hitMissed, blocked);
            }

            skillCast.ActiveCoord = i;
            castInfo.Owner?.SkillTriggerHandler.FireTriggerSkills(skillCast.SkillAttack.SkillConditions, skillCast, castInfo, -1, !hitMissed);
            skillCast.ActiveCoord = -1;

            hitsRemaining--;

            return;
        }
    }

    private static void ProcessFieldTarget(FieldManager field, SkillCast skillCast)
    {
        if (skillCast.SkillAttack is null)
        {
            return;
        }

        IFieldActor? caster = skillCast.Caster;

        for (int i = 0; i < skillCast.EffectCoords.Count; ++i)
        {
            CoordF effectCoord = skillCast.EffectCoords[i];

            ConditionSkillTarget castInfo = new(caster, null, caster);

            skillCast.ActiveCoord = i;
            castInfo.Owner?.SkillTriggerHandler.FireTriggerSkills(skillCast.SkillAttack.SkillConditions, skillCast, castInfo, -1, false);
            skillCast.ActiveCoord = -1;
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
