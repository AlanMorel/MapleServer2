using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Quest {
        public long CharacterId { get; set; }
        public int QuestId { get; set; }

        public int State { get; set; }
        public int CompletionCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Accepted { get; set; }
        public byte[] Conditions { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Quest> entity) {
            entity.HasKey(quest => new {quest.CharacterId, quest.QuestId});
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(quest => quest.CharacterId);
        }
    }
}