using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class JobMetadataStorage
{
    private static readonly Dictionary<int, JobMetadata> Jobs = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Job);
        List<JobMetadata> jobList = Serializer.Deserialize<List<JobMetadata>>(stream);
        foreach (JobMetadata job in jobList)
        {
            Jobs[job.JobId] = job;
        }
    }

    public static JobMetadata? GetJobMetadata(JobCode jobCode)
    {
        return Jobs.GetValueOrDefault((int) jobCode);
    }

    public static List<TutorialItemMetadata>? GetTutorialItems(JobCode jobCode) => GetJobMetadata(jobCode)?.TutorialItems;

    public static List<JobLearnedSkillsMetadata>? GetLearnedSkills(JobCode jobCode) => GetJobMetadata(jobCode)?.LearnedSkills;

    public static List<JobSkillMetadata>? GetJobSkills(JobCode jobCode) => GetJobMetadata(jobCode)?.Skills;

    public static int? GetStartMapId(JobCode jobCode) => GetJobMetadata(jobCode)?.StartMapId;
}
