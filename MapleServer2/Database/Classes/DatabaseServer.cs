using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseServer : DatabaseTable
    {
        public DatabaseServer() : base("server") { }

        public DateTimeOffset GetLastDailyReset()
        {
            dynamic results = QueryFactory.Query(TableName).Select("last_daily_reset").FirstOrDefault();

            return results is null ? DateTimeOffset.UtcNow : (DateTimeOffset) results.last_daily_reset;
        }

        public void SetLastDailyReset(DateTimeOffset date) => QueryFactory.Query(TableName).Update(new { last_daily_reset = date.UtcDateTime });
    }
}
