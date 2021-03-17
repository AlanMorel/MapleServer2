using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;
using MapleServer2.Tools;

namespace MapleServer2.Servers.Game
{
    // TODO: This needs to be thread safe
    // TODO: FieldManager probably needs its own thread to send updates about user position
    // This seems to be done every ~2s rather than on every update.
    public class FieldManager
    {
        private int Counter = 10000000;

        public readonly int MapId;
        public readonly CoordS[] BoundingBox;
        public readonly FieldState State = new FieldState();
        private readonly HashSet<GameSession> Sessions = new HashSet<GameSession>();

        private Task HealingSpotThread;

        public FieldManager(int mapId)
        {
            MapId = mapId;
            BoundingBox = MapEntityStorage.GetBoundingBox(mapId);
            // Load default npcs for map from config
            foreach (MapNpc npc in MapEntityStorage.GetNpcs(mapId))
            {
                IFieldObject<Npc> fieldNpc = RequestFieldObject(new Npc(npc.Id)
                {
                    ZRotation = (short) (npc.Rotation.Z * 10)
                });

                if (fieldNpc.Value.Friendly == 2)
                {
                    fieldNpc.Coord = npc.Coord.ToFloat();
                    AddNpc(fieldNpc);
                }
                else
                {
                    // NPC is an enemy
                    IFieldObject<Mob> fieldMob = RequestFieldObject(new Mob(npc.Id)
                    {
                        ZRotation = (short) (npc.Rotation.Z * 10)
                    });

                    fieldMob.Coord = npc.Coord.ToFloat();
                    AddMob(fieldMob);
                }
            }

            // Spawn map's mobs at the mob spawners
            foreach (MapMobSpawn mobSpawn in MapEntityStorage.GetMobSpawns(mapId))
            {
                if (mobSpawn.SpawnData == null)
                {
                    Debug.WriteLine($"Missing mob spawn data: {mobSpawn}");
                    continue;
                }
                int maxPopulation = mobSpawn.NpcCount;
                List<CoordF> spawnPoints = SpawnGenerator.Points(mobSpawn.SpawnRadius);
                List<NpcMetadata> mobs = SpawnGenerator.Mobs(mobSpawn.SpawnData.Difficulty, mobSpawn.SpawnData.MinDifficulty, mobSpawn.SpawnData.Tags);

                int population = 0;
                foreach (NpcMetadata mob in mobs)
                {
                    int spawnCount = mob.NpcMetadataBasic.GroupSpawnCount;  // Spawn count changes due to field effect (?)
                    if (spawnCount > maxPopulation)
                    {
                        break;
                    }

                    for (int i = 0; i < spawnCount; i++)
                    {
                        IFieldObject<Mob> fieldMob = RequestFieldObject(new Mob(mob.Id));
                        fieldMob.Coord = mobSpawn.Coord.ToFloat() + spawnPoints[population % spawnPoints.Count];
                        AddMob(fieldMob);
                        population++;
                    }
                }
            }

            // Load default portals for map from config
            foreach (MapPortal portal in MapEntityStorage.GetPortals(mapId))
            {
                IFieldObject<Portal> fieldPortal = RequestFieldObject(new Portal(portal.Id)
                {
                    IsVisible = portal.Flags.HasFlag(MapPortalFlag.Visible),
                    IsEnabled = portal.Flags.HasFlag(MapPortalFlag.Enabled),
                    IsMinimapVisible = portal.Flags.HasFlag(MapPortalFlag.MinimapVisible),
                    Rotation = portal.Rotation.ToFloat(),
                    TargetMapId = portal.Target,
                });
                fieldPortal.Coord = portal.Coord.ToFloat();
                AddPortal(fieldPortal);
            }

            // Load default InteractActors
            List<IFieldObject<InteractActor>> actors = new List<IFieldObject<InteractActor>> { };
            foreach (MapInteractActor actor in MapEntityStorage.GetInteractActors(mapId))
            {
                // TODO: Group these fieldActors by their correct packet type. 
                actors.Add(RequestFieldObject(new InteractActor(actor.Uuid, actor.Name, actor.Type) { }));
            }
            AddInteractActor(actors);
        }

