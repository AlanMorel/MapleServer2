using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ItemBoxHelper
{
    public static List<Item> GetItemsFromDropGroup(DropGroupContent dropContent, Player player, Item sourceItem)
    {
        List<Item> items = new();
        Random rng = Random.Shared;
        int amount = rng.Next((int) dropContent.MinAmount, (int) dropContent.MaxAmount);

        foreach (int id in dropContent.ItemIds)
        {
            if (dropContent.SmartDropRate == 100)
            {
                List<Job> recommendJobs = ItemMetadataStorage.GetRecommendJobs(id);
                if (!recommendJobs.Contains(player.Job) && !recommendJobs.Contains(Job.None))
                {
                    continue;
                }
            }

            if (dropContent.SmartGender)
            {
                Gender itemGender = ItemMetadataStorage.GetLimitMetadata(id).Gender;
                if (itemGender != player.Gender && itemGender is not Gender.Neutral)
                {
                    continue;
                }
            }


            int rarity = dropContent.Rarity;
            int constant = ItemMetadataStorage.GetOptionMetadata(sourceItem.Id).Constant;
            if (rarity == 0 && constant is > 0 and < 7)
            {
                rarity = constant;
            }

            Item newItem = new(id, amount, rarity)
            {
                EnchantLevel = dropContent.EnchantLevel
            };
            newItem.Stats = new(newItem);
            items.Add(newItem);
        }

        return items;
    }

    public static bool GiveItemFromSelectBox(GameSession session, Item sourceItem, int index, out OpenBoxResult result)
    {
        result = OpenBoxResult.Success;

        SelectItemBox box = sourceItem.Function.SelectItemBox;
        ItemDropMetadata metadata = ItemDropMetadataStorage.GetItemDropMetadata(box.BoxId);
        if (metadata == null)
        {
            result = OpenBoxResult.UnableToOpen;
            return false;
        }

        IInventory inventory = session.Player.Inventory;

        // Select boxes disregards group ID. Adding these all to a filtered list
        List<DropGroupContent> dropContentsList = new();
        foreach (DropGroup group in metadata.DropGroups)
        {
            foreach (DropGroupContent dropGroupContent in group.Contents)
            {
                if (dropGroupContent.SmartDropRate == 100)
                {
                    List<Job> recommendJobs = ItemMetadataStorage.GetRecommendJobs(dropGroupContent.ItemIds.First());
                    if (recommendJobs.Contains(session.Player.Job) || recommendJobs.Contains(Job.None))
                    {
                        dropContentsList.Add(dropGroupContent);
                    }

                    continue;
                }

                dropContentsList.Add(dropGroupContent);
            }
        }

        DropGroupContent dropContents = dropContentsList[index];
        int amount = Random.Shared.Next((int) dropContents.MinAmount, (int) dropContents.MaxAmount);
        foreach (int id in dropContents.ItemIds)
        {
            if (inventory.CanHold(id, amount))
            {
                continue;
            }

            result = OpenBoxResult.InventoryFull;
            return false;
        }

        inventory.ConsumeItem(session, sourceItem.Uid, 1);

        foreach (int id in dropContents.ItemIds)
        {
            Item newItem = new(id, amount, dropContents.Rarity)
            {
                EnchantLevel = dropContents.EnchantLevel
            };
            newItem.Stats = new(newItem);
            if (inventory.CanHold(newItem))
            {
                inventory.AddItem(session, newItem, true);
                continue;
            }

            result = OpenBoxResult.InventoryFull;
            MailHelper.InventoryWasFull(newItem, session.Player.CharacterId);
        }

        return true;
    }

    public static bool GiveItemFromOpenBox(GameSession session, Item item, out OpenBoxResult boxResult)
    {
        boxResult = OpenBoxResult.Success;

        OpenItemBox box = item.Function.OpenItemBox;
        ItemDropMetadata metadata = ItemDropMetadataStorage.GetItemDropMetadata(box.BoxId);
        if (metadata == null)
        {
            session.Send(NoticePacket.Notice("No items found", NoticeType.Chat));
            boxResult = OpenBoxResult.UnableToOpen;
            return false;
        }

        if (box.AmountRequired > item.Amount)
        {
            boxResult = OpenBoxResult.UnableToOpen;
            return false;
        }

        IInventory inventory = session.Player.Inventory;
        if (box.RequiredItemId > 0)
        {
            Item requiredItem = inventory.GetByUid(box.RequiredItemId);
            if (requiredItem == null)
            {
                boxResult = OpenBoxResult.UnableToOpen;
                return false;
            }

            inventory.ConsumeItem(session, requiredItem.Uid, 1);
        }

        inventory.ConsumeItem(session, item.Uid, box.AmountRequired);

        Random rng = Random.Shared;
        // Receive one item from each drop group
        if (box.ReceiveOneItem)
        {
            foreach (DropGroup group in metadata.DropGroups)
            {
                bool receivedItem = false;

                // Randomize the contents
                IOrderedEnumerable<DropGroupContent> dropContent = group.Contents.OrderBy(_ => rng.Next());
                foreach (DropGroupContent content in dropContent)
                {
                    // If player has already received an item from this group, skip other contents
                    if (box.ReceiveOneItem && receivedItem)
                    {
                        continue;
                    }

                    List<Item> items = GetItemsFromDropGroup(content, session.Player, item);
                    foreach (Item newItem in items)
                    {
                        receivedItem = true;
                        if (inventory.CanHold(newItem))
                        {
                            inventory.AddItem(session, newItem, true);
                            continue;
                        }

                        boxResult = OpenBoxResult.InventoryFull;
                        MailHelper.InventoryWasFull(newItem, session.Player.CharacterId);
                    }
                }
            }

            return true;
        }

        // receive all items from each drop group
        foreach (DropGroup group in metadata.DropGroups)
        {
            foreach (DropGroupContent dropContent in group.Contents)
            {
                List<Item> items = GetItemsFromDropGroup(dropContent, session.Player, item);
                foreach (Item newItem in items)
                {
                    if (inventory.CanHold(newItem))
                    {
                        inventory.AddItem(session, newItem, true);
                        continue;
                    }

                    boxResult = OpenBoxResult.InventoryFull;
                    MailHelper.InventoryWasFull(newItem, session.Player.CharacterId);
                }
            }
        }

        return true;
    }
}
