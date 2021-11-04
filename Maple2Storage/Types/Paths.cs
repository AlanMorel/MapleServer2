namespace Maple2Storage.Types
{
    public static class Paths
    {
        public static readonly string SOLUTION_DIR = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../.."));

        public static readonly string RESOURCES_DIR = $"{SOLUTION_DIR}/Maple2Storage/Resources";
        public static readonly string JSON_DIR = $"{SOLUTION_DIR}/Maple2Storage/Json";
        public static readonly string SCRIPTS_DIR = $"{SOLUTION_DIR}/Maple2Storage/Scripts";

        public static readonly string RESOURCES_INPUT_DIR = $"{SOLUTION_DIR}/GameDataParser/Resources";
        public static readonly string HASH_DIR = $"{SOLUTION_DIR}/GameDataParser/Hash";

        public static readonly string AI_DIR = $"{SOLUTION_DIR}/MobAI";

        public static readonly string DATA_DIR = $"{SOLUTION_DIR}/MapleWebServer/Data";
    }
}
