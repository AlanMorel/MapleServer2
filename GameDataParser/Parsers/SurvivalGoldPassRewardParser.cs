using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SurvivalGoldPassRewardParser : Exporter<List<SurvivalGoldPassRewardMetadata>>
{
    public SurvivalGoldPassRewardParser(MetadataResources resources) : base(resources, MetadataName.SurvivalGoldPassReward) { }

    protected override List<SurvivalGoldPassRewardMetadata> Parse()
    {
        List<SurvivalGoldPassRewardMetadata> rewards = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.Equals("table/na/survivalpassreward_paid.xml"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "survivalPassReward")
                {
                    continue;
                }

                if (int.Parse(node.Attributes["id"].Value) > 1)
                {
                    continue;
                }

                SurvivalGoldPassRewardMetadata metadata = new()
                {
                    Level = int.Parse(node.Attributes["level"].Value),
                    Type1 = node.Attributes["type1"].Value,
                    Id1 = node.Attributes["id1"].Value,
                    Value1 = node.Attributes["value1"].Value,
                    Count1 = node.Attributes["count1"].Value,
                    Type2 = node.Attributes["type2"].Value,
                    Id2 = node.Attributes["id2"].Value,
                    Value2 = node.Attributes["value2"].Value,
                    Count2 = node.Attributes["count2"].Value
                };

                rewards.Add(metadata);
            }
        }

        return rewards;
    }
}
