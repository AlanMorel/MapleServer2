namespace MapleServer2.Types;

public class ChatSticker
{
    public readonly byte GroupId;
    public readonly long Expiration;
    
    public ChatSticker(byte groupId, long expiration)
    {
        GroupId = groupId;
        Expiration = expiration;
    }
}
