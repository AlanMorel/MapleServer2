using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseHome : DatabaseTable
    {
        public DatabaseHome() : base("homes") { }

        public long Insert(Home home)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
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
                permissions = JsonConvert.SerializeObject(home.Permissions),
                interiorrewardsclaimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed),
            });
        }

        public Home FindById(long id) => ReadHome(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());

        public List<Home> FindAllByMapId(int mapId)
        {
            IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("mapid", mapId).Get();
            List<Home> homes = new List<Home>();
            foreach (dynamic data in results)
            {
                homes.Add((Home) ReadHome(data));
            }
            return homes;
        }

        public void Update(Home home)
        {
            QueryFactory.Query(TableName).Where("id", home.Id).Update(new
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
                permissions = JsonConvert.SerializeObject(home.Permissions),
                interiorrewardsclaimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed),
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

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static Home ReadHome(dynamic data)
        {
            if (data == null)
            {
                return null;
            }
            Dictionary<long, Item> warehouseItems = DatabaseManager.Items.FindAllByHomeId(data.id);
            Dictionary<long, Cube> furnishingCubes = DatabaseManager.Cubes.FindAllByHomeId(data.id);
            List<HomeLayout> layouts = DatabaseManager.HomeLayouts.FindAllByHomeId(data.id);
            return new Home()
            {
                Id = data.id,
                AccountId = data.accountid,
                MapId = data.mapid,
                PlotMapId = data.plotmapid,
                PlotNumber = data.plotnumber,
                ApartmentNumber = data.apartmentnumber,
                Expiration = data.expiration,
                Name = data.name,
                Description = data.description,
                Size = data.size,
                Height = data.height,
                ArchitectScoreCurrent = data.architectscorecurrent,
                ArchitectScoreTotal = data.architectscoretotal,
                DecorationExp = data.decorationexp,
                DecorationLevel = data.decorationlevel,
                DecorationRewardTimestamp = data.decorationrewardtimestamp,
                Lighting = data.lighting,
                Background = data.background,
                Camera = data.camera,
                Password = data.password,
                Permissions = JsonConvert.DeserializeObject<Dictionary<HomePermission, byte>>(data.permissions),
                InteriorRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.interiorrewardsclaimed),
                FurnishingInventory = furnishingCubes,
                WarehouseInventory = warehouseItems,
                Layouts = layouts
            };
        }
    }
}
