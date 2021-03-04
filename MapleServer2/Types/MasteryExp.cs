using MapleServer2.Enums;

namespace MapleServer2.Types
{
    public class MasteryExp
    {
        public byte Type { get; private set; }
        public long CurrentExp { get; set; }
        public int Stamina { get; set; }

        public MasteryExp(MasteryType type, long currentExp, int stamina)
        {
            Type = (byte) type;
            CurrentExp = currentExp;
            Stamina = stamina;
        }
    }
}
