using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Types;
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
    public class RequestItemBreakHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_BREAK;

        public RequestItemBreakHandler(ILogger<RequestItemBreakHandler> logger) : base(logger) { }

        private Dictionary<short, long> DismantleSlots;
        private Dictionary<int, int> Rewards;

        private enum ItemBreakMode : byte
        {
            Open = 0x00,
            Add = 0x01,
            Remove = 0x02,
            Dismantle = 0x03,
            AutoAdd = 0x06,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            ItemBreakMode mode = (ItemBreakMode) packet.ReadByte();
            switch (mode)
            {
                case ItemBreakMode.Open:
                    DismantleSlots = new Dictionary<short, long>(100);
                    Rewards = new Dictionary<int, int>();
                    break;
                case ItemBreakMode.Add:
                    HandleAdd(session, packet);
                    break;
                case ItemBreakMode.Remove:
                    HandleRemove(session, packet);
                    break;
                case ItemBreakMode.Dismantle:
                    HandleDismantle(session);
                    break;
                case ItemBreakMode.AutoAdd:
                    HandleAutoAdd(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleAdd(GameSession session, PacketReader packet)
        {
            short slot = (short) packet.ReadInt();
            long uid = packet.ReadLong();
            int amount = packet.ReadInt();

            Add(session, slot, uid, amount);
        }

        private void HandleRemove(GameSession session, PacketReader packet)
        {
            long uid = packet.ReadLong();
            short key = DismantleSlots.FirstOrDefault(x => x.Value == uid).Key;

            DismantleSlots.Remove(key);
            session.Send(ItemBreakPacket.Remove(uid));
            UpdateRewards(session);
        }

        private void HandleDismantle(GameSession session)
        {
            foreach (KeyValuePair<short, long> item in DismantleSlots)
            {
                InventoryController.Remove(session, item.Value, out Item _);
            }
            foreach (KeyValuePair<int, int> item in Rewards)
            {
                InventoryController.Add(session, new Item(item.Key) { Amount = item.Value }, true);
            }
            DismantleSlots = new Dictionary<short, long>(100);
            session.Send(ItemBreakPacket.ShowRewards(Rewards));
        }

        private void UpdateRewards(GameSession session)
        {
            Rewards = new Dictionary<int, int>();
            foreach (KeyValuePair<short, long> slot in DismantleSlots)
            {
                Item item = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Uid == slot.Value).Value;
                if (!ItemMetadataStorage.IsValid(item.Id))
                {
                    continue;
                }

                List<ItemBreakReward> breakRewards = ItemMetadataStorage.GetBreakRewards(item.Id);
                if (breakRewards == null)
                {
                    continue;
                }

                foreach (ItemBreakReward ingredient in breakRewards)
                {
                    if (ingredient.Id != 0)
                    {
                        if (Rewards.ContainsKey(ingredient.Id))
                        {
                            Rewards[ingredient.Id] += ingredient.Count;
                        }
                        else
                        {
                            Rewards[ingredient.Id] = ingredient.Count;
                        }
                    }
                }
                // TODO: Add Onyx Crystal (40100023) and Chaos Onyx Crystal (40100024) to rewards if InventoryTab = Gear, based on level and rarity
                // TODO: Add rewards for outfits
            }
            session.Send(ItemBreakPacket.Results(Rewards));
        }

        private void HandleAutoAdd(GameSession session, PacketReader packet)
        {
            InventoryTab inventoryTab = (InventoryTab) packet.ReadByte();
            byte rarityType = packet.ReadByte();
            Dictionary<long, Item> items = session.Player.Inventory.Items;
            foreach (KeyValuePair<long, Item> item in items)
            {
                if (item.Value.InventoryTab != inventoryTab || item.Value.Rarity > rarityType)
                {
                    continue;
                }
                Add(session, -1, item.Value.Uid, item.Value.Amount);
            }
        }

        private void Add(GameSession session, short slot, long uid, int amount)
        {
            if (slot >= 0)
            {
                if (!DismantleSlots.ContainsKey(slot))
                {
                    DismantleSlots[slot] = uid;
                }
                else
                {
                    slot = -1;
                }
            }

            if (slot == -1)
            {
                for (slot = 0; slot < 100; slot++)
                {
                    if (DismantleSlots.ContainsKey(slot))
                    {
                        continue;
                    }
                    DismantleSlots[slot] = uid;
                    break;
                }
            }

            session.Send(ItemBreakPacket.Add(uid, slot, amount));
            UpdateRewards(session);
        }
    }
}
