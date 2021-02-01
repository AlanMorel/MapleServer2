using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class AchieveParser : Exporter<List<AchieveMetadata>>
    {
        public AchieveParser(MetadataResources resources) : base(resources, "achieve") { }

        protected override List<AchieveMetadata> Parse()
        {
            List<AchieveMetadata> achieveList = new List<AchieveMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("achieve/"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNode achieve = document.SelectSingleNode("/ms2/achieves");

                int id = int.Parse(achieve.Attributes["id"].Value);
                AchieveMetadata newAchieve = new AchieveMetadata();
                newAchieve.Id = id;
                
                XmlNodeList grades = achieve.SelectNodes("grade");

                foreach (XmlNode grade in grades)
                {
                    AchieveGradeMetadata newGrade = new AchieveGradeMetadata();
                    newGrade.Grade = int.Parse(grade.Attributes["value"].Value);

                    XmlNode condition = grade.SelectSingleNode("condition");
                    newGrade.Condition = long.Parse(condition.Attributes["value"].Value);

                    XmlNode reward = grade.SelectSingleNode("reward");
                    newGrade.Reward = int.Parse(reward.Attributes["value"].Value);

                    newAchieve.Grades.Add(newGrade);
                }
                achieveList.Add(newAchieve);
            }

            return achieveList;
        }
    }
}
