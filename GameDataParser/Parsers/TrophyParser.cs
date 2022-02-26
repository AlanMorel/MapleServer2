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

        // Parse trophy names
        Dictionary<int, string> trophyNames = new();
        PackFileEntry file = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/achievename.xml"));
        XmlDocument stringDoc = Resources.XmlReader.GetXmlDocument(file);
        foreach (XmlNode node in stringDoc.DocumentElement.ChildNodes)
        {
            int id = int.Parse(node.Attributes["id"].Value);
            string name = node.Attributes["name"].Value;
            trophyNames[id] = name;
        }

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("achieve/"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNode trophy = document.SelectSingleNode("/ms2/achieves");

            int id = int.Parse(trophy.Attributes["id"].Value);
            TrophyMetadata newTrophy = new()
            {
                Id = id,
                Categories = trophy.Attributes["categoryTag"]?.Value.Split(","),
                AccountWide = trophy.Attributes["account"].Value == "1"
            };
            trophyNames.TryGetValue(id, out newTrophy.Name);

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
                    ConditionCodes = condition.Attributes["code"].Value,
                    ConditionTargets = condition.Attributes["target"].Value,
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
