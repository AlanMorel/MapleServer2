using System.Diagnostics;
using Maple2.PathEngine;
using Maple2.PathEngine.Types;
using Maple2.Trigger;
using Maple2.Trigger.Enum;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Managers.Actors;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Triggers;
using MapleServer2.Types;
using Serilog;
using TaskScheduler = MapleServer2.Tools.TaskScheduler;

namespace MapleServer2.Managers;

// This needs to be thread safe
public class FieldManager
{
    private readonly ILogger Logger = Log.Logger.ForContext<FieldManager>();

    public readonly int MapId;
    public readonly long InstanceId;
    public readonly int Capacity;
    public readonly bool IsTutorialMap;
    public readonly FieldState State = new();
    public readonly CoordS[]? BoundingBox;
    public readonly TriggerScript[] Triggers;

    private static int GlobalIdCounter = 1_000_000;
    private int LocalIdCounter = 10_000_000;
    private int PlayerCount;
    private readonly List<MapTimer> MapTimers = new();
    private readonly List<Widget> Widgets = new();
    private Task? MapLoopTask;
    private Task? TriggerTask;
    private Task? NpcMovementTask;
    private Timer? UGCBannerTimer;
    private static readonly TriggerLoader TriggerLoader = new();
    public readonly FieldNavigator? Navigator;

    #region Constructors

