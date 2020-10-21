using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Account {
        [Key] public long Id { get; set; }
        [Timestamp] public DateTime LastModified { get; set; }

        public long Merets { get; set; }
        public int MaxCharacters { get; set; }
        public int PrestigeLevel { get; set; }
        public long PrestigeExperience { get; set; }
        public long PremiumTime { get; set; }

        public ICollection<Character> Characters { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Account> entity) {
            entity.HasKey(account => account.Id);
            entity.Property(account => account.MaxCharacters)
                .HasDefaultValue(4);
        }
    }
}