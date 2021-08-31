using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class DefaultItemsMetadataStorage
    {
        private static readonly Dictionary<int, DefaultItemsMetadata> defaultItems = new Dictionary<int, DefaultItemsMetadata>();

        static DefaultItemsMetadataStorage()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-default-items-metadata");
            List<DefaultItemsMetadata> items = Serializer.Deserialize<List<DefaultItemsMetadata>>(stream);
            foreach (DefaultItemsMetadata item in items)
            {
                defaultItems[item.JobCode] = item;
            }
        }

        public static bool IsValid(int job, int itemId)
        {
            DefaultItemsMetadata metadata = defaultItems.GetValueOrDefault(job);
            return metadata.DefaultItems.Any(x => x.ItemId == itemId);
        }
    }
}
