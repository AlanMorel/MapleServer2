
using Maple2Storage.Extensions;
using Maple2Storage.Types;
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

        private static readonly int MIN_MYSQL_VERSION = 8;

        public static readonly string ConnectionString;
        public static readonly string ConnectionStringWithoutTable;

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
        public static DatabaseMail Mails { get; private set; } = new DatabaseMail();
        public static DatabaseBlackMarketListing BlackMarketListings { get; private set; } = new DatabaseBlackMarketListing();
        public static DatabaseServer ServerInfo { get; private set; } = new DatabaseServer();

        static DatabaseManager()
        {
            ConnectionStringWithoutTable = $"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};";
            ConnectionString = ConnectionStringWithoutTable + $"DATABASE={Database};";
        }

        public static void Init()
        {
            if (GetVersion() < MIN_MYSQL_VERSION)
            {
                throw new Exception("MySQL version out-of-date, please upgrade to version " + MIN_MYSQL_VERSION + ".");
            }

            if (DatabaseExists())
            {
                Logger.Info("Database already exists.");
                return;
            }

            Logger.Info("Creating database...");
            CreateDatabase();

            string[] seeds = { "Shops", "ShopItems", "MeretMarket", "Mapleopoly", "Events", "CardReverseGame" };

            foreach (string seed in seeds)
            {
                Seed(seed);
            }

            Logger.Info("Database created.".ColorGreen());
        }

        public static void RunQuery(string query) => new QueryFactory(new MySqlConnection(ConnectionString), new MySqlCompiler()).Statement(query);

        public static int GetVersion()
        {
            MySqlConnection conn = new MySqlConnection(ConnectionStringWithoutTable);
            conn.Open();

            return int.Parse(conn.ServerVersion.Split(".")[0]);
        }

        public static bool DatabaseExists()
        {
            dynamic result = new QueryFactory(new MySqlConnection(ConnectionStringWithoutTable), new MySqlCompiler())
                .Select($"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{Database}'")
                .FirstOrDefault();

            return result != null;
        }

        public static void CreateDatabase()
        {
            string fileLines = File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/SQL/Database.sql");
            MySqlScript script = new MySqlScript(new MySqlConnection(ConnectionStringWithoutTable), fileLines.Replace("DATABASE_NAME", Database));
            script.Execute();
        }

        private static void Seed(string type)
        {
            Logger.Info("Seeding " + type + "...");
            ExecuteSqlFile(File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/Seeding/" + type + "Seeding.sql"));
        }

        private static void ExecuteSqlFile(string fileLines)
        {
            MySqlScript script = new MySqlScript(new MySqlConnection(ConnectionString), fileLines);
            script.Execute();
        }
    }
}
