using System;
using System.Threading;

namespace Maple2Storage.Tools
{
    /// <summary>
    /// Thread-safe provider for "Random" instances. Use whenever no custom
    /// seed is required.
    /// </summary>
    public static class RandomProvider
    {
        private static readonly Random _seed = new Random();

        private static readonly ThreadLocal<Random> randomWrapper = new ThreadLocal<Random>(() =>
        {
            lock (_seed)
            {
                return new Random(_seed.Next());
            }
        });

        /// <summary>
        /// Returns an instance of Random for the calling thread.
        /// </summary>
        /// <returns></returns>
        public static Random Get() => randomWrapper.Value;
    }
}
