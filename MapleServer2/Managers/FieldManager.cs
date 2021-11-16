using System.Diagnostics;
using Maple2.Trigger;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using Maple2Storage.Tools;
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

namespace MapleServer2.Managers;

// TODO: This needs to be thread safe
// TODO: FieldManager probably needs its own thread to send updates about user position
// This seems to be done every ~2s rather than on every update.
public class FieldManager
{
    private static readonly TriggerLoader TriggerLoader = new();

    private int Counter = 10000000;

    public readonly int MapId;
    public readonly long InstanceId;
    public readonly CoordS[] BoundingBox;
    public readonly FieldState State = new();
    //public readonly FieldNavigator Navigator;   // TODO: temp remove for PR
    private readonly HashSet<GameSession> Sessions = new();
    public readonly TriggerScript[] Triggers;
    private readonly List<MapTimer> MapTimers = new();
    private readonly List<Widget> Widgets = new();
    public bool SkipScene;
    private Task MapLoopTask;
    private readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private int PlayerCount;

    public FieldManager(Player player)
    {
        MapId = player.MapId;
        InstanceId = player.InstanceId;
        BoundingBox = MapEntityStorage.GetBoundingBox(MapId);

        // TOOD: generate navmeshes for all maps
        //if (MapId == 2000019)
        //{
        //    Navigator = new FieldNavigator();
        //    Navigator.LoadNavMeshFromFile("tok_mesh.snj");
        //}

        // Load default npcs for map from config
        foreach (MapNpc npc in MapEntityStorage.GetNpcs(MapId))
        {
            RequestNpc(npc.Id, npc.Coord.ToFloat(), npc.Rotation.ToFloat());
        }

        // Spawn map's mobs at the mob spawners
        foreach (MapMobSpawn mobSpawn in MapEntityStorage.GetMobSpawns(MapId))
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
        foreach (MapPortal portal in MapEntityStorage.GetPortals(MapId))
        {
            IFieldObject<Portal> fieldPortal = RequestFieldObject(new Portal(portal.Id)
            {
                IsVisible = portal.IsVisible,
                IsEnabled = portal.Enable,
                IsMinimapVisible = portal.MinimapVisible,
                Rotation = portal.Rotation.ToFloat(),
                TargetMapId = portal.Target,
                TargetPortalId = portal.TargetPortalId,
                PortalType = portal.PortalType
            });
            fieldPortal.Coord = portal.Coord.ToFloat();
            AddPortal(fieldPortal);
        }

        MapMetadata mapMetadata = MapMetadataStorage.GetMetadata(MapId);
        if (mapMetadata != null)
        {
            string xBlockName = mapMetadata.XBlockName;
            Triggers = TriggerLoader.GetTriggers(xBlockName).Select(initializer =>
            {
                TriggerContext context = new(this, Logger);
                TriggerState startState = initializer.Invoke(context);
                return new TriggerScript(context, startState);
            }).ToArray();
        }

        foreach (MapTriggerMesh mapTriggerMesh in MapEntityStorage.GetTriggerMeshes(MapId))
        {
            if (mapTriggerMesh != null)
            {
                TriggerMesh triggerMesh = new(mapTriggerMesh.Id, mapTriggerMesh.IsVisible);
                State.AddTriggerObject(triggerMesh);
            }
        }

        foreach (MapTriggerEffect mapTriggerEffect in MapEntityStorage.GetTriggerEffects(MapId))
        {
            if (mapTriggerEffect != null)
            {
                TriggerEffect triggerEffect = new(mapTriggerEffect.Id, mapTriggerEffect.IsVisible);
                State.AddTriggerObject(triggerEffect);
            }
        }

        foreach (MapTriggerActor mapTriggerActor in MapEntityStorage.GetTriggerActors(MapId))
        {
            if (mapTriggerActor != null)
            {
                TriggerActor triggerActor = new(mapTriggerActor.Id, mapTriggerActor.IsVisible, mapTriggerActor.InitialSequence);
                State.AddTriggerObject(triggerActor);
            }
        }

        foreach (MapTriggerCamera mapTriggerCamera in MapEntityStorage.GetTriggerCameras(MapId))
        {
            if (mapTriggerCamera != null)
            {
                TriggerCamera triggerCamera = new(mapTriggerCamera.Id, mapTriggerCamera.IsEnabled);
                State.AddTriggerObject(triggerCamera);
            }
        }

        foreach (MapTriggerCube mapTriggerCube in MapEntityStorage.GetTriggerCubes(MapId))
        {
            if (mapTriggerCube != null)
            {
                TriggerCube triggerCube = new(mapTriggerCube.Id, mapTriggerCube.IsVisible);
                State.AddTriggerObject(triggerCube);
            }
        }

        foreach (MapTriggerLadder mapTriggerLadder in MapEntityStorage.GetTriggerLadders(MapId))
        {
            if (mapTriggerLadder != null)
            {
                TriggerLadder triggerLadder = new(mapTriggerLadder.Id, mapTriggerLadder.IsVisible);
                State.AddTriggerObject(triggerLadder);
            }
        }

        foreach (MapTriggerRope mapTriggerRope in MapEntityStorage.GetTriggerRopes(MapId))
        {
            if (mapTriggerRope != null)
            {
                TriggerRope triggerRope = new(mapTriggerRope.Id, mapTriggerRope.IsVisible);
                State.AddTriggerObject(triggerRope);
            }
        }

        foreach (MapTriggerSound mapTriggerSound in MapEntityStorage.GetTriggerSounds(MapId))
        {
            if (mapTriggerSound != null)
            {
                TriggerSound triggerSound = new(mapTriggerSound.Id, mapTriggerSound.IsEnabled);
                State.AddTriggerObject(triggerSound);
            }
        }

        foreach (MapTriggerSkill mapTriggerSkill in MapEntityStorage.GetTriggerSkills(MapId))
        {
            if (mapTriggerSkill != null)
            {
                TriggerSkill triggerSkill = new(mapTriggerSkill.Id, mapTriggerSkill.SkillId, mapTriggerSkill.SkillLevel, mapTriggerSkill.Count, mapTriggerSkill.Position);
                IFieldObject<TriggerSkill> fieldTriggerSkill = RequestFieldObject(triggerSkill);
                fieldTriggerSkill.Coord = fieldTriggerSkill.Value.Position;

                State.AddTriggerSkills(fieldTriggerSkill);
            }
        }

        // Load breakables
        foreach (MapBreakableActorObject mapActor in MapEntityStorage.GetBreakableActors(MapId))
        {
            if (mapActor != null)
            {
                BreakableActorObject actor = new(mapActor.EntityId, mapActor.IsEnabled, mapActor.HideDuration, mapActor.ResetDuration);
                State.AddBreakable(actor);
            }
        }

        foreach (MapBreakableNifObject mapNif in MapEntityStorage.GetBreakableNifs(MapId))
        {
            if (mapNif != null)
            {
                BreakableNifObject nif = new(mapNif.EntityId, mapNif.IsEnabled, mapNif.TriggerId, mapNif.HideDuration, mapNif.ResetDuration);
                State.AddBreakable(nif);
            }
        }

        // Load interact objects
        foreach (MapInteractObject mapInteract in MapEntityStorage.GetInteractObjects(MapId))
        {
            if (mapInteract != null)
            {
                InteractObject interactObject = new(mapInteract.EntityId, mapInteract.InteractId, mapInteract.Type, InteractObjectState.Default);
                State.AddInteractObject(interactObject);
            }
        }

        // Load cubes
        if (MapId == (int) Map.PrivateResidence)
        {
            Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
            if (home != null)
            {
                // Add cubes to state
                Dictionary<long, Cube> cubes = home.FurnishingInventory;
                foreach (Cube cube in cubes.Values.Where(x => x.PlotNumber == 1))
                {
                    IFieldObject<Cube> ugcCube = RequestFieldObject(cube);
                    ugcCube.Coord = cube.CoordF;
                    ugcCube.Rotation = cube.Rotation;
                    State.AddCube(ugcCube);
                }

                // Add portals to state
                IEnumerable<Cube> cubePortals = cubes.Values.Where(x => x.Item.Id == 50400158);
                foreach (Cube cubePortal in cubePortals)
                {
                    Portal portal = new(GuidGenerator.Int())
                    {
                        IsVisible = true,
                        IsEnabled = true,
                        IsMinimapVisible = false,
                        Rotation = cubePortal.Rotation,
                        PortalType = PortalTypes.Home
                    };

                    IFieldObject<Portal> fieldPortal = RequestFieldObject(portal);
                    fieldPortal.Coord = cubePortal.CoordF;
                    fieldPortal.Value.UGCPortalMethod = cubePortal.PortalSettings.Method;
                    if (!string.IsNullOrEmpty(cubePortal.PortalSettings.DestinationTarget))
                    {
                        switch (cubePortal.PortalSettings.Destination)
                        {
                            case UGCPortalDestination.PortalInHome:
                                fieldPortal.Value.TargetMapId = (int) Map.PrivateResidence;
                                break;
                            case UGCPortalDestination.SelectedMap:
                                fieldPortal.Value.TargetMapId = int.Parse(cubePortal.PortalSettings.DestinationTarget);
                                break;
                            case UGCPortalDestination.FriendHome:
                                fieldPortal.Value.TargetHomeAccountId = long.Parse(cubePortal.PortalSettings.DestinationTarget);
                                break;
                        }
                    }
                    cubePortal.PortalSettings.PortalObjectId = fieldPortal.ObjectId;
                    AddPortal(fieldPortal);
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
    public IEnumerable<PacketWriter> GetUpdates()
    {
        List<PacketWriter> updates = new();
        // Update players locations
        // Update NPCs
        foreach (Npc npc in State.Npcs.Values)
        {
            updates.Add(FieldObjectPacket.ControlNpc(npc));
        }
        foreach (IFieldActor<Player> player in State.Players.Values)
        {
            updates.Add(FieldObjectPacket.UpdatePlayer(player));
        }
        foreach (Mob mob in State.Mobs.Values)
        {
            updates.Add(FieldObjectPacket.ControlMob(mob));
            if (mob.IsDead)
            {
                RemoveMob(mob);
            }
            //else if (mob.SkillCast != null)
            //{
            //    updates.Add(SkillUsePacket.MobSkillUse(mob));
            //    mob.SkillCast = null;
            //}
        }
        foreach (TriggerScript trigger in Triggers)
        {
            trigger.Next();
        }
        return updates;
    }

    private void SendUpdates()
    {
        foreach (PacketWriter update in GetUpdates())
        {
            Broadcast(session =>
            {
                session.Send(update);
            });
        }
    }

    public IFieldObject<T> RequestFieldObject<T>(T wrappingObject)
    {
        return WrapObject(wrappingObject);
    }

    public IFieldActor<Player> RequestCharacter(Player player)
    {
        if (player.Session?.FieldPlayer != null)
        {
            // Bind existing character to this map.
            int objectId = Interlocked.Increment(ref Counter);
            ((FieldActor<Player>) player.Session.FieldPlayer).ObjectId = objectId;
            return player.Session.FieldPlayer;
        }

        return WrapPlayer(player);
    }

    public IFieldActor<NpcMetadata> RequestNpc(int npcId, CoordF coord = default, CoordF rotation = default, short animation = default)
    {
        NpcMetadata meta = NpcMetadataStorage.GetNpcMetadata(npcId);

        if (meta.Friendly == 2)
        {
            Npc npc = WrapNpc(npcId);
            npc.Coord = coord;
            npc.Rotation = rotation;
            if (animation != default)
            {
                npc.Animation = animation;
            }
            AddNpc(npc);
            return npc;
        }

        Mob mob = WrapMob(npcId);
        mob.Coord = coord;
        mob.Rotation = rotation;
        if (animation != default)
        {
            mob.Animation = animation;
        }
        AddMob(mob);
        return mob;
    }

    public IFieldActor<NpcMetadata> RequestMob(int mobId, CoordF coord = default, CoordF rotation = default, short animation = default)
    {
        Mob mob = WrapMob(mobId);
        mob.Coord = coord;
        mob.Rotation = rotation;
        if (animation != default)
        {
            mob.Animation = animation;
        }
        AddMob(mob);
        return mob;
    }

    public IFieldActor<NpcMetadata> RequestMob(int mobId, IFieldObject<MobSpawn> spawnPoint, CoordF coord = default, CoordF rotation = default, short animation = default)
    {
        Mob mob = WrapMob(mobId);
        mob.OriginSpawn = spawnPoint;
        mob.Coord = coord;
        mob.Rotation = rotation;
        if (animation != default)
        {
            mob.Animation = animation;
        }
        AddMob(mob);
        return mob;
    }

    public void AddPlayer(GameSession sender, IFieldActor<Player> player)
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
        foreach (IFieldActor<Player> existingPlayer in State.Players.Values)
        {
            sender.Send(FieldPacket.AddPlayer(existingPlayer));
            sender.Send(FieldObjectPacket.LoadPlayer(existingPlayer));
        }

        State.AddPlayer(player);
        // Broadcast new player to all players in map
        Broadcast(session =>
        {
            session.Send(FieldPacket.AddPlayer(player));
            session.Send(FieldObjectPacket.LoadPlayer(player));
        });

        foreach (IFieldObject<Item> existingItem in State.Items.Values)
        {
            sender.Send(FieldPacket.AddItem(existingItem, 123456));
        }
        foreach (IFieldActor<NpcMetadata> existingNpc in State.Npcs.Values)
        {
            sender.Send(FieldPacket.AddNpc(existingNpc));
            sender.Send(FieldObjectPacket.LoadNpc(existingNpc));
        }
        foreach (IFieldActor<NpcMetadata> existingMob in State.Mobs.Values)
        {
            sender.Send(FieldPacket.AddMob(existingMob));
            sender.Send(FieldObjectPacket.LoadMob(existingMob));

            // TODO: Determine if buffs are sent on Field Enter
        }
        foreach (IFieldObject<Portal> existingPortal in State.Portals.Values)
        {
            sender.Send(FieldPacket.AddPortal(existingPortal));
        }

        if (player.Value.MapId == (int) Map.PrivateResidence && !player.Value.IsInDecorPlanner)
        {
            // Send function cubes
            List<Cube> functionCubes = State.Cubes.Values.Where(x => x.Value.PlotNumber == 1
                                                                    && x.Value.Item.HousingCategory is ItemHousingCategory.Farming or ItemHousingCategory.Ranching)
                .Select(x => x.Value).ToList();

            if (functionCubes.Count > 0)
            {
                sender.Send(FunctionCubePacket.SendCubes(functionCubes));
            }
        }

        foreach (IFieldObject<GuideObject> guide in State.Guide.Values)
        {
            sender.Send(GuideObjectPacket.Add(guide));
        }

        foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
        {
            sender.Send(RegionSkillPacket.Send(healingSpot.ObjectId, healingSpot.Value.Coord, new(70000018, 1, 0, 1)));
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

        List<BreakableObject> breakables = new();
        breakables.AddRange(State.BreakableActors.Values.ToList());
        breakables.AddRange(State.BreakableNifs.Values.ToList());
        sender.Send(BreakablePacket.LoadBreakables(breakables));

        List<InteractObject> interactObjects = new();
        interactObjects.AddRange(State.InteractObjects.Values.Where(t => t is not AdBalloon).ToList());
        sender.Send(InteractObjectPacket.LoadInteractObject(interactObjects));

        List<AdBalloon> adBalloons = new();
        adBalloons.AddRange(State.InteractObjects.Values.OfType<AdBalloon>().ToList());
        foreach (AdBalloon balloon in adBalloons)
        {
            sender.Send(InteractObjectPacket.LoadAdBallon(balloon));
        }

        List<TriggerObject> triggerObjects = new();
        triggerObjects.AddRange(State.TriggerMeshes.Values.ToList());
        triggerObjects.AddRange(State.TriggerEffects.Values.ToList());
        triggerObjects.AddRange(State.TriggerCameras.Values.ToList());
        triggerObjects.AddRange(State.TriggerActors.Values.ToList());
        triggerObjects.AddRange(State.TriggerCubes.Values.ToList());
        triggerObjects.AddRange(State.TriggerLadders.Values.ToList());
        triggerObjects.AddRange(State.TriggerRopes.Values.ToList());
        triggerObjects.AddRange(State.TriggerSounds.Values.ToList());
        sender.Send(TriggerPacket.LoadTriggers(triggerObjects));

        if (MapLoopTask == null)
        {
            MapLoopTask = StartMapLoop(); //TODO: find a better place to initialise MapLoopTask
        }

        if (player.Value.OnlineTimeThread == null)
        {
            player.Value.OnlineTimeThread = player.Value.OnlineTimer();
        }
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
    public void AddNpc(IFieldActor<NpcMetadata> fieldNpc)
    {
        State.AddNpc(fieldNpc);

        Broadcast(session =>
        {
            session.Send(FieldPacket.AddNpc(fieldNpc));
            session.Send(FieldObjectPacket.LoadNpc(fieldNpc));
        });
    }

    public bool RemoveNpc(IFieldActor<NpcMetadata> fieldNpc)
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

    private void AddMob(Mob fieldMob)
    {
        // TODO: remove hack, better organize this
        //fieldMob.Navigator = Navigator;
        // TODO: fieldMob.Field = this;

        fieldMob.OriginSpawn?.Value.Mobs.Add(fieldMob);
        State.AddMob(fieldMob);

        Broadcast(session =>
        {
            session.Send(FieldPacket.AddMob(fieldMob));
            session.Send(FieldObjectPacket.LoadMob(fieldMob));
            for (int i = 0; i < fieldMob.Value.NpcMetadataEffect.EffectIds.Length; i++)
            {
                SkillCast effectCast = new(fieldMob.Value.NpcMetadataEffect.EffectIds[i], fieldMob.Value.NpcMetadataEffect.EffectLevels[i]);
                session.Send(BuffPacket.SendBuff(0, new Status(effectCast, fieldMob.ObjectId, fieldMob.ObjectId, 1)));
            }
        });
    }

    private bool RemoveMob(Mob mob)
    {
        if (!State.RemoveMob(mob.ObjectId))
        {
            return false;
        }

        IFieldObject<MobSpawn> originSpawn = mob.OriginSpawn;
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

    public void AddCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
    {
        State.AddCube(cube);
        BroadcastPacket(ResponseCubePacket.PlaceFurnishing(cube, houseOwnerObjectId, fieldPlayerObjectId, false));
    }

    public void RemoveCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
    {
        State.RemoveCube(cube.ObjectId);
        BroadcastPacket(ResponseCubePacket.RemoveCube(houseOwnerObjectId, fieldPlayerObjectId, cube.Coord.ToByte()));
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

    public void RemovePortal(IFieldObject<Portal> portal)
    {
        State.RemovePortal(portal.ObjectId);
        BroadcastPacket(FieldPacket.RemovePortal(portal.Value));
    }

    public void SendChat(Player player, string message, ChatType type)
    {
        Broadcast(session => session.Send(ChatPacket.Send(player, message, type)));
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
        return State.RemoveItem(objectId, out item);
    }

    public void AddResource(Item item, IFieldObject<NpcMetadata> source, IFieldObject<Player> targetPlayer)
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
    public async Task DelayBroadcastPacket(PacketWriter packet, int delay)
    {
        await Task.Factory.StartNew(async () =>
        {
            await Task.Delay(delay);
            BroadcastPacket(packet);
        });
    }

    // Providing a session will result in packet not being broadcast to self.
    public void BroadcastPacket(PacketWriter packet, GameSession sender = null)
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
        return new(objectId, fieldObject);
    }

    // Initializes a FieldActor with an objectId for this field.
    private Character WrapPlayer(Player player)
    {
        int objectId = Interlocked.Increment(ref Counter);
        return new(objectId, player);
    }

    // Initializes a FieldActor with an objectId for this field.
    private Npc WrapNpc(int npcId)
    {
        int objectId = Interlocked.Increment(ref Counter);
        return new(objectId, npcId);
    }

    // Initializes a FieldActor with an objectId for this field.
    private Mob WrapMob(int mobId)
    {
        int objectId = Interlocked.Increment(ref Counter);
        return new(objectId, mobId);
    }

    // This class is private to ensure that callers must first request entry.
    private class FieldObject<T> : IFieldObject<T>
    {
        public int ObjectId { get; set; }
        public T Value { get; }

        public virtual CoordF Coord { get; set; }
        public CoordF Rotation { get; set; }
        public short LookDirection
        {
            get => (short) (Rotation.Z * 10);
            set => Rotation = CoordF.From(Rotation.X, Rotation.Y, value / 10);
        }

        public FieldObject(int objectId, T value)
        {
            ObjectId = objectId;
            Value = value;
        }
    }

    private abstract class FieldActor<T> : FieldObject<T>, IFieldActor<T>
    {
        public CoordF Velocity { get; set; }
        public short Animation { get; set; }

        public virtual Stats Stats { get; set; }
        public bool IsDead { get; set; }

        public List<Status> Statuses { get; set; }
        public SkillCast SkillCast { get; set; }
        public bool OnCooldown { get; set; }

        //public FieldNavigator Navigator;

        // TODO: add navigation
        //public FieldNavigator Navigator;

        public FieldActor(int objectId, T value) : base(objectId, value) { }

        public virtual void UpdateFixed() { }

        public virtual void Update() { }

        public virtual void MoveBy(CoordF displacement)
        {
            // TODO: Break displacement into segements, then set path.
            Velocity = displacement;
            Coord += displacement;
            return;
        }

        public virtual void MoveTo(CoordF target)
        {
            // TODO: Perform pathfinding, then set path.
            Velocity = target - Coord;
            Coord = target;
            return;
        }

        public virtual void Cast(SkillCast skillCast)
        {
            SkillCast = skillCast;

            // TODO: Move this and all others combat cases like recover sp to its own class.
            // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
            if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
            {
                Status status = new(skillCast, ObjectId, ObjectId, 1);
                //StatusHandler.Handle(Value.Session, status);
            }
        }

        public virtual void RecoverHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Hp];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                }
            }
        }

        public virtual void ConsumeHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Hp];
                stat.Decrease(Math.Min(amount, stat.Total));
            }
        }

