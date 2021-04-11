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
        public readonly ConcurrentDictionary<string, IFieldObject<InteractActor>> InteractActors;
        public readonly ConcurrentDictionary<int, IFieldObject<GuideObject>> Guide;

        public FieldState()
        {
            Items = new ConcurrentDictionary<int, IFieldObject<Item>>();
            Players = new ConcurrentDictionary<int, IFieldObject<Player>>();
            Npcs = new ConcurrentDictionary<int, IFieldObject<Npc>>();
            Portals = new ConcurrentDictionary<int, IFieldObject<Portal>>();
            MobSpawns = new ConcurrentDictionary<int, IFieldObject<MobSpawn>>();
            Mobs = new ConcurrentDictionary<int, IFieldObject<Mob>>();
            InteractActors = new ConcurrentDictionary<string, IFieldObject<InteractActor>>();
            Guide = new ConcurrentDictionary<int, IFieldObject<GuideObject>>();
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

        public void AddInteractActor(IFieldObject<InteractActor> actor)
        {
            InteractActors[actor.Value.Uuid] = actor;
        }

        public void AddGuide(IFieldObject<GuideObject> guide)
        {
            Guide[guide.ObjectId] = guide;
        }

        public bool RemoveGuide(int objectId)
        {
            return Guide.Remove(objectId, out _);
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
    }
}
