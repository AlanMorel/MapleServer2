using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.Types;

public abstract class ItemStat
{
    public dynamic ItemAttribute;
    public ItemStatType Type;
    public dynamic Flat;
    public float Percent;
}

public class NormalStat : ItemStat
{
    public new StatId ItemAttribute;
    public new int Flat;

    public NormalStat() { }

    public NormalStat(StatId attribute, ItemStatType type, int flat, float percent)
    {
        ItemAttribute = attribute;
        Flat = flat;
        Percent = percent;
        Type = type;
    }

    public NormalStat(ParserStat stat, ItemStatType type)
    {
        ItemAttribute = stat.Id;
        Flat = stat.Flat;
        Percent = stat.Percent;
        Type = type;
    }
}

public class SpecialStat : ItemStat
{
    public new SpecialStatId ItemAttribute;
    public new float Flat;

    public SpecialStat() { }

    public SpecialStat(SpecialStatId attribute, ItemStatType type, float flat, float percent)
    {
        ItemAttribute = attribute;
        Flat = flat;
        Percent = percent;
        Type = type;
    }

    public SpecialStat(ParserSpecialStat stat, ItemStatType type)
    {
        ItemAttribute = stat.Id;
        Flat = stat.Flat;
        Percent = stat.Percent;
        Type = type;
    }
}

public class Gemstone
{
    public int Id;
    public long OwnerId = 0;
    public string OwnerName = "";
    public bool IsLocked;
    public long UnlockTime;
}

public class GemSocket
{
    public bool IsUnlocked;
    public Gemstone Gemstone;
}

public class ItemStats
{
    public List<ItemStat> BasicStats;
    public List<ItemStat> BonusStats;
    public List<GemSocket> GemSockets;

    public ItemStats() { }

    public ItemStats(Item item)
    {
        CreateNewStats(item, item.Rarity, item.ItemSlot, item.Level);
    }

    public ItemStats(Item item, int rarity, ItemSlot itemSlot, int itemLevel)
    {
        CreateNewStats(item, rarity, itemSlot, itemLevel);
    }

    public ItemStats(ItemStats other)
    {
        BasicStats = new(other.BasicStats);
        BonusStats = new(other.BonusStats);
        GemSockets = new();
    }

    private void CreateNewStats(Item item, int rarity, ItemSlot itemSlot, int itemLevel)
    {
        BasicStats = new();
        BonusStats = new();
        GemSockets = new();
        if (rarity == 0)
        {
            return;
        }

        int optionId = ItemMetadataStorage.GetOptionId(item.Id);
        float optionLevelFactor = ItemMetadataStorage.GetOptionLevelFactor(item.Id);
        float globalOptionLevelFactor = ItemMetadataStorage.GetGlobalOptionLevelFactor(item.Id);

        GetConstantStats(item, optionId, optionLevelFactor, globalOptionLevelFactor, out List<NormalStat> normalStats, out List<SpecialStat> specialStats);
        BasicStats.AddRange(normalStats);
        BonusStats.AddRange(specialStats);
        GetStaticStats(item, optionId, optionLevelFactor, globalOptionLevelFactor, out List<NormalStat> staticNormalStats, out List<SpecialStat> staticSpecialStats);
        BasicStats.AddRange(staticNormalStats);
        BonusStats.AddRange(staticSpecialStats);
        GetBonusStats(item.Id, rarity);
        if (itemLevel >= 50 && rarity >= 3)
        {
            GetGemSockets(itemSlot, rarity);
        }
    }

