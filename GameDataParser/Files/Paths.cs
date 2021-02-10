using System;
using System.IO;

namespace GameDataParser.Files
{
    public static class Paths
    {
        public static readonly string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        public static readonly string INPUT = $"{SOLUTION_DIR}/GameDataParser/Resources";
        public static readonly string OUTPUT = $"{SOLUTION_DIR}/Maple2Storage/Resources";
        public static readonly string HASH = $"{SOLUTION_DIR}/GameDataParser/Hash";
    }
}
