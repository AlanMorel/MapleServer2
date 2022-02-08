using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class LapenshardHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.ITEM_LAPENSHARD;

    private enum LapenshardMode : byte
    {
        Equip = 0x1,
        Unequip = 0x2,
        AddFusion = 0x3,
        AddCatalyst = 0x4,
        Fusion = 0x5
    }

    private enum LapenshardColor : byte
    {
        Red = 41,
        Blue = 42,
        Green = 43
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        LapenshardMode mode = (LapenshardMode) packet.ReadByte();
        switch (mode)
        {
            case LapenshardMode.Equip:
                HandleEquip(session, packet);
                break;
            case LapenshardMode.Unequip:
                HandleUnequip(session, packet);
                break;
            case LapenshardMode.AddFusion:
                HandleAddFusion(session, packet);
                break;
            case LapenshardMode.AddCatalyst:
                HandleAddCatalyst(session, packet);
                break;
            case LapenshardMode.Fusion:
                HandleFusion(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleEquip(GameSession session, PacketReader packet)
    {
        int slotId = packet.ReadInt();
        long itemUid = packet.ReadLong();

        session.Player.Inventory.Items.TryGetValue(itemUid, out Item item);
        if (item is null)
        {
            return;
        }

        if (item.Type != ItemType.Lapenshard)
        {
            return;
        }

        if (session.Player.Inventory.LapenshardStorage[slotId] is not null)
        {
            return;
        }

        Item newLapenshard = new(item)
        {
            Amount = 1,
            IsEquipped = true,
            Slot = (short) slotId
        };

        newLapenshard.Uid = DatabaseManager.Items.Insert(newLapenshard);

        session.Player.Inventory.LapenshardStorage[slotId] = newLapenshard;
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        session.Send(LapenshardPacket.Equip(slotId, item.Id));
    }

    private static void HandleUnequip(GameSession session, PacketReader packet)
    {
        int slotId = packet.ReadInt();

        Item lapenshard = session.Player.Inventory.LapenshardStorage[slotId];
        if (lapenshard is null)
        {
            return;
        }

        session.Player.Inventory.LapenshardStorage[slotId] = null;
        lapenshard.Slot = -1;
        lapenshard.IsEquipped = false;
        session.Player.Inventory.AddItem(session, lapenshard, true);
        session.Send(LapenshardPacket.Unequip(slotId));
    }

    private static void HandleAddFusion(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int itemId = packet.ReadInt();
        packet.ReadInt();
        Inventory inventory = session.Player.Inventory;

        if (!inventory.Items.TryGetValue(itemUid, out Item item))
        {
            return;
        }
        // GMS2 Always 100% success rate
        session.Send(LapenshardPacket.Select(10000));
    }

    private static void HandleAddCatalyst(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int itemId = packet.ReadInt();
        packet.ReadInt();
        int amount = packet.ReadInt();
        Inventory inventory = session.Player.Inventory;

        if (!inventory.Items.TryGetValue(itemUid, out Item item) || item.Amount < amount)
        {
            return;
        }

        // GMS2 Always 100% success rate
        session.Send(LapenshardPacket.Select(10000));
    }

    private static void HandleFusion(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int itemId = packet.ReadInt();
        packet.ReadInt();
        int catalystCount = packet.ReadInt();
        Inventory inventory = session.Player.Inventory;

        List<(long Uid, int Amount)> items = new();
        for (int i = 0; i < catalystCount; i++)
        {
            long uid = packet.ReadLong();
            int amount = packet.ReadInt();
            (long, int) item = new(uid, amount);
            items.Add(item);
        }

        // Check if items are in inventory
        foreach ((long uid, int amount) in items)
        {
            if (!inventory.Items.TryGetValue(uid, out Item item) || item.Amount < amount)
            {
                return;
            }
        }

        LapenshardColor itemType = (LapenshardColor) (itemId / 1000000);
        string crystal = "";
        switch (itemType)
        {
            case LapenshardColor.Red:
                crystal = "RedCrystal";
                break;
            case LapenshardColor.Blue:
                crystal = "BlueCrystal";
                break;
            case LapenshardColor.Green:
                crystal = "GreenCrystal";
                break;
        }

        //Tier, Copies, Crystals, Mesos
        Dictionary<byte, (byte Copies, short CrystalsAmount, int Mesos)> costs = new()
        {
            { 1, new(4, 34, 600000) },
            { 2, new(5, 41, 800000) },
            { 3, new(6, 51, 1000000) },
            { 4, new(7, 63, 1200000) },
            { 5, new(8, 78, 1500000) },
            { 6, new(14, 102, 2000000) },
            { 7, new(20, 135, 2700000) },
            { 8, new(30, 190, 3800000) },
            { 9, new(50, 305, 6100000) }
        };

        int crystalsTotalAmount = 0;

        // There are multiple ids for each type of material
        // Count all items with the same tag in inventory
        List<KeyValuePair<long, Item>> crystals = inventory.Items.Where(x => x.Value.Tag == crystal).ToList();
        crystals.ForEach(x => crystalsTotalAmount += x.Value.Amount);
        byte tier = (byte) (itemId % 10);

        if (costs[tier].CrystalsAmount > crystalsTotalAmount || !session.Player.Wallet.Meso.Modify(-costs[tier].Mesos))
        {
            return;
        }

        int crystalCost = costs[tier].CrystalsAmount;

        // Consume all Crystals
        foreach ((long uid, Item item) in crystals)
        {
            if (item.Amount >= crystalCost)
            {
                session.Player.Inventory.ConsumeItem(session, uid, crystalCost);
                break;
            }
            crystalCost -= item.Amount;
            session.Player.Inventory.ConsumeItem(session, uid, item.Amount);
        }

        // Consume all Lapenshards
        foreach ((long uid, int amount) in items)
        {
            session.Player.Inventory.ConsumeItem(session, uid, amount);
        }

        session.Player.Inventory.ConsumeItem(session, itemUid, 1);
        session.Player.Inventory.AddItem(session, new(itemId + 1) { Rarity = 3 }, true);
        session.Send(LapenshardPacket.Upgrade(itemId, true));
    }
}
