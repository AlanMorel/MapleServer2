using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class HomeTemplateParser : Exporter<List<HomeTemplateMetadata>>
    {
        public HomeTemplateParser(MetadataResources resources) : base(resources, "home-template") { }

        protected override List<HomeTemplateMetadata> Parse()
        {
            List<HomeTemplateMetadata> homeTemplates = new List<HomeTemplateMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("exportedugcmap/"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                string filename = Path.GetFileNameWithoutExtension(entry.Name);

                if (filename.Length > 1) // Only parsing the default templates. Woodland Path, Pink Perfect and Kerning Bunker
                {
                    continue;
                }

                HomeTemplateMetadata homeTemplate = new HomeTemplateMetadata();
                homeTemplate.Cubes = new List<CubeTemplate>();
                homeTemplate.Id = filename;

                XmlNode item = document.SelectSingleNode("ugcmap");
                string[] size = item.Attributes["indoorSizeType"].Value.Split('x');

                homeTemplate.Size = byte.Parse(size[0]);
                homeTemplate.Height = byte.Parse(size[2]);
                sbyte[] baseCoordB = item.Attributes["baseCubePoint3"].Value.Split(",").Select(sbyte.Parse).ToArray();
                CoordF baseCoordF = CoordF.From(baseCoordB[0] * Block.BLOCK_SIZE, baseCoordB[1] * Block.BLOCK_SIZE, baseCoordB[2] * Block.BLOCK_SIZE);

                XmlNodeList cubes = document.GetElementsByTagName("cube");
                foreach (XmlNode cube in cubes)
                {
                    int itemId = int.Parse(cube.Attributes["itemID"].Value);

                    byte[] coordsB = cube.Attributes["offsetCubePoint3"].Value.Split(",").Select(byte.Parse).ToArray();
                    CoordF cubeCoordF;

                    cubeCoordF = CoordF.From(coordsB[0] * Block.BLOCK_SIZE, coordsB[1] * Block.BLOCK_SIZE, coordsB[2] * Block.BLOCK_SIZE);
                    cubeCoordF.X += baseCoordF.X;
                    cubeCoordF.Y += baseCoordF.Y;

                    int rotation = int.Parse(cube.Attributes["rotation"].Value);
                    CoordF rotationF = CoordF.From(0, 0, rotation);

                    homeTemplate.Cubes.Add(new CubeTemplate(itemId, cubeCoordF, rotationF));
                }

                homeTemplates.Add(homeTemplate);
            }

            return homeTemplates;
        }
    }
}
