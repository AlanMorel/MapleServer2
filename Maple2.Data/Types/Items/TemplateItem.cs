using Maple2Storage.Enums;
using Maple2Storage.Utils;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Types.Items {
    public class TemplateItem : Item {
        public UgcInfo Ugc { get; private set; }

        public TemplateItem(int mapleId, InventoryType inventoryType, EquipSlot[] equipSlots, int slotMax)
                : base(mapleId, inventoryType, equipSlots, slotMax) {
            Ugc = new UgcInfo();
        }

        public override Item Clone() {
            var clone = (TemplateItem) base.Clone();
            clone.Ugc = new UgcInfo(Ugc);

            return clone;
        }

        public override byte[] SerializeExtraBytes() {
            var pWriter = new PacketWriter();
            //pWriter.WriteClass<UgcInfo>(Ugc);
            return pWriter.ToArray();
        }

        public override void DeserializeExtraBytes(byte[] bytes) {
            var packet = new PacketReader(bytes);
            //Ugc = packet.ReadClass<UgcInfo>();
        }
    }
}