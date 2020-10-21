using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Item {
        [Key] public long Id { get; set; }
        [Timestamp] public byte[] LastModified { get; set; }
        [Required]  public long OwnerId { get; set; }

        [Required] public int ItemId { get; set; }
        public short Slot { get; set; }
        public int Amount { get; set; }
        public int Rarity { get; set; }

        public DateTime CreationTime { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int AttributesChangeCount { get; set; }
        public int RemainingUses { get; set; }
        public bool IsLocked { get; set; }
        public DateTime UnlockTime { get; set; }
        public short GlamourForgeCount { get; set; }
        public int Enchants { get; set; }
        public int EnchantExp { get; set; }
        public bool CanRepack { get; set; }
        public int Charges { get; set; }
        public int TradeCount { get; set; }

        public byte MaxSockets { get; set; }
        public byte UnlockedSockets { get; set; }

        public byte[] Stats { get; set; }
        public byte[] Appearance { get; set; }
        public byte[] Transfer { get; set; }
        public byte[] CoupleInfo { get; set; }
        public byte[] ExtraBlob { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Item> entity) {
            entity.Property(item => item.CreationTime)
                .ValueGeneratedOnAdd();
            entity.HasIndex(item => item.OwnerId);
        }
    }
}