        // Gets a list of packets to update the state of all field objects for client.
        public IEnumerable<Packet> GetUpdates()
        {
            List<Packet> updates = new List<Packet>();
            // Update players locations
            // Update NPCs
            foreach (IFieldObject<Npc> npc in State.Npcs.Values)
            {
                updates.Add(FieldObjectPacket.ControlNpc(npc));
            }
            foreach (IFieldObject<Player> player in State.Players.Values)
            {
                updates.Add(FieldObjectPacket.UpdatePlayer(player));
            }
            foreach (IFieldObject<Mob> mob in State.Mobs.Values)
            {
                updates.Add(FieldObjectPacket.ControlMob(mob));
                if (mob.Value.IsDead)
                {
                    RemoveMob(mob);
                }
            }
            return updates;
        }

        public IFieldObject<T> RequestFieldObject<T>(T player)
        {
            return WrapObject(player);
        }

        public void AddPlayer(GameSession sender, IFieldObject<Player> player)
        {
            Debug.Assert(player.ObjectId > 0, "Player was added to field without initialized objectId.");
            player.Coord = player.Value.Coord;
            player.Value.MapId = MapId;
            // TODO: Determine new coordinates for player as well
            lock (Sessions)
            {
                Sessions.Add(sender);
            }

            // TODO: Send the initialization state of the field
            foreach (IFieldObject<Player> existingPlayer in State.Players.Values)
            {
                sender.Send(FieldPacket.AddPlayer(existingPlayer));
                sender.Send(FieldObjectPacket.LoadPlayer(existingPlayer));
            }
            foreach (IFieldObject<Item> existingItem in State.Items.Values)
            {
                sender.Send(FieldPacket.AddItem(existingItem, 123456));
            }
            foreach (IFieldObject<Npc> existingNpc in State.Npcs.Values)
            {
                sender.Send(FieldPacket.AddNpc(existingNpc));
                sender.Send(FieldObjectPacket.LoadNpc(existingNpc));
            }
            foreach (IFieldObject<Portal> existingPortal in State.Portals.Values)
            {
                sender.Send(FieldPacket.AddPortal(existingPortal));
            }
            foreach (IFieldObject<Mob> existingMob in State.Mobs.Values)
            {
                sender.Send(FieldPacket.AddMob(existingMob));
                sender.Send(FieldObjectPacket.LoadMob(existingMob));
            }
            if (State.InteractActors.Values.Count > 0)
            {
                sender.Send(InteractActorPacket.AddInteractActors(State.InteractActors.Values));
            }
            if (MapEntityStorage.HasHealingSpot(MapId))
            {
                if (HealingSpotThread == null || HealingSpotThread.IsCompleted)
                {
                    HealingSpotThread = StartHealingSpot(sender, player);
                }
                sender.Send(RegionSkillPacket.Send(player, MapEntityStorage.GetHealingSpot(MapId), new SkillCast(70000018, 1, 0, 1)));
            }
            State.AddPlayer(player);

            // Broadcast new player to all players in map
            Broadcast(session =>
            {
                session.Send(FieldPacket.AddPlayer(player));
                session.Send(FieldObjectPacket.LoadPlayer(player));
            });
        }

        public void RemovePlayer(GameSession sender, IFieldObject<Player> player)
        {
            lock (Sessions)
            {
                Sessions.Remove(sender);
            }
            State.RemovePlayer(player.ObjectId);

            // Remove player
            Broadcast(session =>
            {
                session.Send(FieldPacket.RemovePlayer(player));
            });

            ((FieldObject<Player>) player).ObjectId = -1; // Reset object id
        }

        // Spawned NPCs will not appear until controlled
        public void AddNpc(IFieldObject<Npc> fieldNpc)
        {
            State.AddNpc(fieldNpc);

            Broadcast(session =>
            {
                session.Send(FieldPacket.AddNpc(fieldNpc));
                session.Send(FieldObjectPacket.LoadNpc(fieldNpc));
            });
        }

        public void AddMob(IFieldObject<Mob> fieldMob)
        {
            State.AddMob(fieldMob);

            Broadcast(session =>
            {
                session.Send(FieldPacket.AddMob(fieldMob));
                session.Send(FieldObjectPacket.LoadMob(fieldMob));
            });
        }

