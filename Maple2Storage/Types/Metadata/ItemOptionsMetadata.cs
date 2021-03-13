using System.Collections.Generic;
using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ItemOptionsMetadata
    {
        [XmlElement(Order = 1)]
        public int ItemId;
        [XmlElement(Order = 2)]
        public List<ItemOptions> Basic = new List<ItemOptions>();
        [XmlElement(Order = 3)]
        public List<ItemOptions> StaticBonus = new List<ItemOptions>();
        [XmlElement(Order = 4)]
        public List<ItemOptions> RandomBonus = new List<ItemOptions>();

        public ItemOptionsMetadata() { }
    }

    [XmlType]
    public class ItemOptions
    {
        [XmlElement(Order = 1)]
        public byte Rarity;
        [XmlElement(Order = 2)]
        public byte Slots;
        [XmlElement(Order = 3)]
        public float MultiplyFactor;
        [XmlElement(Order = 4)]
        public List<ItemAttribute> Stats = new List<ItemAttribute>();
        [XmlElement(Order = 5)]
        public List<SpecialItemAttribute> SpecialStats = new List<SpecialItemAttribute>();
        [XmlElement(Order = 6)]
        public int MinWeaponAtk;
        [XmlElement(Order = 7)]
        public int MaxWeaponAtk;

        public ItemOptions() { }

        public override string ToString() => $"Rarity {Rarity}, Slots {Slots}, MultiplyFactor {MultiplyFactor}," +
        $"MinWeaponAtk {MinWeaponAtk}, MaxWeaponAtk {MaxWeaponAtk}, Stats: ({string.Join(",", Stats)}, SpecialStats: ({string.Join(",", SpecialStats)})";
    }

}
