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
    public InventoryTab Tab;
    [XmlElement(Order = 6)]
    public int Rarity;
    [XmlElement(Order = 7)]
    public int StackLimit;
    [XmlElement(Order = 8)]
    public bool EnableBreak;
    [XmlElement(Order = 9)]
    public bool Sellable;
    [XmlElement(Order = 10)]
    public TransferType TransferType;
    [XmlElement(Order = 11)]
    public byte TradeableCount;
    [XmlElement(Order = 12)]
    public byte RepackageCount;
    [XmlElement(Order = 13)]
    public byte RepackageItemConsumeCount;
    [XmlElement(Order = 14)]
    public bool IsTwoHand;
    [XmlElement(Order = 15)]
    public bool IsDress;
    [XmlElement(Order = 16)]
    public bool IsTemplate;
    [XmlElement(Order = 17)]
    public Gender Gender;
    [XmlElement(Order = 18)]
    public int PlayCount;
    [XmlElement(Order = 19)]
    public bool IsCustomScore;
    [XmlElement(Order = 20)]
    public List<int> SellPrice = new();
    [XmlElement(Order = 21)]
    public List<int> SellPriceCustom = new();
    [XmlElement(Order = 22)]
    public string FileName;
    [XmlElement(Order = 23)]
    public int SkillID;
    [XmlElement(Order = 24)]
    public List<int> RecommendJobs = new();
    [XmlElement(Order = 25)]
    public List<ItemBreakReward> BreakRewards;
    [XmlElement(Order = 26)]
    public ItemFunction FunctionData;
    [XmlElement(Order = 27)]
    public string Tag;
    [XmlElement(Order = 28)]
    public int ShopID;
    [XmlElement(Order = 29)]
    public int Level;
    [XmlElement(Order = 30)]
    public List<HairPresets> HairPresets = new();
    [XmlElement(Order = 31)]
    public int ColorIndex;
    [XmlElement(Order = 32)]
    public int ColorPalette;
    [XmlElement(Order = 33)]
    public int OptionStatic;
    [XmlElement(Order = 34)]
    public int OptionRandom;
    [XmlElement(Order = 35)]
    public int OptionConstant;
    [XmlElement(Order = 36)]
    public int OptionLevelFactor;
    [XmlElement(Order = 37)]
    public bool IsCubeSolid;
    [XmlElement(Order = 38)]
    public ItemHousingCategory HousingCategory;
    [XmlElement(Order = 39)]
    public int ObjectId;
    [XmlElement(Order = 40)]
    public string BlackMarketCategory;
    [XmlElement(Order = 41)]
    public string Category;

    // Required for deserialization
    public ItemMetadata()
    {
        BreakRewards = new();
        FunctionData = new();
    }

    public override string ToString()
    {
        return $"ItemMetadata(Id:{Id},Slot:{Slot},GemSlot:{Gem},Tab:{Tab},Rarity:{Rarity},StackLimit:{StackLimit},IsTwoHand:{IsTwoHand},IsTemplate:{IsTemplate},Gender{Gender},PlayCount:{PlayCount}," +
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

    public ItemFunction() { }

    public ItemFunction(string name, int id)
    {
        Name = name;
        Id = id;
    }

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
