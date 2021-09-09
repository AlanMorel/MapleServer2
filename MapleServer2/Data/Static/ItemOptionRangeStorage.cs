using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ItemOptionRangeStorage
    {
        public static readonly Dictionary<ItemOptionRangeType, Dictionary<ItemAttribute, List<ParserStat>>> NormalRange = new Dictionary<ItemOptionRangeType, Dictionary<ItemAttribute, List<ParserStat>>>();
        public static readonly Dictionary<ItemOptionRangeType, Dictionary<SpecialItemAttribute, List<ParserSpecialStat>>> SpecialRange = new Dictionary<ItemOptionRangeType, Dictionary<SpecialItemAttribute, List<ParserSpecialStat>>>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-item-option-range-metadata");
            List<ItemOptionRangeMetadata> items = Serializer.Deserialize<List<ItemOptionRangeMetadata>>(stream);
            foreach (ItemOptionRangeMetadata optionRange in items)
            {
                NormalRange[optionRange.RangeType] = optionRange.Stats;
                SpecialRange[optionRange.RangeType] = optionRange.SpecialStats;
            }
        }

        public static Dictionary<ItemAttribute, List<ParserStat>> GetAccessoryRanges()
        {
            return NormalRange[ItemOptionRangeType.itemoptionvariation_acc];
        }

        public static Dictionary<ItemAttribute, List<ParserStat>> GetArmorRanges()
        {
            return NormalRange[ItemOptionRangeType.itemoptionvariation_armor];
        }

        public static Dictionary<ItemAttribute, List<ParserStat>> GetPetRanges()
        {
            return NormalRange[ItemOptionRangeType.itemoptionvariation_pet];
        }

        public static Dictionary<ItemAttribute, List<ParserStat>> GetWeaponRanges()
        {
            return NormalRange[ItemOptionRangeType.itemoptionvariation_weapon];
        }

        public static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetAccessorySpecialRanges()
        {
            return SpecialRange[ItemOptionRangeType.itemoptionvariation_acc];
        }

        public static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetArmorSpecialRanges()
        {
            return SpecialRange[ItemOptionRangeType.itemoptionvariation_armor];
        }

        public static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetPetSpecialRanges()
        {
            return SpecialRange[ItemOptionRangeType.itemoptionvariation_pet];
        }

        public static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetWeaponSpecialRanges()
        {
            return SpecialRange[ItemOptionRangeType.itemoptionvariation_weapon];
        }
    }
}
