using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class ItemUseMultipleHandler : GamePacketHandler<ItemUseMultipleHandler>
{
    public override RecvOp OpCode => RecvOp.RequestItemUseMultiple;

    private enum BoxType : byte
    {
        Open = 0x00,
        Select = 0x01
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        int itemId = packet.ReadInt();
        packet.ReadShort(); // Unknown
        int amount = packet.ReadInt();
        BoxType boxType = (BoxType) packet.ReadShort();

        string functionName = ItemMetadataStorage.GetFunctionMetadata(itemId).Name;
        if (functionName != "SelectItemBox" && functionName != "OpenItemBox")
        {
            return;
        }

        IReadOnlyCollection<Item> items = session.Player.Inventory.GetAllById(itemId); // Make copy of items in-case new item is added
        if (items.Count == 0)
        {
            return;
        }

        if (boxType == BoxType.Select)
        {
            int index = packet.ReadShort() - 0x30; // Starts at 0x30 for some reason
            if (index < 0)
            {
                return;
            }

            HandleSelectBox(session, items, index, amount);
            return;
        }

        HandleOpenBox(session, items, amount);
    }

    private static void HandleSelectBox(GameSession session, IReadOnlyCollection<Item> items, int index, int amount)
    {
        int opened = 0;
        OpenBoxResult result = OpenBoxResult.Success;
        foreach (Item item in items)
        {
            for (int i = opened; i < amount; i++)
            {
                if (item.Amount <= 0)
                {
                    break;
                }

                if (!ItemBoxHelper.GiveItemFromSelectBox(session, item, index, out result))
                {
                    break;
                }

                opened++;
            }
        }

        session.Send(ItemUsePacket.Use(items.First().Id, opened, result));
    }

    private static void HandleOpenBox(GameSession session, IReadOnlyCollection<Item> items, int amount)
    {
        int opened = 0;
        OpenBoxResult result = OpenBoxResult.Success;
        foreach (Item item in items)
        {
            for (int i = opened; i < amount; i++)
            {
                if (item.Amount <= 0)
                {
                    break;
                }

                if (!ItemBoxHelper.GiveItemFromOpenBox(session, item, out result))
                {
                    break;
                }

                opened++;
            }
        }

        session.Send(ItemUsePacket.Use(items.First().Id, opened, result));
    }
}

public enum OpenBoxResult : short
{
    Success = 2,
    UnableToOpen = 3,
    InventoryFull = 4,
}
