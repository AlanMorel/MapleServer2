using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Types;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class RewardContentMetadataStorage
{
    private static readonly Dictionary<int, RewardContentMetadata> RewardContent = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.RewardContent}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<RewardContentMetadata> items = Serializer.Deserialize<List<RewardContentMetadata>>(stream);
        foreach (RewardContentMetadata item in items)
        {
            RewardContent[item.Id] = item;
        }
    }

    public static List<Item> GetRewardItems(int id, int playerLevel)
    {
        RewardContentMetadata metadata = RewardContent.GetValueOrDefault(id);
        List<Item> items = new();
        foreach (RewardContentItemMetadata rewardItem in metadata.RewardItems)
        {
            if (rewardItem.MinLevel == 0 && rewardItem.MaxLevel == 0)
            {
                foreach (RewardItemData itemData in rewardItem.Items)
                {
                    items.Add(GetItem(itemData));
                }
            }
            else if (rewardItem.MinLevel >= playerLevel && rewardItem.MaxLevel <= playerLevel)
            {
                foreach (RewardItemData itemData in rewardItem.Items)
                {
                    items.Add(GetItem(itemData));
                }
            }
        }

        return items;
    }

    private static Item GetItem(RewardItemData itemData)
    {
        return new(itemData.Id)
        {
            Amount = itemData.Amount,
            Rarity = itemData.Rarity
        };
    }
}
