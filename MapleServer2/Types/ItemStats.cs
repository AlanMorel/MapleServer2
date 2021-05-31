using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public interface ItemStat
    {
    }

    public class NormalStat : ItemStat
    {
        public ItemAttribute ItemAttribute;
        public int Flat;
        public float Percent;

        public NormalStat(ItemAttribute attribute, int flat, float percent)
        {
            ItemAttribute = attribute;
            Flat = flat;
            Percent = percent;
        }

        public NormalStat(ParserStat stat)
        {
            ItemAttribute = stat.Id;
            Flat = stat.Flat;
            Percent = stat.Percent;
        }
    }

    public class SpecialStat : ItemStat
    {
        public SpecialItemAttribute ItemAttribute;
        public int Flat;
        public float Percent;

        public SpecialStat(SpecialItemAttribute attribute, int flat, float percent)
        {
            ItemAttribute = attribute;
            Flat = flat;
            Percent = percent;
        }

        public SpecialStat(ParserSpecialStat stat)
        {
            ItemAttribute = stat.Id;
            Flat = stat.Flat;
            Percent = stat.Percent;
        }
    }

    public class Gemstone
    {
        public readonly int Id;

        // Used if bound
        public readonly long OwnerId = 0;
        public readonly string OwnerName = "";

        public readonly long Unknown = 0;

        public Gemstone(int id)
        {
            Id = id;
        }
    }

    public class ItemStats
    {
        public List<ItemStat> BasicStats;
        public List<ItemStat> BonusStats;

        public byte TotalSockets;
        public List<Gemstone> Gemstones;

        public ItemStats() { }

        public ItemStats(Item item)
        {
            CreateNewStats(item.Id, item.Rarity);
        }

        public ItemStats(int itemId, int rarity)
        {
            CreateNewStats(itemId, rarity);
        }

        public ItemStats(ItemStats other)
        {
            BasicStats = new List<ItemStat>(other.BasicStats);
            BonusStats = new List<ItemStat>(other.BonusStats);
            TotalSockets = other.TotalSockets;
            Gemstones = new List<Gemstone>(other.Gemstones);
        }

        public void CreateNewStats(int itemId, int rarity)
        {
            BasicStats = new List<ItemStat>();
            BonusStats = new List<ItemStat>();
            Gemstones = new List<Gemstone>();
            if (rarity == 0)
            {
                return;
            }

            GetConstantStats(itemId, rarity, out List<NormalStat> normalStats, out List<SpecialStat> specialStats);
            GetStaticStats(itemId, rarity, normalStats, specialStats);
            GetBonusStats(itemId, rarity);
        }

        public static void GetConstantStats(int itemId, int rarity, out List<NormalStat> normalStats, out List<SpecialStat> specialStats)
        {
            normalStats = new List<NormalStat>();
            specialStats = new List<SpecialStat>();

            // Get Constant Stats
            int constantId = ItemMetadataStorage.GetOptionConstant(itemId);
            ItemOptionsConstant basicOptions = ItemOptionConstantMetadataStorage.GetMetadata(constantId, rarity);
            if (basicOptions == null)
            {
                return;
            }

            foreach (ParserStat stat in basicOptions.Stats)
            {
                normalStats.Add(new NormalStat(stat.Id, stat.Flat, stat.Percent));
            }

            foreach (ParserSpecialStat stat in basicOptions.SpecialStats)
            {
                specialStats.Add(new SpecialStat(stat.Id, stat.Flat, stat.Percent));
            }

            if (basicOptions.HiddenDefenseAdd > 0)
            {
                AddHiddenNormalStat(normalStats, ItemAttribute.Defense, basicOptions.HiddenDefenseAdd, basicOptions.DefenseCalibrationFactor);
            }

            if (basicOptions.HiddenWeaponAtkAdd > 0)
            {
                AddHiddenNormalStat(normalStats, ItemAttribute.MinWeaponAtk, basicOptions.HiddenWeaponAtkAdd, basicOptions.WeaponAtkCalibrationFactor);
                AddHiddenNormalStat(normalStats, ItemAttribute.MaxWeaponAtk, basicOptions.HiddenWeaponAtkAdd, basicOptions.WeaponAtkCalibrationFactor);
            }
        }

        public void GetStaticStats(int itemId, int rarity, List<NormalStat> normalStats, List<SpecialStat> specialStats)
        {
            //Get Static Stats
            int staticId = ItemMetadataStorage.GetOptionStatic(itemId);

            ItemOptionsStatic staticOptions = ItemOptionStaticMetadataStorage.GetMetadata(staticId, rarity);
            if (staticOptions == null)
            {
                BasicStats.AddRange(normalStats);
                BasicStats.AddRange(specialStats);
                return;
            }

            foreach (ParserStat stat in staticOptions.Stats)
            {
                NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == stat.Id);
                if (normalStat == null)
                {
                    normalStats.Add(new NormalStat(stat.Id, stat.Flat, stat.Percent));
                    continue;
                }
                int index = normalStats.FindIndex(x => x.ItemAttribute == stat.Id);
                int summedFlat = normalStat.Flat + stat.Flat;
                float summedPercent = normalStat.Percent + stat.Percent;

                normalStats[index] = new NormalStat(stat.Id, summedFlat, summedPercent);
            }

            foreach (ParserSpecialStat stat in staticOptions.SpecialStats)
            {
                SpecialStat normalStat = specialStats.FirstOrDefault(x => x.ItemAttribute == stat.Id);
                if (normalStat == null)
                {
                    specialStats.Add(new SpecialStat(stat.Id, stat.Flat, stat.Percent));
                    continue;
                }

                int index = specialStats.FindIndex(x => x.ItemAttribute == stat.Id);
                int summedFlat = normalStat.Flat + stat.Flat;
                float summedPercent = normalStat.Percent + stat.Percent;

                specialStats[index] = new SpecialStat(stat.Id, summedFlat, summedPercent);
            }

            if (staticOptions.HiddenDefenseAdd > 0)
            {
                AddHiddenNormalStat(normalStats, ItemAttribute.Defense, staticOptions.HiddenDefenseAdd, staticOptions.DefenseCalibrationFactor);
            }

            if (staticOptions.HiddenWeaponAtkAdd > 0)
            {
                AddHiddenNormalStat(normalStats, ItemAttribute.MinWeaponAtk, staticOptions.HiddenWeaponAtkAdd, staticOptions.WeaponAtkCalibrationFactor);
                AddHiddenNormalStat(normalStats, ItemAttribute.MaxWeaponAtk, staticOptions.HiddenWeaponAtkAdd, staticOptions.WeaponAtkCalibrationFactor);
            }

            BasicStats.AddRange(normalStats);
            BasicStats.AddRange(specialStats);
        }

        private static void AddHiddenNormalStat(List<NormalStat> normalStats, ItemAttribute attribute, int value, float calibrationFactor)
        {
            NormalStat normalStat = normalStats.FirstOrDefault(x => x.ItemAttribute == attribute);
            if (normalStat == null)
            {
                return;
            }
            int calibratedValue = (int) (value * calibrationFactor);

            Random random = new Random();
            int index = normalStats.FindIndex(x => x.ItemAttribute == attribute);
            int summedFlat = normalStat.Flat + random.Next(value, calibratedValue);
            normalStats[index] = new NormalStat(normalStat.ItemAttribute, summedFlat, normalStat.Percent);
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
            Random random = new Random();
            int slots = random.Next(randomOptions.Slots[0], randomOptions.Slots[1]);

            List<ItemStat> itemStats = RollStats(randomOptions, randomId, itemId);
            List<ItemStat> selectedStats = itemStats.OrderBy(x => random.Next()).Take(slots).ToList();

            BonusStats.AddRange(selectedStats);
        }

        public static List<ItemStat> RollStats(ItemOptionRandom randomOptions, int randomId, int itemId)
        {
            List<ItemStat> itemStats = new List<ItemStat>();

            foreach (ParserStat stat in randomOptions.Stats)
            {
                Dictionary<ItemAttribute, List<ParserStat>> rangeDictionary = GetRange(randomId);
                if (!rangeDictionary.ContainsKey(stat.Id))
                {
                    continue;
                }

                NormalStat normalStat = new NormalStat(rangeDictionary[stat.Id][Roll(itemId)]);
                if (randomOptions.MultiplyFactor > 0)
                {
                    normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                    normalStat.Percent *= randomOptions.MultiplyFactor;
                }
                itemStats.Add(normalStat);
            }

            foreach (ParserSpecialStat stat in randomOptions.SpecialStats)
            {
                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> rangeDictionary = GetSpecialRange(randomId);
                if (!rangeDictionary.ContainsKey(stat.Id))
                {
                    continue;
                }

                SpecialStat specialStat = new SpecialStat(rangeDictionary[stat.Id][Roll(itemId)]);
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

            Random random = new Random();

            List<ItemStat> itemStats = new List<ItemStat>();

            List<ParserStat> attributes = isSpecialStat ? randomOptions.Stats : randomOptions.Stats.Where(x => (short) x.Id != ignoreStat).ToList();
            List<ParserSpecialStat> specialAttributes = isSpecialStat ? randomOptions.SpecialStats.Where(x => (short) x.Id != ignoreStat).ToList() : randomOptions.SpecialStats;

            foreach (ParserStat attribute in attributes)
            {
                Dictionary<ItemAttribute, List<ParserStat>> dictionary = GetRange(randomId);
                if (!dictionary.ContainsKey(attribute.Id))
                {
                    continue;
                }

                NormalStat normalStat = new NormalStat(dictionary[attribute.Id][Roll(id)]);
                if (randomOptions.MultiplyFactor > 0)
                {
                    normalStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                    normalStat.Percent *= randomOptions.MultiplyFactor;
                }
                itemStats.Add(normalStat);
            }

            foreach (ParserSpecialStat attribute in specialAttributes)
            {
                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(randomId);
                if (!dictionary.ContainsKey(attribute.Id))
                {
                    continue;
                }

                SpecialStat specialStat = new SpecialStat(dictionary[attribute.Id][Roll(id)]);
                if (randomOptions.MultiplyFactor > 0)
                {
                    specialStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                    specialStat.Percent *= randomOptions.MultiplyFactor;
                }
                itemStats.Add(specialStat);
            }

            return itemStats.OrderBy(x => random.Next()).Take(item.Stats.BonusStats.Count).ToList();
        }

        // Roll new values for existing bonus stats
        public static List<ItemStat> RollNewBonusValues(Item item, short ignoreStat, bool isSpecialStat)
        {
            List<ItemStat> newBonus = new List<ItemStat>();

            foreach (NormalStat stat in item.Stats.BonusStats.Where(x => x.GetType() == typeof(NormalStat)))
            {
                if (!isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
                {
                    newBonus.Add(stat);
                    continue;
                }

                Dictionary<ItemAttribute, List<ParserStat>> dictionary = GetRange(item.Id);
                if (!dictionary.ContainsKey(stat.ItemAttribute))
                {
                    continue;
                }
                newBonus.Add(new NormalStat(dictionary[stat.ItemAttribute][Roll(item.Level)]));
            }

            foreach (SpecialStat stat in item.Stats.BonusStats.Where(x => x.GetType() == typeof(SpecialStat)))
            {
                if (isSpecialStat && (short) stat.ItemAttribute == ignoreStat)
                {
                    newBonus.Add(stat);
                    continue;
                }

                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(item.Id);
                if (!dictionary.ContainsKey(stat.ItemAttribute))
                {
                    continue;
                }
                newBonus.Add(new SpecialStat(dictionary[stat.ItemAttribute][Roll(item.Level)]));
            }

            return newBonus;
        }

        private static Dictionary<ItemAttribute, List<ParserStat>> GetRange(int itemId)
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

        private static Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> GetSpecialRange(int itemId)
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
            int itemLevelFactor = ItemMetadataStorage.GetOptionLevelFactor(itemId);
            Random random = new Random();
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
                    _ => 15,
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
                _ => 7,
            };
        }
    }
}
