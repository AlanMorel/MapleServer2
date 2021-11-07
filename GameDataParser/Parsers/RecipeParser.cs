using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class RecipeParser : Exporter<List<RecipeMetadata>>
{
    public RecipeParser(MetadataResources resources) : base(resources, "recipe") { }

    protected override List<RecipeMetadata> Parse()
    {
        List<RecipeMetadata> recipeList = new();
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
                string locale = recipe.Attributes["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (string.IsNullOrEmpty(recipe.Attributes["id"]?.Value))
                {
                    continue;
                }

                RecipeMetadata newRecipe = new();
                newRecipe.Id = int.Parse(recipe.Attributes["id"]?.Value ?? "0");

                newRecipe.MasteryType = short.Parse(recipe.Attributes["masteryType"]?.Value ?? "0");
                newRecipe.ExceptRewardExp = int.Parse(recipe.Attributes["exceptRewardExp"].Value) == 1;
                newRecipe.RequireMastery = long.Parse(recipe.Attributes["requireMastery"]?.Value ?? "0");
                newRecipe.RequireQuest = recipe.Attributes["requireQuest"]?.Value.Split(",").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();
                newRecipe.RewardMastery = long.Parse(recipe.Attributes["rewardMastery"]?.Value ?? "0");
                newRecipe.GatheringTime = recipe.Attributes["gatheringTime"].Value;

                _ = long.TryParse(recipe.Attributes["requireMeso"]?.Value ?? "0", out newRecipe.RequireMeso);
                _ = long.TryParse(recipe.Attributes["rewardExp"]?.Value ?? "0", out newRecipe.RewardExp);
                _ = int.TryParse(recipe.Attributes["highPropLimitCount"]?.Value ?? "0", out newRecipe.HighPropLimitCount);
                _ = int.TryParse(recipe.Attributes["normalPropLimitCount"]?.Value ?? "0", out newRecipe.NormalPropLimitCount);

                for (int i = 1; i < 6; i++) // 6 being the max amount of required items there can be
                {
                    if (recipe.Attributes["requireItem" + i.ToString()].Value != "")
                    {
                        RecipeItem requiredItem = new();
                        List<int> itemMetadata = recipe.Attributes["requireItem" + i.ToString()].Value.Split(",").Select(int.Parse).ToList();
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
                        RecipeItem rewardItem = new();
                        List<int> itemMetadata = recipe.Attributes["rewardItem" + i.ToString()].Value.Split(",").Select(int.Parse).ToList();
                        rewardItem.ItemId = itemMetadata[0];
                        rewardItem.Rarity = itemMetadata[1];
                        rewardItem.Amount = itemMetadata[2];
                        newRecipe.RewardItems.Add(rewardItem);
                    }
                }

                _ = int.TryParse(recipe.Attributes["habitatMapId"]?.Value ?? "0", out newRecipe.HabitatMapId);
                recipeList.Add(newRecipe);
            }
        }
        return recipeList;
    }
}
