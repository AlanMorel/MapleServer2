using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Achieve {
        public long CharacterId { get; set; }
        public int AchieveId { get; set; }

        public byte Type { get; set; }
        public int StartGrade { get; set; }
        public int CurrentGrade { get; set; }
        public int EndGrade { get; set; }
        // byte unknown
        public long Count { get; set; }
        public byte[] Record { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Achieve> entity) {
            entity.HasKey(achieve => new {achieve.CharacterId, achieve.AchieveId});
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(achieve => achieve.CharacterId);
        }
    }
}