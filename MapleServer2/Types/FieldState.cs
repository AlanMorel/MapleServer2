using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MapleServer2.Types
{
    // All operations on this class should be thread safe
    public class FieldState
    {
        public readonly ConcurrentDictionary<int, IFieldObject<Item>> Items;
        public readonly ConcurrentDictionary<int, IFieldObject<Player>> Players;
        public readonly ConcurrentDictionary<int, IFieldObject<Npc>> Npcs;
        public readonly ConcurrentDictionary<int, IFieldObject<Portal>> Portals;
        public readonly ConcurrentDictionary<int, IFieldObject<MobSpawn>> MobSpawns;
        public readonly ConcurrentDictionary<int, IFieldObject<Mob>> Mobs;
        public readonly ConcurrentDictionary<string, IFieldObject<InteractObject>> InteractObjects;
        public readonly ConcurrentDictionary<int, IFieldObject<GuideObject>> Guide;
        public readonly ConcurrentDictionary<int, IFieldObject<Cube>> Cubes;
        public readonly ConcurrentDictionary<int, IFieldObject<HealingSpot>> HealingSpots;
        public readonly ConcurrentDictionary<string, IFieldObject<InteractAdBalloon>> Balloons;

        public FieldState()
        {
            Items = new ConcurrentDictionary<int, IFieldObject<Item>>();
            Players = new ConcurrentDictionary<int, IFieldObject<Player>>();
            Npcs = new ConcurrentDictionary<int, IFieldObject<Npc>>();
            Portals = new ConcurrentDictionary<int, IFieldObject<Portal>>();
            MobSpawns = new ConcurrentDictionary<int, IFieldObject<MobSpawn>>();
            Mobs = new ConcurrentDictionary<int, IFieldObject<Mob>>();
            InteractObjects = new ConcurrentDictionary<string, IFieldObject<InteractObject>>();
            Guide = new ConcurrentDictionary<int, IFieldObject<GuideObject>>();
            Cubes = new ConcurrentDictionary<int, IFieldObject<Cube>>();
            HealingSpots = new ConcurrentDictionary<int, IFieldObject<HealingSpot>>();
            Balloons = new ConcurrentDictionary<string, IFieldObject<InteractAdBalloon>>();
        }

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

        public void AddPlayer(IFieldObject<Player> player)
        {
            Players[player.ObjectId] = player;
        }

        public bool RemovePlayer(int objectId)
        {
            return Players.Remove(objectId, out _);
        }

        public void AddNpc(IFieldObject<Npc> npc)
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

        public void AddInteractObject(IFieldObject<InteractObject> interactObject)
        {
            InteractObjects[interactObject.Value.Uuid] = interactObject;
        }

        public void AddBalloon(IFieldObject<InteractAdBalloon> balloon)
        {
            Balloons[balloon.Value.Name] = balloon;
        }

        public bool RemoveBalloon(string name)
        {
            return Balloons.Remove(name, out _);
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

        public void AddMobSpawn(IFieldObject<MobSpawn> spawn)
        {
            MobSpawns[spawn.ObjectId] = spawn;
        }

        public bool RemoveMobSpawn(int objectId)
        {
            return MobSpawns.Remove(objectId, out _);
        }

        public void AddMob(IFieldObject<Mob> mob)
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
    }
}
