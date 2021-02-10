using System.Collections.Generic;
using System.Xml;
using GameDataParser.Crypto.Common;
using GameDataParser.Files;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    class RecipeParser : Exporter<List<RecipeMetadata>>
    {
        public RecipeParser(MetadataResources resources) : base(resources, "recipe") { }

        protected override List<RecipeMetadata> Parse()
        {
            List<RecipeMetadata> recipeList = new List<RecipeMetadata>();
            foreach (PackFileEntry entry in Resources.XmlFiles)
            {
                if (!entry.Name.StartsWith("table/masteryreceipe"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlMemFile.GetDocument(entry.FileHeader);
                XmlNodeList recipes = document.SelectNodes("/ms2/receipe");

                foreach (XmlNode recipe in recipes)
                {
                    RecipeMetadata newRecipe = new RecipeMetadata();
                    newRecipe.Id = string.IsNullOrEmpty(recipe.Attributes["id"]?.Value) ? 0 : int.Parse(recipe.Attributes["id"].Value);
                    newRecipe.MasteryType = recipe.Attributes["masteryType"].Value;
                    newRecipe.ExceptRewardExp = int.Parse(recipe.Attributes["exceptRewardExp"].Value) == 1;
                    newRecipe.RequireMastery = recipe.Attributes["requireMastery"].Value;
                    newRecipe.RequireMeso = recipe.Attributes["requireMeso"].Value;
                    newRecipe.RequireQuest = recipe.Attributes["requireQuest"].Value;
                    newRecipe.RewardExp = recipe.Attributes["rewardExp"].Value;
                    newRecipe.RewardMastery = string.IsNullOrEmpty(recipe.Attributes["rewardMastery"]?.Value) ? 0 : long.Parse(recipe.Attributes["rewardMastery"].Value);
                    newRecipe.GatheringTime = recipe.Attributes["gatheringTime"].Value;
                    newRecipe.HighPropLimitCount = string.IsNullOrEmpty(recipe.Attributes["habitatMapId"]?.Value) ? 0 : int.Parse(recipe.Attributes["habitatMapId"].Value);
                    newRecipe.NormalPropLimitCount = string.IsNullOrEmpty(recipe.Attributes["habitatMapId"]?.Value) ? 0 : int.Parse(recipe.Attributes["habitatMapId"].Value);
                    newRecipe.RequireItem1 = recipe.Attributes["requireItem1"].Value;
                    newRecipe.RequireItem2 = recipe.Attributes["requireItem2"].Value;
                    newRecipe.RequireItem3 = recipe.Attributes["requireItem3"].Value;
                    newRecipe.RequireItem4 = recipe.Attributes["requireItem4"].Value;
                    newRecipe.RequireItem5 = recipe.Attributes["requireItem5"].Value;
                    newRecipe.HabitatMapId = string.IsNullOrEmpty(recipe.Attributes["habitatMapId"]?.Value) ? 0 : int.Parse(recipe.Attributes["habitatMapId"].Value);
                    newRecipe.RewardItem1 = recipe.Attributes["rewardItem1"].Value;
                    newRecipe.RewardItem2 = recipe.Attributes["rewardItem2"].Value;
                    newRecipe.RewardItem3 = recipe.Attributes["rewardItem3"].Value;
                    newRecipe.RewardItem4 = recipe.Attributes["rewardItem4"].Value;
                    newRecipe.RewardItem5 = recipe.Attributes["rewardItem5"].Value;
                    newRecipe.Feature = recipe.Attributes["feature"].Value;
                    recipeList.Add(newRecipe);
                }
            }
            return recipeList;
        }
    }
}
