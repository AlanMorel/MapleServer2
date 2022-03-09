using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class TradeInventory
{
    public Player OtherPlayer;
    public Item[] Items;
    public long Mesos;
    public bool IsLocked;

    public TradeInventory(Player otherPlayer)
    {
        Items = new Item[5];
        OtherPlayer = otherPlayer;
    }

    public bool AddItem(GameSession session, Item item)
    {
        int index = Array.FindIndex(Items, 0, Items.Length, x => x == null);
        if (index == -1)
        {
            return false;
        }

        Items[index] = item;
        session.Send(TradePacket.AddItemToTrade(item, index, true));
        OtherPlayer.Session?.Send(TradePacket.AddItemToTrade(item, index, false));

        AlterTrade(session);
        return true;
    }

    public void AlterTrade(GameSession session)
    {
        if (!OtherPlayer.TradeInventory.IsLocked)
        {
            return;
        }

        IsLocked = false;
        OtherPlayer.Session?.Send(TradePacket.OfferedAltered(true));
        session.Send(TradePacket.OfferedAltered(false));
    }

    public void SendItems(Player player, bool isSuccessfulTrade)
    {
        foreach (Item item in Items)
        {
            if (item is null)
            {
                continue;
            }

            if (isSuccessfulTrade)
            {
                item.DecreaseTradeCount();
            }

            player.Inventory.AddItem(player.Session, item, isSuccessfulTrade);
        }
    }

    public bool RemoveItem(GameSession session, long itemUid, int index)
    {
        if (Items[index]?.Uid != itemUid)
        {
            return false;
        }

        Item item = Items[index];
        Items[index] = null;
        AlterTrade(session);
        session.Send(TradePacket.RemoveItemToTrade(item, index, true));
        OtherPlayer.Session?.Send(TradePacket.RemoveItemToTrade(item, index, false));
        session.Player.Inventory.AddItem(session, item, false);
        return true;
    }
}
