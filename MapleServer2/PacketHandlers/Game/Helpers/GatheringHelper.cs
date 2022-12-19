using System.Diagnostics;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class GatheringHelper
{
    public static void HandleGathering(GameSession session, int recipeId, out int numDrop)
    {
        numDrop = 0;
        RecipeMetadata? recipe = RecipeMetadataStorage.GetRecipe(recipeId);
        if (recipe is null)
        {
            return;
        }

        long currentMastery = session.Player.Levels.MasteryExp.First(x => x.Type == (MasteryType) recipe.MasteryType).CurrentExp;
        if (currentMastery < recipe.RequireMastery)
        {
            return;
        }

        session.Player.IncrementGatheringCount(recipe.Id, 0);
        int numCount = session.Player.GatheringCount.FirstOrDefault(x => x.RecipeId == recipe.Id)?.CurrentCount ?? 0;

        List<RecipeItem> items = recipe.RewardItems;
        int? masteryDiffFactor = numCount switch
        {
            var n when n < recipe.HighPropLimitCount => MasteryFactorMetadataStorage.GetFactor(0),
            var n when n < recipe.NormalPropLimitCount => MasteryFactorMetadataStorage.GetFactor(1),
            var n when n < (int) (recipe.NormalPropLimitCount * 1.3) => MasteryFactorMetadataStorage.GetFactor(2),
            _ => MasteryFactorMetadataStorage.GetFactor(3)
        };

        if (masteryDiffFactor is null or 0)
        {
            return;
        }

        foreach (RecipeItem item in items)
        {
            int prob = (int) (masteryDiffFactor / 100);
            if (Random.Shared.Next(100) >= prob)
            {
                continue;
            }

            Debug.Assert(session.Player.FieldPlayer != null, "session.Player.FieldPlayer != null");
            session.FieldManager.AddItem(session.Player.FieldPlayer, new(item.ItemId, item.Amount, item.Rarity, saveToDatabase: false));

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
