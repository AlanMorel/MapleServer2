using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace GameDataParser.Parsers
{
    public static class MapEntityParser
    {

        public static List<MapEntityMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<MapEntityMetadata> entities = new List<MapEntityMetadata>();
            Dictionary<string, string> maps = new Dictionary<string, string>();
            foreach (PackFileEntry entry in entries)
            {
                if (!entry.Name.StartsWith("xblock/")) continue;

                string mapIdStr = Regex.Match(entry.Name, @"\d{8}").Value;
                if (string.IsNullOrEmpty(mapIdStr)) continue;
                int mapId = int.Parse(mapIdStr);
                if (maps.ContainsKey(mapIdStr))
                {
                    Console.WriteLine($"Duplicate {entry.Name} was already added as {maps[mapIdStr]}");
                    continue;
                }
                maps.Add(mapIdStr, entry.Name);

                MapEntityMetadata metadata = new MapEntityMetadata(mapId);
                XmlDocument document = m2dFile.GetDocument(entry.FileHeader);
                XmlNodeList mapEntities = document.SelectNodes("/game/entitySet/entity");

                foreach (XmlNode node in mapEntities)
                {
                    if (node.Attributes["name"].Value.Contains("SpawnPointPC"))
                    {
                        XmlNode playerCoord = node.SelectSingleNode("property[@name='Position']");
                        XmlNode playerRotation = node.SelectSingleNode("property[@name='Rotation']");

                        string playerPositionValue = playerCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";
                        string playerRotationValue = playerRotation?.FirstChild.Attributes["value"].Value ?? "0, 0, 0";

                        metadata.PlayerSpawns.Add(new MapPlayerSpawn(ParseCoord(playerPositionValue), ParseCoord(playerRotationValue)));
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
                            if (targetNode == null) continue;

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
                (short)float.Parse(coord[0]),
                (short)float.Parse(coord[1]),
                (short)float.Parse(coord[2])
            );
        }

        public static void Write(List<MapEntityMetadata> entities)
        {
            using (FileStream writeStream = File.OpenWrite(VariableDefines.OUTPUT + "ms2-map-entity-metadata"))
            {
                Serializer.Serialize(writeStream, entities);
            }
            using (FileStream readStream = File.OpenRead(VariableDefines.OUTPUT + "ms2-map-entity-metadata"))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(entities.SequenceEqual(Serializer.Deserialize<List<MapEntityMetadata>>(readStream)));
            }
            Console.WriteLine("Successfully parsed map entity metadata!");
        }
    }
}
