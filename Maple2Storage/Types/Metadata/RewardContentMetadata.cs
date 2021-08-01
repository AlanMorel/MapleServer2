using System.Collections.Generic;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class RewardContentMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public List<RewardContentItemMetadata> RewardItems = new List<RewardContentItemMetadata>();

        public RewardContentMetadata() { }
    }

    [XmlType]
    public class RewardContentItemMetadata
    {
        [XmlElement(Order = 1)]
        public int MinLevel;
        [XmlElement(Order = 2)]
        public int MaxLevel;
        [XmlElement(Order = 3)]
        public List<RewardItemData> Items = new List<RewardItemData>();

        public RewardContentItemMetadata() { }
    }

    [XmlType]
    public class RewardItemData
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public int Amount;
        [XmlElement(Order = 3)]
        public int Rarity;

        public RewardItemData() { }

        public RewardItemData(int id, int amount, int rarity)
        {
            Id = id;
            Amount = amount;
            Rarity = rarity;
        }
    }
}
