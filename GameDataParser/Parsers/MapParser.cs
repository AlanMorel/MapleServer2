using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MapParser : Exporter<List<MapMetadata>>
    {
        public MapParser(MetadataResources resources) : base(resources, "map") { }

        protected override List<MapMetadata> Parse()
        {
            // Iterate over preset Cubes to later reference while iterating over exported maps
            Dictionary<string, string> mapCubes = new Dictionary<string, string>();
            foreach (PackFileEntry entry in Resources.ExportedReader.Files)
            {
                if (!entry.Name.StartsWith("flat/presets/presets cube/"))
                {
                    continue;
                }

                // Check if file is valid
                string objStr = entry.Name.ToLower();
                if (string.IsNullOrEmpty(objStr))
                {
                    continue;
                }
                if (mapCubes.ContainsKey(objStr))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.ExportedReader.GetXmlDocument(entry);
                XmlElement root = document.DocumentElement;
                string cubeName = root.Attributes["name"].Value.ToLower();
                XmlNode propertyAttribute = root.SelectSingleNode("property[@name='MapAttribute']");
                string mapAttribute = propertyAttribute.FirstChild.Attributes["value"].Value;
                mapCubes.Add(cubeName, mapAttribute);
            }

            // Parse map names
            Dictionary<int, string> mapNames = new Dictionary<int, string>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files.Where(x => x.Name.StartsWith("string/en/mapname.xml")))
            {
                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    int id = int.Parse(node.Attributes["id"].Value);
                    string name = node.Attributes["name"].Value;
                    mapNames[id] = name;
                }
            }

            // Parse every block for each map
            List<MapMetadata> mapsList = new List<MapMetadata>();
            foreach (PackFileEntry entry in Resources.ExportedReader.Files.Where(x => x.Name.StartsWith("xblock/")))
            {
                if (entry.Name.Contains("_cn.xblock") || entry.Name.Contains("_jp.xblock") || entry.Name.Contains("_kr.xblock"))
                {
                    continue;
                }

                string mapIdStr = Regex.Match(entry.Name, @"\d{8}").Value;
                if (string.IsNullOrEmpty(mapIdStr))
                {
                    continue;
                }

                MapMetadata metadata = new MapMetadata();
                metadata.Id = int.Parse(mapIdStr);

                string xblockName = entry.Name[7..];
                metadata.XBlockName = xblockName.Remove(xblockName.Length - 7, 7);

                XmlDocument document = Resources.ExportedReader.GetXmlDocument(entry);
                XmlNodeList mapEntities = document.SelectNodes("/game/entitySet/entity");

                List<MapBlock> blocks = new List<MapBlock>();
                foreach (XmlNode node in mapEntities)
                {
                    MapBlock mapBlock = new MapBlock();
                    string modelName = node.Attributes["modelName"].Value.ToLower();
                    if (!mapCubes.ContainsKey(modelName))
                    {
                        continue;
                    }
                    XmlNode fallReturn = node.SelectSingleNode("property[@name='IsFallReturn']");
                    bool isFallReturn = (fallReturn?.FirstChild.Attributes["value"].Value) != "False";
                    if (!isFallReturn)
                    {
                        continue;
                    }
                    string id = node.Attributes["id"].Value.ToLower();
                    XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                    CoordS coordS = CoordS.Parse(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                    mapBlock.Coord = coordS;

                    XmlNode blockType = node.SelectSingleNode("property[@name='CubeType']");
                    if (blockType != null)
                    {
                        mapBlock.Type = blockType?.FirstChild.Attributes["value"].Value;
                    }

                    XmlNode saleable = node.SelectSingleNode("property[@name='CubeSalableGroup']");
                    if (saleable != null)
                    {
                        mapBlock.SaleableGroup = int.Parse(saleable?.FirstChild.Attributes["value"].Value);
                    }

                    if (mapCubes.ContainsKey(modelName))
                    {
                        mapBlock.Attribute = mapCubes[modelName];
                    }

                    metadata.Blocks.Add(mapBlock);
                }

                if (metadata.Blocks.Count == 0)
                {
                    continue;
                }

                if (mapNames.ContainsKey(metadata.Id))
                {
                    metadata.Name = mapNames[metadata.Id];
                }
                mapsList.Add(metadata);
            }
            return mapsList;
        }
    }
}
