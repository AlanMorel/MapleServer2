using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHomeLayout : DatabaseTable
    {
        public DatabaseHomeLayout() : base("home_layouts") { }

        public long Insert(HomeLayout homeLayout)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                home_id = homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public List<HomeLayout> FindAllByHomeId(long homeId)
        {
            List<HomeLayout> homeLayouts = QueryFactory.Query(TableName).Where("home_id", homeId).Get<HomeLayout>().ToList();
            foreach (HomeLayout homeLayout in homeLayouts)
            {
                homeLayout.Cubes = DatabaseManager.Cubes.FindAllByLayoutUid(homeLayout.Uid);
            }
            return homeLayouts;
        }

        public void Update(HomeLayout homeLayout)
        {
            QueryFactory.Query(TableName).Where("uid", homeLayout.Uid).Update(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                home_id = homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }
}
