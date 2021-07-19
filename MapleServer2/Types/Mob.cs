using System;
using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public enum MobMovement : byte
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
        private readonly MobAI AI;
        public bool IsDead { get; set; }
        public short ZRotation; // In degrees * 10
        public IFieldObject<MobSpawn> OriginSpawn;
        public NpcState State;
        public NpcAction CurrentAction;
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
                StateActions = mob.StateActions;
                MoveRange = mob.MoveRange;
                Stats = mob.Stats;
                Experience = mob.Experience;
                Friendly = mob.Friendly;
                AI = MobAIManager.GetAI(mob.AiInfo);
                State = NpcState.Normal;
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

            (string actionName, NpcAction actionType) = AI.GetAction(this);

            Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, actionName);
            CurrentAction = actionType;
            CurrentMovement = AI.GetMovementAction(this);

            switch (CurrentAction)
            {
                case NpcAction.Idle:
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case NpcAction.Bore:
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case NpcAction.Walk:
                    Move(CurrentMovement);
                    break;
                case NpcAction.Jump:
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
            Random rand = RandomProvider.Get();
            switch (moveType)
            {
                case MobMovement.Patrol:
                    // TODO: Grab move radius from metadata
                    int moveDistance = rand.Next(0, MoveRange);
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
            State = NpcState.Dead;
            int randAnim = RandomProvider.Get().Next(StateActions[NpcState.Dead].Length);
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Model, StateActions[NpcState.Dead][randAnim].Item1);
        }
    }
}
