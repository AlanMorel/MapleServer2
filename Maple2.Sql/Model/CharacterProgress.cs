using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    [Table("char_progress")]
    public class CharacterProgress {
        [Key] public long CharacterId { get; set; }

        public byte[] VisitedMaps { get; set; }
        public byte[] Taxis { get; set; }
        public byte[] Titles { get; set; }
        public byte[] Emotes { get; set; }
        // Stamps

        // Table Configuration
        public static void Configure(EntityTypeBuilder<CharacterProgress> entity) {
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(progress => progress.CharacterId);
        }
    }
}