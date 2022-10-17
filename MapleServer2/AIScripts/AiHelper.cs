using MapleServer2.AIScripts._29000282;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;

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
