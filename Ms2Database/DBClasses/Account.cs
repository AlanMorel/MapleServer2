using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.DbClasses
{
    public class Account // Table structure for account
    {
        public Account() // Constructor for account table
        {
            CreationTime = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Sets field to auto increment
        [Required] // Field cannot be null
        [Key]
        public long AccountId { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(10)]
        public string Username { get; set; }

        [Column(TypeName = "nvarchar")]
        [StringLength(12)]
        public string Password { get; set; }

        [Column("Creation Time", TypeName = "datetime2")] // Sets column name and data type
        public DateTime? CreationTime { get; set; }
    }
}
