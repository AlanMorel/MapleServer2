using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GuideObject
    {
        public readonly int Id;
        public byte Type;

        public GuideObject()
        {
            Id = GuidGenerator.Int();
        }
    }
}
