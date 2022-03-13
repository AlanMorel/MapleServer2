using System.Xml.Serialization;
using Maple2Storage.Enums;
using MapleServer2.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name;
    [XmlElement(Order = 3)]
    public ItemSlot Slot;
    [XmlElement(Order = 4)]
    public GemSlot Gem;
    [XmlElement(Order = 5)]
    public MedalSlot Medal;
    [XmlElement(Order = 6)]
    public InventoryTab Tab;
    [XmlElement(Order = 7)]
    public int Rarity;
    [XmlElement(Order = 8)]
    public int StackLimit;
    [XmlElement(Order = 9)]
    public bool EnableBreak;
    [XmlElement(Order = 10)]
    public bool Sellable;
    [XmlElement(Order = 11)]
    public TransferType TransferType;
    [XmlElement(Order = 12)]
    public int TradeLimitByRarity;
    [XmlElement(Order = 13)]
    public byte TradeableCount;
    [XmlElement(Order = 14)]
    public bool DisableTradeWithinAccount;
    [XmlElement(Order = 15)]
    public byte RepackageCount;
    [XmlElement(Order = 16)]
    public byte RepackageItemConsumeCount;
    [XmlElement(Order = 17)]
    public bool IsTwoHand;
    [XmlElement(Order = 18)]
    public bool IsDress;
    [XmlElement(Order = 19)]
    public bool IsTemplate;
    [XmlElement(Order = 20)]
    public Gender Gender;
    [XmlElement(Order = 21)]
    public int PlayCount;
    [XmlElement(Order = 22)]
    public bool IsCustomScore;
    [XmlElement(Order = 23)]
    public List<long> SellPrice = new();
    [XmlElement(Order = 24)]
    public List<long> SellPriceCustom = new();
    [XmlElement(Order = 25)]
    public string FileName;
    [XmlElement(Order = 26)]
    public int SkillID;
    [XmlElement(Order = 27)]
    public List<int> RecommendJobs = new();
    [XmlElement(Order = 28)]
    public List<ItemBreakReward> BreakRewards = new();
    [XmlElement(Order = 29)]
    public ItemFunction FunctionData = new();
    [XmlElement(Order = 30)]
    public string Tag;
    [XmlElement(Order = 31)]
    public int ShopID;
    [XmlElement(Order = 32)]
    public int PetId;
    [XmlElement(Order = 33)]
    public int Level;
    [XmlElement(Order = 34)]
    public List<HairPresets> HairPresets = new();
    [XmlElement(Order = 35)]
    public int ColorIndex;
    [XmlElement(Order = 36)]
    public int ColorPalette;
    [XmlElement(Order = 37)]
    public int OptionStatic;
    [XmlElement(Order = 38)]
    public int OptionRandom;
    [XmlElement(Order = 39)]
    public int OptionConstant;
    [XmlElement(Order = 40)]
    public float OptionLevelFactor;
    [XmlElement(Order = 41)]
    public bool IsCubeSolid;
    [XmlElement(Order = 42)]
    public ItemHousingCategory HousingCategory;
    [XmlElement(Order = 43)]
    public int ObjectId;
    [XmlElement(Order = 44)]
    public string BlackMarketCategory;
    [XmlElement(Order = 45)]
    public string Category;

    public override string ToString()
    {
        return
            $"ItemMetadata(Id:{Id},Slot:{Slot},GemSlot:{Gem},Tab:{Tab},Rarity:{Rarity},StackLimit:{StackLimit},IsTwoHand:{IsTwoHand},IsTemplate:{IsTemplate},Gender{Gender},PlayCount:{PlayCount}," +
            $"IsCustomScore:{IsCustomScore},FileName:{FileName},SkillID:{SkillID},RecommendJobs:{string.Join(",", RecommendJobs)},Function:{FunctionData}," +
            $"Tag:{Tag},ShopID:{ShopID}";
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

    public override string ToString()
    {
        return $"Id: {Id}, Amount: {Count}";
    }
}

[XmlType]
public class ItemFunction
{
    [XmlElement(Order = 1)]
    public string Name;
    [XmlElement(Order = 2)]
    public int Id;
    [XmlElement(Order = 3)]
    public OpenItemBox OpenItemBox;
    [XmlElement(Order = 4)]
    public SelectItemBox SelectItemBox;
    [XmlElement(Order = 5)]
    public ChatEmoticonAdd ChatEmoticonAdd;
    [XmlElement(Order = 6)]
    public OpenMassiveEvent OpenMassiveEvent;
    [XmlElement(Order = 7)]
    public LevelPotion LevelPotion;
    [XmlElement(Order = 8)]
    public VIPCoupon VIPCoupon;
    [XmlElement(Order = 9)]
    public HongBaoData HongBao;
    [XmlElement(Order = 10)]
    public OpenCoupleEffectBox OpenCoupleEffectBox;
    [XmlElement(Order = 11)]
    public InstallBillboard InstallBillboard;
    [XmlElement(Order = 12)]
    public SurvivalSkin SurvivalSkin;
    [XmlElement(Order = 13)]
    public SurvivalLevelExp SurvivalLevelExp;

    public override string ToString()
    {
        return $"Function(Name: {Name}, Id: {Id}";
    }
}

[XmlType]
public class OpenItemBox
{
    [XmlElement(Order = 1)]
    public int RequiredItemId;
    [XmlElement(Order = 2)]
    public bool ReceiveOneItem;
    [XmlElement(Order = 3)]
    public int BoxId;
    [XmlElement(Order = 4)]
    public int AmountRequired;
}

[XmlType]
public class SelectItemBox
{
    [XmlElement(Order = 1)]
    public int GroupId;
    [XmlElement(Order = 2)]
    public int BoxId;
}

[XmlType]
public class ChatEmoticonAdd
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int Duration;
}

[XmlType]
public class OpenMassiveEvent
{
    [XmlElement(Order = 1)]
    public int FieldId;
    [XmlElement(Order = 2)]
    public int Capacity;
    [XmlElement(Order = 3)]
    public int Duration;
}

[XmlType]
public class LevelPotion
{
    [XmlElement(Order = 1)]
    public short TargetLevel;
}

[XmlType]
public class VIPCoupon
{
    [XmlElement(Order = 1)]
    public int Duration;
}

[XmlType]
public class HongBaoData
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public short Count;
    [XmlElement(Order = 3)]
    public byte TotalUsers;
    [XmlElement(Order = 4)]
    public int Duration;
}

[XmlType]
public class OpenCoupleEffectBox
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public int Rarity;
}

[XmlType]
public class InstallBillboard
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

    public override string ToString()
    {
        return $"AdBalloonData(InteractId:{InteractId}, Model:{Model}, Asset:{Asset}, " +
               $"NormalState:{NormalState}, Reactable:{Reactable}, Scale:{Scale}, Duration:{Duration})";
    }
}

[XmlType]
public class SurvivalSkin
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public MedalSlot Slot;
}

[XmlType]
public class SurvivalLevelExp
{
    [XmlElement(Order = 1)]
    public int SurvivalExp;
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

    public override string ToString()
    {
        return $"HairPreset(BackPositionCoord: {BackPositionCoord}, BackPositionRotation: {BackPositionRotation}, " +
               $"FrontPositionCoord: {FrontPositionCoord}, FrontPositionRotation: {FrontPositionRotation}), MinScale:{MinScale}, MaxScale:{MaxScale}";
    }
}
