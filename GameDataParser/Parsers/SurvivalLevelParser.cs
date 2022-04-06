using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SurvivalLevelParser : Exporter<List<SurvivalLevelMetadata>>
{
    public SurvivalLevelParser(MetadataResources resources) : base(resources, MetadataName.SurvivalLevel) { }

    protected override List<SurvivalLevelMetadata> Parse()
    {
        List<SurvivalLevelMetadata> levels = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/na/survivallevel"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "survivalLevelExp")
                {
                    continue;
                }

                SurvivalLevelMetadata metadata = new()
                {
                    Level = int.Parse(node.Attributes["level"].Value),
                    RequiredExp = long.Parse(node.Attributes["reqExp"].Value),
                    Grade = byte.Parse(node.Attributes["grade"].Value)
                };

                levels.Add(metadata);
            }
        }

        return levels;
    }
}
