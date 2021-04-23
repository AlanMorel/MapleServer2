﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types
{
    public interface ItemStat
    {
        short GetId();
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct NormalStat : ItemStat
    {
        public ItemAttribute Id { get; set; }
        public int Flat { get; set; }
        public float Percent { get; set; }

        public static NormalStat Of(ItemAttribute type, int flat)
        {
            return new NormalStat
            {
                Id = type,
                Flat = flat,
                Percent = 0,
            };
        }

        public static NormalStat Of(ItemAttribute type, float percent)
        {
            return new NormalStat
            {
                Id = type,
                Flat = 0,
                Percent = percent,
            };
        }

        public static NormalStat Of(ParserStat stat)
        {
            return new NormalStat
            {
                Id = stat.Id,
                Flat = stat.Flat,
                Percent = stat.Percent,
            };
        }

        short ItemStat.GetId()
        {
            return (short) Id;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct SpecialStat : ItemStat
    {
        public SpecialItemAttribute Id { get; set; }
        public float Percent { get; set; }
        public float Flat { get; set; }

        public static SpecialStat Of(SpecialItemAttribute type, float percent, float flat)
        {
            return new SpecialStat
            {
                Id = type,
                Percent = percent,
                Flat = flat,
            };
        }

        public static SpecialStat Of(ParserSpecialStat stat)
        {
            return new SpecialStat
            {
                Id = stat.Id,
                Percent = stat.Percent,
                Flat = stat.Flat,
            };
        }

        short ItemStat.GetId()
        {
            return (short) Id;
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
            CreateNewStats(item.Id, item.Rarity, item.Level, item.IsTwoHand);
        }

        public ItemStats(int itemId, int rarity, int level, bool isTwoHand)
        {
            CreateNewStats(itemId, rarity, level, isTwoHand);
        }

        public ItemStats(ItemStats other)
        {
            BasicStats = new List<ItemStat>(other.BasicStats);
            BonusStats = new List<ItemStat>(other.BonusStats);
            TotalSockets = other.TotalSockets;
            Gemstones = new List<Gemstone>(other.Gemstones);
        }

        public void CreateNewStats(int itemId, int rarity, int level, bool isTwoHand)
        {
            BasicStats = new List<ItemStat>();
            BonusStats = new List<ItemStat>();
            Gemstones = new List<Gemstone>();
            if (rarity == 0)
            {
                return;
            }

            List<ItemStat> basicStats = GetBasicStats(itemId, rarity, level, isTwoHand);
            if (basicStats != null)
            {
                foreach (ItemStat stat in basicStats)
                {
                    BasicStats.Add(stat);
                }
            }

            List<ItemStat> bonusStats = GetBonusStats(itemId, rarity, level, isTwoHand);
            if (bonusStats != null)
            {
                foreach (ItemStat stat in bonusStats)
                {
                    BonusStats.Add(stat);
                }
            }
        }

        // Get basic stats
        public List<ItemStat> GetBasicStats(int itemId, int rarity, int level, bool isTwoHand)
        {
            if (!ItemOptionsMetadataStorage.GetBasic(itemId, out List<ItemOption> basicList))
            {
                return null;
            }
            ItemOption itemOptions = basicList.Find(options => options.Rarity == rarity);
            if (itemOptions == null)
            {
                return null;
            }

            // Weapon and pet atk comes from each Item option and not from stat ranges
            if (itemOptions.MaxWeaponAtk != 0)
            {
                BasicStats.Add(NormalStat.Of(ItemAttribute.MinWeaponAtk, itemOptions.MinWeaponAtk));
                BasicStats.Add(NormalStat.Of(ItemAttribute.MaxWeaponAtk, itemOptions.MaxWeaponAtk));
            }
            if (itemOptions.PetAtk != 0)
            {
                BasicStats.Add(NormalStat.Of(ItemAttribute.PetBonusAtk, itemOptions.PetAtk));
            }

            List<ItemStat> itemStats = RollStats(itemId, level, isTwoHand, itemOptions);

            return itemStats;
        }

        // Get bonus stats
        public static List<ItemStat> GetBonusStats(int itemId, int rarity, int level, bool isTwoHand)
        {
            if (!ItemOptionsMetadataStorage.GetRandomBonus(itemId, out List<ItemOption> randomBonusList))
            {
                return null;
            }

            Random random = new Random();
            ItemOption itemOption = randomBonusList.FirstOrDefault(options => options.Rarity == rarity && options.Slots > 0);
            if (itemOption == null)
            {
                return null;
            }

            List<ItemStat> itemStats = RollStats(itemId, level, isTwoHand, itemOption);

            return itemStats.OrderBy(x => random.Next()).Take(itemOption.Slots).ToList();
        }

        // Get random values for each stat and check if item is two-handed
        private static List<ItemStat> RollStats(int itemId, int level, bool isTwoHand, ItemOption itemOption)
        {
            List<ItemStat> itemStats = new List<ItemStat>();

            foreach (ItemAttribute attribute in itemOption.Stats)
            {
                Dictionary<ItemAttribute, List<ParserStat>> dictionary = GetRange(itemId);
                if (!dictionary.ContainsKey(attribute))
                {
                    continue;
                }

                NormalStat normalStat = NormalStat.Of(dictionary[attribute][Roll(level)]);
                if (isTwoHand)
                {
                    normalStat.Flat *= 2;
                    normalStat.Percent *= 2;
                }
                itemStats.Add(normalStat);
            }

            foreach (SpecialItemAttribute attribute in itemOption.SpecialStats)
            {
                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(itemId);
                if (!dictionary.ContainsKey(attribute))
                {
                    continue;
                }

                SpecialStat specialStat = SpecialStat.Of(dictionary[attribute][Roll(level)]);
                if (isTwoHand)
                {
                    specialStat.Flat *= 2;
                    specialStat.Percent *= 2;
                }
                itemStats.Add(specialStat);
            }

            return itemStats;
        }

        // Roll new bonus stats and values except the locked stat
        public static List<ItemStat> RollBonusStatsWithStatLocked(Item item, short ignoreStat, bool isSpecialStat)
        {
            int id = item.Id;
            int level = item.Level;
            bool isTwoHand = item.IsTwoHand;

            if (!ItemOptionsMetadataStorage.GetRandomBonus(id, out List<ItemOption> randomBonusList))
            {
                return null;
            }

            Random random = new Random();
            ItemOption itemOption = randomBonusList.FirstOrDefault(options => options.Rarity == item.Rarity && options.Slots > 0);
            if (itemOption == null)
            {
                return null;
            }

            List<ItemStat> itemStats = new List<ItemStat>();

            List<ItemAttribute> attributes = isSpecialStat ? itemOption.Stats : itemOption.Stats.Where(x => (short) x != ignoreStat).ToList();
            List<SpecialItemAttribute> specialAttributes = isSpecialStat ? itemOption.SpecialStats.Where(x => (short) x != ignoreStat).ToList() : itemOption.SpecialStats;

            foreach (ItemAttribute attribute in attributes)
            {
                Dictionary<ItemAttribute, List<ParserStat>> dictionary = GetRange(id);
                if (!dictionary.ContainsKey(attribute))
                {
                    continue;
                }

                NormalStat normalStat = NormalStat.Of(dictionary[attribute][Roll(level)]);
                if (isTwoHand)
                {
                    normalStat.Flat *= 2;
                    normalStat.Percent *= 2;
                }
                itemStats.Add(normalStat);
            }

            foreach (SpecialItemAttribute attribute in specialAttributes)
            {
                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(id);
                if (!dictionary.ContainsKey(attribute))
                {
                    continue;
                }

                SpecialStat specialStat = SpecialStat.Of(dictionary[attribute][Roll(level)]);
                if (isTwoHand)
                {
                    specialStat.Flat *= 2;
                    specialStat.Percent *= 2;
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
                if (!isSpecialStat && (short) stat.Id == ignoreStat)
                {
                    newBonus.Add(stat);
                    continue;
                }

                Dictionary<ItemAttribute, List<ParserStat>> dictionary = GetRange(item.Id);
                if (!dictionary.ContainsKey(stat.Id))
                {
                    continue;
                }
                newBonus.Add(NormalStat.Of(dictionary[stat.Id][Roll(item.Level)]));
            }

            foreach (SpecialStat stat in item.Stats.BonusStats.Where(x => x.GetType() == typeof(SpecialStat)))
            {
                if (isSpecialStat && (short) stat.Id == ignoreStat)
                {
                    newBonus.Add(stat);
                    continue;
                }

                Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> dictionary = GetSpecialRange(item.Id);
                if (!dictionary.ContainsKey(stat.Id))
                {
                    continue;
                }
                newBonus.Add(SpecialStat.Of(dictionary[stat.Id][Roll(item.Level)]));
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
        private static int Roll(int level)
        {
            Random random = new Random();
            if (level >= 70)
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
