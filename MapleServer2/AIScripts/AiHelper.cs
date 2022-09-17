using MapleServer2.AIScripts._29000282;
using MapleServer2.Managers.Actors;

namespace MapleServer2.AIScripts;

public static class AiHelper
{
    public static readonly Dictionary<int, AIState> AIStates = new()
    {
        {
            29000282,
            new BelmaBehavior()
        }
    };
}
