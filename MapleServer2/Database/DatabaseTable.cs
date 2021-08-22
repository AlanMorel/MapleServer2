
namespace MapleServer2.Database
{
    public abstract class DatabaseTable
    {
        protected string TableName;

        public DatabaseTable(string tableName)
        {
            TableName = tableName;
        }
    }
}
