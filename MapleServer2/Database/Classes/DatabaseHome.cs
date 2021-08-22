using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHome
    {
        private readonly string TableName = "Homes";

        public long Insert(Home home)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                home.AccountId,
                home.MapId,
                home.PlotMapId,
                home.PlotNumber,
                home.ApartmentNumber,
                home.Expiration,
                home.Name,
                home.Description,
                home.Size,
                home.Height,
                home.ArchitectScoreCurrent,
                home.ArchitectScoreTotal,
                home.DecorationExp,
                home.DecorationLevel,
                home.DecorationRewardTimestamp,
                home.Lighting,
                home.Background,
                home.Camera,
                home.Password,
                Permissions = JsonConvert.SerializeObject(home.Permissions),
                InteriorRewardsClaimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed),
            });
        }

        public Home FindById(long id) => ReadHome(DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).FirstOrDefault());

        public List<Home> FindAllByMapId(int mapId)
        {
            IEnumerable<dynamic> results = DatabaseManager.QueryFactory.Query(TableName).Where("MapId", mapId).Get();
            List<Home> homes = new List<Home>();
            foreach (dynamic data in results)
            {
                homes.Add((Home) ReadHome(data));
            }
            return homes;
        }

        public void Update(Home home)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", home.Id).Update(new
            {
                home.MapId,
                home.PlotMapId,
                home.PlotNumber,
                home.ApartmentNumber,
                home.Expiration,
                home.Name,
                home.Description,
                home.Size,
                home.Height,
                home.ArchitectScoreCurrent,
                home.ArchitectScoreTotal,
                home.DecorationExp,
                home.DecorationLevel,
                home.DecorationRewardTimestamp,
                home.Lighting,
                home.Background,
                home.Camera,
                home.Password,
                Permissions = JsonConvert.SerializeObject(home.Permissions),
                InteriorRewardsClaimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed),
            });

            foreach (Item item in home.WarehouseInventory.Where(item => item.Value != null).Select(x => x.Value))
            {
                item.HomeId = home.Id;
                DatabaseManager.Items.Update(item);
            }
            foreach (Cube cube in home.FurnishingInventory.Where(cube => cube.Value != null).Select(x => x.Value))
            {
                DatabaseManager.Cubes.Update(cube);
            }
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;

        private static Home ReadHome(dynamic data)
        {
            if (data == null)
            {
                return null;
            }
            Dictionary<long, Item> warehouseItems = DatabaseManager.Items.FindAllByHomeId(data.Id);
            Dictionary<long, Cube> furnishingCubes = DatabaseManager.Cubes.FindAllByHomeId(data.Id);
            List<HomeLayout> layouts = DatabaseManager.HomeLayouts.FindAllByHomeId(data.Id);
            return new Home()
            {
                Id = data.Id,
                AccountId = data.AccountId,
                MapId = data.MapId,
                PlotMapId = data.PlotMapId,
                PlotNumber = data.PlotNumber,
                ApartmentNumber = data.ApartmentNumber,
                Expiration = data.Expiration,
                Name = data.Name,
                Description = data.Description,
                Size = data.Size,
                Height = data.Height,
                ArchitectScoreCurrent = data.ArchitectScoreCurrent,
                ArchitectScoreTotal = data.ArchitectScoreTotal,
                DecorationExp = data.DecorationExp,
                DecorationLevel = data.DecorationLevel,
                DecorationRewardTimestamp = data.DecorationRewardTimestamp,
                Lighting = data.Lighting,
                Background = data.Background,
                Camera = data.Camera,
                Password = data.Password,
                Permissions = JsonConvert.DeserializeObject<Dictionary<HomePermission, byte>>(data.Permissions),
                InteriorRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.InteriorRewardsClaimed),
                FurnishingInventory = furnishingCubes,
                WarehouseInventory = warehouseItems,
                Layouts = layouts
            };
        }
    }
}
