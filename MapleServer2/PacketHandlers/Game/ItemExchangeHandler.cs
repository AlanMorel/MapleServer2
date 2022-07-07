using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemExchangeHandler : GamePacketHandler<ItemExchangeHandler>
{
    public override RecvOp OpCode => RecvOp.ItemExchange;

    private enum ItemExchangeMode : byte
    {
        Use = 0x1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ItemExchangeMode mode = (ItemExchangeMode) packet.ReadByte();

        switch (mode)
        {
            case ItemExchangeMode.Use:
                HandleUse(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private enum ExchangeNotice : short
    {
        Success = 0x0,
        Invalid = 0x1,
        CannotFuse = 0x2,
        InsufficientMeso = 0x3,
        InsufficientItems = 0x4,
        EnchantLevelTooHigh = 0x5,
        ItemIsLocked = 0x6,
        CheckFusionAmount = 0x7
    }

    private static void HandleUse(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        long unk = packet.ReadLong();
        int quantity = packet.ReadInt();

        if (!session.Player.Inventory.HasItem(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);

        ItemExchangeScrollMetadata exchange = ItemExchangeScrollMetadataStorage.GetMetadata(item.Function.Id);

        if (!session.Player.Wallet.Meso.Modify(-exchange.MesoCost * quantity))
        {
            session.Send(ItemExchangePacket.Notice((short) ExchangeNotice.InsufficientMeso));
            return;
        }

        if (exchange.ItemCost.Count != 0 && !PlayerHasIngredients(exchange, session.Player.Inventory, quantity))
        {
            session.Send(ItemExchangePacket.Notice((short) ExchangeNotice.InsufficientItems));
            return;
        }

        if (!RemoveRequiredItemsFromInventory(session, exchange, item, quantity))
        {
            return;
        }

        Item exchangeRewardItem = new(exchange.RewardId, exchange.RewardAmount * quantity, exchange.RewardRarity);

        session.Player.Inventory.AddItem(session, exchangeRewardItem, true);
        session.Send(ItemExchangePacket.Notice((short) ExchangeNotice.Success));
    }

    private static bool PlayerHasIngredients(ItemExchangeScrollMetadata exchange, IInventory inventory, int quantity)
    {
        foreach (ItemRequirementMetadata ingredient in exchange.ItemCost)
        {
            IReadOnlyCollection<Item> ingredientTotal = inventory.GetAllById(ingredient.Id);
            if (!string.IsNullOrEmpty(ingredient.StringTag))
            {
                ingredientTotal = inventory.GetAllByTag(ingredient.StringTag);
            }

            if (ingredientTotal.Where(x => x.Rarity == ingredient.Rarity).Sum(x => x.Amount) < ingredient.Amount * quantity)
            {
                return false;
            }
        }

        return true;
    }

    private static bool RemoveRequiredItemsFromInventory(GameSession session, ItemExchangeScrollMetadata exchange, Item originItem, int quantity)
    {
        foreach (ItemRequirementMetadata itemRequirement in exchange.ItemCost)
        {
            if (!string.IsNullOrEmpty(itemRequirement.StringTag))
            {
                session.Player.Inventory.ConsumeByTag(session, itemRequirement.StringTag, itemRequirement.Amount * quantity, itemRequirement.Rarity);
                continue;
            }

            session.Player.Inventory.ConsumeById(session, itemRequirement.Id, itemRequirement.Amount * quantity, itemRequirement.Rarity);
        }

        session.Player.Inventory.ConsumeItem(session, originItem.Uid, exchange.RecipeAmount * quantity);
        return true;
    }
}
