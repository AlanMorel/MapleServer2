using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using System.Reflection;

namespace MapleServer2.Types
{
    public class SkillTab
    {
        public long Id { get; set; }
        public string Name { get; private set; }
        public int[] Order { get; private set; }
        public byte Split { get; private set; }
        public List<SkillMetadata> SkillsM { get; private set; }
        public Dictionary<int, SkillMetadata> SkillJob { get; set; }

        public SkillTab(Job job)
        {
            Id = 0x000032DF995949B9; // temporary hard coded id
            Name = "Build";
            Split = SkillTreeOrdered.GetListOrderedSplit(job);
            SkillsM = SkillMetadataStorage.GetJobSkills(job);
            Order = SkillTreeOrdered.GetListOrdered(job);
            SkillJob = AddOnDictionary();
        }

        public Dictionary<int, SkillMetadata> AddOnDictionary()
        {
            Dictionary<int, SkillMetadata> skillJob = new Dictionary<int, SkillMetadata>();
            foreach (SkillMetadata skill in SkillsM)
            {
                skillJob[skill.SkillId] = skill;
            }
            return skillJob;
        }

        public void AddOrUpdate(int id, short level, byte learned)
        {
            SkillJob[id].Learned = learned;
            SkillJob[id].SkillLevel.Level = level;
        }

        public void Rename(string name)
        {
            this.Name = name;
        }

        public List<SkillMetadata> GetJobFeatureSkills(Job job)
        {
            return SkillMetadataStorage.GetJobSkills(job);
        }

        public override string ToString() => $"SkillTab(Id:{Id},Name:{Name},Skills:{string.Join(",", SkillJob)})";
    }
}
