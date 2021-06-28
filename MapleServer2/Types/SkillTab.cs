using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class SkillTab
    {
        public long Uid { get; private set; }
        public long TabId { get; set; }
        public string Name { get; set; }

        public Player Player;

        public List<int> Order { get; private set; }
        public Dictionary<int, SkillMetadata> SkillJob { get; private set; }
        public Dictionary<int, int> SkillLevels { get; private set; }

        public SkillTab() { }

        public SkillTab(Player player, Job job)
        {
            Name = $"Build {(player.SkillTabs == null ? "1" : player.SkillTabs.Count + 1)}";
            ResetSkillTree(job);
            Player = player;
            TabId = player.CharacterId;
            Uid = DatabaseManager.AddSkillTab(this);
        }

        public SkillTab(Player player, Job job, long id, string name)
        {
            Name = name;
            ResetSkillTree(job);
            Player = player;
            TabId = id;
            Uid = DatabaseManager.AddSkillTab(this);
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

        public void AddOrUpdate(int id, short level, bool isLearned)
        {
            SkillLevels[id] = isLearned ? level : 0;
            if (!SkillJob.ContainsKey(id))
            {
                return;
            }

            foreach (int sub in SkillJob[id].SubSkills)
            {
                SkillLevels[sub] = isLearned ? level : 0;
            }
        }

        public void GenerateSkills(Job job)
        {
            Order = SkillTreeOrdered.GetListOrdered(job);
            SkillJob = AddOnDictionary(job);
        }

        public void ResetSkillTree(Job job)
        {
            GenerateSkills(job);
            SkillLevels = SkillJob.ToDictionary(x => x.Key, x => (int) x.Value.Learned);
        }

        public static List<SkillMetadata> GetJobFeatureSkills(Job job) => SkillMetadataStorage.GetJobSkills(job);

        public override string ToString() => $"SkillTab(Id:{Uid},Name:{Name},Skills:{string.Join(",", SkillJob)})";
    }
}
