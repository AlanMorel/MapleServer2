﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ms2Database.DbClasses
{
    public class Character
    {
        public Character() // Constructor to catch any fields that are not entered
        {
            CreationTime = DateTime.Now;
            Gender = 0;
            Awakened = false;
            Level = 1;
            Exp = 0;
            RestExp = 0;
            PrestigeLvl = 1;
            PrestigeExp = 0;
            Mesos = 0;
            Meret = 0;
            ValorToken = 0;
            Treva = 0;
            Rue = 0;
            MesoToken = 0;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Key]
        public long CharacterId { get; set; }

        [ForeignKey("Account")]
        [Required]
        public long AccountId { get; set; }
        public Account Account { get; set; }

        [Column("Creation Time", TypeName = "datetime2")]
        public DateTime? CreationTime { get; set; }

        [MaxLength(25)]
        public string Name { get; set; }

        public byte Gender { get; set; }

        public int JobId { get; set; }

        public bool Awakened { get; set; }

        [Column(TypeName = "smallint")]
        public short Level { get; set; }

        public long Exp { get; set; }

        public long RestExp { get; set; }

        public int PrestigeLvl { get; set; }

        public long PrestigeExp { get; set; }

        public int? TitleId { get; set; }

        [Column(TypeName = "smallint")]
        public short? InsigniaId { get; set; }

        public long Mesos { get; set; }

        public long Meret { get; set; }

        public long ValorToken { get; set; }

        public long Treva { get; set; }

        public long Rue { get; set; }

        public long HaviFruit { get; set; }

        public long MesoToken { get; set; }

        public long? PartyId { get; set; }

        public long? ClubId { get; set; }

        public long? GuildId { get; set; }

        [MaxLength(25)]
        public string GuildName { get; set; }

        public string ProfileUrl { get; set; }

        [MaxLength(25)]
        public string Motto { get; set; }

        [MaxLength(25)]
        public string HomeName { get; set; }

        [Required]
        public int MapId { get; set; }

        [Required]
        public float CoordX { get; set; }

        [Required]
        public float CoordY { get; set; }

        [Required]
        public float CoordZ { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public ICollection<SkillTree> SkillTrees { get; set; }
    }
}
