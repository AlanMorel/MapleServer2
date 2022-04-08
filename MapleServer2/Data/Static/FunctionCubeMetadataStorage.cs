using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class FunctionCubeMetadataStorage
{
    private static readonly Dictionary<int, FunctionCubeMetadata> FunctionCube = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.FunctionCube);
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
