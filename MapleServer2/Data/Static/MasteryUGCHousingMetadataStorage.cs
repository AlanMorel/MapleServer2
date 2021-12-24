using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MasteryUGCHousingMetadataStorage
{
    private static readonly Dictionary<byte, MasteryUGCHousingMetadata> MasteryMetadata = new();

    public static void Init()
    {
        using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-mastery-ugc-housing-metadata");
        List<MasteryUGCHousingMetadata> masteryMetadatas = Serializer.Deserialize<List<MasteryUGCHousingMetadata>>(stream);
        foreach (MasteryUGCHousingMetadata metadata in masteryMetadatas)
        {
            MasteryMetadata[metadata.Grade] = metadata;
        }
    }

    public static MasteryUGCHousingMetadata GetMetadata(byte grade)
    {
        MasteryMetadata.TryGetValue(grade, out MasteryUGCHousingMetadata metadata);
        return metadata;
    }
}
