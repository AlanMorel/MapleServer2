using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Managers;

public partial class FieldManager
{
    private partial class Mob : FieldActor<NpcMetadata>, INpc
    {
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
            int roll = RandomProvider.Get().Next(100);
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
            if (AI == null)
            {
                return;
            }

            (string actionName, NpcAction actionType) = AI.GetAction(this);

            if (actionName != null)
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

        public void Move(MobMovement moveType)
        {
            Random rand = RandomProvider.Get();

            switch (moveType)
            {
                case MobMovement.Patrol:
                    // Fallback Dummy Movement
                    int moveDistance = rand.Next(0, Value.MoveRange);
                    short moveDir = (short) rand.Next(-1800, 1800);

                    Velocity = CoordF.From(moveDistance, moveDir);
                    // Keep near spawn
                    if ((SpawnDistance - Velocity).Length() >= Block.BLOCK_SIZE * 2)
                    {
                        moveDir = (short) SpawnDistance.XYAngle();
                        Velocity = CoordF.From(Block.BLOCK_SIZE, moveDir);
                    }

                    LookDirection = moveDir; // looking direction of the monster
                    break;
                case MobMovement.Follow: // move towards target
                    Velocity = CoordF.From(0, 0, 0);
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

        public override void Perish()
        {
            IsDead = true;
            State = NpcState.Dead;
            int randAnim = RandomProvider.Get().Next(Value.StateActions[NpcState.Dead].Length);
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, Value.StateActions[NpcState.Dead][randAnim].Item1);
        }
    }
}
