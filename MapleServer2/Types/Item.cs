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
        public InventoryTab InventoryTab { get; private set; }
        public ItemSlot ItemSlot { get; private set; }
        public GemSlot GemSlot { get; private set; }
        public int Rarity { get; set; }
        public int SlotMax { get; private set; }
        public bool IsTemplate { get; set; }
        public int PlayCount { get; set; }
        public List<Job> RecommendJobs { get; set; }
        public List<ItemContent> Content { get; private set; }

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

        public ItemStats Stats;

        public Item(int id)
        {
            this.Id = id;
            this.Uid = GuidGenerator.Long();
            this.InventoryTab = ItemMetadataStorage.GetTab(id);
            this.ItemSlot = ItemMetadataStorage.GetSlot(id);
            this.GemSlot = ItemMetadataStorage.GetGem(id);
            this.Rarity = ItemMetadataStorage.GetRarity(id);
            this.SlotMax = ItemMetadataStorage.GetSlotMax(id);
            this.IsTemplate = ItemMetadataStorage.GetIsTemplate(id);
            this.PlayCount = ItemMetadataStorage.GetPlayCount(id);
            this.RecommendJobs = ItemMetadataStorage.GetRecommendJobs(id);
            this.Content = ItemMetadataStorage.GetContent(id);
            this.Slot = -1;
            this.Amount = 1;
            this.Stats = new ItemStats();
            this.CanRepackage = true; // If false, item becomes untradable
        }

        // Make a copy of item
        public Item(Item other)
        {
            Id = other.Id;
            InventoryTab = other.InventoryTab;
            ItemSlot = other.ItemSlot;
            GemSlot = other.GemSlot;
            Rarity = other.Rarity;
            SlotMax = other.SlotMax;
            PlayCount = other.PlayCount;
            Content = other.Content;
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
            Stats = new ItemStats(other.Stats);
        }

        public static Item Ear()
        {
            return new Item(10500001)
            {
                Uid = 2754959794416496488,
                CreationTime = 1558494660,
                Color = new EquipColor(),
                Stats = new ItemStats(),
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
                HairD = HairData.hairData(0.3f, 0.3f, new byte[24], new byte[24]),
                AppearanceFlag = 2,
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
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
                Stats = new ItemStats
                {
                    BasicAttributes = {
                        ItemStat.Of(Enums.ItemAttribute.CriticalRate, 12),
                        ItemStat.Of(Enums.ItemAttribute.MinWeaponAtk, 15),
                        ItemStat.Of(Enums.ItemAttribute.MaxWeaponAtk, 17)
                    }
                },
                TransferFlag = TransferFlag.Binds | TransferFlag.Splitable,
            };
        }

        public bool TrySplit(int amount, out Item splitItem)
        {
            if (this.Amount <= amount)
            {
                splitItem = null;
                return false;
            }

            splitItem = new Item(this);
            this.Amount -= amount;
            splitItem.Amount = amount;
            splitItem.Slot = -1;
            splitItem.Uid = Environment.TickCount64;
            return true;
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
                HairD = HairData.hairData(0.3f, 0.3f, new byte[24], new byte[24]),
                AppearanceFlag = 2,
                Stats = new ItemStats(),
            };
        }
        public static Item EarMale()
        {
            return new Item(10500001)
            {
                Uid = 2754959794416496488,
                CreationTime = 1558494660,
                Color = new EquipColor(),
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
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
                Stats = new ItemStats(),
            };
        }
    }
}
