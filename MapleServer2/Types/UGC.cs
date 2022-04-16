using MapleServer2.Database;

namespace MapleServer2.Types;

public class UGC
{
    public long Uid;

    public Guid Guid;
    public string Name;
    public string Url;
    public UGCType Type;

    public long CharacterId;
    public string CharacterName;

    public long AccountId;

    public long CreationTime;
    public long SalePrice;

    public UGC() { }

    public UGC(string name, long characterId, string characterName, long accountId, long salePrice, UGCType type)
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
        Uid = DatabaseManager.UGC.Insert(this);
    }
}

// All valid UGC types
public enum UGCType : byte
{
    Item = 0x01,
    Furniture = 0x02,
    Banner = 0x03,
    Unk = 0x04,
    Unk2 = 0x05,
    GuildEmblem = 0x06,
    Unk3 = 0x07,
    Unk4 = 0x08,
    Unk5 = 0x09,
    Unk6 = 0x0A,
    Unk7 = 0xC9,
    Unk8 = 0xCA,
    Unk9 = 0xD1
}
