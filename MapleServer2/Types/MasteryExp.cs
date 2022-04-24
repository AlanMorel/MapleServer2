using MapleServer2.Enums;

namespace MapleServer2.Types;

public class MasteryExp
{
    public MasteryType Type { get; }
    public int Level;
    public long CurrentExp { get; set; }

    public MasteryExp(MasteryType type, int level = 0, long currentExp = 0)
    {
        Type = type;
        Level = level;
        CurrentExp = currentExp;
    }
}
