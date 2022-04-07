using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryFactorMetadataStorage
{
    private static readonly Dictionary<int, MasteryFactorMetadata> MasteryFactorMetadatas = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.MasteryFactor);
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
