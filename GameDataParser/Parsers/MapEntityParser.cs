using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.Flat;
using Maple2.File.Flat.maplestory2library;
using Maple2.File.Flat.standardmodellibrary;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Flat;
using Maple2.File.Parser.MapXBlock;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MapEntityParser : Exporter<List<MapEntityMetadata>>
    {
        private List<MapEntityMetadata> Entities;
        private Dictionary<int, int> InteractRecipeMap;
        private Dictionary<string, Dictionary<int, SpawnMetadata>> SpawnTagMap;
        private Dictionary<string, string> Maps;

        public MapEntityParser(MetadataResources resources) : base(resources, "map-entity") { }

        protected override List<MapEntityMetadata> Parse()
        {
            Entities = new List<MapEntityMetadata>();

            // fetch interactID and recipeID relation from xml (can be expanded to parse other xml info)
            InteractRecipeMap = new Dictionary<int, int>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files
                .Where(entry => Regex.Match(entry.Name, "table/interactobject_mastery").Success))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList interactNodes = document.SelectNodes("/ms2/interact");

                foreach (XmlNode node in interactNodes)
                {
                    int interactID = int.Parse(node.Attributes["id"].Value);
                    XmlNode gatheringNode = node.SelectSingleNode("gathering");
                    int recipeID = int.Parse(gatheringNode.Attributes["receipeID"].Value);
                    InteractRecipeMap[interactID] = recipeID;
                }
            }

            // Get mob spawn ID and mob spawn information from xml (can be expanded to parse other xml info)
            SpawnTagMap = new Dictionary<string, Dictionary<int, SpawnMetadata>>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files
                .Where(entry => Regex.Match(entry.Name, "table/mapspawntag").Success))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList regionNodes = document.SelectNodes("/ms2/region");

                foreach (XmlNode node in regionNodes)
                {
                    string mapID = node.Attributes["mapCode"].Value;
                    int spawnPointID = int.Parse(node.Attributes["spawnPointID"].Value);

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

                    bool isPetSpawn = node.Attributes["petPopulation"] != null &&
                                      int.Parse(node.Attributes["petPopulation"].Value) > 0;

                    SpawnMetadata spawnData = new SpawnMetadata(spawnTags, population, spawnTime, difficulty,
                        minDifficulty, isPetSpawn);
                    if (!SpawnTagMap.ContainsKey(mapID))
                    {
                        SpawnTagMap[mapID] = new Dictionary<int, SpawnMetadata>();
                    }

                    SpawnTagMap[mapID][spawnPointID] = spawnData;
                }
            }

            // Iterate over map xblocks
            Maps = new Dictionary<string, string>();
            XBlockParser parser = new XBlockParser(Resources.ExportedReader, new FlatTypeIndex(Resources.ExportedReader));
            parser.Parse(BuildMetadata);

            Console.Out.WriteLine($"Parsed {Entities.Count} entities");

            // Since parsing is done in parallel, sort at the end for deterministic order.
            Entities.Sort((metadata1, metadata2) => metadata1.MapId.CompareTo(metadata2.MapId));
            return Entities;
        }

        private void BuildMetadata(string xblock, IEnumerable<IMapEntity> mapEntities)
        {
            if (xblock.EndsWith("_cn") || xblock.EndsWith("_jp") || xblock.EndsWith("_kr"))
            {
                return;
            }

            Match isParsableField = Regex.Match(xblock, @"^(\d{8})");
            if (!isParsableField.Success)
            {
                // TODO: Handle these later, if we need them. They're xblock files with some other names like
                //  character_test.xblock, login.xblock, 
                return;
            }

            string mapId = isParsableField.Groups[1].Value;
            if (Maps.ContainsKey(mapId))
            {
                return;
            }

            Maps.Add(mapId, xblock); // Only used to check if we've visited this node before.

            // TODO: metadata should be keyed on xblock name, not mapId
            MapEntityMetadata metadata = new MapEntityMetadata(int.Parse(mapId));

            foreach (IMapEntity entity in mapEntities)
            {
                switch (entity)
                {
                    case IMS2Bounding bounding:
                        if (bounding.EntityName.EndsWith("0"))
                        {
                            metadata.BoundingBox0 = ToCoordS(bounding.Position);
                        }
                        else if (bounding.EntityName.EndsWith("1"))
                        {
                            metadata.BoundingBox1 = ToCoordS(bounding.Position);
                        }
                        break;
                    // TODO: This can probably be more generally handled as IMS2RegionSkill
                    case IMS2HealingRegionSkillSound healingRegion:
                        if (healingRegion.Position == default)
                        {
                            continue;
                        }

                        metadata.HealingSpot.Add(ToCoordS(healingRegion.Position));
                        break;
                    case IMS2InteractObject interact: // TODO: this one is kinda fucked
                        if (interact.interactID == 0)
                        {
                            continue;
                        }

                        if (interact.ModelName.Contains("funct_extract_"))
                        {
                            metadata.InteractObjects.Add(new MapInteractObject(interact.EntityId, interact.EntityName,
                                InteractObjectType.Extractor, interact.interactID));
                            continue;
                        }

                        if (entity is IMS2Telescope)
                        {
                            metadata.InteractObjects.Add(new MapInteractObject(interact.EntityId, interact.EntityName,
                                InteractObjectType.Binoculars, interact.interactID));
                            continue;
                        }

                        if (interact.ModelName.EndsWith("hub") || interact.ModelName.EndsWith("vein"))
                        {
                            if (InteractRecipeMap.TryGetValue(interact.interactID, out int recipeId))
                            {
                                metadata.InteractObjects.Add(new MapInteractObject(interact.EntityId, interact.EntityName,
                                    InteractObjectType.Gathering, recipeId));
                            }
                            continue;
                        }

                        if (entity is IMS2InteractDisplay)
                        {
                            // TODO: Implement Interactive Displays like 02000183, Dark Wind Wanted Board (7bb334fe41f94182a9569ab884004c32)
                            // "mixinMS2InteractDisplay" ("ID_19100003_")
                        }

                        if (entity is IMS2InteractMesh)
                        {
                            // TODO: InteractMesh IS InteractObject, maybe shouldn't separate them...
                            metadata.InteractMeshes.Add(new MapInteractMesh(entity.EntityId, entity.EntityName));
                        }

                        metadata.InteractObjects.Add(new MapInteractObject(interact.EntityId, interact.EntityName,
                            InteractObjectType.Unknown, interact.interactID));
                        break;
                    case ISpawnPoint spawn:
                        switch (spawn)
                        {
                            case ISpawnPointPC pcSpawn:
                                metadata.PlayerSpawns.Add(
                                    new MapPlayerSpawn(ToCoordS(pcSpawn.Position), ToCoordS(pcSpawn.Rotation)));
                                break;
                            case ISpawnPointNPC npcSpawn:
                                // These tend to be vendors, shops, etc.
                                // If the name tag begins with SpawnPointNPC, I think these are mob spawn locations. Skipping these.
                                string npcIdStr = npcSpawn.NpcList.FirstOrDefault().Key ?? "0";
                                if (npcIdStr == "0" || !int.TryParse(npcIdStr, out int npcId))
                                {
                                    continue;
                                }

                                MapNpc npc = new MapNpc(npcId, npcSpawn.ModelName, npcSpawn.EntityName, ToCoordS(npcSpawn.Position),
                                    ToCoordS(npcSpawn.Rotation));
                                // Parse some additional flat supplemented data about this NPC.
                                npc.IsSpawnOnFieldCreate = npcSpawn.IsSpawnOnFieldCreate;
                                npc.IsDayDie = npcSpawn.dayDie;
                                npc.IsNightDie = npcSpawn.nightDie;

                                metadata.Npcs.Add(npc);
                                break;
                        }
                        break;
                    case IMS2RegionSpawnBase spawnBase:
                        switch (spawnBase)
                        {
                            case IMS2RegionSpawn regionSpawn:
                                SpawnMetadata mobSpawnData =
                                    (SpawnTagMap.ContainsKey(mapId) && SpawnTagMap[mapId].ContainsKey(regionSpawn.SpawnPointID))
                                        ? SpawnTagMap[mapId][regionSpawn.SpawnPointID]
                                        : null; // Do we need this spawn data (?)
                                int mobNpcCount = mobSpawnData?.Population ?? 6;

                                // TODO: This previously relied in "NpcList" to be set. NpcList is impossible to be set on
                                // MS2RegionSpawn, it's only set for SpawnPointNPC.
                                List<int> mobNpcList = new List<int>();
                                mobNpcList.Add(21000025); // Placeholder
                                metadata.MobSpawns.Add(new MapMobSpawn(regionSpawn.SpawnPointID, ToCoordS(regionSpawn.Position),
                                    mobNpcCount, mobNpcList, (int) regionSpawn.SpawnRadius, mobSpawnData));
                                break;
                            case IMS2RegionBoxSpawn boxSpawn:
                                // "QR_10000264_" is Quest Reward Chest? This is tied to a MS2TriggerAgent making this object appear.
                                break;
                        }
                        break;
                    case IPortal portal:
                        MapPortalFlag flags = portal.IsVisible ? MapPortalFlag.Visible : MapPortalFlag.None;
                        flags |= portal.PortalEnable ? MapPortalFlag.Enabled : MapPortalFlag.None;
                        flags |= portal.MinimapIconVisible ? MapPortalFlag.MinimapVisible : MapPortalFlag.None;
                        metadata.Portals.Add(new MapPortal(portal.PortalID, portal.ModelName, flags, portal.TargetFieldSN,
                            ToCoordS(portal.Position), ToCoordS(portal.Rotation), portal.TargetPortalID,
                            (byte) portal.PortalType));
                        break;
                    case IMS2Breakable:
                        // case IMS2BreakableActor
                        // TODO: Do we need to parse these as some special NPC object?
                        // "mixinMS2Breakable"  But not "mixinMS2BreakableActor", as in ke_fi_prop_buoy_A01_ or el_move_woodbox_B04_
                        break;
                    case IPlaceable placeable: // TODO: placeable might be too generic
                        // These are objects which you can place in the world
                        string nameCoord = placeable.EntityName.ToLower();
                        Match coordMatch = Regex.Match(nameCoord, @"-?\d+, -?\d+, -?\d+");
                        if (!coordMatch.Success)
                        {
                            continue;
                        }

                        // Only MS2MapProperties has ObjectWeaponItemCode
                        if (entity is not IMS2MapProperties mapProperties)
                        {
                            continue;
                        }

                        try
                        {
                            CoordB coord = CoordB.Parse(coordMatch.Value, ", ");
                            metadata.Objects.Add(new MapObject(coord, int.Parse(mapProperties.ObjectWeaponItemCode)));
                        }
                        catch (FormatException)
                        {
                            // ignored
                            Console.WriteLine($"Format error parsing {coordMatch.Value} as CoordB");
                        }
                        catch (OverflowException ex)
                        {
                            Console.WriteLine($"Error parsing {coordMatch.Value} as CoordB: {ex.Message}");
                        }
                        break;
                    case IMS2TriggerObject triggerObject:
                        switch (triggerObject)
                        {
                            case IMS2TriggerMesh triggerMesh:
                                Console.WriteLine($"found trigger mesh: \n ID:{triggerMesh.TriggerObjectID}\n {triggerMesh.ModelName}");
                                break;
                        }
                        break;
                }

                /* NPC Objects have a modelName of 8 digits followed by an underscore and a name that's the same thing,
                     *  but with a number (for each instance on that map) after it
                     *
                     * IM_ Prefixed items are Interactable Meshes supplemented by data in "xml/table/interactobject.xml"
                     * IA_ prefixed items are Interactable Actors (Doors, etc). Have an interactID, which is an event on interact.
                     * "mixinMS2MapProperties" is generic field items
                     *  "mixinMS2SalePost" - is for sale signs. Does a packet need to respond to this item?
                     */
                /*
                
                // Unhandled Items:
                // "mixinEventSpawnPointNPC", 
                // "mixinMS2Actor" as in "fa_fi_funct_irondoor_A01_"
                // MS2RegionSkill as in "SkillObj_co_Crepper_C03_" (Only on 8xxxx and 9xxxxx maps)
                // "mixinMS2FunctionCubeKFM" as in "ry_functobj_lamp_B01_", "ke_functobj_bath_B01_"
                // "mixinMS2FunctionCubeNIF"
                // "MS2MapProperties"->"MS2PhysXProp" that's not a weapon. Standard 
                
                /*
                 * if (Regex.Match(modelName, @"^\d{8}_").Success && Regex.Match(name, @"\d{1,3}").Success)
                 * {
                    // Parse non-permanent NPCs. These have no .flat files to supplement them.
                    string npcListIndex = node.SelectSingleNode("property[@name='NpcList']")?.FirstChild.Attributes["index"].Value ?? "-1";
                    if (npcListIndex == "-1")
                    {
                        continue;
                    }
                    string npcPositionValue = node.SelectSingleNode("property[@name='Position']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                    string npcRotationValue = node.SelectSingleNode("property[@name='Rotation']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                    // metadata.Npcs.Add(new MapNpc(int.Parse(npcListIndex), modelName, name, ParseCoord(npcPositionValue), ParseCoord(npcRotationValue)));
                }
                */
            }

            // No data on this map
            if (metadata.Npcs.Count == 0 && metadata.Portals.Count == 0 && metadata.PlayerSpawns.Count == 0)
            {
                return;
            }

            Entities.Add(metadata);
        }

        private static CoordS ToCoordS(Vector3 vector3)
        {
            return CoordS.From((short) vector3.X, (short) vector3.Y, (short) vector3.Z);
        }
    }
}
