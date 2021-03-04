using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MapEntityParser : Exporter<List<MapEntityMetadata>>
    {
        public MapEntityParser(MetadataResources resources) : base(resources, "map-entity") { }

        protected override List<MapEntityMetadata> Parse()
        {
            List<MapEntityMetadata> entities = new List<MapEntityMetadata>();

            /* Iterate over preset objects to later reference while iterating over exported maps
             * Key is modelName, value is parsed key/value from property
             * in json, this would look like: 
             * {
             *   "11000003_": {"isSpawnPointNPC": "true", "SpawnPointID": "0", "SpawnRadius", "0"}
             * }
             */
            Dictionary<string, Dictionary<string, string>> mapObjects = new Dictionary<string, Dictionary<string, string>>();

            foreach (PackFileEntry entry in Resources.ExportedFiles
                .Where(entry => Regex.Match(entry.Name, @"^flat/presets/presets (object|npc)/").Success)
                .OrderBy(entry => entry.Name))
            {
                // Parse XML
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
                string modelName = document.DocumentElement.Attributes["name"].Value;

                // A local in-mem storage for all flat file supplementary data.
                Dictionary<string, string> thisNode = new Dictionary<string, string> { };

                foreach (XmlNode node in document.SelectNodes("model/mixin"))
                {
                    // These define the superclass this model inherits from. We store these as a property for later processing
                    // TODO: Should we only parse type="Active"?
                    if (node.Attributes["type"].Value.Equals("Active"))
                    {
                        string name = node.Attributes["name"].Value;
                        thisNode[$"mixin{name}"] = "true";  // isMS2InteractActor = "true";
                    }
                }

                foreach (XmlNode node in document.SelectNodes("model/property"))
                {
                    if (node.ChildNodes.Count > 0)  // hasChildren
                    {
                        thisNode[node.Attributes["name"].Value] = node.FirstChild?.Attributes["value"].Value;
                    }
                }
                mapObjects.Add(modelName, thisNode);
            }

            // Iterate over map xblocks
            Dictionary<string, string> maps = new Dictionary<string, string>();  // Have we already parsed this map?
            foreach (PackFileEntry entry in Resources.ExportedFiles
                .Where(entry => entry.Name.StartsWith("xblock/"))
                .OrderByDescending(entry => entry.Name))
            {
                Match isParsableField = Regex.Match(Path.GetFileNameWithoutExtension(entry.Name), @"^(\d{8})");
                if (!isParsableField.Success)
                {
                    // TODO: Handle these later, if we need them. They're xblock files with some other names like
                    //  character_test.xblock, login.xblock, 
                    continue;
                }

                string mapId = isParsableField.Groups[1].Value;
                if (maps.ContainsKey(mapId))
                {
                    continue;
                }
                maps.Add(mapId, entry.Name);  // Only used to check if we've visited this node before.

                MapEntityMetadata metadata = new MapEntityMetadata(int.Parse(mapId));
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);

                foreach (XmlNode node in document.SelectNodes("/game/entitySet/entity"))
                {
                    string modelName = node.Attributes["modelName"].Value;  // Always maps to a .flat fileName for additional supplemented data
                    string name = node.Attributes["name"].Value;

                    if (modelName == "MS2Bounding")
                    {
                        XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                        CoordS boundingBox = CoordS.Parse(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                        if (name.EndsWith("0") && metadata.BoundingBox0.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox0 = boundingBox;
                        }
                        else if (name.EndsWith("1") && metadata.BoundingBox1.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox1 = boundingBox;
                        }
                    }
                    else if (modelName.StartsWith("Skill_HealingSpot"))
                    {
                        XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                        CoordS healingCoord = CoordS.Parse(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                        if (healingCoord.Equals(CoordS.From(0, 0, 0)))
                        {
                            continue;
                        }
                        metadata.HealingSpot = healingCoord;
                    }
                    else if (modelName == "MS2Telescope")
                    {
                        string uuid = node.Attributes["id"].Value;
                        int interactId = int.Parse(node.SelectSingleNode("property[@name='interactID']")?.FirstChild.Attributes["value"]?.Value);
                        metadata.InteractActors.Add(new MapInteractActor(uuid, name, InteractActorType.Binoculars, interactId));
                    }
                    else if (modelName == "SpawnPointPC")  // Player Spawn point on map
                    {
                        XmlNode playerCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode playerRotation = node.SelectSingleNode("property[@name='Rotation']");

                        string playerPositionValue = playerCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string playerRotationValue = playerRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                        metadata.PlayerSpawns.Add(new MapPlayerSpawn(CoordS.Parse(playerPositionValue), CoordS.Parse(playerRotationValue)));
                    }
                    else if (modelName == "Portal_entrance" || modelName == "Portal_cube")
                    {
                        XmlNode portalIdNode = node.SelectSingleNode("property[@name='PortalID']");
                        XmlNode targetNode = node.SelectSingleNode("property[@name='TargetFieldSN']");
                        if (targetNode == null || portalIdNode == null)
                        {
                            continue;
                        }

                        XmlNode visibleNode = node.SelectSingleNode("property[@name='IsVisible']");
                        XmlNode enabledNode = node.SelectSingleNode("property[@name='PortalEnable']");
                        XmlNode minimapVisibleNode = node.SelectSingleNode("property[@name='MinimapIconVisible']");
                        XmlNode coordNode = node.SelectSingleNode("property[@name='Position']");
                        XmlNode rotationNode = node.SelectSingleNode("property[@name='Rotation']");

                        if (!bool.TryParse(visibleNode?.FirstChild.Attributes["value"].Value, out bool visibleValue))
                        {
                            visibleValue = true;
                        }
                        if (!bool.TryParse(enabledNode?.FirstChild.Attributes["value"].Value, out bool enabledValue))
                        {
                            enabledValue = true;
                        }
                        if (!bool.TryParse(minimapVisibleNode?.FirstChild.Attributes["value"].Value, out bool minimapVisibleValue))
                        {
                            minimapVisibleValue = true;
                        }

                        int target = int.Parse(targetNode.FirstChild.Attributes["value"].Value);
                        int portalId = int.Parse(portalIdNode?.FirstChild.Attributes["value"].Value);
                        string positionValue = coordNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string rotationValue = rotationNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                        MapPortalFlag flags = visibleValue ? MapPortalFlag.Visible : MapPortalFlag.None;
                        flags |= enabledValue ? MapPortalFlag.Enabled : MapPortalFlag.None;
                        flags |= minimapVisibleValue ? MapPortalFlag.MinimapVisible : MapPortalFlag.None;

                        CoordS position = CoordS.Parse(positionValue);
                        CoordS rotation = CoordS.Parse(rotationValue);
                        metadata.Portals.Add(new MapPortal(portalId, flags, target, position, rotation));
                    }
                    else if (modelName == "SpawnPointNPC" && !name.StartsWith("SpawnPointNPC"))
                    {
                        // These tend to be vendors, shops, etc.
                        // If the name tag begins with SpawnPointNPC, I think these are mob spawn locations. Skipping these.
                        string npcId = node.SelectSingleNode("property[@name='NpcList']")?.FirstChild.Attributes["index"].Value ?? "0";
                        if (npcId == "0")
                        {
                            continue;
                        }
                        string npcPositionValue = node.SelectSingleNode("property[@name='Position']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string npcRotationValue = node.SelectSingleNode("property[@name='Rotation']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        MapNpc thisNpc = new MapNpc(int.Parse(npcId), modelName, name, CoordS.Parse(npcPositionValue), CoordS.Parse(npcRotationValue));
                        thisNpc.IsSpawnOnFieldCreate = true;
                        metadata.Npcs.Add(thisNpc);
                    }
                    // Parse the rest of the objects in the xblock if they have a flat component.
                    else if (mapObjects.ContainsKey(modelName))  // There was .flat file data about this item
                    {
                        /* NPC Objects have a modelName of 8 digits followed by an underscore and a name that's the same thing,
                         *  but with a number (for each instance on that map) after it
                         *
                         * IM_ Prefixed items are Interactable Meshes supplemented by data in "xml/table/interactobject.xml"
                         * IA_ prefixed items are Interactable Actors (Doors, etc). Have an interactID, which is an event on interact.
                         * "mixinMS2MapProperties" is generic field items
                         *  "mixinMS2SalePost" - is for sale signs. Does a packet need to respond to this item?
                         */
                        Dictionary<string, string> modelData = mapObjects[modelName];

                        if (modelData.ContainsKey("mixinMS2TimeShowSetting") ||
                            modelData.ContainsKey("mixinMS2Sound") ||
                            modelData.ContainsKey("mixinMS2SalePost") ||
                            modelData.ContainsKey("mixinMS2Actor") ||  // "fa_fi_funct_irondoor_A01_"
                            modelData.ContainsKey("mixinMS2RegionSkill") ||
                            modelData.ContainsKey("mixinMS2FunctionCubeKFM") ||
                            modelData.ContainsKey("mixinMS2FunctionCubeNIF")
                            )
                        {
                            continue;  // Skip these for now.
                        }
                        else if (modelData.ContainsKey("mixinMS2InteractObject"))
                        {
                            if (modelData.ContainsKey("mixinMS2InteractMesh"))
                            {
                                // TODO: Implement mesh packet
                                string uuid = node.Attributes["id"].Value.ToLower();
                                metadata.InteractMeshes.Add(new MapInteractMesh(uuid, name));
                            }
                            else if (modelData.ContainsKey("mixinMS2InteractActor"))
                            {
                                string uuid = node.Attributes["id"].Value.ToLower();
                                metadata.InteractActors.Add(new MapInteractActor(uuid, name, InteractActorType.Unknown, 0));
                            }
                            else if (modelData.ContainsKey("mixinMS2InteractDisplay"))
                            {
                                // TODO: Implement Interactive Displays like 02000183, Dark Wind Wanted Board (7bb334fe41f94182a9569ab884004c32)
                                // "mixinMS2InteractDisplay" ("ID_19100003_")
                            }
                            else
                            {
                                // TODO: Any others?

                                if (name.Contains("funct_extract_"))
                                {
                                    string uuid = node.Attributes["id"].Value.ToLower();
                                    metadata.InteractActors.Add(new MapInteractActor(uuid, name, InteractActorType.Extractor, 0));
                                }
                                else if (name.Contains("funct_telescope_"))
                                {
                                    string uuid = node.Attributes["id"].Value;
                                    int interactId = int.Parse(node.SelectSingleNode("property[@name='interactID']")?.FirstChild.Attributes["value"]?.Value);
                                    metadata.InteractActors.Add(new MapInteractActor(uuid, name, InteractActorType.Binoculars, interactId));
                                }
                            }
                        }
                        else if (modelData.ContainsKey("mixinSpawnPointNPC"))
                        {
                            // These can be full natural spawns, or only spawnable as a reaction to a quest, or something else as well.
                            string npcId = Regex.Match(modelName, @"^(\d{8})_").Groups[1].Value;
                            string npcPositionValue = node.SelectSingleNode("property[@name='Position']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                            string npcRotationValue = node.SelectSingleNode("property[@name='Rotation']")?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                            MapNpc thisNpc = new MapNpc(int.Parse(npcId), modelName, name, CoordS.Parse(npcPositionValue), CoordS.Parse(npcRotationValue));
                            // Parse some additional flat supplemented data about this NPC.
                            if (mapObjects.ContainsKey(modelName))
                            {
                                Dictionary<string, string> thisFlatData = mapObjects[modelName];

                                // Parse IsSpawnOnFieldCreate
                                thisNpc.IsSpawnOnFieldCreate = thisFlatData["IsSpawnOnFieldCreate"] == "True";
                                thisNpc.IsDayDie = thisFlatData["dayDie"] == "True";
                                thisNpc.IsNightDie = thisFlatData["nightDie"] == "True";
                            }
                            metadata.Npcs.Add(thisNpc);
                        }
                        else if (modelData.ContainsKey("mixinMS2BreakableActor"))
                        {
                            // TODO: Do we need to parse these as some special NPC object?
                        }
                        else if (modelData.ContainsKey("mixinMS2Placeable"))
                        {
                            // These are objects which you can place in the world
                            string nameCoord = node.Attributes["name"].Value.ToLower();

                            Match coordMatch = Regex.Match(nameCoord, @"[\-]?\d+[,]\s[\-]?\d+[,]\s[\-]?\d+");

                            if (!coordMatch.Success)
                            {
                                continue;
                            }

                            CoordB coord = CoordB.Parse(coordMatch.Value, ", ");
                            metadata.Objects.Add(new MapObject(coord, int.Parse(modelData["ObjectWeaponItemCode"])));
                        }
                        else if (modelData.ContainsKey("mixinMS2CubeProp"))
                        {
                            if (!modelData.ContainsKey("ObjectWeaponItemCode"))
                            {
                                continue;
                            }
                            string weaponId = modelData["ObjectWeaponItemCode"] ?? "0";
                            if (!weaponId.Equals("0"))
                            {
                                // Extract the coordinate from the name. rhy tried just grabbing position, but it wasn't reliable.
                                Match coordMatch = Regex.Match(name, @"[\-]?\d+[,]\s[\-]?\d+[,]\s[\-]?\d+");
                                if (coordMatch.Success)
                                {
                                    metadata.Objects.Add(new MapObject(CoordB.Parse(coordMatch.Value, ", "), int.Parse(weaponId)));
                                }
                            }
                        }
                        else if (modelData.ContainsKey("mixinMS2Breakable"))
                        {
                            // "mixinMS2Breakable"  But not "mixinMS2BreakableActor", as in ke_fi_prop_buoy_A01_ or el_move_woodbox_B04_
                        }
                        else if (modelData.ContainsKey("mixinMS2RegionBoxSpawn"))
                        {
                            // "QR_10000264_" is Quest Reward Chest? This is tied to a MS2TriggerAgent making this object appear.
                        }
                        // Unhandled Items:
                        // "mixinEventSpawnPointNPC", 
                        // "mixinMS2Actor" as in "fa_fi_funct_irondoor_A01_"
                        // MS2RegionSkill as in "SkillObj_co_Crepper_C03_" (Only on 8xxxx and 9xxxxx maps)
                        // "mixinMS2FunctionCubeKFM" as in "ry_functobj_lamp_B01_", "ke_functobj_bath_B01_"
                        // "mixinMS2FunctionCubeNIF"
                        // "MS2MapProperties"->"MS2PhysXProp" that's not a weapon. Standard 
                    }
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
                    continue;
                }

                entities.Add(metadata);
            }
            Console.Out.WriteLine($"Parsed {entities.Count} entities");
            return entities;
        }
    }
}
