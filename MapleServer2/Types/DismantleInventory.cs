using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class DismantleInventory
{
    public Tuple<long, int>[] Slots;
    public Dictionary<int, int> Rewards;

    public void Dismantle(GameSession session)
    {
        foreach (Tuple<long, int> slot in Slots.Where(i => i != null))
        {
            session.Player.Inventory.ConsumeItem(session, slot.Item1, slot.Item2);
        }
        foreach (KeyValuePair<int, int> reward in Rewards)
        {
            Item item = new(reward.Key)
            {
                Amount = reward.Value
            };

            session.Player.Inventory.AddItem(session, item, true);
        }
        Slots = new Tuple<long, int>[100];
        session.Send(ItemBreakPacket.ShowRewards(Rewards));
    }

    public void AutoAdd(GameSession session, InventoryTab inventoryTab, int rarityType)
    {
        Dictionary<long, Item> items = session.Player.Inventory.Items;

        foreach (KeyValuePair<long, Item> item in items)
        {
            if (item.Value.InventoryTab != inventoryTab || item.Value.Rarity > rarityType
                || !item.Value.EnableBreak || Slots.Any(x => x != null && x.Item1 == item.Key))
            {
                continue;
            }
            DismantleAdd(session, -1, item.Value.Uid, item.Value.Amount);
        }
    }

    public void DismantleAdd(GameSession session, short slot, long uid, int amount)
    {
        if (slot >= 0)
        {
            if (Slots[slot] == null)
            {
                Slots[slot] = new(uid, amount);
                session.Send(ItemBreakPacket.Add(uid, slot, amount));
                UpdateRewards(session);
                return;
            }

            slot = -1;
        }

        if (slot == -1)
        {
            for (slot = 0; slot < Slots.Length; slot++)
            {
                if (Slots[slot] != null)
                {
                    continue;
                }
                Slots[slot] = new(uid, amount);
                session.Send(ItemBreakPacket.Add(uid, slot, amount));
                UpdateRewards(session);
                return;
            }
        }
    }

    public void Remove(GameSession session, long uid)
    {
        int index = Array.FindIndex(Slots, 0, Slots.Length, x => x != null && x.Item1 == uid);

        if (index == -1)
        {
            return;
        }

        Slots[index] = null;
        session.Send(ItemBreakPacket.Remove(uid));
        UpdateRewards(session);
    }

    public void UpdateRewards(GameSession session)
    {
        Rewards = new();
        foreach (Tuple<long, int> slot in Slots.Where(x => x != null))
        {
            Item item = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Uid == slot.Item1).Value;
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
                    Rewards[ingredient.Id] *= slot.Item2;
                }
            }
            // TODO: Add Onyx Crystal (40100023) and Chaos Onyx Crystal (40100024) to rewards if InventoryTab = Gear, based on level and rarity
            // TODO: Add rewards for outfits
        }
        session.Send(ItemBreakPacket.Results(Rewards));
    }
}
