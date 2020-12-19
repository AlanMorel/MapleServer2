using System;
using MapleServer2.Types;
using MapleServer2.Packets;
using System.Diagnostics;
using MapleServer2.Servers.Game;

public class InventoryController
{
	public static void Add(GameSession session, Item item)
	{
        // Checks if item is stackable or not.
        if (item.SlotMax > 1)
        {
            foreach (Item i in session.Player.Inventory.Items.Values)
            {
                // Checks to see if item exists in database (dictionary)
                if (i.Id != item.Id || i.Amount >= i.SlotMax)
                {
                    continue;
                }
                // Updates inventory for item amount overflow.
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    item.Amount = item.Amount - (i.SlotMax - i.Amount);
                    i.Amount = i.SlotMax;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                }
                // Updates item amount
                else
                {
                    i.Amount = i.Amount + item.Amount;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    return;
                }
            }
        }
        session.Player.Inventory.Add(item); // adds item into internal database
        session.Send(ItemInventoryPacket.Add(item)); // sends packet to add item clientside.
    }
}
