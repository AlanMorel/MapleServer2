using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.Flat;
using Maple2.File.Flat.maplestory2library;
using Maple2.File.IO.Crypto.Common;
using Maple2.File.Parser.Flat;
using Maple2.File.Parser.MapXBlock;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MapParser : Exporter<List<MapMetadata>>
    {
        private List<MapMetadata> mapsList;
        private Dictionary<int, string> mapNames;
        public MapParser(MetadataResources resources) : base(resources, "map") { }

        protected override List<MapMetadata> Parse()
        {
            mapsList = new List<MapMetadata>();
            
            // Parse map names
            mapNames = new Dictionary<int, string>();
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
            FlatTypeIndex index = new FlatTypeIndex(Resources.ExportedReader);
            XBlockParser parser = new XBlockParser(Resources.ExportedReader, index);
            parser.Include(typeof(IMS2CubeProp)); // We only care about cubes here
            
            parser.Parse(AddMetadata);
            
            return mapsList;
        }

        private void AddMetadata(string xblock, IEnumerable<IMapEntity> entities)
        {
            if (xblock.EndsWith("_cn") || xblock.EndsWith("_jp") || xblock.EndsWith("_kr"))
            {
                return;
            }
                
            string mapIdStr = Regex.Match(xblock, @"\d{8}").Value;
            if (string.IsNullOrEmpty(mapIdStr))
            {
                return;
            }

            MapMetadata metadata = new MapMetadata();
            metadata.Id = int.Parse(mapIdStr);
            metadata.XBlockName = xblock;

            foreach (IMapEntity entity in entities)
            {
                if (entity is not IMS2CubeProp cube)
                {
                    continue;
                }

                MapBlock mapBlock = new MapBlock
                {
                    Coord = CoordS.From((short) cube.Position.X, (short) cube.Position.Y,
                        (short) cube.Position.Z),
                    Type = cube.CubeType,
                    SaleableGroup = cube.CubeSalableGroup,
                    Attribute = cube.MapAttribute
                };
                    
                metadata.Blocks.Add(mapBlock);
            }
                
            if (metadata.Blocks.Count == 0)
            {
                return;
            }
                
            if (mapNames.ContainsKey(metadata.Id))
            {
                metadata.Name = mapNames[metadata.Id];
            }
            mapsList.Add(metadata);
        }
    }
}
