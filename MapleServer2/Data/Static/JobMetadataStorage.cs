using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class JobMetadataStorage
{
    private static readonly Dictionary<int, JobMetadata> jobs = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-job-metadata");
        List<JobMetadata> jobList = Serializer.Deserialize<List<JobMetadata>>(stream);
        foreach (JobMetadata job in jobList)
        {
            jobs[job.JobId] = job;
        }
    }

    public static JobMetadata GetJobMetadata(int jobId)
    {
        return jobs.GetValueOrDefault(jobId);
    }

    public static List<TutorialItemMetadata> GetTutorialItems(int jobId)
    {
        return jobs.GetValueOrDefault(jobId).TutorialItems;
    }

    public static List<JobLearnedSkillsMetadata> GetLearnedSkills(int jobId)
    {
        return jobs.GetValueOrDefault(jobId).LearnedSkills;
    }

    public static List<JobSkillMetadata> GetJobskills(int jobId)
    {
        return jobs.GetValueOrDefault(jobId).Skills;
    }

    public static int GetStartMapId(int jobId)
    {
        return jobs.GetValueOrDefault(jobId).StartMapId;
    }
}
