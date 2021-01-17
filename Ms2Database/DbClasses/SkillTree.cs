using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class SkillTree
    {
        public SkillTree()
        {
            SkillName = "";
            Level = 0;
            Learned = false;
        }

        [ForeignKey("Character")]
        [Required]
        public long CharacterId { get; set; }
        public Character Character { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Key]
        public long SkillId { get; set; }

        public string SkillName { get; set; }
        public int Level { get; set; }

        [Required]
        public bool Learned { get; set; }
    }
}
