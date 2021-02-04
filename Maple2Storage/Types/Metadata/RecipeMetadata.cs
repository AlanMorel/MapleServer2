using System;
using System.Collections.Generic;
using System.Xml.Serialization;
namespace Maple2Storage.Types.Metadata
{
    [XmlType]
    public class RecipeMetadata
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public byte MasteryType;
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

        protected bool Equals(RecipeMetadata other)
        {
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((RecipeMetadata) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, MasteryType, RequireItem1, RewardItem1);
        }

        public List<RecipeItem> GetIngredients()
        {
            List<RecipeItem> result = new List<RecipeItem>();
            List<string> requiredItems = new List<string>
            {
                RequireItem1,
                RequireItem2,
                RequireItem3,
                RequireItem4,
                RequireItem5
            };

            foreach (string requiredItem in requiredItems)
            {
                if (!string.IsNullOrEmpty(requiredItem))
                {
                    int[] split = SplitStringIntoInts(requiredItem);
                    result.Add(new RecipeItem
                    {
                        Id = split[0],
                        Amount = split[2]
                    });
                }
            }

            return result;
        }

        public List<RecipeItem> GetResult()
        {
            List<RecipeItem> result = new List<RecipeItem>();
            List<string> rewardItems = new List<string>
            {
                RewardItem1,
                RewardItem2,
                RewardItem3,
                RewardItem4,
                RewardItem5
            };

            foreach (string rewardItem in rewardItems)
            {
                if (!string.IsNullOrEmpty(rewardItem))
                {
                    int[] split = SplitStringIntoInts(rewardItem);
                    result.Add(new RecipeItem
                    {
                        Id = split[0],
                        Rarity = split[1],
                        Amount = split[2]
                    });
                }
            }

            return result;
        }

        public static bool operator ==(RecipeMetadata left, RecipeMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RecipeMetadata left, RecipeMetadata right)
        {
            return !Equals(left, right);
        }

        static int[] SplitStringIntoInts(string list)
        {
            string[] split = list.Split(new char[1] { ',' });
            List<int> numbers = new List<int>();

            foreach (string n in split)
            {
                if (int.TryParse(n, out int parsed))
                    numbers.Add(parsed);
            }

            return numbers.ToArray();
        }
    }

    public class RecipeItem
    {
        public long Uid;
        public int Id;
        public int Rarity;
        public int Amount;

        public RecipeItem()
        {

        }
    }
}
