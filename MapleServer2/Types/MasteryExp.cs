using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class MasteryExp
    {
        public MasteryType Type { get; private set; }
        public long CurrentExp { get; set; }

        public MasteryExp(MasteryType type, long currentExp)
        {
            Type = type;
            CurrentExp = currentExp;
        }
    }
}
