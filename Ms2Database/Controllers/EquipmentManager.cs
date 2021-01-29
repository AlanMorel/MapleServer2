using System;
using System.Collections.Generic;
using System.Linq;
using Ms2Database.DbClasses;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Ms2Database.Controllers
{
    public class EquipmentManager
    {
        private readonly Ms2DbContext context = new Ms2DbContext(); //Opens connection to database
        public void EquipItem(long characterId, long uid, string slot, bool isCosmetic = true)
        {
            Item item = context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item
            Item equippedItem;

            if (isCosmetic) // Handles cosmetic
            {
                equippedItem = context.Items.Include(table => table.Inventory)
                                            .Where(column => column.Inventory.CharacterId == characterId)
                                            .FirstOrDefault(column => column.CosmeticSlot == slot.ToUpper());

                if (equippedItem == null || equippedItem.CosmeticSlot == "")
                {
                    item.CosmeticSlot = slot.ToUpper(); // Sets the item slot location
                }
                else // Unequips item occupying slot and sets incoming item slot location
                {
                    UnequipItem(equippedItem.UniqueId, true);
                    item.CosmeticSlot = slot.ToUpper();
                }
            }
            else // Handles regular equipment
            {
                equippedItem = context.Items.Include(table => table.Inventory)
                                            .Where(column => column.Inventory.CharacterId == characterId)
                                            .FirstOrDefault(column => column.EquipmentSlot == slot.ToUpper());

                if (equippedItem == null || equippedItem.EquipmentSlot == "")
                {
                    item.EquipmentSlot = slot.ToUpper();
                }
                else
                {
                    UnequipItem(equippedItem.UniqueId, true);
                    item.EquipmentSlot = slot.ToUpper();
                }
            }
            context.SaveChanges();
        }

        public void UnequipItem(long uid, bool isCosmetic = true)
        {
            Item item = context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item

            if (isCosmetic == true)
            {
                item.CosmeticSlot = null;
            }
            else
            {
                item.EquipmentSlot = null;
            }
            context.SaveChanges();
        }
    }
}
