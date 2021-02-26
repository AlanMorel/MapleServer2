namespace MapleServer2.Types
{
    public class ChatSticker
    {
        public byte GroupId { get; set; }
        public long Expiration { get; set; }

        public ChatSticker() { }

        public ChatSticker(byte groupId, long expiration)
        {
            GroupId = groupId;
            Expiration = expiration;
        }
    }
}
