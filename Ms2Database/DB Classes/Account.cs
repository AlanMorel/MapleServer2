using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DB_Classes
{
    public class Account
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public long AccountId { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(10)]
        public string Username { get; set; }
        [Column(TypeName = "nvarchar")]
        [StringLength(12)]
        public string Password { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("Creation Time", TypeName = "DateTime2")]
        public DateTime? CreationTime { get; set; }
    }
}
