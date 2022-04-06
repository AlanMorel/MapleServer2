using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class RecipeMetadataStorage
{
    private static readonly Dictionary<int, RecipeMetadata> Recipes = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Recipe}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
        List<RecipeMetadata> recipeList = Serializer.Deserialize<List<RecipeMetadata>>(stream);
        foreach (RecipeMetadata recipe in recipeList)
        {
            Recipes[recipe.Id] = recipe;
        }
    }

    public static List<int> GetRecipeIds()
    {
        return new(Recipes.Keys);
    }

    public static RecipeMetadata GetRecipe(int id)
    {
        return Recipes.GetValueOrDefault(id);
    }
}
