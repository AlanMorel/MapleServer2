using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class ItemOptionRangeStorage
{
    private static readonly Dictionary<ItemOptionRangeType, Dictionary<StatAttribute, List<ParserStat>>> NormalRange = new();
    private static readonly Dictionary<ItemOptionRangeType, Dictionary<StatAttribute, List<ParserSpecialStat>>> SpecialRange = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.ItemOptionRange);
        List<ItemOptionRangeMetadata> items = Serializer.Deserialize<List<ItemOptionRangeMetadata>>(stream);
        foreach (ItemOptionRangeMetadata optionRange in items)
        {
            NormalRange[optionRange.RangeType] = optionRange.Stats;
            SpecialRange[optionRange.RangeType] = optionRange.SpecialStats;
        }
    }

    public static Dictionary<StatAttribute, List<ParserStat>> GetAccessoryRanges()
    {
        return NormalRange[ItemOptionRangeType.itemoptionvariation_acc];
    }

    public static Dictionary<StatAttribute, List<ParserStat>> GetArmorRanges()
    {
        return NormalRange[ItemOptionRangeType.itemoptionvariation_armor];
    }

    public static Dictionary<StatAttribute, List<ParserStat>> GetPetRanges()
    {
        return NormalRange[ItemOptionRangeType.itemoptionvariation_pet];
    }

    public static Dictionary<StatAttribute, List<ParserStat>> GetWeaponRanges()
    {
        return NormalRange[ItemOptionRangeType.itemoptionvariation_weapon];
    }

    public static Dictionary<StatAttribute, List<ParserSpecialStat>> GetAccessorySpecialRanges()
    {
        return SpecialRange[ItemOptionRangeType.itemoptionvariation_acc];
    }

    public static Dictionary<StatAttribute, List<ParserSpecialStat>> GetArmorSpecialRanges()
    {
        return SpecialRange[ItemOptionRangeType.itemoptionvariation_armor];
    }

    public static Dictionary<StatAttribute, List<ParserSpecialStat>> GetPetSpecialRanges()
    {
        return SpecialRange[ItemOptionRangeType.itemoptionvariation_pet];
    }

    public static Dictionary<StatAttribute, List<ParserSpecialStat>> GetWeaponSpecialRanges()
    {
        return SpecialRange[ItemOptionRangeType.itemoptionvariation_weapon];
    }
}
