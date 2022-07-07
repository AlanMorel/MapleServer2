using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class LapenshardHandler : GamePacketHandler<LapenshardHandler>
{
    public override RecvOp OpCode => RecvOp.ItemLapenshard;

    private enum Mode : byte
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
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Equip:
                HandleEquip(session, packet);
                break;
            case Mode.Unequip:
                HandleUnequip(session, packet);
                break;
            case Mode.AddFusion:
                HandleAddFusion(session, packet);
                break;
            case Mode.AddCatalyst:
                HandleAddCatalyst(session, packet);
                break;
            case Mode.Fusion:
                HandleFusion(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    public static void AddEffects(Player player, Item lapenshard)
    {
        if (lapenshard.AdditionalEffects.Id == null)
        {
            return;
        }

        player.AddEffects(lapenshard.AdditionalEffects);
    }

    public static void RemoveEffects(Player player, Item lapenshard)
    {
        if (lapenshard.AdditionalEffects.Id == null)
        {
            return;
        }

        player.RemoveEffects(lapenshard.AdditionalEffects);
    }

    private static void HandleEquip(GameSession session, PacketReader packet)
    {
        int slotId = packet.ReadInt();
        long itemUid = packet.ReadLong();

        Item item = session.Player.Inventory.GetByUid(itemUid);
        if (item is null)
        {
            return;
        }

        if (item.Type != ItemType.Lapenshard)
        {
            return;
        }

        if (session.Player.Inventory.LapenshardStorage[slotId - 1] is not null)
        {
            return;
        }

        Item newLapenshard = new(item)
        {
            Amount = 1,
            IsEquipped = true,
            Slot = (short) slotId
        };

        AddEffects(session.Player, newLapenshard);

        newLapenshard.Uid = DatabaseManager.Items.Insert(newLapenshard);
        session.Player.Inventory.LapenshardStorage[slotId - 1] = newLapenshard;
        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
        session.Send(LapenshardPacket.Equip(slotId, item.Id));
    }

    private static void HandleUnequip(GameSession session, PacketReader packet)
    {
        int slotId = packet.ReadInt();

        Item lapenshard = session.Player.Inventory.LapenshardStorage[slotId - 1];
        if (lapenshard is null)
        {
            return;
        }

        RemoveEffects(session.Player, lapenshard);

        session.Player.Inventory.LapenshardStorage[slotId - 1] = null;
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
        IInventory inventory = session.Player.Inventory;

        if (!inventory.HasItem(itemUid))
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
        IInventory inventory = session.Player.Inventory;

        Item item = inventory.GetByUid(itemUid);
        if (item == null || item.Amount < amount)
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
        IInventory inventory = session.Player.Inventory;

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
            Item item = inventory.GetByUid(uid);
            if (item == null || item.Amount < amount)
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


        // There are multiple ids for each type of material
        // Count all items with the same tag in inventory
        IReadOnlyCollection<Item> crystals = inventory.GetAllByTag(crystal).ToList();
        int crystalsTotalAmount = crystals.Sum(x => x.Amount);

        byte tier = (byte) (itemId % 10);

        if (costs[tier].CrystalsAmount > crystalsTotalAmount || !session.Player.Wallet.Meso.Modify(-costs[tier].Mesos))
        {
            return;
        }

        int crystalCost = costs[tier].CrystalsAmount;

        // Consume all Crystals
        foreach (Item item in crystals)
        {
            if (item.Amount >= crystalCost)
            {
                session.Player.Inventory.ConsumeItem(session, item.Uid, crystalCost);
                break;
            }
            crystalCost -= item.Amount;
            session.Player.Inventory.ConsumeItem(session, item.Uid, item.Amount);
        }

        // Consume all Lapenshards
        foreach ((long uid, int amount) in items)
        {
            session.Player.Inventory.ConsumeItem(session, uid, amount);
        }

        session.Player.Inventory.ConsumeItem(session, itemUid, 1);
        session.Player.Inventory.AddItem(session, new(itemId + 1, rarity: 3), true);
        session.Send(LapenshardPacket.Upgrade(itemId, true));
    }
}
