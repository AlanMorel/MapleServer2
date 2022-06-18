using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class DismantleInventory
{
    public (long Uid, int Amount)[] Slots;
    public Dictionary<int, int> Rewards;

    public void Dismantle(GameSession session)
    {
        foreach ((long uid, int amount) in Slots.Where(x => x != default))
        {
            session.Player.Inventory.ConsumeItem(session, uid, amount);
        }

        foreach ((int id, int amount) in Rewards)
        {
            session.Player.Inventory.AddItem(session, new(id, amount), true);
        }

        Slots = new (long, int)[100];
        session.Send(ItemBreakPacket.ShowRewards(Rewards));
    }

    public void AutoAdd(GameSession session, InventoryTab inventoryTab, int rarityType)
    {
        IReadOnlyCollection<Item> items = session.Player.Inventory.GetItems(inventoryTab);

        foreach (Item item in items)
        {
            if (item.Rarity > rarityType || item.GachaDismantleId is 0 || Slots.Any(x => x.Uid == item.Uid))
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
            if (Slots[slot] == (0, 0))
            {
                Slots[slot] = (uid, amount);
                session.Send(ItemBreakPacket.Add(uid, slot, amount));
                UpdateRewards(session);
                return;
            }

            slot = -1;
        }

        if (slot != -1)
        {
            return;
        }

        for (slot = 0; slot < Slots.Length; slot++)
        {
            if (Slots[slot] != (0, 0))
            {
                continue;
            }

            Slots[slot] = (uid, amount);
            session.Send(ItemBreakPacket.Add(uid, slot, amount));
            UpdateRewards(session);
            return;
        }
    }

    public void Remove(GameSession session, long uid)
    {
        int index = Array.FindIndex(Slots, 0, Slots.Length, x => x.Uid == uid);

        if (index == -1)
        {
            return;
        }

        Slots[index] = (0, 0);
        session.Send(ItemBreakPacket.Remove(uid));
        UpdateRewards(session);
    }

    private void UpdateRewards(GameSession session)
    {
        Rewards = new();
        foreach ((long uid, int amount) in Slots.Where(x => x != default))
        {
            Item item = session.Player.Inventory.GetByUid(uid);
            if (!ItemMetadataStorage.IsValid(item.Id))
            {
                continue;
            }

            List<ItemBreakReward> breakRewards = ItemMetadataStorage.GetBreakRewards(item.Id);
            if (breakRewards != null)
            {
                foreach (ItemBreakReward ingredient in breakRewards)
                {
                    if (ingredient.Id == 0)
                    {
                        continue;
                    }

                    AddReward(ingredient.Id, ingredient.Count, amount);
                }
            }
            // TODO: Add Onyx Crystal (40100023) and Chaos Onyx Crystal (40100024) to rewards if InventoryTab = Gear, based on level and rarity

            // Cosmetics gacha coins
            GachaMetadata gachaMetadata = GachaMetadataStorage.GetMetadata(item.GachaDismantleId);
            if (gachaMetadata is not null)
            {
                int ingredientCount = item.Rarity switch
                {
                    (int) RarityType.Epic => 3,
                    (int) RarityType.Legendary => 5,
                    _ => 1
                };
                AddReward(gachaMetadata.CoinId, ingredientCount, amount);
            }
        }

        session.Send(ItemBreakPacket.Results(Rewards));
    }

    private void AddReward(int ingredientId, int ingredientCount, int multiplier)
    {
        int reward = ingredientCount * multiplier;
        if (Rewards.ContainsKey(ingredientId))
        {
            Rewards[ingredientId] += reward;
            return;
        }

        Rewards[ingredientId] = reward;
    }
}
