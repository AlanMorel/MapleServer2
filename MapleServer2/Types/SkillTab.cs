using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Data.Static;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class SkillTab
    {
        public long Id { get; set; }
        public string Name { get; private set; }
        public List<int> Order { get; private set; }
        public Dictionary<int, SkillMetadata> SkillJob { get; private set; }
        public Dictionary<int, int> SkillLevels { get; private set; }  // TODO: fill using database

        public SkillTab(Job job)
        {
            Id = 0x000032DF995949B9; // temporary hard coded id
            Name = "Build";
            Order = SkillTreeOrdered.GetListOrdered(job);
            SkillJob = AddOnDictionary(job);
            SkillLevels = SkillJob.ToDictionary(x => x.Key, x => (int) x.Value.Learned);
        }

        public static Dictionary<int, SkillMetadata> AddOnDictionary(Job job)
        {
            Dictionary<int, SkillMetadata> skillJob = new Dictionary<int, SkillMetadata>();

            foreach (SkillMetadata skill in SkillMetadataStorage.GetJobSkills(job))
            {
                skillJob[skill.SkillId] = skill;
            }
            return skillJob;
        }

        public void AddOrUpdate(int id, short level, byte learned)
        {
            SkillLevels[id] = level;
            foreach (int sub in SkillJob[id].SubSkills)
            {
                SkillLevels[sub] = level;
            }
        }

        public void Rename(string name)
        {
            Name = name;
        }

        public static List<SkillMetadata> GetJobFeatureSkills(Job job) => SkillMetadataStorage.GetJobSkills(job);

        public override string ToString() => $"SkillTab(Id:{Id},Name:{Name},Skills:{string.Join(",", SkillJob)})";
    }
}
