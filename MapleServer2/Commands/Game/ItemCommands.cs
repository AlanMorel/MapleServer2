using MapleServer2.Commands.Core;
using MapleServer2.Tools;
using MapleServer2.Enums;
using MapleServer2.Data.Static;
using MapleServer2.Types;
using System;

namespace MapleServer2.Commands.Game
{
    public class ItemCommand : InGameCommand
    {
        public ItemCommand()
        {
            Aliases = new[]
            {
                "item"
            };
            Description = "Give an item to the current player.";
            AddParameter<int>("id", "Item id");
            AddParameter<int>("amount", "Amount of the same item.");
            AddParameter<int>("rarity", "Item rarity.");
        }

        public override void Execute(GameCommandTrigger trigger)
        {
            int itemId = trigger.Get<int>("id");
            int rarity = trigger.Get<int>("rarity");
            int amount = trigger.Get<int>("amount");

            if (!ItemMetadataStorage.IsValid(itemId))
            {
                trigger.Session.SendNotice("Invalid item: " + itemId);
                return;
            }

            Item item = new Item(itemId)
            {
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                TransferFlag = TransferFlag.Splitable | TransferFlag.Tradeable,
                PlayCount = itemId.ToString().StartsWith("35") ? 10 : 0,
                Rarity = rarity >= 0 ? rarity : ItemMetadataStorage.GetRarity(itemId),
                Amount = amount >= 0 ? amount : 1
            };
            item.Stats = new ItemStats(item);

            // Simulate looting item
            InventoryController.Add(trigger.Session, item, true);
        }
    }
}
