using System.Linq;
using System.Collections.Generic;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using System.Text.RegularExpressions;
using System.Xml;
using Maple2Storage.Types;

namespace GameDataParser.Parsers
{
    public class MapParser : Exporter<List<MapMetadata>>
    {
        public MapParser(MetadataResources resources) : base(resources, "map") { }

        protected override List<MapMetadata> Parse()
        {
            // Iterate over preset Cubes to later reference while iterating over exported maps
            List<string> mapCubes = new List<string>();
            foreach (PackFileEntry entry in Resources.ExportedFiles)
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
                if (mapCubes.Contains(objStr))
                {
                    continue;
                }

                // Parse XML
                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
                XmlElement root = document.DocumentElement;
                string cubeName = root.Attributes["name"].Value.ToLower();
                mapCubes.Add(cubeName);
            }

            // Parse every block for each map
            List<MapMetadata> mapsList = new List<MapMetadata>();
            foreach (PackFileEntry entry in Resources.ExportedFiles.Where(x => x.Name.StartsWith("xblock/")))
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
                int mapId = int.Parse(mapIdStr);
                metadata.Id = mapId;

                XmlDocument document = Resources.ExportedMemFile.GetDocument(entry.FileHeader);
                XmlNodeList mapEntities = document.SelectNodes("/game/entitySet/entity");

                List<CoordS> blockList = new List<CoordS>();
                foreach (XmlNode node in mapEntities)
                {
                    string modelName = node.Attributes["modelName"].Value.ToLower();
                    if (mapCubes.Contains(modelName))
                    {
                        XmlNode fallReturn = node.SelectSingleNode("property[@name='IsFallReturn']");
                        bool isFallReturn = (fallReturn?.FirstChild.Attributes["value"].Value) != "False";
                        if (!isFallReturn)
                        {
                            continue;
                        }
                        string id = node.Attributes["id"].Value.ToLower();
                        XmlNode blockCoord = node.SelectSingleNode("property[@name='Position']");
                        CoordS coordS = ParseCoord(blockCoord?.FirstChild.Attributes["value"].Value ?? "0, 0, 0");
                        blockList.Add(coordS);
                    }
                }

                if (blockList.Count == 0)
                {
                    continue;
                }

                metadata.Blocks.AddRange(blockList);
                mapsList.Add(metadata);
            }
            return mapsList;
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
