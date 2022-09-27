using Maple2Storage.Enums;
using MapleServer2.Servers.Game;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public interface IInventory
{
    Dictionary<InventoryTab, short> ExtraSize { get; }
    long Id { get; }
    Dictionary<ItemSlot, Item> Equips { get; }
    Dictionary<ItemSlot, Item> Cosmetics { get; }
    Item[] Badges { get; }
    Item[] LapenshardStorage { get; }
    List<SetBonus> SetBonuses { get; }
    Dictionary<long, Item> TemporaryStorage { get; }
    void RecomputeSetBonuses(GameSession session);
    void ItemEquipped(GameSession session, Item item);
    void ItemUnequipped(GameSession session, Item item);
    void AddItem(GameSession session, Item item, bool isNew);
    void ConsumeItem(GameSession session, long uid, int amount);
    void ConsumeByTag(GameSession session, string tag, int amount, int rarity = 0);
    void ConsumeById(GameSession session, int id, int amount, int rarity = 0);
    bool RemoveItem(GameSession session, long uid, out Item item);
    void DropItem(GameSession session, Item item, int amount);
    void MoveItem(GameSession session, long uid, short dstSlot);

    /// <summary>
    /// Determines whether the inventory contains an item with given Unique ID (UID)
    /// </summary>
    /// <param name="uid">The UID of the item</param>
    bool HasItem(long uid);

    /// <summary>
    /// Determines whether the inventory contains an item with given Item ID
    /// </summary>
    /// <param name="id">The Item ID of the item</param>
    bool HasItem(int id);

    /// <summary>
    /// Gets the first item matching the given Unique ID (UID)
    /// </summary>
    /// <param name="uid">The UID of the item</param>
    /// <remarks>Can return null</remarks>
    Item? GetByUid(long uid);

    /// <summary>
    /// Gets the first item matching the given Item ID
    /// </summary>
    /// <param name="id">The Item ID of the item</param>
    /// <remarks>Can return null</remarks>
    Item? GetById(int id);

    /// <summary>
    /// Gets all non-null items in the inventory
    /// </summary>
    IEnumerable<Item> GetItemsNotNull();

    /// <summary>
    /// Gets all items matching the given Item ID
    /// </summary>
    /// <param name="id">The Item ID of the item</param>
    /// <remarks>Never returns null, can return empty</remarks>
    IReadOnlyCollection<Item> GetAllById(int id);

    /// <summary>
    /// Gets all items matching the given tag
    /// </summary>
    /// <param name="tag">The tag of the item</param>
    /// <remarks>Never returns null, can return empty</remarks>
    IReadOnlyCollection<Item> GetAllByTag(string tag);

    /// <summary>
    /// Gets all items matching the given Function ID
    /// </summary>
    /// <param name="functionId">The Function ID of the item</param>
    /// <remarks>Never returns null, can return empty</remarks>
    IEnumerable<Item> GetAllByFunctionId(int functionId);
    Item? GetFromInventoryOrEquipped(long uid);
    bool ItemIsEquipped(long uid);
    bool TryEquip(GameSession session, long uid, ItemSlot slot);
    bool TryUnequip(GameSession session, long uid);
    Item? GetEquippedItem(long uid);
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