        public virtual void RecoverSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Spirit];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                }
            }
        }

        public virtual void ConsumeSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Spirit];
                Stats[StatId.Spirit].Decrease(Math.Min(amount, stat.Total));
            }
        }

        public virtual void RecoverStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Stamina];
                if (stat.Total < stat.Bonus)
                {
                    Stats[StatId.Stamina].Increase(Math.Min(amount, stat.Bonus - stat.Total));
                }
            }
        }

        public virtual void ConsumeStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Stamina];
                Stats[StatId.Stamina].Decrease(Math.Min(amount, stat.Total));
            }
        }

        public virtual void Damage(DamageHandler damage)
        {
            Stat health = Stats[StatId.Hp];
            health.Decrease((long) damage.Damage);
            if (health.Total <= 0)
            {
                Perish();
            }
        }

        public virtual void Perish()
        {
            IsDead = true;
        }
    }

    private class Character : FieldActor<Player>
    {
        public override CoordF Coord
        {
            get => Value.Coord;
            set => Value.Coord = value;
        }

        public override Stats Stats
        {
            get => Value.Stats;
            set => Value.Stats = value;
        }

        private CancellationTokenSource CombatCTS;

        private Task HpRegenThread;
        private Task SpRegenThread;
        private Task StaRegenThread;

        public Character(int objectId, Player value) : base(objectId, value)
        {
            if (HpRegenThread == null || HpRegenThread.IsCompleted)
            {
                HpRegenThread = StartRegen(StatId.Hp, StatId.HpRegen, StatId.HpRegenTime);
            }
            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(StatId.Spirit, StatId.SpRegen, StatId.SpRegenTime);
            }
            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(StatId.Stamina, StatId.StaRegen, StatId.StaRegenTime);
            }
        }

        public override void Cast(SkillCast skillCast)
        {
            int spiritCost = skillCast.GetSpCost();
            int staminaCost = skillCast.GetStaCost();

            if (Value.Stats[StatId.Spirit].Total >= spiritCost && Value.Stats[StatId.Stamina].Total >= staminaCost)
            {
                SkillCast = skillCast;

                ConsumeSp(spiritCost);
                ConsumeStamina(staminaCost);
                Value.Session.SendNotice(skillCast.SkillId.ToString());

                // TODO: Move this and all others combat cases like recover sp to its own class.
                // Since the cast is always sent by the skill, we have to check buffs even when not doing damage.
                if (skillCast.IsBuffToOwner() || skillCast.IsBuffToEntity() || skillCast.IsBuffShield() || skillCast.IsDebuffToOwner())
                {
                    Status status = new(skillCast, ObjectId, ObjectId, 1);
                    StatusHandler.Handle(Value.Session, status);
                }

                StartCombatStance();
            }
        }

        public override void RecoverHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Hp];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Hp));
                }
            }
        }

        public override void ConsumeHp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Hp];
                stat.Decrease(Math.Min(amount, stat.Total));
            }

            if (HpRegenThread == null || HpRegenThread.IsCompleted)
            {
                HpRegenThread = StartRegen(StatId.Hp, StatId.HpRegen, StatId.HpRegenTime);
            }
        }

        public override void RecoverSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Spirit];
                if (stat.Total < stat.Bonus)
                {
                    stat.Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Spirit));
                }
            }
        }

        public override void ConsumeSp(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Spirit];
                Stats[StatId.Spirit].Decrease(Math.Min(amount, stat.Total));
            }

            if (SpRegenThread == null || SpRegenThread.IsCompleted)
            {
                SpRegenThread = StartRegen(StatId.Spirit, StatId.SpRegen, StatId.SpRegenTime);
            }
        }

        public override void RecoverStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Stamina];
                if (stat.Total < stat.Bonus)
                {
                    Stats[StatId.Stamina].Increase(Math.Min(amount, stat.Bonus - stat.Total));
                    Value.Session.Send(StatPacket.UpdateStats(this, StatId.Stamina));
                }
            }
        }

        public override void ConsumeStamina(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            lock (Stats)
            {
                Stat stat = Stats[StatId.Stamina];
                Stats[StatId.Stamina].Decrease(Math.Min(amount, stat.Total));
            }

            if (StaRegenThread == null || StaRegenThread.IsCompleted)
            {
                StaRegenThread = StartRegen(StatId.Stamina, StatId.StaRegen, StatId.StaRegenTime);
            }
        }

        private Task StartRegen(StatId statId, StatId regenStatId, StatId timeStatId)
        {
            // TODO: merge regen updates with larger packets
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(Stats[timeStatId].Total);

                    lock (Stats)
                    {
                        if (Stats[statId].Total >= Stats[statId].Bonus)
                        {
                            return;
                        }

                        // TODO: Check if regen-enabled
                        AddStatRegen(statId, regenStatId);
                        Value.Session?.FieldManager.BroadcastPacket(StatPacket.UpdateStats(this, statId));
                        if (Value.Party != null)
                        {
                            Value.Party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(Value));
                        }
                    }
                }
            });
        }

        public Task StartCombatStance()
        {
            // Refresh out-of-combat timer
            CombatCTS?.Cancel();
            CancellationTokenSource cts = new();
            cts.Token.Register(() => cts.Dispose());
            CombatCTS = cts;

            // Enter combat
            Value.Session.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(this, true));
            return Task.Run(async () =>
            {
                await Task.Delay(5000);

                if (!cts.Token.IsCancellationRequested)
                {
                    CombatCTS = null;
                    cts.Dispose();
                    Value.Session?.FieldManager.BroadcastPacket(UserBattlePacket.UserBattle(this, false));
                }
            }, cts.Token);
        }

        private void AddStatRegen(StatId statIndex, StatId regenStatIndex)
        {
            int regenAmount = Stats[regenStatIndex].Total;
            if (regenAmount <= 0)
            {
                return;
            }

            Stat stat = Stats[statIndex];
            lock (stat)
            {
                if (stat.Total < stat.Bonus)
                {
                    int missingAmount = stat.Bonus - stat.Total;
                    stat.Increase(Math.Clamp(regenAmount, 0, missingAmount));
                }
            }
        }
    }

    private class Npc : FieldActor<NpcMetadata>, INpc
    {
        public NpcState State { get; set; }
        public NpcAction Action { get; set; }
        public MobMovement Movement { get; set; }

        public Npc(int objectId, int npcId) : this(objectId, NpcMetadataStorage.GetNpcMetadata(npcId)) { }

        public Npc(int objectId, NpcMetadata metadata) : base(objectId, metadata)
        {
            Animation = 255;
            Stats = new(metadata);
            State = NpcState.Normal;
            Action = NpcAction.Idle;
        }
    }

    private class Mob : FieldActor<NpcMetadata>, INpc
    {
        private readonly MobAI AI;
        public IFieldObject<MobSpawn> OriginSpawn;

        private CoordF SpawnDistance;

        public NpcState State { get; set; }
        public NpcAction Action { get; set; }
        public MobMovement Movement { get; set; }
        public IFieldActor<Player> Target;

        public Mob(int objectId, int mobId) : this(objectId, NpcMetadataStorage.GetNpcMetadata(mobId)) { }

        public Mob(int objectId, NpcMetadata metadata) : base(objectId, metadata)
        {
            Animation = AnimationStorage.GetSequenceIdBySequenceName(metadata.Model, "Idle_A");
            AI = MobAIManager.GetAI(metadata.AiInfo);
            Stats = new(metadata);
            State = NpcState.Normal;
        }

        public void Attack()
        {
            Console.WriteLine($"MobId {Value.Id}: {Value.NpcMetadataSkill.SkillIds.Length} {Value.NpcMetadataSkill.SkillLevels.Length} {Value.NpcMetadataSkill.SkillProbs.Length} CD {Value.NpcMetadataSkill.SkillCooldown}");
            int roll = RandomProvider.Get().Next(100);
            for (int i = 0; i < Value.NpcMetadataSkill.SkillIds.Length; i++)
            {
                if (roll < Value.NpcMetadataSkill.SkillProbs[i])
                {
                    // Rolled this skill.
                    Cast(new SkillCast(Value.NpcMetadataSkill.SkillIds[i], Value.NpcMetadataSkill.SkillLevels[i]));
                    StartSkillTimer((Value.NpcMetadataSkill.SkillCooldown > 0) ? Value.NpcMetadataSkill.SkillCooldown : 1000);
                }

                roll -= Value.NpcMetadataSkill.SkillProbs[i];
            }
        }

        private Task StartSkillTimer(int CooldownMilliseconds)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(CooldownMilliseconds);

                OnCooldown = false;
            });
        }

        public void Act()
        {
            if (AI == null)
            {
                return;
            }

            (string actionName, NpcAction actionType) = AI.GetAction(this);

            if (actionName != null)
            {
                Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, actionName);
            }
            Action = actionType;
            Movement = AI.GetMovementAction(this);

            switch (Action)
            {
                case NpcAction.Idle:
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case NpcAction.Bore:
                    Move(MobMovement.Hold); // temp, maybe remove the option to specify movement in AI
                    break;
                case NpcAction.Walk:
                case NpcAction.Run:
                    Move(Movement);
                    break;
                case NpcAction.Skill:
                    // Cast skill
                    if (!OnCooldown)
                    {
                        Attack();
                        Move(MobMovement.Hold);
                    }
                    else
                    {
                        Move(Movement);
                    }
                    break;
                case NpcAction.Jump:
                default:
                    break;
            }
        }

        public void Move(MobMovement moveType)
        {
            Random rand = RandomProvider.Get();

            switch (moveType)
            {
                case MobMovement.Patrol:
                    //if (Navigator == null)
                    //{
                        // Fallback Dummy Movement
                        int moveDistance = rand.Next(0, Value.MoveRange);
                        short moveDir = (short) rand.Next(-1800, 1800);

                        Velocity = CoordF.From(moveDistance, moveDir);
                        // Keep near spawn
                        if ((SpawnDistance - Velocity).Length() >= Block.BLOCK_SIZE * 2)
                        {
                            moveDir = (short) SpawnDistance.XYAngle();
                            Velocity = CoordF.From(Block.BLOCK_SIZE, moveDir);
                        }

                        LookDirection = moveDir; // looking direction of the monster
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        int moveDistance = rand.Next(0, Value.MoveRange);

                    //        List<CoordF> path = Navigator.GenerateRandomPath(OriginSpawn.Coord, moveDistance);
                    //        CoordF from = path[0];
                    //        CoordF to = CoordF.From(0, 0, 0);
                    //        foreach (CoordF coord in path)
                    //        {
                    //            if ((coord - from).Length() > Value.NpcMetadataSpeed.WalkSpeed)
                    //            {
                    //                break;
                    //            }
                    //            to = coord;
                    //        }

                    //        MoveTo(to);
                    //        LookDirection = (short) Velocity.XYAngle(); // looking direction of the monster
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine($"Patrol: {ex.Message}");
                    //    }
                    //}
                    break;
                case MobMovement.Follow: // move towards target
                    //if (Navigator == null)
                    //{
                        Velocity = CoordF.From(0, 0, 0);
                    //}
                    //else
                    //{
                    //    try
                    //    {
                    //        float moveDistance = Math.Min((Coord - Target.Coord).Length(), Value.MoveRange);

                    //        List<CoordF> path = Navigator.GenerateMoveToPath(Coord, Target.Coord);
                    //        CoordF from = path[0];
                    //        CoordF to = CoordF.From(0, 0, 0);
                    //        foreach (CoordF coord in path)
                    //        {
                    //            if ((coord - from).Length() > Value.NpcMetadataSpeed.RunSpeed)
                    //            {
                    //                break;
                    //            }
                    //            to = coord;
                    //        }
                    //        MoveTo(to);
                    //        LookDirection = (short) Velocity.XYAngle(); // looking direction of the monster
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Console.WriteLine($"Follow: {ex.Message}");
                    //    }
                    //}
                    break;
                case MobMovement.Strafe: // move around target
                case MobMovement.Run: // move away from target
                case MobMovement.LookAt:
                case MobMovement.Hold:
                default:
                    Velocity = CoordF.From(0, 0, 0);
                    break;
            }

            SpawnDistance -= Velocity;
        }

        public override void Perish()
        {
            IsDead = true;
            State = NpcState.Dead;
            int randAnim = RandomProvider.Get().Next(Value.StateActions[NpcState.Dead].Length);
            Animation = AnimationStorage.GetSequenceIdBySequenceName(Value.Model, Value.StateActions[NpcState.Dead][randAnim].Item1);
        }
    }

    private Task StartMapLoop()
    {
        return Task.Run(async () =>
        {
            while (!State.Players.IsEmpty)
            {
                UpdatePhysics();
                UpdateEvents();
                HealingSpot();
                UpdateObjects();
                SendUpdates();
                await Task.Delay(1000);
            }
        });
    }

    private void UpdateEvents()
    {
        // Manage mob aggro + targets
        foreach (IFieldActor<Player> player in State.Players.Values)
        {
            foreach (Mob mob in State.Mobs.Values)
            {
                float playerMobDist = CoordF.Distance(player.Coord, mob.Coord);
                if (playerMobDist <= mob.Value.NpcMetadataDistance.Sight)
                {
                    mob.State = NpcState.Combat;
                    mob.Target = player;
                }
                else
                {
                    if (mob.State == NpcState.Combat)
                    {
                        mob.State = NpcState.Normal;
                        mob.Target = null;
                    }
                }
            }
        }
    }

    private void UpdatePhysics()
    {
        foreach (Mob mob in State.Mobs.Values)
        {
            mob.Coord += mob.Velocity; // Set current position (given to ControlMob Packet)
        }
    }

    private void UpdateObjects()
    {
        foreach (Mob mob in State.Mobs.Values)
        {
            mob.Act();
        }
    }

    private void HealingSpot()
    {
        foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
        {
            CoordS healingCoord = healingSpot.Value.Coord;
            foreach (IFieldActor<Player> player in State.Players.Values)
            {
                if ((healingCoord - player.Coord.ToShort()).Length() < Block.BLOCK_SIZE * 2 && healingCoord.Z == player.Coord.ToShort().Z - 1) // 3x3x1 area
                {
                    int healAmount = (int) (player.Value.Stats[StatId.Hp].Bonus * 0.03);
                    Status status = new(new(70000018, 1, 0, 1), player.ObjectId, healingSpot.ObjectId, 1);

                    player.Value.Session.Send(BuffPacket.SendBuff(0, status));
                    BroadcastPacket(SkillDamagePacket.Heal(status, healAmount));

                    player.Stats[StatId.Hp].Increase(healAmount);
                    player.Value.Session.Send(StatPacket.UpdateStats(player, StatId.Hp));
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
            int groupSpawnCount = mob.NpcMetadataBasic.GroupSpawnCount; // Spawn count changes due to field effect (?)
            if (mobSpawn.Value.Mobs.Count + groupSpawnCount > mobSpawn.Value.MaxPopulation)
            {
                break;
            }

            for (int i = 0; i < groupSpawnCount; i++)
            {
                CoordF spawnCoord = mobSpawn.Coord + spawnPoints[mobSpawn.Value.Mobs.Count % spawnPoints.Count];
                IFieldActor<NpcMetadata> fieldMob = RequestMob(mob.Id, mobSpawn, spawnCoord);
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

    public int Increment()
    {
        return Interlocked.Increment(ref PlayerCount);
    }

    public int Decrement()
    {
        return Interlocked.Decrement(ref PlayerCount);
    }

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

    public MapTimer GetMapTimer(string id)
    {
        return MapTimers.FirstOrDefault(x => x.Id == id);
    }

    public void AddWidget(Widget widget)
    {
        Widgets.Add(widget);
    }

    public Widget GetWidget(WidgetType type)
    {
        return Widgets.FirstOrDefault(x => x.Type == type);
    }

    public void EnableSceneSkip(bool enable)
    {
        SkipScene = enable;
    }
}
