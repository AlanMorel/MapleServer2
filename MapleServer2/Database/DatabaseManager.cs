
using MapleServer2.Constants;
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

        static DatabaseManager()
        {
            ConnectionString = $"SERVER={Server};PORT={Port};USER={User};PASSWORD={Password};DATABASE={Database}";
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
