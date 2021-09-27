using System.Diagnostics;
using Maple2.Trigger;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Triggers;
using MapleServer2.Types;
using NLog;

namespace MapleServer2.Managers
{
    // TODO: This needs to be thread safe
    // TODO: FieldManager probably needs its own thread to send updates about user position
    // This seems to be done every ~2s rather than on every update.
    public class FieldManager
    {
        private static readonly TriggerLoader TriggerLoader = new TriggerLoader();

        private int Counter = 10000000;

        public readonly int MapId;
        public readonly long InstanceId;
        public readonly CoordS[] BoundingBox;
        public readonly FieldState State = new FieldState();
        private readonly HashSet<GameSession> Sessions = new HashSet<GameSession>();
        public readonly TriggerScript[] Triggers;
        private readonly List<BreakableObject> Breakables = new List<BreakableObject>();
        private readonly List<MapTimer> MapTimers = new List<MapTimer>();
        private readonly List<Widget> Widgets = new List<Widget>();
        public bool SkipScene;
        private Task MapLoopTask;
        private readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private int PlayerCount;

        public FieldManager(int mapId, long instanceId)
        {
            MapId = mapId;
            InstanceId = instanceId;
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
                    IsVisible = portal.IsVisible,
                    IsEnabled = portal.Enable,
                    IsMinimapVisible = portal.MinimapVisible,
                    Rotation = portal.Rotation.ToFloat(),
                    TargetMapId = portal.Target,
                    PortalType = portal.PortalType
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

            MapMetadata mapMetadata = MapMetadataStorage.GetMetadata(mapId);
            if (mapMetadata != null)
            {
                string xBlockName = mapMetadata.XBlockName;
                Triggers = TriggerLoader.GetTriggers(xBlockName).Select(initializer =>
                {
                    TriggerContext context = new TriggerContext(this, Logger);
                    TriggerState startState = initializer.Invoke(context);
                    return new TriggerScript(context, startState);
                }).ToArray();
            }

            foreach (MapTriggerMesh mapTriggerMesh in MapEntityStorage.GetTriggerMeshes(mapId))
            {
                if (mapTriggerMesh != null)
                {
                    TriggerMesh triggerMesh = new TriggerMesh(mapTriggerMesh.Id, mapTriggerMesh.IsVisible);
                    State.AddTriggerObject(triggerMesh);
                }
            }

            foreach (MapTriggerEffect mapTriggerEffect in MapEntityStorage.GetTriggerEffects(mapId))
            {
                if (mapTriggerEffect != null)
                {
                    TriggerEffect triggerEffect = new TriggerEffect(mapTriggerEffect.Id, mapTriggerEffect.IsVisible);
                    State.AddTriggerObject(triggerEffect);
                }
            }

            foreach (MapTriggerActor mapTriggerActor in MapEntityStorage.GetTriggerActors(mapId))
            {
                if (mapTriggerActor != null)
                {
                    TriggerActor triggerActor = new TriggerActor(mapTriggerActor.Id, mapTriggerActor.IsVisible, mapTriggerActor.InitialSequence);
                    State.AddTriggerObject(triggerActor);
                }
            }

            foreach (MapTriggerCamera mapTriggerCamera in MapEntityStorage.GetTriggerCameras(mapId))
            {
                if (mapTriggerCamera != null)
                {
                    TriggerCamera triggerCamera = new TriggerCamera(mapTriggerCamera.Id, mapTriggerCamera.IsEnabled);
                    State.AddTriggerObject(triggerCamera);
                }
            }

            foreach (MapTriggerCube mapTriggerCube in MapEntityStorage.GetTriggerCubes(mapId))
            {
                if (mapTriggerCube != null)
                {
                    TriggerCube triggerCube = new TriggerCube(mapTriggerCube.Id, mapTriggerCube.IsVisible);
                    State.AddTriggerObject(triggerCube);
                }
            }

            foreach (MapTriggerLadder mapTriggerLadder in MapEntityStorage.GetTriggerLadders(mapId))
            {
                if (mapTriggerLadder != null)
                {
                    TriggerLadder triggerLadder = new TriggerLadder(mapTriggerLadder.Id, mapTriggerLadder.IsVisible);
                    State.AddTriggerObject(triggerLadder);
                }
            }

            foreach (MapTriggerRope mapTriggerRope in MapEntityStorage.GetTriggerRopes(mapId))
            {
                if (mapTriggerRope != null)
                {
                    TriggerRope triggerRope = new TriggerRope(mapTriggerRope.Id, mapTriggerRope.IsVisible);
                    State.AddTriggerObject(triggerRope);
                }
            }

            foreach (MapTriggerSound mapTriggerSound in MapEntityStorage.GetTriggerSounds(mapId))
            {
                if (mapTriggerSound != null)
                {
                    TriggerSound triggerSound = new TriggerSound(mapTriggerSound.Id, mapTriggerSound.IsEnabled);
                    State.AddTriggerObject(triggerSound);
                }
            }

            foreach (MapTriggerSkill mapTriggerSkill in MapEntityStorage.GetTriggerSkills(mapId))
            {
                if (mapTriggerSkill != null)
                {
                    TriggerSkill triggerSkill = new TriggerSkill(mapTriggerSkill.Id, mapTriggerSkill.SkillId, mapTriggerSkill.SkillLevel, mapTriggerSkill.Count, mapTriggerSkill.Position);
                    IFieldObject<TriggerSkill> fieldTriggerSkill = RequestFieldObject(triggerSkill);
                    fieldTriggerSkill.Coord = fieldTriggerSkill.Value.Position;

                    State.AddTriggerSkills(fieldTriggerSkill);
                }
            }

            // Load breakables
            foreach (MapBreakableActorObject mapActor in MapEntityStorage.GetBreakableActors(mapId))
            {
                if (mapActor != null)
                {
                    BreakableActorObject actor = new BreakableActorObject(mapActor.EntityId, mapActor.IsEnabled, mapActor.HideDuration, mapActor.ResetDuration);
                    State.AddBreakable(actor);
                }
            }

            foreach (MapBreakableNifObject mapNif in MapEntityStorage.GetBreakableNifs(mapId))
            {
                if (mapNif != null)
                {
                    BreakableNifObject nif = new BreakableNifObject(mapNif.EntityId, mapNif.IsEnabled, mapNif.TriggerId, mapNif.HideDuration, mapNif.ResetDuration);
                    State.AddBreakable(nif);
                }
            }

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
                //trigger.Next();
            }
            return updates;
        }

