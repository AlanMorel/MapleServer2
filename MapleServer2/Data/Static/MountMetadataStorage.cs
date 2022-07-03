using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class MountMetadataStorage
{
    private static readonly Dictionary<int, MountMetadata> Mounts = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Mount);
        List<MountMetadata> mountList = Serializer.Deserialize<List<MountMetadata>>(stream);
        foreach (MountMetadata mount in mountList)
        {
            Mounts.Add(mount.Id, mount);
        }
    }

    public static MountMetadata GetMountMetadata(int id) => Mounts.GetValueOrDefault(id);
}
