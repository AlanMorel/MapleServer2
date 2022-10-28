using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SurvivalSilverPassRewardParser : Exporter<List<SurvivalSilverPassRewardMetadata>>
{
    public SurvivalSilverPassRewardParser(MetadataResources resources) : base(resources, MetadataName.SurvivalSilverPassReward) { }

    protected override List<SurvivalSilverPassRewardMetadata> Parse()
    {
        List<SurvivalSilverPassRewardMetadata> rewards = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.Equals("table/na/survivalpassreward.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            if (document.DocumentElement?.ChildNodes is null)
            {
                continue;
            }

            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "survivalPassReward")
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(node, "id", "level", "type1", "id1", "value1", "count1"))
                {
                    continue;
                }

                if (int.Parse(node.Attributes!["id"]!.Value) > 1)
                {
                    continue;
                }

                SurvivalSilverPassRewardMetadata metadata = new()
                {
                    Level = int.Parse(node.Attributes["level"]!.Value),
                    Type1 = node.Attributes["type1"]!.Value,
                    Id1 = node.Attributes["id1"]!.Value,
                    Value1 = node.Attributes["value1"]!.Value,
                    Count1 = node.Attributes["count1"]!.Value
                };

                rewards.Add(metadata);
            }
        }

        return rewards;
    }
}
