using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class SkillTab {
        [Required] public long CharacterId { get; set; }

        [Key] public long Id { get; set; }
        public int Job { get; set; }
        public string Name { get; set; }
        public byte[] Skills { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<SkillTab> entity) {
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(skillTab => skillTab.CharacterId);
        }
    }
}