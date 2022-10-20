using MapleServer2.Managers;
using MapleServer2.Managers.Actors;
using Serilog;

namespace MapleServer2.AI.Functions;

public partial class AIContext
{
    private readonly Npc Npc;
    private readonly ILogger Logger;

    public AIContext(Npc npc)
    {
        Npc = npc;
        Logger = Log.Logger.ForContext<AIContext>();
    }
}
