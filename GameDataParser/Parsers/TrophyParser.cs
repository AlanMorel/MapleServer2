using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class TrophyParser : Exporter<List<TrophyMetadata>>
{
    public TrophyParser(MetadataResources resources) : base(resources, "trophy") { }

    protected override List<TrophyMetadata> Parse()
    {
        List<TrophyMetadata> trophyList = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("achieve/"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNode trophy = document.SelectSingleNode("/ms2/achieves");

            TrophyMetadata newTrophy = new()
            {
                Id = int.Parse(trophy.Attributes["id"].Value),
                Categories = trophy.Attributes["categoryTag"]?.Value.Split(","),
                AccountWide = trophy.Attributes["account"].Value == "1"
            };

            XmlNodeList grades = trophy.SelectNodes("grade");

            foreach (XmlNode grade in grades)
            {
                XmlNode condition = grade.SelectSingleNode("condition");
                XmlNode reward = grade.SelectSingleNode("reward");
                Enum.TryParse(reward.Attributes["type"].Value, true, out RewardType type);

                TrophyGradeMetadata newGrade = new()
                {
                    Grade = int.Parse(grade.Attributes["value"].Value),
                    Condition = long.Parse(condition.Attributes["value"].Value),
                    ConditionType = condition.Attributes["type"].Value,
                    ConditionCodes = condition.Attributes["code"].Value.Split(","),
                    ConditionTargets = condition.Attributes["target"].Value.Split(","),
                    RewardType = type,
                    RewardCode = int.Parse(reward.Attributes["code"].Value),
                    RewardValue = int.Parse(reward.Attributes["value"].Value)
                };

                newTrophy.Grades.Add(newGrade);
            }

            trophyList.Add(newTrophy);
        }

        return trophyList;
    }
}
