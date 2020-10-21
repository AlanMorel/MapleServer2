using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Character {
        [Required] public long AccountId { get; set; }
        [ForeignKey(nameof(AccountId))] public Account Account { get; set; }

        [Key] public long Id { get; set; }
        [Required] public string Name { get; set; }
        [Timestamp] public DateTime LastModified { get; set; }

        public DateTime CreationTime { get; set; }
        public byte Gender { get; set; } // Gender - 0 = male, 1 = female
        public int Job { get; set; }
        public short Level { get; set; }
        public byte[] SkinColor { get; set; }

        public long Experience { get; set; }
        public long RestExperience { get; set; }

        public int MapId { get; set; }
        public int Title { get; set; }
        public short Insignia { get; set; }

        public byte[] AttributePoints { get; set; }

        public long? GuildId { get; set; }

        /* Private */
        public int ReturnMapId { get; set; }
        public byte[] ReturnPosition { get; set; }

        public Currency Currency { get; set; }
        public SkillBook SkillBook { get; set; }

        public byte[] Mastery { get; set; }

        public Character() {
            Currency = new Currency();
            SkillBook = new SkillBook();
        }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Character> entity) {
            entity.HasIndex(character => character.Name)
                .IsUnique();
            entity.Property(character => character.CreationTime)
                .ValueGeneratedOnAdd();
            entity.OwnsOne(character => character.SkillBook,
                builder => builder.Property(skillBook => skillBook.MaxTabCount).HasDefaultValue(1));
            entity.HasOne<Guild>()
                .WithMany()
                .HasForeignKey(character => character.GuildId)
                .IsRequired(false);
        }
    }

    [Owned]
    public class Currency {
        public long Mesos { get; set; }
        public long ValorToken { get; set; }
        public long Treva { get; set; }
        public long Rue { get; set; }
        public long HaviFruit { get; set; }
        public long MesoToken { get; set; }
    }

    [Owned]
    public class SkillBook {
        public long ActiveTabId { get; set; }
        public int MaxTabCount { get; set; }
    }
}