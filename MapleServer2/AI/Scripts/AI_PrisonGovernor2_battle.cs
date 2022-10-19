using Maple2Storage.Types;
using MapleServer2.AI.Functions;
using MapleServer2.Data.Static;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;

namespace MapleServer2.AI.Scripts;

// ReSharper disable once InconsistentNaming
// ReSharper disable once UnusedType.Global
public class AI_PrisonGovernor2_battle : AIState
{
    // First constructor must be public, others can be internal.
    public AI_PrisonGovernor2_battle(AIContext context) : base(context) { }

    public override void OnEnter()
    {
        // Ok to assume this will only happen on tutorial, so always attack the only player available
        Context.DefineTargetToFirstPlayer();
    }

    public override AIState? Execute()
    {
        if (Context.CheckHPThreshold(50))
        {
            return new BattleStop(Context);
        }

        if (Context.OutOfSight())
        {
            return null;
        }

        if (Context.InSight())
        {
            Context.Follow();
            return null;
        }

        // TODO: Attack
        // npc.Attack();
        return null;
    }

    public override void OnExit() { }
}

public class BattleStop : AIState
{
    public BattleStop(AIContext context) : base(context) { }

    public override void OnEnter() { }

    public override AIState? Execute()
    {
        Context.SetUserValue("battleStop", 1);
        return null;
    }

    public override void OnExit() { }
}
