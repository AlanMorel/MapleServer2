using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHomeLayout
    {
        public static long CreateHomeLayout(HomeLayout homeLayout)
        {
            return DatabaseManager.QueryFactory.Query("HomeLayouts").InsertGetId<long>(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public static List<HomeLayout> FindAllByHomeId(long homeId)
        {
            List<HomeLayout> homeLayouts = DatabaseManager.QueryFactory.Query("HomeLayouts").Where("HomeId", homeId).Get<HomeLayout>().ToList();
            foreach (HomeLayout homeLayout in homeLayouts)
            {
                homeLayout.Cubes = DatabaseCube.FindAllByLayoutUid(homeLayout.Uid);
            }
            return homeLayouts;
        }

        public static void Update(HomeLayout homeLayout)
        {
            DatabaseManager.QueryFactory.Query("HomeLayouts").Where("Uid", homeLayout.Uid).Update(new
            {
                homeLayout.Id,
                homeLayout.Size,
                homeLayout.Height,
                homeLayout.HomeId,
                homeLayout.Name,
                homeLayout.Timestamp,
            });
        }

        public static bool Delete(long uid) => DatabaseManager.QueryFactory.Query("HomeLayouts").Where("Uid", uid).Delete() == 1;
    }
}
