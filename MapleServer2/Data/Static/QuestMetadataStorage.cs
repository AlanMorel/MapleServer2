using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class QuestMetadataStorage
{
    private static readonly Dictionary<int, QuestMetadata> Quests = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Quest);
        List<QuestMetadata> items = Serializer.Deserialize<List<QuestMetadata>>(stream);
        foreach (QuestMetadata item in items)
        {
            Quests[item.Basic.Id] = item;
        }
    }

    public static QuestMetadata GetMetadata(int questId) => Quests.GetValueOrDefault(questId);

    public static List<QuestMetadata> GetAvailableQuests(int level, Job job)
    {
        // TODO: Check achievement
        return Quests.Values.Where(questMetadata => questMetadata.Require.Level <= level
                                                 && (questMetadata.Require.Job.Contains((short) job) || questMetadata.Require.Job.Count == 0)
                                                 && questMetadata.Require.RequiredQuests.Count == 0
                                                 && questMetadata.Basic.QuestType is QuestType.Epic or QuestType.World)
            .ToList();
    }

    public static Dictionary<int, QuestMetadata> GetAllQuests() => Quests;

    public static bool IsValid(int questId) => Quests.ContainsKey(questId);
}
