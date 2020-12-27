using MapleServer2.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

public class InventoryController
{
	public static void Add(GameSession session, Item item)
	{
        // Checks if item is stackable or not
        if (item.SlotMax > 1)
        {
            foreach (Item i in session.Player.Inventory.Items.Values)
            {
                // Continue if inventory item id doesn't match or it is at max amount
                if (i.Id != item.Id || i.Amount >= i.SlotMax)
                {
                    continue;
                }
                // Updates inventory for item amount overflow
                if ((i.Amount + item.Amount) > i.SlotMax)
                {
                    int added = i.SlotMax - i.Amount; // For marking item new with correct added amount

                    item.Amount -= added;
                    i.Amount = i.SlotMax;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    session.Send(ItemInventoryPacket.MarkItemNew(i, added));
                }
                // Updates item amount
                else
                {
                    i.Amount += item.Amount;
                    session.Send(ItemInventoryPacket.Update(i.Uid, i.Amount));
                    session.Send(ItemInventoryPacket.MarkItemNew(i, item.Amount));
                    return;
                }
            }
        }
        // Add item to inventory if cannot stack any further
        session.Player.Inventory.Add(item); 
        session.Send(ItemInventoryPacket.Add(item));
        session.Send(ItemInventoryPacket.MarkItemNew(item, item.Amount));
    }
}