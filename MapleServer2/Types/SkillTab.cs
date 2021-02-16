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
        public List<SkillMetadata> Skills { get; private set; }
        public Dictionary<int, SkillMetadata> SkillJob { get; set; }

        public SkillTab(Job job)
        {
            Id = 0x000032DF995949B9; // temporary hard coded id
            Name = "Build";
            Skills = SkillMetadataStorage.GetJobSkills(job);
            Order = SkillTreeOrdered.GetListOrdered(job);
            SkillJob = AddOnDictionary();
        }

        public Dictionary<int, SkillMetadata> AddOnDictionary()
        {
            Dictionary<int, SkillMetadata> skillJob = new Dictionary<int, SkillMetadata>();

            foreach (SkillMetadata skill in Skills)
            {
                skillJob[skill.SkillId] = skill;
            }
            return skillJob;
        }

        public void AddOrUpdate(int id, short level, byte learned)
        {
            SkillJob[id].Learned = learned;
            SkillJob[id].SkillLevels.Find(x => x.Level != 0).Level = level;
            if (SkillJob[id].SubSkills.Length != 0)
            {
                foreach (int sub in SkillJob[id].SubSkills.Select(x => x))
                {
                    if (SkillJob.ContainsKey(sub))
                    {
                        SkillJob[sub].Learned = learned;
                        SkillJob[sub].SkillLevels.Find(x => x.Level != 0).Level = level;
                    }
                }
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
