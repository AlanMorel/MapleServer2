using System;
using Maple2Storage.Enums;
using Maple2Storage.Types;

namespace Maple2.Data.Types.Items {
    public class Item : IComparable<Item> {
        public InventoryType InventoryType { get; }
        public EquipSlot[] EquipSlots { get; }
        public int SlotMax { get; }
        private EquipSlot DefaultEquipSlot => EquipSlots.Length > 0 ? EquipSlots[0] : EquipSlot.NONE;
        public bool IsMeso => MapleId > 90000000 && MapleId < 90000004;
        public bool IsStamina => MapleId == 90000010;
        public bool IsBeauty => DefaultEquipSlot == EquipSlot.HR
                                || DefaultEquipSlot == EquipSlot.ER
                                || DefaultEquipSlot == EquipSlot.FA
                                || DefaultEquipSlot == EquipSlot.FD;
        public bool IsTemplate;
        public bool CanRepackage;
        public long Id;

        public int MapleId { get; protected set; }
        public short Slot;
        public EquipSlot EquippedSlot =>
            Enum.IsDefined(typeof(EquipSlot), (sbyte) Slot) ? (EquipSlot) Slot : EquipSlot.NONE;
        public int Amount;
        public int Rarity;

        public long CreationTime;
        public long ExpiryTime;

        public int AttributesChangeCount;
        public int RemainingUses; // Music Score
        public bool IsLocked;
        public long UnlockTime;
        public short GlamourForgeCount;
        public int Enchants;

        // EnchantExp (10000 = 100%) for Peachy
        public int EnchantExp;
        public bool CanRepack;
        public int Charges;
        public int TradeCount;
        public ItemStats Stats;

        // For friendship badges
        public long PairedCharacterId;
        public string PairedCharacterName;

        public ItemAppearance Appearance { get; private set; }
        public TransferFlag Transfer;
        public ItemSockets Sockets;
        //public ItemCoupleInfo CoupleInfo;
        public Character Owner;
        public byte AppearanceFlag;
        public EquipColor Color;

        // Unknown, this was always default
        //public readonly ItemBinding Binding = new ItemBinding();

        public Item(int mapleId, InventoryType inventoryType, EquipSlot[] equipSlots, int slotMax) {
            this.MapleId = mapleId;
            this.InventoryType = inventoryType;
            this.EquipSlots = equipSlots;
            this.SlotMax = slotMax;
            /*this.Appearance = (DefaultEquipSlot) switch {
                EquipSlot.HR => new HairAppearance(default),
                EquipSlot.FD => new DecalAppearance(default),
                EquipSlot.CP => new CapAppearance(default),
                _ => new ItemAppearance(default)
            };*/
        }


        // Getting a single Item
        public Item(int id)
        {
            this.Id = id;
            this.InventoryType = ItemMetadataStorage.GetTab(id);
            //this.EquippedSlot = ItemMetadataStorage.GetSlot(id);
            this.SlotMax = ItemMetadataStorage.GetSlotMax(id);
            this.IsTemplate = ItemMetadataStorage.GetIsTemplate(id);
            this.Slot = Convert.ToInt16(ItemMetadataStorage.GetSlot(id));
            this.Amount = 1;
            this.Stats = new ItemStats();
            this.CanRepackage = true; // If false, item becomes untradable
        }

        // Transfer and CoupleInfo are immutable, so don't need deep copy
        public virtual Item Clone() {
            var clone = (Item) this.MemberwiseClone();
            clone.Stats = new ItemStats();
            //clone.Appearance = Appearance.Clone();
            clone.Sockets = new ItemSockets(Sockets);

            return clone;
        }


        // TODO: this is a temporary hacky solution
        // Make sure item is gemstone or lapenshard
        public void Upgrade() {
            int type = MapleId / 100000;
            if (type == 402 || type == 410 || type == 420 || type == 430) {
                MapleId++;
            }
        }
        public bool TrySplit(int amount, out Item splitItem) {
            if (this.Amount <= amount) {
                splitItem = null;
                return false;
            }

            splitItem = Clone();
            this.Amount -= amount;
            splitItem.Amount = amount;
            splitItem.Slot = -1;
            splitItem.Id = 0;
            return true;
        }
        public bool CanStack(Item item) {
            return MapleId == item.MapleId
                   && Amount < SlotMax
                   && Rarity == item.Rarity
                   && Transfer.Equals(item.Transfer);
        }
        public int CompareTo(Item other) {
            if (ReferenceEquals(null, other)) return 1;

            int result = MapleId.CompareTo(other.MapleId);
            if (result != 0) return result;
            return Rarity.CompareTo(other.Rarity);
        }

        // Used for serializing type specific data
        public virtual byte[] SerializeExtraBytes() { return new byte[0]; }
        public virtual void DeserializeExtraBytes(byte[] bytes) { }
    }
}
