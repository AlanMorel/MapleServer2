using Maple2Storage.Types;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;

namespace MapleServer2.AIScripts.Default;

public class DefaultAttackState : AIState
{
    public override void OnEnter(Npc npc)
    {
        npc.Movement = MobMovement.Follow;
    }

    public override AIState? Execute(Npc npc)
    {
        if (npc.Target is null)
        {
            return new DefaultPatrolState();
        }

        float playerMobDist = CoordF.Distance(npc.Target.Coord, npc.Coord);

        if (playerMobDist > npc.Value.NpcMetadataDistance.Sight)
        {
            return new DefaultPatrolState();
        }

        if (playerMobDist >= npc.Value.NpcMetadataDistance.Avoid * 2)
        {
            npc.Animation = AnimationStorage.GetSequenceIdBySequenceName(npc.Value.NpcMetadataModel.Model, "Run_A");

            // Get coord between the player and the mob that is the avoid distance
            CoordF coord = CoordF.From(
                npc.Target.Coord.X + (npc.Coord.X - npc.Target.Coord.X) * npc.Value.NpcMetadataDistance.Avoid / playerMobDist,
                npc.Target.Coord.Y + (npc.Coord.Y - npc.Target.Coord.Y) * npc.Value.NpcMetadataDistance.Avoid / playerMobDist,
                npc.Target.Coord.Z + (npc.Coord.Z - npc.Target.Coord.Z) * npc.Value.NpcMetadataDistance.Avoid / playerMobDist
            );

            npc.Follow(coord);
            return null;
        }

        // TODO: Attack
        // npc.Attack();
        return null;
    }

    public override void OnExit(Npc npc) { }
}
