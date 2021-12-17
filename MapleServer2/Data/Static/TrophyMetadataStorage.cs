using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TrophyMetadataStorage
{
    private static readonly Dictionary<int, TrophyMetadata> map = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-trophy-metadata");
        List<TrophyMetadata> trophies = Serializer.Deserialize<List<TrophyMetadata>>(stream);
        foreach (TrophyMetadata trophy in trophies)
        {
            map[trophy.Id] = trophy;
        }
    }

    public static IEnumerable<TrophyMetadata> GetTrophiesByCondition(string type, string code, string target) => map.Values.Where(x =>
        x.Grades[0].ConditionType == type &&
        (x.Grades[0].ConditionCodes.Length == 0 || x.Grades[0].ConditionCodes.Contains(code)) &&
        (x.Grades[0].ConditionTargets.Length == 0 || x.Grades[0].ConditionTargets.Contains(target)));

    public static TrophyMetadata GetMetadata(int id) => map.GetValueOrDefault(id);
}
