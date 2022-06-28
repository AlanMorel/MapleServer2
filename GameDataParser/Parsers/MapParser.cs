using System.Xml;
using GameDataParser.Files;
using GameDataParser.Tools;
using Maple2.File.Flat;
using Maple2.File.Flat.maplestory2library;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Flat;
using Maple2.File.Parser.MapXBlock;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Map;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MapParser : Exporter<List<MapMetadata>>
{
    private List<MapMetadata> MapMetadatas;
    private Dictionary<int, Dictionary<int, SpawnMetadata>> SpawnTagMap;
    private Dictionary<int, InteractObjectType> InteractTypes;
    public MapParser(MetadataResources resources) : base(resources, MetadataName.Map) { }

    protected override List<MapMetadata> Parse()
    {
        MapMetadatas = new();

        ParseInteractObjectTable();
        ParseMapSpawnTagTable();
        ParseMapMetadata();

        // Parse every block for each map
        FlatTypeIndex index = new(Resources.ExportedReader);
        XBlockParser parser = new(Resources.ExportedReader, index);

        parser.Parse(BuildMetadata);
        // Since parsing is done in parallel, sort at the end for deterministic order.
        MapMetadatas.Sort((metadata1, metadata2) => metadata1.Id.CompareTo(metadata2.Id));
        return MapMetadatas;
    }

    private void BuildMetadata(string xblock, IEnumerable<IMapEntity> mapEntities)
    {
        if (xblock.EndsWith("_cn") || xblock.EndsWith("_jp") || xblock.EndsWith("_kr"))
        {
            return;
        }

        MapMetadata mapMetadata = MapMetadatas.FirstOrDefault(x => x.XBlockName == xblock);
        if (mapMetadata is null)
        {
            return;
        }

        MapEntityMetadata mapEntity = mapMetadata.Entities;

        List<IMS2WayPoint> tempWaypoints = new();
        List<IMS2PatrolData> tempPatrolData = new();
        foreach (IMapEntity entity in mapEntities)
        {
            if (entity is IMS2CubeProp cube)
            {
                MapBlock mapBlock = new()
                {
                    Coord = CoordS.FromVector3(cube.Position),
                    Type = cube.CubeType,
                    SaleableGroup = cube.CubeSalableGroup,
                    Attribute = cube.MapAttribute
                };

                mapMetadata.Blocks.TryAdd(mapBlock.Coord, mapBlock);
            }

            switch (entity)
            {
                case IMS2Bounding bounding:
                    if (bounding.EntityName.EndsWith("0") && mapEntity.BoundingBox0.Equals(CoordS.From(0, 0, 0)))
                    {
                        mapEntity.BoundingBox0 = CoordS.FromVector3(bounding.Position);
                    }
                    else if (bounding.EntityName.EndsWith("1") && mapEntity.BoundingBox1.Equals(CoordS.From(0, 0, 0)))
                    {
                        mapEntity.BoundingBox1 = CoordS.FromVector3(bounding.Position);
                    }

                    break;
                case IMS2PatrolData patrolData:
                    tempPatrolData.Add(patrolData);
                    break;
                case IMS2WayPoint wayPoint:
                    tempWaypoints.Add(wayPoint);
                    break;
                // TODO: This can probably be more generally handled as IMS2RegionSkill
                case IMS2HealingRegionSkillSound healingRegion:
                    if (healingRegion.Position == default)
                    {
                        continue;
                    }

                    mapEntity.HealingSpot.Add(CoordS.FromVector3(healingRegion.Position));
                    break;
                case IMS2InteractObject interact:
                    InteractObjectType type = GetInteractObjectType(interact.interactID);
                    if (type == InteractObjectType.None)
                    {
                        continue;
                    }

                    switch (interact)
                    {
                        case IMS2SimpleUiObject uiObject:
                            mapEntity.InteractObjects.Add(new(uiObject.EntityId, uiObject.interactID, uiObject.Enabled, type,
                                CoordF.FromVector3(uiObject.Position),
                                CoordF.FromVector3(uiObject.Rotation)));
                            break;
                        case IMS2InteractMesh interactMesh:
                            mapEntity.InteractObjects.Add(new(interactMesh.EntityId, interactMesh.interactID, interactMesh.IsVisible, type,
                                CoordF.FromVector3(interactMesh.Position),
                                CoordF.FromVector3(interactMesh.Rotation)));
                            break;
                        case IMS2Telescope telescope:
                            mapEntity.InteractObjects.Add(new(telescope.EntityId, telescope.interactID, telescope.Enabled, type,
                                CoordF.FromVector3(telescope.Position),
                                CoordF.FromVector3(telescope.Rotation)));
                            break;
                        case IMS2InteractActor interactActor:
                            mapEntity.InteractObjects.Add(new(interactActor.EntityId, interactActor.interactID, interactActor.IsVisible, type
                                , CoordF.FromVector3(interactActor.Position),
                                CoordF.FromVector3(interactActor.Rotation)));
                            break;
                        case IMS2InteractDisplay interactDisplay:
                            mapEntity.InteractObjects.Add(new(interactDisplay.EntityId, interactDisplay.interactID, interactDisplay.IsVisible, type,
                                CoordF.FromVector3(interactDisplay.Position),
                                CoordF.FromVector3(interactDisplay.Rotation)));
                            break;
                    }

                    break;
                case ISpawnPoint spawn:
                    switch (spawn)
                    {
                        // TODO: Parse "value" from NPCList.
                        case IEventSpawnPointNPC eventSpawnNpc: // trigger mob/npc spawns
                            List<string> npcIds = new();
                            npcIds.AddRange(eventSpawnNpc.NpcList.Keys);

                            mapEntity.EventNpcSpawnPoints.Add(new(eventSpawnNpc.SpawnPointID, eventSpawnNpc.NpcCount, npcIds, eventSpawnNpc.SpawnAnimation,
                                eventSpawnNpc.SpawnRadius,
                                CoordF.FromVector3(eventSpawnNpc.Position), CoordF.FromVector3(eventSpawnNpc.Rotation)));
                            break;
                        case ISpawnPointPC pcSpawn:
                            mapEntity.PlayerSpawns.Add(new(CoordS.FromVector3(pcSpawn.Position), CoordS.FromVector3(pcSpawn.Rotation)));
                            break;
                        case ISpawnPointNPC npcSpawn:
                            // These tend to be vendors, shops, etc.
                            // If the name tag begins with SpawnPointNPC, I think these are mob spawn locations. Skipping these.
                            string npcIdStr = npcSpawn.NpcList.FirstOrDefault().Key ?? "0";
                            if (npcIdStr == "0" || !int.TryParse(npcIdStr, out int npcId))
                            {
                                continue;
                            }

                            MapNpc npc = new(npcId, npcSpawn.ModelName, npcSpawn.EntityName, CoordS.FromVector3(npcSpawn.Position),
                                CoordS.FromVector3(npcSpawn.Rotation), npcSpawn.IsSpawnOnFieldCreate, npcSpawn.dayDie, npcSpawn.nightDie, npcSpawn.PatrolData);
                            // Parse some additional flat supplemented data about this NPC.

                            mapEntity.Npcs.Add(npc);
                            break;
                    }

                    break;
                case IMS2RegionSpawnBase spawnBase:
                    SpawnMetadata spawnData = SpawnTagMap.ContainsKey(mapMetadata.Id) && SpawnTagMap[mapMetadata.Id].ContainsKey(spawnBase.SpawnPointID)
                        ? SpawnTagMap[mapMetadata.Id][spawnBase.SpawnPointID]
                        : null;
                    switch (spawnBase)
                    {
                        case IMS2RegionBoxSpawn boxSpawn:
                            if (boxSpawn.ModelName.Contains("chest", StringComparison.CurrentCultureIgnoreCase))
                            {
                                mapEntity.MapChests.Add(new()
                                {
                                    IsGolden = boxSpawn.ModelName.Contains("rare", StringComparison.CurrentCultureIgnoreCase),
                                    Position = CoordF.FromVector3(boxSpawn.Position),
                                    Rotation = CoordF.FromVector3(boxSpawn.Rotation)
                                });
                                continue;
                            }

                            AddMobSpawn(spawnData, mapEntity, boxSpawn);
                            break;
                        case IMS2RegionSpawn regionSpawn:
                            int mobNpcCount = spawnData?.Population ?? 6;

                            // TODO: This previously relied in "NpcList" to be set. NpcList is impossible to be set on
                            // MS2RegionSpawn, it's only set for SpawnPointNPC.
                            List<int> mobNpcList = new()
                            {
                                21000025 // Placeholder
                            };
                            mapEntity.MobSpawns.Add(new(regionSpawn.SpawnPointID, CoordS.FromVector3(regionSpawn.Position),
                                mobNpcCount, mobNpcList, (int) regionSpawn.SpawnRadius, spawnData));
                            break;
                    }

                    break;
                case IPortal portal:
                    mapEntity.Portals.Add(new(portal.PortalID, portal.ModelName, portal.PortalEnable, portal.IsVisible, portal.MinimapIconVisible,
                        portal.TargetFieldSN,
                        CoordS.FromVector3(portal.Position), CoordS.FromVector3(portal.Rotation), portal.TargetPortalID, (PortalTypes) portal.PortalType));
                    break;
                case IMS2Breakable breakable:
                    switch (breakable)
                    {
                        case IMS2BreakableActor actor:
                            mapEntity.BreakableActors.Add(new(actor.EntityId, actor.Enabled, actor.hideTimer, actor.resetTimer));
                            break;
                        case IMS2BreakableNIF nif:
                            mapEntity.BreakableNifs.Add(new(nif.EntityId, nif.Enabled, (int) nif.TriggerBreakableID, nif.hideTimer, nif.resetTimer));
                            break;
                    }

                    break;
                case IMS2TriggerObject triggerObject:
                    switch (triggerObject)
                    {
                        case IMS2TriggerMesh triggerMesh:
                            mapEntity.TriggerMeshes.Add(new(triggerMesh.TriggerObjectID, triggerMesh.IsVisible));
                            break;
                        case IMS2TriggerEffect triggerEffect:
                            mapEntity.TriggerEffects.Add(new(triggerEffect.TriggerObjectID, triggerEffect.IsVisible));
                            break;
                        case IMS2TriggerCamera triggerCamera:
                            mapEntity.TriggerCameras.Add(new(triggerCamera.TriggerObjectID, triggerCamera.Enabled));
                            break;
                        case IMS2TriggerBox triggerBox:
                            mapEntity.TriggerBoxes.Add(new(triggerBox.TriggerObjectID, CoordF.FromVector3(triggerBox.Position),
                                CoordF.FromVector3(triggerBox.ShapeDimensions)));
                            break;
                        case IMS2TriggerLadder triggerLadder: // TODO: Find which parameters correspond to animationeffect (bool) and animation delay (int?)
                            mapEntity.TriggerLadders.Add(new(triggerLadder.TriggerObjectID, triggerLadder.IsVisible));
                            break;
                        case IMS2TriggerRope triggerRope: // TODO: Find which parameters correspond to animationeffect (bool) and animation delay (int?)
                            mapEntity.TriggerRopes.Add(new(triggerRope.TriggerObjectID, triggerRope.IsVisible));
                            break;
                        case IMS2TriggerPortal triggerPortal:
                            mapEntity.Portals.Add(new(triggerPortal.PortalID, triggerPortal.ModelName, triggerPortal.PortalEnable, triggerPortal.IsVisible,
                                triggerPortal.MinimapIconVisible,
                                triggerPortal.TargetFieldSN, CoordS.FromVector3(triggerPortal.Position), CoordS.FromVector3(triggerPortal.Rotation),
                                triggerPortal.TargetPortalID, (PortalTypes) triggerPortal.PortalType, triggerPortal.TriggerObjectID));
                            break;
                        case IMS2TriggerActor triggerActor:
                            mapEntity.TriggerActors.Add(new(triggerActor.TriggerObjectID, triggerActor.IsVisible, triggerActor.InitialSequence));
                            break;
                        case IMS2TriggerCube triggerCube:
                            mapEntity.TriggerCubes.Add(new(triggerCube.TriggerObjectID, triggerCube.IsVisible));
                            break;
                        case IMS2TriggerSound triggerSound:
                            mapEntity.TriggerSounds.Add(new(triggerSound.TriggerObjectID, triggerSound.Enabled));
                            break;
                        case IMS2TriggerSkill triggerSkill:
                            mapEntity.TriggerSkills.Add(new(triggerSkill.TriggerObjectID, triggerSkill.skillID,
                                (short) triggerSkill.skillLevel, (byte) triggerSkill.count, CoordF.FromVector3(triggerSkill.Position)));
                            break;
                    }

                    break;
                case IMS2LiftableTargetBox liftableTargetBox:
                    mapEntity.LiftableTargets.Add(new(liftableTargetBox.liftableTarget, CoordF.FromVector3(liftableTargetBox.Position),
                        CoordF.FromVector3(liftableTargetBox.ShapeDimensions)));
                    break;
                case IMS2PhysXProp physXProp:
                    switch (physXProp)
                    {
                        case IMS2CubeProp prop:
                            if (!prop.IsObjectWeapon)
                            {
                                break;
                            }

                            List<int> weaponIds = prop.ObjectWeaponItemCode.SplitAndParseToInt(',').ToList();
                            mapEntity.WeaponObjects.Add(new(CoordB.FromVector3(prop.Position), weaponIds));
                            break;

                        case IMS2Liftable liftable:
                            mapEntity.LiftableObjects.Add(new(liftable.EntityId, (int) liftable.ItemID, liftable.ItemStackCount, liftable.MaskQuestID,
                                liftable.MaskQuestState, liftable.EffectQuestID, liftable.EffectQuestState, liftable.ItemLifeTime,
                                liftable.LiftableRegenCheckTime, liftable.LiftableFinishTime, CoordF.FromVector3(liftable.Position),
                                CoordF.FromVector3(liftable.Rotation)));
                            break;
                        case IMS2Vibrate vibrate:
                            mapEntity.VibrateObjects.Add(new(vibrate.EntityId, CoordF.FromVector3(physXProp.Position)));
                            break;
                    }

                    break;
            }
        }

        BuildWaypoints(tempPatrolData, tempWaypoints, mapEntity);
    }

    private static void BuildWaypoints(List<IMS2PatrolData> tempPatrolData, List<IMS2WayPoint> tempWaypoints, MapEntityMetadata mapEntity)
    {
        foreach (IMS2PatrolData patrolData in tempPatrolData)
        {
            List<string> wayPointIds = patrolData.WayPoints.Select(entry => entry.Value.Replace("-", string.Empty)).ToList();

            List<string> approachAnimations = patrolData.ApproachAnims.Select(entry => entry.Value).ToList();

            List<string> arriveAnimations = patrolData.ArriveAnims.Select(entry => entry.Value).ToList();

            List<int> arriveAnimationTimes = patrolData.ArriveAnimsTime.Select(entry => (int) entry.Value).ToList();

            List<WayPoint> wayPoints = new();
            for (int i = 0; i < wayPointIds.Count; i++)
            {
                string wayPointId = wayPointIds.ElementAtOrDefault(i);
                string approachAnimation = approachAnimations.ElementAtOrDefault(i);
                string arriveAnimation = arriveAnimations.ElementAtOrDefault(i);
                int arriveAnimationTime = arriveAnimationTimes.ElementAtOrDefault(i);

                IMS2WayPoint waypoint = tempWaypoints.FirstOrDefault(x => x.EntityId == wayPointId);
                if (waypoint is null)
                {
                    continue;
                }

                wayPoints.Add(new(wayPointId, waypoint.IsVisible,
                    CoordS.FromVector3(waypoint.Position),
                    CoordS.FromVector3(waypoint.Rotation),
                    approachAnimation,
                    arriveAnimation,
                    arriveAnimationTime));
            }

            mapEntity.PatrolDatas.Add(new(patrolData.EntityId,
                patrolData.EntityName,
                patrolData.IsAirWayPoint,
                (int) patrolData.PatrolSpeed,
                patrolData.IsLoop,
                wayPoints));
        }
    }

    private static void AddMobSpawn(SpawnMetadata mobSpawnDataBox, MapEntityMetadata metadata, IMS2RegionBoxSpawn boxSpawn)
    {
        int mobNpcCountBox = mobSpawnDataBox?.Population ?? 6;
        int mobSpawnRadiusBox = 150;
        // TODO: This previously relied in "NpcList" to be set. NpcList is impossible to be set on
        // MS2RegionSpawn, it's only set for SpawnPointNPC.
        List<int> mobNpcListBox = new()
        {
            21000025 // Placeholder
        };
        metadata.MobSpawns.Add(new(boxSpawn.SpawnPointID, CoordS.FromVector3(boxSpawn.Position),
            mobNpcCountBox, mobNpcListBox, mobSpawnRadiusBox, mobSpawnDataBox));
        // "QR_10000264_" is Quest Reward Chest? This is tied to a MS2TriggerAgent making this object appear.
    }

    private InteractObjectType GetInteractObjectType(int interactId) =>
        InteractTypes.ContainsKey(interactId) ? InteractTypes[interactId] : InteractObjectType.None;

    private void ParseMapSpawnTagTable()
    {
        SpawnTagMap = new();

        PackFileEntry mapSpawnTag = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/mapspawntag"));
        XmlDocument mapSpawnTagXml = Resources.XmlReader.GetXmlDocument(mapSpawnTag);
        XmlNodeList regionNodes = mapSpawnTagXml.SelectNodes("/ms2/region");

        foreach (XmlNode node in regionNodes)
        {
            int mapId = int.Parse(node.Attributes["mapCode"].Value);
            int spawnPointId = int.Parse(node.Attributes["spawnPointID"].Value);

            int difficulty = int.Parse(node.Attributes["difficulty"].Value);
            int minDifficulty = int.Parse(node.Attributes["difficultyMin"].Value);
            string[] spawnTags = node.Attributes["tag"].Value.Split(",").Select(p => p.Trim()).ToArray();
            if (!int.TryParse(node.Attributes["coolTime"].Value, out int spawnTime))
            {
                spawnTime = 0;
            }

            if (!int.TryParse(node.Attributes["population"].Value, out int population))
            {
                population = 0;
            }

            _ = int.TryParse(node.Attributes["petPopulation"]?.Value, out int petPopulation);
            bool isPetSpawn = petPopulation > 0;

            SpawnMetadata spawnData = new(spawnTags, population, spawnTime, difficulty, minDifficulty, isPetSpawn);
            if (!SpawnTagMap.ContainsKey(mapId))
            {
                SpawnTagMap[mapId] = new();
            }

            SpawnTagMap[mapId][spawnPointId] = spawnData;
        }
    }

    private void ParseInteractObjectTable()
    {
        InteractTypes = new();

        IEnumerable<PackFileEntry> interactObjectTables = Resources.XmlReader.Files.Where(x => x.Name.StartsWith("table/interactobject"));
        foreach (PackFileEntry interactObjectTable in interactObjectTables)
        {
            XmlDocument interactObjectTableXml = Resources.XmlReader.GetXmlDocument(interactObjectTable);
            XmlNodeList interactNodes = interactObjectTableXml.GetElementsByTagName("interact");
            foreach (XmlNode node in interactNodes)
            {
                string locale = node.Attributes["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                int interactId = int.Parse(node.Attributes["id"].Value);
                _ = Enum.TryParse(node.Attributes["type"].Value, out InteractObjectType objectType);

                InteractTypes[interactId] = objectType;
            }
        }
    }

    private void ParseMapMetadata()
    {
        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.MapParser parser = new(Resources.XmlReader);
        foreach ((int id, string name, MapData data) in parser.Parse())
        {
            MapMetadata mapMetadata = new()
            {
                Id = id,
                XBlockName = data.xblock.name.ToLower(),
                Name = name,
                Property = new()
                {
                    RevivalReturnMapId = data.property.revivalreturnid,
                    EnterReturnMapId = data.property.enterreturnid,
                    Capacity = data.property.capacity,
                    IsTutorialMap = data.property.tutorialType == 1
                }
            };

            MapMetadatas.Add(mapMetadata);
        }
    }
}
