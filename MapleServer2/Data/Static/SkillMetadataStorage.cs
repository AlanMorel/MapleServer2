using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants.Skills;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class SkillMetadataStorage
    {
        private static readonly Dictionary<int, SkillMetadata> skill = new Dictionary<int, SkillMetadata>();

        static SkillMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-skill-metadata");
            List<SkillMetadata> skillList = Serializer.Deserialize<List<SkillMetadata>>(stream);
            foreach (SkillMetadata skills in skillList)
            {
                skill[skills.SkillId] = skills;
            }
        }

        public static SkillMetadata GetSkill(int id)
        {
            return skill.GetValueOrDefault(id);
        }

        public static List<SkillMetadata> GetJobSkills(Job job = Job.None)
        {
            List<SkillMetadata> jobSkill = new List<SkillMetadata>();

            foreach (KeyValuePair<int, SkillMetadata> skills in skill)
            {
                if (skills.Value.Job == (int) job)
                {
                    jobSkill.Add(skills.Value);
                    List<int> defaultSkills = SkillTreeOrdered.GetDefaultLearnedSkill(job);
                    for (int i = 0; i < defaultSkills.Count; i++)
                    {
                        if (skills.Value.SkillId == defaultSkills[i])
                        {
                            skills.Value.Learned = 1;
                        }
                    }
                }
                else if (skills.Value.SkillId == 20000001) // Swiming
                {
                    jobSkill.Add(skills.Value);
                    skills.Value.Learned = 1;
                }
                else if (skills.Value.SkillId == 20000011) // Climbing walls
                {
                    jobSkill.Add(skills.Value);
                    skills.Value.Learned = 1;
                }
            }
            return jobSkill;
        }
    }
}
