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

    private enum Mode : byte
    {
        Use = 0x1
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Use:
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

        if (exchange.ItemCost.Count != 0 && !PlayerHasAllIngredients(session, exchange, quantity))
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

    private static bool PlayerHasAllIngredients(GameSession session, ItemExchangeScrollMetadata exchange, int quantity)
    {
        // TODO: Check if rarity matches

        for (int i = 0; i < exchange.ItemCost.Count; i++)
        {
            ItemRequirementMetadata exchangeItem = exchange.ItemCost.ElementAt(i);
            Item item = session.Player.Inventory.GetById(exchangeItem.Id);

            if (item == null)
            {
                continue;
            }

            return item.Amount >= exchangeItem.Amount * quantity;
        }

        return false;
    }

    private static bool RemoveRequiredItemsFromInventory(GameSession session, ItemExchangeScrollMetadata exchange, Item originItem, int quantity)
    {
        if (exchange.ItemCost.Count != 0)
        {
            for (int i = 0; i < exchange.ItemCost.Count; i++)
            {
                ItemRequirementMetadata exchangeItem = exchange.ItemCost.ElementAt(i);
                Item item = session.Player.Inventory.GetById(exchangeItem.Id);
                if (item == null)
                {
                    continue;
                }

                session.Player.Inventory.ConsumeItem(session, item.Uid, exchangeItem.Amount * quantity);
            }
        }

        session.Player.Inventory.ConsumeItem(session, originItem.Uid, exchange.RecipeAmount * quantity);
        return true;
    }
}
