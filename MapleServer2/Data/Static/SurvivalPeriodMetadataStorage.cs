using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class SurvivalPeriodMetadataStorage
{
    private static readonly Dictionary<int, SurvivalPeriodMetadata> SurvivalPeriodMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.SurvivalPeriod}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<SurvivalPeriodMetadata> items = Serializer.Deserialize<List<SurvivalPeriodMetadata>>(stream);
        foreach (SurvivalPeriodMetadata item in items)
        {
            SurvivalPeriodMetadatas[item.Id] = item;
        }
    }

    public static SurvivalPeriodMetadata GetMetadata()
    {
        // Currently set to get the first value. Normally it should be getting the value that is in between start and end time. The XML needs to be edited first to properly grab it, as the current End Date is already passed.
        // return SurvivalPeriodMetadatas.Values.FirstOrDefault(x => x.StartTime <= DateTime.UtcNow && x.EndTime >= DateTime.UtcNow);
        return SurvivalPeriodMetadatas.Values.FirstOrDefault();
    }
}
