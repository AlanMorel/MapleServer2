using Pastel;

namespace Maple2Storage.Extensions;

public static class StringExtensions
{
    public static string ColorGreen(this string input)
    {
        return input.Pastel("#aced66");
    }

    public static string ColorRed(this string input)
    {
        return input.Pastel("#E05561");
    }

    public static string ColorYellow(this string input)
    {
        return input.Pastel("#FFE212");
    }
}
