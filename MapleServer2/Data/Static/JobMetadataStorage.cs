using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class JobMetadataStorage
    {
        private static readonly Dictionary<int, JobMetadata> jobs = new Dictionary<int, JobMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-job-metadata");
            List<JobMetadata> jobList = Serializer.Deserialize<List<JobMetadata>>(stream);
            foreach (JobMetadata job in jobList)
            {
                jobs[job.JobId] = job;
            }
        }

        public static JobMetadata GetJobMetadata(int jobId) => jobs.GetValueOrDefault(jobId);

        public static List<TutorialItemMetadata> GetTutorialItems(int jobId) => jobs.GetValueOrDefault(jobId).TutorialItems;

        public static List<JobLearnedSkillsMetadata> GetLearnedSkills(int jobId) => jobs.GetValueOrDefault(jobId).LearnedSkills;

        public static List<JobSkillMetadata> GetJobskills(int jobId) => jobs.GetValueOrDefault(jobId).Skills;

        public static int GetStartMapId(int jobId) => jobs.GetValueOrDefault(jobId).StartMapId;
    }
}
