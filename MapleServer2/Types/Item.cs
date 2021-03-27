using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class Item
    {
        public int Level { get; set; }
        public InventoryTab InventoryTab { get; private set; }
        public ItemSlot ItemSlot { get; private set; }
        public GemSlot GemSlot { get; private set; }
        public int Rarity { get; set; }
        public int StackLimit { get; private set; }
        public bool EnableBreak { get; private set; }
        public bool IsTwoHand { get; private set; }
        public bool IsDress { get; private set; }
        public bool IsTemplate { get; set; }
        public bool IsCustomScore { get; set; }
        public int PlayCount { get; set; }
        public byte Gender { get; }
        public string FileName { get; set; }
        public int SkillId { get; set; }
        public List<Job> RecommendJobs { get; set; }
        public List<ItemContent> Content { get; private set; }
        public ItemFunction Function { get; set; }
        public string Tag { get; set; }
        public int ShopID { get; set; }

        public readonly int Id;
        public long Uid;
        public short Slot;
        public int Amount;

        public long CreationTime;
        public long ExpiryTime;

        public int TimesAttributesChanged;
        public bool IsLocked;
        public long UnlockTime;
        public short RemainingGlamorForges;
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

        public Player Owner;

        public EquipColor Color;

        public HairData HairD;

        public byte[] FaceDecorationD;
        public byte AppearanceFlag;

        public MusicScore Score;

        public ItemStats Stats;

        public Item(int id)
        {
            Id = id;
            Level = ItemMetadataStorage.GetLevel(id);
            Uid = GuidGenerator.Long();
            InventoryTab = ItemMetadataStorage.GetTab(id);
            ItemSlot = ItemMetadataStorage.GetSlot(id);
            GemSlot = ItemMetadataStorage.GetGem(id);
            Rarity = ItemMetadataStorage.GetRarity(id);
            StackLimit = ItemMetadataStorage.GetStackLimit(id);
            EnableBreak = ItemMetadataStorage.GetEnableBreak(id);
            IsTwoHand = ItemMetadataStorage.GetIsTwoHand(id);
            IsDress = ItemMetadataStorage.GetIsDress(id);
            IsTemplate = ItemMetadataStorage.GetIsTemplate(id);
            IsCustomScore = ItemMetadataStorage.GetIsCustomScore(id);
            Gender = ItemMetadataStorage.GetGender(id);
            PlayCount = ItemMetadataStorage.GetPlayCount(id);
            FileName = ItemMetadataStorage.GetFileName(id);
            SkillId = ItemMetadataStorage.GetSkillID(id);
            RecommendJobs = ItemMetadataStorage.GetRecommendJobs(id);
            Content = ItemMetadataStorage.GetContent(id);
            Function = ItemMetadataStorage.GetFunction(id);
            Tag = ItemMetadataStorage.GetTag(id);
            ShopID = ItemMetadataStorage.GetShopID(id);
            Slot = -1;
            Amount = 1;
            Score = new MusicScore();
            Stats = new ItemStats(id, Rarity, Level);
            CanRepackage = true; // If false, item becomes untradable
        }

        // Make a copy of item
        public Item(Item other)
        {
            Id = other.Id;
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
            Uid = other.Uid;
            Slot = other.Slot;
            Amount = other.Amount;
            CreationTime = other.CreationTime;
            ExpiryTime = other.ExpiryTime;
            TimesAttributesChanged = other.TimesAttributesChanged;
            IsLocked = other.IsLocked;
            UnlockTime = other.UnlockTime;
            RemainingGlamorForges = other.RemainingGlamorForges;
            Enchants = other.Enchants;
            EnchantExp = other.EnchantExp;
            CanRepackage = other.CanRepackage;
            Charges = other.Charges;
            TransferFlag = other.TransferFlag;
            RemainingTrades = other.RemainingTrades;
            PairedCharacterId = other.PairedCharacterId;
            PairedCharacterName = other.PairedCharacterName;
            Owner = other.Owner;
            Color = other.Color;
            HairD = other.HairD;
            AppearanceFlag = other.AppearanceFlag;
            Score = new MusicScore();
            Stats = new ItemStats(other.Stats);
        }

        public static Item Ear()
        {
            return new Item(10500001)
            {
                Uid = 2754959794416496488,
                CreationTime = 1558494660,
                Color = new EquipColor(),
            };
        }

        public static Item Hair()
        {
            return new Item(10200148)
            {
                Uid = 2867972925711604442,
                CreationTime = 1565575851,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0x7E, 0xCC, 0xF7),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x85, 0xDB),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x48, 0x5E, 0xA8),
                    15
                ),
                HairD = new HairData(0.3f, 0.3f, new CoordF(), new CoordF(), new CoordF(), new CoordF()),
                AppearanceFlag = 2,
            };
        }

        public static Item Face()
        {
            return new Item(10300004)
            {
                Uid = 2754959794416496483,
                CreationTime = 1558494660,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0xB5, 0x24, 0x29),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xF7, 0xE3, 0xE3),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x14, 0x07, 0x02),
                    0
                ),
                AppearanceFlag = 3,
            };
        }

        public static Item FaceDecoration()
        {
            return new Item(10400000)
            {
                Uid = 2754959794416496484,
                CreationTime = 1558494660,
                Color = new EquipColor(),
                FaceDecorationD = new byte[16],
            };
        }

        public static Item TutorialBow(Player owner)
        {
            // bow 15100216
            // [longsword]  Tairen Royal Longsword - 13200309
            // [shield] Tairen Royal Shield - 14100279
            // [greatsword] Tairen Royal Greatsword - 15000313
            // [scepter] Tairen Royal Scepter - 13300308
            // [codex] Tairen Royal Codex - 14000270
            // [staff] Tairen Royal Staff - 15200312
            // [cannon] Tairen Royal Cannon - 15300308
            // [bow] Tairen Royal Bow - 15100305
            // [dagger] Tairen Royal Knife - 13100314
            // [star] Tairen Royal Star - 13400307
            // [blade] Tairen Royal Blade - 15400294
            // [knuckles] Tairen Royal Knuckles - 15500226
            // [orb] Tairen Royal Spirit - 15600228
            return new Item(15100216)
            {
                Uid = 3430503306390578751, // Make sure its unique! If the UID is equipped, it will say "Equipped" on the item in your inventory
                Rarity = 1,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Owner = owner,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0xBC, 0xBC, 0xB3),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xC3, 0xDA, 0x3D),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xB0, 0xB4, 0xBA),
                    0x13
                ),
                AppearanceFlag = 0x5,
                TransferFlag = TransferFlag.Binds | TransferFlag.Splitable,
            };
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
            splitItem.Uid = Environment.TickCount64;
            return true;
        }

        public static Item DefaultScepter(Player owner)
        {
            return new Item(13300308)
            {
                Uid = 3430503306390578751, // Make sure its unique! If the UID is equipped, it will say "Equipped" on the item in your inventory
                Rarity = 1,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Owner = owner,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0xBC, 0xBC, 0xB3),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xC3, 0xDA, 0x3D),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xB0, 0xB4, 0xBA),
                    0x13
                ),
                AppearanceFlag = 0x5,
                TransferFlag = TransferFlag.Binds | TransferFlag.Splitable,
            };
        }

        public static Item DefaultCodex(Player owner)
        {
            return new Item(14000270)
            {
                Uid = 3430503306390578751, // Make sure its unique! If the UID is equipped, it will say "Equipped" on the item in your inventory
                Rarity = 1,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Owner = owner,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0xBC, 0xBC, 0xB3),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xC3, 0xDA, 0x3D),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xB0, 0xB4, 0xBA),
                    0x13
                ),
                AppearanceFlag = 0x5,
                TransferFlag = TransferFlag.Binds | TransferFlag.Splitable,
            };
        }
        // MALE ITEMS
        public static Item HairMale()
        {
            return new Item(10200003)
            {
                Uid = 2867972925711604442,
                CreationTime = 1565575851,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x69, 0xB5),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x85, 0xDB),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x48, 0x5E, 0xA8),
                    4
                ),
                HairD = new HairData(0.3f, 0.3f, new CoordF(), new CoordF(), new CoordF(), new CoordF()),
                AppearanceFlag = 2,
            };
        }
        public static Item EarMale()
        {
            return new Item(10500001)
            {
                Uid = 2754959794416496488,
                CreationTime = 1558494660,
                Color = new EquipColor(),
            };
        }
        public static Item FaceMale()
        {
            return new Item(10300051)
            {
                Uid = 2754959794416496483,
                CreationTime = 1558494660,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0x7E, 0xF3, 0xF8),
                    Maple2Storage.Types.Color.Argb(0xFF, 0xF7, 0xE3, 0xE3),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x14, 0x07, 0x02),
                    0
                ),
                AppearanceFlag = 3,
            };
        }
        public static Item FaceDecorationMale()
        {
            return new Item(10400002)
            {
                Uid = 2754959794416496484,
                CreationTime = 1558494660,
                Color = new EquipColor(),
                FaceDecorationD = new byte[16],
            };
        }
        public static Item CloathMale()
        {
            return new Item(12200398)
            {
                Uid = 2754959794416496484,
                CreationTime = 1558494660,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x69, 0xB5),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x85, 0xDB),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x48, 0x5E, 0xA8),
                    4
                ),
            };
        }

        public static Item ShoesMale()
        {
            return new Item(11700852)
            {
                Uid = 2754959794416496484,
                CreationTime = 1558494660,
                Color = EquipColor.Custom(
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x69, 0xB5),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x4C, 0x85, 0xDB),
                    Maple2Storage.Types.Color.Argb(0xFF, 0x48, 0x5E, 0xA8),
                    4
                ),
            };
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
    }
}
