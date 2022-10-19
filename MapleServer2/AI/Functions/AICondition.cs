using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Types;

namespace MapleServer2.AI.Functions;

public partial class AIContext
{
    public bool CheckHPThreshold(int threshold)
    {
        return Npc.Stats[StatAttribute.Hp].Total <= (Npc.Stats[StatAttribute.Hp].Bonus * threshold / 100);
    }

    public bool OutOfSight()
    {
        if (Npc.Target is null)
        {
            return true;
        }

        float playerMobDist = GetDistanceToTarget();
        return playerMobDist > Npc.Value.NpcMetadataDistance.Sight * 2;
    }

    public bool InSight()
    {
        if (Npc.Target is null)
        {
            return true;
        }

        float playerMobDist = GetDistanceToTarget();
        return playerMobDist < Npc.Value.NpcMetadataDistance.Sight * 2;
    }

    public float GetDistanceToNextCoord()
    {
        return Npc.Distance;
    }

    public long GetTickSinceLastMovement()
    {
        return Environment.TickCount - Npc.LastMovementTime;
    }

    public IFieldActor? GetTarget()
    {
        return Npc.Target;
    }

    public float GetDistanceToTarget()
    {
        if (Npc.Target is null)
        {
            return 0;
        }

        return CoordF.Distance(Npc.Coord, Npc.Target.Coord);
    }
}
