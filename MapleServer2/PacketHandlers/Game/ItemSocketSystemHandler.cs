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
    public class ItemSocketSystemHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ITEM_SOCKET_SYSTEM;

        public ItemSocketSystemHandler(ILogger<ItemSocketSystemHandler> logger) : base(logger) { }

        private enum ItemSocketSystemMode : byte
        {
            UpgradeGem = 0x4,
            SelectGemUpgrade = 0x6,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ItemSocketSystemMode mode = (ItemSocketSystemMode) packet.ReadByte();

            switch (mode)
            {
                case ItemSocketSystemMode.SelectGemUpgrade:
                    HandleSelectGemUpgrade(session, packet);
                    break;
                case ItemSocketSystemMode.UpgradeGem:
                    HandleUpgradeGem(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleUpgradeGem(GameSession session, PacketReader packet)
        {
            long unk = packet.ReadLong(); // empty?
            byte unk2 = packet.ReadByte(); // always -1?
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            Item gem = session.Player.Inventory.Items[itemUid];

            ItemGemstoneUpgradeMetadata metadata = ItemGemstoneUpgradeMetadataStorage.GetMetadata(gem.Id);
            if (metadata == null || metadata.NextItemId == 0)
            {
                return;
            }

            if (!CheckGemUpgradeIngredients(session.Player.Inventory, metadata))
            {
                return;
            }

            ConsumeIngredients(session, metadata);
        }

        private static bool CheckGemUpgradeIngredients(Inventory inventory, ItemGemstoneUpgradeMetadata metadata)
        {
            for (int i = 0; i < metadata.IngredientItems.Count; i++)
            {
                int inventoryItemCount = 0;
                List<KeyValuePair<long, Item>> ingredients = new List<KeyValuePair<long, Item>>();
                ingredients = inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();
                ingredients.ForEach(x => inventoryItemCount += x.Value.Amount);

                if (inventoryItemCount < metadata.IngredientAmounts[i])
                {
                    return false;
                }
            }
            return true;
        }

        private static void ConsumeIngredients(GameSession session, ItemGemstoneUpgradeMetadata metadata)
        {
            for (int i = 0; i < metadata.IngredientItems.Count; i++)
            {
                List<KeyValuePair<long, Item>> ingredients = new List<KeyValuePair<long, Item>>();
                ingredients = session.Player.Inventory.Items.Where(x => x.Value.Tag == metadata.IngredientItems[i]).ToList();

                foreach (KeyValuePair<long, Item> item in ingredients)
                {
                    if (item.Value.Amount >= metadata.IngredientAmounts[i])
                    {
                        InventoryController.Consume(session, item.Key, metadata.IngredientAmounts[i]);
                        break;
                    }
                    else
                    {
                        metadata.IngredientAmounts[i] -= item.Value.Amount;
                        InventoryController.Consume(session, item.Key, item.Value.Amount);
                    }
                }
            }
        }

        private static void HandleSelectGemUpgrade(GameSession session, PacketReader packet)
        {
            long unk = packet.ReadLong(); // empty?
            byte unk2 = packet.ReadByte(); // always -1?
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            session.Send(ItemSocketSystemPacket.SelectGemUpgrade(itemUid));
        }
    }
}
