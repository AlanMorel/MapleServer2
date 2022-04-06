using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SurvivaLevelMetadataStorage
{
    private static readonly Dictionary<int, SurvivalLevelMetadata> SurvivalLevelMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.SurvivalLevel}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<SurvivalLevelMetadata> items = Serializer.Deserialize<List<SurvivalLevelMetadata>>(stream);
        foreach (SurvivalLevelMetadata item in items)
        {
            SurvivalLevelMetadatas[item.Level] = item;
        }
    }

    public static long GetExpToNextLevel(int level)
    {
        return LevelExist(level + 1) ? SurvivalLevelMetadatas.GetValueOrDefault(level + 1).RequiredExp : 0;
    }

    public static bool LevelExist(int level)
    {
        return SurvivalLevelMetadatas.ContainsKey(level);
    }
}
