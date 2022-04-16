namespace MapleServer2.Types;

public class PrestigeMission
{
    public readonly int Id;
    public int LevelCount;
    public bool Claimed;

    public PrestigeMission(int id)
    {
        Id = id;
    }
}
