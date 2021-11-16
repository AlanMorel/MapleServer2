using MapleServer2.Database;

namespace MapleServer2.Types;

public class UGC
{
    public long Uid;

    public Guid Guid;
    public string Name;
    public string Url;

    public long CharacterId;
    public string CharacterName;

    public long AccountId;

    public long CreationTime;
    public long SalePrice;

    public UGC() { }

    public UGC(string name, long characterId, string characterName, long accountId, long salePrice)
    {
        Guid = Guid.NewGuid();
        Name = name;
        CharacterId = characterId;
        CharacterName = characterName;
        AccountId = accountId;
        CreationTime = TimeInfo.Now();
        SalePrice = salePrice;
        Url = string.Empty;
        Uid = DatabaseManager.UGC.Insert(this);
    }
}
