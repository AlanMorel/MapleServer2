using Maple2Storage.Enums;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public interface IInventory
{
    Dictionary<InventoryTab, short> ExtraSize { get; }
    long Id { get; }
    Dictionary<ItemSlot, Item> Equips { get; }
    Dictionary<ItemSlot, Item> Cosmetics { get; }
    Item[] Badges { get; }
    Item[] LapenshardStorage { get; }
    Dictionary<long, Item> TemporaryStorage { get; }
    void AddItem(GameSession session, Item item, bool isNew);
    void ConsumeItem(GameSession session, long uid, int amount);
    bool RemoveItem(GameSession session, long uid, out Item item);
    void DropItem(GameSession session, long uid, int amount, bool isBound);
    void MoveItem(GameSession session, long uid, short dstSlot);
    bool HasItem(long uid);
    bool HasItem(int id);
    Item GetByUid(long uid);
    Item GetById(int id);
    IReadOnlyCollection<Item> GetItemsNotNull();
    IReadOnlyCollection<Item> GetAllById(int id);
    IReadOnlyCollection<Item> GetAllByTag(string tag);
    IReadOnlyCollection<Item> GetAllByFunctionId(int functionId);
    bool Replace(Item item);
    void SortInventory(GameSession session, InventoryTab tab);
    void LoadInventoryTab(GameSession session, InventoryTab tab);
    void ExpandInventory(GameSession session, InventoryTab tab);
    int GetFreeSlots(InventoryTab tab);
    bool CanHold(Item item, int amount = -1);
    bool CanHold(int itemId, int amount);
    bool SlotTaken(Item item, short slot = -1);
    IReadOnlyCollection<Item> GetItems(InventoryTab tab);
}
