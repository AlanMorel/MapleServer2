using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHomeLayout
    {
        private readonly string TableName = "HomeLayouts";

        public long CreateHomeLayout(HomeLayout homeLayout)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
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
            List<HomeLayout> homeLayouts = DatabaseManager.QueryFactory.Query(TableName).Where("HomeId", homeId).Get<HomeLayout>().ToList();
            foreach (HomeLayout homeLayout in homeLayouts)
            {
                homeLayout.Cubes = DatabaseManager.Cubes.FindAllByLayoutUid(homeLayout.Uid);
            }
            return homeLayouts;
        }

        public void Update(HomeLayout homeLayout)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Uid", homeLayout.Uid).Update(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public bool Delete(long uid) => DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;
    }
}
