using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class Mastery
    {
        public byte Type { get; private set; }
        public long CurrentExp { get; set; }

        public Mastery(MasteryType type, long currentExp)
        {
            Type = (byte) type;
            CurrentExp = currentExp;
        }
    }
}
