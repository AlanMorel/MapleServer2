using System.Text.RegularExpressions;

namespace MapleServer2.Tools;

public static class ErrorParser
{
    private static readonly Regex InfoRegex = new(@"\[type=(\d+)\]\[offset=(\d+)\]\[hint=(\w+)\]");

    public static SockExceptionInfo Parse(string error)
    {
        Match match = InfoRegex.Match(error);
        if (match.Groups.Count != 4)
        {
            throw new ArgumentException($"Failed to parse error: {error}");
        }

        SockExceptionInfo info;
        info.Type = ushort.Parse(match.Groups[1].Value);
        info.Offset = uint.Parse(match.Groups[2].Value);
        info.Hint = match.Groups[3].Value.ToSockHint();

        return info;
    }
}

public struct SockExceptionInfo
{
    public ushort Type;
    public uint Offset;
    public SockHint Hint;
}
