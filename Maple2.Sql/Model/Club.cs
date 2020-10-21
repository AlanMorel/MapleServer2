using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maple2.Sql.Model {
    public class Club {
        [Key] public long Id { get; set; }
        [ForeignKey("ClubId")] public ICollection<Character> Characters { get; set; }

        [Required] public string Name { get; set; }
    }
}