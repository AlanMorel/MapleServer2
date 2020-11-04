using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using MapleServer2.Types.FieldObjects;
using MapleServer2.Types.Npcs;
using Maple2.Data.Types.Items;

namespace MapleServer2.Types.Skills {
    // All operations on this class should be thread safe
    public class FieldState {
        public IReadOnlyDictionary<int, IFieldObject<Item>> Items => items;
        public IReadOnlyDictionary<int, IFieldObject<Player>> Players => players;
        public IReadOnlyDictionary<int, IFieldObject<Npc>> Npcs => npcs;
        public IReadOnlyDictionary<int, IFieldObject<Portal>> Portals => portals;

        private readonly ConcurrentDictionary<int, IFieldObject<Item>> items;
        private readonly ConcurrentDictionary<int, IFieldObject<Player>> players;
        private readonly ConcurrentDictionary<int, IFieldObject<Npc>> npcs;
        private readonly ConcurrentDictionary<int, IFieldObject<Portal>> portals;

        public FieldState() {
            this.items = new ConcurrentDictionary<int, IFieldObject<Item>>();
            this.players = new ConcurrentDictionary<int, IFieldObject<Player>>();
            this.npcs = new ConcurrentDictionary<int, IFieldObject<Npc>>();
            this.portals = new ConcurrentDictionary<int, IFieldObject<Portal>>();
        }

        public bool TryGetItem(int objectId, out IFieldObject<Item> item) {
            return items.TryGetValue(objectId, out item);
        }

        public void AddItem(IFieldObject<Item> item) {
            items[item.ObjectId] = item;
        }

        public bool RemoveItem(int objectId, out Item item) {
            bool result = items.Remove(objectId, out IFieldObject<Item> fieldItem);
            item = fieldItem?.Value;

            return result;
        }

        public void AddPlayer(IFieldObject<Player> player) {
            players[player.ObjectId] = player;
        }

        public bool RemovePlayer(int objectId) {
            return players.Remove(objectId, out _);
        }

        public void AddNpc(IFieldObject<Npc> npc) {
            npcs[npc.ObjectId] = npc;
        }

        public bool RemoveNpc(int objectId) {
            return npcs.Remove(objectId, out _);
        }

        public void AddPortal(IFieldObject<Portal> portal) {
            portals[portal.ObjectId] = portal;
        }

        public bool RemovePortal(int objectId) {
            return portals.Remove(objectId, out _);
        }
    }
}