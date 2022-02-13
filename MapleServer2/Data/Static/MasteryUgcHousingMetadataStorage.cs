using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryUgcHousingMetadataStorage
{
    private static readonly Dictionary<byte, MasteryUgcHousingMetadata> MasteryMetadata = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-mastery-ugc-housing-metadata");
        List<MasteryUgcHousingMetadata> masteryMetadatas = Serializer.Deserialize<List<MasteryUgcHousingMetadata>>(stream);
        foreach (MasteryUgcHousingMetadata metadata in masteryMetadatas)
        {
            MasteryMetadata[metadata.Grade] = metadata;
        }
    }

    public static MasteryUgcHousingMetadata GetMetadata(byte grade)
    {
        MasteryMetadata.TryGetValue(grade, out MasteryUgcHousingMetadata metadata);
        return metadata;
    }
}
