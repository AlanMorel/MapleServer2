using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SkillMetadataStorage
{
    private static readonly Dictionary<int, SkillMetadata> Skills = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Skill}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<SkillMetadata> skillList = Serializer.Deserialize<List<SkillMetadata>>(stream);
        foreach (SkillMetadata skills in skillList)
        {
            Skills[skills.SkillId] = skills;
        }
    }

    public static SkillMetadata GetSkill(int id) => Skills.GetValueOrDefault(id);

    public static List<int> GetEmotes() => Skills.Values.Where(x => x.SkillId / 100000 == 902).Select(x => x.SkillId).ToList();

    // Get a List of Skills corresponding to the Job
    public static IEnumerable<SkillMetadata> GetJobSkills(Job job)
    {
        List<SkillMetadata> skillMetadatas = new();

        if (Job.GameMaster == job)
        {
            return GameMasterSkills.Select(skillId => Skills[skillId]);
        }

        List<JobSkillMetadata> jobSkills = JobMetadataStorage.GetJobSkills(job);
        if (jobSkills is null)
        {
            return skillMetadatas;
        }

        foreach (JobSkillMetadata jobSkill in jobSkills)
        {
            SkillMetadata skillMetadata = GetSkill(jobSkill.SkillId);
            if (skillMetadata is null)
            {
                continue;
            }

            skillMetadatas.Add(skillMetadata);
        }

        return skillMetadatas;
    }

    public static bool IsPassive(int skillId) => GetSkill(skillId)?.Type == SkillType.Passive;

    private static readonly List<int> GameMasterSkills = new()
    {
        20000001,
        20000011,
        19900001,
        19900011,
        19900021,
        19900032,
        19900042,
        19900052,
        19900061,
        19999991
    };
}
