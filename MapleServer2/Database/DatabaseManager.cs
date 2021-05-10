using System;
using System.Collections.Generic;
using System.Linq;
using MapleServer2.Database.Types;
using MapleServer2.Types;
using Microsoft.EntityFrameworkCore;

namespace MapleServer2.Database
{
    public class DatabaseManager
    {
        public static bool Insert(dynamic entry)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(entry).State = EntityState.Added;
                return SaveChanges(context);
            }
        }

        public static bool Update(dynamic entry)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(entry).State = EntityState.Modified;
                return SaveChanges(context);
            }
        }

        public static bool Delete(dynamic entry)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(entry).State = EntityState.Deleted;
                return SaveChanges(context);
            }
        }

        public static long CreateAccount(Account account)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Accounts.Add(account);
                SaveChanges(context);
                return account.Id;
            }
        }

        public static Account GetAccount(string username, string passwordHash)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.FirstOrDefault(a => a.Username == username && a.PasswordHash == passwordHash);
            }
        }

        public static Account Authenticate(string username, string password)
        {
            Account account;
            using (DatabaseContext context = new DatabaseContext())
            {
                account = context.Accounts.SingleOrDefault(x => x.Username == username);
            }

            if (account == null || !BCrypt.Net.BCrypt.Verify(password, account.PasswordHash))
            {
                return null;
            }

            return account;
        }

        public static bool AccountExists(string username)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.FirstOrDefault(a => a.Username == username) != null;
            }
        }

        public static List<Player> GetAccountCharacters(long accountId)
        {
            List<Player> characters;
            using (DatabaseContext context = new DatabaseContext())
            {
                characters = context.Characters
                .Where(p => p.AccountId == accountId)
                .Include(p => p.Levels)
                .Include(p => p.Inventory).ThenInclude(p => p.DB_Items)
                .ToList();
            }

            foreach (Player player in characters)
            {
                player.Inventory = new Inventory(player.Inventory);
                Levels levels = player.Levels;
                player.Levels = new Levels(player, levels.Level, levels.Exp, levels.RestExp, levels.PrestigeLevel, levels.PrestigeExp, levels.MasteryExp, levels.Id);
            }

            return characters;
        }

        public static long CreateCharacter(Player player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Characters.Add(player);
                SaveChanges(context);
                return player.CharacterId;
            }
        }

        public static Player GetCharacter(long characterId)
        {
            Player player;
            List<Mail> mails = new List<Mail>();
            using (DatabaseContext context = new DatabaseContext())
            {
                player = context.Characters
                .Include(p => p.Levels)
                .Include(p => p.SkillTabs)
                .Include(p => p.Guild)
                // .Include(p => p.Home)
                .Include(p => p.GameOptions)
                .Include(p => p.Wallet)
                .Include(p => p.BuddyList)
                .Include(p => p.QuestList)
                .Include(p => p.Trophies)
                .Include(p => p.Inventory).ThenInclude(p => p.DB_Items)
                .Include(p => p.BankInventory).ThenInclude(p => p.DB_Items)
                .FirstOrDefault(p => p.CharacterId == characterId);
                if (player == null)
                {
                    return null;
                }
                List<Mail> dbMails = context.Mails.Where(m => m.PlayerId == player.CharacterId).ToList();
                mails.AddRange(dbMails);
            }

            Levels levels = player.Levels;
            Wallet wallet = player.Wallet;
            foreach (Trophy trophy in player.Trophies)
            {
                player.TrophyData[trophy.Id] = trophy;
            }
            foreach (QuestStatus item in player.QuestList)
            {
                item.SetMetadataValues(item.Id);
            }
            player.BankInventory = new BankInventory(player.BankInventory);
            player.Inventory = new Inventory(player.Inventory);
            player.Mailbox = new Mailbox(mails);
            player.SkillTabs.ForEach(skilltab => skilltab.GenerateSkills(player.Job));
            player.Levels = new Levels(player, levels.Level, levels.Exp, levels.RestExp, levels.PrestigeLevel, levels.PrestigeExp, levels.MasteryExp, levels.Id);
            player.Wallet = new Wallet(player, wallet.Meso.Amount, wallet.Meret.Amount, wallet.GameMeret.Amount,
                                wallet.EventMeret.Amount, wallet.ValorToken.Amount, wallet.Treva.Amount,
                                wallet.Rue.Amount, wallet.HaviFruit.Amount, wallet.MesoToken.Amount, wallet.Bank.Amount, wallet.Id);

            return player;
        }

        public static void UpdateCharacter(Player player)
        {
            player.TrophyCount = new int[3];
            if (player.Trophies != null)
            {
                List<List<long>> combat = player.Trophies.Where(x => x.Type == "combat").Select(x => x.Timestamps).ToList();
                foreach (List<long> item in combat.Where(x => x.Count != 0))
                {
                    player.TrophyCount[0] += item.Count;
                }

                List<List<long>> adventure = player.Trophies.Where(x => x.Type == "adventure").Select(x => x.Timestamps).ToList();
                foreach (List<long> item in adventure.Where(x => x.Count != 0))
                {
                    player.TrophyCount[1] += item.Count;
                }

                List<List<long>> living = player.Trophies.Where(x => x.Type == "living").Select(x => x.Timestamps).ToList();
                foreach (List<long> item in living.Where(x => x.Count != 0))
                {
                    player.TrophyCount[2] += item.Count;
                }
            }

            using (DatabaseContext context = new DatabaseContext())
            {
                Player dbPlayer = context.Characters.Find(player.CharacterId);
                Wallet dbWallet = context.Wallets.Find(player.Wallet.Id);
                Levels dbLevels = context.Levels.Find(player.Levels.Id);
                BankInventory dbBankInventory = context.BankInventories.Find(player.BankInventory.Id);
                Inventory dbInventory = context.Inventories.Find(player.Inventory.Id);

                context.Entry(dbPlayer).CurrentValues.SetValues(player);
                context.Entry(dbWallet).CurrentValues.SetValues(player.Wallet);
                context.Entry(dbLevels).CurrentValues.SetValues(player.Levels);
                context.Entry(dbBankInventory).CurrentValues.SetValues(player.BankInventory);
                context.Entry(dbInventory).CurrentValues.SetValues(player.Inventory);

                if (player.GuildMember != null)
                {
                    GuildMember dbGuildMember = context.GuildMembers.Find(player.CharacterId);
                    context.Entry(dbGuildMember).CurrentValues.SetValues(player.GuildMember);
                }
                UpdateQuests(player);
                UpdateTrophies(player);
                UpdateItems(player);
                SaveChanges(context);
            }
        }

        public static bool DeleteCharacter(Player player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Player character = context.Characters.Find(player.CharacterId);
                List<Item> items = context.Items.Where(x => x.BankInventory.Id == player.BankInventory.Id || x.Inventory.Id == player.Inventory.Id).ToList();
                List<Buddy> buddies = context.Buddies.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                List<QuestStatus> quests = context.Quests.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                List<SkillTab> skilltabs = context.SkillTabs.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                List<Mail> mails = context.Mails.Where(x => x.PlayerId == player.CharacterId).ToList();
                Inventory inventory = context.Inventories.First(x => x.Id == player.Inventory.Id);
                BankInventory bankInventory = context.BankInventories.First(x => x.Id == player.BankInventory.Id);
                Wallet wallet = context.Wallets.First(x => x.Id == player.Wallet.Id);
                Levels level = context.Levels.First(x => x.Id == player.Levels.Id);
                GameOptions gameOptions = context.GameOptions.First(x => x.Id == player.GameOptions.Id);

                if (player.Trophies.Count != 0)
                {
                    List<Trophy> trophies = context.Trophies.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                    trophies.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                }

                items.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                buddies.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                quests.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                skilltabs.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                mails.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                context.Entry(inventory).State = EntityState.Deleted;
                context.Entry(wallet).State = EntityState.Deleted;
                context.Entry(bankInventory).State = EntityState.Deleted;
                context.Entry(level).State = EntityState.Deleted;
                context.Entry(gameOptions).State = EntityState.Deleted;
                context.Entry(character).State = EntityState.Deleted;

                return SaveChanges(context);
            }
        }

        public static bool NameExists(string name)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Characters.FirstOrDefault(x => x.Name.ToLower() == name.ToLower()) != null;
            }
        }

        public static long AddItem(Item item)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Items.Add(item);
                SaveChanges(context);
                return item.Uid;
            }
        }

        public static Item GetItem(long uid)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Items.FirstOrDefault(x => x.Uid == uid);
            }
        }

        public static bool UpdateItems(Player player)
        {
            Inventory inventory = player.Inventory;
            inventory.DB_Items = inventory.Items.Values.Where(x => x != null).ToList();
            inventory.DB_Items.AddRange(inventory.Equips.Values.Where(x => x != null).ToList());
            inventory.DB_Items.AddRange(inventory.Cosmetics.Values.Where(x => x != null).ToList());

            BankInventory bankInventory = player.BankInventory;
            bankInventory.DB_Items = bankInventory.Items.Where(x => x != null).ToList();

            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Item item in inventory.DB_Items)
                {
                    Item dbItem = context.Items.Include(x => x.Inventory).Include(x => x.BankInventory).FirstOrDefault(x => x.Uid == item.Uid);
                    if (dbItem == null)
                    {
                        item.Inventory = inventory;
                        context.Entry(item).State = EntityState.Added;
                        continue;
                    }
                    item.BankInventory = null;
                    dbItem.BankInventory = null;
                    context.Entry(dbItem).CurrentValues.SetValues(item);
                }

                foreach (Item item in bankInventory.DB_Items)
                {
                    Item dbItem = context.Items.Include(x => x.Inventory).Include(x => x.BankInventory).FirstOrDefault(x => x.Uid == item.Uid);
                    if (dbItem == null)
                    {
                        item.BankInventory = bankInventory;
                        context.Entry(item).State = EntityState.Added;
                        continue;
                    }
                    item.Inventory = null;
                    dbItem.Inventory = null;
                    context.Entry(dbItem).CurrentValues.SetValues(item);
                }
                return SaveChanges(context);
            }
        }

        public static long AddTrophy(Trophy trophy)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(trophy).State = EntityState.Added;
                SaveChanges(context);
                return trophy.Uid;
            }
        }

        public static bool UpdateTrophies(Player player)
        {
            player.Trophies = player.TrophyData.Values.ToList();
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Trophy trophy in player.Trophies)
                {
                    Trophy dbTrophy = context.Trophies.Find(trophy.Uid);
                    if (dbTrophy == null)
                    {
                        context.Entry(trophy).State = EntityState.Added;
                        continue;
                    }
                    context.Entry(dbTrophy).CurrentValues.SetValues(trophy);
                }
                return SaveChanges(context);
            }
        }

        public static List<Guild> GetGuilds()
        {
            List<Guild> guilds = new List<Guild>();
            using (DatabaseContext context = new DatabaseContext())
            {
                guilds = context.Guilds
                .Include(p => p.Members).ThenInclude(p => p.Player).ThenInclude(p => p.Levels)
                .Include(p => p.Leader).ToList();
            }
            return guilds;
        }

        public static bool GuildExists(string guildName)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Guild result = context.Guilds.FirstOrDefault(p => p.Name.ToLower() == guildName.ToLower());

                return result != null;
            }
        }

        public static long CreateGuild(Guild guild)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Guilds.Add(guild);
                SaveChanges(context);
                return guild.Id;
            }
        }

        public static Guild GetGuild(long guildId)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Guild guild = context.Guilds
                .Include(p => p.Members).ThenInclude(p => p.Player).ThenInclude(p => p.Levels)
                .Include(p => p.Leader)
                .FirstOrDefault(p => p.Id == guildId);
                if (guild == null)
                {
                    return null;
                }

                return guild;
            }
        }

        public static bool UpdateGuild(Guild guild)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Guild dbGuild = context.Guilds.Find(guild.Id);
                context.Entry(dbGuild).CurrentValues.SetValues(guild);
                return SaveChanges(context);
            }
        }

        public static bool CreateGuildMember(GuildMember member)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.GuildMembers.Add(member);
                return SaveChanges(context);
            }
        }

        public static bool CreateGuildApplication(GuildApplication application)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.GuildApplications.Add(application);
                return SaveChanges(context);
            }
        }

        public static void InsertShops(List<Shop> shops)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Shop shop in shops)
                {
                    context.Shops.Add(shop);
                }
                SaveChanges(context);
            }
        }

        public static Shop GetShop(int id)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Shops.Include(x => x.Items).FirstOrDefault(x => x.Id == id);
            }
        }

        public static ShopItem GetShopItem(long uid)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.ShopItems.FirstOrDefault(x => x.Uid == uid);
            }
        }

        public static long AddQuest(QuestStatus questStatus)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(questStatus).State = EntityState.Added;
                SaveChanges(context);
                return questStatus.Uid;
            }
        }

        public static bool UpdateQuests(Player player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (QuestStatus quest in player.QuestList)
                {
                    QuestStatus dbQuest = context.Quests.Find(quest.Uid);
                    if (dbQuest == null)
                    {
                        context.Entry(quest).State = EntityState.Added;
                        continue;
                    }
                    context.Entry(dbQuest).CurrentValues.SetValues(quest);
                }
                return SaveChanges(context);
            }
        }

        private static bool SaveChanges(DatabaseContext context)
        {
            try
            {
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
