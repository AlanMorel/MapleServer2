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

    public static Type EnumParseDefault<Type>(this string? value, Type defaultValue) where Type : struct, Enum
    {
        if (Enum.TryParse(value, true, out Type result))
        {
            return result;
        }

        return defaultValue;
    }

    public static IEnumerable<Type> SplitAndParseToEnum<Type>(this string value, char separator, Type defaultValue) where Type : struct, Enum
    {
        return value.Split(separator)
            .Where(x => !string.IsNullOrEmpty(x))
            .Select((value) => value.EnumParseDefault<Type>(defaultValue));
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
