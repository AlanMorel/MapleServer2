using Pastel;

namespace MapleServer2.Extensions
{
    public static class StringExtensions
    {
        public static string ColorGreen(this string input) => input.Pastel("#aced66");

        public static string ColorRed(this string input) => input.Pastel("#E05561");
    }
}
