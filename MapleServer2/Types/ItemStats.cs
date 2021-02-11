using System;
using System.Collections.Generic;
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

        public ItemStats(int itemId, int grade)
        {
            BasicAttributes = new List<ItemStat>();
            BonusAttributes = new List<ItemStat>();
            Gemstones = new List<Gemstone>();
            if (grade == 0)
            {
                return;
            }
            if (ItemStatsMetadataStorage.GetConstantStat(itemId, out List<ItemOptions> constantList))
            {
                foreach (ItemOptions item in constantList)
                {
                    if (item.Grade != grade)
                    {
                        continue;
                    }
                    foreach (Stat i in item.Stats)
                    {
                        if (i.Value != 0)
                        {
                            BasicAttributes.Add(ItemStat.Of(i.Type, i.Value)); // TODO: add randomness to value
                        }
                        else
                        {
                            BasicAttributes.Add(ItemStat.Of(i.Type, i.Percentage)); // TODO: add randomness to value
                        }
                    }
                }
            }

            if (ItemStatsMetadataStorage.GetRandomStat(itemId, out List<ItemOptions> randomList))
            {
                foreach (ItemOptions item in randomList)
                {
                    if (item.Grade != grade)
                    {
                        continue;
                    }
                    if (item.OptionNumPick == 0)
                    {
                        continue;
                    }
                    List<int> indexes = GetRandomOptions(item.OptionNumPick, item.Stats.Count);
                    foreach (int i in indexes)
                    {
                        if (item.Stats[i].Value != 0)
                        {
                            BonusAttributes.Add(ItemStat.Of(item.Stats[i].Type, item.Stats[i].Value)); // TODO: add randomness to value
                        }
                        else
                        {
                            BonusAttributes.Add(ItemStat.Of(item.Stats[i].Type, item.Stats[i].Percentage)); // TODO: add randomness to value
                        }
                    }
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

        private static List<int> GetRandomOptions(int rolls, int optionsSize)
        {
            List<int> list = new List<int>();
            Random random = new Random();

            if (rolls > optionsSize)
            {
                for (int i = 0; i < optionsSize; i++)
                {
                    list.Add(i);
                }
                return list;
            }

            do
            {
                int rng = random.Next(0, optionsSize);
                if (!list.Contains(rng))
                {
                    list.Add(rng);
                }
            } while (list.Count < rolls);

            return list;
        }
    }
}
