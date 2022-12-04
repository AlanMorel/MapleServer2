using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Tools;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

internal class RecipeParser : Exporter<List<RecipeMetadata>>
{
    public RecipeParser(MetadataResources resources) : base(resources, MetadataName.Recipe) { }

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
            XmlNodeList? recipes = document.SelectNodes("/ms2/receipe");
            if (recipes is null)
            {
                continue;
            }

            foreach (XmlNode recipe in recipes)
            {
                string locale = recipe.Attributes?["locale"]?.Value ?? "";
                if (locale != "NA" && locale != "")
                {
                    continue;
                }

                if (string.IsNullOrEmpty(recipe.Attributes?["id"]?.Value) || recipe.Attributes is null)
                {
                    continue;
                }

                RecipeMetadata newRecipe = new()
                {
                    Id = int.Parse(recipe.Attributes["id"]?.Value ?? "0"),
                    MasteryType = short.Parse(recipe.Attributes["masteryType"]?.Value ?? "0"),
                    ExceptRewardExp = int.Parse(recipe.Attributes["exceptRewardExp"]?.Value ?? "0") == 1,
                    RequireMastery = long.Parse(recipe.Attributes["requireMastery"]?.Value ?? "0"),
                    RequireQuest = recipe.Attributes["requireQuest"]?.Value.SplitAndParseToInt(',').ToList(),
                    RewardMastery = long.Parse(recipe.Attributes["rewardMastery"]?.Value ?? "0"),
                    GatheringTime = recipe.Attributes["gatheringTime"]?.Value ?? "",
                };

                _ = long.TryParse(recipe.Attributes["requireMeso"]?.Value ?? "0", out newRecipe.RequireMeso);
                _ = long.TryParse(recipe.Attributes["rewardExp"]?.Value ?? "0", out newRecipe.RewardExp);
                _ = int.TryParse(recipe.Attributes["highPropLimitCount"]?.Value ?? "0", out newRecipe.HighPropLimitCount);
                _ = int.TryParse(recipe.Attributes["normalPropLimitCount"]?.Value ?? "0", out newRecipe.NormalPropLimitCount);

                for (int i = 1; i < 6; i++) // 6 being the max amount of required items there can be
                {
                    if (recipe.Attributes["requireItem" + i]?.Value == "")
                    {
                        continue;
                    }

                    RecipeItem requiredItem = new();
                    List<int> itemMetadata = recipe.Attributes["requireItem" + i]!.Value.SplitAndParseToInt(',').ToList();
                    requiredItem.ItemId = itemMetadata[0];
                    requiredItem.Rarity = itemMetadata[1];
                    requiredItem.Amount = itemMetadata[2];
                    newRecipe.RequiredItems.Add(requiredItem);
                }

                for (int i = 1; i < 6; i++) // 6 being the max amount of reward items there can be
                {
                    if (recipe.Attributes["rewardItem" + i]?.Value == "")
                    {
                        continue;
                    }

                    RecipeItem rewardItem = new();
                    List<int> itemMetadata = recipe.Attributes["rewardItem" + i]!.Value.SplitAndParseToInt(',').ToList();
                    rewardItem.ItemId = itemMetadata[0];
                    rewardItem.Rarity = itemMetadata[1];
                    rewardItem.Amount = itemMetadata[2];
                    newRecipe.RewardItems.Add(rewardItem);
                }

                _ = int.TryParse(recipe.Attributes["habitatMapId"]?.Value ?? "0", out newRecipe.HabitatMapId);
                recipeList.Add(newRecipe);
            }
        }

        return recipeList;
    }
}
