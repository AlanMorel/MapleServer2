using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class RecipeMetadataStorage
{
    private static readonly Dictionary<int, RecipeMetadata> Recipes = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Recipe);
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

    public static RecipeMetadata? GetRecipe(int id)
    {
        return Recipes.GetValueOrDefault(id);
    }
}
