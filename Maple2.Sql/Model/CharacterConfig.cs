using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    [Table("char_config")]
    public class CharacterConfig {
        [Key] public long CharacterId { get; set; }

        public byte[] KeyBinds { get; set; }
        public byte[] HotBars { get; set; }
        public byte[] Macros { get; set; }
        public byte[] Wardrobe { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<CharacterConfig> entity) {
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(config => config.CharacterId);
        }
    }
}