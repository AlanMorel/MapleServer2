using Maple2.Data.Factory;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;
using Maple2Storage.Types;
using Maple2Storage.Utils;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Converter {
    public class ItemConverter : IModelConverter<Item, Maple2.Sql.Model.Item> {
        private readonly ItemFactory factory;

        public ItemConverter(ItemFactory factory) {
            this.factory = factory;
        }

        public Maple2.Sql.Model.Item ToModel(Item value, Maple2.Sql.Model.Item item) {
            if (value == null) return null;

            item ??= new Maple2.Sql.Model.Item();
            item.Id = value.Id;
            item.ItemId = value.MapleId;
            item.Slot = value.Slot;
            item.Amount = value.Amount;
            item.Rarity = value.Rarity;
            item.ExpiryTime = value.ExpiryTime.FromEpochSeconds();
            item.AttributesChangeCount = value.AttributesChangeCount;
            item.RemainingUses = value.RemainingUses;
            item.IsLocked = value.IsLocked;
            item.UnlockTime = value.UnlockTime.FromEpochSeconds();
            item.GlamourForgeCount = value.GlamourForgeCount;
            item.Enchants = value.Enchants;
            item.EnchantExp = value.EnchantExp;
            item.CanRepack = value.CanRepack;
            item.Charges = value.Charges;
            item.TradeCount = value.TradeCount;
            item.MaxSockets = value.Sockets.MaxSockets;
            item.UnlockedSockets = value.Sockets.Unlocked;
            item.Stats = value.Stats.Serialize();
            item.Appearance = value.Appearance.Serialize();
            item.Transfer = value.Transfer.Serialize();
            //item.CoupleInfo = value.CoupleInfo.Serialize();
            item.ExtraBlob = value.SerializeExtraBytes();

            return item;
        }

        public Item FromModel(Maple2.Sql.Model.Item value) {
            if (value == null) return null;

            Item item = factory.Init(value.ItemId);
            item.Uid = value.Id; // Need to be change for the Long ID
            item.Slot = value.Slot;
            item.Amount = value.Amount;
            item.Rarity = value.Rarity;
            item.CreationTime = value.CreationTime.ToEpochSeconds();
            item.ExpiryTime = value.ExpiryTime.ToEpochSeconds();
            item.AttributesChangeCount = value.AttributesChangeCount;
            item.RemainingUses = value.RemainingUses;
            item.IsLocked = value.IsLocked;
            item.UnlockTime = value.UnlockTime.ToEpochSeconds();
            item.GlamourForgeCount = value.GlamourForgeCount;
            item.Enchants = value.Enchants;
            item.EnchantExp = value.EnchantExp;
            item.CanRepack = value.CanRepack;
            item.Charges = value.Charges;
            item.TradeCount = value.TradeCount;
            item.Sockets = new ItemSockets(value.MaxSockets, value.UnlockedSockets);
            item.Stats = value.Stats.Deserialize<ItemStats>();
            //item.Appearance.ReadFrom(new ByteReader(value.Appearance));
            //item.Transfer = value.Transfer.Deserialize<ItemTransfer>();
            //item.CoupleInfo = value.CoupleInfo.Deserialize<ItemCoupleInfo>();
            item.DeserializeExtraBytes(value.ExtraBlob);

            return item;
        }
    }
}