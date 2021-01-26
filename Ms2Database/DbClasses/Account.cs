using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Ms2Database.DbClasses
{
    public class Account
    {
        public Account() // Constructor for account table
        {
            CreationTime = DateTime.Now;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Sets field to auto increment
        [Required] // Field cannot be null
        [Key]
        public long AccountId { get; set; }

        [MaxLength(25)]
        public string Username { get; set; }

        [MaxLength(25)]
        public string Password { get; set; }

        [Column("Creation Time", TypeName = "datetime2")] // Sets column name and data type
        public DateTime? CreationTime { get; set; }

        public ICollection<Character> Characters { get; set; }
    }
}
