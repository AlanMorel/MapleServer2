using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game {
    // TODO: This needs to be thread safe
    // TODO: FieldManager probably needs its own thread to send updates about user position
    // This seems to be done every ~2s rather than on every update.
    public class FieldManager {
        private int counter = 10000000;

        public readonly int MapId;
        public readonly FieldState State = new FieldState();
        private readonly HashSet<GameSession> sessions = new HashSet<GameSession>();

        public FieldManager(int mapId) {
            this.MapId = mapId;

            // Load default npcs for map from config
            foreach (MapNpc npc in MapEntityStorage.GetNpcs(mapId)) {
                IFieldObject<Npc> fieldNpc = RequestFieldObject(new Npc(npc.Id) {
                    Rotation = (short)(npc.Rotation.Z * 10)
                });
                fieldNpc.Coord = npc.Coord.ToFloat();
                AddTestNpc(fieldNpc);
            }

            // Load default portals for map from config
            foreach (MapPortal portal in MapEntityStorage.GetPortals(mapId)) {
                IFieldObject<Portal> fieldPortal = RequestFieldObject(new Portal(portal.Id) {
                    IsVisible = portal.Flags.HasFlag(MapPortalFlag.Visible),
                    IsEnabled = portal.Flags.HasFlag(MapPortalFlag.Enabled),
                    IsMinimapVisible = portal.Flags.HasFlag(MapPortalFlag.MinimapVisible),
                    Rotation = portal.Rotation.ToFloat(),
                    TargetMapId = portal.Target,
                });
                fieldPortal.Coord = portal.Coord.ToFloat();
                AddPortal(fieldPortal);
            }
        }

        // Gets a list of packets to update the state of all field objects for client.
        public IEnumerable<Packet> GetUpdates() {
            List<Packet> updates = new List<Packet>();
            // Update players locations
            // Update NPCs
            foreach (IFieldObject<Npc> npc in State.Npcs.Values) {
                updates.Add(FieldObjectPacket.ControlNpc(npc));
            }
            foreach (IFieldObject<Player> player in State.Players.Values) {
                updates.Add(FieldObjectPacket.UpdatePlayer(player));
            }

            return updates;
        }

        public IFieldObject<T> RequestFieldObject<T>(T player) {
            return WrapObject(player);
        }

        public void AddPlayer(GameSession sender, IFieldObject<Player> player) {
            Debug.Assert(player.ObjectId > 0, "Player was added to field without initialized objectId.");
            player.Coord = player.Value.Coord;
            player.Value.MapId = MapId;
            // TODO: Determine new coordinates for player as well
            lock (sessions) {
                sessions.Add(sender);
            }

            // TODO: Send the initialization state of the field
            foreach (IFieldObject<Player> existingPlayer in State.Players.Values) {
                sender.Send(FieldPacket.AddPlayer(existingPlayer));
                sender.Send(FieldObjectPacket.LoadPlayer(existingPlayer));
            }
            foreach (IFieldObject<Item> existingItem in State.Items.Values) {
                sender.Send(FieldPacket.AddItem(existingItem, 123456));
            }
            foreach (IFieldObject<Npc> existingNpc in State.Npcs.Values) {
                sender.Send(FieldPacket.AddNpc(existingNpc));
                sender.Send(FieldObjectPacket.LoadNpc(existingNpc));
            }
            foreach (IFieldObject<Portal> existingPortal in State.Portals.Values) {
                sender.Send(FieldPacket.AddPortal(existingPortal));
            }
            State.AddPlayer(player);

            // Broadcast new player to all players in map
            Broadcast(session => {
                session.Send(FieldPacket.AddPlayer(player));
                session.Send(FieldObjectPacket.LoadPlayer(player));
            });
        }

        public void RemovePlayer(GameSession sender, IFieldObject<Player> player) {
            lock (sessions) {
                sessions.Remove(sender);
            }
            State.RemovePlayer(player.ObjectId);

            // Remove player
            Broadcast(session => {
                session.Send(FieldPacket.RemovePlayer(player));
            });

            ((FieldObject<Player>) player).ObjectId = -1; // Reset object id
        }

        // Spawned NPCs will not appear until controlled
        public void AddNpc(GameSession sender, Npc npc) {
            FieldObject<Npc> fieldNpc = WrapObject(npc);
            fieldNpc.Coord = sender.FieldPlayer.Coord; // Spawn NPC on player for now

            State.AddNpc(fieldNpc);

            Broadcast(session => {
                session.Send(FieldPacket.AddNpc(fieldNpc));
                session.Send(FieldObjectPacket.LoadNpc(fieldNpc));
            });
        }

        public void AddTestNpc(IFieldObject<Npc> fieldNpc) {
            State.AddNpc(fieldNpc);

            Broadcast(session => {
                session.Send(FieldPacket.AddNpc(fieldNpc));
                session.Send(FieldObjectPacket.LoadNpc(fieldNpc));
            });
        }

        public void AddPortal(IFieldObject<Portal> portal) {
            State.AddPortal(portal);
            BroadcastPacket(FieldPacket.AddPortal(portal));
        }

        public void SendChat(Player player, string message) {
            Broadcast(session => {
                session.Send(ChatPacket.Send(player, message, ChatType.All));
            });
        }

        public void AddItem(GameSession sender, Item item) {
            FieldObject<Item> fieldItem = WrapObject(item);
            fieldItem.Coord = sender.FieldPlayer.Coord;

            State.AddItem(fieldItem);

            Broadcast(session => {
                session.Send(FieldPacket.AddItem(fieldItem, session.FieldPlayer.ObjectId));
            });
        }

        public bool RemoveItem(int objectId, out Item item) {
            if (!State.RemoveItem(objectId, out item)) {
                return false;
            }

            Broadcast(session => {
                session.Send(FieldPacket.PickupItem(objectId, session.FieldPlayer.ObjectId));
                session.Send(FieldPacket.RemoveItem(objectId));
            });
            return true;
        }

        // Providing a session will result in packet not being broadcast to self.
        public void BroadcastPacket(Packet packet, GameSession sender = null) {
            Broadcast(session => {
                if (session == sender) return;
                session.Send(packet);
            });
        }

        // Broadcasts packets to all sessions.
        // TODO: This should be optimized to avoid regenerating packets for each session.
        private void Broadcast(Action<GameSession> action) {
            lock (sessions) {
                foreach (GameSession session in sessions) {
                    action?.Invoke(session);
                }
            }
        }

        // Initializes a FieldObject with an objectId for this field.
        private FieldObject<T> WrapObject<T>(T fieldObject) {
            int objectId = Interlocked.Increment(ref counter);
            return new FieldObject<T>(objectId, fieldObject);
        }

        // This class is private to ensure that callers must first request entry.
        private class FieldObject<T> : IFieldObject<T> {
            public int ObjectId { get; set; }
            public T Value { get; }

            public CoordF Coord { get; set; }

            public FieldObject(int objectId, T value) {
                this.ObjectId = objectId;
                this.Value = value;
            }
        }
    }
}