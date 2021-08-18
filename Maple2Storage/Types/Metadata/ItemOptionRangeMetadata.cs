using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class ItemOptionRangeMetadata
    {
        [XmlElement(Order = 1)]
        public ItemOptionRangeType RangeType;
        [XmlElement(Order = 2)]
        public Dictionary<ItemAttribute, List<ParserStat>> Stats;
        [XmlElement(Order = 3)]
        public Dictionary<SpecialItemAttribute, List<ParserSpecialStat>> SpecialStats;

        public ItemOptionRangeMetadata()
        {
            Stats = new Dictionary<ItemAttribute, List<ParserStat>>();
            SpecialStats = new Dictionary<SpecialItemAttribute, List<ParserSpecialStat>>();
        }
    }


    [XmlType]
    public class ParserStat
    {
        [XmlElement(Order = 1)]
        public ItemAttribute Id;
        [XmlElement(Order = 2)]
        public int Flat;
        [XmlElement(Order = 3)]
        public float Percent;

        public ParserStat() { }

        public ParserStat(ItemAttribute type, int flat)
        {
            Id = type;
            Flat = flat;
            Percent = 0;
        }
        public ParserStat(ItemAttribute type, float percent)
        {
            Id = type;
            Flat = 0;
            Percent = percent;
        }

        public override string ToString() => $"Id: {Id}, Flat: {Flat}, Percent: {Percent}";
    }

    [XmlType]
    public class ParserSpecialStat
    {
        [XmlElement(Order = 1)]
        public SpecialItemAttribute Id;
        [XmlElement(Order = 2)]
        public float Percent;
        [XmlElement(Order = 3)]
        public float Flat;

        public ParserSpecialStat() { }

        public ParserSpecialStat(SpecialItemAttribute id, float percent, float flat)
        {
            Id = id;
            Percent = percent;
            Flat = flat;
        }

        public override string ToString() => $"Id: {Id}, Flat: {Flat}, Percent: {Percent}";
    }
}
