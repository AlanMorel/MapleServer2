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
        foreach ((long uid, int amount) in Slots.Where(i => i != null))
        {
            session.Player.Inventory.ConsumeItem(session, uid, amount);
        }

        foreach ((int id, int amount) in Rewards)
        {
            Item item = new(id)
            {
                Amount = amount
            };

            session.Player.Inventory.AddItem(session, item, true);
        }

        Slots = new Tuple<long, int>[100];
        session.Send(ItemBreakPacket.ShowRewards(Rewards));
    }

    public void AutoAdd(GameSession session, InventoryTab inventoryTab, int rarityType)
    {
        Dictionary<long, Item> items = session.Player.Inventory.Items;

        foreach ((long uid, Item item) in items)
        {
            if (item.InventoryTab != inventoryTab || item.Rarity > rarityType || !item.EnableBreak || Slots.Any(x => x != null && x.Item1 == uid))
            {
                continue;
            }

            DismantleAdd(session, -1, item.Uid, item.Amount);
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
        foreach ((long uid, int amount) in Slots.Where(x => x != null))
        {
            Item item = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Uid == uid).Value;
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
                if (ingredient.Id == 0)
                {
                    continue;
                }

                if (Rewards.ContainsKey(ingredient.Id))
                {
                    Rewards[ingredient.Id] += ingredient.Count;
                }
                else
                {
                    Rewards[ingredient.Id] = ingredient.Count;
                }

                Rewards[ingredient.Id] *= amount;
            }
            // TODO: Add Onyx Crystal (40100023) and Chaos Onyx Crystal (40100024) to rewards if InventoryTab = Gear, based on level and rarity
            // TODO: Add rewards for outfits
        }

        session.Send(ItemBreakPacket.Results(Rewards));
    }
}
