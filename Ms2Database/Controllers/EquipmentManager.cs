using System.Linq;
using Microsoft.EntityFrameworkCore;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class EquipmentManager
    {
        private readonly Ms2DbContext Context = new Ms2DbContext(); //Opens connection to database

        public void EquipItem(long characterId, long uid, string slot, bool isCosmetic = true)
        {
            Item item = Context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item
            Item equippedItem;

            if (isCosmetic) // Handles cosmetic
            {
                equippedItem = Context.Items.Include(table => table.Inventory)
                                             .Where(column => column.Inventory.CharacterId == characterId)
                                             .FirstOrDefault(column => column.CosmeticSlot == slot.ToUpper());

                if (equippedItem == null || equippedItem.CosmeticSlot == "")
                {
                    UnequipItem(equippedItem.UniqueId, true); // Unequips item occupying slot
                }
                item.CosmeticSlot = slot.ToUpper(); // Sets the item slot location
            }
            else // Handles regular equipment
            {
                equippedItem = Context.Items.Include(table => table.Inventory)
                                             .Where(column => column.Inventory.CharacterId == characterId)
                                             .FirstOrDefault(column => column.EquipmentSlot == slot.ToUpper());

                if (equippedItem != null)
                {
                    UnequipItem(equippedItem.UniqueId, true);
                }
                item.EquipmentSlot = slot.ToUpper();
            }
            Context.SaveChanges();
        }

        public void UnequipItem(long uid, bool isCosmetic = true)
        {
            Item item = Context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item

            if (isCosmetic == true)
            {
                item.CosmeticSlot = null;
            }
            else
            {
                item.EquipmentSlot = null;
            }
            Context.SaveChanges();
        }
    }
}
