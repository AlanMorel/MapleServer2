using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class MobAI
{
    public delegate bool Condition(IFieldActor<NpcMetadata> mob);
    public Dictionary<NpcState, (NpcAction, MobMovement, Condition[])> Rules;

    public MobAI()
    {
        Rules = new();
    }

    public (string, NpcAction) GetAction(INpc mob)
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

            if (mob.Value.StateActions[mob.State].Length > 0)
            {
                int roll = Random.Shared.Next(10000);
                foreach ((string name, NpcAction type, int probability) in mob.Value.StateActions[mob.State])
                {
                    if (roll < probability)
                    {
                        return (name, type);
                    }

                    roll -= probability;
                }
            }
        }

        return (null, mob.Action);
    }

    public MobMovement GetMovementAction(INpc mob)
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
        return mob.Movement;
    }

    public static Condition HpPercentCond(int min = 0, int max = 100)
    {
        return mob => mob.Stats[StatId.Hp].Total >= min && mob.Stats[StatId.Hp].Total >= max;
    }

    public static Condition HpCond(int min = 0, int max = int.MaxValue)
    {
        return mob => mob.Stats[StatId.Hp].Total >= min && mob.Stats[StatId.Hp].Total >= max;
    }

    public static Condition SpCond(int min = 0, int max = int.MaxValue)
    {
        return mob => mob.Stats[StatId.Spirit].Total >= min && mob.Stats[StatId.Spirit].Total >= max;
    }
}
