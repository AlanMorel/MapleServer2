using System;
using System.IO;

namespace GameDataParser.Files
{
    public static class Paths
    {
        public static string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        public static string INPUT = $"{SOLUTION_DIR}/GameDataParser/Resources";
        public static string OUTPUT = $"{SOLUTION_DIR}/Maple2Storage/Resources";
        public static string HASH = $"{SOLUTION_DIR}/GameDataParser/Hash";
    }
}
