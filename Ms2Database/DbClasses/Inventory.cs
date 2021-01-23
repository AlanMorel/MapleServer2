using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class Inventory // Inventory table structure
    {
        public Inventory()
        {
            TabGearExtraSlots = 0;
            TabOutfitExtraSlots = 0;
            TabMountExtraSlots = 0;
            TabCatalystExtraSlots = 0;
            TabFishingMusicExtraSlots = 0;
            TabQuestExtraSlots = 0;
            TabGemstoneExtraSlots = 0;
            TabMiscExtraSlots = 0;
            TabLifeSkillExtraSlots = 0;
            TabPetExtraSlots = 0;
            TabConsumableExtraSlots = 0;
            TabCurrencyExtraSlots = 0;
            TabBadgeExtraSlots = 0;
            TabLapenshardExtraSlots = 0;
            TabFragmentExtraSlots = 0;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long InventoryId { get; set; }

        [ForeignKey("Character")]
        [Required]
        public long CharacterId { get; set; }
        public Character Character { get; set; }

        public int TabGearExtraSlots { get; set; }

        public int TabOutfitExtraSlots { get; set; }

        public int TabMountExtraSlots { get; set; }

        public int TabCatalystExtraSlots { get; set; }

        public int TabFishingMusicExtraSlots { get; set; }

        public int TabQuestExtraSlots { get; set; }

        public int TabGemstoneExtraSlots { get; set; }

        public int TabMiscExtraSlots { get; set; }

        public int TabLifeSkillExtraSlots { get; set; }

        public int TabPetExtraSlots { get; set; }

        public int TabConsumableExtraSlots { get; set; }

        public int TabCurrencyExtraSlots { get; set; }

        public int TabBadgeExtraSlots { get; set; }

        public int TabLapenshardExtraSlots { get; set; }

        public int TabFragmentExtraSlots { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
