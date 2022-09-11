using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SurvivaLevelMetadataStorage
{
    private static readonly Dictionary<int, SurvivalLevelMetadata> SurvivalLevelMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.SurvivalLevel);
        List<SurvivalLevelMetadata> items = Serializer.Deserialize<List<SurvivalLevelMetadata>>(stream);
        foreach (SurvivalLevelMetadata item in items)
        {
            SurvivalLevelMetadatas[item.Level] = item;
        }
    }

    public static long GetExpToNextLevel(int level)
    {
        return LevelExist(level + 1) ? SurvivalLevelMetadatas[level + 1].RequiredExp : 0;
    }

    public static bool LevelExist(int level)
    {
        return SurvivalLevelMetadatas.ContainsKey(level);
    }
}
