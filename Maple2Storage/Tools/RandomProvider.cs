namespace Maple2Storage.Tools;

/// <summary>
/// Thread-safe provider for "Random" instances. Use whenever no custom
/// seed is required.
/// </summary>
public static class RandomProvider
{
    private static readonly Random Seed = new();

    private static readonly ThreadLocal<Random> RandomWrapper = new(() =>
    {
        lock (Seed)
        {
            return new(Seed.Next());
        }
    });

    /// <summary>
    /// Returns an instance of Random for the calling thread.
    /// </summary>
    /// <returns></returns>
    public static Random Get()
    {
        return RandomWrapper.Value;
    }
}
