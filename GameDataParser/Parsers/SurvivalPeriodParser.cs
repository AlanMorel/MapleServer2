using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class SurvivalPeriodParser : Exporter<List<SurvivalPeriodMetadata>>
{
    public SurvivalPeriodParser(MetadataResources resources) : base(resources, MetadataName.SurvivalPeriod) { }

    protected override List<SurvivalPeriodMetadata> Parse()
    {
        List<SurvivalPeriodMetadata> periods = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("table/na/maplesurvivalopenperiod"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name != "period")
                {
                    continue;
                }

                SurvivalPeriodMetadata metadata = new()
                {
                    Id = int.Parse(node.Attributes["id"].Value),
                    RewardId = int.Parse(node.Attributes["survivalRewardID"].Value),
                    StartTime = DateTime.ParseExact(node.Attributes["startTime"].Value, "yyyy-MM-dd-HH-mm", System.Globalization.CultureInfo.InvariantCulture),
                    EndTime = DateTime.ParseExact(node.Attributes["endTime"].Value, "yyyy-MM-dd-HH-mm", System.Globalization.CultureInfo.InvariantCulture),
                    PassPrice = int.Parse(node.Attributes["passPrice"].Value)
                };

                periods.Add(metadata);
            }
        }

        return periods;
    }
}