        private void SendUpdates()
        {
            foreach (Packet update in GetUpdates())
            {
                Broadcast(session =>
                {
                    session.Send(update);
                });
            }
        }

        public IFieldObject<T> RequestFieldObject<T>(T wrappingObject) => WrapObject(wrappingObject);

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

            if (State.Cubes.IsEmpty && !player.Value.IsInDecorPlanner)
            {
                if (MapId == (int) Map.PrivateResidence)
                {
                    Home home = GameServer.HomeManager.GetHome(player.Value.VisitingHomeId);
                    if (home != null)
                    {
                        Dictionary<long, Cube> cubes = home.FurnishingInventory;
                        foreach (Cube cube in cubes.Values.Where(x => x.PlotNumber == 1))
                        {
                            IFieldObject<Cube> ugcCube = RequestFieldObject(cube);
                            ugcCube.Coord = cube.CoordF;
                            ugcCube.Rotation = cube.Rotation;
                            State.AddCube(ugcCube);
                        }
                    }
                }
                else
                {
                    List<Home> homes = GameServer.HomeManager.GetPlots(MapId);
                    foreach (Home home in homes)
                    {
                        Dictionary<long, Cube> cubes = home.FurnishingInventory;
                        foreach (Cube cube in cubes.Values.Where(x => x.PlotNumber != 1))
                        {
                            IFieldObject<Cube> ugcCube = RequestFieldObject(cube);
                            ugcCube.Coord = cube.CoordF;
                            ugcCube.Rotation = cube.Rotation;
                            State.AddCube(ugcCube);
                        }
                    }
                }
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

            List<BreakableObject> breakables = new List<BreakableObject>();
            breakables.AddRange(State.BreakableActors.Values.ToList());
            breakables.AddRange(State.BreakableNifs.Values.ToList());
            sender.Send(BreakablePacket.LoadBreakables(breakables));

            List<TriggerObject> triggerObjects = new List<TriggerObject>();
            triggerObjects.AddRange(State.TriggerMeshes.Values.ToList());
            triggerObjects.AddRange(State.TriggerEffects.Values.ToList());
            triggerObjects.AddRange(State.TriggerCameras.Values.ToList());
            triggerObjects.AddRange(State.TriggerActors.Values.ToList());
            triggerObjects.AddRange(State.TriggerCubes.Values.ToList());
            triggerObjects.AddRange(State.TriggerLadders.Values.ToList());
            triggerObjects.AddRange(State.TriggerRopes.Values.ToList());
            triggerObjects.AddRange(State.TriggerSounds.Values.ToList());
            sender.Send(TriggerPacket.LoadTriggers(triggerObjects));
            State.AddPlayer(player);

            if (MapLoopTask == null)
            {
                MapLoopTask = StartMapLoop(); //TODO: find a better place to initialise MapLoopTask
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
            player.Value.Triggers.Clear();

            // Remove player
            Broadcast(session =>
            {
                session.Send(FieldPacket.RemovePlayer(player));
                session.Send(FieldObjectPacket.RemovePlayer(player));
            });

            ((FieldObject<Player>) player).ObjectId = -1; // Reset object id
        }

        public static bool IsPlayerInBox(MapTriggerBox box, IFieldObject<Player> player)
        {
            CoordF minCoord = CoordF.From(
                    box.Position.X - box.Dimension.X / 2,
                    box.Position.Y - box.Dimension.Y / 2,
                    box.Position.Z - box.Dimension.Z / 2);
            CoordF maxCoord = CoordF.From(
                box.Position.X + box.Dimension.X / 2,
                box.Position.Y + box.Dimension.Y / 2,
                box.Position.Z + box.Dimension.Z / 2);
            bool min = player.Coord.X >= minCoord.X && player.Coord.Y >= minCoord.Y && player.Coord.Z >= minCoord.Z;
            bool max = player.Coord.X <= maxCoord.X && player.Coord.Y <= maxCoord.Y && player.Coord.Z <= maxCoord.Z;
            return min && max;
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

        public void AddGuide(IFieldObject<GuideObject> fieldGuide) => State.AddGuide(fieldGuide);

        public bool RemoveGuide(IFieldObject<GuideObject> fieldGuide) => State.RemoveGuide(fieldGuide.ObjectId);

        public void AddCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
        {
            State.AddCube(cube);
            BroadcastPacket(ResponseCubePacket.PlaceFurnishing(cube, houseOwnerObjectId, fieldPlayerObjectId, sendOnlyObjectId: false));
        }

        public void RemoveCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
        {
            State.RemoveCube(cube.ObjectId);
            BroadcastPacket(ResponseCubePacket.RemoveCube(houseOwnerObjectId, fieldPlayerObjectId, cube.Coord.ToByte()));
        }

        public void AddInstrument(IFieldObject<Instrument> instrument) => State.AddInstrument(instrument);

        public bool RemoveInstrument(IFieldObject<Instrument> instrument) => State.RemoveInstrument(instrument.ObjectId);

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
            if (!State.RemoveItem(objectId, out Item itemResult))
            {
                item = itemResult;
                return false;
            }
            item = itemResult;

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

        private Task StartMapLoop()
        {
            return Task.Run(async () =>
            {
                while (!State.Players.IsEmpty)
                {
                    HealingSpot();
                    UpdateMobs();
                    SendUpdates();
                    await Task.Delay(1000);
                }
            });
        }

        private void UpdateMobs()
        {
            foreach (IFieldObject<Mob> mob in State.Mobs.Values)
            {
                mob.Coord += mob.Value.Velocity;    // Set current position (given to ControlMob Packet)
                mob.Value.Act();
            }
        }

        private void HealingSpot()
        {
            foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
            {
                CoordS healingCoord = healingSpot.Value.Coord;
                foreach (IFieldObject<Player> player in State.Players.Values)
                {
                    if ((healingCoord - player.Coord.ToShort()).Length() < Block.BLOCK_SIZE * 2 && healingCoord.Z == player.Coord.ToShort().Z - 1) // 3x3x1 area
                    {
                        int healAmount = (int) (player.Value.Stats[PlayerStatId.Hp].Max * 0.03);
                        Status status = new Status(new SkillCast(70000018, 1, 0, 1), target: player.ObjectId, source: healingSpot.ObjectId, stacks: 1);

                        player.Value.Session.Send(BuffPacket.SendBuff(0, status));
                        BroadcastPacket(SkillDamagePacket.Heal(status, healAmount));

                        player.Value.Session.Player.Stats.Increase(PlayerStatId.Hp, healAmount);
                        player.Value.Session.Send(StatPacket.UpdateStats(player, PlayerStatId.Hp));
                    }
                }
            }
        }

        private void SpawnMobs(IFieldObject<MobSpawn> mobSpawn)
        {
            List<CoordF> spawnPoints = MobSpawn.SelectPoints(mobSpawn.Value.SpawnRadius);

            foreach (NpcMetadata mob in mobSpawn.Value.SpawnMobs)
            {
                if (mob.Name == "Constructor Type 13")
                {
                    continue;
                }
                int groupSpawnCount = mob.NpcMetadataBasic.GroupSpawnCount;  // Spawn count changes due to field effect (?)
                if (mobSpawn.Value.Mobs.Count + groupSpawnCount > mobSpawn.Value.MaxPopulation)
                {
                    break;
                }

                for (int i = 0; i < groupSpawnCount; i++)
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

        public int Increment() => Interlocked.Increment(ref PlayerCount);

        public int Decrement() => Interlocked.Decrement(ref PlayerCount);

        public void AddMapTimer(MapTimer timer)
        {
            MapTimer existingTimer = MapTimers.FirstOrDefault(x => x.Id == timer.Id);
            if (existingTimer != null)
            {
                existingTimer = timer;
                return;
            }

            MapTimers.Add(timer);
        }

        public MapTimer GetMapTimer(string id) => MapTimers.FirstOrDefault(x => x.Id == id);

        public void AddWidget(Widget widget) => Widgets.Add(widget);

        public Widget GetWidget(WidgetType type) => Widgets.FirstOrDefault(x => x.Type == type);

        public void EnableSceneSkip(bool enable) => SkipScene = enable;
    }
}
