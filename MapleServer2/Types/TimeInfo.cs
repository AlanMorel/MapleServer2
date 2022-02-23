namespace MapleServer2.Types;

public static class TimeInfo
{
    public static long Now() => DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    public static DateTime CurrentDate() => DateTimeOffset.UtcNow.UtcDateTime;

    public static long AddDays(int days) => DateTimeOffset.UtcNow.AddDays(days).ToUnixTimeSeconds();

    public static long Tomorrow() => (long) DateTime.Now.AddDays(1).ToUniversalTime().Date.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

    public static DateTime TimestampToDateTime(long timestamp) => new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
}
