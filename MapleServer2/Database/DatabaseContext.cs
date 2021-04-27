﻿using System;
using System.Collections.Generic;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Types;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MapleServer2.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Player> Characters { get; set; }
        public DbSet<Levels> Levels { get; set; }
        public DbSet<SkillTab> SkillTabs { get; set; }
        public DbSet<GameOptions> GameOptions { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<BankInventory> BankInventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Mailbox> MailBoxes { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Buddy> Buddies { get; set; }
        public DbSet<QuestStatus> Quests { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        // public DbSet<Guild> Guilds { get; set; }
        // public DbSet<Home> Homes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Environment.GetEnvironmentVariable("DATABASE_URL"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(25);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(255);
            });

            modelBuilder.Entity<Player>(entity =>
            {
                entity.HasKey(e => e.CharacterId);
                entity.HasIndex(e => e.Name).IsUnique();
                entity.HasOne(e => e.Account).WithMany(p => p.Players);
                entity.Property(e => e.Name).HasMaxLength(25).IsRequired();
                entity.Property(e => e.ProfileUrl).HasDefaultValue("").HasMaxLength(50);
                entity.Property(e => e.Motto).HasDefaultValue("").HasMaxLength(25);
                entity.Property(e => e.HomeName).HasDefaultValue("").HasMaxLength(25);
                entity.Property(e => e.PartyId);
                entity.Property(e => e.ClubId);
                entity.Property(e => e.GuildId);
                entity.Property(e => e.GuildName).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.GuildContribution);
                entity.Property(e => e.ReturnMapId);

                entity.Property(e => e.Titles).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(i));

                entity.Property(e => e.Stats).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new PlayerStats() : JsonConvert.DeserializeObject<PlayerStats>(i));

                entity.Property(e => e.TrophyCount).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new int[3] : JsonConvert.DeserializeObject<int[]>(i));

                entity.Property(e => e.ChatSticker).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<ChatSticker>() : JsonConvert.DeserializeObject<List<ChatSticker>>(i));

                entity.Property(e => e.FavoriteStickers).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(i));

                entity.Property(e => e.Emotes).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(i));

                entity.Property(e => e.Coord).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new CoordF() : JsonConvert.DeserializeObject<CoordF>(i));

                entity.Property(e => e.ReturnCoord).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new CoordF() : JsonConvert.DeserializeObject<CoordF>(i));

                entity.Property(e => e.ReturnCoord).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new CoordF() : JsonConvert.DeserializeObject<CoordF>(i));

                entity.Property(e => e.SkinColor).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new SkinColor() : JsonConvert.DeserializeObject<SkinColor>(i));

                entity.Property(e => e.StatPointDistribution).HasConversion(
                   i => JsonConvert.SerializeObject(i),
                   i => i == null ? new StatDistribution() : JsonConvert.DeserializeObject<StatDistribution>(i));

                entity.Property(e => e.GroupChatId).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new int[3] : JsonConvert.DeserializeObject<int[]>(i));

                entity.HasMany(e => e.SkillTabs).WithOne(x => x.Player);
                entity.HasOne(e => e.GameOptions);
                entity.HasOne(e => e.Inventory);
                entity.HasOne(e => e.BankInventory);
                entity.HasOne(e => e.Mailbox);
                entity.HasMany(e => e.BuddyList).WithOne(e => e.Player);
                //TODO: Trophy Data
                entity.HasOne(e => e.Levels);
                entity.HasOne(e => e.Wallet);
                entity.HasMany(e => e.QuestList).WithOne(e => e.Player);

                entity.Ignore(e => e.DismantleInventory);
                entity.Ignore(e => e.LockInventory);
                entity.Ignore(e => e.HairInventory);
                entity.Ignore(e => e.FishAlbum);
                entity.Ignore(e => e.FishingRod);
                entity.Ignore(e => e.GatheringCount);
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MasteryExp).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<MasteryExp>() : JsonConvert.DeserializeObject<List<MasteryExp>>(i));
                entity.Ignore(e => e.Player);
            });

            // modelBuilder.Entity<Guild>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.Property(e => e.Name).IsRequired().HasMaxLength(25);
            //     entity.HasOne(e => e.Leader);
            //     entity.HasMany(e => e.Members).WithOne(p => p.Guild);
            // });

            // modelBuilder.Entity<Home>(entity =>
            // {
            //     entity.HasKey(e => e.Id);
            //     entity.HasOne(e => e.Owner);
            //     entity.Property(e => e.Name).HasDefaultValue("My Home");
            //     entity.Property(e => e.MapId);
            //     entity.Property(e => e.PlotId);
            //     entity.Property(e => e.PlotNumber);
            //     entity.Property(e => e.ApartmentNumber);
            //     entity.Property(e => e.Expiration);
            // });

            modelBuilder.Entity<SkillTab>(entity =>
            {
                entity.Property(e => e.Name).HasDefaultValue("").HasMaxLength(25);
                entity.Property(e => e.SkillLevels).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new Dictionary<int, int>() : JsonConvert.DeserializeObject<Dictionary<int, int>>(i));

                entity.Ignore(e => e.Order);
                entity.Ignore(e => e.SkillJob);
            });

            modelBuilder.Entity<GameOptions>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.KeyBinds).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new Dictionary<int, KeyBind>() : JsonConvert.DeserializeObject<Dictionary<int, KeyBind>>(i));

                entity.Property(e => e.Hotbars).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<Hotbar>() : JsonConvert.DeserializeObject<List<Hotbar>>(i));
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExtraSize).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new Dictionary<InventoryTab, short>() : JsonConvert.DeserializeObject<Dictionary<InventoryTab, short>>(i));

                entity.HasMany(e => e.DB_Items);

                entity.Ignore(e => e.Equips);
                entity.Ignore(e => e.Cosmetics);
            });

            modelBuilder.Entity<BankInventory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExtraSize);
                entity.HasMany(e => e.DB_Items);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Id);
                entity.Property(e => e.Slot);
                entity.Property(e => e.Amount);
                entity.Property(e => e.CreationTime);
                entity.Property(e => e.ExpiryTime);
                entity.Property(e => e.TimesAttributesChanged);
                entity.Property(e => e.IsLocked);
                entity.Property(e => e.UnlockTime);
                entity.Property(e => e.RemainingGlamorForges);
                entity.Property(e => e.GachaDismantleId);
                entity.Property(e => e.Enchants);
                entity.Property(e => e.EnchantExp);
                entity.Property(e => e.CanRepackage);
                entity.Property(e => e.Charges);
                entity.Property(e => e.TransferFlag);
                entity.Property(e => e.PairedCharacterId);
                entity.Property(e => e.PairedCharacterName).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.PetSkinBadgeId);
                entity.Property(e => e.IsEquipped);
                entity.HasOne(e => e.Owner);

                entity.Property(e => e.RecommendJobs).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<Job>() : JsonConvert.DeserializeObject<List<Job>>(i));

                entity.Property(e => e.TransparencyBadgeBools).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new byte[10] : JsonConvert.DeserializeObject<byte[]>(i));

                entity.Property(e => e.Color).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new EquipColor() : JsonConvert.DeserializeObject<EquipColor>(i));

                entity.Property(e => e.HairData).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new HairData() : JsonConvert.DeserializeObject<HairData>(i));

                entity.Property(e => e.HatData).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new HatData() : JsonConvert.DeserializeObject<HatData>(i));

                entity.Property(e => e.FaceDecorationData).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new byte[16] : JsonConvert.DeserializeObject<byte[]>(i));

                entity.Property(e => e.Score).HasConversion(
                   i => JsonConvert.SerializeObject(i),
                   i => i == null ? new MusicScore() : JsonConvert.DeserializeObject<MusicScore>(i));

                entity.Property(e => e.Stats).HasConversion(
                    i => JsonConvert.SerializeObject(i, settings),
                    i => i == null ? new ItemStats() : JsonConvert.DeserializeObject<ItemStats>(i, settings));

                entity.Ignore(e => e.Content);
                entity.Ignore(e => e.Function);
                entity.Ignore(e => e.AdBalloon);
            });

            modelBuilder.Entity<Mailbox>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Player);
                entity.HasMany(e => e.Mails);
            });

            modelBuilder.Entity<Mail>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.HasMany(e => e.Items);
                entity.Property(e => e.SenderName).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.Title).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.Body);
            });

            modelBuilder.Entity<Buddy>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SharedId);
                entity.Property(e => e.Timestamp);
                entity.Property(e => e.Message).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.BlockReason).HasMaxLength(25).HasDefaultValue("");
            });

            modelBuilder.Entity<QuestStatus>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Basic).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new QuestBasic() : JsonConvert.DeserializeObject<QuestBasic>(i));

                entity.Property(e => e.Condition).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<QuestCondition>() : JsonConvert.DeserializeObject<List<QuestCondition>>(i));

                entity.Ignore(x => x.Basic);
                entity.Ignore(x => x.StartNpcId);
                entity.Ignore(x => x.CompleteNpcId);
                entity.Ignore(x => x.Reward);
                entity.Ignore(x => x.RewardItems);
            });

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Meso).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.Meso, i));
                entity.Property(e => e.Meret).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.Meret, i));
                entity.Property(e => e.GameMeret).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.GameMeret, i));
                entity.Property(e => e.EventMeret).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.EventMeret, i));
                entity.Property(e => e.ValorToken).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.ValorToken, i));
                entity.Property(e => e.Treva).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.Treva, i));
                entity.Property(e => e.Rue).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.Rue, i));
                entity.Property(e => e.HaviFruit).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.HaviFruit, i));
                entity.Property(e => e.MesoToken).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.MesoToken, i));
                entity.Property(e => e.Bank).HasConversion(
                    i => i.Amount,
                    i => new Currency(null, CurrencyType.Bank, i));
            });
        }
    }
}

