using System;
using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;
using Maple2Storage.Enums;

namespace GameDataParser.Parsers
{
    class TrophyParser : Exporter<List<TrophyMetadata>>
    {
        public TrophyParser(MetadataResources resources) : base(resources, "trophy") { }

        protected override List<TrophyMetadata> Parse()
        {
            HashSet<string> test = new HashSet<string>();
            List<TrophyMetadata> trophyList = new List<TrophyMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("achieve/"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNode trophy = document.SelectSingleNode("/ms2/achieves");

                TrophyMetadata newTrophy = new TrophyMetadata();
                newTrophy.Id = int.Parse(trophy.Attributes["id"].Value);
                newTrophy.Categories = trophy.Attributes["categoryTag"]?.Value.Split(",");

                XmlNodeList grades = trophy.SelectNodes("grade");

                foreach (XmlNode grade in grades)
                {
                    TrophyGradeMetadata newGrade = new TrophyGradeMetadata();
                    newGrade.Grade = int.Parse(grade.Attributes["value"].Value);

                    XmlNode condition = grade.SelectSingleNode("condition");
                    newGrade.Condition = long.Parse(condition.Attributes["value"].Value);

                    XmlNode reward = grade.SelectSingleNode("reward");
                    Enum.TryParse(reward.Attributes["type"].Value, true, out RewardType type);
                    newGrade.RewardType = (byte) type;
                    newGrade.RewardCode = int.Parse(reward.Attributes["code"].Value);
                    newGrade.RewardValue = int.Parse(reward.Attributes["value"].Value);

                    newTrophy.Grades.Add(newGrade);
                }
                trophyList.Add(newTrophy);
            }
            System.Console.WriteLine(string.Join(", ", test));

            return trophyList;
        }
    }
}
