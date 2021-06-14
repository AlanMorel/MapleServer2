using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple2.Trigger;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Triggers;
using MapleServer2.Types;
using NLog;

namespace MapleServer2.Servers.Game
{
    // TODO: This needs to be thread safe
    // TODO: FieldManager probably needs its own thread to send updates about user position
    // This seems to be done every ~2s rather than on every update.
    public class FieldManager
    {
        private static readonly TriggerLoader TriggerLoader = new TriggerLoader();

        private int Counter = 10000000;

        public readonly int MapId;
        public readonly CoordS[] BoundingBox;
        public readonly FieldState State = new FieldState();
        private readonly HashSet<GameSession> Sessions = new HashSet<GameSession>();
        private readonly TriggerScript[] Triggers;

        private Task HealingSpotThread;
        private readonly ILogger Logger = LogManager.GetCurrentClassLogger();

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
                IFieldObject<MobSpawn> fieldMobSpawn = RequestFieldObject(new MobSpawn(mobSpawn));
                fieldMobSpawn.Coord = mobSpawn.Coord.ToFloat();
                State.AddMobSpawn(fieldMobSpawn);
                SpawnMobs(fieldMobSpawn);
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

            // Load default InteractObjects
            List<IFieldObject<InteractObject>> actors = new List<IFieldObject<InteractObject>> { };
            foreach (MapInteractObject interactObject in MapEntityStorage.GetInteractObject(mapId))
            {
                // TODO: Group these fieldActors by their correct packet type. 
                actors.Add(RequestFieldObject(new InteractObject(interactObject.Uuid, interactObject.Name, interactObject.Type) { }));
            }
            AddInteractObject(actors);

            string mapName = MapMetadataStorage.GetMetadata(mapId).Name;
            Triggers = TriggerLoader.GetTriggers(mapName).Select(initializer =>
            {
                TriggerContext context = new TriggerContext(this, Logger);
                TriggerState startState = initializer.Invoke(context);
                return new TriggerScript(context, startState);
            }).ToArray();

