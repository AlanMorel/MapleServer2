using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ChangeAttributesHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.CHANGE_ATTRIBUTES;

    public ChangeAttributesHandler() : base() { }

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

        Inventory inventory = session.Player.Inventory;

        int greenCrystalTotalAmount = 0;
        int metacellTotalAmount = 0;
        int crystalFragmentTotalAmount = 0;

        // There are multiple ids for each type of material
        List<KeyValuePair<long, Item>> greenCrystals = inventory.Items.Where(x => x.Value.Tag == "GreenCrystal").ToList();
        greenCrystals.ForEach(x => greenCrystalTotalAmount += x.Value.Amount);

        List<KeyValuePair<long, Item>> metacells = inventory.Items.Where(x => x.Value.Tag == "MetaCell").ToList();
        metacells.ForEach(x => metacellTotalAmount += x.Value.Amount);

        List<KeyValuePair<long, Item>> crystalFragments = inventory.Items.Where(x => x.Value.Tag == "CrystalPiece").ToList();
        crystalFragments.ForEach(x => crystalFragmentTotalAmount += x.Value.Amount);

        Item gear = inventory.Items.FirstOrDefault(x => x.Key == itemUid).Value;
        Item scrollLock = null;

        // Check if gear exist in inventory
        if (gear == null)
        {
            return;
        }

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
        else if (Item.IsPet(gear.Id))
        {
            tag = "LockItemOptionPet";
        }

        if (useLock)
        {
            scrollLock = inventory.Items.FirstOrDefault(x => x.Value.Tag == tag && x.Value.Rarity == gear.Rarity).Value;
            // Check if scroll lock exist in inventory
            if (scrollLock == null)
            {
                return;
            }
        }

        int greenCrystalCost = 5;
        int metacellCosts = Math.Min(11 + gear.TimesAttributesChanged, 25);

        // Relation between TimesAttributesChanged to amount of crystalFragments for epic gear
        int[] crystalFragmentsEpicGear = new int[]
        {
            200, 250, 312, 390, 488, 610, 762, 953, 1192, 1490, 1718, 2131, 2642, 3277, 4063
        };

        int crystalFragmentsCosts = crystalFragmentsEpicGear[Math.Min(gear.TimesAttributesChanged, 14)];

        if (gear.Rarity > (short) RarityType.Epic)
        {
            greenCrystalCost = 25;
            metacellCosts = Math.Min(165 + gear.TimesAttributesChanged * 15, 375);
            if (gear.Rarity == (short) RarityType.Legendary)
            {
                crystalFragmentsCosts = Math.Min(400 + gear.TimesAttributesChanged * 400, 6000);
            }
            else if (gear.Rarity == (short) RarityType.Ascendant)
            {
                crystalFragmentsCosts = Math.Min(600 + gear.TimesAttributesChanged * 600, 9000);
            }
        }

        // Check if player has enough materials
        if (greenCrystalTotalAmount < greenCrystalCost || metacellTotalAmount < metacellCosts || crystalFragmentTotalAmount < crystalFragmentsCosts)
        {
            return;
        }

        gear.TimesAttributesChanged++;

        Item newItem = new(gear);

        // Get random stats except stat that is locked
        List<ItemStat> randomList = ItemStats.RollBonusStatsWithStatLocked(newItem, lockStatId, isSpecialStat);

        for (int i = 0; i < newItem.Stats.BonusStats.Count; i++)
        {
            // Check if BonusStats[i] is NormalStat and isSpecialStat is false
            // Check if BonusStats[i] is SpecialStat and isSpecialStat is true
            switch (newItem.Stats.BonusStats[i])
            {
                case NormalStat when !isSpecialStat:
                case SpecialStat when isSpecialStat:
                    ItemStat stat = newItem.Stats.BonusStats[i];
                    switch (stat)
                    {
                        case NormalStat ns when ns.ItemAttribute == (StatId) lockStatId:
                        case SpecialStat ss when ss.ItemAttribute == (SpecialStatId) lockStatId:
                            continue;
                    }
                    break;
            }

            newItem.Stats.BonusStats[i] = randomList[i];
        }

        // Consume materials from inventory
        ConsumeMaterials(session, greenCrystalCost, metacellCosts, crystalFragmentsCosts, greenCrystals, metacells, crystalFragments);

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

        Inventory inventory = session.Player.Inventory;
        Item gear = inventory.TemporaryStorage.FirstOrDefault(x => x.Key == itemUid).Value;
        if (gear == null)
        {
            return;
        }

        inventory.TemporaryStorage.Remove(itemUid);
        inventory.Replace(gear);
        session.Send(ChangeAttributesPacket.AddNewItem(gear));
    }

    private static void ConsumeMaterials(GameSession session, int greenCrystalCost, int metacellCosts, int crystalFragmentsCosts, List<KeyValuePair<long, Item>> greenCrystals, List<KeyValuePair<long, Item>> metacells, List<KeyValuePair<long, Item>> crystalFragments)
    {
        Inventory inventory = session.Player.Inventory;
        foreach (KeyValuePair<long, Item> item in greenCrystals)
        {
            if (item.Value.Amount >= greenCrystalCost)
            {
                inventory.ConsumeItem(session, item.Key, greenCrystalCost);
                break;
            }

            greenCrystalCost -= item.Value.Amount;
            inventory.ConsumeItem(session, item.Key, item.Value.Amount);
        }

        foreach (KeyValuePair<long, Item> item in metacells)
        {
            if (item.Value.Amount >= metacellCosts)
            {
                inventory.ConsumeItem(session, item.Key, metacellCosts);
                break;
            }

            metacellCosts -= item.Value.Amount;
            inventory.ConsumeItem(session, item.Key, item.Value.Amount);
        }

        foreach (KeyValuePair<long, Item> item in crystalFragments)
        {
            if (item.Value.Amount >= crystalFragmentsCosts)
            {
                inventory.ConsumeItem(session, item.Key, crystalFragmentsCosts);
                break;
            }

            crystalFragmentsCosts -= item.Value.Amount;
            inventory.ConsumeItem(session, item.Key, item.Value.Amount);
        }
    }
}
