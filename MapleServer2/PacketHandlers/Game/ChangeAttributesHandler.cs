using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game;

public class ChangeAttributesHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ChangeAttribute;

    private enum ChangeAttributesMode : byte
    {
        ChangeAttributes = 0,
        SelectNewAttributes = 2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ChangeAttributesMode function = (ChangeAttributesMode) packet.ReadByte();

        switch (function)
        {
            case ChangeAttributesMode.ChangeAttributes:
                HandleChangeAttributes(session, packet);
                break;
            case ChangeAttributesMode.SelectNewAttributes:
                HandleSelectNewAttributes(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(function);
                break;
        }
    }

    private static void HandleChangeAttributes(GameSession session, PacketReader packet)
    {
        short lockStatId = -1;
        bool isSpecialStat = false;
        long itemUid = packet.ReadLong();
        packet.Skip(8);
        bool useLock = packet.ReadBool();
        if (useLock)
        {
            isSpecialStat = packet.ReadBool();
            lockStatId = packet.ReadShort();
        }

        IInventory inventory = session.Player.Inventory;
        Item gear = inventory.GetByUid(itemUid);
        // Check if gear exist in inventory
        if (gear is null)
        {
            return;
        }

        ScriptLoader scriptLoader = new("Functions/calcGetItemRemakeIngredient");
        DynValue scriptResults = scriptLoader.Call("calcGetItemRemakeIngredientNew", (int) gear.Type, gear.TimesAttributesChanged, gear.Rarity, gear.Level);

        IReadOnlyCollection<Item> ingredient1 = inventory.GetAllByTag(scriptResults.Tuple[0].String);
        int ingredient1Cost = (int) scriptResults.Tuple[1].Number;
        IReadOnlyCollection<Item> ingredient2 = inventory.GetAllByTag(scriptResults.Tuple[2].String);
        int ingredient2Cost = (int) scriptResults.Tuple[3].Number;
        IReadOnlyCollection<Item> ingredient3 = inventory.GetAllByTag(scriptResults.Tuple[4].String);
        int ingredient3Cost = (int) scriptResults.Tuple[5].Number;

        Console.WriteLine($"Cost: {scriptResults.Tuple[0].String}: {scriptResults.Tuple[1].Number} -- {scriptResults.Tuple[2].String}: {scriptResults.Tuple[3].Number} -- {scriptResults.Tuple[4].String}: {scriptResults.Tuple[5].Number}");

        int ingredient1TotalAmount = ingredient1.Sum(x => x.Amount);
        int ingredient2TotalAmount = ingredient2.Sum(x => x.Amount);
        int ingredient3TotalAmount = ingredient3.Sum(x => x.Amount);

        // Check if player has enough materials
        if (ingredient1TotalAmount < ingredient1Cost || ingredient2TotalAmount < ingredient2Cost || ingredient3TotalAmount < ingredient3Cost)
        {
            return;
        }

        Item scrollLock = null;

        string tag = "";
        if (Item.IsAccessory(gear.ItemSlot))
        {
            tag = "LockItemOptionAccessory";
        }
        else if (Item.IsArmor(gear.ItemSlot))
        {
            tag = "LockItemOptionArmor";
        }
        else if (Item.IsWeapon(gear.ItemSlot))
        {
            tag = "LockItemOptionWeapon";
        }
        else if (gear.IsPet())
        {
            tag = "LockItemOptionPet";
        }

        if (useLock)
        {
            scrollLock = inventory.GetAllByTag(tag)
                .FirstOrDefault(i => i.Rarity == gear.Rarity);
            // Check if scroll lock exist in inventory
            if (scrollLock == null)
            {
                return;
            }
        }

        gear.TimesAttributesChanged++;

        Item newItem = new(gear);

        // Get random stats except stat that is locked
        List<ItemStat> randomList = RandomStats.RollBonusStatsWithStatLocked(newItem, lockStatId, isSpecialStat);

        Dictionary<StatAttribute, ItemStat> newRandoms = new();
        for (int i = 0; i < newItem.Stats.Randoms.Count; i++)
        {
            ItemStat stat = newItem.Stats.Randoms.ElementAt(i).Value;
            // Check if BonusStats[i] is BasicStat and isSpecialStat is false
            // Check if BonusStats[i] is SpecialStat and isSpecialStat is true
            switch (stat)
            {
                case BasicStat when !isSpecialStat:
                case SpecialStat when isSpecialStat:
                    switch (stat)
                    {
                        case SpecialStat ns when ns.ItemAttribute == (StatAttribute) lockStatId:
                        case SpecialStat ss when ss.ItemAttribute == (StatAttribute) lockStatId:
                            newRandoms[stat.ItemAttribute] = stat;
                            continue;
                    }
                    break;
            }

            newRandoms[randomList[i].ItemAttribute] = randomList[i];
        }
        newItem.Stats.Randoms = newRandoms;

        // Consume materials from inventory
        ConsumeMaterials(session, ingredient1Cost, ingredient2Cost, ingredient3Cost, ingredient1, ingredient2, ingredient3);

        if (useLock)
        {
            session.Player.Inventory.ConsumeItem(session, scrollLock.Uid, 1);
        }
        inventory.TemporaryStorage[newItem.Uid] = newItem;

        session.Send(ChangeAttributesPacket.PreviewNewItem(newItem));
    }

    private static void HandleSelectNewAttributes(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        IInventory inventory = session.Player.Inventory;
        Item gear = inventory.TemporaryStorage.FirstOrDefault(x => x.Key == itemUid).Value;
        if (gear == null)
        {
            return;
        }

        inventory.TemporaryStorage.Remove(itemUid);
        inventory.Replace(gear);
        session.Send(ChangeAttributesPacket.AddNewItem(gear));
    }

    private static void ConsumeMaterials(GameSession session, int ingredient1Cost, int ingredient2Cost, int ingredient3Cost, IEnumerable<Item> ingredient1, IEnumerable<Item> ingredient2, IEnumerable<Item> ingredient3)
    {
        IInventory inventory = session.Player.Inventory;
        foreach (Item item in ingredient1)
        {
            if (item.Amount >= ingredient1Cost)
            {
                inventory.ConsumeItem(session, item.Uid, ingredient1Cost);
                break;
            }

            ingredient1Cost -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }

        foreach (Item item in ingredient2)
        {
            if (item.Amount >= ingredient2Cost)
            {
                inventory.ConsumeItem(session, item.Uid, ingredient2Cost);
                break;
            }

            ingredient2Cost -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }

        foreach (Item item in ingredient3)
        {
            if (item.Amount >= ingredient3Cost)
            {
                inventory.ConsumeItem(session, item.Uid, ingredient3Cost);
                break;
            }

            ingredient3Cost -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }
    }
}
