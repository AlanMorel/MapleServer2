using System.Collections.Generic;
using System.Linq;

namespace MapleServer2.Types
{
    public class SkillTab
    {
        public long Id { get; set; }
        public string Name { get; private set; }

        private Dictionary<int, Skill> Skills;

        public SkillTab(string name, Dictionary<int, Skill> skills = null)
        {
            this.Id = 0x000032DF995949B9; // temporary hard coded id
            this.Name = name;
            this.Skills = (skills != null && skills.Any()) ? skills : new Dictionary<int, Skill>();

            // Add default skills
            AddOrUpdate(Skill.skill(20000001, 1, 1)); // Swift Swimming
            AddOrUpdate(Skill.skill(20000011, 1, 1)); // Wall Climbing
        }

        public void AddOrUpdate(Skill skill)
        {
            // Add or update skill
            if (Skills.ContainsKey(skill.Id)) Skills[skill.Id] = skill;
            else Skills.Add(skill.Id, skill);

            // Recursive add or update for sub skills
            if (skill.Sub != null) foreach (int sub in skill.Sub) AddOrUpdate(Skill.skill(sub, skill.Level, skill.Learned, skill.Feature));
        }

        public void Rename(string name)
        {
            this.Name = name;
        }

        public void SetSkills(Dictionary<int, Skill> skills)
        {
            this.Skills = skills;
        }

        public Dictionary<int, Skill> GetSkills()
        {
            return this.Skills;
        }

        public List<Skill> GetJobFeatureSkills(string feature = "")
        {
            List<Skill> jobFeatureSkills = new List<Skill>();
            foreach (KeyValuePair<int, Skill> skill in Skills)
            {
                if (skill.Value.Feature.ToLower().Equals(feature.ToLower()) && (skill.Value.Id > 10000000 && skill.Value.Id < 20000000))
                {
                    jobFeatureSkills.Add(skill.Value);
                }
            }
            return jobFeatureSkills;
        }

        public override string ToString() => $"SkillTab(Id:{Id},Name:{Name},Skills:{string.Join(",", Skills)})";
    }
}