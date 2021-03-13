using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class AchieveMetadataStorage
    {
        private static readonly Dictionary<int, AchieveMetadata> map = new Dictionary<int, AchieveMetadata>();

        static AchieveMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-achieve-metadata");
            List<AchieveMetadata> achieves = Serializer.Deserialize<List<AchieveMetadata>>(stream);
            foreach (AchieveMetadata achieve in achieves)
            {
                map[achieve.Id] = achieve;
            }
        }

        public static List<int> GetAchieveIds()
        {
            return new List<int>(map.Keys);
        }

        public static AchieveMetadata GetMetadata(int id)
        {
            return map.GetValueOrDefault(id);
        }

        public static AchieveGradeMetadata GetGrade(int id, int grade)
        {
            if ((grade < 1) || (grade > GetNumGrades(id)))
            {
                return null;
            }
            return map.GetValueOrDefault(id).Grades[grade - 1];
        }

        public static int GetNumGrades(int id)
        {
            return map.GetValueOrDefault(id).Grades.Count;
        }
    }
}
