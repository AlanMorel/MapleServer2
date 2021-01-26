﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ItemMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public ItemSlot Slot;
        [XmlElement(Order = 3)]
        public GemSlot Gem;
        [XmlElement(Order = 4)]
        public InventoryTab Tab;
        [XmlElement(Order = 5)]
        public int Rarity;
        [XmlElement(Order = 6)]
        public int StackLimit;
        [XmlElement(Order = 7)]
        public bool IsTwoHand;
        [XmlElement(Order = 8)]
        public bool IsTemplate;
        [XmlElement(Order = 9)]
        public int PlayCount;
        [XmlElement(Order = 10)]
        public List<int> RecommendJobs = new List<int>();
        [XmlElement(Order = 11)]
        public List<ItemContent> Content;

        // Required for deserialization
        public ItemMetadata()
        {
            this.Content = new List<ItemContent>();
        }

        public override string ToString() =>
            $"ItemMetadata(Id:{Id},Slot:{Slot},GemSlot:{Gem},Tab:{Tab},Rarity:{Rarity},StackLimit:{StackLimit},IsTwoHand:{IsTwoHand},IsTemplate:{IsTemplate},PlayCount:{PlayCount}," +
            $"RecommendJobs:{string.Join(",", RecommendJobs)},Content:{string.Join(",", Content)})";

        protected bool Equals(ItemMetadata other)
        {
            return Id == other.Id && Slot == other.Slot && Gem == other.Gem && Tab == other.Tab && Rarity == other.Rarity &&
            StackLimit == other.StackLimit && IsTwoHand == other.IsTwoHand && IsTemplate == other.IsTemplate && PlayCount == other.PlayCount && Content.SequenceEqual(other.Content);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ItemMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Slot, Gem, Tab, Rarity, StackLimit);
        }

        public static bool operator ==(ItemMetadata left, ItemMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ItemMetadata left, ItemMetadata right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class ItemContent
    {
        [XmlElement(Order = 1)]
        public readonly int Id;
        [XmlElement(Order = 2)]
        public readonly int Id2;
        [XmlElement(Order = 3)]
        public readonly int MinAmount;
        [XmlElement(Order = 4)]
        public readonly int MaxAmount;
        [XmlElement(Order = 5)]
        public readonly int DropGroup;
        [XmlElement(Order = 6)]
        public readonly int SmartDropRate;
        [XmlElement(Order = 7)]
        public readonly int Rarity;
        [XmlElement(Order = 8)]
        public readonly int EnchantLevel;

        // Required for deserialization
        public ItemContent() { }

        public ItemContent(int id, int minAmount, int maxAmount, int dropGroup, int smartDropRate, int rarity, int enchant, int id2 = 0)
        {
            this.Id = id;
            this.Id2 = id2;
            this.MinAmount = minAmount;
            this.MaxAmount = maxAmount;
            this.DropGroup = dropGroup;
            this.SmartDropRate = smartDropRate;
            this.Rarity = rarity;
            this.EnchantLevel = enchant;
        }

        public override string ToString() =>
            $"ItemContent(Id:{Id},Id2:{Id2},MinAmount:{MinAmount},MaxAmount:{MaxAmount},DropGroup:{DropGroup},SmartDropRate:{SmartDropRate},Rarity:{Rarity},EnchantLevel:{EnchantLevel})";

        protected bool Equals(ItemContent other)
        {
            return Id == other.Id && Id2 == other.Id2 && MinAmount == other.MinAmount && MaxAmount == other.MaxAmount && DropGroup == other.DropGroup &&
                    SmartDropRate == other.SmartDropRate && Rarity == other.Rarity && EnchantLevel == other.EnchantLevel;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return Equals((ItemContent) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Id2, MinAmount, MaxAmount, DropGroup, SmartDropRate, Rarity, EnchantLevel);
        }

        public static bool operator ==(ItemContent left, ItemContent right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ItemContent left, ItemContent right)
        {
            return !Equals(left, right);
        }
    }
}
