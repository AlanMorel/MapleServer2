using System.Linq;
using System.Runtime.InteropServices;
using Maple2Storage.Enums;
using Maple2Storage.Utils;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Types.Items {
    public class PetItem : Item {
        public string PetName;
        public long PetExp;
        public short PetLevel;

        public PetPotionSetting[] PotionSettings;
        public PetLootSettings LootSettings;

        public InventoryState PetInventory; // Instance is shared if cloning

        public PetItem(int mapleId, InventoryType inventoryType, EquipSlot[] equipSlots, int slotMax)
                : base(mapleId, inventoryType, equipSlots, slotMax) {
            PetName = "";
            LootSettings = new PetLootSettings(true, true, true, true, true, true, true, false, 1, true);
            PetInventory = new InventoryState(InventoryType.PetStorage); // TODO: parse xml for actual sizes
        }

        public override byte[] SerializeExtraBytes() {
            var writer = new ByteWriter();
            writer.WriteUnicodeString(PetName);
            writer.WriteLong(PetExp);
            writer.WriteInt(); // Unknown
            writer.WriteShort(PetLevel);
            writer.WriteShort(); // 1
            writer.WriteShort();
            writer.WriteCollection<PetPotionSetting>(PotionSettings);
            writer.Write<PetLootSettings>(LootSettings);
            return writer.ToArray();
        }

        public override void DeserializeExtraBytes(byte[] bytes) {
            var packet = new ByteReader(bytes);
            PetName = packet.ReadUnicodeString();
            packet.ReadInt();
            PetExp = packet.ReadLong();
            PetLevel = packet.ReadShort();
            packet.ReadShort();
            packet.ReadShort();
            PotionSettings = packet.ReadCollection<PetPotionSetting>().ToArray();
            LootSettings = packet.Read<PetLootSettings>();
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 4, Size = 12)]
    public readonly struct PetPotionSetting {
        public readonly int Index;
        public readonly float Threshold;
        public readonly int ItemId;

        public PetPotionSetting(int index, float threshold, int itemId) {
            Index = index;
            Threshold = threshold;
            ItemId = itemId;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 13)]
    public readonly struct PetLootSettings {
        private readonly bool Mesos;
        private readonly bool Merets;
        private readonly bool Other;
        private readonly bool Currency;
        private readonly bool Equipment;
        private readonly bool Consumable;
        private readonly bool Gemstone;
        private readonly bool Dropped;
        private readonly int Rarity;
        private readonly bool Enabled;

        public PetLootSettings(bool mesos, bool merets, bool other, bool currency, bool equipment, bool consumable,
                bool gemstone, bool dropped, int rarity, bool enabled) {
            Mesos = mesos;
            Merets = merets;
            Other = other;
            Currency = currency;
            Equipment = equipment;
            Consumable = consumable;
            Gemstone = gemstone;
            Dropped = dropped;
            Rarity = rarity;
            Enabled = enabled;
        }
    }
}