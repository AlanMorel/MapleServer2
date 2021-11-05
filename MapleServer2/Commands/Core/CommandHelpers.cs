using System.Text;
using Maple2Storage.Types;
using Color = System.Drawing.Color;

namespace MapleServer2.Commands.Core;

public static class CommandHelpers
{
    internal static void TryParseCoord(int index, string[] args, out CoordF result)
    {
        float[] coord = new float[3];
        coord[0] = float.TryParse(args[index + 1], out float x) ? x : 0;
        coord[1] = float.TryParse(args[index + 2], out float y) ? y : 0;
        coord[2] = float.TryParse(args[index + 3], out float z) ? z : 0;
        result = CoordF.From(coord[0], coord[1], coord[2]);
    }

    internal static string BuildString(string[] args, string name)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append($"[{name}] ");

        foreach (string arg in args[1..])
        {
            stringBuilder.Append($"{arg} ");
        }
        return stringBuilder.ToString();
    }

    internal static string Bold(this string message)
    {
        return "<b>" + message + "</b>";
    }

    internal static string Italic(this string message)
    {
        return "<i>" + message + "</i>";
    }

    internal static string Underline(this string message)
    {
        return "<u>" + message + "</u>";
    }

    internal static string Color(this string message, Color color)
    {
        return "<font color=\"#" + color.ToArgb().ToString("X") + "\">" + message + "</font>";
    }
}
