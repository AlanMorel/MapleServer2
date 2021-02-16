using System;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
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

            // Iterate over preset objects to later reference while iterating over exported maps
            Dictionary<string, Dictionary<string, string>> mapObjects = new Dictionary<string, Dictionary<string, string>>();
            foreach (PackFileEntry entry in Resources.ExportedFiles.Where(entry => entry.Name.StartsWith("flat/presets/presets object/")))
            {
                // Check if file is valid
                string objStr = entry.Name.ToLower();
                if (string.IsNullOrEmpty(objStr))
                {
                    continue;
                }
                if (mapObjects.ContainsKey(objStr))  // TODO: handle the regional files as well as the base files (by selecting a specific region)
                {
                    //Console.WriteLine($"Duplicate {entry.Name} was already added as {mapObjects[objStr]}");
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
                XmlElement root = document.DocumentElement;
                string modelName = root.Attributes["name"].Value;

                string fileName = Path.GetFileNameWithoutExtension(entry.Name);

                // Insert an empty dictionary into the mapObjects dictionary to hold keys we care about.
                mapObjects.Add(modelName, new Dictionary<string, string> { });

                // Parse InteractMesh nodes. These are doors, etc. Filename is im_
                // Parse InteractActor nodes. These are portals and things the toon can interact with normally.
                if (Regex.Match(fileName, @"^i(m|a)_\d{8}_").Success)
                {
                    // Look for IsVisible, add it if found.
                    XmlNode isVisibleNode = document.SelectSingleNode("/model/property[@name='IsVisible']");
                    if (isVisibleNode != null)
                        if (isVisibleNode.FirstChild.Attributes["value"].Value.Equals("True"))
                            mapObjects[modelName]["IsVisible"] = "True";
                }
                else
                {
                    XmlNode objectWeaponItemCodeNode = document.SelectSingleNode("/model/property[@name='ObjectWeaponItemCode']");
                    if (objectWeaponItemCodeNode != null)
                    {
                        string weaponId = objectWeaponItemCodeNode.FirstChild.Attributes["value"].Value ?? "0";
                        if (!weaponId.Equals("0"))
                        {
                            mapObjects[modelName]["ObjectWeaponItemCode"] = weaponId;
                        }
                    }
                }
            }

            // TODO: Iterate over "presets npc" objects to supplement the initial state of an npc. Especially useful for patroldata, etc.
            /*
            foreach (PackFileEntry entry in Resources.ExportedFiles.Where(entry => entry.Name.StartsWith("flat/presets/presets npc/")))
            {
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
            }
            */

            // Iterate over map xblocks
            Dictionary<string, string> maps = new Dictionary<string, string>();
            foreach (PackFileEntry entry in Resources.ExportedFiles.Where(entry => entry.Name.StartsWith("xblock/")))
            {
                string mapIdStr = Regex.Match(entry.Name, @"\d{8}").Value;
                if (string.IsNullOrEmpty(mapIdStr))
                {
                    continue;
                }
                int mapId = int.Parse(mapIdStr);
                if (maps.ContainsKey(mapIdStr))
                {
                    //Console.WriteLine($"Duplicate {entry.Name} was already added as {maps[mapIdStr]}");
                    continue;
                }
                maps.Add(mapIdStr, entry.Name);

                MapEntityMetadata metadata = new MapEntityMetadata(mapId);
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);

                foreach (XmlNode node in document.SelectNodes("/game/entitySet/entity"))
                {
                    string modelName = node.Attributes["modelName"].Value;
                    string name = node.Attributes["name"].Value;

                    /* NPC Objects have a modelName of 8 digits followed by an underscore and a name that's the same thing,
                     *  but with a number (for each instance on that map) after it
                     */
                    // IM_ Prefixed items are Interactable Meshes supplemented by data in "xml/table/interactobject.xml"
                    // IA_ prefixed items are Interactable Actors (Doors, etc). Have an interactID, which is an event on interact.
                    // SpawnPointNPC is where event baddies spawn. It gets a "SpawnPointID" (e.g. 998)
                    Match npcEntityMatch = Regex.Match(modelName, @"^(\d{8})_$");
                    // Parse NPCs whose modelName matches the name field, but with an underscore on modelname.
                    if (npcEntityMatch.Success)
                    {
                        int npcId = int.Parse(npcEntityMatch.Groups[1].Value);
                        if (Regex.Match(name, @$"^{modelName}\d+$").Success)  // The name is the same as the modelName, but with an underscore in modelName
                        {
                            XmlNode npcCoord = node.SelectSingleNode("property[@name='Position']");
                            XmlNode npcRotation = node.SelectSingleNode("property[@name='Rotation']");
                            string npcPositionValue = npcCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                            string npcRotationValue = npcRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                            metadata.Npcs.Add(new MapNpc(npcId, ParseCoord(npcPositionValue), ParseCoord(npcRotationValue)));
                        }
                        // TODO: an else. There's a lot of other cases to choose an NPC. Most *other* npcs are event/quest driven.
                    }

                    if (name.Contains("MS2Bounding0"))
                    {
                        XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                        CoordS boundingBox = ParseCoord(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                        if (metadata.BoundingBox0.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox0 = boundingBox;
                        }
                    }
                    else if (name.Contains("MS2Bounding1"))
                    {
                        XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                        CoordS boundingBox = ParseCoord(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                        if (metadata.BoundingBox1.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox1 = boundingBox;
                        }
                    }
                    else if (name.Contains("SpawnPointPC"))
                    {
                        XmlNode playerCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode playerRotation = node.SelectSingleNode("property[@name='Rotation']");

                        string playerPositionValue = playerCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string playerRotationValue = playerRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                        metadata.PlayerSpawns.Add(new MapPlayerSpawn(ParseCoord(playerPositionValue), ParseCoord(playerRotationValue)));
                    }
                    else if (modelName.StartsWith("IA_"))  // InteractActor
                    {
                        string uuid = node.Attributes["id"].Value.ToLower();
                        metadata.InteractActors.Add(new MapInteractActor(uuid, name));
                    }
                    else if (mapObjects.ContainsKey(modelName))
                    {
                        XmlNode npcId = node.SelectSingleNode("property[@name='Name']");
                        XmlNode npcCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode npcRotation = node.SelectSingleNode("property[@name='Rotation']");

                        if (mapObjects[modelName].ContainsKey("ObjectWeaponItemCode"))
                        {
                            string nameCoord = node.Attributes["name"].Value.ToLower();
                            Match coordMatch = Regex.Match(nameCoord, @"[\-]?\d+[,]\s[\-]?\d+[,]\s[\-]?\d+");

                            if (!coordMatch.Success)
                            {
                                continue;
                            }

                            CoordB coord = CoordB.Parse(coordMatch.Value, ", ");
                            metadata.Objects.Add(new MapObject(coord, int.Parse(mapObjects[modelName]["ObjectWeaponItemCode"])));
                        }
                    }
                }

                //XmlNodeList nodes = document.SelectNodes("/game/entitySet/entity/property[@name='NpcList']");
                XmlNodeList nodes = document.SelectNodes("/game/entitySet/entity/property");
                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["name"].Value == "NpcList")
                    {
                        if (node.FirstChild != null)
                        {
                            XmlNode parent = node.ParentNode;
                            try
                            {
                                XmlNode coordNode = parent.SelectSingleNode("property[@name='Position']");
                                XmlNode rotationNode = parent.SelectSingleNode("property[@name='Rotation']");

                                int npcId = int.Parse(node.FirstChild.Attributes["index"].Value);
                                string positionValue = coordNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                                string rotationValue = rotationNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                                CoordS position = ParseCoord(positionValue);
                                CoordS rotation = ParseCoord(rotationValue);
                                metadata.Npcs.Add(new MapNpc(npcId, position, rotation));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                                Console.WriteLine(mapId);
                                Console.WriteLine("Failed NPC " + parent.InnerXml);
                            }
                        }
                    }
                    else if (node.Attributes["name"].Value == "PortalID")
                    {
                        XmlNode parent = node.ParentNode;
                        try
                        {

                            XmlNode visibleNode = parent.SelectSingleNode("property[@name='IsVisible']");
                            XmlNode enabledNode = parent.SelectSingleNode("property[@name='PortalEnable']");
                            XmlNode minimapVisibleNode = parent.SelectSingleNode("property[@name='MinimapIconVisible']");
                            XmlNode targetNode = parent.SelectSingleNode("property[@name='TargetFieldSN']");
                            XmlNode coordNode = parent.SelectSingleNode("property[@name='Position']");
                            XmlNode rotationNode = parent.SelectSingleNode("property[@name='Rotation']");
                            if (targetNode == null)
                                continue;

                            if (!bool.TryParse(visibleNode?.FirstChild.Attributes["value"].Value,
                                    out bool visibleValue))
                            {
                                visibleValue = true;
                            }
                            if (!bool.TryParse(enabledNode?.FirstChild.Attributes["value"].Value,
                                    out bool enabledValue))
                            {
                                enabledValue = true;
                            }
                            if (!bool.TryParse(minimapVisibleNode?.FirstChild.Attributes["value"].Value,
                                    out bool minimapVisibleValue))
                            {
                                minimapVisibleValue = true;
                            }

                            int target = int.Parse(targetNode.FirstChild.Attributes["value"].Value);
                            int portalId = int.Parse(node.FirstChild.Attributes["value"].Value);
                            string positionValue = coordNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                            string rotationValue = rotationNode?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";


                            MapPortalFlag flags = visibleValue ? MapPortalFlag.Visible : MapPortalFlag.None;
                            flags |= enabledValue ? MapPortalFlag.Enabled : MapPortalFlag.None;
                            flags |= minimapVisibleValue ? MapPortalFlag.MinimapVisible : MapPortalFlag.None;

                            CoordS position = ParseCoord(positionValue);
                            CoordS rotation = ParseCoord(rotationValue);
                            metadata.Portals.Add(new MapPortal(portalId, flags, target, position, rotation));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            Console.WriteLine(mapId);
                            Console.WriteLine("Failed NPC " + parent.InnerXml);
                        }
                    }
                }

                // No data on this map
                if (metadata.Npcs.Count == 0 && metadata.Portals.Count == 0 && metadata.PlayerSpawns.Count == 0)
                {
                    continue;
                }

                entities.Add(metadata);
            }

            return entities;
        }

        private static CoordS ParseCoord(string value)
        {
            string[] coord = value.Split(", ");
            return CoordS.From(
                (short) float.Parse(coord[0]),
                (short) float.Parse(coord[1]),
                (short) float.Parse(coord[2])
            );
        }
    }
}
