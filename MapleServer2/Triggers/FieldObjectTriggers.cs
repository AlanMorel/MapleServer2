using Maple2Storage.Tools;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Triggers
{
    public partial class TriggerContext
    {
        public void SetActor(int actorId, bool isVisible, string stateName, bool arg4, bool arg5)
        {
            Field.State.TriggerActors[actorId].IsVisible = isVisible;
            Field.State.TriggerActors[actorId].StateName = stateName;
            Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerActors[actorId]));
        }

        public void SetAgent(int[] arg1, bool arg2)
        {
        }

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
                if (Field.State.TriggerEffects.TryGetValue(triggerId, out TriggerEffect triggerEffect))
                {
                    triggerEffect.IsVisible = isVisible;
                    Field.BroadcastPacket(TriggerPacket.UpdateTrigger(triggerEffect));
                }
            }
        }

        public void SetInteractObject(int[] interactObjectIds, byte state, bool arg4, bool arg3)
        {
            InteractObjectState objectState = (InteractObjectState) state;
            foreach (int interactObjectId in interactObjectIds)
            {
                InteractObject interactObject = Field.State.InteractObjects.Values.FirstOrDefault(x => x.InteractId == interactObjectId);
                if (interactObject == null)
                {
                    continue;
                }
                interactObject.State = objectState;
                Field.BroadcastPacket(InteractObjectPacket.SetInteractObject(interactObject));
            }
        }

        public void SetLadder(int ladderId, bool isVisible, bool animationEffect, byte animationDelay)
        {
            Field.State.TriggerLadders[ladderId].IsVisible = isVisible;
            Field.State.TriggerLadders[ladderId].AnimationEffect = animationEffect;
            Field.State.TriggerLadders[ladderId].AnimationDelay = animationDelay;
            Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerLadders[ladderId]));
        }

        public void SetMesh(int[] meshIds, bool isVisible, int arg3, int arg4, float arg5)
        {
            foreach (int triggerMeshId in meshIds)
            {
                if (Field.State.TriggerMeshes.TryGetValue(triggerMeshId, out TriggerMesh triggerMesh))
                {
                    triggerMesh.IsVisible = isVisible;
                    Field.BroadcastPacket(TriggerPacket.UpdateTrigger(triggerMesh));
                }
            }
        }

        public void SetMeshAnimation(int[] arg1, bool arg2, byte arg3, byte arg4)
        {
        }

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

            IFieldObject<Portal> portal = Field.State.Portals.Values.First(p => p.Value.Id == portalId);
            if (portal == null)
            {
                return;
            }
            portal.Value.Update(visible, enabled, minimapVisible);
            Field.BroadcastPacket(FieldPacket.UpdatePortal(portal));
        }

        public void SetRandomMesh(int[] meshIds, bool isVisible, byte meshCount, int arg4, int delayTime)
        {
            Random random = RandomProvider.Get();
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
            foreach (int triggerId in triggerIds)
            {
                IFieldObject<TriggerSkill> triggerSkill = Field.State.GetTriggerSkill(triggerId);
                if (triggerSkill != null)
                {
                    //TODO: Do skillcast once skill manager can cast skills by id
                    //eventually we want to be able to do something like:
                    //SkillManager.SkillCast(id) and the skillcast function takes care 
                    //of sending the correct skill type / packet
                }
            }
        }

        public void SetSound(int soundId, bool isEnabled)
        {
            Field.State.TriggerSounds[soundId].IsEnabled = isEnabled;
            Field.BroadcastPacket(TriggerPacket.UpdateTrigger(Field.State.TriggerSounds[soundId]));
        }

        public void SetVisibleBreakableObject(int[] arg1, bool arg2)
        {
        }

        public void CreateItem(int[] arg1, int arg2, int arg3, int arg5)
        {
        }

        public void SpawnItemRange(int[] rangeId, byte randomPickCount)
        {
        }
    }
}
