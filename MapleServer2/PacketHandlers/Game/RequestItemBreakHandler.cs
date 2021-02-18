using System;
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
                    session.Player.DismantleSlots = new Tuple<long, int>[100];
                    session.Player.Rewards = new Dictionary<int, int>();
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

        private static void HandleAdd(GameSession session, PacketReader packet)
        {
            short slot = (short) packet.ReadInt();
            long uid = packet.ReadLong();
            int amount = packet.ReadInt();

            DismantleAdd(session, slot, uid, amount);
        }

        private static void HandleRemove(GameSession session, PacketReader packet)
        {
            long uid = packet.ReadLong();
            Tuple<long, int>[] dismantleSlots = session.Player.DismantleSlots;
            int index = Array.FindIndex(dismantleSlots, 0, dismantleSlots.Length, x => x != null && x.Item1 == uid);

            session.Player.DismantleSlots[index] = null;
            session.Send(ItemBreakPacket.Remove(uid));
            UpdateRewards(session);
        }

        private static void HandleDismantle(GameSession session)
        {
            Player player = session.Player;

            foreach (Tuple<long, int> slot in player.DismantleSlots.Where(i => i != null))
            {
                InventoryController.Consume(session, slot.Item1, slot.Item2);
            }
            foreach (KeyValuePair<int, int> reward in player.Rewards)
            {
                Item item = new Item(reward.Key)
                {
                    Amount = reward.Value
                };

                InventoryController.Add(session, item, true);
            }
            player.DismantleSlots = new Tuple<long, int>[100];
            session.Send(ItemBreakPacket.ShowRewards(player.Rewards));
        }

        private static void UpdateRewards(GameSession session)
        {
            Player player = session.Player;
            player.Rewards = new Dictionary<int, int>();
            foreach (Tuple<long, int> slot in player.DismantleSlots.Where(x => x != null))
            {
                Item item = player.Inventory.Items.FirstOrDefault(x => x.Value.Uid == slot.Item1).Value;
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
                        if (player.Rewards.ContainsKey(ingredient.Id))
                        {
                            player.Rewards[ingredient.Id] += ingredient.Count;
                        }
                        else
                        {
                            player.Rewards[ingredient.Id] = ingredient.Count;
                        }
                        player.Rewards[ingredient.Id] *= slot.Item2;
                    }
                }
                // TODO: Add Onyx Crystal (40100023) and Chaos Onyx Crystal (40100024) to rewards if InventoryTab = Gear, based on level and rarity
                // TODO: Add rewards for outfits
            }
            session.Send(ItemBreakPacket.Results(player.Rewards));
        }

        private static void HandleAutoAdd(GameSession session, PacketReader packet)
        {
            InventoryTab inventoryTab = (InventoryTab) packet.ReadByte();
            byte rarityType = packet.ReadByte();
            Player player = session.Player;
            Dictionary<long, Item> items = player.Inventory.Items;

            foreach (KeyValuePair<long, Item> item in items)
            {
                if (item.Value.InventoryTab != inventoryTab || item.Value.Rarity > rarityType || !item.Value.EnableBreak)
                {
                    continue;
                }
                DismantleAdd(session, -1, item.Value.Uid, item.Value.Amount);
            }
        }

        private static void DismantleAdd(GameSession session, short slot, long uid, int amount)
        {
            Player player = session.Player;
            if (slot >= 0)
            {
                if (player.DismantleSlots[slot] == null)
                {
                    player.DismantleSlots[slot] = new Tuple<long, int>(uid, amount);
                    session.Send(ItemBreakPacket.Add(uid, slot, amount));
                    UpdateRewards(session);
                    return;
                }
                else
                {
                    slot = -1;
                }
            }

            if (slot == -1)
            {
                for (slot = 0; slot < player.DismantleSlots.Length; slot++)
                {
                    if (player.DismantleSlots[slot] != null)
                    {
                        continue;
                    }
                    player.DismantleSlots[slot] = new Tuple<long, int>(uid, amount);
                    session.Send(ItemBreakPacket.Add(uid, slot, amount));
                    UpdateRewards(session);
                    return;
                }
            }
        }
    }
}
