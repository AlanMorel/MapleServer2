using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class GatheringHelper
{
    public static void HandleGathering(GameSession session, int recipeId, out int numDrop)
    {
        numDrop = 0;
        RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(recipeId);
        if (recipe == null)
        {
            return;
        }

        long currentMastery = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == (MasteryType) recipe.MasteryType).CurrentExp;
        if (currentMastery < recipe.RequireMastery)
        {
            return;
        }

        session.Player.IncrementGatheringCount(recipe.Id, 0);
        int numCount = session.Player.GatheringCount.FirstOrDefault(x => x.RecipeId == recipe.Id).CurrentCount;

        List<RecipeItem> items = recipe.RewardItems;
        int masteryDiffFactor = numCount switch
        {
            int n when n < recipe.HighPropLimitCount => MasteryFactorMetadataStorage.GetFactor(0),
            int n when n < recipe.NormalPropLimitCount => MasteryFactorMetadataStorage.GetFactor(1),
            int n when n < (int) (recipe.NormalPropLimitCount * 1.3) => MasteryFactorMetadataStorage.GetFactor(2),
            _ => MasteryFactorMetadataStorage.GetFactor(3)
        };

        if (masteryDiffFactor == 0)
        {
            return;
        }

        foreach (RecipeItem item in items)
        {
            int prob = masteryDiffFactor / 100;
            if (RandomProvider.Get().Next(100) >= prob)
            {
                continue;
            }

            session.FieldManager.AddItem(session, new(item.ItemId)
            {
                Rarity = item.Rarity, Amount = item.Amount
            });

            numDrop += item.Amount;
        }
        if (numDrop <= 0)
        {
            return;
        }

        session.Player.IncrementGatheringCount(recipe.Id, 1);
        session.Player.Levels.GainMasteryExp((MasteryType) recipe.MasteryType, recipe.RewardMastery);
    }
}
