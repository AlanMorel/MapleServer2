using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class InventoryManager
    {
        public static void CreateInventory(long characterId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Inventory inventory = new Inventory()
                {
                    CharacterId = characterId
                };
                context.Add(inventory);
                context.SaveChanges();
            }
        }

        public static void AddItem(long characterId, long itemId, long amount, int tab, int slot, int rarity)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Inventory inventory = context.Inventories.Find(characterId);
                Item item = new Item()
                {
                    InventoryId = inventory.InventoryId,
                    Id = itemId,
                    Amount = amount,
                    Tab = tab,
                    Slot = slot,
                    Rarity = rarity
                };
                context.Add(item);
                context.SaveChanges();
            }
        }

        public static void DeleteItem(long uid)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = context.Items.Find(uid);

                context.Remove(item);
                context.SaveChanges();
            }
        }

        public static Item FindItem(long uid)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = context.Items.Find(uid);

                return item;
            }
        }

        public static void EditItem(/*Item item*/)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                context.SaveChanges();
            }
        }
    }
}
