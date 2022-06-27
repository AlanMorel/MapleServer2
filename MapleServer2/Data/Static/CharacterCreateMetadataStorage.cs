using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class CharacterCreateMetadataStorage
{
    private static CharacterCreateMetadata CharacterCreate = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.CharacterCreate);
        List<CharacterCreateMetadata> items = Serializer.Deserialize<List<CharacterCreateMetadata>>(stream);
        foreach (CharacterCreateMetadata item in items)
        {
            CharacterCreate = item; // there's only one entry in this metadata
        }
    }

    public static bool JobIsDisabled(int jobId)
    {
        return CharacterCreate.DisabledJobs.Contains(jobId);
    }
}
