using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    internal class RecipeParser : Exporter<List<RecipeMetadata>>
    {
        public RecipeParser(MetadataResources resources) : base(resources, "recipe") { }

        protected override List<RecipeMetadata> Parse()
        {
            List<RecipeMetadata> recipeList = new List<RecipeMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/masteryreceipe"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList recipes = document.SelectNodes("/ms2/receipe");

                foreach (XmlNode recipe in recipes)
                {
                    string locale = string.IsNullOrEmpty(recipe.Attributes["locale"]?.Value) ? "" : recipe.Attributes["locale"].Value;
                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    RecipeMetadata newRecipe = new RecipeMetadata();
                    newRecipe.Id = string.IsNullOrEmpty(recipe.Attributes["id"]?.Value) ? 0 : int.Parse(recipe.Attributes["id"].Value);
                    if (!string.IsNullOrEmpty(recipe.Attributes["masteryType"].Value))
                    {
                        newRecipe.MasteryType = short.Parse(recipe.Attributes["masteryType"].Value);
                    }
                    newRecipe.ExceptRewardExp = int.Parse(recipe.Attributes["exceptRewardExp"].Value) == 1;
                    if (!string.IsNullOrEmpty(recipe.Attributes["requireMastery"].Value))
                    {
                        newRecipe.RequireMastery = long.Parse(recipe.Attributes["requireMastery"].Value);
                    }
                    if (!string.IsNullOrEmpty(recipe.Attributes["requireMeso"].Value))
                    {
                        newRecipe.RequireMeso = long.Parse(recipe.Attributes["requireMeso"].Value);
                    }
                    if (!string.IsNullOrEmpty(recipe.Attributes["requireQuest"].Value))
                    {
                        newRecipe.RequireQuest.AddRange(Array.ConvertAll(recipe.Attributes["requireQuest"].Value.Split(","), int.Parse));
                    }
                    if (!string.IsNullOrEmpty(recipe.Attributes["rewardExp"].Value))
                    {
                        newRecipe.RewardExp = long.Parse(recipe.Attributes["rewardExp"].Value);
                    }
                    newRecipe.RewardMastery = string.IsNullOrEmpty(recipe.Attributes["rewardMastery"]?.Value) ? 0 : long.Parse(recipe.Attributes["rewardMastery"].Value);
                    newRecipe.GatheringTime = recipe.Attributes["gatheringTime"].Value;
                    newRecipe.HighPropLimitCount = string.IsNullOrEmpty(recipe.Attributes["highPropLimitCount"]?.Value) ? 0 : int.Parse(recipe.Attributes["highPropLimitCount"].Value);
                    newRecipe.NormalPropLimitCount = string.IsNullOrEmpty(recipe.Attributes["normalPropLimitCount"]?.Value) ? 0 : int.Parse(recipe.Attributes["normalPropLimitCount"].Value);

                    for (int i = 1; i < 6; i++) // 6 being the max amount of required items there can be
                    {
                        if (recipe.Attributes["requireItem" + i.ToString()].Value != "")
                        {
                            RecipeItem requiredItem = new RecipeItem();
                            List<int> itemMetadata = new List<int>();
                            itemMetadata.AddRange(Array.ConvertAll(recipe.Attributes["requireItem" + i.ToString()].Value.Split(","), int.Parse));
                            requiredItem.ItemId = itemMetadata[0];
                            requiredItem.Rarity = itemMetadata[1];
                            requiredItem.Amount = itemMetadata[2];
                            newRecipe.RequiredItems.Add(requiredItem);
                        }
                    }

                    for (int i = 1; i < 6; i++) // 6 being the max amount of reward items there can be
                    {
                        if (recipe.Attributes["rewardItem" + i.ToString()].Value != "")
                        {
                            RecipeItem rewardItem = new RecipeItem();
                            List<int> itemMetadata = new List<int>();
                            itemMetadata.AddRange(Array.ConvertAll(recipe.Attributes["rewardItem" + i.ToString()].Value.Split(","), int.Parse));
                            rewardItem.ItemId = itemMetadata[0];
                            rewardItem.Rarity = itemMetadata[1];
                            rewardItem.Amount = itemMetadata[2];
                            newRecipe.RewardItems.Add(rewardItem);
                        }
                    }

                    newRecipe.HabitatMapId = string.IsNullOrEmpty(recipe.Attributes["habitatMapId"]?.Value) ? 0 : int.Parse(recipe.Attributes["habitatMapId"].Value);
                    recipeList.Add(newRecipe);
                }
            }
            return recipeList;
        }
    }
}
