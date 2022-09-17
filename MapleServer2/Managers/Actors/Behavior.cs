using Serilog;

namespace MapleServer2.Managers.Actors;

public class BehaviorScript
{
    private readonly Npc Npc;
    private AIState? State;
    private AIState? NextState;

    public BehaviorScript(Npc npc, AIState start)
    {
        Npc = npc;
        NextState = start;
    }

    public void Next()
    {
        if (NextState != null)
        {
            if (State is not null)
            {
                Log.Logger.ForContext<BehaviorScript>().Debug($"[{Npc.Value.Name}] OnExit {State} -> OnEnter {NextState}");
            }

            State?.OnExit(Npc);
            State = NextState;
            State?.OnEnter(Npc);
            NextState = null;
        }

        NextState = State?.Execute(Npc);
    }
}
