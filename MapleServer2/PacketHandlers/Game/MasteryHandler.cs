using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class MasteryHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CONSTRUCT_RECIPE;

        public MasteryHandler(ILogger<MasteryHandler> logger) : base(logger) { }

        private enum MasteryMode : byte
        {
            RewardBox = 0x01,
            CraftItem = 0x02
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            MasteryMode mode = (MasteryMode) packet.ReadByte();
            switch (mode)
            {
                case MasteryMode.RewardBox:
                    HandleRewardBox(session, packet);
                    break;
                case MasteryMode.CraftItem:
                    HandleCraftItem(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleRewardBox(GameSession session, PacketReader packet)
        {
            int rewardBoxDetails = packet.ReadInt();
            int type = rewardBoxDetails / 1000;
            int grade = rewardBoxDetails % 100;

            // get the reward box item ID
            MasteryMetadata mastery = MasteryMetadataStorage.GetMastery(type);
            if (mastery == null)
            {
                Logger.LogError($"Unknown mastery type {type} from user: {session.Player.Name}");
                return;
            }

            int rewardBoxItemId = mastery.Grades[grade - 1].RewardJobItemID;
            Item rewardBox = new Item(rewardBoxItemId) { Amount = 1 };

            // give player the reward box item
            InventoryController.Add(session, rewardBox, true);

            // mark reward box as claimed
            session.Send(MasteryPacket.ClaimReward(rewardBoxDetails, 1, (int) rewardBoxItemId));
        }

        private void HandleCraftItem(GameSession session, PacketReader packet)
        {
            int recipeId = packet.ReadInt();

            // attempt to oad the recipe metadata
            RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(recipeId);
            if (recipe == null)
            {
                Logger.LogError($"Unknown recipe ID {recipeId} from user: {session.Player.Name}");
                return;
            }

            // does the play have enough mesos for this recipe?
            if (!session.Player.Wallet.Meso.Modify(-recipe.GetMesosRequired()))
            {
                // send notice to player saying they haven't got enough mesos
                session.SendNotice("You don't have enough mesos.");
                return;
            }

            // does the player have all the required ingredients for this recipe?
            if (!PlayerHasAllIngredients(session, recipe))
            {
                // send notice to player saying they haven't got enough materials
                session.SendNotice("You've run out of materials.");
                return;
            }

            // only add reward items once all required items & mesos have been removed from player
            if (RemoveRequiredItemsFromInventory(session, recipe))
            {
                AddRewardItemsToInventory(session, recipe);
            }
        }

        private static bool RemoveRequiredItemsFromInventory(GameSession session, RecipeMetadata recipe)
        {
            List<Item> playerInventoryItems = new(session.Player.Inventory.Items.Values);
            List<RecipeItem> ingredients = recipe.GetIngredients();

            for (int i = 0; i < ingredients.Count; i++)
            {
                RecipeItem ingredient = ingredients.ElementAt(i);
                Item item = playerInventoryItems.FirstOrDefault(x => x.Id == ingredient.Id);
                if (item == null)
                {
                    continue;
                }

                // check if whole stack will be used, and remove the item
                // otherwise we want to just want to subtract the amount
                if (ingredient.Amount == item.Amount)
                {
                    InventoryController.Remove(session, item.Uid, out Item _);
                }
                else
                {
                    InventoryController.Update(session, item.Uid, item.Amount - 1);
                }
            }

            return true;
        }

        private static void AddRewardItemsToInventory(GameSession session, RecipeMetadata recipe)
        {
            // award items
            List<RecipeItem> result = recipe.GetResult();
            for (int i = 0; i < result.Count; i++)
            {
                Item rewardItem = new(result.ElementAt(i).Id)
                {
                    Rarity = result.ElementAt(i).Rarity,
                    Amount = result.ElementAt(i).Amount
                };
                InventoryController.Add(session, rewardItem, true);
            }

            // add mastery exp
            session.Player.Levels.GainMasteryExp(Enum.Parse<MasteryType>(recipe.MasteryType, true),
                recipe.RewardMastery);

            // add player exp
            if (recipe.HasExpReward())
            {
                // TODO: add metadata for common exp tables to be able to look up exp amount for masteries etc.
            }
        }

        private static bool PlayerHasEnoughMesos(GameSession session, RecipeMetadata recipe)
        {
            long mesoBalance = session.Player.Wallet.Meso.Amount;
            if (mesoBalance == 0)
            {
                return false;
            }

            return mesoBalance >= recipe.GetMesosRequired();
        }

        private static bool PlayerHasAllIngredients(GameSession session, RecipeMetadata recipe)
        {
            List<Item> playerInventoryItems = new(session.Player.Inventory.Items.Values);
            List<RecipeItem> ingredients = recipe.GetIngredients();

            for (int i = 0; i < ingredients.Count; i++)
            {
                RecipeItem ingredient = ingredients.ElementAt(i);
                Item item = playerInventoryItems.FirstOrDefault(x => x.Id == ingredient.Id);
                if (item != null)
                {
                    return item.Amount >= ingredient.Amount;
                }
            }

            return false;
        }
    }
}
