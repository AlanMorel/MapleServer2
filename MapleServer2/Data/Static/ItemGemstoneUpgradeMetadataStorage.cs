using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemGemstoneUpgradeMetadataStorage
    {
        private static readonly Dictionary<int, ItemGemstoneUpgradeMetadata> gem = new Dictionary<int, ItemGemstoneUpgradeMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-gemstone-upgrade-metadata");
            List<ItemGemstoneUpgradeMetadata> items = Serializer.Deserialize<List<ItemGemstoneUpgradeMetadata>>(stream);
            foreach (ItemGemstoneUpgradeMetadata item in items)
            {
                gem[item.ItemId] = item;
            }
        }

        public static ItemGemstoneUpgradeMetadata GetMetadata(int itemId)
        {
            return gem.GetValueOrDefault(itemId);
        }
    }
}
