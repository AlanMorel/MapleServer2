using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryFactorMetadataStorage
{
    private static readonly Dictionary<int, MasteryFactorMetadata> MasteryFactorMetadatas = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.MasteryFactor}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<MasteryFactorMetadata> masteryFactors = Serializer.Deserialize<List<MasteryFactorMetadata>>(stream);
        foreach (MasteryFactorMetadata masteryFactor in masteryFactors)
        {
            MasteryFactorMetadatas[masteryFactor.Differential] = masteryFactor;
        }
    }

    public static List<int> GetMasteryFactorIds()
    {
        return new(MasteryFactorMetadatas.Keys);
    }

    public static int GetFactor(int id)
    {
        return MasteryFactorMetadatas.GetValueOrDefault(id).Factor;
    }
}
