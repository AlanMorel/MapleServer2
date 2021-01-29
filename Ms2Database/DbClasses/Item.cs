using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class Item // Items table structure
    {
        public Item() // Autofills these attributes currently when items are added, edit InventoryManager.cs if you need more attributes set manually
        {
            Tab = 0;
            InventorySlot = 0;
            Rarity = 1;
            Amount = 1;
            MaxAmount = 1;
            Enchants = 0;
            EnchantExp = 0;
            Charges = 0;
            TimesAttributeChanged = 0;
            IsTemplate = false;
            IsLocked = false;
            UnlockTime = DateTime.Now;
            GlamorForges = 0;
            Repackage = false;
            CreationTime = DateTime.Now;
            ExpirationTime = DateTime.Now.AddDays(1);
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public long UniqueId { get; set; }

        [ForeignKey("Inventory")]
        [Required]
        public long InventoryId { get; set; }
        public Inventory Inventory { get; set; }

        [Required]
        public long Id { get; set; }

        [Required]
        public int Tab { get; set; }

        [Required]
        public int InventorySlot { get; set; }

        public string EquipmentSlot { get; set; }

        public string CosmeticSlot { get; set; }

        [Required]
        public int Rarity { get; set; }

        [Required]
        public long Amount { get; set; }

        public long MaxAmount { get; set; }

        public int Enchants { get; set; }

        public int EnchantExp { get; set; }

        public int Charges { get; set; }

        public int TimesAttributeChanged { get; set; }

        public bool IsTemplate { get; set; }

        public bool IsLocked { get; set; }

        public DateTime? UnlockTime { get; set; }

        public short GlamorForges { get; set; }

        public bool Repackage { get; set; }

        public int? TradeLimit { get; set; }

        public int? TransferFlag { get; set; }

        public long? PairedCharacterId { get; set; }

        public string PairedCharacterName { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? ExpirationTime { get; set; }
    }
}
