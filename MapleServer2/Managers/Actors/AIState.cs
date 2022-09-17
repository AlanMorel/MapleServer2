namespace MapleServer2.Managers.Actors;

public abstract class AIState
{
    public abstract void OnEnter(Npc npc);

    public abstract AIState? Execute(Npc npc);

    public abstract void OnExit(Npc npc);
}
