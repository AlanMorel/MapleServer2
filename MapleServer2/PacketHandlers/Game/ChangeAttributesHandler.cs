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


        // There are multiple ids for each type of material
        IReadOnlyCollection<Item> greenCrystals = inventory.GetAllByTag("GreenCrystal");
        IReadOnlyCollection<Item> metacells = inventory.GetAllByTag("MetaCell");
        IReadOnlyCollection<Item> crystalFragments = inventory.GetAllByTag("CrystalPiece");

        int greenCrystalTotalAmount = greenCrystals.Sum(x => x.Amount);
        int metacellTotalAmount = metacells.Sum(x => x.Amount);
        int crystalFragmentTotalAmount = crystalFragments.Sum(x => x.Amount);

        Item gear = inventory.GetByUid(itemUid);
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

        int greenCrystalCost = 5;
        int metacellCosts = Math.Min(11 + gear.TimesAttributesChanged, 25);

        // Relation between TimesAttributesChanged to amount of crystalFragments for epic gear
        int[] crystalFragmentsEpicGear =
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
        List<ItemStat> randomList = RandomStats.RollBonusStatsWithStatLocked(newItem, lockStatId, isSpecialStat);

        for (int i = 0; i < newItem.Stats.Randoms.Count; i++)
        {
            // Check if BonusStats[i] is BasicStat and isSpecialStat is false
            // Check if BonusStats[i] is SpecialStat and isSpecialStat is true
            switch (newItem.Stats.Randoms[i])
            {
                case BasicStat when !isSpecialStat:
                case SpecialStat when isSpecialStat:
                    ItemStat stat = newItem.Stats.Randoms[i];
                    switch (stat)
                    {
                        case SpecialStat ns when ns.ItemAttribute == (StatAttribute) lockStatId:
                        case SpecialStat ss when ss.ItemAttribute == (StatAttribute) lockStatId:
                            continue;
                    }
                    break;
            }

            newItem.Stats.Randoms[i] = randomList[i];
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

    private static void ConsumeMaterials(GameSession session, int greenCrystalCost, int metacellCosts, int crystalFragmentsCosts, IEnumerable<Item> greenCrystals, IEnumerable<Item> metacells, IEnumerable<Item> crystalFragments)
    {
        IInventory inventory = session.Player.Inventory;
        foreach (Item item in greenCrystals)
        {
            if (item.Amount >= greenCrystalCost)
            {
                inventory.ConsumeItem(session, item.Uid, greenCrystalCost);
                break;
            }

            greenCrystalCost -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }

        foreach (Item item in metacells)
        {
            if (item.Amount >= metacellCosts)
            {
                inventory.ConsumeItem(session, item.Uid, metacellCosts);
                break;
            }

            metacellCosts -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }

        foreach (Item item in crystalFragments)
        {
            if (item.Amount >= crystalFragmentsCosts)
            {
                inventory.ConsumeItem(session, item.Uid, crystalFragmentsCosts);
                break;
            }

            crystalFragmentsCosts -= item.Amount;
            inventory.ConsumeItem(session, item.Uid, item.Amount);
        }
    }
}
