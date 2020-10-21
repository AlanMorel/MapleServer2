using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Mail {
        [Required] public long SenderId { get; set; }
        [Required] public long RecipientId { get; set; }

        [Key] public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public DateTime ReadTime { get; set; }

        public byte Type { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Mail> entity) {
            entity.Property(item => item.CreationTime)
                .ValueGeneratedOnAdd();
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(listing => listing.SenderId);
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(listing => listing.RecipientId);
        }
    }
}