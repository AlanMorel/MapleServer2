using System.Xml;
using GameDataParser.Files;
using Maple2.File.Flat;
using Maple2.File.Flat.maplestory2library;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Flat;
using Maple2.File.Parser.MapXBlock;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MapParser : Exporter<List<MapMetadata>>
{
    private List<MapMetadata> MapMetadatas;
    private Dictionary<int, string> MapNames;
    private Dictionary<int, Dictionary<int, SpawnMetadata>> SpawnTagMap;
    private Dictionary<int, InteractObjectType> InteractTypes;
    public MapParser(MetadataResources resources) : base(resources, "map") { }

    protected override List<MapMetadata> Parse()
    {
        MapMetadatas = new();

        ParseMapNames();
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
                    string patrolDataName = patrolData.EntityName.Replace("-", string.Empty);
                    List<string> wayPointIds = patrolData.WayPoints.Select(entry => entry.Value.Replace("-", string.Empty)).ToList();

                    List<string> arriveAnimations = patrolData.ArriveAnims.Select(entry => entry.Value).ToList();

                    List<string> approachAnimations = patrolData.ApproachAnims.Select(entry => entry.Value).ToList();

                    List<int> arriveAnimationTimes = patrolData.ArriveAnimsTime.Select(entry => (int) entry.Value).ToList();

                    mapEntity.PatrolDatas.Add(new(patrolDataName, wayPointIds, (int) patrolData.PatrolSpeed, patrolData.IsLoop, patrolData.IsAirWayPoint,
                        arriveAnimations, approachAnimations, arriveAnimationTimes));
                    break;
                case IMS2WayPoint wayPoint:
                    mapEntity.WayPoints.Add(new(wayPoint.EntityId, wayPoint.IsVisible, CoordS.FromVector3(wayPoint.Position),
                        CoordS.FromVector3(wayPoint.Rotation)));
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
                            mapEntity.InteractObjects.Add(new(uiObject.EntityId, uiObject.interactID, uiObject.Enabled, type));
                            break;
                        case IMS2InteractMesh interactMesh:
                            mapEntity.InteractObjects.Add(new(interactMesh.EntityId, interactMesh.interactID, interactMesh.IsVisible, type));
                            break;
                        case IMS2Telescope telescope:
                            mapEntity.InteractObjects.Add(new(telescope.EntityId, telescope.interactID, telescope.Enabled, type));
                            break;
                        case IMS2InteractActor interactActor:
                            mapEntity.InteractObjects.Add(new(interactActor.EntityId, interactActor.interactID, interactActor.IsVisible, type));
                            break;
                        case IMS2InteractDisplay interactDisplay:
                            mapEntity.InteractObjects.Add(new(interactDisplay.EntityId, interactDisplay.interactID, interactDisplay.IsVisible, type));
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
                            mapEntity.PlayerSpawns.Add(
                                new(CoordS.FromVector3(pcSpawn.Position), CoordS.FromVector3(pcSpawn.Rotation)));
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
                                CoordS.FromVector3(npcSpawn.Rotation), npcSpawn.IsSpawnOnFieldCreate, npcSpawn.dayDie, npcSpawn.nightDie);
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

                            List<int> weaponIds = prop.ObjectWeaponItemCode.Split(",").Select(int.Parse).ToList();
                            mapEntity.WeaponObjects.Add(new(CoordB.FromVector3(prop.Position), weaponIds));
                            break;
                        case IMS2Liftable liftable:
                            mapEntity.LiftableObjects.Add(new(liftable.EntityId, (int) liftable.ItemID, liftable.EffectQuestID, liftable.EffectQuestState,
                                liftable.ItemLifeTime, liftable.LiftableRegenCheckTime));
                            break;
                        case IMS2Vibrate vibrate:
                            mapEntity.VibrateObjects.Add(new(vibrate.EntityId, CoordF.FromVector3(physXProp.Position)));
                            break;
                    }

                    break;
            }
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

        PackFileEntry interactObjectTable = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("table/interactobject"));
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

    private void ParseMapNames()
    {
        MapNames = new();

        PackFileEntry mapNames = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/mapname.xml"));
        XmlDocument mapNamesXml = Resources.XmlReader.GetXmlDocument(mapNames);
        foreach (XmlNode node in mapNamesXml.DocumentElement.ChildNodes)
        {
            int id = int.Parse(node.Attributes["id"].Value);
            string name = node.Attributes["name"].Value;
            MapNames[id] = name;
        }
    }

    private void ParseMapMetadata()
    {
        foreach (PackFileEntry map in Resources.XmlReader.Files.Where(x => x.Name.StartsWith("map/")))
        {
            XmlDocument mapXml = Resources.XmlReader.GetXmlDocument(map);
            foreach (XmlNode node in mapXml.DocumentElement.ChildNodes)
            {
                if (node.Attributes["locale"].Value is "KR" or "CN" or "JP")
                {
                    continue;
                }

                // map.Name: map/00000001.xml
                int id = int.Parse(map.Name.Split('/')[1].Split('.')[0]);
                string xblock = node.SelectSingleNode("xblock").Attributes["name"].Value;

                XmlNode propertyNode = node.SelectSingleNode("property");
                MapProperty mapProperty = new()
                {
                    RevivalReturnMapId = int.Parse(propertyNode.Attributes["revivalreturnid"]?.Value ?? "0"),
                    EnterReturnMapId = propertyNode.Attributes["enterreturnid"]?.Value ?? "",
                    Capacity = short.Parse(propertyNode.Attributes["capacity"]?.Value ?? "0"),
                    IsTutorialMap = propertyNode.Attributes["tutorialType"]?.Value == "1"
                };

                MapMetadata mapMetadata = new()
                {
                    Id = id,
                    XBlockName = xblock.ToLower(),
                    Property = mapProperty
                };
                MapNames.TryGetValue(id, out mapMetadata.Name);

                MapMetadatas.Add(mapMetadata);
                break; // only use first environment
            }
        }
    }
}
