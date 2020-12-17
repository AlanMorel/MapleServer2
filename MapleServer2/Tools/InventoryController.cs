using System;
using MapleServer2.Types;
using MapleServer2.Packets;
using System.Diagnostics;
using MapleServer2.Servers.Game;

public class InventoryController
{
	public static void Add(GameSession session, Item item)
	{
        if (item.SlotMax > 1)
        {
            foreach (Item i in session.Inventory.Items.Values)
            {
                if (i.Id != item.Id || i.Amount >= i.SlotMax)
                {
                    continue;
                }
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    item.Amount = item.Amount - (i.SlotMax - i.Amount);
                    i.Amount = i.SlotMax;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                }
                else
                {
                    i.Amount = i.Amount + item.Amount;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    return;
                }
            }
        }
        session.Inventory.Add(item);
        session.Send(ItemInventoryPacket.Add(item));
    }
}
