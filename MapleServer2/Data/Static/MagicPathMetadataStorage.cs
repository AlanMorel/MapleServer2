using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class MagicPathMetadataStorage
    {
        private static readonly Dictionary<long, MagicPathMetadata> MagicPaths = new Dictionary<long, MagicPathMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-magicpath-metadata");
            List<MagicPathMetadata> magicPathList = Serializer.Deserialize<List<MagicPathMetadata>>(stream);
            foreach (MagicPathMetadata magicPath in magicPathList)
            {
                MagicPaths[magicPath.Id] = magicPath;
            }
        }

        public static MagicPathMetadata GetMagicPath(long id) => MagicPaths.GetValueOrDefault(id);
    }
}
