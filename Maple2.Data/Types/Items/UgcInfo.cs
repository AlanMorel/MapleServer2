using Maple2Storage.Utils;
using MaplePacketLib2.Tools;

namespace Maple2.Data.Types.Items {
    public class UgcInfo : IByteSerializable {
        public string FileName;
        public string Name;
        public long AccountId;
        public long CharacterId;
        public string Author;
        public long CreationTime;
        public string Url;

        public UgcInfo() {
            this.FileName = "";
            this.Name = "";
            this.Author = "";
            this.Url = "";
        }

        public UgcInfo(UgcInfo other) {
            this.FileName = other.FileName;
            this.Name = other.Name;
            this.AccountId = other.AccountId;
            this.CharacterId = other.CharacterId;
            this.Author = other.Author;
            this.CreationTime = other.CreationTime;
            this.Url = other.Url;
        }

        public void WriteTo(IByteWriter writer) {
            writer.WriteLong();
            writer.WriteUnicodeString(FileName); // UUID (filename)
            writer.WriteUnicodeString(Name); // Name (itemname)
            writer.WriteByte();
            writer.WriteInt();
            writer.WriteLong(AccountId); // AccountId
            writer.WriteLong(CharacterId); // CharacterId
            writer.WriteUnicodeString(Author); // CharacterName
            writer.WriteLong(CreationTime); // CreationTime
            writer.WriteUnicodeString(Url); // URL (no domain)
            writer.WriteByte();
        }

        public void ReadFrom(IByteReader reader) {
            reader.ReadLong();
            FileName = reader.ReadUnicodeString();
            Name = reader.ReadUnicodeString();
            reader.ReadByte();
            reader.ReadInt();
            AccountId = reader.ReadLong();
            CharacterId = reader.ReadLong();
            Author = reader.ReadUnicodeString();
            CreationTime = reader.ReadLong();
            Url = reader.ReadUnicodeString();
            reader.ReadByte();
        }
    }
}