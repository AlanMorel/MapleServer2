using Maple2.PathEngine.Types;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.AI;
using MapleServer2.AI.Functions;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers.Actors;

public class Npc : FieldActor<NpcMetadata>, INpc
{
    private static readonly ILogger Logger = Log.Logger.ForContext<Npc>();

    public MobAI? AI;
    public readonly AIScript? Behavior;
    public IFieldObject<MobSpawn>? OriginSpawn;
    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }
    public IFieldActor<Player>? Target;

    private CoordS NextMovementTarget;
    public float Distance;
    public int LastMovementTime;

    private float BaseVelocity => Movement is MobMovement.Follow or MobMovement.Run ? Value.NpcMetadataSpeed.RunSpeed : Value.NpcMetadataSpeed.WalkSpeed;

    private PatrolData PatrolData;
    private Queue<WayPoint> WayPointQueue = new();
    private WayPoint? CurrentWayPoint;
    private bool IsInAnimation;

    public int SpawnPointId;

    public Npc(int objectId, int mobId, FieldManager fieldManager) : this(objectId, NpcMetadataStorage.GetNpcMetadata(mobId), fieldManager) { }

    public Npc(int objectId, NpcMetadata metadata, FieldManager fieldManager) : base(objectId, metadata, fieldManager)
    {
        Animation = GetDefaultAnimation();
        AI = MobAIManager.GetAI(metadata.AiInfo);
        Stats = new(metadata);
        State = NpcState.Normal;
        Action = NpcAction.Idle;

        if (string.IsNullOrEmpty(metadata.AiInfo))
        {
            return;
        }

        AIContext aiContext = new(this);
        AIState aiState = AIHelper.GetAIState(metadata.AiInfo.Split(".")[0], aiContext);
        Behavior = new(aiContext, aiState);
    }

    public void Attack()
    {
        int roll = Random.Shared.Next(100);
        NpcMetadataSkill metadata = Value.NpcMetadataSkill;
        for (int i = 0; i < metadata.SkillIds.Length; i++)
        {
            if (roll < metadata.SkillProbs[i])
            {
                // Rolled this skill.
                SkillCast skillCast = new(metadata.SkillIds[i], metadata.SkillLevels[i], GuidGenerator.Long(), Target!.Value.Session!.ServerTick, this,
                    Target.Value.Session.ClientTick, 1)
                {
                    Position = Coord,
                    Direction = default,
                    Rotation = default,
                };
                Cast(skillCast);
                StartSkillTimer((metadata.SkillCooldown > 0) ? metadata.SkillCooldown : 1000);
            }

            roll -= metadata.SkillProbs[i];
        }
    }

    public override void Animate(string sequenceName, float duration = -1f)
    {
        SequenceMetadata? metadata = AnimationStorage.GetSequenceMetadataByName(Value.NpcMetadataModel.Model, sequenceName);
        Animation = metadata?.SequenceId ?? 0;

        if (Animation == 0)
        {
            return;
        }

        // Wait for animation to finish and set to idle.
        Task.Run(async () =>
        {
            if (duration != -1)
            {
                await Task.Delay(TimeSpan.FromMilliseconds(duration));
            }

            KeyMetadata keyMetadata = metadata.Keys.FirstOrDefault(x => x.KeyName == "end");
            await Task.Delay(TimeSpan.FromSeconds(keyMetadata?.KeyTime ?? 0));

            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, "Idle_A");
        });
    }

    public override void Damage(DamageHandler damage, GameSession session)
    {
        if (Value.Type is NpcType.Friendly || IsDead)
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
        Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, Value.NpcMetadataDead.Actions.ElementAtOrDefault(0));
        Velocity = default;
    }

    /// <summary>
    /// Update the NPC's velocity and LookDirection.
    /// </summary>
    /// <returns>True if the NPC is moving.</returns>
    public bool UpdateVelocity()
    {
        if (Distance <= 0)
        {
            return WayPointQueue.Count > 0 && GetNextPath();
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

        CoordF addCoord = difference.Normalize() * (Constant.UnitPerMs * BaseVelocity * timeDelta);
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
        if (IsInAnimation)
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
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, CurrentWayPoint.ApproachAnimation);
        }
        else // use default animation for walk.
        {
            Movement = MobMovement.Patrol;
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, "Walk_A"); // TODO: remove "Walk_A" hardcode?
        }

        // TODO: Here we should pathfind to the next movement target. Pathfinder is having trouble with stairs so we need to find a way to fix it.
        NextMovementTarget = CurrentWayPoint.Position;
        CoordF difference = NextMovementTarget.ToFloat() - Coord;

        Distance = difference.Length();
        LookDirection = (short) difference.XYAngle();
        Velocity = difference.Normalize() * BaseVelocity;
        return true;
    }

    public void Patrol()
    {
        int moveDistance = Random.Shared.Next(150, Value.MoveRange * 2);

        CoordF originSpawnCoord = OriginSpawn?.Coord ?? Coord;
        List<CoordS>? coordSPath = Navigator.GenerateRandomPathAroundCoord(Agent, originSpawnCoord, moveDistance);
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

    public void Follow(CoordF targetCoords)
    {
        IEnumerable<CoordS>? path = Navigator.FindPath(Agent, targetCoords.ToShort());
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

        if ((coordS.ToFloat() - Coord).Length() <= Value.NpcMetadataDistance.Avoid)
        {
            SetMovementTargetToDefault();
            return;
        }

        NextMovementTarget = coordS;
        Distance = (coordS.ToFloat() - Coord).Length() - Value.NpcMetadataDistance.Avoid;
    }

    private static void HandleMobKill(GameSession session, Npc npc)
    {
        // TODO: Add trophy + item drops
        // Drop Money
        bool dropMeso = Random.Shared.Next(2) == 0;
        if (dropMeso)
        {
            // TODO: Calculate meso drop rate
            Item meso = new(id: 90000001, amount: Random.Shared.Next(2, 800), saveToDatabase: false);
            session.FieldManager.AddItem(session.Player.FieldPlayer, meso, npc);
        }

        // Drop Meret
        bool dropMeret = Random.Shared.Next(40) == 0;
        if (dropMeret)
        {
            Item meret = new(id: 90000004, amount: 20, saveToDatabase: false);
            session.FieldManager.AddItem(session.Player.FieldPlayer, meret, npc);
        }

        // Drop SP
        bool dropSP = Random.Shared.Next(6) == 0;
        if (dropSP)
        {
            Item spBall = new(id: 90000009, amount: 20, saveToDatabase: false);
            session.FieldManager.AddItem(session.Player.FieldPlayer, spBall, npc);
        }

        // Drop EP
        bool dropEP = Random.Shared.Next(10) == 0;
        if (dropEP)
        {
            Item epBall = new(id: 90000010, amount: 20, saveToDatabase: false);
            session.FieldManager.AddItem(session.Player.FieldPlayer, epBall, npc);
        }

        // Drop Items
        // Send achieves (?)
        // Gain Mob EXP
        session.Player.Levels.GainExp(npc.Value.Experience);
        // Send achieves (2)

        // Quest Check
        QuestManager.OnNpcKill(session.Player, npc.Value.Id, session.Player.MapId);
    }

    private void MoveAgent()
    {
        Position position = Navigator.FindPositionFromCoordS(Coord);
        if (Navigator.PositionIsValid(position))
        {
            Agent?.moveTo(position);
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

        SequenceMetadata? metadata = AnimationStorage.GetSequenceMetadataByName(Value.NpcMetadataModel.Model, CurrentWayPoint.ArriveAnimation);
        if (metadata is null)
        {
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, "Idle_A");
            return;
        }

        Animation = metadata.SequenceId;

        KeyMetadata? keyMetadata = metadata.Keys.FirstOrDefault(x => x.KeyName == "end");
        if (keyMetadata is null)
        {
            return;
        }

        // wait until animation is done
        Task.Run(async () =>
        {
            IsInAnimation = true;
            await Task.Delay(TimeSpan.FromSeconds(keyMetadata.KeyTime));
            IsInAnimation = false;
        });
    }

    public void SetPatrolData(PatrolData? patrolData)
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

    private short GetDefaultAnimation()
    {
        NpcMetadata? npcMetadata = NpcMetadataStorage.GetNpcMetadata(Value.Id);
        if (npcMetadata is null || !npcMetadata.StateActions.TryGetValue(NpcState.Normal, out NpcActionChance[]? stateAction))
        {
            return AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, "Idle_A");
        }

        return AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, stateAction.Length == 0 ? "Idle_A" : stateAction[0].Id);
    }

    public NpcActionChance GetRandomAction()
    {
        int[] probabilities = Value.StateActions[NpcState.Normal].Select(x => (int) x.Chance).OrderBy(x => x).ToArray();

        int chance = Random.Shared.Next(probabilities.Sum());

        int sum = 0;
        for (int i = 0; i < probabilities.Length; i++)
        {
            sum += probabilities[i];
            if (chance < sum)
            {
                return Value.StateActions[NpcState.Normal][i];
            }
        }

        return Value.StateActions[NpcState.Normal][0];
    }
}