        public void AddPortal(IFieldObject<Portal> portal)
        {
            State.AddPortal(portal);
            BroadcastPacket(FieldPacket.AddPortal(portal));
        }

        public void AddInteractActor(ICollection<IFieldObject<Types.InteractActor>> actors)
        {
            foreach (IFieldObject<InteractActor> actor in actors)
            {
                State.AddInteractActor(actor);
            }

            if (actors.Count > 0)
            {
                Broadcast(session =>
                {
                    session.Send(InteractActorPacket.AddInteractActors(actors));
                });
            }
        }

        public void SendChat(Player player, string message, ChatType type)
        {
            Broadcast(session =>
            {
                session.Send(ChatPacket.Send(player, message, type));
            });
        }

        public void AddItem(GameSession sender, Item item)
        {
            FieldObject<Item> fieldItem = WrapObject(item);
            fieldItem.Coord = sender.FieldPlayer.Coord;

            State.AddItem(fieldItem);

            Broadcast(session =>
            {
                session.Send(FieldPacket.AddItem(fieldItem, session.FieldPlayer.ObjectId));
            });
        }

        public bool RemoveItem(int objectId, out Item item)
        {
            if (!State.RemoveItem(objectId, out item))
            {
                return false;
            }

            Broadcast(session =>
            {
                session.Send(FieldPacket.PickupItem(objectId, session.FieldPlayer.ObjectId));
                session.Send(FieldPacket.RemoveItem(objectId));
            });
            return true;
        }

        public bool RemoveMob(IFieldObject<Mob> mob)
        {
            // TODO: Spawn mob based on timer
            if (!State.RemoveMob(mob.ObjectId))
            {
                return false;
            }

            Broadcast(session =>
            {
                session.Send(FieldPacket.RemoveMob(mob));
            });
            return true;
        }

        //Broadcast a packet after the specified delay.
        public async Task DelayBroadcastPacket(Packet packet, int delay)
        {
            await Task.Factory.StartNew(async () =>
            {
                await Task.Delay(delay);
                BroadcastPacket(packet);
            });
        }

        // Providing a session will result in packet not being broadcast to self.
        public void BroadcastPacket(Packet packet, GameSession sender = null)
        {
            Broadcast(session =>
            {
                if (session == sender)
                {
                    return;
                }
                session.Send(packet);
            });
        }

        // Broadcasts packets to all sessions.
        // TODO: This should be optimized to avoid regenerating packets for each session.
        private void Broadcast(Action<GameSession> action)
        {
            lock (Sessions)
            {
                foreach (GameSession session in Sessions)
                {
                    action?.Invoke(session);
                }
            }
        }

        // Initializes a FieldObject with an objectId for this field.
        private FieldObject<T> WrapObject<T>(T fieldObject)
        {
            int objectId = Interlocked.Increment(ref Counter);
            return new FieldObject<T>(objectId, fieldObject);
        }

        // This class is private to ensure that callers must first request entry.
        private class FieldObject<T> : IFieldObject<T>
        {
            public int ObjectId { get; set; }
            public T Value { get; }

            public CoordF Coord { get; set; }

            public FieldObject(int objectId, T value)
            {
                ObjectId = objectId;
                Value = value;
            }
        }

        private Task StartHealingSpot(GameSession session, IFieldObject<Player> player)
        {
            int healAmount = 30;
            Status status = new Status(new SkillCast(70000018, 1, 0, 1), player.ObjectId, player.ObjectId, 1, healAmount);

            return Task.Run(async () =>
            {
                while (!State.Players.IsEmpty)
                {
                    CoordS healingCoord = MapEntityStorage.GetHealingSpot(MapId);

                    if ((healingCoord - player.Coord.ToShort()).Length() < Block.BLOCK_SIZE * 2 && healingCoord.Z == player.Coord.ToShort().Z - 1) // 3x3x1 area
                    {
                        session.Send(BuffPacket.SendBuff(0, status));
                        session.Send(SkillDamagePacket.ApplyHeal(player, status));
                        session.Player.Stats.Increase(PlayerStatId.Hp, healAmount);
                        session.Send(StatPacket.UpdateStats(player, PlayerStatId.Hp));
                    }

                    await Task.Delay(1000);
                }
            });
        }
    }
}
