using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Maple2.Sql.Model {
    public class Guild {
        [Key] public long Id { get; set; }

        [Required] public string Name { get; set; }
        public string DisplayPicture { get; set; }
        public byte MaxMembers { get; set; }
        public string Message { get; set; }

        // For leader AccountId, CharacterId, Name
        [Required] public long LeaderId { get; set; }

        public long CreationTime { get; set; }
        public int Focus { get; set; }
        public int Experience { get; set; }
        public int Funds { get; set; }
        public ICollection<Character> Members { get; set; }
        public byte[] Ranks { get; set; }
        public byte[] Skills { get; set; }
        public byte[] Events { get; set; }
        public byte[] Posters { get; set; }
        public byte[] Npcs { get; set; }

        // Table Configuration
        public static void Configure(EntityTypeBuilder<Guild> entity) {
            entity.HasOne<Character>()
                .WithMany()
                .HasForeignKey(guild => guild.LeaderId);
        }
    }
}