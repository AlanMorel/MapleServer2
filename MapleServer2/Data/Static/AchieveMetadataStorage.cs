using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class AchieveMetadataStorage
    {
        private static readonly Dictionary<int, AchieveMetadata> achieves = new Dictionary<int, AchieveMetadata>();

        static AchieveMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-achieve-metadata");
            List<AchieveMetadata> achieveList = Serializer.Deserialize<List<AchieveMetadata>>(stream);
            foreach (AchieveMetadata achieve in achieveList)
            {
                achieves[achieve.Id] = achieve;
            }
        }

        public static List<int> GetAchieveIds()
        {
            return new List<int>(achieves.Keys);
        }

        public static AchieveMetadata GetAchieve(int id)
        {
            return achieves.GetValueOrDefault(id);
        }

        // return condition needed to reach that grade, -1 if grade invalid
        public static long GetCondition(int id, int grade)
        {
            if (grade > GetNumGrades(id))
                return -1;
            return achieves.GetValueOrDefault(id).Grades[grade - 1].Condition;
        }

        public static int GetNumGrades(int id)
        {
            return achieves.GetValueOrDefault(id).Grades.Count;
        }
    }
}
