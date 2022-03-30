using System.Diagnostics;
using Maple2.PathEngine;
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

namespace MapleServer2.Managers;

// TODO: This needs to be thread safe
// TODO: FieldManager probably needs its own thread to send updates about user position
// This seems to be done every ~2s rather than on every update.
public partial class FieldManager : IDisposable
{
    private readonly ILogger Logger = LogManager.GetCurrentClassLogger();

    public readonly int MapId;
    public readonly long InstanceId;
    public readonly short Capacity;
    public readonly FieldState State = new();
    public readonly CoordS[] BoundingBox;
    public readonly TriggerScript[] Triggers;

    private int Counter = 10000000;
    private int PlayerCount;
    private readonly HashSet<GameSession> Sessions = new();
    private readonly List<MapTimer> MapTimers = new();
    private readonly List<Widget> Widgets = new();
    private Task MapLoopTask;
    private Task TriggerTask;
    private static readonly TriggerLoader TriggerLoader = new();
    private readonly FieldNavigator Navigator;
    private readonly CancellationTokenSource CancellationToken = new();

    #region Constructors

    public FieldManager(Player player)
    {
        MapId = player.MapId;

        MapMetadata metadata = MapMetadataStorage.GetMetadata(MapId);

        Capacity = metadata.Property.Capacity;
        Navigator = new(metadata.XBlockName);

        BoundingBox = MapEntityMetadataStorage.GetBoundingBox(MapId);

        // Capacity 0 means solo instances
        if (Capacity == 0)
        {
            // Set instance id to player id so it's unique
            InstanceId = player.CharacterId;
            player.InstanceId = player.CharacterId;
        }
        else
        {
            InstanceId = player.InstanceId;
        }

        // Load triggers
        Triggers = TriggerLoader.GetTriggers(metadata.XBlockName).Select(initializer =>
        {
            TriggerContext context = new(this, Logger);
            TriggerState startState = initializer.Invoke(context);
            return new TriggerScript(context, startState);
        }).ToArray();

        // Add entities to state from MapEntityStorage
        AddEntitiesToState();

        // Add to state home cubes
        if (MapId == (int) Map.PrivateResidence)
        {
            AddHomeCubes(player);
            return;
        }

        // Add to state all plots cubes
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

    #endregion

    #region Request Methods

    public IFieldObject<T> RequestFieldObject<T>(T wrappingObject)
    {
        return WrapObject(wrappingObject);
    }

    public IFieldActor<Player> RequestCharacter(Player player)
    {
        if (player.FieldPlayer is null)
        {
            return WrapPlayer(player);
        }

        // Bind existing character to this map.
        int objectId = Interlocked.Increment(ref Counter);
        ((FieldActor<Player>) player.FieldPlayer).ObjectId = objectId;
        return player.FieldPlayer;
    }

    public IFieldActor<NpcMetadata> RequestNpc(int npcId, CoordF coord = default, CoordF rotation = default, short animation = default)
    {
        NpcMetadata meta = NpcMetadataStorage.GetNpcMetadata(npcId);

        if (meta.Friendly != 2)
        {
            return RequestMob(npcId, coord, rotation, animation);
        }

        Npc npc = WrapNpc(npcId);
        npc.Coord = coord;
        npc.Rotation = rotation;
        npc.Agent = Navigator.AddAgent(npc, Navigator.AddShape(meta.NpcMetadataCapsule));

        if (animation != default)
        {
            npc.Animation = animation;
        }

        AddNpc(npc);
        return npc;
    }

    public IFieldActor<NpcMetadata> RequestMob(int mobId, CoordF coord = default, CoordF rotation = default, short animation = default)
    {
        Mob mob = WrapMob(mobId);
        mob.Coord = coord;
        mob.Rotation = rotation;
        mob.Agent = Navigator.AddAgent(mob, Navigator.AddShape(mob.Value.NpcMetadataCapsule));
        mob.Navigator = Navigator;

        if (animation != default)
        {
            mob.Animation = animation;
        }

        AddMob(mob);
        return mob;
    }

    public IFieldActor<NpcMetadata> RequestMob(int mobId, IFieldObject<MobSpawn> spawnPoint)
    {
        Mob mob = WrapMob(mobId);
        mob.OriginSpawn = spawnPoint;

        Shape shape = Navigator.AddShape(mob.Value.NpcMetadataCapsule);
        CoordS? randomPositionAround = Navigator.FindClosestUnobstructedCoordS(shape, spawnPoint.Coord, spawnPoint.Value.SpawnRadius);
        if (randomPositionAround is null)
        {
            Logger.Error("Could not find a random position around spawn point {0}", spawnPoint.Coord);
            return null;
        }

        mob.Coord = randomPositionAround.Value.ToFloat();
        mob.Rotation = default;
        mob.Animation = default;
        mob.Agent = Navigator.AddAgent(mob, shape);
        mob.Navigator = Navigator;

        mob.OriginSpawn.Value.Mobs.Add(mob);
        AddMob(mob);
        return mob;
    }

    #endregion

    #region Public Methods

    public int Increment() => Interlocked.Increment(ref PlayerCount);

    public int Decrement() => Interlocked.Decrement(ref PlayerCount);

    public void AddPlayer(GameSession sender)
    {
        Player player = sender.Player;
        Debug.Assert(player.FieldPlayer.ObjectId > 0, "Player was added to field without initialized objectId.");

        player.MapId = MapId;
        if (Capacity == 0)
        {
            MapPlayerSpawn spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(MapId);
            player.FieldPlayer.Coord = spawn.Coord.ToFloat();
            player.FieldPlayer.Rotation = spawn.Rotation.ToFloat();
        }
        else
        {
            player.FieldPlayer.Coord = player.SavedCoord;
            player.FieldPlayer.Rotation = player.SavedRotation;
        }

        player.SafeBlock = player.SavedCoord;

        lock (Sessions)
        {
            Sessions.Add(sender);
        }

        // TODO: Send the initialization state of the field
        foreach (IFieldActor<Player> existingPlayer in State.Players.Values)
        {
            sender.Send(FieldPlayerPacket.AddPlayer(existingPlayer));
            sender.Send(FieldObjectPacket.LoadPlayer(existingPlayer));
        }

        State.AddPlayer(player.FieldPlayer);
        // Broadcast new player to all players in map
        Broadcast(session =>
        {
            session.Send(FieldPlayerPacket.AddPlayer(player.FieldPlayer));
            session.Send(FieldObjectPacket.LoadPlayer(player.FieldPlayer));
        });

        foreach (IFieldObject<Item> existingItem in State.Items.Values)
        {
            sender.Send(FieldItemPacket.AddItem(existingItem, 123456));
        }

        foreach (IFieldActor<NpcMetadata> existingNpc in State.Npcs.Values)
        {
            sender.Send(FieldNpcPacket.AddNpc(existingNpc));
            sender.Send(FieldObjectPacket.LoadNpc(existingNpc));
        }

        foreach (IFieldActor<NpcMetadata> existingMob in State.Mobs.Values)
        {
            sender.Send(FieldNpcPacket.AddMob(existingMob));
            sender.Send(FieldObjectPacket.LoadMob(existingMob));

            // TODO: Determine if buffs are sent on Field Enter
        }

        foreach (IFieldObject<Portal> existingPortal in State.Portals.Values)
        {
            sender.Send(FieldPortalPacket.AddPortal(existingPortal));
        }

        if (player.MapId == (int) Map.PrivateResidence && !player.IsInDecorPlanner)
        {
            // Send function cubes
            List<Cube> functionCubes = State.Cubes.Values
                .Where(x => x.Value.PlotNumber == 1 && x.Value.Item.HousingCategory is ItemHousingCategory.Farming or ItemHousingCategory.Ranching)
                .Select(x => x.Value).ToList();

            if (functionCubes.Count > 0)
            {
                sender.Send(FunctionCubePacket.SendCubes(functionCubes));
            }
        }

        foreach (IFieldObject<GuideObject> guide in State.Guide.Values)
        {
            if (guide.Value.IsBall)
            {
                sender.Send(HomeActionPacket.AddBall(guide));
                continue;
            }

            sender.Send(GuideObjectPacket.Add(guide));
        }

        foreach (IFieldObject<HealingSpot> healingSpot in State.HealingSpots.Values)
        {
            SkillCast skillCast = new(70000018, 1)
            {
                SkillObjectId = healingSpot.ObjectId,
                CasterObjectId = healingSpot.ObjectId
            };
            skillCast.EffectCoords.Add(healingSpot.Value.Coord.ToFloat());
            sender.Send(RegionSkillPacket.Send(skillCast));
        }

        foreach (IFieldObject<Instrument> instrument in State.Instruments.Values)
        {
            if (instrument.Value.Improvise)
            {
                sender.Send(InstrumentPacket.StartImprovise(instrument));
                continue;
            }

            sender.Send(InstrumentPacket.PlayScore(instrument));
        }

        sender.Send(LiftablePacket.LoadLiftables(State.LiftableObjects.Values.ToList()));

        List<BreakableObject> breakables = new();
        breakables.AddRange(State.BreakableActors.Values.ToList());
        breakables.AddRange(State.BreakableNifs.Values.ToList());
        sender.Send(BreakablePacket.LoadBreakables(breakables));

        sender.Send(InteractObjectPacket.LoadObjects(State.InteractObjects.Values.Where(t => t is not AdBalloon && t is not MapChest).ToList()));

        foreach (MapChest mapChest in State.InteractObjects.Values.OfType<MapChest>())
        {
            sender.Send(InteractObjectPacket.Add(mapChest));
        }

        foreach (AdBalloon balloon in State.InteractObjects.Values.OfType<AdBalloon>())
        {
            sender.Send(InteractObjectPacket.Add(balloon));
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

        if (MapLoopTask is null)
        {
            MapLoopTask = StartMapLoop(); //TODO: find a better place to initialise MapLoopTask
        }

        if (TriggerTask is null)
        {
            TriggerTask = StartTriggerTask();
        }

        if (player.OnlineTimeThread is null)
        {
            player.OnlineTimeThread = player.OnlineTimer();
        }
    }

    public void RemovePlayer(GameSession sender)
    {
        Player player = sender.Player;
        lock (Sessions)
        {
            Sessions.Remove(sender);
        }

        State.RemovePlayer(player.FieldPlayer.ObjectId);
        player.Triggers.Clear();

        // Remove player
        Broadcast(session =>
        {
            session.Send(FieldPlayerPacket.RemovePlayer(player.FieldPlayer));
            session.Send(FieldObjectPacket.RemovePlayer(player.FieldPlayer));
        });

        ((FieldObject<Player>) player.FieldPlayer).ObjectId = -1; // Reset object id
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
    private void AddNpc(IFieldActor<NpcMetadata> fieldNpc)
    {
        State.AddNpc(fieldNpc);

        Broadcast(session =>
        {
            session.Send(FieldNpcPacket.AddNpc(fieldNpc));
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
            session.Send(FieldNpcPacket.RemoveNpc(fieldNpc));
            session.Send(FieldObjectPacket.RemoveNpc(fieldNpc));
        });

        fieldNpc.Dispose();
        return true;
    }

    private void AddMob(Mob fieldMob)
    {
        State.AddMob(fieldMob);

        Broadcast(session =>
        {
            session.Send(FieldNpcPacket.AddMob(fieldMob));

            session.Send(FieldObjectPacket.LoadMob(fieldMob));
            for (int i = 0; i < fieldMob.Value.NpcMetadataEffect.EffectIds.Length; i++)
            {
                SkillCast effectCast = new(fieldMob.Value.NpcMetadataEffect.EffectIds[i], fieldMob.Value.NpcMetadataEffect.EffectLevels[i]);
                session.Send(BuffPacket.SendBuff(0, new(effectCast, fieldMob.ObjectId, fieldMob.ObjectId, 1)));
            }
        });
    }

    public bool RemoveMob(IFieldActor<NpcMetadata> mob)
    {
        if (!State.RemoveMob(mob.ObjectId))
        {
            return false;
        }

        if (mob is Mob fieldMob)
        {
            IFieldObject<MobSpawn> originSpawn = fieldMob.OriginSpawn;
            if (originSpawn != null && originSpawn.Value.Mobs.Remove(mob) && originSpawn.Value.Mobs.Count == 0)
            {
                StartSpawnTimer(originSpawn);
            }
        }

        Broadcast(session =>
        {
            session.Send(FieldNpcPacket.RemoveNpc(mob));
            session.Send(FieldObjectPacket.RemoveMob(mob));
        });

        mob.Dispose();
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
        if (cube.Value.Item.HousingCategory is ItemHousingCategory.Ranching or ItemHousingCategory.Farming)
        {
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 1, 0));
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 2, 1));
        }

