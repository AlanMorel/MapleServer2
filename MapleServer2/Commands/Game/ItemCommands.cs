using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public class ItemCommand : InGameCommand
{
    public ItemCommand()
    {
        Aliases = new()
        {
            "item"
        };
        Description = "Give an item to the current player.";
        Parameters = new()
        {
            new Parameter<int>("id", "Item id", 20000027),
            new Parameter<int>("amount", "Amount of the same item.", 1),
            new Parameter<int>("rarity", "Item rarity.")
        };
        Usage = "/item [id] [amount] [rarity]";
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

        Item item = new(itemId)
        {
            Rarity = rarity >= 0 ? rarity : ItemMetadataStorage.GetRarity(itemId),
            TransferFlag = ItemMetadataStorage.GetTransferFlag(itemId, rarity),
            Amount = Math.Max(1, amount)
        };
        item.Stats = new(item);

        // Simulate looting item
        trigger.Session.Player.Inventory.AddItem(trigger.Session, item, true);
    }
}