            if (MapEntityStorage.HasHealingSpot(MapId))
            {
                List<CoordS> healingSpots = MapEntityStorage.GetHealingSpot(MapId);
                if (State.HealingSpots.IsEmpty)
                {
                    foreach (CoordS coord in healingSpots)
                    {
                        int objectId = GuidGenerator.Int();
                        State.AddHealingSpot(RequestFieldObject(new HealingSpot(objectId, coord)));
                    }
                }
            }
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
            foreach (TriggerScript trigger in Triggers)
            {
                trigger.Next();
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
            if (State.InteractObjects.Values.Count > 0)
            {
                ICollection<IFieldObject<InteractObject>> balloons = State.InteractObjects.Values.Where(x => x.Value.Type == InteractObjectType.AdBalloon).ToList();
                if (balloons.Count > 0)
                {
                    foreach (IFieldObject<InteractObject> balloon in balloons)
                    {
                        sender.Send(InteractObjectPacket.AddAdBallons(balloon));
                    }
                }
                ICollection<IFieldObject<InteractObject>> objects = State.InteractObjects.Values.Where(x => x.Value.Type != InteractObjectType.AdBalloon).ToList();
                if (objects.Count > 0)
                {
                    sender.Send(InteractObjectPacket.AddInteractObjects(objects));
                }
            }
            if (State.Cubes.Values.Count > 0)
            {
                sender.Send(CubePacket.LoadCubes(State.Cubes.Values));
            }
            foreach (IFieldObject<GuideObject> guide in State.Guide.Values)
            {
                sender.Send(GuideObjectPacket.Add(guide));
            }

            foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
            {
                sender.Send(RegionSkillPacket.Send(healingSpot.ObjectId, healingSpot.Value.Coord, new SkillCast(70000018, 1, 0, 1)));
            }

            foreach (IFieldObject<Instrument> instrument in State.Instruments.Values)
            {
                if (instrument.Value.Improvise)
                {
                    sender.Send(InstrumentPacket.StartImprovise(instrument));
                }
                else
                {
                    sender.Send(InstrumentPacket.PlayScore(instrument));
                }
            }

            State.AddPlayer(player);

            if (!State.HealingSpots.IsEmpty)
            {
                if (HealingSpotThread == null)
                {
                    HealingSpotThread = StartHealingSpot();
                }
            }

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
                session.Send(FieldObjectPacket.RemovePlayer(player));
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

        public bool RemoveNpc(IFieldObject<Npc> fieldNpc)
        {
            if (!State.RemoveNpc(fieldNpc.ObjectId))
            {
                return false;
            }

            Broadcast(session =>
            {
                // TODO: Add field remove NPC packet
                session.Send(FieldObjectPacket.RemoveNpc(fieldNpc));
            });
            return true;
        }

        public void AddMob(IFieldObject<Mob> fieldMob)
        {
            State.AddMob(fieldMob);

            fieldMob.Value.OriginSpawn?.Value.Mobs.Add(fieldMob);

            Broadcast(session =>
            {
                session.Send(FieldPacket.AddMob(fieldMob));
                session.Send(FieldObjectPacket.LoadMob(fieldMob));
                // TODO: Add spawn buff (ID: 0x055D4DAE)
                //session.Send();
            });
        }

        public bool RemoveMob(IFieldObject<Mob> mob)
        {
            if (!State.RemoveMob(mob.ObjectId))
            {
                return false;
            }

            IFieldObject<MobSpawn> originSpawn = mob.Value.OriginSpawn;
            if (originSpawn != null && originSpawn.Value.Mobs.Remove(mob) && originSpawn.Value.Mobs.Count == 0)
            {
                StartSpawnTimer(originSpawn);
            }

            Broadcast(session =>
            {
                session.Send(FieldPacket.RemoveMob(mob));
                session.Send(FieldObjectPacket.RemoveMob(mob));
            });
            return true;
        }

        public void AddGuide(IFieldObject<GuideObject> fieldGuide)
        {
            State.AddGuide(fieldGuide);
        }

        public bool RemoveGuide(IFieldObject<GuideObject> fieldGuide)
        {
            return State.RemoveGuide(fieldGuide.ObjectId);
        }

        public void AddCube(IFieldObject<Cube> cube, IFieldObject<Player> player)
        {
            State.AddCube(cube);
            BroadcastPacket(ResponseCubePacket.PlaceFurnishing(cube, player));
        }

        public bool RemoveCube(IFieldObject<Cube> cube)
        {
            return State.RemoveCube(cube.ObjectId);
        }

        public void AddInstrument(IFieldObject<Instrument> instrument)
        {
            State.AddInstrument(instrument);
        }

        public bool RemoveInstrument(IFieldObject<Instrument> instrument)
        {
            return State.RemoveInstrument(instrument.ObjectId);
        }

        public void AddPortal(IFieldObject<Portal> portal)
        {
            State.AddPortal(portal);
            BroadcastPacket(FieldPacket.AddPortal(portal));
        }

        public void AddInteractObject(ICollection<IFieldObject<InteractObject>> objects)
        {
            foreach (IFieldObject<InteractObject> interactObject in objects)
            {
                State.AddInteractObject(interactObject);
            }

            if (objects.Count > 0)
            {
                Broadcast(session =>
                {
                    session.Send(InteractObjectPacket.AddInteractObjects(objects));
                });
            }
        }

        public void AddBalloon(IFieldObject<InteractObject> balloon)
        {
            State.AddBalloon(balloon);

            Broadcast(session =>
            {
                session.Send(InteractObjectPacket.AddAdBallons(balloon));
            });
        }

        public bool RemoveBalloon(IFieldObject<InteractObject> balloon)
        {
            return State.RemoveBalloon(balloon.Value.Name);
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
            Item itemResult;
            if (!State.RemoveItem(objectId, out itemResult))
            {
                item = itemResult;
                return false;
            }
            item = itemResult;

            Broadcast(session =>
            {
                session.Send(FieldPacket.PickupItem(objectId, itemResult, session.FieldPlayer.ObjectId));
                session.Send(FieldPacket.RemoveItem(objectId));
            });
            return true;
        }

        public void AddResource(Item item, IFieldObject<Mob> source, IFieldObject<Player> targetPlayer)
        {
            FieldObject<Item> fieldItem = WrapObject(item);
            fieldItem.Coord = source.Coord;

            State.AddItem(fieldItem);

            Broadcast(session =>
            {
                session.Send(FieldPacket.AddItem(fieldItem, source, targetPlayer));
            });
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

            public CoordF Rotation { get; set; }

            public FieldObject(int objectId, T value)
            {
                ObjectId = objectId;
                Value = value;
            }
        }

        private Task StartHealingSpot()
        {
            return Task.Run(async () =>
            {
                while (!State.Players.IsEmpty)
                {
                    foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
                    {
                        CoordS healingCoord = healingSpot.Value.Coord;
                        foreach (IFieldObject<Player> player in State.Players.Values)
                        {
                            if ((healingCoord - player.Coord.ToShort()).Length() < Block.BLOCK_SIZE * 2 && healingCoord.Z == player.Coord.ToShort().Z - 1) // 3x3x1 area
                            {
                                int healAmount = (int) (player.Value.Stats[PlayerStatId.Hp].Max * 0.03);
                                Status status = new Status(new SkillCast(70000018, 1, 0, 1), owner: player.ObjectId, source: healingSpot.ObjectId, duration: 100, stacks: 1);

                                player.Value.Session.Send(BuffPacket.SendBuff(0, status));
                                BroadcastPacket(SkillDamagePacket.ApplyHeal(status, healAmount));

                                player.Value.Session.Player.Stats.Increase(PlayerStatId.Hp, healAmount);
                                player.Value.Session.Send(StatPacket.UpdateStats(player, PlayerStatId.Hp));
                            }
                        }
                    }

                    await Task.Delay(1000);
                }
            });
        }

        private void SpawnMobs(IFieldObject<MobSpawn> mobSpawn)
        {
            List<CoordF> spawnPoints = MobSpawn.SelectPoints(mobSpawn.Value.SpawnRadius);

            foreach (NpcMetadata mob in mobSpawn.Value.SpawnMobs)
            {
                int spawnCount = mob.NpcMetadataBasic.GroupSpawnCount;  // Spawn count changes due to field effect (?)
                if (mobSpawn.Value.Mobs.Count + spawnCount > mobSpawn.Value.MaxPopulation)
                {
                    break;
                }

                for (int i = 0; i < spawnCount; i++)
                {
                    IFieldObject<Mob> fieldMob = RequestFieldObject(new Mob(mob.Id, mobSpawn));
                    fieldMob.Coord = mobSpawn.Coord + spawnPoints[mobSpawn.Value.Mobs.Count % spawnPoints.Count];
                    AddMob(fieldMob);
                }
            }
        }

        private Task StartSpawnTimer(IFieldObject<MobSpawn> mobSpawn)
        {
            int spawnTimer = mobSpawn.Value.SpawnData.SpawnTime * 1000;
            return Task.Run(async () =>
            {
                await Task.Delay(spawnTimer);

                if (mobSpawn.Value.Mobs.Count == 0)
                {
                    SpawnMobs(mobSpawn);
                }
            });
        }
    }
}
