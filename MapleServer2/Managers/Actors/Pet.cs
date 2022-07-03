using Maple2.PathEngine.Types;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers.Actors;

public class Pet : FieldActor<NpcMetadata>, INpc
{
    private static readonly Random Rand = Random.Shared;
    private static readonly ILogger Logger = Log.Logger.ForContext<Pet>();

    public MobAI AI;
    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }

    public IFieldActor Target;

    private CoordS NextMovementTarget;
    private float Distance;

    private int LastMovementTime;
    private float BaseVelocity => Movement is MobMovement.Follow or MobMovement.Run ? Value.NpcMetadataSpeed.RunSpeed : Value.NpcMetadataSpeed.WalkSpeed;

    public Character Owner { get; set; }

    public Item Item { get; set; }

    public Pet(int objectId, Item item, Character owner, FieldManager fieldManager)
        : this(objectId, NpcMetadataStorage.GetNpcMetadata(ItemMetadataStorage.GetMetadata(item.Id).Pet.PetId), fieldManager, item, owner) { }

    public Pet(int objectId, NpcMetadata value, FieldManager fieldManager, Item item, Character owner) : base(objectId, value, fieldManager)
    {
        AI = MobAIManager.GetAI(value.AiInfo);
        Stats = new(value);
        Item = item;
        Owner = owner;
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
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.NpcMetadataModel.Model, actionName);
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
                    // Attack();
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

                    CoordF originSpawnCoord = Owner.Coord;
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
            case MobMovement.Follow: // move towards owner
                {
                    IEnumerable<CoordS> path = Navigator.FindPath(Agent, Owner.Coord.ToShort());
                    if (path is null)
                    {
                        SetMovementTargetToDefault();
                        Logger.Debug("Path for mob {0} towards target {1} is null.", Value.Id, Owner.Value.Name);
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

    /// <summary>
    /// Update the PET's velocity and LookDirection.
    /// </summary>
    /// <returns>True if the PET is moving.</returns>
    public bool UpdateVelocity()
    {
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
    /// Update the PET's position based on BaseVelocity and last movement time delta.
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

    private void MoveAgent()
    {
        Position position = Navigator.FindPositionFromCoordS(Coord);
        if (Navigator.PositionIsValid(position))
        {
            Agent?.moveTo(position);
        }
    }

    private void SetMovementTargetToDefault()
    {
        NextMovementTarget = default;
        Velocity = default;
        Distance = 0;
    }
}
