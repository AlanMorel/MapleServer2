using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class ItemMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public string Name;
    [XmlElement(Order = 3)]
    public InventoryTab Tab;
    [XmlElement(Order = 4)]
    public List<ItemSlot> Slots;
    [XmlElement(Order = 5)]
    public MedalSlot Medal;
    [XmlElement(Order = 8)]
    public int Rarity = 1;
    [XmlElement(Order = 9)]
    public ItemGemMetadata Gem;
    [XmlElement(Order = 10)]
    public ItemUgcMetadata UGC;
    [XmlElement(Order = 11)]
    public ItemLifeMetadata Life;
    [XmlElement(Order = 12)]
    public ItemPetMetadata Pet;
    [XmlElement(Order = 13)]
    public ItemBasicMetadata Basic;
    [XmlElement(Order = 14)]
    public ItemLimitMetadata Limit;
    [XmlElement(Order = 15)]
    public ItemSkillMetadata Skill;
    [XmlElement(Order = 16)]
    public ItemFusionMetadata Fusion;
    [XmlElement(Order = 17)]
    public ItemInstallMetadata Install;
    [XmlElement(Order = 18)]
    public ItemPropertyMetadata Property;
    [XmlElement(Order = 19)]
    public ItemCustomizeMetadata Customize;
    [XmlElement(Order = 20)]
    public ItemFunctionMetadata Function;
    [XmlElement(Order = 21)]
    public ItemOptionMetadata Option;
    [XmlElement(Order = 22)]
    public ItemMusicMetadata Music;
    [XmlElement(Order = 23)]
    public ItemHousingMetadata Housing;
    [XmlElement(Order = 24)]
    public ItemShopMetadata Shop;
    [XmlElement(Order = 25)]
    public List<ItemBreakReward> BreakRewards = new();
    [XmlElement(Order = 26)]
    public ItemAdditionalEffectMetadata AdditionalEffect;
}

[XmlType]
public class ItemGemMetadata
{
    [XmlElement(Order = 1)]
    public GemSlot Gem;
}

[XmlType]
public class ItemUgcMetadata
{
    [XmlElement(Order = 1)]
    public string Mesh;
}

[XmlType]
public class ItemLifeMetadata
{
    [XmlElement(Order = 1)]
    public int DurationPeriod;
    [XmlElement(Order = 2)]
    public DateTime ExpirationTime;
    [XmlElement(Order = 3)]
    public ItemExpirationType ExpirationType;
    [XmlElement(Order = 4)]
    public int ExpirationTypeDuration;
}

[XmlType]
public class ItemPetMetadata
{
    [XmlElement(Order = 1)]
    public int PetId;
}

[XmlType]
public class ItemBasicMetadata
{
    [XmlElement(Order = 1)]
    public string Tag;
}

[XmlType]
public class ItemLimitMetadata
{
    [XmlElement(Order = 1)]
    public List<int> JobRequirements;
    [XmlElement(Order = 2)]
    public List<int> JobRecommendations;
    [XmlElement(Order = 3)]
    public int LevelLimitMin;
    [XmlElement(Order = 4)]
    public int LevelLimitMax;
    [XmlElement(Order = 5)]
    public Gender Gender;
    [XmlElement(Order = 6)]
    public TransferType TransferType;
    [XmlElement(Order = 7)]
    public bool Sellable;
    [XmlElement(Order = 8)]
    public bool Breakable;
    [XmlElement(Order = 9)]
    public bool MeretMarketListable;
    [XmlElement(Order = 10)]
    public bool DisableEnchant;
    [XmlElement(Order = 11)]
    public int TradeLimitByRarity;
    [XmlElement(Order = 12)]
    public bool VipOnly;
}

[XmlType]
public class ItemSkillMetadata
{
    [XmlElement(Order = 1)]
    public int SkillId;
    [XmlElement(Order = 2)]
    public int SkillLevel;
}

[XmlType]
public class ItemFusionMetadata
{
    [XmlElement(Order = 1)]
    public bool Fusionable;
}

[XmlType]
public class ItemInstallMetadata
{
    [XmlElement(Order = 1)]
    public bool IsCubeSolid;
    [XmlElement(Order = 2)]
    public int ObjectId;
}

[XmlType]
public class ItemPropertyMetadata
{
    [XmlElement(Order = 1)]
    public int StackLimit;
    [XmlElement(Order = 2)]
    public ItemSkinType SkinType;
    [XmlElement(Order = 3)]
    public string Category;
    [XmlElement(Order = 4)]
    public string BlackMarketCategory;
    [XmlElement(Order = 5)]
    public bool DisableAttributeChange;
    [XmlElement(Order = 6)]
    public int GearScoreFactor;
    [XmlElement(Order = 7)]
    public int TradeableCount;
    [XmlElement(Order = 8)]
    public byte RepackageCount;
    [XmlElement(Order = 9)]
    public byte RepackageItemConsumeCount;
    [XmlElement(Order = 10)]
    public bool DisableTradeWithinAccount;
    [XmlElement(Order = 11)]
    public bool DisableDrop;
    [XmlElement(Order = 12)]
    public int SocketDataId;
    [XmlElement(Order = 13)]
    public ItemSellMetadata Sell;
}

[XmlType]
public class ItemSellMetadata
{
    [XmlElement(Order = 1)]
    public List<long> SellPrice = new();
    [XmlElement(Order = 2)]
    public List<long> SellPriceCustom = new();
}

[XmlType]
public class ItemCustomizeMetadata
{
    [XmlElement(Order = 1)]
    public int ColorPalette;
    [XmlElement(Order = 2)]
    public int ColorIndex;
    [XmlElement(Order = 3)]
    public List<HairPresets> HairPresets = new();
}

[XmlType]
public class ItemFunctionMetadata
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
}

[XmlType]
public class ItemOptionMetadata
{
    [XmlElement(Order = 1)]
    public int Static;
    [XmlElement(Order = 2)]
    public int Random;
    [XmlElement(Order = 3)]
    public int Constant;
    [XmlElement(Order = 4)]
    public float OptionLevelFactor;
    [XmlElement(Order = 5)]
    public int OptionId;
}

[XmlType]
public class ItemMusicMetadata
{
    [XmlElement(Order = 1)]
    public int PlayCount;
    [XmlElement(Order = 2)]
    public int MasteryValue;
    [XmlElement(Order = 3)]
    public int MasteryValueMax;
    [XmlElement(Order = 4)]
    public bool IsCustomScore;
    [XmlElement(Order = 5)]
    public string FileName;
    [XmlElement(Order = 6)]
    public int PlayTime;
}

[XmlType]
public class ItemHousingMetadata
{
    [XmlElement(Order = 1)]
    public ItemHousingCategory HousingCategory;
    [XmlElement(Order = 2)]
    public int TrophyId;
    [XmlElement(Order = 3)]
    public int TrophyLevel;
}

[XmlType]
public class ItemShopMetadata
{
    [XmlElement(Order = 1)]
    public int ShopId;
}

[XmlType]
public class ItemAdditionalEffectMetadata
{
    [XmlElement(Order = 1)]
    public int[] Id;
    [XmlElement(Order = 2)]
    public int[] Level;
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
