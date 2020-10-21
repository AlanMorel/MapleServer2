using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    [Table("bm_listing")]
    public class BlackMarketListing {
        [Required] public long AccountId { get; set; }
        [Required] public long CharacterId { get; set; }
        [Required] public long ItemId { get; set; }
        [ForeignKey(nameof(ItemId))] public Item Item { get; set; }

        [Key] public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public long Price { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<BlackMarketListing> entity) {
            entity.Property(listing => listing.CreationTime)
                .ValueGeneratedOnAdd();
            entity.HasOne<Account>()
                .WithMany()
                .HasForeignKey(listing => listing.AccountId);
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(listing => listing.CharacterId);
        }
    }
}