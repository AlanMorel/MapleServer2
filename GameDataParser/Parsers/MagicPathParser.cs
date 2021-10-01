using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class MagicPathParser : Exporter<List<MagicPathMetadata>>
    {
        public MagicPathParser(MetadataResources resources) : base(resources, "magicpath") { }

        protected override List<MagicPathMetadata> Parse()
        {
            List<MagicPathMetadata> magicPathList = new List<MagicPathMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

                XmlNodeList pathTypeList = document.SelectNodes("/ms2/type");
                foreach (XmlNode pathType in pathTypeList)
                {
                    long id = long.Parse(pathType.Attributes["id"]?.Value ?? "0");

                    List<MagicPathMove> pathMoves = new List<MagicPathMove>();
                    CoordF fireOffsetPosition = CoordF.From(0, 0, 0);
                    CoordF direction = CoordF.From(0, 0, 0);
                    CoordF controlValue0 = CoordF.From(0, 0, 0);
                    CoordF controlValue1 = CoordF.From(0, 0, 0);

                    XmlNodeList pathMoveList = pathType.SelectNodes("move");
                    foreach (XmlNode pathMove in pathMoveList)
                    {
                        int rotation = int.Parse(pathMove.Attributes["rotation"]?.Value ?? "0");

                        if (pathMove.Attributes["fireOffsetPosition"] != null)
                        {
                            fireOffsetPosition = ParseCoordWithoutLastChar(pathMove.Attributes["fireOffsetPosition"]?.Value ?? "0");
                        }

                        if (pathMove.Attributes["direction"] != null)
                        {
                            direction = ParseCoordWithDuplicateDot(pathMove.Attributes["direction"].Value ?? "0");
                        }

                        if (pathMove.Attributes["controlValue0"] != null)
                        {
                            controlValue0 = ParseCoordFromString(pathMove.Attributes["controlValue0"].Value ?? "0");
                        }

                        if (pathMove.Attributes["controlValue1"] != null)
                        {
                            controlValue1 = ParseCoordFromString(pathMove.Attributes["controlValue1"].Value ?? "0");
                        }

                        bool ignoreAdjust = pathMove.Attributes["ignoreAdjustCubePosition"] != null;

                        pathMoves.Add(new MagicPathMove(rotation, fireOffsetPosition, direction, controlValue0, controlValue1, ignoreAdjust));
                    }

                    MagicPathMetadata newMagicPath = new MagicPathMetadata(id, pathMoves);
                    magicPathList.Add(newMagicPath);
                }
            }
            return magicPathList;
        }

        public static CoordF ParseCoordWithDuplicateDot(string input)
        {
            float[] floatArray = new float[input.Length];

            for (int i = 0; i < input.Split(",").Length; i++)
            {
                floatArray[i] = float.Parse(Regex.Match(input.Split(",")[i], "[+-]?([0-9]*[.])?[0-9]+").Value);
            }

            return CoordF.From(floatArray[0], floatArray[1], floatArray[2]);
        }

        public static CoordF ParseCoordWithoutLastChar(string input)
        {
            float[] floatArray = new float[input.Length];

            if (input.EndsWith(','))
            {
                string tempString = input.Remove(input.Length - 1);
                return ParseCoordFromString(tempString);
            }
            else
            {
                return ParseCoordFromString(input);
            }
        }

        public static CoordF ParseCoordFromString(string input)
        {
            float[] floatArray = Array.ConvertAll(input.Split(","), float.Parse);

            if (floatArray.Length < 3)
            {
                float[] tempFloat = new float[3] { floatArray[0], floatArray[1], 0 };
                floatArray = tempFloat;
            }

            return CoordF.From(floatArray[0], floatArray[1], floatArray[2]);
        }
    }
}
