using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ItemBoxHelper
{
    public static List<Item> GetItemsFromDropGroup(DropGroupContent dropContent, byte playerGender, Job job)
    {
        List<Item> items = new();
        Random rng = RandomProvider.Get();
        int amount = rng.Next((int) dropContent.MinAmount, (int) dropContent.MaxAmount);
        foreach (int id in dropContent.ItemIds)
        {
            if (dropContent.SmartGender)
            {
                byte itemGender = ItemMetadataStorage.GetGender(id);
                if (itemGender != playerGender || itemGender != 2)
                {
                    continue;
                }
            }

            List<Job> recommendJobs = ItemMetadataStorage.GetRecommendJobs(id);
            if (recommendJobs.Contains(job) || recommendJobs.Contains(Job.None))
            {
                Item newItem = new(id)
                {
                    Enchants = dropContent.EnchantLevel,
                    Amount = amount,
                    Rarity = dropContent.Rarity

                };
                items.Add(newItem);
            }
        }
        return items;
    }

    public static void GiveItemFromSelectBox(GameSession session, Item sourceItem, int index)
    {
        SelectItemBox box = sourceItem.Function.SelectItemBox;
        ItemDropMetadata metadata = ItemDropMetadataStorage.GetItemDropMetadata(box.BoxId);
        if (metadata == null)
        {
            session.Send(NoticePacket.Notice("No items found", NoticeType.Chat));
            return;
        }

        Inventory inventory = session.Player.Inventory;
        inventory.ConsumeItem(session, sourceItem.Uid, 1);

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

        Random rng = RandomProvider.Get();
        int amount = rng.Next((int) dropContents.MinAmount, (int) dropContents.MaxAmount);
        foreach (int id in dropContents.ItemIds)
        {
            Item newItem = new(id)
            {
                Enchants = dropContents.EnchantLevel,
                Amount = amount,
                Rarity = dropContents.Rarity

            };
            inventory.AddItem(session, newItem, true);
        }
    }

    public static void GiveItemFromOpenBox(GameSession session, Item item)
    {
        OpenItemBox box = item.Function.OpenItemBox;
        ItemDropMetadata metadata = ItemDropMetadataStorage.GetItemDropMetadata(box.BoxId);
        if (metadata == null)
        {
            session.Send(NoticePacket.Notice("No items found", NoticeType.Chat));
            return;
        }

        if (box.AmountRequired > item.Amount)
        {
            return;
        }

        Inventory inventory = session.Player.Inventory;
        if (box.RequiredItemId > 0)
        {
            Item requiredItem = inventory.Items[box.RequiredItemId];
            if (requiredItem == null)
            {
                return;
            }

            inventory.ConsumeItem(session, requiredItem.Uid, 1);
        }

        inventory.ConsumeItem(session, item.Uid, box.AmountRequired);

        Random rng = RandomProvider.Get();

        // Receive one item from each drop group
        if (box.ReceiveOneItem)
        {
            foreach (DropGroup group in metadata.DropGroups)
            {
                //randomize the contents
                List<DropGroupContent> contentList = group.Contents.OrderBy(x => rng.Next()).ToList();
                foreach (DropGroupContent dropContent in contentList)
                {
                    List<Item> items = GetItemsFromDropGroup(dropContent, session.Player.Gender, session.Player.Job);
                    foreach (Item newItem in items)
                    {
                        inventory.AddItem(session, newItem, true);
                    }
                }
            }
            return;
        }

        // receive all items from each drop group
        foreach (DropGroup group in metadata.DropGroups)
        {
            foreach (DropGroupContent dropContent in group.Contents)
            {
                List<Item> items = GetItemsFromDropGroup(dropContent, session.Player.Gender, session.Player.Job);
                foreach (Item newItem in items)
                {
                    inventory.AddItem(session, newItem, true);
                }
            }
        }
    }
}
