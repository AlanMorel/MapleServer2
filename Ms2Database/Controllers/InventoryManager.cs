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

        public void AddItem(long inventoryId, long itemId, long amount, int tab, int slot, int rarity)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = new Item()
                {
                    InventoryId = inventoryId,
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

        public void DeleteItem(long inventoryId, long itemId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Item item = context.Items.Where(i => i.InventoryId == inventoryId)
                                         .FirstOrDefault(i => i.Id == itemId);

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
    }
}
