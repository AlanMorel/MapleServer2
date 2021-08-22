using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHomeLayout : DatabaseTable
    {
        public DatabaseHomeLayout(string tableName) : base(tableName) { }

        public long Insert(HomeLayout homeLayout)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public List<HomeLayout> FindAllByHomeId(long homeId)
        {
            List<HomeLayout> homeLayouts = QueryFactory.Query(TableName).Where("HomeId", homeId).Get<HomeLayout>().ToList();
            foreach (HomeLayout homeLayout in homeLayouts)
            {
                homeLayout.Cubes = DatabaseManager.Cubes.FindAllByLayoutUid(homeLayout.Uid);
            }
            return homeLayouts;
        }

        public void Update(HomeLayout homeLayout)
        {
            QueryFactory.Query(TableName).Where("Uid", homeLayout.Uid).Update(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;
    }
}
