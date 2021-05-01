using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Account GetAccount(string username, string password)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Accounts.FirstOrDefault(a => a.Username == username && a.Password == password);
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
                .Include(p => p.SkillTabs)
                // .Include(p => p.Guild)
                // .Include(p => p.Home)
                .Include(p => p.GameOptions)
                .Include(p => p.Mailbox).ThenInclude(p => p.Mails)
                .Include(p => p.Wallet)
                .Include(p => p.BuddyList)
                .Include(p => p.Inventory).ThenInclude(p => p.DB_Items)
                .Include(p => p.BankInventory).ThenInclude(p => p.DB_Items)
                .Include(p => p.QuestList)
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

        public static bool CreateCharacter(Player player)
        {
            player.BankInventory.DB_Items = player.BankInventory.Items.Where(x => x != null).ToList();
            player.Inventory.DB_Items = player.Inventory.Items.Values.Where(x => x != null).ToList();
            player.Inventory.DB_Items.AddRange(player.Inventory.Equips.Values.Where(x => x != null).ToList());
            player.Inventory.DB_Items.AddRange(player.Inventory.Cosmetics.Values.Where(x => x != null).ToList());
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Characters.Add(player);
                return SaveChanges(context);
            }
        }

        public static Player GetCharacter(long characterId)
        {
            Player player;
            using (DatabaseContext context = new DatabaseContext())
            {
                player = context.Characters
                .Include(p => p.Levels)
                .Include(p => p.SkillTabs)
                .Include(p => p.Guild)
                // .Include(p => p.Home)
                .Include(p => p.GameOptions)
                .Include(p => p.Mailbox).ThenInclude(p => p.Mails)
                .Include(p => p.Wallet)
                .Include(p => p.BuddyList)
                .Include(p => p.QuestList)
                .Include(p => p.Inventory).ThenInclude(p => p.DB_Items)
                .Include(p => p.BankInventory).ThenInclude(p => p.DB_Items)
                .FirstOrDefault(p => p.CharacterId == characterId);
                if (player == null)
                {
                    return null;
                }
            }

            Levels levels = player.Levels;
            Wallet wallet = player.Wallet;
            player.BankInventory = new BankInventory(player.BankInventory);
            player.Inventory = new Inventory(player.Inventory);
            player.SkillTabs.ForEach(skilltab => skilltab.GenerateSkills(player.Job));
            player.Levels = new Levels(player, levels.Level, levels.Exp, levels.RestExp, levels.PrestigeLevel, levels.PrestigeExp, levels.MasteryExp, levels.Id);
            player.Wallet = new Wallet(player, wallet.Meso.Amount, wallet.Meret.Amount, wallet.GameMeret.Amount,
                                wallet.EventMeret.Amount, wallet.ValorToken.Amount, wallet.Treva.Amount,
                                wallet.Rue.Amount, wallet.HaviFruit.Amount, wallet.MesoToken.Amount, wallet.Bank.Amount, wallet.Id);

            return player;
        }

        public static void UpdateCharacter(Player player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Entry(player).State = EntityState.Modified;
                context.Entry(player.Wallet).State = EntityState.Modified;
                context.Entry(player.Levels).State = EntityState.Modified;
                context.Entry(player.BankInventory).State = EntityState.Modified;
                context.Entry(player.Inventory).State = EntityState.Modified;
                context.Entry(player.Guild).State = EntityState.Modified;
                context.Entry(player.GuildMember).State = EntityState.Modified;
                SaveChanges(context);
            }
        }

        public static bool DeleteCharacter(Player player)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                Player character = context.Characters.Find(player.CharacterId);
                List<Item> items = context.Items.Where(x => x.Owner.CharacterId == player.CharacterId).ToList();
                List<Buddy> buddies = context.Buddies.Where(x => x.Player.AccountId == player.CharacterId).ToList();
                List<QuestStatus> quests = context.Quests.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                List<SkillTab> skilltabs = context.SkillTabs.Where(x => x.Player.CharacterId == player.CharacterId).ToList();
                List<Mail> mails = context.Mails.Where(x => x.PlayerId == player.CharacterId).ToList();
                List<Inventory> inventories = context.Inventories.Where(x => x.Id == player.Inventory.Id).ToList();
                BankInventory bankInventory = context.BankInventories.First(x => x.Id == player.BankInventory.Id);
                Wallet wallet = context.Wallets.First(x => x.Id == player.Wallet.Id);
                Levels level = context.Levels.First(x => x.Id == player.Levels.Id);
                GameOptions gameOptions = context.GameOptions.First(x => x.Id == player.GameOptions.Id);
                Mailbox mailBox = context.MailBoxes.First(x => x.Id == player.Mailbox.Id);

                items.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                buddies.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                quests.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                skilltabs.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                inventories.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                mails.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                context.Entry(wallet).State = EntityState.Deleted;
                context.Entry(bankInventory).State = EntityState.Deleted;
                context.Entry(level).State = EntityState.Deleted;
                context.Entry(gameOptions).State = EntityState.Deleted;
                context.Entry(mailBox).State = EntityState.Deleted;
                context.Entry(character).State = EntityState.Deleted;

                return SaveChanges(context);
            }
        }

        public static Item GetItem(long uid)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                return context.Items.FirstOrDefault(x => x.Uid == uid);
            }
        }

        public static bool UpdateMultipleItems(List<Item> items)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                foreach (Item item in items)
                {
                    context.Entry(item).State = EntityState.Modified;
                }

                return SaveChanges(context);
            }
        }

        public static bool CreateGuild(Guild guild)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.Guilds.Add(guild);
                return SaveChanges(context);
            }
        }

        public static Guild GetGuild(long guildId)
        {
            Guild guild;
            using (DatabaseContext context = new DatabaseContext())
            {
                guild = context.Guilds
                .Include(p => p.Members).ThenInclude(p => p.Player).ThenInclude(p => p.Levels)
                .Include(p => p.Leader)
                .FirstOrDefault(p => p.Id == guildId);
                if (guild == null)
                {
                    return null;
                }
            }
            return guild;
        }

        public static bool CreateGuildMember(GuildMember member)
        {
            using (DatabaseContext context = new DatabaseContext())
            {
                context.GuildMembers.Add(member);
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
