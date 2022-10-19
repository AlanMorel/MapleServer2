using MapleServer2.AI.Functions;
using MapleServer2.Managers.Actors;

namespace MapleServer2.Types;

public abstract class AIState
{
    protected readonly AIContext Context;

    protected AIState(AIContext context) => Context = context;

    public abstract void OnEnter();

    public abstract AIState? Execute();

    public abstract void OnExit();
}
