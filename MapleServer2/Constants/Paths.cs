using System;
using System.IO;

namespace MapleServer2.Constants
{
    public static class Paths
    {
        public static readonly string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\..\\"));
        public static readonly string RESOURCES = $"{SOLUTION_DIR}/Maple2Storage/Resources";
    }
}
