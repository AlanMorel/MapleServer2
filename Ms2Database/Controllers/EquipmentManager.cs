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
        private readonly Ms2DbContext _context = new Ms2DbContext(); //Opens connection to database

        public void EquipItem(long characterId, long uid, string slot, bool isCosmetic = true)
        {
            Item item = _context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item
            Item equippedItem;

            if (isCosmetic) // Handles cosmetic
            {
                equippedItem = _context.Items.Include(table => table.Inventory)
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
                equippedItem = _context.Items.Include(table => table.Inventory)
                                             .Where(column => column.Inventory.CharacterId == characterId)
                                             .FirstOrDefault(column => column.EquipmentSlot == slot.ToUpper());

                if (equippedItem != null)
                {
                    UnequipItem(equippedItem.UniqueId, true);
                }
                item.EquipmentSlot = slot.ToUpper();
            }
            _context.SaveChanges();
        }

        public void UnequipItem(long uid, bool isCosmetic = true)
        {
            Item item = _context.Items.FirstOrDefault(column => column.UniqueId == uid); // Query for item

            if (isCosmetic == true)
            {
                item.CosmeticSlot = null;
            }
            else
            {
                item.EquipmentSlot = null;
            }
            _context.SaveChanges();
        }
    }
}