    private static void GetConstantStats(Item item, int optionId, float optionLevelFactor, float globalOptionLevelFactor, out List<NormalStat> normalStats, out List<SpecialStat> specialStats)
    {
        normalStats = new();
        specialStats = new();

        int constantId = ItemMetadataStorage.GetOptionConstant(item.Id);
        ItemOptionsConstant basicOptions = ItemOptionConstantMetadataStorage.GetMetadata(constantId, item.Rarity);
        if (basicOptions == null)
        {
            GetDefaultConstantStats(item, normalStats, optionId, optionLevelFactor, globalOptionLevelFactor);
            return;
        }

        foreach (ParserStat stat in basicOptions.Stats)
        {
            normalStats.Add(new(stat.Id, ItemStatType.Constant, stat.Flat, stat.Percent));
        }

        foreach (ParserSpecialStat stat in basicOptions.SpecialStats)
        {
            specialStats.Add(new(stat.Id, ItemStatType.Constant, stat.Flat, stat.Percent));
        }

        if (basicOptions.HiddenDefenseAdd > 0)
        {
            AddHiddenNormalStat(normalStats, ItemStatType.Constant, StatId.Defense, basicOptions.HiddenDefenseAdd, basicOptions.DefenseCalibrationFactor);
        }

        if (basicOptions.HiddenWeaponAtkAdd > 0)
        {
            AddHiddenNormalStat(normalStats, ItemStatType.Constant, StatId.MinWeaponAtk, basicOptions.HiddenWeaponAtkAdd, basicOptions.WeaponAtkCalibrationFactor);
            AddHiddenNormalStat(normalStats, ItemStatType.Constant, StatId.MaxWeaponAtk, basicOptions.HiddenWeaponAtkAdd, basicOptions.WeaponAtkCalibrationFactor);
        }

        GetDefaultConstantStats(item, normalStats, optionId, optionLevelFactor, globalOptionLevelFactor);
    }

