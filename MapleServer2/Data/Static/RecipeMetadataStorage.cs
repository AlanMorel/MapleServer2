using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class RecipeMetadataStorage
    {
        private static readonly Dictionary<int, RecipeMetadata> recipes = new Dictionary<int, RecipeMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES}/ms2-recipe-metadata");
            List<RecipeMetadata> recipeList = Serializer.Deserialize<List<RecipeMetadata>>(stream);
            foreach (RecipeMetadata recipe in recipeList)
            {
                recipes[recipe.Id] = recipe;
            }
        }

        public static List<int> GetRecipeIds()
        {
            return new List<int>(recipes.Keys);
        }

        public static RecipeMetadata GetRecipe(int id)
        {
            return recipes.GetValueOrDefault(id);
        }
    }
}
