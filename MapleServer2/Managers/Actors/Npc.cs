using Maple2.PathEngine.Types;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers.Actors;

public class Npc : FieldActor<NpcMetadata>, INpc
{
    private static readonly Random Rand = Random.Shared;
    private static readonly ILogger Logger = Log.Logger.ForContext<Npc>();

    public MobAI AI;
    public IFieldObject<MobSpawn> OriginSpawn;
    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }
    public IFieldActor<Player> Target;

    private CoordS NextMovementTarget;
    private float Distance;

    private int LastMovementTime;
    private float BaseVelocity => Movement is MobMovement.Follow or MobMovement.Run ? Value.NpcMetadataSpeed.RunSpeed : Value.NpcMetadataSpeed.WalkSpeed;

    private PatrolData PatrolData;
    public Queue<WayPoint> WayPointQueue = new();
    private WayPoint CurrentWayPoint;
    private bool WaitForAnimation;

    public Npc(int objectId, int mobId, FieldManager fieldManager) : this(objectId, NpcMetadataStorage.GetNpcMetadata(mobId), fieldManager) { }

    public Npc(int objectId, NpcMetadata metadata, FieldManager fieldManager) : base(objectId, metadata, fieldManager)
    {
        Animation = AnimationStorage.GetSequenceIdBySequenceName(metadata.Model, "Idle_A");
        AI = MobAIManager.GetAI(metadata.AiInfo);
        Stats = new(metadata);
        State = NpcState.Normal;
        Action = NpcAction.Idle;
    }

    public void Attack()
    {
        int roll = Rand.Next(100);
        for (int i = 0; i < Value.NpcMetadataSkill.SkillIds.Length; i++)
        {
            if (roll < Value.NpcMetadataSkill.SkillProbs[i])
            {
                // Rolled this skill.
                Cast(new(Value.NpcMetadataSkill.SkillIds[i], Value.NpcMetadataSkill.SkillLevels[i]));
                StartSkillTimer((Value.NpcMetadataSkill.SkillCooldown > 0) ? Value.NpcMetadataSkill.SkillCooldown : 1000);
            }

            roll -= Value.NpcMetadataSkill.SkillProbs[i];
        }
    }

    public override void Animate(string sequenceName)
    {
        Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, sequenceName);
        // TODO implement stopping animation
    }

    public override void Damage(DamageHandler damage, GameSession session)
    {
        if (Value.Friendly == 2) // Attacking friendly NPC
        {
            return;
        }

        base.Damage(damage, session);

        session.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, StatAttribute.Hp));
        if (IsDead)
        {
            HandleMobKill(session, this);
        }
    }

    public override void Perish()
    {
        IsDead = true;
        State = NpcState.Dead;
        Animate(Value.NpcMetadataDead.Actions.ElementAtOrDefault(0));
        Velocity = default;
    }

    /// <summary>
    /// Update the NPC's velocity and LookDirection.
    /// </summary>
    /// <returns>True if the NPC is moving.</returns>
    public bool UpdateVelocity()
    {
        if (WayPointQueue.Count > 0 && Distance <= 0)
        {
            return GetNextPath();
        }

        if (Distance <= 0)
        {
            return false;
        }

        CoordF difference = NextMovementTarget.ToFloat() - Coord;

        LookDirection = (short) difference.XYAngle();

        Velocity = difference.Normalize() * BaseVelocity;
        return true;
    }

    /// <summary>
    /// Update the NPC's position based on BaseVelocity and last movement time delta.
    /// </summary>
    public void UpdateCoord()
    {
        if (Distance <= 0)
        {
            return;
        }

        CoordF difference = NextMovementTarget.ToFloat() - Coord;
        if (difference.Length() < 0.1f)
        {
            SetMovementTargetToDefault();
            return;
        }

        int tickNow = Environment.TickCount;
        int timeDelta = Math.Clamp(tickNow - LastMovementTime, 0, 400);

        const float UnitPerMs = 0.001f; // Velocity in which all NPCs move per ms. 150 units per second.
        CoordF addCoord = difference.Normalize() * (UnitPerMs * BaseVelocity * timeDelta);
        Coord += addCoord;

        Distance -= addCoord.Length();

        MoveAgent();
        LastMovementTime = tickNow;

        if (Distance > 0)
        {
            return;
        }

        Coord = NextMovementTarget;
        SetMovementTargetToDefault();
    }

    /// <summary>
    /// Updates NextMovementTarget trough MovePath queue.
    /// </summary>
    /// <returns>True if NextMovementTarget was set.</returns>
    private bool GetNextPath()
    {
        if (WayPointQueue.Count == 0)
        {
            return false;
        }

        if (WaitForAnimation)
        {
            return false;
        }

        CurrentWayPoint = WayPointQueue.Dequeue();
        if (PatrolData.IsLoop)
        {
            WayPointQueue.Enqueue(CurrentWayPoint);
        }

        if (!string.IsNullOrEmpty(CurrentWayPoint.ApproachAnimation)) // If approach animation is defined, use it.
        {
            Movement = CurrentWayPoint.ApproachAnimation == "Run_A" ? MobMovement.Run : MobMovement.Patrol;
            Animate(CurrentWayPoint.ApproachAnimation);
        }
        else // use default animation for walk.
        {
            Movement = MobMovement.Patrol;
            Animate("Walk_A"); // TODO: remove "Walk_A" hardcode?
        }

        // TODO: Here we should pathfind to the next movement target. Pathfinder is having trouble with stairs so we need to find a way to fix it.
        NextMovementTarget = CurrentWayPoint.Position;
        CoordF difference = NextMovementTarget.ToFloat() - Coord;

        Distance = difference.Length();
        LookDirection = (short) difference.XYAngle();
        Velocity = difference.Normalize() * BaseVelocity;
        return true;
    }

    public void Act()
    {
        if (AI is null)
        {
            return;
        }

        (string actionName, NpcAction actionType) = AI.GetAction(this);

        if (actionName is not null)
        {
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, actionName);
        }

        Action = actionType;
        Movement = AI.GetMovementAction(this);

        switch (Action)
        {
            case NpcAction.Idle:
            case NpcAction.Bore:
                Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                break;
            case NpcAction.Walk:
            case NpcAction.Run:
                Move(Movement);
                break;
            case NpcAction.Skill:
                // Cast skill
                if (!OnCooldown)
                {
                    Attack();
                    Move(MobMovement.Hold);
                    break;
                }

                Move(Movement);
                break;
            case NpcAction.Jump:
                break;
        }
    }

    private void Move(MobMovement moveType)
    {
        switch (moveType)
        {
            case MobMovement.Patrol:
                {
                    int moveDistance = Rand.Next(150, Value.MoveRange * 2);

                    CoordF originSpawnCoord = OriginSpawn?.Coord ?? Coord;
                    List<CoordS> coordSPath = Navigator.GenerateRandomPathAroundCoord(Agent, originSpawnCoord, moveDistance);
                    if (coordSPath is null)
                    {
                        SetMovementTargetToDefault();
                        return;
                    }

                    CoordS nextMovementTarget = coordSPath.Skip(1).FirstOrDefault();
                    if (nextMovementTarget == default)
                    {
                        SetMovementTargetToDefault();
                        return;
                    }

                    NextMovementTarget = nextMovementTarget;
                    Distance = (nextMovementTarget.ToFloat() - Coord).Length();
                }
                break;
            case MobMovement.Follow: // move towards target
                {
                    IEnumerable<CoordS> path = Navigator.FindPath(Agent, Target.Coord.ToShort());
                    if (path is null)
                    {
                        SetMovementTargetToDefault();
                        Logger.Debug("Path for mob {0} towards target {1} is null.", Value.Id, Target.Value.Name);
                        return;
                    }

                    CoordS coordS = path.Skip(1).FirstOrDefault();
                    if (coordS == default)
                    {
                        SetMovementTargetToDefault();
                        return;
                    }

                    NextMovementTarget = coordS;
                    Distance = (coordS.ToFloat() - Coord).Length();
                }
                break;
            case MobMovement.Strafe: // move around target
            case MobMovement.Run: // move away from target
            case MobMovement.LookAt:
            case MobMovement.Hold:
            default:
                Velocity = default;
                break;
        }
    }

    private static void HandleMobKill(GameSession session, IFieldObject<NpcMetadata> mob)
    {
        // TODO: Add trophy + item drops
        // Drop Money
        bool dropMeso = Rand.Next(2) == 0;
        if (dropMeso)
        {
            // TODO: Calculate meso drop rate
            Item meso = new(90000001, Rand.Next(2, 800));
            session.FieldManager.AddResource(meso, mob, session.Player.FieldPlayer);
        }

        // Drop Meret
        bool dropMeret = Rand.Next(40) == 0;
        if (dropMeret)
        {
            Item meret = new(90000004, 20);
            session.FieldManager.AddResource(meret, mob, session.Player.FieldPlayer);
        }

        // Drop SP
        bool dropSP = Rand.Next(6) == 0;
        if (dropSP)
        {
            Item spBall = new(90000009, 20);
            session.FieldManager.AddResource(spBall, mob, session.Player.FieldPlayer);
        }

        // Drop EP
        bool dropEP = Rand.Next(10) == 0;
        if (dropEP)
        {
            Item epBall = new(90000010, 20);
            session.FieldManager.AddResource(epBall, mob, session.Player.FieldPlayer);
        }

        // Drop Items
        // Send achieves (?)
        // Gain Mob EXP
        session.Player.Levels.GainExp(mob.Value.Experience);
        // Send achieves (2)

        // Quest Check
        QuestManager.OnNpcKill(session.Player, mob.Value.Id, session.Player.MapId);
    }

    private void MoveAgent()
    {
        Position position = Navigator.FindPositionFromCoordS(Coord);
        if (Navigator.PositionIsValid(position))
        {
            Agent.moveTo(position);
        }
    }

    private Task StartSkillTimer(int cooldownMilliseconds)
    {
        return Task.Run(async () =>
        {
            await Task.Delay(cooldownMilliseconds);

            OnCooldown = false;
        });
    }

    private void SetMovementTargetToDefault()
    {
        NextMovementTarget = default;
        Velocity = default;
        Distance = 0;

        if (CurrentWayPoint is null)
        {
            return;
        }

        SequenceMetadata metadata = AnimationStorage.GetSequenceMetadataByName(Value.Model, CurrentWayPoint.ArriveAnimation);
        if (metadata is null)
        {
            Animate("Idle_A");
            return;
        }

        Animation = metadata.SequenceId;

        KeyMetadata keyMetadata = metadata.Keys.FirstOrDefault(x => x.KeyName == "end");
        if (keyMetadata is null)
        {
            return;
        }

        // wait until animation is done
        Task.Run(async () =>
        {
            WaitForAnimation = true;
            await Task.Delay(TimeSpan.FromSeconds(keyMetadata.KeyTime));
            WaitForAnimation = false;
        });
    }

    public void SetPatrolData(PatrolData patrolData)
    {
        if (patrolData is null)
        {
            return;
        }

        PatrolData = patrolData;

        WayPointQueue = new();
        foreach (WayPoint node in patrolData.WayPoints)
        {
            WayPointQueue.Enqueue(node);
        }
    }
}
