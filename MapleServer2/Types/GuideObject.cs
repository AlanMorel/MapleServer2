using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuideObject
    {
        public readonly int Id;
        public long BoundCharacterId;
        public byte Type;

        public GuideObject(byte type, long characterId)
        {
            Id = GuidGenerator.Int();
            Type = type;
            BoundCharacterId = characterId;
        }
    }
}
