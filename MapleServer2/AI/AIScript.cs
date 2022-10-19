using MapleServer2.AI.Functions;
using MapleServer2.Types;

namespace MapleServer2.AI;

public class AIScript
{
    private readonly AIContext Context;
    private AIState? State;
    private AIState? NextState;

    public AIScript(AIContext context, AIState start)
    {
        Context = context;
        NextState = start;
    }

    public void Next()
    {
        if (NextState != null)
        {
            State?.OnExit();
            State = NextState;
            State?.OnEnter();
            NextState = null;
        }

        NextState = State?.Execute();
    }
}
