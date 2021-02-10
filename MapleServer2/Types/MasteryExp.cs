using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class MasteryExp
    {
        public byte Type { get; private set; }
        public long CurrentExp { get; set; }

        public MasteryExp(MasteryType type, long currentExp)
        {
            Type = (byte) type;
            CurrentExp = currentExp;
        }
    }
}
