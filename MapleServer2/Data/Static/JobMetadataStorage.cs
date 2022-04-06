﻿using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class JobMetadataStorage
{
    private static readonly Dictionary<int, JobMetadata> Jobs = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Job}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<JobMetadata> jobList = Serializer.Deserialize<List<JobMetadata>>(stream);
        foreach (JobMetadata job in jobList)
        {
            Jobs[job.JobId] = job;
        }
    }

    public static JobMetadata GetJobMetadata(Job job)
    {
        return Jobs.GetValueOrDefault((int) job);
    }

    public static List<TutorialItemMetadata> GetTutorialItems(Job job) => GetJobMetadata(job).TutorialItems;

    public static List<JobLearnedSkillsMetadata> GetLearnedSkills(Job job) => GetJobMetadata(job).LearnedSkills;

    public static List<JobSkillMetadata> GetJobSkills(Job job) => GetJobMetadata(job).Skills;

    public static int GetStartMapId(Job job) => GetJobMetadata(job).StartMapId;
}
