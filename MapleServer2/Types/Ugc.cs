using MapleServer2.Database;

namespace MapleServer2.Types;

public class Ugc
{
    public long Uid;

    public Guid Guid;
    public string Name;
    public string Url;
    public UgcType Type;

    public long CharacterId;
    public string CharacterName;

    public long AccountId;

    public long CreationTime;
    public long SalePrice;

    public Ugc() { }

    public Ugc(string name, long characterId, string characterName, long accountId, long salePrice, UgcType type)
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
        Uid = DatabaseManager.Ugc.Insert(this);
    }
}

public enum UgcType : byte
{
    Item = 0x01,
    Furniture = 0x02,
    GuildEmblem = 0x06
}
