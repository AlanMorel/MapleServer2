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
        private List<MapMetadata> MapsList;
        private Dictionary<int, string> MapNames;
        public MapParser(MetadataResources resources) : base(resources, "map") { }

        protected override List<MapMetadata> Parse()
        {
            MapsList = new List<MapMetadata>();

            // Parse map names
            MapNames = new Dictionary<int, string>();
            PackFileEntry file = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/mapname.xml"));
            XmlDocument document = Resources.XmlReader.GetXmlDocument(file);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                int id = int.Parse(node.Attributes["id"].Value);
                string name = node.Attributes["name"].Value;
                MapNames[id] = name;
            }

            // Parse every block for each map
            FlatTypeIndex index = new FlatTypeIndex(Resources.ExportedReader);
            XBlockParser parser = new XBlockParser(Resources.ExportedReader, index);
            parser.Include(typeof(IMS2CubeProp)); // We only care about cubes here

            parser.Parse(AddMetadata);

            return MapsList;
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

                metadata.Blocks.TryAdd(mapBlock.Coord, mapBlock);
            }

            if (metadata.Blocks.Count == 0)
            {
                return;
            }

            if (MapNames.ContainsKey(metadata.Id))
            {
                metadata.Name = MapNames[metadata.Id];
            }
            MapsList.Add(metadata);
        }
    }
}
