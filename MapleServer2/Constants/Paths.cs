using System;
using System.IO;

namespace MapleServer2.Constants
{
    public static class Paths
    {
        public static string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        public static string RESOURCES = $"{SOLUTION_DIR}/Maple2Storage/Resources";
    }
}
