
using MapleServer2.Constants;
using MapleServer2.Database.Classes;
using MySql.Data.MySqlClient;
using NLog;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace MapleServer2.Database
{
    public class DatabaseManager
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static readonly string ConnectionString;

        private static readonly string Server = Environment.GetEnvironmentVariable("DB_IP");
        private static readonly string Port = Environment.GetEnvironmentVariable("DB_PORT");
        private static readonly string Database = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string User = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string Password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        public static DatabaseAccount Accounts { get; private set; } = new DatabaseAccount();
        public static DatabaseBankInventory BankInventories { get; private set; } = new DatabaseBankInventory();
        public static DatabaseBanner Banners { get; private set; } = new DatabaseBanner();
        public static DatabaseBuddy Buddies { get; private set; } = new DatabaseBuddy();
        public static DatabaseCardReverseGame CardReverseGame { get; private set; } = new DatabaseCardReverseGame();
        public static DatabaseCharacter Characters { get; private set; } = new DatabaseCharacter();
        public static DatabaseCube Cubes { get; private set; } = new DatabaseCube();
        public static DatabaseEvent Events { get; private set; } = new DatabaseEvent();
        public static DatabaseGameOptions GameOptions { get; private set; } = new DatabaseGameOptions();
        public static DatabaseGuild Guilds { get; private set; } = new DatabaseGuild();
        public static DatabaseGuildApplication GuildApplications { get; private set; } = new DatabaseGuildApplication();
        public static DatabaseGuildMember GuildMembers { get; private set; } = new DatabaseGuildMember();
        public static DatabaseHome Homes { get; private set; } = new DatabaseHome();
        public static DatabaseHomeLayout HomeLayouts { get; private set; } = new DatabaseHomeLayout();
        public static DatabaseHotbar Hotbars { get; private set; } = new DatabaseHotbar();
        public static DatabaseInventory Inventories { get; private set; } = new DatabaseInventory();
        public static DatabaseItem Items { get; private set; } = new DatabaseItem();
        public static DatabaseLevels Levels { get; private set; } = new DatabaseLevels();
        public static DatabaseMapleopoly Mapleopoly { get; private set; } = new DatabaseMapleopoly();
        public static DatabaseMeretMarket MeretMarket { get; private set; } = new DatabaseMeretMarket();
        public static DatabaseQuest Quests { get; private set; } = new DatabaseQuest();
        public static DatabaseShop Shops { get; private set; } = new DatabaseShop();
        public static DatabaseShopItem ShopItems { get; private set; } = new DatabaseShopItem();
        public static DatabaseSkillTab SkillTabs { get; private set; } = new DatabaseSkillTab();
        public static DatabaseTrophy Trophies { get; private set; } = new DatabaseTrophy();
        public static DatabaseWallet Wallets { get; private set; } = new DatabaseWallet();

        static DatabaseManager()
        {
            ConnectionString = $"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};DATABASE={Database};";
        }

        public static bool DatabaseExists()
        {
            dynamic result = new QueryFactory(new MySqlConnection($"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};"), new MySqlCompiler())
                .Select($"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{Database}'")
                .FirstOrDefault();

            return result != null;
        }

        public static void CreateDatabase()
        {
            string fileLines = File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/SQL/Database.sql");
            MySqlScript script = new MySqlScript(new MySqlConnection($"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};"), fileLines.Replace("DATABASE_NAME", Database));
            script.Execute();
        }

        public static void SeedShops() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/ShopsSeeding.sql"));

        public static void SeedShopItems() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/ShopItemsSeeding.sql"));

        public static void SeedMeretMarket() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/MeretMarketSeeding.sql"));

        public static void SeedMapleopoly() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/MapleopolySeeding.sql"));

        public static void SeedEvents() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/EventsSeeding.sql"));

        public static void SeedCardReverseGame() => ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/CardReverseGameSeeding.sql"));

        private static void ExecuteSqlFile(string fileLines)
        {
            MySqlScript script = new MySqlScript(new MySqlConnection(ConnectionString), fileLines);
            script.Execute();
        }
    }
}
