using Maple2Storage.Types;
using MapleServer2.Data.Static;
using MapleServer2.Managers.Actors;
using MapleServer2.Types;

namespace MapleServer2.AIScripts;

public class PrisonGovernor2Battle : AIState
{
    public override void OnEnter(Npc npc)
    {
        // Ok to assume this will only happen on tutorial, so always attack the only player available
        npc.Target = npc.FieldManager!.State.Players.FirstOrDefault().Value;
    }

    public override AIState? Execute(Npc npc)
    {
        if (npc.CheckHPThreshold(50))
        {
            return new BattleStop();
        }

        float playerMobDist = CoordF.Distance(npc.Target!.Coord, npc.Coord);

        if (playerMobDist > npc.Value.NpcMetadataDistance.Sight)
        {
            return null;
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

public class BattleStop : AIState
{
    public override void OnEnter(Npc npc) { }

    public override AIState? Execute(Npc npc)
    {
        PlayerTrigger? playerTrigger = npc.Target!.Value.Triggers.FirstOrDefault(y => y.Key == "battleStop");
        if (playerTrigger is not null)
        {
            playerTrigger.Value = 1;
            return null;
        }

        npc.Target.Value.Triggers.Add(new("battleStop", 1));
        return null;
    }

    public override void OnExit(Npc npc) { }
}
