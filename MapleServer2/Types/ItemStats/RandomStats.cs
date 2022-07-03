using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types;

public static class RandomStats
{
    public static void GetStats(Item item, out Dictionary<StatAttribute, ItemStat> randomStats)
    {
        randomStats = new();
        int randomId = ItemMetadataStorage.GetOptionMetadata(item.Id).Random;
        ItemOptionRandom randomOptions = ItemOptionRandomMetadataStorage.GetMetadata(randomId, item.Rarity);
        if (randomOptions == null)
        {
            return;
        }

        // get amount of slots
        Random random = Random.Shared;
        int slots = random.Next(randomOptions.Slots[0], randomOptions.Slots[1]);

        IEnumerable<ItemStat> itemStats = RollStats(randomOptions, item.Id);
        List<ItemStat> selectedStats = itemStats.OrderBy(x => random.Next()).Take(slots).ToList();

        foreach (ItemStat stat in selectedStats)
        {
            randomStats[stat.ItemAttribute] = stat;
        }
    }
    public static IEnumerable<ItemStat> RollStats(ItemOptionRandom randomOptions, int itemId)
    {
        List<ItemStat> itemStats = new();

        foreach (ParserStat stat in randomOptions.Stats)
        {
            Dictionary<StatAttribute, List<ParserStat>> rangeDictionary = GetRange(itemId);
            if (!rangeDictionary.ContainsKey(stat.Attribute))
            {
                continue;
            }

            BasicStat normalStat = new(rangeDictionary[stat.Attribute][Roll(itemId)]);
            if (randomOptions.MultiplyFactor > 0)
            {
                normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.Rate *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(normalStat);
        }

        foreach (ParserSpecialStat stat in randomOptions.SpecialStats)
        {
            Dictionary<StatAttribute, List<ParserSpecialStat>> rangeDictionary = GetSpecialRange(itemId);
            if (!rangeDictionary.ContainsKey(stat.Attribute))
            {
                continue;
            }

            SpecialStat specialStat = new(rangeDictionary[stat.Attribute][Roll(itemId)]);
            if (randomOptions.MultiplyFactor > 0)
            {
                specialStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.Rate *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(specialStat);
        }

        return itemStats;
    }

    // Roll new bonus stats and values except the locked stat
    public static List<ItemStat> RollBonusStatsWithStatLocked(Item item, short ignoreStat, bool isSpecialStat)
    {
        int id = item.Id;

        int randomId = ItemMetadataStorage.GetOptionMetadata(id).Random;
        ItemOptionRandom randomOptions = ItemOptionRandomMetadataStorage.GetMetadata(randomId, item.Rarity);
        if (randomOptions == null)
        {
            return null;
        }

        List<ItemStat> itemStats = new();

        List<ParserStat> attributes = isSpecialStat ? randomOptions.Stats : randomOptions.Stats.Where(x => (short) x.Attribute != ignoreStat).ToList();
        List<ParserSpecialStat> specialAttributes = isSpecialStat ? randomOptions.SpecialStats.Where(x => (short) x.Attribute != ignoreStat).ToList() : randomOptions.SpecialStats;

        foreach (ParserStat attribute in attributes)
        {
            Dictionary<StatAttribute, List<ParserStat>> dictionary = GetRange(item.Id);
            if (!dictionary.ContainsKey(attribute.Attribute))
            {
                continue;
            }

            BasicStat normalStat = new(dictionary[attribute.Attribute][Roll(id)]);
            if (randomOptions.MultiplyFactor > 0)
            {
                normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.Rate *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(normalStat);
        }

        foreach (ParserSpecialStat attribute in specialAttributes)
        {
            Dictionary<StatAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(item.Id);
            if (!dictionary.ContainsKey(attribute.Attribute))
            {
                continue;
            }

            SpecialStat specialStat = new(dictionary[attribute.Attribute][Roll(id)]);
            if (randomOptions.MultiplyFactor > 0)
            {
                specialStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.Rate *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(specialStat);
        }

        return itemStats.OrderBy(x => Random.Shared.Next()).Take(item.Stats.Randoms.Count).ToList();
    }

    // Roll new values for existing bonus stats
    public static Dictionary<StatAttribute, ItemStat> RollNewBonusValues(Item item, short ignoreStat, bool isSpecialStat)
    {
        Dictionary<StatAttribute, ItemStat> newBonus = new();

        foreach (BasicStat stat in item.Stats.Randoms.Values.OfType<BasicStat>())
        {
            if (!isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
            {
                newBonus[stat.ItemAttribute] = stat;
                continue;
            }

            Dictionary<StatAttribute, List<ParserStat>> dictionary = GetRange(item.Id);
            if (!dictionary.ContainsKey(stat.ItemAttribute))
            {
                continue;
            }
            newBonus[stat.ItemAttribute] = new BasicStat(dictionary[stat.ItemAttribute][Roll(item.Level)]);
        }

        foreach (SpecialStat stat in item.Stats.Randoms.OfType<SpecialStat>())
        {
            if (isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
            {
                newBonus[stat.ItemAttribute] = stat;
                continue;
            }

            Dictionary<StatAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(item.Id);
            if (!dictionary.ContainsKey(stat.ItemAttribute))
            {
                continue;
            }
            newBonus[stat.ItemAttribute] = new SpecialStat(dictionary[stat.ItemAttribute][Roll(item.Level)]);
        }

        return newBonus;
    }

    private static Dictionary<StatAttribute, List<ParserStat>> GetRange(int itemId)
    {
        List<ItemSlot> slots = ItemMetadataStorage.GetItemSlots(itemId);
        if (Item.IsAccessory(slots))
        {
            return ItemOptionRangeStorage.GetAccessoryRanges();
        }

        if (Item.IsArmor(slots))
        {
            return ItemOptionRangeStorage.GetArmorRanges();
        }

        if (Item.IsWeapon(slots))
        {
            return ItemOptionRangeStorage.GetWeaponRanges();
        }

        return ItemOptionRangeStorage.GetPetRanges();
    }

    private static Dictionary<StatAttribute, List<ParserSpecialStat>> GetSpecialRange(int itemId)
    {
        List<ItemSlot> slots = ItemMetadataStorage.GetItemSlots(itemId);
        if (Item.IsAccessory(slots))
        {
            return ItemOptionRangeStorage.GetAccessorySpecialRanges();
        }

        if (Item.IsArmor(slots))
        {
            return ItemOptionRangeStorage.GetArmorSpecialRanges();
        }

        if (Item.IsWeapon(slots))
        {
            return ItemOptionRangeStorage.GetWeaponSpecialRanges();
        }

        return ItemOptionRangeStorage.GetPetSpecialRanges();
    }

    // Returns index 0~7 for equip level 70-
    // Returns index 8~15 for equip level 70+
    private static int Roll(int itemId)
    {
        float itemLevelFactor = ItemMetadataStorage.GetOptionMetadata(itemId).OptionLevelFactor;
        Random random = Random.Shared;
        if (itemLevelFactor >= 70)
        {
            return random.NextDouble() switch
            {
                >= 0.0 and < 0.24 => 8,
                >= 0.24 and < 0.48 => 9,
                >= 0.48 and < 0.74 => 10,
                >= 0.74 and < 0.9 => 11,
                >= 0.9 and < 0.966 => 12,
                >= 0.966 and < 0.985 => 13,
                >= 0.985 and < 0.9975 => 14,
                _ => 15
            };
        }
        return random.NextDouble() switch
        {
            >= 0.0 and < 0.24 => 0,
            >= 0.24 and < 0.48 => 1,
            >= 0.48 and < 0.74 => 2,
            >= 0.74 and < 0.9 => 3,
            >= 0.9 and < 0.966 => 4,
            >= 0.966 and < 0.985 => 5,
            >= 0.985 and < 0.9975 => 6,
            _ => 7
        };
    }
}
