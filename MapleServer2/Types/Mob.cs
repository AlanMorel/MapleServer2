using System;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public enum MobState
    {
        Spawn,
        Normal,
        Aggro,
        Dead,
        EnterAttackRange,
        EnterProjectileRange,
    }

    public enum MobAction
    {
        None,
        Idle,
        Bore,
        Walk,
        Jump,
        Attack,
        Buff,
        Heal,
    }

    public enum MobMovement
    {
        Hold,
        Patrol,
        LookAt,
        Follow,
        Strafe,
        Run,
        Dodge,
    }

    public class Mob : NpcMetadata
    {
        private static readonly Random rand = new Random();

        private MobAI AI;
        public bool IsDead { get; set; }
        public short ZRotation; // In degrees * 10
        public IFieldObject<MobSpawn> OriginSpawn;
        public MobState State;
        public MobAction CurrentAction;
        public MobMovement CurrentMovement;
        public IFieldObject<Player> Enemy;
        public CoordF Velocity;
        private CoordF SpawnDistance;

        public Mob(int id)
        {
            NpcMetadata mob = NpcMetadataStorage.GetNpcMetadata(id);
            if (mob != null)
            {
                Id = mob.Id;
                Model = mob.Model;
                Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, "Idle_A");
                Stats = mob.Stats;
                Experience = mob.Experience;
                Friendly = mob.Friendly;
                AI = MobAIManager.GetAI(mob.AiInfo);
                State = MobState.Normal;
            }
        }

        public Mob(int id, IFieldObject<MobSpawn> originSpawn) : this(id)
        {
            OriginSpawn = originSpawn;
        }

        public void Act()
        {
            if (AI == null)
            {
                return;
            }

            CurrentAction = AI.GetAction(this);
            CurrentMovement = AI.GetMovementAction(this);

            // TODO: Get animations
            switch (CurrentAction)
            {
                case MobAction.Idle:
                    Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, "Idle_A");
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case MobAction.Bore:
                    Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, "Bore_A");
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case MobAction.Walk:
                    Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, "Walk_A");
                    Move(CurrentMovement);
                    break;
                case MobAction.Jump:
                default:
                    break;
            }
        }

        public void Damage(double damage)
        {
            Stats.Hp.Total -= (long) damage;
            if (Stats.Hp.Total <= 0)
            {
                Perish();
            }
        }

        public void Move(MobMovement moveType)
        {
            switch (moveType)
            {
                case MobMovement.Patrol:
                    // TODO: Grab move radius from metadata
                    int moveDistance = rand.Next(0, Block.BLOCK_SIZE);
                    short moveAngle = (short) rand.Next(-1800, 1800);

                    Velocity = CoordF.From(moveDistance, moveAngle);

                    // Keep near spawn
                    if ((SpawnDistance - Velocity).Length() >= Block.BLOCK_SIZE * 2)
                    {
                        moveAngle = (short) SpawnDistance.XYAngle();
                        Velocity = CoordF.From(Block.BLOCK_SIZE, moveAngle);
                    }

                    ZRotation = moveAngle;  // looking direction of the monster
                    break;
                case MobMovement.Follow:    // move towards target
                case MobMovement.Strafe:    // move around target
                case MobMovement.Run:       // move away from target
                case MobMovement.LookAt:
                case MobMovement.Hold:
                default:
                    Velocity = CoordF.From(0, 0, 0);
                    break;
            }

            SpawnDistance -= Velocity;
        }

        public void Perish()
        {
            IsDead = true;
            State = MobState.Dead;
            // TODO: Retrieve death animation from MobMetadata <dead> tag
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, "Dead_A");
        }
    }
}
