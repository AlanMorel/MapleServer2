
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
        private static readonly string ConnectionString;

        public static QueryFactory QueryFactory
        {
            get
            {
                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    QueryFactory queryFactory = new QueryFactory(connection, new MySqlCompiler());
                    // Log the compiled query to the console
                    // queryFactory.Logger = compiled =>
                    // {
                    //     Logger.Debug(compiled.ToString());
                    // };
                    return queryFactory;
                }
            }
        }

        private static readonly string Server = Environment.GetEnvironmentVariable("DB_IP");
        private static readonly string Port = Environment.GetEnvironmentVariable("DB_PORT");
        private static readonly string Database = Environment.GetEnvironmentVariable("DB_NAME");
        private static readonly string User = Environment.GetEnvironmentVariable("DB_USER");
        private static readonly string Password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        public static DatabaseAccount Accounts { get; private set; } = new DatabaseAccount("Accounts");
        public static DatabaseBankInventory BankInventories { get; private set; } = new DatabaseBankInventory("BankInventories");
        public static DatabaseBanner Banners { get; private set; } = new DatabaseBanner("Banners");
        public static DatabaseCardReverseGame CardReverseGame { get; private set; } = new DatabaseCardReverseGame("CardReverseGame");
        public static DatabaseCharacter Characters { get; private set; } = new DatabaseCharacter("Characters");
        public static DatabaseCube Cubes { get; private set; } = new DatabaseCube("Cubes");
        public static DatabaseEvent Events { get; private set; } = new DatabaseEvent("Events");
        public static DatabaseGameOptions GameOptions { get; private set; } = new DatabaseGameOptions("GameOptions");
        public static DatabaseGuild Guilds { get; private set; } = new DatabaseGuild("Guilds");
        public static DatabaseGuildApplication GuildApplications { get; private set; } = new DatabaseGuildApplication("GuildApplications");
        public static DatabaseGuildMember GuildMembers { get; private set; } = new DatabaseGuildMember("GuildMembers");
        public static DatabaseHome Homes { get; private set; } = new DatabaseHome("Homes");
        public static DatabaseHomeLayout HomeLayouts { get; private set; } = new DatabaseHomeLayout("HomeLayouts");
        public static DatabaseHotbar Hotbars { get; private set; } = new DatabaseHotbar("Hotbars");
        public static DatabaseInventory Inventories { get; private set; } = new DatabaseInventory("Inventories");
        public static DatabaseItem Items { get; private set; } = new DatabaseItem("Items");
        public static DatabaseLevels Levels { get; private set; } = new DatabaseLevels("Levels");
        public static DatabaseMapleopoly Mapleopoly { get; private set; } = new DatabaseMapleopoly("Mapleopoly");
        public static DatabaseMeretMarket MeretMarket { get; private set; } = new DatabaseMeretMarket("MeretMarketItems");
        public static DatabaseQuest Quests { get; private set; } = new DatabaseQuest("Quests");
        public static DatabaseShop Shops { get; private set; } = new DatabaseShop("Shops");
        public static DatabaseShopItem ShopItems { get; private set; } = new DatabaseShopItem("ShopItems");
        public static DatabaseSkillTab SkillTabs { get; private set; } = new DatabaseSkillTab("SkillTabs");
        public static DatabaseTrophy Trophies { get; private set; } = new DatabaseTrophy("Trophies");
        public static DatabaseWallet Wallets { get; private set; } = new DatabaseWallet("Wallets");

        static DatabaseManager()
        {
            ConnectionString = $"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};DATABASE={Database};";
        }

        public static bool DatabaseExists()
        {
            dynamic result = new QueryFactory(new MySqlConnection($"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};"), new MySqlCompiler())
                .Select("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'Maple2DB'")
                .FirstOrDefault();

            return result != null;
        }

        public static void CreateDatabase()
        {
            string fileLines = File.ReadAllText(Paths.SOLUTION_DIR + "/MapleServer2/Database/SQL/Database.sql");
            MySqlScript script = new MySqlScript(new MySqlConnection($"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};"), fileLines);
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
