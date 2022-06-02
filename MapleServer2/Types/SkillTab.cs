using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types;

public class SkillTab
{
    public long Uid { get; private set; }
    public long TabId { get; set; }
    public string Name { get; set; }

    public Dictionary<int, SkillMetadata> SkillJob { get; private set; }
    public Dictionary<int, short> SkillLevels { get; private set; }

    public SkillTab() { }

    public SkillTab(long characterId, Job job, JobCode jobCode, long id, string name)
    {
        Name = name;
        ResetSkillTree(job, jobCode);
        TabId = id;
        Uid = DatabaseManager.SkillTabs.Insert(this, characterId);
    }

    public SkillTab(string name, int jobId, long tabId, long uid, Dictionary<int, short> skillLevels)
    {
        Name = name;
        TabId = tabId;
        Uid = uid;
        SkillJob = GetSkillsMetadata((Job) jobId);
        SkillLevels = skillLevels;
    }

    public void AddOrUpdate(int id, short level, bool isLearned)
    {
        SkillLevels[id] = isLearned ? level : (short) 0;
        if (!SkillJob.ContainsKey(id))
        {
            return;
        }

        foreach (int sub in SkillJob[id].SubSkills)
        {
            SkillLevels[sub] = isLearned ? level : (short) 0;
        }
    }

    public void ResetSkillTree(Job job, JobCode jobCode)
    {
        SkillJob = GetSkillsMetadata(job);
        if (job is Job.GameMaster)
        {
            SkillLevels = SkillJob.Keys.ToDictionary(skillId => skillId, _ => (short) 1);
            return;
        }

        SkillLevels = SkillJob.Keys.ToDictionary(skillId => skillId, _ => (short) 0);
        LearnDefaultSkills();

        void LearnDefaultSkills()
        {
            JobMetadata jobMetadata = JobMetadataStorage.GetJobMetadata(job);
            if (jobMetadata is null)
            {
                return;
            }

            List<int> skillIds = new();
            jobMetadata.LearnedSkills.ForEach(x => skillIds.AddRange(x.SkillIds));

            foreach (int skillId in skillIds)
            {
                JobSkillMetadata jobSkillMetadata = jobMetadata.Skills.First(x => x.SkillId == skillId);
                if (jobSkillMetadata.SubJobCode != (int) jobCode && jobSkillMetadata.SubJobCode != 0)
                {
                    continue;
                }

                AddOrUpdate(skillId, 1, true);
            }
        }
    }

    /// <summary>
    /// Returns all skills by type.
    /// </summary>
    /// <param name="type"><see cref="SkillType"/></param>
    /// <returns>List of skill id and skill level</returns>
    public List<(int skillId, short skillLevel)> GetSkillsByType(SkillType type)
    {
        List<(int, short)> skills = new();
        foreach ((int skillId, SkillMetadata metadata) in SkillJob.Where(x => x.Value.Type == type))
        {
            short level = SkillLevels.GetValueOrDefault(skillId);
            skills.Add((skillId, level));
            skills.AddRange(metadata.SubSkills.Select(metadataSubSkill => (metadataSubSkill, level)));
        }

        return skills;
    }

    private static Dictionary<int, SkillMetadata> GetSkillsMetadata(Job job)
    {
        return SkillMetadataStorage.GetJobSkills(job).ToDictionary(x => x.SkillId, x => x);
    }
}
