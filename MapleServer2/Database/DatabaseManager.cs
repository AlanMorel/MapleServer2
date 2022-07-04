using Maple2Storage.Types;
using MapleServer2.Database.Classes;
using MySql.Data.MySqlClient;
using Serilog;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace MapleServer2.Database;

public static class DatabaseManager
{
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(DatabaseManager));
    private static readonly Tuple<int, int> MIN_MYSQL_VERSION = new(8, 0); // Primary support version of MySQL (major version, minor version)

    public static readonly string ConnectionString;
    public static readonly string ConnectionStringWithoutTable;

    private static readonly string Server = Environment.GetEnvironmentVariable("DB_IP");
    private static readonly string Port = Environment.GetEnvironmentVariable("DB_PORT");
    private static readonly string Database = Environment.GetEnvironmentVariable("DB_NAME");
    private static readonly string User = Environment.GetEnvironmentVariable("DB_USER");
    private static readonly string Password = Environment.GetEnvironmentVariable("DB_PASSWORD");

    public static DatabaseAccount Accounts { get; } = new();
    public static DatabaseAuthData AuthData { get; } = new();
    public static DatabaseBankInventory BankInventories { get; } = new();
    public static DatabaseBanner Banners { get; } = new();
    public static DatabaseBuddy Buddies { get; } = new();
    public static DatabaseCardReverseGame CardReverseGame { get; } = new();
    public static DatabaseCharacter Characters { get; } = new();
    public static DatabaseCube Cubes { get; } = new();
    public static DatabaseEvent Events { get; } = new();
    public static DatabaseGameOptions GameOptions { get; } = new();
    public static DatabaseMacros Macros { get; } = new();
    public static DatabaseGuild Guilds { get; } = new();
    public static DatabaseGuildApplication GuildApplications { get; } = new();
    public static DatabaseGuildMember GuildMembers { get; } = new();
    public static DatabaseClub Clubs { get; } = new();
    public static DatabaseClubMember ClubMembers { get; } = new();
    public static DatabaseHome Homes { get; } = new();
    public static DatabaseHomeLayout HomeLayouts { get; } = new();
    public static DatabaseHotbar Hotbars { get; } = new();
    public static DatabaseInventory Inventories { get; } = new();
    public static DatabaseItem Items { get; } = new();
    public static DatabaseLevels Levels { get; } = new();
    public static DatabaseMapleopoly Mapleopoly { get; } = new();
    public static DatabaseGameEventUserValue GameEventUserValue { get; } = new();
    public static DatabaseMeretMarket MeretMarket { get; } = new();
    public static DatabaseQuest Quests { get; } = new();
    public static DatabaseShop Shops { get; } = new();
    public static DatabaseShopItem ShopItems { get; } = new();
    public static DatabaseBeautyShop BeautyShops { get; } = new();
    public static DatabaseBeautyShopItem BeautyShopItems { get; } = new();
    public static DatabaseSkillTab SkillTabs { get; } = new();
    public static DatabaseTrophy Trophies { get; } = new();
    public static DatabaseUGC UGC { get; } = new();
    public static DatabaseWallet Wallets { get; } = new();
    public static DatabaseMail Mails { get; } = new();
    public static DatabaseBlackMarketListing BlackMarketListings { get; } = new();
    public static DatabaseMesoMarketListing MesoMarketListings { get; } = new();
    public static DatabaseUgcMarketItem UgcMarketItems { get; } = new();
    public static DatabaseUgcMarketSale UgcMarketSales { get; } = new();
    public static DatabaseServer ServerInfo { get; } = new();
    public static DatabaseMushkingRoyaleStats MushkingRoyaleStats { get; } = new();
    public static DatabaseMedal MushkingRoyaleMedals { get; } = new();
    public static DatabaseBannerSlot BannerSlot { get; } = new();
    public static DatabaseRouletteGameItem RouletteGameItems { get; } = new();
    public static DatabaseOXQuizQuestion OxQuizQuestion { get; } = new();
    public static DatabasePets Pets { get; } = new();

    static DatabaseManager()
    {
        ConnectionStringWithoutTable = $"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};";
        ConnectionString = ConnectionStringWithoutTable + $"DATABASE={Database};";
    }

    public static void Init()
    {
        Tuple<Tuple<int, int>, string> mysqlVersionFormat = GetVersion();
        Tuple<int, int> mysqlVersion = mysqlVersionFormat.Item1;
        bool useLegacy = false;

        if (mysqlVersion.Item1 >= MIN_MYSQL_VERSION.Item1 && mysqlVersion.Item2 >= MIN_MYSQL_VERSION.Item2)
        {
            Logger.Information("Found supported MySQL version.");
        } else if (mysqlVersionFormat.Item2.Contains("MariaDB"))
        {
            Logger.Warning($"MariaDB isn't officially supported! Use at your OWN RISK! (DO NOT report bugs about MariaDB)");
            useLegacy = true;
        } else
        {
            throw new($"MySQL version out-of-date, please upgrade to version {MIN_MYSQL_VERSION.Item1}.${MIN_MYSQL_VERSION.Item2}");
        }

        if (DatabaseExists())
        {
            Logger.Information("Database already exists.");
            return;
        }

        Logger.Information("Creating database...");
        CreateDatabase(useLegacy);

        string[] seeds =
        {
            "Shops", "ShopItems", "BeautyShops", "BeautyShopItems", "MeretMarket", "Mapleopoly", "Events", "CardReverseGame", "RouletteGameItems", "OXQuiz"
        };

        foreach (string seed in seeds)
        {
            Seed(seed, useLegacy);
        }

        Logger.Information("Database created.");
    }

    public static void RunQuery(string query)
    {
        new QueryFactory(new MySqlConnection(ConnectionString), new MySqlCompiler()).Statement(query);
    }

    public static Tuple<Tuple<int, int>, string> GetVersion()
    {
        MySqlConnection conn = new(ConnectionStringWithoutTable);
        conn.Open();

        string versionString = conn.ServerVersion;
        string[] versionStrings = versionString.Split(".");

        return new(new(int.Parse(versionStrings[0]), int.Parse(versionStrings[1])), versionString);
    }

    public static bool DatabaseExists()
    {
        dynamic result = new QueryFactory(new MySqlConnection(ConnectionStringWithoutTable), new MySqlCompiler())
            .Select($"SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = '{Database}'")
            .FirstOrDefault();

        return result != null;
    }

    public static void CreateDatabase(bool useLegacy = false)
    {
        string fileLines = ReadSqlFile($"{Paths.SOLUTION_DIR}/MapleServer2/Database/SQL/Database.sql", useLegacy);
        MySqlScript script = new(new(ConnectionStringWithoutTable), fileLines.Replace("DATABASE_NAME", Database));
        script.Execute();
    }

    private static void Seed(string type, bool useLegacy = false)
    {
        Logger.Information("Seeding {type}...", type);
        string fileLines = ReadSqlFile($"{Paths.SOLUTION_DIR}/MapleServer2/Database/Seeding/{type}Seeding.sql", useLegacy);
        ExecuteSqlFile(fileLines);
    }

    private static void ExecuteSqlFile(string fileLines)
    {
        MySqlScript script = new(new(ConnectionString), fileLines);
        script.Execute();
    }

    private static string ReadSqlFile(string filePath, bool useLegacy = false)
    {
        string fileLines = File.ReadAllText(filePath);
        if (useLegacy)
        {
            // Use Unicode 5.2.0 instead of Unicode 9.0.0 when MySQL(MariaDB) version is older than MIN_MYSQL_VERSION
            fileLines = fileLines.Replace("utf8mb4_0900_ai_ci", "utf8mb4_unicode_520_ci");
        }
        return fileLines;
    }
}
