using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.AI.Functions;

public partial class AIContext
{
    public void SetMovement(MobMovement movement)
    {
        Npc.Movement = movement;
    }

    public bool TryDefineTarget()
    {
        if (Npc.FieldManager is null)
        {
            return false;
        }

        // Manage mob aggro + targets
        foreach (IFieldActor<Player> player in Npc.FieldManager.State.Players.Values)
        {
            float playerMobDist = CoordF.Distance(player.Coord, Npc.Coord);
            if (playerMobDist <= Npc.Value.NpcMetadataDistance.Sight)
            {
                Npc.State = NpcState.Combat;
                Npc.Target = player;
                return true;
            }

            if (Npc.State != NpcState.Combat)
            {
                continue;
            }

            Npc.State = NpcState.Normal;
            Npc.Target = null;
        }

        return false;
    }

    public void Patrol()
    {
        NpcActionChance actionChance = Npc.GetRandomAction();
        Npc.Action = actionChance.Action;
        Npc.Animation = AnimationStorage.GetSequenceIdBySequenceName(Npc.Value.NpcMetadataModel.Model, actionChance.Id);

        if (actionChance.Action == NpcAction.Walk)
        {
            Npc.Patrol();
        }
    }

    public void Follow()
    {
        Npc.Animation = AnimationStorage.GetSequenceIdBySequenceName(Npc.Value.NpcMetadataModel.Model, "Run_A");

        // Get coord between the player and the mob that is the avoid distance
        // not sure if this is the correct way to do it
        float distanceToTarget = GetDistanceToTarget();
        CoordF coord = CoordF.From(
            Npc.Target!.Coord.X + (Npc.Coord.X - Npc.Target.Coord.X) * Npc.Value.NpcMetadataDistance.Avoid / distanceToTarget,
            Npc.Target.Coord.Y + (Npc.Coord.Y - Npc.Target.Coord.Y) * Npc.Value.NpcMetadataDistance.Avoid / distanceToTarget,
            Npc.Target.Coord.Z + (Npc.Coord.Z - Npc.Target.Coord.Z) * Npc.Value.NpcMetadataDistance.Avoid / distanceToTarget
        );

        Npc.Follow(coord);
    }

    public void DefineTargetToFirstPlayer()
    {
        Npc.Target = Npc.FieldManager!.State.Players.FirstOrDefault().Value;
    }
}
