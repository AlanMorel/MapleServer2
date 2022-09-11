using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FieldWarMetadataStorage
{
    private static readonly Dictionary<int, FieldWarMetadata> FieldWar = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.FieldWar);
        List<FieldWarMetadata> metadatas = Serializer.Deserialize<List<FieldWarMetadata>>(stream);
        foreach (FieldWarMetadata metadata in metadatas)
        {
            FieldWar[metadata.FieldWarId] = metadata;
        }
    }

    public static FieldWarMetadata? GetMetadata(int fieldWarId)
    {
        return FieldWar.GetValueOrDefault(fieldWarId);
    }

    public static int? MapId(int fieldWarId)
    {
        return GetMetadata(fieldWarId)?.MapId;
    }
}
