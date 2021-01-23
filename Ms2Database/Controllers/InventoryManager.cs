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
        public void CreateInventory(long characterId)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Inventory Inventory = new Inventory()
                {
                    CharacterId = characterId
                };
                Context.Add(Inventory);
                Context.SaveChanges();
            }
        }

        public void AddItem(long characterId, long itemId, long amount, int tab, int slot, int rarity)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Inventory Inventory = Context.Inventories.Find(characterId);
                Item Item = new Item()
                {
                    InventoryId = Inventory.InventoryId,
                    Id = itemId,
                    Amount = amount,
                    Tab = tab,
                    Slot = slot,
                    Rarity = rarity
                };
                Context.Add(Item);
                Context.SaveChanges();
            }
        }

        public void DeleteItem(long uid)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Item Item = Context.Items.Find(uid);

                Context.Remove(Item);
                Context.SaveChanges();
            }
        }

        public Item findItem(long uid)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Item Item = Context.Items.Find(uid);

                return Item;
            }
        }

        public void EditItem(Item item)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Item Item = item;
                Context.SaveChanges();
            }
        }
    }
}
