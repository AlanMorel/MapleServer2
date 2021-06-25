using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;

namespace MapleServer2.Types
{
    public class MobAI
    {
        public delegate bool Condition(Mob mob);

        private static readonly Random rand = new Random();

        public Dictionary<NpcState, (NpcAction, MobMovement, Condition[])> Rules;

        public MobAI()
        {
            Rules = new Dictionary<NpcState, (NpcAction, MobMovement, Condition[])>();
        }

        public (string, NpcAction) GetAction(Mob mob)
        {
            if (mob.State == NpcState.Dead)
            {
                return (null, NpcAction.None);
            }

            (NpcAction nextAction, _, Condition[] conds) = Rules.GetValueOrDefault(mob.State, (NpcAction.None, MobMovement.Hold, Array.Empty<Condition>()));

            bool meetsConds = conds.Aggregate(true, (acc, nextCond) => acc && nextCond(mob));
            if (meetsConds)
            {
                if (nextAction != NpcAction.None)
                {
                    return (null, nextAction);
                }
                else if (mob.StateActions[mob.State].Length > 0)
                {
                    int roll = rand.Next(10000);
                    foreach ((string name, NpcAction type, int probability) in mob.StateActions[mob.State])
                    {
                        if (roll < probability)
                        {
                            return (name, type);
                        }

                        roll -= probability;
                    }
                }
            }
            return (null, mob.CurrentAction);
        }

        public MobMovement GetMovementAction(Mob mob)
        {
            if (mob.State == NpcState.Dead)
            {
                return MobMovement.Hold;
            }

            (_, MobMovement movementAction, Condition[] conds) = Rules.GetValueOrDefault(mob.State, (NpcAction.None, MobMovement.Hold, Array.Empty<Condition>()));

            bool meetsConds = conds.Aggregate(true, (acc, nextCond) => acc && nextCond(mob));
            if (meetsConds)
            {
                return movementAction;
            }
            return mob.CurrentMovement;
        }

        public static Condition HpPercentCond(int min = 0, int max = 100)
        {
            return new Condition((Mob mob) => mob.Stats.Hp.Total >= min && mob.Stats.Hp.Total >= max);
        }

        public static Condition HpCond(int min = 0, int max = int.MaxValue)
        {
            return new Condition((Mob mob) => mob.Stats.Hp.Total >= min && mob.Stats.Hp.Total >= max);
        }

        public static Condition SpCond(int min = 0, int max = int.MaxValue)
        {
            return new Condition((Mob mob) => mob.Stats.Sp.Total >= min && mob.Stats.Sp.Total >= max);
        }
    }
}
