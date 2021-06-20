using System;
using System.Collections.Generic;
using System.Linq;

namespace MapleServer2.Types
{
    public class MobAI
    {
        public delegate bool Condition(Mob mob);

        private static readonly Random rand = new Random();

        public Dictionary<MobState, (MobAction, MobMovement, Condition[])> Rules;

        public MobAI()
        {
            Rules = new Dictionary<MobState, (MobAction, MobMovement, Condition[])>();
        }

        public MobAction GetAction(Mob mob)
        {
            if (mob.State == MobState.Dead)
            {
                return MobAction.None;
            }

            (MobAction nextAction, _, Condition[] conds) = Rules.GetValueOrDefault(mob.State, (MobAction.None, MobMovement.Hold, Array.Empty<Condition>()));

            bool meetsConds = conds.Aggregate(true, (acc, nextCond) => acc && nextCond(mob));
            if (meetsConds)
            {
                if (nextAction != MobAction.None)
                {
                    return nextAction;
                }
                // else if (mob.Actions[mob.State].length > 0)
                else
                {
                    // TODO: Retrieve state's actions from mob metadata
                    (MobAction, int)[] availableActs = { (MobAction.Idle, 3500), (MobAction.Bore, 3500), (MobAction.Walk, 3000) };

                    int roll = rand.Next(10000);
                    // foreach ((MobAction action, int probability) in mob.Actions[mob.State])
                    foreach ((MobAction action, int probability) in availableActs)
                    {
                        if (roll < probability)
                        {
                            return action;
                        }

                        roll -= probability;
                    }
                }
            }
            return mob.CurrentAction;
        }

        public MobMovement GetMovementAction(Mob mob)
        {
            if (mob.State == MobState.Dead)
            {
                return MobMovement.Hold;
            }

            (_, MobMovement movementAction, Condition[] conds) = Rules.GetValueOrDefault(mob.State, (MobAction.None, MobMovement.Hold, Array.Empty<Condition>()));

            bool meetsConds = conds.Aggregate(true, (acc, nextCond) => acc && nextCond(mob));
            if (meetsConds)
            {
                return movementAction;
            }
            return mob.CurrentMovement;
        }

        public static Condition StateCond(MobState state)
        {
            return new Condition((Mob mob) => mob.State == state);
        }

        public static Condition HpCond(int min, int max)
        {
            return new Condition((Mob mob) => mob.Stats.Hp.Total >= min && mob.Stats.Hp.Total >= max);
        }

        public static Condition SpCond(int min, int max)
        {
            return new Condition((Mob mob) => mob.Stats.Sp.Total >= min && mob.Stats.Sp.Total >= max);
        }
    }
}
