using System.Text.RegularExpressions;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class MagicPathParser : Exporter<List<MagicPathMetadata>>
{
    public MagicPathParser(MetadataResources resources) : base(resources, "magicpath") { }

    protected override List<MagicPathMetadata> Parse()
    {
        List<MagicPathMetadata> magicPathList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/magicpath"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);

            XmlNodeList pathTypeList = document.SelectNodes("/ms2/type");
            foreach (XmlNode pathType in pathTypeList)
            {
                long id = long.Parse(pathType.Attributes["id"]?.Value ?? "0");

                List<MagicPathMove> pathMoves = new();
                XmlNodeList pathMoveList = pathType.SelectNodes("move");
                foreach (XmlNode pathMove in pathMoveList)
                {
                    int rotation = int.Parse(pathMove.Attributes["rotation"]?.Value ?? "0");

                    CoordF fireOffsetPosition = ParseCoordWithoutLastChar(pathMove.Attributes["fireOffsetPosition"]?.Value ?? "0,0,");
                    CoordF direction = ParseCoordWithDuplicateDot(pathMove.Attributes["direction"]?.Value ?? "0");
                    CoordF controlValue0 = ParseCoordFromString(pathMove.Attributes["controlValue0"]?.Value ?? "0,0,0");
                    CoordF controlValue1 = ParseCoordFromString(pathMove.Attributes["controlValue1"]?.Value ?? "0,0,0");

                    bool ignoreAdjust = pathMove.Attributes["ignoreAdjustCubePosition"] is null;
                    bool traceTargetOffsetPos = pathMove.Attributes["traceTargetOffsetPos"]?.Value == "1";
                    float distance = float.Parse(pathMove.Attributes["distance"]?.Value ?? "0");

                    pathMoves.Add(new(rotation, fireOffsetPosition, direction, controlValue0, controlValue1, ignoreAdjust, traceTargetOffsetPos, distance));
                }

                MagicPathMetadata newMagicPath = new(id, pathMoves);
                magicPathList.Add(newMagicPath);
            }
        }

        return magicPathList;
    }

    private static CoordF ParseCoordWithDuplicateDot(string input)
    {
        float[] floatArray = new float[input.Length];

        for (int i = 0; i < input.Split(",").Length; i++)
        {
            floatArray[i] = float.Parse(Regex.Match(input.Split(",")[i], "[+-]?([0-9]*[.])?[0-9]+").Value);
        }

        return CoordF.From(floatArray[0], floatArray[1], floatArray[2]);
    }

    private static CoordF ParseCoordWithoutLastChar(string input)
    {
        if (input.EndsWith(','))
        {
            return ParseCoordFromString(input.Remove(input.Length - 1));
        }

        return ParseCoordFromString(input);
    }

    private static CoordF ParseCoordFromString(string input)
    {
        float[] floatArray = Array.ConvertAll(input.Split(","), float.Parse);

        if (floatArray.Length < 3)
        {
            floatArray = new[]{
                floatArray[0], floatArray[1], 0
            };
        }

        return CoordF.From(floatArray[0], floatArray[1], floatArray[2]);
    }
}
