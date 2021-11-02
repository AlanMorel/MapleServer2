using Maple2Storage.Enums;
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
        public GemSlot GemSlot { get; set; }
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
        public ItemFunction Function { get; set; }
        public string Tag { get; set; }
        public int ShopID { get; set; }
        public ItemHousingCategory HousingCategory;
        public string BlackMarketCategory;

        public int Id;
        public long Uid;
        public string Name;
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
        public int RepackageCount;
        public int Charges;
        public TransferFlag TransferFlag;
        public TransferType TransferType;
        public int RemainingTrades;

        // For friendship badges
        public long PairedCharacterId;
        public string PairedCharacterName;
        public int PetSkinBadgeId;
        public byte[] TransparencyBadgeBools;

        public long OwnerCharacterId;
        public string OwnerCharacterName;
        public EquipColor Color;
        public HairData HairData;
        public HatData HatData;
        public byte[] FaceDecorationData;
        public MusicScore Score;
        public ItemStats Stats;

        public long InventoryId;
        public long BankInventoryId;
        public long HomeId;
        public long MailId;

        public Item() { }

        public Item(int id, bool saveToDatabase = true)
        {
            Id = id;
            SetMetadataValues();
            Name = ItemMetadataStorage.GetName(id);
            IsTemplate = ItemMetadataStorage.GetIsTemplate(id);
            Level = ItemMetadataStorage.GetLevel(id);
            ItemSlot = ItemMetadataStorage.GetSlot(id);
            if (GemSlot == GemSlot.TRANS)
            {
                TransparencyBadgeBools = new byte[10];
            }
            Rarity = ItemMetadataStorage.GetRarity(id);
            PlayCount = ItemMetadataStorage.GetPlayCount(id);
            Color = ItemMetadataStorage.GetEquipColor(id);
            CreationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            RemainingGlamorForges = ItemExtractionMetadataStorage.GetExtractionCount(id);
            Slot = -1;
            Amount = 1;
            Score = new MusicScore();
            Stats = new ItemStats(id, Rarity, ItemSlot, Level);
            if (saveToDatabase)
            {
                Uid = DatabaseManager.Items.Insert(this);
            }
        }

        public Item(int id, int amount, bool saveToDatabase = true) : this(id, saveToDatabase)
        {
            Amount = amount;
        }

        // Make a copy of item
        public Item(Item other)
        {
            Id = other.Id;
            Name = other.Name;
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
            Function = other.Function;
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
            RepackageCount = other.RepackageCount;
            Charges = other.Charges;
            TransferFlag = other.TransferFlag;
            RemainingTrades = other.RemainingTrades;
            PairedCharacterId = other.PairedCharacterId;
            PairedCharacterName = other.PairedCharacterName;
            PetSkinBadgeId = other.PetSkinBadgeId;
            RecommendJobs = other.RecommendJobs;
            OwnerCharacterId = other.OwnerCharacterId;
            OwnerCharacterName = other.OwnerCharacterName;
            InventoryId = other.InventoryId;
            BankInventoryId = other.BankInventoryId;
            BlackMarketCategory = other.BlackMarketCategory;
            HomeId = other.HomeId;
            Color = other.Color;
            HairData = other.HairData;
            HatData = other.HatData;
            Score = new MusicScore();
            Stats = new ItemStats(other.Stats);
        }

        public bool TrySplit(int splitAmount, out Item splitItem)
        {
            splitItem = null;
            if (Amount <= splitAmount)
            {
                return false;
            }

            Amount -= splitAmount;

            splitItem = new Item(this)
            {
                Amount = splitAmount,
                Slot = -1,
                InventoryId = 0
            };
            splitItem.Uid = DatabaseManager.Items.Insert(splitItem);

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

        public void SetMetadataValues()
        {
            InventoryTab = ItemMetadataStorage.GetTab(Id);
            GemSlot = ItemMetadataStorage.GetGem(Id);
            StackLimit = ItemMetadataStorage.GetStackLimit(Id);
            EnableBreak = ItemMetadataStorage.GetEnableBreak(Id);
            IsTwoHand = ItemMetadataStorage.GetIsTwoHand(Id);
            IsDress = ItemMetadataStorage.GetIsDress(Id);
            IsCustomScore = ItemMetadataStorage.GetIsCustomScore(Id);
            Gender = ItemMetadataStorage.GetGender(Id);
            FileName = ItemMetadataStorage.GetFileName(Id);
            SkillId = ItemMetadataStorage.GetSkillID(Id);
            RecommendJobs = ItemMetadataStorage.GetRecommendJobs(Id);
            Function = ItemMetadataStorage.GetFunction(Id);
            Tag = ItemMetadataStorage.GetTag(Id);
            ShopID = ItemMetadataStorage.GetShopID(Id);
            RemainingTrades = ItemMetadataStorage.GetTradeableCount(Id);
            TransferType = ItemMetadataStorage.GetTransferType(Id);
            RepackageCount = ItemMetadataStorage.GetRepackageCount(Id);
            HousingCategory = ItemMetadataStorage.GetHousingCategory(Id);
            BlackMarketCategory = ItemMetadataStorage.GetBlackMarketCategory(Id);
        }
    }
}
