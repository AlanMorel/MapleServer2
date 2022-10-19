using MapleServer2.Types;

namespace MapleServer2.AIScripts;

public static class AiHelper
{
    public static readonly Dictionary<string, AIState> AIStates = new()
    {
        {
            "AI_DefaultNew.xml",
            new DefaultNew()
        },
        {
            "AI_PrisonGovernor2_battle.xml",
            new PrisonGovernor2Battle()
        }
    };

    public static AIState GetAIState(string aiName)
    {
        return AIStates.ContainsKey(aiName) ? AIStates[aiName] : new DefaultNew();
    }
}
