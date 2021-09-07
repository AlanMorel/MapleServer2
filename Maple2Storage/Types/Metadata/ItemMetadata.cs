using System.Xml.Serialization;
using Maple2Storage.Enums;
using MapleServer2.Enums;

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
        public bool EnableBreak;
        [XmlElement(Order = 8)]
        public bool Sellable;
        [XmlElement(Order = 9)]
        public TransferType TransferType;
        [XmlElement(Order = 10)]
        public byte TradeableCount;
        [XmlElement(Order = 11)]
        public byte RepackageCount;
        [XmlElement(Order = 12)]
        public byte RepackageItemConsumeCount;
        [XmlElement(Order = 13)]
        public bool IsTwoHand;
        [XmlElement(Order = 14)]
        public bool IsDress;
        [XmlElement(Order = 15)]
        public bool IsTemplate;
        [XmlElement(Order = 16)]
        public byte Gender;
        [XmlElement(Order = 17)]
        public int PlayCount;
        [XmlElement(Order = 18)]
        public bool IsCustomScore;
        [XmlElement(Order = 19)]
        public List<int> SellPrice = new List<int>();
        [XmlElement(Order = 20)]
        public List<int> SellPriceCustom = new List<int>();
        [XmlElement(Order = 21)]
        public string FileName;
        [XmlElement(Order = 22)]
        public int SkillID;
        [XmlElement(Order = 23)]
        public List<int> RecommendJobs = new List<int>();
        [XmlElement(Order = 24)]
        public List<ItemContent> Content;
        [XmlElement(Order = 25)]
        public List<ItemBreakReward> BreakRewards;
        [XmlElement(Order = 26)]
        public ItemFunction FunctionData;
        [XmlElement(Order = 27)]
        public AdBalloonData AdBalloonData;
        [XmlElement(Order = 28)]
        public string Tag;
        [XmlElement(Order = 29)]
        public int ShopID;
        [XmlElement(Order = 30)]
        public int Level;
        [XmlElement(Order = 31)]
        public List<HairPresets> HairPresets = new List<HairPresets>();
        [XmlElement(Order = 32)]
        public int ColorIndex;
        [XmlElement(Order = 33)]
        public int ColorPalette;
        [XmlElement(Order = 34)]
        public int OptionStatic;
        [XmlElement(Order = 35)]
        public int OptionRandom;
        [XmlElement(Order = 36)]
        public int OptionConstant;
        [XmlElement(Order = 37)]
        public int OptionLevelFactor;
        [XmlElement(Order = 38)]
        public bool IsCubeSolid;
        [XmlElement(Order = 39)]
        public ItemHousingCategory HousingCategory;

        // Required for deserialization
        public ItemMetadata()
        {
            Content = new List<ItemContent>();
            BreakRewards = new List<ItemBreakReward>();
            FunctionData = new ItemFunction();
        }

        public override string ToString() =>
            $"ItemMetadata(Id:{Id},Slot:{Slot},GemSlot:{Gem},Tab:{Tab},Rarity:{Rarity},StackLimit:{StackLimit},IsTwoHand:{IsTwoHand},IsTemplate:{IsTemplate},Gender{Gender},PlayCount:{PlayCount}," +
            $"IsCustomScore:{IsCustomScore},FileName:{FileName},SkillID:{SkillID},RecommendJobs:{string.Join(",", RecommendJobs)},Content:{string.Join(",", Content)},Function:{FunctionData}," +
            $"AdBalloonData:{AdBalloonData},Tag:{Tag},ShopID:{ShopID}";

        protected bool Equals(ItemMetadata other)
        {
            return Id == other.Id && Slot == other.Slot && Gem == other.Gem && Tab == other.Tab && Rarity == other.Rarity &&
            StackLimit == other.StackLimit && IsTwoHand == other.IsTwoHand && IsTemplate == other.IsTemplate && other.IsCustomScore == IsCustomScore
            && PlayCount == other.PlayCount && FileName == other.FileName && SkillID == other.SkillID && Content.SequenceEqual(other.Content);
        }

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
            Id = id;
            Id2 = id2;
            MinAmount = minAmount;
            MaxAmount = maxAmount;
            DropGroup = dropGroup;
            SmartDropRate = smartDropRate;
            Rarity = rarity;
            EnchantLevel = enchant;
        }

        public override string ToString() =>
            $"ItemContent(Id:{Id},Id2:{Id2},MinAmount:{MinAmount},MaxAmount:{MaxAmount},DropGroup:{DropGroup},SmartDropRate:{SmartDropRate},Rarity:{Rarity},EnchantLevel:{EnchantLevel})";

        protected bool Equals(ItemContent other)
        {
            return Id == other.Id && Id2 == other.Id2 && MinAmount == other.MinAmount && MaxAmount == other.MaxAmount &&
                   DropGroup == other.DropGroup &&
                   SmartDropRate == other.SmartDropRate && Rarity == other.Rarity &&
                   EnchantLevel == other.EnchantLevel;
        }

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

    [XmlType]
    public class ItemBreakReward
    {
        [XmlElement(Order = 1)]
        public int Id;
        [XmlElement(Order = 2)]
        public int Count;

        public ItemBreakReward() { }

        public ItemBreakReward(int id, int count)
        {
            Id = id;
            Count = count;
        }

        public override string ToString() => $"Id: {Id}, Amount: {Count}";
    }

    [XmlType]
    public class ItemFunction
    {
        [XmlElement(Order = 1)]
        public string Name;
        [XmlElement(Order = 2)]
        public int Id;
        [XmlElement(Order = 3)]
        public int Duration;
        [XmlElement(Order = 4)]
        public int FieldId;
        [XmlElement(Order = 5)]
        public byte Capacity;
        [XmlElement(Order = 6)]
        public byte TargetLevel;
        [XmlElement(Order = 7)]
        public short Count;
        [XmlElement(Order = 8)]
        public byte TotalUser;
        [XmlElement(Order = 9)]
        public byte Rarity;

        public ItemFunction() { }

        public ItemFunction(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public override string ToString() => $"Function(Name: {Name}, Id: {Id}, " +
            $"Duration: {Duration}, FieldId: {FieldId}), Capacity:{Capacity}, TargetLevel:{TargetLevel}, Count:{Count}, TotalUser:{TotalUser}, Rarity:{Rarity}";

        protected bool Equals(ItemFunction other)
        {
            return Name == other.Name &&
                Id == other.Id &&
                Duration == other.Duration &&
                FieldId == other.FieldId &&
                Capacity == other.Capacity &&
                TargetLevel == other.TargetLevel &&
                Count == other.Count &&
                TotalUser == other.TotalUser &&
                Rarity == other.Rarity;
        }

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

            return Equals((ItemFunction) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }

        public static bool operator ==(ItemFunction left, ItemFunction right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ItemFunction left, ItemFunction right)
        {
            return !Equals(left, right);
        }
    }

    [XmlType]
    public class AdBalloonData
    {
        [XmlElement(Order = 1)]
        public int InteractId;
        [XmlElement(Order = 2)]
        public string Model;
        [XmlElement(Order = 3)]
        public string Asset = "";
        [XmlElement(Order = 4)]
        public string NormalState;
        [XmlElement(Order = 5)]
        public string Reactable;
        [XmlElement(Order = 6)]
        public float Scale = 1;
        [XmlElement(Order = 7)]
        public int Duration;

        public AdBalloonData() { }

        protected bool Equals(AdBalloonData other)
        {
            return InteractId == other.InteractId &&
                Model == other.Model &&
                Asset == other.Asset &&
                NormalState == other.NormalState &&
                Reactable == other.Reactable &&
                Scale == other.Scale &&
                Duration == other.Duration;
        }

        public override string ToString() => $"AdBalloonData(InteractId:{InteractId}, Model:{Model}, Asset:{Asset}, " +
            $"NormalState:{NormalState}, Reactable:{Reactable}, Scale:{Scale}, Duration:{Duration})";
    }

    [XmlType]
    public class HairPresets
    {
        [XmlElement(Order = 1)]
        public CoordF BackPositionCoord;
        [XmlElement(Order = 2)]
        public CoordF BackPositionRotation;
        [XmlElement(Order = 3)]
        public CoordF FrontPositionCoord;
        [XmlElement(Order = 4)]
        public CoordF FrontPositionRotation;
        [XmlElement(Order = 5)]
        public float MinScale;
        [XmlElement(Order = 6)]
        public float MaxScale;

        public HairPresets() { }

        public override string ToString() => $"HairPreset(BackPositionCoord: {BackPositionCoord}, BackPositionRotation: {BackPositionRotation}, " +
            $"FrontPositionCoord: {FrontPositionCoord}, FrontPositionRotation: {FrontPositionRotation}), MinScale:{MinScale}, MaxScale:{MaxScale}";
    }
}
