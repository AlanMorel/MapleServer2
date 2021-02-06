using System.Collections.Generic;
using System.IO;
using Maple2Storage.Helpers;
using Maple2Storage.Types.Metadata;
using MapleServer2.Constants;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class RecipeMetadataStorage
    {
        private static readonly Dictionary<int, RecipeMetadata> recipes = new Dictionary<int, RecipeMetadata>();

        static RecipeMetadataStorage()
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
        
        public static List<RecipeItem> GetIngredients(this RecipeMetadata recipe)
        {
            List<RecipeItem> result = new List<RecipeItem>();
            List<string> requiredItems = new List<string>
            {
                recipe.RequireItem1,
                recipe.RequireItem2,
                recipe.RequireItem3,
                recipe.RequireItem4,
                recipe.RequireItem5
            };

            foreach (string requiredItem in requiredItems)
            {
                if (string.IsNullOrEmpty(requiredItem))
                {
                    continue;
                }

                int[] split = StringUtils.SplitStringIntoInts(requiredItem);
                result.Add(new RecipeItem
                {
                    Id = split[0],
                    Amount = split[2]
                });
            }

            return result;
        }

        public static List<RecipeItem> GetResult(this RecipeMetadata recipe)
        {
            List<RecipeItem> result = new List<RecipeItem>();
            List<string> rewardItems = new List<string>
            {
                recipe.RewardItem1,
                recipe.RewardItem2,
                recipe.RewardItem3,
                recipe.RewardItem4,
                recipe.RewardItem5
            };

            foreach (string rewardItem in rewardItems)
            {
                if (string.IsNullOrEmpty(rewardItem))
                {
                    continue;
                }

                int[] split = StringUtils.SplitStringIntoInts(rewardItem);
                result.Add(new RecipeItem
                {
                    Id = split[0],
                    Rarity = split[1],
                    Amount = split[2]
                });
            }

            return result;
        }
        
        public static long GetMesosRequired(this RecipeMetadata recipe)
        {
            if (string.IsNullOrEmpty(recipe.RequireMeso))
            {
                return 0;
            }

            return long.Parse(recipe.RequireMeso);
        }
        
        public static bool HasExpReward(this RecipeMetadata recipe)
        {
            return !recipe.ExceptRewardExp;
        }
    }
    public class RecipeItem
    {
        public int Id;
        public int Rarity;
        public int Amount;
    }
}
