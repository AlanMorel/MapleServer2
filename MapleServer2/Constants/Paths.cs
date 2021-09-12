namespace MapleServer2.Constants
{
    public static class Paths
    {
        public static readonly string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));
        public static readonly string RESOURCES = Path.Combine(SOLUTION_DIR, "Maple2Storage/Resources");
        public static readonly string JSON = Path.Combine(SOLUTION_DIR, "Maple2Storage/Json");
        public static readonly string AI_DIR = Path.Combine(SOLUTION_DIR, "MobAI");
        public static readonly string SCRIPTS_DIR = Path.Combine(SOLUTION_DIR, "Maple2Storage/Scripts");
    }
}
