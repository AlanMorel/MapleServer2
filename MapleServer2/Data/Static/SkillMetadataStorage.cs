using Maple2Storage.Types;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class SkillMetadataStorage
    {
        private static readonly Dictionary<int, List<SkillLevel>> skillLevel = new Dictionary<int, List<SkillLevel>>();

        static SkillMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-skill-metadata");
            List<SkillMetadata> skillList = Serializer.Deserialize<List<SkillMetadata>>(stream);
            foreach (SkillMetadata skills in skillList)
            {
                skillLevel.Add(skills.SkillId, skills.SkillLevel);
            }
        }

        // TODO: Get Skills from JobId

        public static IEnumerable<SkillLevel> GetSkills(int level)
        {
            return skillLevel.GetValueOrDefault(level);
        }
    }
}