    private static void GetDefaultConstantStats(Item item, List<NormalStat> normalStats, int optionId, float optionLevelFactor, float globalOptionLevelFactor)
    {
        ItemOptionPick baseOptions = ItemOptionPickMetadataStorage.GetMetadata(optionId, item.Rarity);
        if (baseOptions is null)
        {
            return;
        }

        ScriptLoader scriptLoader = new("Functions/calcItemValues");

        foreach (ConstantPick constantPick in baseOptions.Constants)
        {
            string calcScript = "";
            switch (constantPick.Stat)
            {
                case StatId.Hp:
                    calcScript = "constant_value_hp";
                    break;
                case StatId.Defense:
                    calcScript = "constant_value_ndd";
                    break;
                case StatId.MagicRes:
                    calcScript = "constant_value_mar";
                    break;
                case StatId.PhysicalRes:
                    calcScript = "constant_value_par";
                    break;
                case StatId.CritRate:
                    calcScript = "constant_value_cap";
                    break;
                case StatId.Str:
                    calcScript = "constant_value_str";
                    break;
                case StatId.Dex:
                    calcScript = "constant_value_dex";
                    break;
                case StatId.Int:
                    calcScript = "constant_value_int";
                    break;
                case StatId.Luk:
                    calcScript = "constant_value_luk";
                    break;
                case StatId.MagicAtk:
                    calcScript = "constant_value_map";
                    break;
                case StatId.MinWeaponAtk:
                    calcScript = "constant_value_wapmin";
                    break;
                case StatId.MaxWeaponAtk:
                    calcScript = "constant_value_wapmax";
                    break;
                default:
                    continue;
            }

            NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == constantPick.Stat);
            if (normalStat is null)
            {
                normalStat = new(constantPick.Stat, ItemStatType.Constant, 0, 0);
            }

            DynValue result = scriptLoader.Call(calcScript, normalStat.Flat, constantPick.DeviationValue, (int) item.Type,
                (int) item.RecommendJobs.First(), optionLevelFactor, item.Rarity, globalOptionLevelFactor, 0);

            if (result.Number == 0)
            {
                continue;
            }

            normalStat.Flat += (int) result.Number;

            int statIndex = normalStats.FindIndex(x => x.ItemAttribute == normalStat.ItemAttribute);
            if (statIndex == -1)
            {
                normalStats.Add(normalStat);
                continue;
            }

            normalStats[statIndex] = normalStat;
        }
    }

    private static void GetStaticStats(Item item, int optionId, float optionLevelFactor, float globalOptionLevelFactor, out List<NormalStat> normalStats, out List<SpecialStat> specialStats)
    {
        normalStats = new();
        specialStats = new();

        int staticId = ItemMetadataStorage.GetOptionStatic(item.Id);

        ItemOptionsStatic staticOptions = ItemOptionStaticMetadataStorage.GetMetadata(staticId, item.Rarity);
        if (staticOptions == null)
        {
            GetDefaultStaticStats(item, normalStats, optionId, optionLevelFactor, globalOptionLevelFactor);
            return;
        }

        foreach (ParserStat stat in staticOptions.Stats)
        {
            NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == stat.Id);
            if (normalStat == null)
            {
                normalStats.Add(new(stat.Id, ItemStatType.Static, stat.Flat, stat.Percent));
                continue;
            }
            int index = normalStats.FindIndex(x => x.ItemAttribute == stat.Id);
            int summedFlat = normalStat.Flat + stat.Flat;
            float summedPercent = normalStat.Percent + stat.Percent;

            normalStats[index] = new(stat.Id, ItemStatType.Static, summedFlat, summedPercent);
        }

        foreach (ParserSpecialStat stat in staticOptions.SpecialStats)
        {
            SpecialStat normalStat = specialStats.FirstOrDefault(x => x.ItemAttribute == stat.Id);
            if (normalStat == null)
            {
                specialStats.Add(new(stat.Id, ItemStatType.Static, stat.Flat, stat.Percent));
                continue;
            }

            int index = specialStats.FindIndex(x => x.ItemAttribute == stat.Id);
            float summedFlat = normalStat.Flat + stat.Flat;
            float summedPercent = normalStat.Percent + stat.Percent;

            specialStats[index] = new(stat.Id, ItemStatType.Static, summedFlat, summedPercent);
        }

        if (staticOptions.HiddenDefenseAdd > 0)
        {
            AddHiddenNormalStat(normalStats, ItemStatType.Static, StatId.Defense, staticOptions.HiddenDefenseAdd, staticOptions.DefenseCalibrationFactor);
        }

        if (staticOptions.HiddenWeaponAtkAdd > 0)
        {
            AddHiddenNormalStat(normalStats, ItemStatType.Static, StatId.MinWeaponAtk, staticOptions.HiddenWeaponAtkAdd, staticOptions.WeaponAtkCalibrationFactor);
            AddHiddenNormalStat(normalStats, ItemStatType.Static, StatId.MaxWeaponAtk, staticOptions.HiddenWeaponAtkAdd, staticOptions.WeaponAtkCalibrationFactor);
        }

        GetDefaultStaticStats(item, normalStats, optionId, optionLevelFactor, globalOptionLevelFactor);
    }

    private static void GetDefaultStaticStats(Item item, List<NormalStat> normalStats, int optionId, float optionLevelFactor, float globalOptionLevelFactor)
    {
        ItemOptionPick baseOptions = ItemOptionPickMetadataStorage.GetMetadata(optionId, item.Rarity);
        if (baseOptions is null)
        {
            return;
        }

        Random random = RandomProvider.Get();
        ScriptLoader scriptLoader = new("Functions/calcItemValues");
        foreach (StaticPick staticPick in baseOptions.Statics)
        {
            string calcScript = "";
            switch (staticPick.Stat)
            {
                case StatId.Hp:
                    calcScript = "static_value_hp";
                    break;
                case StatId.Defense:
                    calcScript = "static_value_ndd";
                    break;
                case StatId.MagicRes:
                    calcScript = "static_value_mar";
                    break;
                case StatId.PhysicalRes:
                    calcScript = "static_value_par";
                    break;
                case StatId.PhysicalAtk:
                    calcScript = "static_value_pap";
                    break;
                case StatId.MagicAtk:
                    calcScript = "static_value_map";
                    break;
                case StatId.PerfectGuard:
                    calcScript = "static_rate_abp";
                    break;
                case StatId.MaxWeaponAtk:
                    calcScript = "static_value_wapmax";
                    break;
                default:
                    continue;
            }

            NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == staticPick.Stat);
            if (normalStat is null)
            {
                normalStat = new(staticPick.Stat, ItemStatType.Static, 0, 0);
            }

            DynValue result = scriptLoader.Call(calcScript, normalStat.Flat, staticPick.DeviationValue, (int) item.Type,
                (int) item.RecommendJobs.First(), optionLevelFactor, item.Rarity, globalOptionLevelFactor, 0);

            if (result.Tuple.Length == 0)
            {
                continue;
            }

            // Get random between min and max values
            double statValue = random.NextDouble() * (result.Tuple[1].Number - result.Tuple[0].Number) + result.Tuple[0].Number;

            if (normalStat.ItemAttribute == StatId.PerfectGuard)
            {
                normalStat.Percent += (float) statValue;
            }
            else
            {
                normalStat.Flat += (int) statValue;
            }

            int statIndex = normalStats.FindIndex(x => x.ItemAttribute == normalStat.ItemAttribute);
            if (statIndex == -1)
            {
                normalStats.Add(normalStat);
                continue;
            }

            normalStats[statIndex] = normalStat;
        }
    }

    private static void AddHiddenNormalStat(List<NormalStat> normalStats, ItemStatType type, StatId attribute, int value, float calibrationFactor)
    {
        NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == attribute);
        if (normalStat == null)
        {
            return;
        }
        int calibratedValue = (int) (value * calibrationFactor);

        int index = normalStats.FindIndex(x => x.ItemAttribute == attribute);
        int biggerValue = Math.Max(value, calibratedValue);
        int smallerValue = Math.Min(value, calibratedValue);
        int summedFlat = normalStat.Flat + RandomProvider.Get().Next(smallerValue, biggerValue);
        normalStats[index] = new(normalStat.ItemAttribute, type, summedFlat, normalStat.Percent);
    }

    public void GetBonusStats(int itemId, int rarity)
    {
        int randomId = ItemMetadataStorage.GetOptionRandom(itemId);
        ItemOptionRandom randomOptions = ItemOptionRandomMetadataStorage.GetMetadata(randomId, rarity);
        if (randomOptions == null)
        {
            return;
        }

        // get amount of slots
        Random random = RandomProvider.Get();
        int slots = random.Next(randomOptions.Slots[0], randomOptions.Slots[1]);

        IEnumerable<ItemStat> itemStats = RollStats(randomOptions, randomId, itemId);
        List<ItemStat> selectedStats = itemStats.OrderBy(x => random.Next()).Take(slots).ToList();

        BonusStats.AddRange(selectedStats);
    }

    public static IEnumerable<ItemStat> RollStats(ItemOptionRandom randomOptions, int randomId, int itemId)
    {
        List<ItemStat> itemStats = new();

        foreach (ParserStat stat in randomOptions.Stats)
        {
            Dictionary<StatId, List<ParserStat>> rangeDictionary = GetRange(randomId);
            if (!rangeDictionary.ContainsKey(stat.Id))
            {
                continue;
            }

            NormalStat normalStat = new(rangeDictionary[stat.Id][Roll(itemId)], ItemStatType.Random);
            if (randomOptions.MultiplyFactor > 0)
            {
                normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.Percent *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(normalStat);
        }

        foreach (ParserSpecialStat stat in randomOptions.SpecialStats)
        {
            Dictionary<SpecialStatId, List<ParserSpecialStat>> rangeDictionary = GetSpecialRange(randomId);
            if (!rangeDictionary.ContainsKey(stat.Id))
            {
                continue;
            }

            SpecialStat specialStat = new(rangeDictionary[stat.Id][Roll(itemId)], ItemStatType.Random);
            if (randomOptions.MultiplyFactor > 0)
            {
                specialStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.Percent *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(specialStat);
        }

        return itemStats;
    }

    // Roll new bonus stats and values except the locked stat
    public static List<ItemStat> RollBonusStatsWithStatLocked(Item item, short ignoreStat, bool isSpecialStat)
    {
        int id = item.Id;

        int randomId = ItemMetadataStorage.GetOptionRandom(id);
        ItemOptionRandom randomOptions = ItemOptionRandomMetadataStorage.GetMetadata(randomId, item.Rarity);
        if (randomOptions == null)
        {
            return null;
        }

        List<ItemStat> itemStats = new();

        List<ParserStat> attributes = isSpecialStat ? randomOptions.Stats : randomOptions.Stats.Where(x => (short) x.Id != ignoreStat).ToList();
        List<ParserSpecialStat> specialAttributes = isSpecialStat ? randomOptions.SpecialStats.Where(x => (short) x.Id != ignoreStat).ToList() : randomOptions.SpecialStats;

        foreach (ParserStat attribute in attributes)
        {
            Dictionary<StatId, List<ParserStat>> dictionary = GetRange(randomId);
            if (!dictionary.ContainsKey(attribute.Id))
            {
                continue;
            }

            NormalStat normalStat = new(dictionary[attribute.Id][Roll(id)], ItemStatType.Random);
            if (randomOptions.MultiplyFactor > 0)
            {
                normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                normalStat.Percent *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(normalStat);
        }

        foreach (ParserSpecialStat attribute in specialAttributes)
        {
            Dictionary<SpecialStatId, List<ParserSpecialStat>> dictionary = GetSpecialRange(randomId);
            if (!dictionary.ContainsKey(attribute.Id))
            {
                continue;
            }

            SpecialStat specialStat = new(dictionary[attribute.Id][Roll(id)], ItemStatType.Random);
            if (randomOptions.MultiplyFactor > 0)
            {
                specialStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                specialStat.Percent *= randomOptions.MultiplyFactor;
            }
            itemStats.Add(specialStat);
        }

        return itemStats.OrderBy(x => RandomProvider.Get().Next()).Take(item.Stats.BonusStats.Count).ToList();
    }

    // Roll new values for existing bonus stats
    public static List<ItemStat> RollNewBonusValues(Item item, short ignoreStat, bool isSpecialStat)
    {
        List<ItemStat> newBonus = new();

        foreach (NormalStat stat in item.Stats.BonusStats.OfType<NormalStat>())
        {
            if (!isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
            {
                newBonus.Add(stat);
                continue;
            }

            Dictionary<StatId, List<ParserStat>> dictionary = GetRange(item.Id);
            if (!dictionary.ContainsKey(stat.ItemAttribute))
            {
                continue;
            }
            newBonus.Add(new NormalStat(dictionary[stat.ItemAttribute][Roll(item.Level)], ItemStatType.Random));
        }

        foreach (SpecialStat stat in item.Stats.BonusStats.OfType<SpecialStat>())
        {
            if (isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
            {
                newBonus.Add(stat);
                continue;
            }

            Dictionary<SpecialStatId, List<ParserSpecialStat>> dictionary = GetSpecialRange(item.Id);
            if (!dictionary.ContainsKey(stat.ItemAttribute))
            {
                continue;
            }
            newBonus.Add(new SpecialStat(dictionary[stat.ItemAttribute][Roll(item.Level)], ItemStatType.Random));
        }

        return newBonus;
    }

    private static Dictionary<StatId, List<ParserStat>> GetRange(int itemId)
    {
        ItemSlot slot = ItemMetadataStorage.GetSlot(itemId);
        if (Item.IsAccessory(slot))
        {
            return ItemOptionRangeStorage.GetAccessoryRanges();
        }

        if (Item.IsArmor(slot))
        {
            return ItemOptionRangeStorage.GetArmorRanges();
        }

        if (Item.IsWeapon(slot))
        {
            return ItemOptionRangeStorage.GetWeaponRanges();
        }

        return ItemOptionRangeStorage.GetPetRanges();
    }

    private static Dictionary<SpecialStatId, List<ParserSpecialStat>> GetSpecialRange(int itemId)
    {
        ItemSlot slot = ItemMetadataStorage.GetSlot(itemId);
        if (Item.IsAccessory(slot))
        {
            return ItemOptionRangeStorage.GetAccessorySpecialRanges();
        }

        if (Item.IsArmor(slot))
        {
            return ItemOptionRangeStorage.GetArmorSpecialRanges();
        }

        if (Item.IsWeapon(slot))
        {
            return ItemOptionRangeStorage.GetWeaponSpecialRanges();
        }

        return ItemOptionRangeStorage.GetPetSpecialRanges();
    }

    // Returns index 0~7 for equip level 70-
    // Returns index 8~15 for equip level 70+
    private static int Roll(int itemId)
    {
        float itemLevelFactor = ItemMetadataStorage.GetOptionLevelFactor(itemId);
        Random random = RandomProvider.Get();
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

    private void GetGemSockets(ItemSlot itemSlot, int rarity)
    {
        if (itemSlot != ItemSlot.EA &&
            itemSlot != ItemSlot.RI &&
            itemSlot != ItemSlot.PD)
        {
            return;
        }

        int rollAmount = 0;
        if (rarity == 3)
        {
            rollAmount = 1;
        }
        else if (rarity > 3)
        {
            rollAmount = 3;
        }

        // add sockets
        for (int i = 0; i < rollAmount; i++)
        {
            GemSocket socket = new();
            GemSockets.Add(socket);
        }

        // roll to unlock sockets
        for (int i = 0; i < GemSockets.Count; i++)
        {
            int successNumber = RandomProvider.Get().Next(0, 100);

            // 5% success rate to unlock a gemsocket
            if (successNumber < 95)
            {
                break;
            }
            GemSockets[i].IsUnlocked = true;
        }
    }
}
