using MapleServer2.Managers.Actors;

namespace MapleServer2.AIScripts.Default;

public class DefaultState : AIState

{
    public override void OnEnter(Npc npc) { }

    public override AIState? Execute(Npc npc)
    {
        if (npc.TryDefineTarget())
        {
            return new DefaultAttackState();
        }

        return new DefaultPatrolState();
    }

    public override void OnExit(Npc npc) { }
}
