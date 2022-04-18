using Maple2.PathEngine.Types;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Managers.Actors;

public class Mob : FieldActor<NpcMetadata>, INpc
{
    private static readonly Random Rand = Random.Shared;
    private readonly MobAI AI;
    public IFieldObject<MobSpawn> OriginSpawn;

    private CoordF SpawnDistance;

    public NpcState State { get; set; }
    public NpcAction Action { get; set; }
    public MobMovement Movement { get; set; }
    public IFieldActor<Player> Target;

    public Mob(int objectId, int mobId) : this(objectId, NpcMetadataStorage.GetNpcMetadata(mobId)) { }

    public Mob(int objectId, NpcMetadata metadata) : base(objectId, metadata)
    {
        Animation = AnimationStorage.GetSequenceIdBySequenceName(metadata.Model, "Idle_A");
        AI = MobAIManager.GetAI(metadata.AiInfo);
        Stats = new(metadata);
        State = NpcState.Normal;
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

    private Task StartSkillTimer(int cooldownMilliseconds)
    {
        return Task.Run(async () =>
        {
            await Task.Delay(cooldownMilliseconds);

            OnCooldown = false;
        });
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
            default:
                break;
        }
    }

    private void Move(MobMovement moveType)
    {
        switch (moveType)
        {
            case MobMovement.Patrol:
                {
                    int moveDistance = Rand.Next(1, Value.MoveRange);

                    CoordF originSpawnCoord = OriginSpawn?.Coord ?? Coord;
                    IEnumerable<CoordS> path = Navigator.GenerateRandomPathAroundCoord(Agent, originSpawnCoord.ToShort(), moveDistance);
                    if (path is null)
                    {
                        return;
                    }

                    CoordS coordF = path.Last();
                    Position newPosition = Navigator.FindPositionFromCoordS(coordF);
                    if (Navigator.PositionIsValid(newPosition))
                    {
                        Agent.moveTo(newPosition);
                    }

                    MoveTo(coordF.ToFloat());
                    LookDirection = (short) Velocity.XYAngle(); // looking direction of the monster
                }
                break;
            case MobMovement.Follow: // move towards target
                // {
                //     IEnumerable<CoordS> path = Navigator.FindPath(Agent, Target.Coord.ToShort());
                //     if (path is null)
                //     {
                //         Console.WriteLine("Path is null");
                //         return;
                //     }
                //
                //     CoordS coordF = path.Last();
                //     Position newPosition = Navigator.FindPositionFromCoordS(coordF);
                //     if (Navigator.PositionIsValid(newPosition))
                //     {
                //         Agent.moveTo(newPosition);
                //     }
                //
                //     MoveTo(coordF.ToFloat());
                //     LookDirection = (short) Velocity.XYAngle();
                // }
                break;
            case MobMovement.Strafe: // move around target
            case MobMovement.Run: // move away from target
            case MobMovement.LookAt:
            case MobMovement.Hold:
            default:
                Velocity = CoordF.From(0, 0, 0);
                break;
        }

        SpawnDistance -= Velocity;
    }

    public override void Damage(DamageHandler damage, GameSession session)
    {
        base.Damage(damage, session);

        session.FieldManager.BroadcastPacket(StatPacket.UpdateMobStats(this));
        if (IsDead)
        {
            HandleMobKill(session, this);
        }
    }

    public override void Perish()
    {
        IsDead = true;
        State = NpcState.Dead;
        int randAnim = Random.Shared.Next(Value.StateActions[NpcState.Dead].Length);
        Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, Value.StateActions[NpcState.Dead][randAnim].Item1);
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
}
