using MapleServer2.Types;

namespace MapleServer2.AI.Functions;

public partial class AIContext
{
    public void SetUserValue(string key, int value)
    {
        IFieldActor<Player>? iFieldActor = Npc.Target;
        if (iFieldActor is null)
        {
            return;
        }

        PlayerTrigger? playerTrigger = iFieldActor!.Value.Triggers.FirstOrDefault(y => y.Key == key);
        if (playerTrigger is not null)
        {
            playerTrigger.Value = value;
            return;
        }

        iFieldActor.Value.Triggers.Add(new(key, value));
    }
}
