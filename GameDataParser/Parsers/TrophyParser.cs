using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class TrophyParser : Exporter<List<TrophyMetadata>>
{
    public TrophyParser(MetadataResources resources) : base(resources, MetadataName.Trophy) { }

    protected override List<TrophyMetadata> Parse()
    {
        List<TrophyMetadata> trophyList = new();

        // Parse trophy names
        Dictionary<int, string> trophyNames = new();
        PackFileEntry? file = Resources.XmlReader.Files.FirstOrDefault(x => x.Name.StartsWith("string/en/achievename.xml"));
        XmlDocument stringDoc = Resources.XmlReader.GetXmlDocument(file);
        if (stringDoc.DocumentElement?.ChildNodes is null)
        {
            return trophyList;
        }

        foreach (XmlNode node in stringDoc.DocumentElement.ChildNodes)
        {
            if (ParserHelper.CheckForNull(node, "id", "name"))
            {
                continue;
            }

            int id = int.Parse(node.Attributes!["id"]!.Value);
            string name = node.Attributes["name"]!.Value;
            trophyNames[id] = name;
        }

        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("achieve/"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            XmlNode? trophy = document.SelectSingleNode("/ms2/achieves");

            if (ParserHelper.CheckForNull(trophy, "id", "account"))
            {
                continue;
            }

            int id = int.Parse(trophy!.Attributes!["id"]!.Value);
            TrophyMetadata newTrophy = new()
            {
                Id = id,
                Categories = trophy.Attributes["categoryTag"]?.Value.Split(","),
                AccountWide = trophy.Attributes["account"]!.Value == "1"
            };
            trophyNames.TryGetValue(id, out newTrophy.Name);

            XmlNodeList? grades = trophy.SelectNodes("grade");
            if (grades is null)
            {
                continue;
            }

            foreach (XmlNode grade in grades)
            {
                XmlNode? condition = grade.SelectSingleNode("condition");
                XmlNode? reward = grade.SelectSingleNode("reward");
                if (ParserHelper.CheckForNull(reward, "type", "code", "value", "rank"))
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(condition, "type", "code", "value", "target"))
                {
                    continue;
                }

                if (ParserHelper.CheckForNull(grade, "value"))
                {
                    continue;
                }

                Enum.TryParse(reward!.Attributes!["type"]!.Value, true, out RewardType type);

                if (string.IsNullOrEmpty(newTrophy.ConditionType) || string.IsNullOrEmpty(newTrophy.ConditionCodes))
                {
                    newTrophy.ConditionType = condition!.Attributes!["type"]!.Value;
                    newTrophy.ConditionCodes = condition.Attributes["code"]!.Value;
                }

                TrophyGradeMetadata newGrade = new()
                {
                    Grade = int.Parse(grade.Attributes!["value"]!.Value),
                    Condition = long.Parse(condition!.Attributes!["value"]!.Value),
                    ConditionTargets = condition.Attributes["target"]!.Value,
                    RewardType = type,
                    RewardCode = int.Parse(reward.Attributes["code"]!.Value),
                    RewardValue = int.Parse(reward.Attributes["value"]!.Value),
                    RewardRank = int.Parse(reward.Attributes["rank"]!.Value)
                };

                int.TryParse(reward.Attributes["subJobLevel"]?.Value, out newGrade.RewardSubJobLevel);

                newTrophy.Grades.Add(newGrade);
            }

            trophyList.Add(newTrophy);
        }

        return trophyList;
    }
}
