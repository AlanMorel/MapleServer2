using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FunctionCubeMetadataStorage
{
    private static readonly Dictionary<int, FunctionCubeMetadata> FunctionCube = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.FunctionCube}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<FunctionCubeMetadata> list = Serializer.Deserialize<List<FunctionCubeMetadata>>(stream);
        foreach (FunctionCubeMetadata metadata in list)
        {
            FunctionCube[metadata.CubeId] = metadata;
        }
    }

    public static int GetRecipeId(int cubeId)
    {
        return FunctionCube.GetValueOrDefault(cubeId)?.RecipeId ?? 0;
    }
}
