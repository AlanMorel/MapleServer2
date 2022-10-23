using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Database;
using MapleServer2.Packets.Helpers;
using MySqlX.XDevAPI.CRUD;

namespace MapleServer2.Types;

public class BuyBackItem
{
    public long AddedTimestamp;
    public long Price;
    public Item Item;

    public BuyBackItem(Item item, long price)
    {
        AddedTimestamp = TimeInfo.Now();
        Price = price;
        Item = item;
    }

    public static void Remove(BuyBackItem?[] buyBackItems, BuyBackItem buyBackItem)
    {
        int index = Array.IndexOf(buyBackItems, buyBackItem);
        buyBackItems[index] = null;
    }
}
