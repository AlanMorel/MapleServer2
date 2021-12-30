using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class TrophyMetadataStorage
{
    private static readonly Dictionary<int, TrophyMetadata> Trophies = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-trophy-metadata");
        List<TrophyMetadata> trophies = Serializer.Deserialize<List<TrophyMetadata>>(stream);
        foreach (TrophyMetadata trophy in trophies)
        {
            Trophies[trophy.Id] = trophy;
        }
    }

    public static IEnumerable<TrophyMetadata> GetTrophiesByCondition(string type, string code, string target)
    {
        foreach (TrophyMetadata x in Trophies.Values)
        {
            if (x.Grades[0].ConditionType != type)
            {
                continue;
            }

            if (x.Grades[0].ConditionTargets.Length != 0 && !x.Grades[0].ConditionTargets.Contains(target))
            {
                continue;
            }

            if (x.Grades[0].ConditionCodes[0].Contains('-'))
            {
                if (IsInConditionRange(x.Grades[0].ConditionCodes[0], code))
                {
                    yield return x;
                }
            }
            else
            {
                if (x.Grades[0].ConditionCodes.Length == 0 || x.Grades[0].ConditionCodes.Contains(code))
                {
                    yield return x;
                }
            }
        }
    }
    
    public static IEnumerable<TrophyMetadata> GetTrophiesByType(string type)
        => Trophies.Values.Where(m => m.Grades.Any(g => g.ConditionType == type));

    public static TrophyMetadata GetMetadata(int id) => Trophies.GetValueOrDefault(id);

    public static bool IsInConditionRange(string trophyCondition, string condition)
    {
        string[] parts = trophyCondition.Split('-');
        if (!long.TryParse(condition, out long conditionValue))
        {
            return false;
        }

        if (!long.TryParse(parts[0], out long lowerBound))
        {
            return false;
        }

        if (!long.TryParse(parts[1], out long upperBound))
        {
            return false;
        }

        return conditionValue >= lowerBound && conditionValue <= upperBound;
    }
}
