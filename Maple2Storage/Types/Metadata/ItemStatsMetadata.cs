using System.Collections.Generic;
using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ItemStatsMetadata
    {
        [XmlElement(Order = 1)]
        public int ItemId;
        [XmlElement(Order = 2)]
        public List<ItemOptions> Basic = new List<ItemOptions>();
        [XmlElement(Order = 3)]
        public List<ItemOptions> StaticBonus = new List<ItemOptions>();
        [XmlElement(Order = 4)]
        public List<ItemOptions> RandomBonus = new List<ItemOptions>();

        public ItemStatsMetadata() { }
    }

    [XmlType]
    public class ItemOptions
    {
        [XmlElement(Order = 1)]
        public byte Grade;
        [XmlElement(Order = 2)]
        public byte Slots;
        [XmlElement(Order = 3)]
        public float MultiplyFactor;
        [XmlElement(Order = 4)]
        public List<Stat> Stats = new List<Stat>();

        public ItemOptions() { }
    }

    [XmlType]
    public class Stat
    {
        [XmlElement(Order = 1)]
        public ItemAttribute Type;
        [XmlElement(Order = 2)]
        public int Value;
        [XmlElement(Order = 3)]
        public float Percentage;

        public Stat() { }

        public Stat(ItemAttribute type, int value)
        {
            Type = type;
            Value = value;
            Percentage = 0;
        }
        public Stat(ItemAttribute type, float percent)
        {
            Type = type;
            Value = 0;
            Percentage = percent;
        }
    }
}
