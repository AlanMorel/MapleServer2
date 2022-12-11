using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Managers;

namespace MapleServer2.Types;

public class ProximityQuery
{
    public int TargetRange = 0;
    public int TargetMinRange = 0;
    public TargetAllieganceType Type = TargetAllieganceType.ClosestEnemy;
    public int TargetCount = 0;
}

public class ProximityTracker
{
    public IFieldActor Parent;
    public List<ProximityQuery> Queries = new();

    public ProximityTracker(IFieldActor parent)
    {
        Parent = parent;
    }

    private void CheckTarget(ProximityQuery query, IFieldActor target)
    {
        CoordF offset = Parent.Coord - target.Coord;
        float distance = CoordF.Dot(offset, offset);

        if (distance >= query.TargetMinRange * query.TargetMinRange && distance <= query.TargetRange * query.TargetRange)
        {
            ++query.TargetCount;
        }
    }

    public void Update()
    {
        if (Parent.FieldManager is null)
        {
            return;
        }

        foreach (ProximityQuery query in Queries)
        {
            query.TargetCount = 0;

            if (query.Type == TargetAllieganceType.ClosestEnemy)
            {
                foreach ((int id, IFieldActor target) in Parent.FieldManager.State.Mobs)
                {
                    CheckTarget(query, target);
                }
            }

            if (query.Type == TargetAllieganceType.ClosestAlly)
            {
                foreach ((int id, IFieldActor target) in Parent.FieldManager.State.Players)
                {
                    CheckTarget(query, target);
                }
            }
        }
    }
}
