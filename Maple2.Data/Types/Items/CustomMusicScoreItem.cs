using Maple2Storage.Enums;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Types.Items {
    public class CustomMusicScoreItem : Item {
        public int MusicId;
        public int Instrument; // Preferred instrument
        public string Title;
        public string Author;
        public long AuthorId; // AccountId
        public string Code;

        public CustomMusicScoreItem(int mapleId, InventoryType inventoryType, EquipSlot[] equipSlots, int slotMax)
                : base(mapleId, inventoryType, equipSlots, slotMax) {
            this.Title = "";
            this.Author = "";
            this.Code = "";
            // TODO: Remaining uses needs to be set properly at blank score item creation
            this.RemainingUses = 10;
        }

        public override byte[] SerializeExtraBytes() {
            var writer = new ByteWriter();
            writer.WriteInt(MusicId);
            writer.WriteInt(Instrument);
            writer.WriteUnicodeString(Title);
            writer.WriteUnicodeString(Author);
            writer.WriteLong(AuthorId);
            writer.WriteString(Code);
            return writer.ToArray();
        }

        public override void DeserializeExtraBytes(byte[] bytes) {
            var packet = new ByteReader(bytes);
            MusicId = packet.ReadInt();
            Instrument = packet.ReadInt();
            Title = packet.ReadUnicodeString();
            Author = packet.ReadUnicodeString();
            AuthorId = packet.ReadLong();
            Code = packet.ReadString();
        }
    }
}