using Maple2Storage.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;

namespace MapleServer2.AIScripts.Default;

public class DefaultPatrolState : AIState
{
    public override void OnEnter(Npc npc)
    {
        npc.Movement = MobMovement.Patrol;
    }

    public override AIState? Execute(Npc npc)
    {
        if (npc.TryDefineTarget())
        {
            return new DefaultAttackState();
        }

        long timeSinceLastMovement = Environment.TickCount - npc.LastMovementTime;
        if (npc.Distance > 0 && timeSinceLastMovement < 1000)
        {
            return null;
        }

        (string id, NpcAction action, short _) = npc.GetRandomAction();
        npc.Action = action;
        npc.Animation = AnimationStorage.GetSequenceIdBySequenceName(npc.Value.NpcMetadataModel.Model, id);

        if (action == NpcAction.Walk)
        {
            npc.Patrol();
        }

        return null;
    }

    public override void OnExit(Npc npc) { }
}
