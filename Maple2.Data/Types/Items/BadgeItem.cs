using Maple2Storage.Enums;

namespace Maple2.Data.Types.Items {
    public class BadgeItem : Item {
        // These types can be parsed from xml
        public BadgeType BadgeType => (MapleId / 100000) switch {
            701 => (MapleId % 10) switch {
                0 => BadgeType.PetSkin,
                1 => BadgeType.Transparency,
                _ => BadgeType.AutoGather
            },
            702 => BadgeType.ChatBubble,
            703 => BadgeType.NameTag,
            704 => BadgeType.Damage,
            705 => BadgeType.Tombstone,
            706 => BadgeType.SwimTube,
            707 => BadgeType.Fishing,
            708 => BadgeType.Buddy,
            709 => BadgeType.Effect,
            _ => BadgeType.None
        };

        public BadgeItem(int mapleId, InventoryType inventoryType, EquipSlot[] equipSlots, int slotMax)
            : base(mapleId, inventoryType, equipSlots, slotMax) { }
    }
}