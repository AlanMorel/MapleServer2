using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

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
        public int SlotMax;
        [XmlElement(Order = 7)]
        public bool IsTemplate;
        [XmlElement(Order = 8)]
        public int PlayCount;
        [XmlElement(Order = 9)]
        public List<int> RecommendJobs = new List<int>();
        [XmlElement(Order = 10)]
        public List<ItemContent> Content;

        // Required for deserialization
        public ItemMetadata()
        {
            this.Content = new List<ItemContent>();
        }

        public override string ToString() =>
            $"ItemMetadata(Id:{Id},Slot:{Slot},GemSlot:{Gem},Tab:{Tab},Rarity:{Rarity},SlotMax:{SlotMax},IsTemplate:{IsTemplate},PlayCount:{PlayCount},RecommendJobs:{string.Join(",", RecommendJobs)},Content:{string.Join(",", Content)})";

        protected bool Equals(ItemMetadata other)
        {
            return Id == other.Id && Slot == other.Slot && Gem == other.Gem && Tab == other.Tab && Rarity == other.Rarity &&
            SlotMax == other.SlotMax && IsTemplate == other.IsTemplate && PlayCount == other.PlayCount && Content.SequenceEqual(other.Content);
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
            return HashCode.Combine(Id, Slot, Gem, Tab, Rarity, SlotMax);
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
        public readonly int Amount;

        // Required for deserialization
        public ItemContent() { }

        public ItemContent(int id, int amount)
        {
            this.Id = id;
            this.Amount = amount;
        }

        public override string ToString() =>
            $"ItemContent(Id:{Id},Amount:{Amount})";

        protected bool Equals(ItemContent other)
        {
            return Id == other.Id && Amount == other.Amount;
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
            return HashCode.Combine(Id, Amount);
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
