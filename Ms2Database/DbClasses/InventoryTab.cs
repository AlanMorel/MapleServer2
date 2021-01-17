using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class InventoryTab
    {
        public InventoryTab()
        {
            ExtraSlotAmount = 0;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int TabId { get; set; }

        [ForeignKey("Character")]
        [Required]
        public long CharacterId { get; set; }
        public Character Character { get; set; }

        public int ExtraSlotAmount { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
