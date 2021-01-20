using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class InventoryManager
    {
        public void CreateInventory(long CharId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Inventory inventory = new Inventory()
                {
                    CharacterId = CharId
                };
                context.Add(inventory);
                context.SaveChanges();
            }
        }

        public void AddItem(long characterId, long itemId, long amount, int tab, int slot, int rarity)
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

        public void DeleteItem(long Uid)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = context.Items.Find(Uid);

                context.Remove(item);
                context.SaveChanges();
            }
        }

        public Item findItem(long Uid)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = context.Items.Find(Uid);

                return item;
            }
        }

        public void EditItem(Item item)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item OriginItem = item;
                context.SaveChanges();
            }
        }
    }
}
