namespace MapleServer2.Tools;

public static class ListExtension
{
    public static IEnumerable<List<T>> SplitList<T>(this List<T> locations, int nSize = 30)
    {
        for (int i = 0; i < locations.Count; i += nSize)
        {
            yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
        }
    }
}
