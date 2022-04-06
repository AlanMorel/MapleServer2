using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryMetadataStorage
{
    private static readonly Dictionary<int, MasteryMetadata> Masteries = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Mastery}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<MasteryMetadata> masteryList = Serializer.Deserialize<List<MasteryMetadata>>(stream);
        foreach (MasteryMetadata mastery in masteryList)
        {
            Masteries[mastery.Type] = mastery;
        }
    }

    public static List<int> GetMasteryTypes()
    {
        return new(Masteries.Keys);
    }

    public static MasteryMetadata GetMastery(int type)
    {
        return Masteries.GetValueOrDefault(type);
    }

    public static int GetGradeFromXP(MasteryType type, long xp)
    {
        return GetMastery((int) type).Grades.Count(x => x.Value <= xp);
    }
}
