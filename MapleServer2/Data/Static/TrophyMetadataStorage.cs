using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TrophyMetadataStorage
{
    private static readonly Dictionary<int, TrophyMetadata> Trophies = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Trophy}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<TrophyMetadata> trophies = Serializer.Deserialize<List<TrophyMetadata>>(stream);
        foreach (TrophyMetadata trophy in trophies)
        {
            Trophies[trophy.Id] = trophy;
        }
    }

    public static IEnumerable<TrophyMetadata> GetTrophiesByType(string type)
        => Trophies.Values.Where(m => m.Grades.Any(g => g.ConditionType == type));

    public static TrophyMetadata GetMetadata(int id) => Trophies.GetValueOrDefault(id);

    public static IEnumerable<TrophyMetadata> GetAll() => Trophies.Values;
}
