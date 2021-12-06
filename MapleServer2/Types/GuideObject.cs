namespace MapleServer2.Types;

public class GuideObject
{
    public long BoundCharacterId;
    public byte Type;
    public bool IsBall;

    public GuideObject(byte type, long characterId)
    {
        Type = type;
        BoundCharacterId = characterId;
    }
}
