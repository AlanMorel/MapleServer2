using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Item
    {
        public int Level { get; set; }
        public InventoryTab InventoryTab { get; private set; }
        public ItemSlot ItemSlot { get; set; }
        public GemSlot GemSlot { get; private set; }
        public int Rarity { get; set; }
        public int StackLimit { get; private set; }
        public bool EnableBreak { get; private set; }
        public bool IsTwoHand { get; private set; }
        public bool IsDress { get; private set; }
        public bool IsTemplate { get; set; }
        public bool IsCustomScore { get; set; }
        public int PlayCount { get; set; }
        public byte Gender { get; private set; }
        public string FileName { get; set; }
        public int SkillId { get; set; }
        public List<Job> RecommendJobs { get; set; }
        public List<ItemContent> Content { get; private set; }
        public ItemFunction Function { get; set; }
        public AdBalloonData AdBalloon { get; set; }
        public string Tag { get; set; }
        public int ShopID { get; set; }

        public readonly int Id;
        public long Uid;
        public short Slot;
        public int Amount;
        public bool IsEquipped;

        public long CreationTime;
        public long ExpiryTime;

        public int TimesAttributesChanged;
        public bool IsLocked;
        public long UnlockTime;
        public short RemainingGlamorForges;
        public int GachaDismantleId;
        public int Enchants;
        // EnchantExp (10000 = 100%) for Peachy
        public int EnchantExp;
        public bool CanRepackage;
        public int Charges;
        public TransferFlag TransferFlag;
        public int RemainingTrades;

        // For friendship badges
        public long PairedCharacterId;
        public string PairedCharacterName;
        public int PetSkinBadgeId;
        public byte[] TransparencyBadgeBools;

        public Player Owner;
        public EquipColor Color;
        public HairData HairData;
        public HatData HatData;
        public byte[] FaceDecorationData;
        public MusicScore Score;
        public ItemStats Stats;

        public Inventory Inventory;
        public BankInventory BankInventory;

        public Item() { }

        public Item(int id)
        {
            Id = id;
            SetMetadataValues(id);
            IsTemplate = ItemMetadataStorage.GetIsTemplate(id);
            Level = ItemMetadataStorage.GetLevel(id);
            ItemSlot = ItemMetadataStorage.GetSlot(id);
            Rarity = ItemMetadataStorage.GetRarity(id);
            PlayCount = ItemMetadataStorage.GetPlayCount(id);
            Color = ItemMetadataStorage.GetEquipColor(id);
            CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            RemainingGlamorForges = ItemExtractionMetadataStorage.GetExtractionCount(id);
            Slot = -1;
            Amount = 1;
            Score = new MusicScore();
            Stats = new ItemStats(id, Rarity);
            CanRepackage = true; // If false, item becomes untradable
            Uid = DatabaseManager.AddItem(this);
        }

        public Item(int id, int amount) : this(id)
        {
            Amount = amount;
        }

        // Make a copy of item
        public Item(Item other)
        {
            Id = other.Id;
            Level = other.Level;
            Gender = other.Gender;
            InventoryTab = other.InventoryTab;
            ItemSlot = other.ItemSlot;
            GemSlot = other.GemSlot;
            Rarity = other.Rarity;
            StackLimit = other.StackLimit;
            EnableBreak = other.EnableBreak;
            IsTwoHand = other.IsTwoHand;
            IsDress = other.IsDress;
            IsTemplate = other.IsTemplate;
            IsCustomScore = other.IsCustomScore;
            PlayCount = other.PlayCount;
            FileName = other.FileName;
            Content = other.Content;
            Function = other.Function;
            AdBalloon = other.AdBalloon;
            Uid = other.Uid;
            Slot = other.Slot;
            Amount = other.Amount;
            CreationTime = other.CreationTime;
            ExpiryTime = other.ExpiryTime;
            TimesAttributesChanged = other.TimesAttributesChanged;
            IsLocked = other.IsLocked;
            UnlockTime = other.UnlockTime;
            RemainingGlamorForges = other.RemainingGlamorForges;
            GachaDismantleId = other.GachaDismantleId;
            Enchants = other.Enchants;
            EnchantExp = other.EnchantExp;
            CanRepackage = other.CanRepackage;
            Charges = other.Charges;
            TransferFlag = other.TransferFlag;
            RemainingTrades = other.RemainingTrades;
            PairedCharacterId = other.PairedCharacterId;
            PairedCharacterName = other.PairedCharacterName;
            PetSkinBadgeId = other.PetSkinBadgeId;
            RecommendJobs = other.RecommendJobs;
            Owner = other.Owner;
            Color = other.Color;
            HairData = other.HairData;
            HatData = other.HatData;
            Score = new MusicScore();
            Stats = new ItemStats(other.Stats);
        }

        public bool TrySplit(int amount, out Item splitItem)
        {
            if (Amount <= amount)
            {
                splitItem = null;
                return false;
            }

            splitItem = new Item(this);
            Amount -= amount;
            splitItem.Amount = amount;
            splitItem.Slot = -1;
            splitItem.Uid = DatabaseManager.AddItem(this);
            return true;
        }

        public static bool IsWeapon(ItemSlot slot)
        {
            return slot == ItemSlot.RH || slot == ItemSlot.LH || slot == ItemSlot.OH;
        }

        public static bool IsAccessory(ItemSlot slot)
        {
            return slot == ItemSlot.FH || slot == ItemSlot.EA || slot == ItemSlot.PD || slot == ItemSlot.RI || slot == ItemSlot.BE;
        }

        public static bool IsArmor(ItemSlot slot)
        {
            return slot == ItemSlot.CP || slot == ItemSlot.CL || slot == ItemSlot.PA || slot == ItemSlot.GL || slot == ItemSlot.SH || slot == ItemSlot.MT;
        }

        public static bool IsPet(int itemId)
        {
            return itemId >= 60000001 && itemId < 61000000;
        }

        public void SetMetadataValues(int id)
        {
            InventoryTab = ItemMetadataStorage.GetTab(id);
            GemSlot = ItemMetadataStorage.GetGem(id);
            StackLimit = ItemMetadataStorage.GetStackLimit(id);
            EnableBreak = ItemMetadataStorage.GetEnableBreak(id);
            IsTwoHand = ItemMetadataStorage.GetIsTwoHand(id);
            IsDress = ItemMetadataStorage.GetIsDress(id);
            IsCustomScore = ItemMetadataStorage.GetIsCustomScore(id);
            Gender = ItemMetadataStorage.GetGender(id);
            FileName = ItemMetadataStorage.GetFileName(id);
            SkillId = ItemMetadataStorage.GetSkillID(id);
            RecommendJobs = ItemMetadataStorage.GetRecommendJobs(id);
            Content = ItemMetadataStorage.GetContent(id);
            Function = ItemMetadataStorage.GetFunction(id);
            AdBalloon = ItemMetadataStorage.GetBalloonData(id);
            Tag = ItemMetadataStorage.GetTag(id);
            ShopID = ItemMetadataStorage.GetShopID(id);
        }
    }
}
