using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
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
            // Iterate over preset objects to later reference while iterating over exported maps
            Dictionary<string, string> mapObjects = new Dictionary<string, string>();
            foreach (PackFileEntry entry in Resources.ExportedFiles)
            {
                if (!entry.Name.StartsWith("flat/presets/presets object/"))
                {
                    continue;
                }

                // Check if file is valid
                string objStr = entry.Name.ToLower();
                if (string.IsNullOrEmpty(objStr))
                {
                    continue;
                }
                if (mapObjects.ContainsKey(objStr))
                {
                    //Console.WriteLine($"Duplicate {entry.Name} was already added as {mapObjects[objStr]}");
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
                XmlElement root = document.DocumentElement;
                XmlNodeList objProperties = document.SelectNodes("/model/property");

                string objectName = root.Attributes["name"].Value.ToLower();

                foreach (XmlNode node in objProperties)
                {
                    // Storing only weapon item code for now, but there are other uses
                    if (node.Attributes["name"].Value.Contains("ObjectWeaponItemCode"))
                    {
                        string weaponId = node?.FirstChild.Attributes["value"].Value ?? "0";

                        if (!weaponId.Equals("0"))
                        {
                            mapObjects.Add(objectName, weaponId);
                        }
                    }
                }
            }

            // Iterate over map xblocks
            List<MapEntityMetadata> entities = new List<MapEntityMetadata>();
            Dictionary<string, string> maps = new Dictionary<string, string>();
            foreach (PackFileEntry entry in Resources.ExportedFiles)
            {
                if (!entry.Name.StartsWith("xblock/"))
                {
                    continue;
                }

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
                XmlNodeList mapEntities = document.SelectNodes("/game/entitySet/entity");

                foreach (XmlNode node in mapEntities)
                {
                    string modelName = node.Attributes["modelName"].Value.ToLower();
                    // IM_ Prefixed items are Interactable Models defined/linked in "xml/table/interactobject.xml"
                    Match NpcEntityMatch = Regex.Match(node.Attributes["modelName"].Value, @"([1-4]\d{7})_(.*)?");

                    XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                    CoordS boundingBox = ParseCoord(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                    if (node.Attributes["name"].Value.Contains("MS2Bounding0"))
                    {
                        if (metadata.BoundingBox0.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox0 = boundingBox;
                        }
                    }
                    else if (node.Attributes["name"].Value.Contains("MS2Bounding1"))
                    {
                        if (metadata.BoundingBox1.Equals(CoordS.From(0, 0, 0)))
                        {
                            metadata.BoundingBox1 = boundingBox;
                        }
                    }
                    else if (node.Attributes["name"].Value.Contains("SpawnPointPC"))
                    {
                        XmlNode playerCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode playerRotation = node.SelectSingleNode("property[@name='Rotation']");

                        string playerPositionValue = playerCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string playerRotationValue = playerRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                        metadata.PlayerSpawns.Add(new MapPlayerSpawn(ParseCoord(playerPositionValue), ParseCoord(playerRotationValue)));
                    }
                    else if (mapObjects.ContainsKey(modelName))
                    {
                        string nameCoord = node.Attributes["name"].Value.ToLower();

                        Match coordMatch = Regex.Match(nameCoord, @"[\-]?\d+[,]\s[\-]?\d+[,]\s[\-]?\d+");

                        if (!coordMatch.Success)
                        {
                            continue;
                        }

                        CoordB coord = CoordB.Parse(coordMatch.Value, ", ");
                        metadata.Objects.Add(new MapObject(coord, int.Parse(mapObjects[modelName])));
                    }
                    else if (NpcEntityMatch.Success)
                    {
                        // Skip some items we don't handle yet
                        if (NpcEntityMatch.Groups[0].Value.Contains("WayPoint"))
                        {
                            continue;
                        }
                        else if (NpcEntityMatch.Groups[0].Value.Contains("PatrolData"))
                        {
                            continue;
                        }

                        int npcId = int.Parse(NpcEntityMatch.Groups[1].Value.Split("_")[0]);
                        XmlNode playerCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode playerRotation = node.SelectSingleNode("property[@name='Rotation']");
                        if (playerCoord == null) //  Some entities don't have a playerRotation. Just means 0,0,0.
                        {
                            continue;
                        }

                        string npcPositionValue = playerCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string npcRotationValue = playerRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        metadata.Npcs.Add(new MapNpc(npcId, ParseCoord(npcPositionValue), ParseCoord(npcRotationValue)));
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
