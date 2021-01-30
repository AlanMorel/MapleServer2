using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms2Database.DbClasses
{
    public class SkillTree
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public long UniqueId { get; set; }

        [ForeignKey("Character")]
        [Required]
        public long CharacterId { get; set; }
        public Character Character { get; set; }

        [Required]
        public long SkillId { get; set; }

        public string SkillName { get; set; }
        public int Level { get; set; }

        [Required]
        public bool Learned { get; set; }
    }
}
