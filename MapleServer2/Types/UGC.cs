using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;

namespace MapleServer2.Types;

public class UGC : IPacketDeserializable, IPacketSerializable
{
    public static readonly UGC Default = new();
    public long Uid;

    public Guid Guid;
    public string Name = "";
    public string Url = "";
    public UGCType Type;

    public long CharacterId;
    public string CharacterName = "";

    public long AccountId;

    public long CreationTime;
    public long SalePrice;

    public int GuildPosterId;

    public UGC() { }

    public UGC(string name, long characterId, string characterName, long accountId, long salePrice, UGCType type, int guildPosterId = 0)
    {
        Guid = Guid.NewGuid();
        Name = name;
        CharacterId = characterId;
        CharacterName = characterName;
        AccountId = accountId;
        CreationTime = TimeInfo.Now();
        SalePrice = salePrice;
        Url = string.Empty;
        Type = type;
        GuildPosterId = guildPosterId;
        Uid = DatabaseManager.UGC.Insert(this);
    }

    public void UpdateItem(string name, long characterId, string characterName, long accountId, long salePrice, UGCType type)
    {
        Guid = Guid.NewGuid();
        Name = name;
        CharacterId = characterId;
        CharacterName = characterName;
        CreationTime = TimeInfo.Now();
        AccountId = accountId;
        SalePrice = salePrice;
        Type = type;
        DatabaseManager.UGC.Update(this);
    }

    public void ReadFrom(PacketReader packet)
    {
        Uid = packet.ReadLong();
        Guid = Guid.Parse(packet.ReadUnicodeString());
        Name = packet.ReadUnicodeString();
        packet.ReadByte();
        packet.ReadInt();
        AccountId = packet.ReadLong();
        CharacterId = packet.ReadLong();
        CharacterName = packet.ReadUnicodeString();
        CreationTime = packet.ReadLong();
        Url = packet.ReadUnicodeString();
        packet.ReadByte();
    }

    public void WriteTo(PacketWriter pWriter)
    {
        pWriter.WriteLong(Uid);
        pWriter.WriteUnicodeString(Guid.ToString());
        pWriter.WriteUnicodeString(Name);
        pWriter.WriteByte(1);
        pWriter.WriteInt(1); // sometimes 2
        pWriter.WriteLong(AccountId);
        pWriter.WriteLong(CharacterId);
        pWriter.WriteUnicodeString(CharacterName);
        pWriter.WriteLong(CreationTime);
        pWriter.WriteUnicodeString(Url);
        pWriter.WriteByte();
    }
}
