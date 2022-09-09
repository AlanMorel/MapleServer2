using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MagicPathMetadataStorage
{
    private static readonly Dictionary<long, MagicPathMetadata> MagicPaths = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.MagicPath);
        List<MagicPathMetadata> magicPathList = Serializer.Deserialize<List<MagicPathMetadata>>(stream);
        foreach (MagicPathMetadata magicPath in magicPathList)
        {
            MagicPaths[magicPath.Id] = magicPath;
        }
    }

    public static MagicPathMetadata? GetMagicPath(long id)
    {
        return MagicPaths.GetValueOrDefault(id);
    }
}
