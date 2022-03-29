using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FieldWarMetadataStorage
{
    private static readonly Dictionary<int, FieldWarMetadata> FieldWar = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-fieldwar-metadata");
        List<FieldWarMetadata> metadatas = Serializer.Deserialize<List<FieldWarMetadata>>(stream);
        foreach (FieldWarMetadata metadata in metadatas)
        {
            FieldWar[metadata.FieldWarId] = metadata;
        }
    }

    public static IEnumerable<FieldWarMetadata> GetAll()
    {
        return FieldWar.Values;
    }

    public static bool IsValid(int fieldWarId)
    {
        return FieldWar.ContainsKey(fieldWarId);
    }

    public static FieldWarMetadata GetMetadata(int fieldWarId)
    {
        return FieldWar.GetValueOrDefault(fieldWarId);
    }

    public static int GetFieldWarId(int fieldWarId)
    {
        return GetMetadata(fieldWarId).FieldWarId;
    }

    public static int GetRewardId(int fieldWarId)
    {
        return GetMetadata(fieldWarId).RewardId;
    }

    public static int MapId(int fieldWarId)
    {
        return GetMetadata(fieldWarId).MapId;
    }
}
