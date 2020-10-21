using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    [Table("mm_listing")]
    public class MesoMarketListing {
        [Required] public long AccountId { get; set; }

        [Key] public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public long Price { get; set; }
        public long Mesos { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<MesoMarketListing> entity) {
            entity.Property(listing => listing.CreationTime)
                .ValueGeneratedOnAdd();
            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(listing => listing.AccountId);
        }
    }
}