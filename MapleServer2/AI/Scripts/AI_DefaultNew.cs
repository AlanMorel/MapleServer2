using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.AI.Functions;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;

namespace MapleServer2.AI.Scripts;

// ReSharper disable once InconsistentNaming
public class AI_DefaultNew : AIState

{
    public AI_DefaultNew(AIContext context) : base(context) { }

    public override void OnEnter() { }

    public override AIState? Execute()
    {
        if (Context.TryDefineTarget())
        {
            return new DefaultAttackState(Context);
        }

        return new DefaultPatrolState(Context);
    }

    public override void OnExit() { }
}

public class DefaultAttackState : AIState
{
    public DefaultAttackState(AIContext context) : base(context) { }

    public override void OnEnter()
    {
        Context.SetMovement(MobMovement.Follow);
    }

    public override AIState? Execute()
    {
        if (Context.GetTarget() is null)
        {
            return new DefaultPatrolState(Context);
        }

        if (Context.OutOfSight())
        {
            return new DefaultPatrolState(Context);
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

public class DefaultPatrolState : AIState
{
    public DefaultPatrolState(AIContext context) : base(context) { }

    public override void OnEnter()
    {
        Context.SetMovement(MobMovement.Patrol);
    }

    public override AIState? Execute()
    {
        if (Context.TryDefineTarget())
        {
            return new DefaultAttackState(Context);
        }

        if (Context.GetDistanceToNextCoord() > 0 && Context.GetTickSinceLastMovement() < 1000)
        {
            return null;
        }

        Context.Patrol();
        return null;
    }

    public override void OnExit() { }
}
