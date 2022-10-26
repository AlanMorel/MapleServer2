using System.Reflection;
using MapleServer2.AI.Functions;
using MapleServer2.AI.Scripts;
using MapleServer2.Types;

namespace MapleServer2.AI;

public static class AIHelper
{
    public static AIState GetAIState(string aiName, AIContext context)
    {
        Type? type = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(t => t.Name == aiName);

        if (type is null)
        {
            return new AI_DefaultNew(context);
        }

        return Activator.CreateInstance(type, context) as AIState ?? throw new InvalidOperationException($"AIState is null. AiName: {aiName}");
    }
}
