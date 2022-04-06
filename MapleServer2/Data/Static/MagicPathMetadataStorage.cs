using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MagicPathMetadataStorage
{
    private static readonly Dictionary<long, MagicPathMetadata> MagicPaths = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.MagicPath}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<MagicPathMetadata> magicPathList = Serializer.Deserialize<List<MagicPathMetadata>>(stream);
        foreach (MagicPathMetadata magicPath in magicPathList)
        {
            MagicPaths[magicPath.Id] = magicPath;
        }
    }

    public static MagicPathMetadata GetMagicPath(long id)
    {
        return MagicPaths.GetValueOrDefault(id);
    }
}
