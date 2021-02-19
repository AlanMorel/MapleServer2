using System;
using System.Xml.Serialization;

namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class RecipeMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public string MasteryType;
        [XmlElement(Order = 3)]
        public bool ExceptRewardExp;
        [XmlElement(Order = 4)]
        public string RequireMastery;
        [XmlElement(Order = 5)]
        public string RequireMeso;
        [XmlElement(Order = 6)]
        public string RequireQuest;
        [XmlElement(Order = 7)]
        public string RewardExp;
        [XmlElement(Order = 8)]
        public long RewardMastery;
        [XmlElement(Order = 9)]
        public string GatheringTime;
        [XmlElement(Order = 10)]
        public int HighPropLimitCount;
        [XmlElement(Order = 11)]
        public int NormalPropLimitCount;
        [XmlElement(Order = 12)]
        public string RequireItem1;
        [XmlElement(Order = 13)]
        public string RequireItem2;
        [XmlElement(Order = 14)]
        public string RequireItem3;
        [XmlElement(Order = 15)]
        public string RequireItem4;
        [XmlElement(Order = 16)]
        public string RequireItem5;
        [XmlElement(Order = 17)]
        public int HabitatMapId;
        [XmlElement(Order = 18)]
        public string RewardItem1;
        [XmlElement(Order = 19)]
        public string RewardItem2;
        [XmlElement(Order = 20)]
        public string RewardItem3;
        [XmlElement(Order = 21)]
        public string RewardItem4;
        [XmlElement(Order = 22)]
        public string RewardItem5;
        [XmlElement(Order = 23)]
        public string Feature;

        // Required for deserialization
        public RecipeMetadata()
        {
        }

        public override string ToString() =>
            $"RecipeMetadata(Id:{Id}, MasteryType:{MasteryType}, RequireItem1: {RequireItem1}, RewardItem1: {RewardItem1}";

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((RecipeMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, MasteryType, RequireItem1, RewardItem1);
        }

        public static bool operator ==(RecipeMetadata left, RecipeMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RecipeMetadata left, RecipeMetadata right)
        {
            return !Equals(left, right);
        }
    }
}
