using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types {
    [XmlType]
    public class ItemMetadata {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public ItemSlot Slot;
        [XmlElement(Order = 3)]
        public InventoryTab Tab;
        [XmlElement(Order = 4)]
        public int SlotMax;
        [XmlElement(Order = 5)]
        public bool IsTemplate;

        protected bool Equals(ItemMetadata other) {
            return Id == other.Id && Slot == other.Slot && Tab == other.Tab && SlotMax == other.SlotMax && IsTemplate == other.IsTemplate;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemMetadata) obj);
        }

        public override int GetHashCode() {
            return HashCode.Combine(Id, Slot, Tab, SlotMax, IsTemplate);
        }

        public static bool operator ==(ItemMetadata left, ItemMetadata right) {
            return Equals(left, right);
        }

        public static bool operator !=(ItemMetadata left, ItemMetadata right) {
            return !Equals(left, right);
        }
    }
}