using System.Collections.Concurrent;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

// All operations on this class should be thread safe
public class FieldState
{
    public readonly ConcurrentDictionary<int, IFieldObject<Item>> Items = new();
    public readonly ConcurrentDictionary<int, IFieldObject<Portal>> Portals = new();
    public readonly ConcurrentDictionary<int, IFieldObject<MobSpawn>> MobSpawns = new();
    public readonly ConcurrentDictionary<int, IFieldActor<Player>> Players = new();
    public readonly ConcurrentDictionary<int, IFieldActor<NpcMetadata>> Npcs = new();
    public readonly ConcurrentDictionary<int, IFieldActor<NpcMetadata>> Mobs = new();
    public readonly ConcurrentDictionary<int, IFieldObject<GuideObject>> Guide = new();
    public readonly ConcurrentDictionary<int, IFieldObject<Cube>> Cubes = new();
    public readonly ConcurrentDictionary<int, IFieldObject<HealingSpot>> HealingSpots = new();
    public readonly ConcurrentDictionary<int, IFieldObject<Instrument>> Instruments = new();
    public readonly ConcurrentDictionary<int, TriggerMesh> TriggerMeshes = new();
    public readonly ConcurrentDictionary<int, TriggerEffect> TriggerEffects = new();
    public readonly ConcurrentDictionary<int, TriggerCamera> TriggerCameras = new();
    public readonly ConcurrentDictionary<int, TriggerActor> TriggerActors = new();
    public readonly ConcurrentDictionary<int, TriggerCube> TriggerCubes = new();
    public readonly ConcurrentDictionary<int, TriggerLadder> TriggerLadders = new();
    public readonly ConcurrentDictionary<int, TriggerRope> TriggerRopes = new();
    public readonly ConcurrentDictionary<int, TriggerSound> TriggerSounds = new();
    public readonly ConcurrentDictionary<string, BreakableActorObject> BreakableActors = new();
    public readonly ConcurrentDictionary<string, BreakableNifObject> BreakableNifs = new();
    public readonly ConcurrentDictionary<int, IFieldObject<TriggerSkill>> TriggerSkills = new();
    public readonly ConcurrentDictionary<string, InteractObject> InteractObjects = new();
    public readonly ConcurrentDictionary<string, LiftableObject> LiftableObjects = new();

    public bool TryGetItem(int objectId, out IFieldObject<Item> item)
    {
        return Items.TryGetValue(objectId, out item);
    }

    public void AddItem(IFieldObject<Item> item)
    {
        Items[item.ObjectId] = item;
    }

    public bool RemoveItem(int objectId, out Item item)
    {
        bool result = Items.Remove(objectId, out IFieldObject<Item> fieldItem);
        item = fieldItem?.Value;

        return result;
    }

    public void AddPlayer(IFieldActor<Player> player)
    {
        Players[player.ObjectId] = player;
    }

    public bool RemovePlayer(int objectId)
    {
        return Players.Remove(objectId, out _);
    }

    public void AddNpc(IFieldActor<NpcMetadata> npc)
    {
        Npcs[npc.ObjectId] = npc;
    }

    public bool RemoveNpc(int objectId)
    {
        return Npcs.Remove(objectId, out _);
    }

    public void AddPortal(IFieldObject<Portal> portal)
    {
        Portals[portal.ObjectId] = portal;
    }

    public bool RemovePortal(int objectId)
    {
        return Portals.Remove(objectId, out _);
    }

    public bool RemoveBalloon(string name)
    {
        return InteractObjects.Remove(name, out _);
    }

    public void AddGuide(IFieldObject<GuideObject> guide)
    {
        Guide[guide.ObjectId] = guide;
    }

    public bool RemoveGuide(int objectId)
    {
        return Guide.Remove(objectId, out _);
    }

    public void AddCube(IFieldObject<Cube> ugcCube)
    {
        Cubes[ugcCube.ObjectId] = ugcCube;
    }

    public bool RemoveCube(int objectId)
    {
        return Cubes.Remove(objectId, out _);
    }

    public void AddInstrument(IFieldObject<Instrument> instrument)
    {
        Instruments[instrument.ObjectId] = instrument;
    }

    public bool RemoveInstrument(int objectId)
    {
        return Instruments.Remove(objectId, out _);
    }

    public void AddMobSpawn(IFieldObject<MobSpawn> spawn)
    {
        MobSpawns[spawn.ObjectId] = spawn;
    }

    public bool RemoveMobSpawn(int objectId)
    {
        return MobSpawns.Remove(objectId, out _);
    }

    public void AddMob(IFieldActor<NpcMetadata> mob)
    {
        Mobs[mob.ObjectId] = mob;
    }

    public bool RemoveMob(int objectId)
    {
        return Mobs.Remove(objectId, out _);
    }

    public void AddHealingSpot(IFieldObject<HealingSpot> healingSpot)
    {
        HealingSpots[healingSpot.ObjectId] = healingSpot;
    }

    public void AddTriggerObject(TriggerObject triggerObject)
    {
        switch (triggerObject)
        {
            case TriggerMesh triggerMesh:
                TriggerMeshes[triggerMesh.Id] = triggerMesh;
                break;
            case TriggerEffect triggerEffect:
                TriggerEffects[triggerEffect.Id] = triggerEffect;
                break;
            case TriggerCamera triggerCamera:
                TriggerCameras[triggerCamera.Id] = triggerCamera;
                break;
            case TriggerActor triggerActor:
                TriggerActors[triggerActor.Id] = triggerActor;
                break;
            case TriggerCube triggerCube:
                TriggerCubes[triggerCube.Id] = triggerCube;
                break;
            case TriggerLadder triggerLadder:
                TriggerLadders[triggerLadder.Id] = triggerLadder;
                break;
            case TriggerRope triggerRope:
                TriggerRopes[triggerRope.Id] = triggerRope;
                break;
            case TriggerSound triggerSound:
                TriggerSounds[triggerSound.Id] = triggerSound;
                break;
        }
    }

    public void AddBreakable(BreakableObject breakable)
    {
        switch (breakable)
        {
            case BreakableActorObject actor:
                BreakableActors[actor.Id] = actor;
                break;
            case BreakableNifObject nif:
                BreakableNifs[nif.Id] = nif;
                break;
        }
    }

    public void AddTriggerSkills(IFieldObject<TriggerSkill> triggerSkill)
    {
        TriggerSkills[triggerSkill.ObjectId] = triggerSkill;
    }

    public IFieldObject<TriggerSkill> GetTriggerSkill(int triggerId)
    {
        return TriggerSkills.FirstOrDefault(skill => skill.Value.Value.Id == triggerId).Value;
    }

    public void AddInteractObject(InteractObject interactObject)
    {
        InteractObjects[interactObject.Id] = interactObject;
    }

    public void RemoveInteractObject(string interactObjectId)
    {
        InteractObjects.Remove(interactObjectId, out _);
    }

    public void AddLiftableObject(LiftableObject liftableObject)
    {
        LiftableObjects[liftableObject.EntityId] = liftableObject;
    }
}