    public FieldManager(Player player)
    {
        MapMetadata? metadata = MapMetadataStorage.GetMetadata(player.MapId);

        if (metadata is null)
        {
            throw new($"No metadata found for map {player.MapId}");
        }

        MapId = player.MapId;
        Capacity = metadata.Property.Capacity;
        IsTutorialMap = metadata.Property.IsTutorialMap;
        if (File.Exists(Paths.NAVMESH_DIR + $"/{metadata.XBlockName}.tok"))
        {
            Navigator = new(metadata.XBlockName);
        }
        else
        {
            Logger.Warning("No navmesh found for map {0}", metadata.XBlockName);
        }

        BoundingBox = MapEntityMetadataStorage.GetBoundingBox(MapId);

        // Capacity 0 means solo instances
        if (Capacity == 0 || IsTutorialMap)
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

    public Character RequestCharacter(Player player) => player.FieldPlayer ?? WrapPlayer(player);

    public Npc? RequestNpc(int npcId, CoordF coord = default, CoordF rotation = default, short animation = -1)
    {
        NpcMetadata? meta = NpcMetadataStorage.GetNpcMetadata(npcId);
        if (meta is null)
        {
            return null;
        }

        Npc npc = WrapNpc(meta);
        if (Navigator?.FindFirstCoordSBelow(coord, out CoordS coordBelow) ?? false)
        {
            npc.Coord = coordBelow;
        }
        else
        {
            npc.Coord = coord;
        }

        npc.Rotation = rotation;
        npc.Agent = Navigator?.AddAgent(npc, Navigator.AddShape(meta.NpcMetadataCapsule));

        if (animation != -1)
        {
            npc.Animation = animation;
        }

        AddNpc(npc);
        return npc;
    }

    private void RequestNpc(MapNpc mapNpc, CoordF coord, CoordF rotation)
    {
        Npc? npc = RequestNpc(mapNpc.Id, coord, rotation);

        if (string.IsNullOrEmpty(mapNpc.PatrolDataUuid))
        {
            return;
        }

        PatrolData? patrolDataByUuid = MapEntityMetadataStorage.GetPatrolDataByUuid(MapId, mapNpc.PatrolDataUuid.Replace("-", string.Empty));
        if (patrolDataByUuid is null)
        {
            return;
        }

        npc?.SetPatrolData(patrolDataByUuid);
    }

    private void RequestMob(int mobId, IFieldObject<MobSpawn> spawnPoint)
    {
        Npc npc = WrapMob(mobId);
        npc.OriginSpawn = spawnPoint;

        if (Navigator is null)
        {
            return;
        }

        Shape shape = Navigator.AddShape(npc.Value.NpcMetadataCapsule);
        Position spawnPointPosition = Navigator.FindPositionFromCoordS(spawnPoint.Coord);
        if (!Navigator.PositionIsValid(spawnPointPosition))
        {
            if (!Navigator.FindFirstPositionBelow(spawnPoint.Coord, out spawnPointPosition))
            {
                Logger.Warning("Could not find a random position around spawn point {0}, in map ID {1} for mob ID {2}", spawnPoint.Coord, MapId,
                    npc.Value.Id);
                return;
            }
        }

        CoordS? randomPositionAround = Navigator.FindClosestUnobstructedCoordS(shape, spawnPointPosition, spawnPoint.Value.SpawnRadius);
        if (randomPositionAround is null || randomPositionAround.Value == CoordS.From(0, 0, 0))
        {
            Logger.Warning("Could not find a random position around spawn point {0}, in map ID {1} for mob ID {2}", spawnPoint.Coord, MapId, npc.Value.Id);
            return;
        }

        npc.Coord = randomPositionAround.Value.ToFloat();
        npc.Rotation = default;
        npc.Animation = default;
        npc.Agent = Navigator?.AddAgent(npc, shape);

        npc.OriginSpawn.Value.Mobs.Add(npc);
        AddNpc(npc);
    }

    public Pet? RequestPet(Item item, Character character)
    {
        Pet pet = new(NextLocalId(), item, character, fieldManager: this);
        if (Navigator is null)
        {
            return null;
        }

        Shape shape = Navigator.AddShape(pet.Value.NpcMetadataCapsule);
        Position spawnPointPosition = Navigator.FindPositionFromCoordS(character.Coord);
        if (!Navigator.PositionIsValid(spawnPointPosition))
        {
            if (!Navigator.FindFirstPositionBelow(character.Coord, out spawnPointPosition))
            {
                Logger.Warning("Could not find a random position around character obj id {0}, in map ID {1} for pet ID {2}",
                    character.ObjectId, MapId, pet.Value.Id);
                return null;
            }
        }

        CoordS? randomPositionAround = Navigator.FindClosestUnobstructedCoordS(shape, spawnPointPosition, pet.Value.NpcMetadataDistance.Avoid);
        if (randomPositionAround is null || randomPositionAround.Value == CoordS.From(0, 0, 0))
        {
            Logger.Warning("Could not find a random position around character obj id {0}, in map ID {1} for pet ID {2}",
                character.ObjectId, MapId, pet.Value.Id);
            return null;
        }

        pet.Coord = randomPositionAround.Value.ToFloat();
        pet.Rotation = default;
        pet.Animation = default;
        pet.Agent = Navigator.AddAgent(pet, shape);

        AddPet(pet);
        return pet;
    }

    public void AddPet(GameSession session, long uid)
    {
        Player player = session.Player;
        Item? item = player.Inventory.GetByUid(uid);
        if (item is null)
        {
            return;
        }

        if (player.FieldPlayer == null)
        {
            return;
        }

        Pet? pet = RequestPet(item, player.FieldPlayer);
        if (pet is null)
        {
            return;
        }

        if (item.TransferType == TransferType.BindOnSummonEnchantOrReroll & !item.IsBound())
        {
            item.BindItem(session.Player);
        }

        player.ActivePet = pet.Item;
        player.FieldPlayer.ActivePet = pet;

        session.Send(PetPacket.LoadPetSettings(pet));
        session.Send(NoticePacket.Notice(SystemNotice.PetSummonOn, NoticeType.Chat | NoticeType.FastText));
    }

    private void AddEntitiesToState()
    {
        // Load default npcs for map from config
        foreach (MapNpc npc in MapEntityMetadataStorage.GetNpcs(MapId)!)
        {
            try
            {
                RequestNpc(npc, npc.Coord.ToFloat(), npc.Rotation.ToFloat());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Spawn map's mobs at the mob spawners
        foreach (MapMobSpawn mobSpawn in MapEntityMetadataStorage.GetMobSpawns(MapId)!)
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
        foreach (MapPortal portal in MapEntityMetadataStorage.GetPortals(MapId)!)
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

        foreach (MapTriggerMesh mapTriggerMesh in MapEntityMetadataStorage.GetTriggerMeshes(MapId)!)
        {
            State.AddTriggerObject(new TriggerMesh(mapTriggerMesh.Id, mapTriggerMesh.IsVisible));
        }

        foreach (MapTriggerEffect mapTriggerEffect in MapEntityMetadataStorage.GetTriggerEffects(MapId)!)
        {
            State.AddTriggerObject(new TriggerEffect(mapTriggerEffect.Id, mapTriggerEffect.IsVisible));
        }

        foreach (MapTriggerActor mapTriggerActor in MapEntityMetadataStorage.GetTriggerActors(MapId)!)
        {
            State.AddTriggerObject(new TriggerActor(mapTriggerActor.Id, mapTriggerActor.IsVisible, mapTriggerActor.InitialSequence));
        }

        foreach (MapTriggerCamera mapTriggerCamera in MapEntityMetadataStorage.GetTriggerCameras(MapId)!)
        {
            State.AddTriggerObject(new TriggerCamera(mapTriggerCamera.Id, mapTriggerCamera.IsEnabled));
        }

        foreach (MapTriggerCube mapTriggerCube in MapEntityMetadataStorage.GetTriggerCubes(MapId)!)
        {
            State.AddTriggerObject(new TriggerCube(mapTriggerCube.Id, mapTriggerCube.IsVisible));
        }

        foreach (MapTriggerLadder mapTriggerLadder in MapEntityMetadataStorage.GetTriggerLadders(MapId)!)
        {
            State.AddTriggerObject(new TriggerLadder(mapTriggerLadder.Id, mapTriggerLadder.IsVisible));
        }

        foreach (MapTriggerRope mapTriggerRope in MapEntityMetadataStorage.GetTriggerRopes(MapId)!)
        {
            State.AddTriggerObject(new TriggerRope(mapTriggerRope.Id, mapTriggerRope.IsVisible));
        }

        foreach (MapTriggerSound mapTriggerSound in MapEntityMetadataStorage.GetTriggerSounds(MapId)!)
        {
            State.AddTriggerObject(new TriggerSound(mapTriggerSound.Id, mapTriggerSound.IsEnabled));
        }

        foreach (MapTriggerSkill mapTriggerSkill in MapEntityMetadataStorage.GetTriggerSkills(MapId)!)
        {
            TriggerSkill triggerSkill = new(mapTriggerSkill.Id,
                mapTriggerSkill.SkillId,
                mapTriggerSkill.SkillLevel,
                mapTriggerSkill.Count,
                mapTriggerSkill.Position,
                null);
            IFieldObject<TriggerSkill> fieldTriggerSkill = RequestFieldObject(triggerSkill);
            fieldTriggerSkill.Coord = fieldTriggerSkill.Value.Position;

            State.AddTriggerSkills(fieldTriggerSkill);
        }

        // Load breakables
        foreach (MapBreakableActorObject mapActor in MapEntityMetadataStorage.GetBreakableActors(MapId)!)
        {
            State.AddBreakable(new BreakableActorObject(mapActor.EntityId, mapActor.IsEnabled, mapActor.HideDuration, mapActor.ResetDuration));
        }

        foreach (MapBreakableNifObject mapNif in MapEntityMetadataStorage.GetBreakableNifs(MapId)!)
        {
            State.AddBreakable(new BreakableNifObject(mapNif.EntityId, mapNif.IsEnabled, mapNif.TriggerId, mapNif.HideDuration, mapNif.ResetDuration));
        }

        // Load interact objects
        foreach (MapInteractObject mapInteract in MapEntityMetadataStorage.GetInteractObjects(MapId)!)
        {
            FieldObject<InteractObject> fieldInteractObject =
                WrapObject(new InteractObject(mapInteract.EntityId, mapInteract.InteractId, mapInteract.Type, InteractObjectState.Default));
            fieldInteractObject.Coord = mapInteract.Position;
            fieldInteractObject.Rotation = mapInteract.Rotation;
            State.AddInteractObject(fieldInteractObject);
        }

        foreach (MapLiftableObject liftable in MapEntityMetadataStorage.GetLiftablesObjects(MapId)!)
        {
            FieldObject<LiftableObject> fieldLiftableObject = WrapObject(new LiftableObject(liftable.EntityId, liftable));
            fieldLiftableObject.Coord = liftable.Position;
            fieldLiftableObject.Rotation = liftable.Rotation;
            State.AddLiftableObject(fieldLiftableObject);
        }

        foreach (MapChestMetadata mapChest in MapEntityMetadataStorage.GetMapChests(MapId)!)
        {
            // TODO: Create a chest manager to spawn chests randomly and
            // TODO: Golden chests ids should always increase by 1 when a new chest is added
            // For more details about chests, see https://github.com/AlanMorel/MapleServer2/issues/513
            int chestId = mapChest.IsGolden ? 14000147 : 11000004;
            IFieldObject<InteractObject> fieldChest = WrapObject(
                new MapChest($"EventCreate_{GuidGenerator.Int()}", chestId, InteractObjectType.Common, InteractObjectState.Default)
                {
                    Model = "MS2InteractActor",
                    Asset = mapChest.IsGolden ? "interaction_chestA_02" : "interaction_chestA_01", // 01 = wooden, 02 = golden
                    NormalState = "Opened_A",
                    Reactable = "Idle_A",
                    Scale = 1f
                });
            fieldChest.Coord = mapChest.Position;
            fieldChest.Rotation = mapChest.Rotation;
            State.AddInteractObject(fieldChest);
        }

        foreach (CoordS coord in MapEntityMetadataStorage.GetHealingSpot(MapId)!)
        {
            State.AddHealingSpot(RequestFieldObject(new HealingSpot(coord)));
        }

        foreach (MapVibrateObject mapVibrateObject in MapEntityMetadataStorage.GetVibrateObjects(MapId)!)
        {
            State.AddVibrateObject(mapVibrateObject);
        }
    }

    #endregion

    #region State Methods

    private void Increment() => Interlocked.Increment(ref PlayerCount);

    private int Decrement() => Interlocked.Decrement(ref PlayerCount);

    public void AddPlayer(GameSession sender)
    {
        Player player = sender.Player;
        Debug.Assert(player.FieldPlayer?.ObjectId > 0, "Player was added to field without initialized objectId.");

        player.MapId = MapId;
        if (Capacity == 0 || IsTutorialMap)
        {
            MapPlayerSpawn? spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(MapId);

            if (spawn is not null)
            {
                player.FieldPlayer.Coord = spawn.Coord.ToFloat();
                player.FieldPlayer.Rotation = spawn.Rotation.ToFloat();
            }
            else
            {
                player.FieldPlayer.Coord = player.SavedCoord;
                player.FieldPlayer.Rotation = player.SavedRotation;
            }
        }
        else
        {
            player.FieldPlayer.Coord = player.SavedCoord;
            player.FieldPlayer.Rotation = player.SavedRotation;
        }

        player.SafeBlock = player.SavedCoord;

        foreach (Character existingPlayer in State.Players.Values)
        {
            sender.Send(FieldPlayerPacket.AddPlayer(existingPlayer));
            sender.Send(FieldObjectPacket.LoadPlayer(existingPlayer));
        }

        State.AddPlayer(player.FieldPlayer);
        Increment();
        // Broadcast new player to all players in map
        Broadcast(session =>
        {
            session.Send(FieldPlayerPacket.AddPlayer(player.FieldPlayer));
            session.Send(FieldObjectPacket.LoadPlayer(player.FieldPlayer));
        });

        foreach (Pet pet in State.Pets.Values)
        {
            sender.Send(FieldPetPacket.AddPet(pet));
        }

        foreach (IFieldObject<Item> existingItem in State.Items.Values)
        {
            sender.Send(FieldItemPacket.AddItem(existingItem));
        }

        foreach (Npc existingNpc in State.Npcs.Values)
        {
            sender.Send(FieldNpcPacket.AddNpc(existingNpc));
            sender.Send(FieldObjectPacket.LoadNpc(existingNpc));
        }

        foreach (Npc existingMob in State.Mobs.Values)
        {
            sender.Send(FieldNpcPacket.AddNpc(existingMob));
            sender.Send(FieldObjectPacket.LoadNpc(existingMob));

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
                //CasterObjectId = healingSpot.ObjectId
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
        breakables.AddRange(State.BreakableActors.Values);
        breakables.AddRange(State.BreakableNifs.Values);
        sender.Send(BreakablePacket.LoadBreakables(breakables));

        sender.Send(InteractObjectPacket.LoadObjects(State.InteractObjects.Values.Where(t => t.Value is not AdBalloon and not MapChest).ToList()));

        foreach (IFieldObject<InteractObject> mapObject in State.InteractObjects.Values.Where(t => t.Value is MapChest or AdBalloon))
        {
            sender.Send(InteractObjectPacket.Add(mapObject));
        }

        List<TriggerObject> triggerObjects = new();
        triggerObjects.AddRange(State.TriggerMeshes.Values);
        triggerObjects.AddRange(State.TriggerEffects.Values);
        triggerObjects.AddRange(State.TriggerCameras.Values);
        triggerObjects.AddRange(State.TriggerActors.Values);
        triggerObjects.AddRange(State.TriggerCubes.Values);
        triggerObjects.AddRange(State.TriggerLadders.Values);
        triggerObjects.AddRange(State.TriggerRopes.Values);
        triggerObjects.AddRange(State.TriggerSounds.Values);
        sender.Send(TriggerPacket.LoadTriggers(triggerObjects));

        MapLoopTask ??= StartMapLoop();
        NpcMovementTask ??= StartNpcLoop();
        TriggerTask ??= StartTriggerTask();

        UGCBannerTimer ??= TaskScheduler.Instance.ScheduleTask(0, 0, 60, () => { GameServer.UGCBannerManager.UGCBannerLoop(this); });

        player.Inventory.RecomputeSetBonuses(player.Session);
    }

    public void RemovePlayer(Player player)
    {
        if (player.FieldPlayer is null)
        {
            return;
        }

        if (!State.RemovePlayer(player.FieldPlayer.ObjectId))
        {
            return;
        }

        player.Triggers.Clear();
        if (player.Guide is not null)
        {
            RemoveGuide(player.Guide);
        }

        if (player.FieldPlayer.ActivePet is not null)
        {
            RemovePet(player.FieldPlayer.ActivePet);
        }

        if (Decrement() <= 0)
        {
            FreezeField(player);
            return;
        }

        // Remove player
        Broadcast(session =>
        {
            session.Send(FieldPlayerPacket.RemovePlayer(player.FieldPlayer));
            session.Send(FieldObjectPacket.RemovePlayer(player.FieldPlayer));
        });
    }

    public static bool IsActorInBox(MapTriggerBox box, IFieldObject actor)
    {
        CoordF minCoord = CoordF.From(
            box.Position.X - box.Dimension.X / 2,
            box.Position.Y - box.Dimension.Y / 2,
            box.Position.Z - box.Dimension.Z / 2);

        CoordF maxCoord = CoordF.From(
            box.Position.X + box.Dimension.X / 2,
            box.Position.Y + box.Dimension.Y / 2,
            box.Position.Z + box.Dimension.Z / 2);

        bool min = actor.Coord.X >= minCoord.X && actor.Coord.Y >= minCoord.Y && actor.Coord.Z >= minCoord.Z;
        bool max = actor.Coord.X <= maxCoord.X && actor.Coord.Y <= maxCoord.Y && actor.Coord.Z <= maxCoord.Z;

        return min && max;
    }

    // Spawned NPCs will not appear until controlled
    private void AddNpc(Npc fieldNpc)
    {
        if (fieldNpc.Value.Type is NpcType.Friendly or NpcType.Ally)
        {
            State.AddNpc(fieldNpc);
        }
        else
        {
            State.AddMob(fieldNpc);
        }

        Broadcast(session =>
        {
            session.Send(FieldNpcPacket.AddNpc(fieldNpc));
            session.Send(FieldObjectPacket.LoadNpc(fieldNpc));

            // TODO: Find a better place to do this when buffs are implemented
            for (int i = 0; i < fieldNpc.Value.NpcMetadataEffect.EffectIds.Length; i++)
            {
                SkillCast effectCast = new(fieldNpc.Value.NpcMetadataEffect.EffectIds[i], fieldNpc.Value.NpcMetadataEffect.EffectLevels[i]);
                session.Send(BuffPacket.AddBuff(new(effectCast, fieldNpc.ObjectId, fieldNpc.ObjectId, 1)));
            }
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
        return true;
    }

    private void AddPet(Pet pet)
    {
        State.AddPet(pet);

        Broadcast(session =>
        {
            session.Send(FieldPetPacket.AddPet(pet));
            session.Send(PetPacket.Add(pet));
        });
    }

    public void RemovePet(Pet pet)
    {
        State.RemovePet(pet.ObjectId);

        Broadcast(session =>
        {
            session.Send(PetPacket.Remove(pet));
            session.Send(FieldPetPacket.RemovePet(pet));
        });
    }

    public bool RemoveMob(Npc mob)
    {
        if (!State.RemoveMob(mob.ObjectId))
        {
            return false;
        }

        IFieldObject<MobSpawn>? originSpawn = mob.OriginSpawn;
        if (originSpawn is not null && originSpawn.Value.Mobs.Remove(mob) && originSpawn.Value.Mobs.Count == 0)
        {
            StartSpawnTimer(originSpawn);
        }

        BroadcastPacket(FieldObjectPacket.ControlNpc(mob));
        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromSeconds(mob.Value.NpcMetadataDead.Time));

            BroadcastPacket(FieldNpcPacket.RemoveNpc(mob));
            BroadcastPacket(FieldObjectPacket.RemoveNpc(mob));
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
        BroadcastPacket(CubePacket.PlaceFurnishing(cube, houseOwnerObjectId, fieldPlayerObjectId, false));

        if (cube.Value.Item.HousingCategory is ItemHousingCategory.Ranching or ItemHousingCategory.Farming)
        {
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 1, 0));
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 2, 1));
        }

        if (cube.Value.Item.Id == 50400158) // portal cube
        {
            BroadcastPacket(FunctionCubePacket.UpdateFunctionCube(cube.Coord.ToByte(), 0, 0));
        }
    }

    public void RemoveCube(IFieldObject<Cube> cube, int houseOwnerObjectId, int fieldPlayerObjectId)
    {
        if (cube.Value.Item.Id == 50400158) // portal cube
        {
            if (State.Portals.TryGetValue(cube.Value.PortalSettings.PortalObjectId, out IFieldObject<Portal>? fieldPortal))
            {
                RemovePortal(fieldPortal);
            }
        }

        State.RemoveCube(cube.ObjectId);
        BroadcastPacket(CubePacket.RemoveCube(houseOwnerObjectId, fieldPlayerObjectId, cube.Coord.ToByte()));
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

    public void AddItem(Character character, Item item, IFieldActor? source = null)
    {
        item.DropInformation = new()
        {
            SourceObjectId = source?.ObjectId ?? 0,
            BoundToCharacterId = character.Value.CharacterId // TODO: Find when item should be bound to character
        };

        FieldObject<Item> fieldItem = WrapObject(item);
        fieldItem.Coord = source?.Coord ?? character.Coord;

        CancellationToken cancellationToken = item.DropInformation.CancellationToken.Token;

        Task.Run(async () =>
        {
            await Task.Delay(TimeSpan.FromMinutes(3), cancellationToken);

            RemoveItem(fieldItem.ObjectId);
        }, cancellationToken);

        State.AddItem(fieldItem);

        // TODO: If item is bound to character, character.Value.Session.Send(FieldItemPacket.AddItem(fieldItem));
        BroadcastPacket(FieldItemPacket.AddItem(fieldItem));
    }

    private bool RemoveItem(int objectId)
    {
        if (!State.RemoveItem(objectId, out IFieldObject<Item> fieldItem))
        {
            return false;
        }

        BroadcastPacket(FieldItemPacket.RemoveItem(fieldItem.ObjectId));
        fieldItem.Value.DropInformation.CancellationToken.Cancel();

        return true;
    }

    public bool PickupItem(int objectId, int receiverObjectId, out IFieldObject<Item> fieldItem)
    {
        if (!State.RemoveItem(objectId, out fieldItem))
        {
            return false;
        }

        DropInformation dropInformation = fieldItem.Value.DropInformation;

        BroadcastPacket(FieldItemPacket.PickupItem(fieldItem, receiverObjectId));
        BroadcastPacket(FieldItemPacket.RemoveItem(fieldItem.ObjectId));

        dropInformation.CancellationToken.Cancel();

        return true;
    }

    public void AddMapTimer(MapTimer timer)
    {
        MapTimer existingTimer = MapTimers.FirstOrDefault(x => x.Id == timer.Id) ?? timer;

        MapTimers.Add(existingTimer);
    }

    public MapTimer? GetMapTimer(string id)
    {
        return MapTimers.FirstOrDefault(x => x.Id == id);
    }

    public void AddWidget(Widget widget)
    {
        Widgets.Add(widget);
    }

    public Widget? GetWidget(WidgetType type)
    {
        return Widgets.FirstOrDefault(x => x.Type == type);
    }

    public void AddSkillCast(SkillCast skillCast) => State.AddSkillCast(skillCast);

    public bool RemoveSkillCast(long skillSn, out SkillCast skillCast) => State.RemoveSkillCast(skillSn, out skillCast);

    public IFieldObject<InteractObject>? AddAdBalloon(AdBalloon adBalloon)
    {
        IFieldObject<InteractObject> fieldAdBalloon = WrapObject(adBalloon);
        if (adBalloon.Owner.FieldPlayer == null)
        {
            return null;
        }

        fieldAdBalloon.Coord = adBalloon.Owner.FieldPlayer.Coord;
        fieldAdBalloon.Rotation = adBalloon.Owner.FieldPlayer.Rotation;

        State.AddInteractObject(fieldAdBalloon);
        return fieldAdBalloon;
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
    public void BroadcastPacket(PacketWriter packet, GameSession? sender = null)
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
        foreach (Character player in State.Players.Values)
        {
            action?.Invoke(player.Value.Session);
        }
    }

    #endregion

    #region Wrap Methods

    // Initializes a FieldObject with an objectId for this field.
    private FieldObject<T> WrapObject<T>(T fieldObject)
    {
        return new(NextLocalId(), fieldObject);
    }

    // Initializes a FieldActor with an objectId for this field.
    private Character WrapPlayer(Player player)
    {
        return new(NextGlobalId(), player, fieldManager: this);
    }

    private Npc WrapNpc(NpcMetadata metadata)
    {
        return new(NextLocalId(), metadata, fieldManager: this);
    }

    // Initializes a Mob with an objectId for this field.
    private Npc WrapMob(int mobId)
    {
        return new(NextLocalId(), mobId, fieldManager: this);
    }

    /// <summary>
    /// Generates an Object ID unique across all field instances.
    /// </summary>
    /// <returns>Returns a globally unique object id</returns>
    public static int NextGlobalId() => Interlocked.Increment(ref GlobalIdCounter);

    /// <summary>
    /// Generates an Object ID unique within this field instance.
    /// </summary>
    /// <returns>Returns a unique object id</returns>
    private int NextLocalId() => Interlocked.Increment(ref LocalIdCounter);

    #endregion

    public void SendChat(Player player, string message, ChatType type)
    {
        Broadcast(session => session.Send(ChatPacket.Send(player, message, type)));
    }

    public void MovePlayerAlongPath(IFieldActor<Player> player, PatrolData patrolData)
    {
        int dummyNpcId = player.Value.Gender is Gender.Male ? 2040998 : 2040999; // dummy npc must match player gender

        Npc? dummyNpc = RequestNpc(dummyNpcId, player.Coord, player.Rotation);
        if (dummyNpc is null)
        {
            return;
        }

        dummyNpc.SetPatrolData(patrolData);

        player.Value.Session?.Send(FollowNpcPacket.FollowNpc(dummyNpc.ObjectId));

        Task.Run(async () =>
        {
            while (true)
            {
                await Task.Delay(1000);

                CoordF coord = patrolData.WayPoints.Last().Position.ToFloat();
                float distance = CoordF.Distance(dummyNpc.Coord, coord);
                if (distance > 0.2)
                {
                    continue;
                }

                RemoveNpc(dummyNpc);
                break;
            }
        });
    }

    public void AddRegionSkillEffect(SkillCast skillCast)
    {
        skillCast.SkillObjectId = NextLocalId();

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
        Home? home = GameServer.HomeManager.GetHomeById(player.VisitingHomeId);
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

    private void FreezeField(Player player)
    {
        UGCBannerTimer?.Dispose();
        UGCBannerTimer = null;
        MapLoopTask = null;
        TriggerTask = null;
        NpcMovementTask = null;

        if (Capacity == 0 || IsTutorialMap)
        {
            foreach (IFieldObject<Item> item in State.Items.Values)
            {
                // Cancel all item fadeout tasks so field can be released.
                item.Value.DropInformation.CancellationToken.Cancel();
            }

            FieldManagerFactory.ReleaseManager(this);
        }

        // --- Dungeon Session ---
        // Is only called if the leaving player is the last player on the map
        // Get the DungeonSession that corresponds with the about to be released instance, in case that the player is in a party (group session) and solo session
        if (GameServer.DungeonManager.IsDungeonUsingFieldInstance(this, player))
        {
            return;
        }

        DungeonSession? dungeonSession = GameServer.DungeonManager.GetDungeonSessionBySessionId(player.DungeonSessionId);

        //If instance is destroyed, reset dungeonSession
        //further conditions for dungeon completion could be checked here.
        if (dungeonSession is null || !dungeonSession.IsDungeonSessionMap(MapId))
        {
            return;
        }

        GameServer.DungeonManager.ResetDungeonSession(player, dungeonSession);
    }

    #region Map loop

    private Task StartMapLoop()
    {
        return Task.Run(async () =>
        {
            while (PlayerCount > 0)
            {
                UpdatePetEvents();
                UpdateObjects();
                HealingSpot();
                SendUpdates();

                await Task.Delay(1000);
            }
        });
    }

    private Task StartNpcLoop()
    {
        return Task.Run(async () =>
        {
            while (PlayerCount > 0)
            {
                try
                {
                    foreach (Npc mob in State.Mobs.Values)
                    {
                        if (mob.IsDead)
                        {
                            RemoveMob(mob);
                            continue;
                        }

                        mob.UpdateVelocity();
                        BroadcastPacket(FieldObjectPacket.ControlNpc(mob)); // TODO: Optimize this to only send packets when needed
                        mob.UpdateCoord();
                    }

                    foreach (Npc npc in State.Npcs.Values)
                    {
                        npc.UpdateVelocity();
                        BroadcastPacket(FieldObjectPacket.ControlNpc(npc)); // TODO: Optimize this to only send packets when needed
                        npc.UpdateCoord();
                    }

                    foreach (Pet pet in State.Pets.Values)
                    {
                        pet.UpdateVelocity();
                        BroadcastPacket(FieldObjectPacket.ControlNpc(pet)); // TODO: Optimize this to only send packets when needed
                        pet.UpdateCoord();
                    }
                }
                catch (Exception e)
                {
                    Logger.Error("Error in mob loop {0}", e);
                    throw;
                }

                // Npc update can be theoretically any delay, just make sure to not eat all the CPU. Default is 300ms
                await Task.Delay(300);
            }
        });
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
        // Update players state
        foreach (Character player in State.Players.Values)
        {
            updates.Add(FieldObjectPacket.UpdatePlayer(player));
        }

        return updates;
    }

    private void UpdatePetEvents()
    {
        // TODO: loop trough all mobs and check if pet should attack mob
        // Manage pet aggro + targets
        foreach (Pet pet in State.Pets.Values)
        {
            float playerPetDistance = CoordF.Distance(pet.Owner.Coord, pet.Coord);
            // TODO: NpcMetadataDistance.Sight is incorrect, parse and use petproperty.xml
            if (playerPetDistance > pet.Value.NpcMetadataDistance.Sight)
            {
                // Teleport pet to player if they are too far away
                pet.Coord = pet.Owner.Coord;
                continue;
            }

            if (playerPetDistance > Block.BLOCK_SIZE * 2)
            {
                pet.State = NpcState.Combat; // Setting state as combat so pet will run towards player, probably not the best way to do this.
                pet.Target = pet.Owner;
                continue;
            }

            pet.State = NpcState.Normal;
            pet.Target = null;
        }
    }

    private void UpdateObjects()
    {
        foreach (Npc mob in State.Mobs.Values)
        {
            mob.Behavior.Next();
        }

        foreach (Pet pet in State.Pets.Values)
        {
            pet.Act();
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

                player.AdditionalEffects.AddEffect(new(70000018, 1)); // applies a healing effect to the player
            }
        }
    }

    private Task StartTriggerTask()
    {
        return Task.Run(async () =>
        {
            while (PlayerCount > 0)
            {
                foreach (TriggerScript trigger in Triggers)
                {
                    try
                    {
                        trigger.Next();
                    }
                    catch (Exception e)
                    {
                        Logger.Error(e.ToString());
                        // Disconnect everyone in the field if a trigger has an exception
                        foreach (IFieldActor<Player> fieldPlayers in State.Players.Values)
                        {
                            fieldPlayers.Value.Session?.Disconnect(logoutNotice: true);
                        }
                    }
                }

                await Task.Delay(100);
            }
        });
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
}
