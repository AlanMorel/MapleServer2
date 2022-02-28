using Maple2Storage.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public interface IInventory
{
    void AddItem(GameSession session, Item item, bool isNew);
    void ConsumeItem(GameSession session, long uid, int amount);
    bool RemoveItem(GameSession session, long uid, out Item item);
    void DropItem(GameSession session, long uid, int amount, bool isBound);
    void MoveItem(GameSession session, long uid, short dstSlot);
    bool HasItem(long uid);
    bool Replace(Item item);
    void SortInventory(GameSession session, InventoryTab tab);
    void LoadInventoryTab(GameSession session, InventoryTab tab);
    void ExpandInventory(GameSession session, InventoryTab tab);
    int GetFreeSlots(InventoryTab tab);
    bool CanHold(Item item, int amount = -1);
    bool CanHold(int itemId, int amount);
    bool SlotTaken(Item item, short slot = -1);
    ICollection<Item> GetItems(InventoryTab tab);
}
