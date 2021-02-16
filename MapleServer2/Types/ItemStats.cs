using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct ItemStat
    {
        public ItemAttribute Type { get; private set; }
        public int Value { get; private set; }
        public float Percent { get; private set; }

        public static ItemStat Of(ItemAttribute type, int value)
        {
            return new ItemStat
            {
                Type = type,
                Value = value,
                Percent = 0,
            };
        }

        public static ItemStat Of(ItemAttribute type, float percent)
        {
            return new ItemStat
            {
                Type = type,
                Value = 0,
                Percent = percent,
            };
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 2, Size = 10)]
    public struct SpecialItemStat
    {
        public ItemSpecialAttribute Type { get; private set; }
        public float Value { get; private set; }
        public float Unknown { get; private set; }

        public static SpecialItemStat Of(ItemSpecialAttribute type, float value)
        {
            return new SpecialItemStat
            {
                Type = type,
                Value = value,
                Unknown = 0,
            };
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
        public readonly List<ItemStat> BasicAttributes;
        public readonly List<ItemStat> BonusAttributes;

        public byte TotalSockets;
        public readonly List<Gemstone> Gemstones;

        public ItemStats(int itemId, int rarity)
        {
            BasicAttributes = new List<ItemStat>();
            BonusAttributes = new List<ItemStat>();
            Gemstones = new List<Gemstone>();
            if (rarity == 0)
            {
                return;
            }

            if (ItemStatsMetadataStorage.GetBasic(itemId, out List<ItemOptions> basicList))
            {
                ItemOptions itemoption = basicList.Find(options => options.Rarity == rarity);
                if (itemoption != null)
                {
                    AddStat(BasicAttributes, itemoption.Stats);
                }
            }

            if (ItemStatsMetadataStorage.GetRandomBonus(itemId, out List<ItemOptions> randomBonusList))
            {
                ItemOptions itemoption = randomBonusList.Find(options => options.Rarity == rarity && options.Slots > 0);
                if (itemoption != null)
                {
                    List<Stat> indexes = GetRandomOptions(itemoption, itemoption.Slots);
                    AddStat(BonusAttributes, indexes);
                }
            }
        }

        public ItemStats(ItemStats other)
        {
            BasicAttributes = new List<ItemStat>(other.BasicAttributes);
            BonusAttributes = new List<ItemStat>(other.BonusAttributes);
            TotalSockets = other.TotalSockets;
            Gemstones = new List<Gemstone>(other.Gemstones);
        }

        private static List<Stat> GetRandomOptions(ItemOptions list, int rolls)
        {
            Random random = new Random();
            return list.Stats.OrderBy(x => random.Next()).Take(rolls).ToList();
        }

        private static void AddStat(List<ItemStat> listAttributes, List<Stat> listStats)
        {
            foreach (Stat stat in listStats)
            {
                if (stat.Value != 0)
                {
                    listAttributes.Add(ItemStat.Of(stat.Type, stat.Value)); // TODO: add randomness to value
                }
                else
                {
                    listAttributes.Add(ItemStat.Of(stat.Type, stat.Percentage)); // TODO: add randomness to value
                }
            }
        }
    }
}
