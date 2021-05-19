using System;
using System.Collections.Generic;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Database.Types;
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
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Buddy> Buddies { get; set; }
        public DbSet<QuestStatus> Quests { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Trophy> Trophies { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<GuildMember> GuildMembers { get; set; }
        public DbSet<GuildApplication> GuildApplications { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ShopItem> ShopItems { get; set; }
        // public DbSet<Home> Homes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(Environment.GetEnvironmentVariable("DATABASE_URL"));
            // optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(25);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
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
                entity.HasOne(e => e.Guild);
                entity.HasOne(e => e.GuildMember);
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

                entity.Property(e => e.GuildApplications).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<GuildApplication>() : JsonConvert.DeserializeObject<List<GuildApplication>>(i));

                entity.HasMany(e => e.SkillTabs).WithOne(x => x.Player);
                entity.HasOne(e => e.GameOptions);
                entity.HasOne(e => e.Inventory);
                entity.HasOne(e => e.BankInventory);
                entity.HasMany(e => e.BuddyList).WithOne(e => e.Player);
                entity.HasMany(e => e.Trophies);
                entity.HasOne(e => e.Levels);
                entity.HasOne(e => e.Wallet);
                entity.HasMany(e => e.QuestList).WithOne(e => e.Player);

                entity.Property(e => e.UnlockedTaxis).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(i));

                entity.Property(e => e.UnlockedMaps).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(i));

                entity.Ignore(e => e.DismantleInventory);
                entity.Ignore(e => e.LockInventory);
                entity.Ignore(e => e.HairInventory);
                entity.Ignore(e => e.FishAlbum);
                entity.Ignore(e => e.FishingRod);
                entity.Ignore(e => e.GatheringCount);
                entity.Ignore(e => e.Mailbox);
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MasteryExp).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<MasteryExp>() : JsonConvert.DeserializeObject<List<MasteryExp>>(i));
                entity.Ignore(e => e.Player);
            });

            modelBuilder.Entity<Guild>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(12);
                entity.Property(e => e.CreationTimestamp);
                entity.HasOne(e => e.Leader);
                entity.Property(e => e.Capacity);
                entity.HasMany(e => e.Members);
                entity.Property(e => e.Ranks).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new GuildRank[6] : JsonConvert.DeserializeObject<GuildRank[]>(i));

                entity.Property(e => e.Buffs).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<GuildBuff>() : JsonConvert.DeserializeObject<List<GuildBuff>>(i));

                entity.Property(e => e.Services).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<GuildService>() : JsonConvert.DeserializeObject<List<GuildService>>(i));

                entity.HasMany(e => e.GiftBank);
                entity.HasMany(e => e.Applications);
                entity.Property(e => e.Funds);
                entity.Property(e => e.Exp);
                entity.Property(e => e.Searchable);
                entity.Property(e => e.Notice).HasDefaultValue("").HasMaxLength(300);
                entity.Property(e => e.Emblem).HasDefaultValue("").HasMaxLength(50);
                entity.Property(e => e.FocusAttributes);
                entity.Property(e => e.HouseRank);
                entity.Property(e => e.HouseTheme);
            });

            modelBuilder.Entity<GuildMember>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasOne(e => e.Player);
                entity.Property(e => e.Motto).HasDefaultValue("").HasMaxLength(50);
                entity.Property(e => e.Rank);
                entity.Property(e => e.DailyContribution);
                entity.Property(e => e.ContributionTotal);
                entity.Property(e => e.DailyDonationCount);
                entity.Property(e => e.AttendanceTimestamp);
                entity.Property(e => e.JoinTimestamp);
            });

            modelBuilder.Entity<GuildApplication>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.GuildId);
                entity.Property(e => e.CharacterId);
                entity.Property(e => e.CreationTimestamp);
            });

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

                entity.HasMany(e => e.DB_Items).WithOne(x => x.Inventory);

                entity.Ignore(e => e.Equips);
                entity.Ignore(e => e.Cosmetics);
                entity.Ignore(e => e.Items);
                entity.Ignore(e => e.Badges);
            });

            modelBuilder.Entity<BankInventory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExtraSize);
                entity.HasMany(e => e.DB_Items).WithOne(x => x.BankInventory);
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Id);
                entity.Property(e => e.Slot);
                entity.Property(e => e.Amount);
                entity.Property(e => e.Level);
                entity.Property(e => e.Rarity);
                entity.Property(e => e.PlayCount);
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

                entity.Ignore(e => e.InventoryTab);
                entity.Ignore(e => e.GemSlot);
                entity.Ignore(e => e.StackLimit);
                entity.Ignore(e => e.EnableBreak);
                entity.Ignore(e => e.IsTwoHand);
                entity.Ignore(e => e.IsDress);
                entity.Ignore(e => e.IsTemplate);
                entity.Ignore(e => e.IsCustomScore);
                entity.Ignore(e => e.Gender);
                entity.Ignore(e => e.FileName);
                entity.Ignore(e => e.SkillId);
                entity.Ignore(e => e.RecommendJobs);
                entity.Ignore(e => e.Content);
                entity.Ignore(e => e.Function);
                entity.Ignore(e => e.AdBalloon);
                entity.Ignore(e => e.Tag);
                entity.Ignore(e => e.ShopID);
            });

            modelBuilder.Entity<Mail>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.HasMany(e => e.Items);
                entity.Property(e => e.SenderName).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.Title).HasMaxLength(25).HasDefaultValue("");
                entity.Property(e => e.Body);
                entity.HasOne(e => e.Player);
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
                entity.Property(e => e.Id);
                entity.Property(e => e.Condition).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<Condition>() : JsonConvert.DeserializeObject<List<Condition>>(i));
                entity.Property(e => e.Started);
                entity.Property(e => e.Completed);
                entity.Property(e => e.StartTimestamp);
                entity.Property(e => e.CompleteTimestamp);

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

            modelBuilder.Entity<Trophy>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Id);
                entity.Property(e => e.NextGrade);
                entity.Property(e => e.MaxGrade);
                entity.Property(e => e.Counter);
                entity.Property(e => e.Condition);
                entity.Property(e => e.IsDone);
                entity.Property(e => e.Type).HasMaxLength(25);
                entity.Property(e => e.Timestamps).HasConversion(
                    i => JsonConvert.SerializeObject(i),
                    i => i == null ? new List<long>() : JsonConvert.DeserializeObject<List<long>>(i));
            });

            modelBuilder.Entity<Shop>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.Id);
                entity.Property(e => e.TemplateId);
                entity.Property(e => e.Category);
                entity.Property(e => e.Name).HasMaxLength(25);
                entity.Property(e => e.ShopType);
                entity.Property(e => e.RestrictSales);
                entity.Property(e => e.CanRestock);
                entity.Property(e => e.NextRestock);
                entity.Property(e => e.AllowBuyback);
                entity.HasMany(e => e.Items);
            });

            modelBuilder.Entity<ShopItem>(entity =>
            {
                entity.HasKey(e => e.Uid);
                entity.Property(e => e.ItemId);
                entity.Property(e => e.TokenType);
                entity.Property(e => e.RequiredItemId);
                entity.Property(e => e.Price);
                entity.Property(e => e.SalePrice);
                entity.Property(e => e.ItemRank);
                entity.Property(e => e.StockCount);
                entity.Property(e => e.StockPurchased);
                entity.Property(e => e.GuildTrophy);
                entity.Property(e => e.Category).HasMaxLength(25);
                entity.Property(e => e.RequiredAchievementId);
                entity.Property(e => e.RequiredAchievementGrade);
                entity.Property(e => e.RequiredChampionshipGrade);
                entity.Property(e => e.RequiredChampionshipJoinCount);
                entity.Property(e => e.RequiredGuildMerchantType);
                entity.Property(e => e.RequiredGuildMerchantLevel);
                entity.Property(e => e.Quantity);
                entity.Property(e => e.Flag);
                entity.Property(e => e.TemplateName).HasMaxLength(25);
                entity.Property(e => e.RequiredQuestAlliance);
                entity.Property(e => e.RequiredFameGrade);
                entity.Property(e => e.AutoPreviewEquip);
            });
        }
    }
}

