using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class ItemExchangeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ITEM_EXCHANGE;

        public ItemExchangeHandler(ILogger<ItemExchangeHandler> logger) : base(logger) { }

        private enum ItemExchangeMode : byte
        {
            Use = 0x1,
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
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        public enum ExchangeNotice : short
        {
            Sucess = 0x0,
            Invalid = 0x1,
            CannotFuse = 0x2,
            InsufficientMeso = 0x3,
            InsufficientItems = 0x4,
            EnchantLevelTooHigh = 0x5,
            ItemIsLocked = 0x6,
            CheckFusionAmount = 0x7,
        }

        private static void HandleUse(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            long unk = packet.ReadLong();
            int quantity = packet.ReadInt();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            Item item = session.Player.Inventory.Items[itemUid];

            ItemExchangeScrollMetadata exchange = ItemExchangeScrollMetadataStorage.GetMetadata(item.FunctionId);

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

            Item exchangeRewardItem = new(exchange.RewardId)
            {
                Rarity = exchange.RewardRarity,
                Amount = exchange.RewardAmount * quantity,
            };

            InventoryController.Add(session, exchangeRewardItem, true);
            session.Send(ItemExchangePacket.Notice((short) ExchangeNotice.Sucess));

        }

        private static bool PlayerHasAllIngredients(GameSession session, ItemExchangeScrollMetadata exchange, int quantity)
        {
            // TODO: Check if rarity matches

            List<Item> playerInventoryItems = new(session.Player.Inventory.Items.Values);

            for (int i = 0; i < exchange.ItemCost.Count; i++)
            {
                ItemRequirementMetadata exchangeItem = exchange.ItemCost.ElementAt(i);
                Item item = playerInventoryItems.FirstOrDefault(x => x.Id == exchangeItem.Id);

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
            List<Item> playerInventoryItems = new(session.Player.Inventory.Items.Values);

            if (exchange.ItemCost.Count != 0)
            {
                for (int i = 0; i < exchange.ItemCost.Count; i++)
                {
                    ItemRequirementMetadata exchangeItem = exchange.ItemCost.ElementAt(i);
                    Item item = playerInventoryItems.FirstOrDefault(x => x.Id == exchangeItem.Id);
                    if (item == null)
                    {
                        continue;
                    }
                    InventoryController.Consume(session, item.Uid, exchangeItem.Amount * quantity); 
                }
            }

            InventoryController.Consume(session, originItem.Uid, exchange.RecipeAmount * quantity);

            return true;
        }
    }
}
