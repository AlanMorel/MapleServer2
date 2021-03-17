using System.Collections.Generic;
using System.IO;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class TrophyMetadataStorage
    {
        private static readonly Dictionary<int, TrophyMetadata> map = new Dictionary<int, TrophyMetadata>();

        static TrophyMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-trophy-metadata");
            List<TrophyMetadata> trophies = Serializer.Deserialize<List<TrophyMetadata>>(stream);
            foreach (TrophyMetadata trophy in trophies)
            {
                map[trophy.Id] = trophy;
            }
        }

        public static List<int> GetTrophyIds()
        {
            return new List<int>(map.Keys);
        }

        public static TrophyMetadata GetMetadata(int id)
        {
            return map.GetValueOrDefault(id);
        }

        public static TrophyGradeMetadata GetGrade(int id, int grade)
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