        if (cube.Value.Item.Id == 50400158) // portal cube
        {
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 0, 0));
        }

        State.AddCube(cube);
        BroadcastPacket(ResponseCubePacket.PlaceFurnishing(cube, houseOwnerObjectId, fieldPlayerObjectId, false));
    }

    public void RemoveCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
    {
        if (cube.Value.Item.Id == 50400158) // portal cube
        {
            State.Portals.TryGetValue(cube.Value.PortalSettings.PortalObjectId, out IFieldObject<Portal> fieldPortal);
            RemovePortal(fieldPortal);
        }

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
        BroadcastPacket(FieldPortalPacket.AddPortal(portal));
    }

    public void RemovePortal(IFieldObject<Portal> portal)
    {
        State.RemovePortal(portal.ObjectId);
        BroadcastPacket(FieldPortalPacket.RemovePortal(portal.Value));
    }

    public void SendChat(Player player, string message, ChatType type)
    {
        Broadcast(session => session.Send(ChatPacket.Send(player, message, type)));
    }

    public void AddItem(GameSession sender, Item item)
    {
        FieldObject<Item> fieldItem = WrapObject(item);
        fieldItem.Coord = sender.Player.FieldPlayer.Coord;

        State.AddItem(fieldItem);

        Broadcast(session =>
        {
            session.Send(FieldItemPacket.AddItem(fieldItem, session.Player.FieldPlayer.ObjectId));
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
            session.Send(FieldItemPacket.AddItem(fieldItem, source, targetPlayer));
        });
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

    public void AddSkillCast(SkillCast skillCast) => State.AddSkillCast(skillCast);

    public bool RemoveSkillCast(long skillSn, out SkillCast skillCast) => State.RemoveSkillCast(skillSn, out skillCast);

    public void AddRegionSkillEffect(SkillCast skillCast)
    {
        int objectId = Interlocked.Increment(ref Counter);
        skillCast.SkillObjectId = objectId;

        AddSkillCast(skillCast);
        BroadcastPacket(RegionSkillPacket.Send(skillCast));
    }

    public bool RemoveRegionSkillEffect(SkillCast skillCast)
    {
        if (!RemoveSkillCast(skillCast.SkillSn, out skillCast))
        {
            return false;
        }

        BroadcastPacket(RegionSkillPacket.Remove(skillCast.SkillObjectId));
        return true;
    }

    #endregion

    #region Broadcast Methods

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

    #endregion

    #region Wrap Methods

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

    #endregion

    private void SpawnMobs(IFieldObject<MobSpawn> mobSpawn)
    {
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
                RequestMob(mob.Id, mobSpawn);
            }
        }
    }

    private void AddHomeCubes(Player player)
    {
        Home home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
        if (home is null)
        {
            return;
        }

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
            fieldPortal.Value.UgcPortalMethod = cubePortal.PortalSettings.Method;
            if (!string.IsNullOrEmpty(cubePortal.PortalSettings.DestinationTarget))
            {
                switch (cubePortal.PortalSettings.Destination)
                {
                    case UgcPortalDestination.PortalInHome:
                        fieldPortal.Value.TargetMapId = (int) Map.PrivateResidence;
                        break;
                    case UgcPortalDestination.SelectedMap:
                        fieldPortal.Value.TargetMapId = int.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                    case UgcPortalDestination.FriendHome:
                        fieldPortal.Value.TargetHomeAccountId = long.Parse(cubePortal.PortalSettings.DestinationTarget);
                        break;
                }
            }

            cubePortal.PortalSettings.PortalObjectId = fieldPortal.ObjectId;
            AddPortal(fieldPortal);
        }
    }

    private void AddEntitiesToState()
    {
        // Load default npcs for map from config
        foreach (MapNpc npc in MapEntityMetadataStorage.GetNpcs(MapId))
        {
            RequestNpc(npc.Id, npc.Coord.ToFloat(), npc.Rotation.ToFloat());
        }

        // Spawn map's mobs at the mob spawners
        foreach (MapMobSpawn mobSpawn in MapEntityMetadataStorage.GetMobSpawns(MapId))
        {
            if (mobSpawn.SpawnData is null)
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
        foreach (MapPortal portal in MapEntityMetadataStorage.GetPortals(MapId))
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
            fieldPortal.Rotation = portal.Rotation.ToFloat();
            AddPortal(fieldPortal);
        }

        foreach (MapTriggerMesh mapTriggerMesh in MapEntityMetadataStorage.GetTriggerMeshes(MapId))
        {
            if (mapTriggerMesh is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerMesh(mapTriggerMesh.Id, mapTriggerMesh.IsVisible));
        }

        foreach (MapTriggerEffect mapTriggerEffect in MapEntityMetadataStorage.GetTriggerEffects(MapId))
        {
            if (mapTriggerEffect is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerEffect(mapTriggerEffect.Id, mapTriggerEffect.IsVisible));
        }

        foreach (MapTriggerActor mapTriggerActor in MapEntityMetadataStorage.GetTriggerActors(MapId))
        {
            if (mapTriggerActor is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerActor(mapTriggerActor.Id, mapTriggerActor.IsVisible, mapTriggerActor.InitialSequence));
        }

        foreach (MapTriggerCamera mapTriggerCamera in MapEntityMetadataStorage.GetTriggerCameras(MapId))
        {
            if (mapTriggerCamera is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerCamera(mapTriggerCamera.Id, mapTriggerCamera.IsEnabled));
        }

        foreach (MapTriggerCube mapTriggerCube in MapEntityMetadataStorage.GetTriggerCubes(MapId))
        {
            if (mapTriggerCube is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerCube(mapTriggerCube.Id, mapTriggerCube.IsVisible));
        }

        foreach (MapTriggerLadder mapTriggerLadder in MapEntityMetadataStorage.GetTriggerLadders(MapId))
        {
            if (mapTriggerLadder is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerLadder(mapTriggerLadder.Id, mapTriggerLadder.IsVisible));
        }

        foreach (MapTriggerRope mapTriggerRope in MapEntityMetadataStorage.GetTriggerRopes(MapId))
        {
            if (mapTriggerRope is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerRope(mapTriggerRope.Id, mapTriggerRope.IsVisible));
        }

        foreach (MapTriggerSound mapTriggerSound in MapEntityMetadataStorage.GetTriggerSounds(MapId))
        {
            if (mapTriggerSound is null)
            {
                continue;
            }

            State.AddTriggerObject(new TriggerSound(mapTriggerSound.Id, mapTriggerSound.IsEnabled));
        }

        foreach (MapTriggerSkill mapTriggerSkill in MapEntityMetadataStorage.GetTriggerSkills(MapId))
        {
            if (mapTriggerSkill is null)
            {
                continue;
            }

            TriggerSkill triggerSkill = new(mapTriggerSkill.Id,
                mapTriggerSkill.SkillId,
                mapTriggerSkill.SkillLevel,
                mapTriggerSkill.Count,
                mapTriggerSkill.Position);
            IFieldObject<TriggerSkill> fieldTriggerSkill = RequestFieldObject(triggerSkill);
            fieldTriggerSkill.Coord = fieldTriggerSkill.Value.Position;

            State.AddTriggerSkills(fieldTriggerSkill);
        }

        // Load breakables
        foreach (MapBreakableActorObject mapActor in MapEntityMetadataStorage.GetBreakableActors(MapId))
        {
            if (mapActor is null)
            {
                continue;
            }

            State.AddBreakable(new(mapActor.EntityId, mapActor.IsEnabled, mapActor.HideDuration, mapActor.ResetDuration));
        }

        foreach (MapBreakableNifObject mapNif in MapEntityMetadataStorage.GetBreakableNifs(MapId))
        {
            if (mapNif is null)
            {
                continue;
            }

            State.AddBreakable(new BreakableNifObject(mapNif.EntityId, mapNif.IsEnabled, mapNif.TriggerId, mapNif.HideDuration, mapNif.ResetDuration));
        }

        // Load interact objects
        foreach (MapInteractObject mapInteract in MapEntityMetadataStorage.GetInteractObjects(MapId))
        {
            if (mapInteract is null)
            {
                continue;
            }

            State.AddInteractObject(new(mapInteract.EntityId, mapInteract.InteractId, mapInteract.Type, InteractObjectState.Default));
        }

        foreach (MapLiftableObject liftable in MapEntityMetadataStorage.GetLiftablesObjects(MapId))
        {
            if (liftable is null)
            {
                continue;
            }

            State.AddLiftableObject(new(liftable.EntityId, liftable));
        }

        foreach (MapChestMetadata mapChest in MapEntityMetadataStorage.GetMapChests(MapId))
        {
            if (mapChest is null)
            {
                continue;
            }

            // TODO: Create a chest manager to spawn chests randomly and
            // TODO: Golden chests ids should always increase by 1 when a new chest is added
            // For more details about chests, see https://github.com/AlanMorel/MapleServer2/issues/513
            int chestId = mapChest.IsGolden ? 14000147 : 11000004;
            State.AddInteractObject(
                new MapChest($"EventCreate_{GuidGenerator.Int()}", chestId, InteractObjectType.Common, InteractObjectState.Default)
                {
                    Position = mapChest.Position,
                    Rotation = mapChest.Rotation,
                    Model = "MS2InteractActor",
                    Asset = mapChest.IsGolden ? "interaction_chestA_02" : "interaction_chestA_01", // 01 = wooden, 02 = golden
                    NormalState = "Opened_A",
                    Reactable = "Idle_A",
                    Scale = 1f
                }
            );
        }

        foreach (CoordS coord in MapEntityMetadataStorage.GetHealingSpot(MapId))
        {
            State.AddHealingSpot(RequestFieldObject(new HealingSpot(GuidGenerator.Int(), coord)));
        }

        foreach (MapVibrateObject mapVibrateObject in MapEntityMetadataStorage.GetVibrateObjects(MapId))
        {
            State.AddVibrateObject(mapVibrateObject);
        }
    }

    #region Map loop

    private Task StartMapLoop()
    {
        CancellationToken ct = CancellationToken.Token;
        return Task.Run(async () =>
        {
            while (!State.Players.IsEmpty)
            {
                UpdatePhysics();
                UpdateEvents();
                UpdateObjects();
                HealingSpot();
                SendUpdates();
                await Task.Delay(1000, ct);
            }
        }, ct);
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

    // Gets a list of packets to update the state of all field objects for client.
    private IEnumerable<PacketWriter> GetUpdates()
    {
        List<PacketWriter> updates = new();
        // Update NPCs
        foreach (Npc npc in State.Npcs.Values)
        {
            updates.Add(FieldObjectPacket.ControlNpc(npc));
        }

        // Update players state
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
        }

        return updates;
    }

    private void UpdatePhysics()
    {
        foreach (Mob mob in State.Mobs.Values)
        {
            mob.Coord += mob.Velocity; // Set current position (given to ControlMob Packet)
        }
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
                    continue;
                }

                if (mob.State != NpcState.Combat)
                {
                    continue;
                }

                mob.State = NpcState.Normal;
                mob.Target = null;
            }
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
                if ((healingCoord - player.Coord.ToShort()).Length() >= Block.BLOCK_SIZE * 2 || healingCoord.Z != player.Coord.ToShort().Z - 1)
                {
                    continue;
                }

                int healAmount = (int) (player.Value.Stats[StatAttribute.Hp].Bonus * 0.03);
                Status status = new(new(70000018, 1, 0, 1), player.ObjectId, healingSpot.ObjectId, 1);

                player.Value.Session.Send(BuffPacket.SendBuff(0, status));
                BroadcastPacket(SkillDamagePacket.Heal(status, healAmount));

                player.Stats[StatAttribute.Hp].Increase(healAmount);
                player.Value.Session.Send(StatPacket.UpdateStats(player, StatAttribute.Hp));
            }
        }
    }

    private Task StartTriggerTask()
    {
        CancellationToken ct = CancellationToken.Token;
        return Task.Run(async () =>
        {
            while (!State.Players.IsEmpty)
            {
                foreach (TriggerScript trigger in Triggers)
                {
                    try
                    {
                        trigger.Next();
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e);
                        // Disconnect everyone in the field if a trigger has an exception
                        foreach (IFieldActor<Player> fieldPlayers in State.Players.Values)
                        {
                            fieldPlayers.Value.Session.Disconnect(logoutNotice: true);
                        }
                    }
                }

                await Task.Delay(200, ct);
            }
        }, ct);
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

    #endregion

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

    private abstract partial class FieldActor<T>
    {
    }

    private partial class Character
    {
    }

    private partial class Mob
    {
    }

    private partial class Npc
    {
    }

    public void Dispose()
    {
        CancellationToken.Cancel();
        TaskUtils.WaitForTask(MapLoopTask);
        TaskUtils.WaitForTask(TriggerTask);

        MapLoopTask?.Dispose();
        TriggerTask?.Dispose();
        Navigator?.Dispose();
        GC.SuppressFinalize(this);
    }

    ~FieldManager()
    {
        Dispose();
    }
}
