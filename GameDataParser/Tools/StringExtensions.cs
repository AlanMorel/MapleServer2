namespace GameDataParser.Tools;

public static class StringExtensions
{
    public static IEnumerable<byte> SplitAndParseToByte(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(byte.Parse);
    }

    public static IEnumerable<sbyte> SplitAndParseToSByte(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(sbyte.Parse);
    }

    public static IEnumerable<short> SplitAndParseToShort(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(short.Parse);
    }

    public static IEnumerable<int> SplitAndParseToInt(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse);
    }

    public static IEnumerable<float> SplitAndParseToFloat(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(float.Parse);
    }

    public static IEnumerable<long> SplitAndParseToLong(this string value, char separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(long.Parse);
    }

    public static IEnumerable<byte> SplitAndParseToByte(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(byte.Parse);
    }

    public static IEnumerable<sbyte> SplitAndParseToSByte(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(sbyte.Parse);
    }

    public static IEnumerable<short> SplitAndParseToShort(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(short.Parse);
    }

    public static IEnumerable<int> SplitAndParseToInt(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(int.Parse);
    }

    public static IEnumerable<float> SplitAndParseToFloat(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(float.Parse);
    }

    public static IEnumerable<long> SplitAndParseToLong(this string value, params char[] separator)
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select(long.Parse);
    }
}
