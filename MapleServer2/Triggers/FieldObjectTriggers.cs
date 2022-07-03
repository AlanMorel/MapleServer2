using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Triggers;

public partial class TriggerContext
{
    public void SetActor(int actorId, bool isVisible, string stateName, bool arg4, bool arg5)
    {
        if (!Field.State.TriggerActors.TryGetValue(actorId, out TriggerActor stateTriggerActor))
        {
            Logger.Warning("SetActor: Actor not found: {0}", actorId);
            return;
        }

        stateTriggerActor.IsVisible = isVisible;
        stateTriggerActor.StateName = stateName;
        Field.BroadcastPacket(TriggerPacket.UpdateTrigger(stateTriggerActor));
    }

    public void SetAgent(int[] arg1, bool arg2) { }

    public void SetCube(int[] triggerIds, bool isVisible, byte randomCount)
    {
        foreach (int triggerId in triggerIds)
        {
            Field.State.TriggerCubes[triggerId].IsVisible = isVisible;
            Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerCubes[triggerId]));
        }
    }

    public void SetEffect(int[] triggerIds, bool isVisible, int arg3, byte arg4)
    {
        foreach (int triggerId in triggerIds)
        {
            if (!Field.State.TriggerEffects.TryGetValue(triggerId, out TriggerEffect triggerEffect))
            {
                continue;
            }

            bool oldValue = triggerEffect.IsVisible;
            triggerEffect.IsVisible = isVisible;

            if (oldValue != isVisible) // If the value changed, broadcast the packet.
            {
                Field.BroadcastPacket(TriggerPacket.UpdateTrigger(triggerEffect));
            }
        }
    }

    public void SetInteractObject(int[] interactObjectIds, byte state, bool arg4, bool arg3)
    {
        InteractObjectState objectState = (InteractObjectState) state;
        foreach (int interactObjectId in interactObjectIds)
        {
            IFieldObject<InteractObject> interactObject = Field.State.InteractObjects.Values.FirstOrDefault(x => x.Value.InteractId == interactObjectId);
            if (interactObject == null)
            {
                continue;
            }

            interactObject.Value.State = objectState;
            Field.BroadcastPacket(InteractObjectPacket.Set(interactObject.Value));
        }
    }

    public void SetLadder(int ladderId, bool isVisible, bool animationEffect, byte animationDelay)
    {
        if (!Field.State.TriggerLadders.TryGetValue(ladderId, out TriggerLadder ladder))
        {
            return;
        }

        ladder.IsVisible = isVisible;
        ladder.AnimationEffect = animationEffect;
        ladder.AnimationDelay = animationDelay;
        Field.BroadcastPacket(TriggerPacket.UpdateTrigger(ladder));
    }

    public void SetMesh(int[] meshIds, bool isVisible, int arg3, int delay, float arg5)
    {
        Task.Run(async () =>
        {
            foreach (int triggerMeshId in meshIds)
            {
                if (!Field.State.TriggerMeshes.TryGetValue(triggerMeshId, out TriggerMesh triggerMesh))
                {
                    continue;
                }

                triggerMesh.IsVisible = isVisible;
                Field.BroadcastPacket(TriggerPacket.UpdateTrigger(triggerMesh));
                await Task.Delay(delay);
            }
        });
    }

    public void SetMeshAnimation(int[] arg1, bool arg2, byte arg3, byte arg4) { }

    public void SetBreakable(int[] triggerIds, bool isEnabled)
    {
        foreach (int triggerId in triggerIds)
        {
            BreakableNifObject breakable = Field.State.BreakableNifs.Values.FirstOrDefault(x => x.TriggerId == triggerId);
            if (breakable == null)
            {
                continue;
            }

            breakable.IsEnabled = isEnabled;
            Field.BroadcastPacket(BreakablePacket.Interact(breakable));
        }
    }

    public void SetPortal(int portalId, bool visible, bool enabled, bool minimapVisible, bool arg5)
    {
        if (Field.State.Portals.IsEmpty)
        {
            return;
        }

        IFieldObject<Portal> portal = Field.State.Portals.Values.FirstOrDefault(p => p.Value.Id == portalId);
        if (portal == null)
        {
            return;
        }

        portal.Value.Update(visible, enabled, minimapVisible);
        Field.BroadcastPacket(FieldPortalPacket.UpdatePortal(portal));
    }

    public void SetRandomMesh(int[] meshIds, bool isVisible, byte meshCount, int arg4, int delayTime)
    {
        Random random = Random.Shared;
        int[] pickedMeshIds = meshIds.OrderBy(x => random.Next()).Take(meshCount).ToArray();
        Task.Run(async () =>
        {
            foreach (int triggerMeshId in pickedMeshIds)
            {
                Field.State.TriggerMeshes[triggerMeshId].IsVisible = isVisible;
                Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerMeshes[triggerMeshId]));
                await Task.Delay(delayTime);
            }
        });
    }

    public void SetRope(int ropeId, bool isVisible, bool animationEffect, byte animationDelay)
    {
        Field.State.TriggerRopes[ropeId].IsVisible = isVisible;
        Field.State.TriggerRopes[ropeId].AnimationEffect = animationEffect;
        Field.State.TriggerRopes[ropeId].AnimationDelay = animationDelay;
        Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerLadders[ropeId]));
    }

    public void SetSkill(int[] triggerIds, bool arg2)
    {
        if (arg2 is false) // Not sure what this means
        {
            return;
        }

        foreach (int triggerId in triggerIds)
        {
            IFieldObject<TriggerSkill> triggerSkill = Field.State.GetTriggerSkill(triggerId);
            if (triggerSkill != null)
            {
                // this is 100% not perfect.
                SkillCast skillCast = new(triggerSkill.Value.SkillId, triggerSkill.Value.SkillLevel, GuidGenerator.Long(), Environment.TickCount)
                {
                    SkillObjectId = triggerSkill.ObjectId,
                    CasterObjectId = triggerSkill.ObjectId,
                    Position = triggerSkill.Coord
                };
                RegionSkillHandler.HandleEffect(Field, skillCast, 0);
            }
        }
    }

    public void SetSound(int soundId, bool isEnabled)
    {
        Field.State.TriggerSounds[soundId].IsEnabled = isEnabled;
        Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerSounds[soundId]));
    }

    public void SetVisibleBreakableObject(int[] arg1, bool arg2) { }

    public void CreateItem(int[] arg1, int arg2, int arg3, int arg5) { }

    public void SpawnItemRange(int[] rangeId, byte randomPickCount) { }
}
