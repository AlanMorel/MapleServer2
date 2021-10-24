using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class FunctionCubeMetadataStorage
    {
        private static readonly Dictionary<int, FunctionCubeMetadata> map = new Dictionary<int, FunctionCubeMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-function-cube-metadata");
            List<FunctionCubeMetadata> list = Serializer.Deserialize<List<FunctionCubeMetadata>>(stream);
            foreach (FunctionCubeMetadata metadata in list)
            {
                map[metadata.CubeId] = metadata;
            }
        }

        public static int GetRecipeId(int cubeId) => map.GetValueOrDefault(cubeId)?.RecipeId ?? 0;
    }
}
