using System.Collections.Generic;
using System.Runtime.InteropServices;
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
            this.Id = id;
        }
    }

    public class ItemStats
    {
        public readonly List<ItemStat> BasicAttributes;
        public readonly List<ItemStat> BonusAttributes;

        public byte TotalSockets;
        public readonly List<Gemstone> Gemstones;

        public ItemStats()
        {
            this.BasicAttributes = new List<ItemStat>();
            this.BonusAttributes = new List<ItemStat>();
            this.Gemstones = new List<Gemstone>();
        }

        public ItemStats(ItemStats other)
        {
            BasicAttributes = new List<ItemStat>(other.BasicAttributes);
            BonusAttributes = new List<ItemStat>(other.BonusAttributes);
            TotalSockets = other.TotalSockets;
            Gemstones = new List<Gemstone>(other.Gemstones);
        }
    }
}
