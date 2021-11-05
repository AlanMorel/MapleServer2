using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseHome : DatabaseTable
{
    public DatabaseHome() : base("homes") { }

    public long Insert(Home home)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            account_id = home.AccountId,
            map_id = home.MapId,
            plot_map_id = home.PlotMapId,
            plot_number = home.PlotNumber,
            apartment_number = home.ApartmentNumber,
            home.Expiration,
            home.Name,
            home.Description,
            home.Size,
            home.Height,
            architect_score_current = home.ArchitectScoreCurrent,
            architect_score_total = home.ArchitectScoreTotal,
            decoration_exp = home.DecorationExp,
            decoration_level = home.DecorationLevel,
            decoration_reward_timestamp = home.DecorationRewardTimestamp,
            home.Lighting,
            home.Background,
            home.Camera,
            home.Password,
            permissions = JsonConvert.SerializeObject(home.Permissions),
            interior_rewards_claimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed)
        });
    }

    public Home FindById(long id)
    {
        return ReadHome(QueryFactory.Query(TableName).Where("id", id).FirstOrDefault());
    }

    public Home FindByAccountId(long accountId)
    {
        return ReadHome(QueryFactory.Query(TableName).Where("account_id", accountId).FirstOrDefault());
    }

    public List<Home> FindAllByMapId(int mapId)
    {
        IEnumerable<dynamic> results = QueryFactory.Query(TableName).Where("map_id", mapId).Get();
        List<Home> homes = new();
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
            map_id = home.MapId,
            plot_map_id = home.PlotMapId,
            plot_number = home.PlotNumber,
            apartment_number = home.ApartmentNumber,
            home.Expiration,
            home.Name,
            home.Description,
            home.Size,
            home.Height,
            architect_score_current = home.ArchitectScoreCurrent,
            architect_score_total = home.ArchitectScoreTotal,
            decoration_exp = home.DecorationExp,
            decoration_level = home.DecorationLevel,
            decoration_reward_timestamp = home.DecorationRewardTimestamp,
            home.Lighting,
            home.Background,
            home.Camera,
            home.Password,
            permissions = JsonConvert.SerializeObject(home.Permissions),
            interior_rewards_claimed = JsonConvert.SerializeObject(home.InteriorRewardsClaimed)
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

    public bool Delete(long id)
    {
        return QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }

    private static Home ReadHome(dynamic data)
    {
        if (data == null)
        {
            return null;
        }
        Dictionary<long, Item> warehouseItems = DatabaseManager.Items.FindAllByHomeId(data.id);
        Dictionary<long, Cube> furnishingCubes = DatabaseManager.Cubes.FindAllByHomeId(data.id);
        List<HomeLayout> layouts = DatabaseManager.HomeLayouts.FindAllByHomeId(data.id);

        foreach (Item item in warehouseItems.Values)
        {
            item.SetMetadataValues();
        }

        foreach (Cube cube in furnishingCubes.Values)
        {
            cube.Item.SetMetadataValues();
        }

        return new()
        {
            Id = data.id,
            AccountId = data.account_id,
            MapId = data.map_id,
            PlotMapId = data.plot_map_id,
            PlotNumber = data.plot_number,
            ApartmentNumber = data.apartment_number,
            Expiration = data.expiration,
            Name = data.name,
            Description = data.description,
            Size = data.size,
            Height = data.height,
            ArchitectScoreCurrent = data.architect_score_current,
            ArchitectScoreTotal = data.architect_score_total,
            DecorationExp = data.decoration_exp,
            DecorationLevel = data.decoration_level,
            DecorationRewardTimestamp = data.decoration_reward_timestamp,
            Lighting = data.lighting,
            Background = data.background,
            Camera = data.camera,
            Password = data.password,
            Permissions = JsonConvert.DeserializeObject<Dictionary<HomePermission, byte>>(data.permissions),
            InteriorRewardsClaimed = JsonConvert.DeserializeObject<List<int>>(data.interior_rewards_claimed),
            FurnishingInventory = furnishingCubes,
            WarehouseInventory = warehouseItems,
            Layouts = layouts
        };
    }
}
