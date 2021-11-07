namespace MapleServer2.Types;

public static class TimeInfo
{ 
    public static long Now()
    {
        return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
}
