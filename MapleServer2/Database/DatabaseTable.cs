using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace MapleServer2.Database;

public abstract class DatabaseTable
{
    protected string TableName;
    protected static QueryFactory QueryFactory
    {
        get
        {
            using (MySqlConnection connection = new(DatabaseManager.ConnectionString))
            {
                QueryFactory queryFactory = new(connection, new MySqlCompiler());
                // Log the compiled query to the console
                // queryFactory.Logger = compiled =>
                // {
                //     Logger.Debug(compiled.ToString());
                // };
                return queryFactory;
            }
        }
    }

    public DatabaseTable(string tableName)
    {
        TableName = tableName;
    }
}
