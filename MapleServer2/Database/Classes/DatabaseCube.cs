using Maple2Storage.Types;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCube : DatabaseTable
    {
        public DatabaseCube() : base("cubes") { }

        public long Insert(Cube cube)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                coord_x = cube.CoordF.X,
                coord_y = cube.CoordF.Y,
                coord_z = cube.CoordF.Z,
                home_id = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                item_uid = cube.Item.Uid,
                layout_uid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                plot_number = cube.PlotNumber,
                rotation = cube.Rotation.Z
            });
        }

        public Cube FindById(long uid) => ReadCube(QueryFactory.Query(TableName).Where("uid", uid).FirstOrDefault());

        public Dictionary<long, Cube> FindAllByHomeId(long homeId)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("home_id", homeId).Get();
            Dictionary<long, Cube> cubes = new Dictionary<long, Cube>();
            foreach (dynamic data in result)
            {
                Cube cube = (Cube) ReadCube(data);
                cubes.Add(cube.Uid, cube);
            }
            return cubes;
        }

        public List<Cube> FindAllByLayoutUid(long layoutUid)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("layout_uid", layoutUid).Get();
            List<Cube> cubes = new List<Cube>();
            foreach (dynamic data in result)
            {
                cubes.Add((Cube) ReadCube(data));
            }
            return cubes;
        }

        public void Update(Cube cube)
        {
            QueryFactory.Query(TableName).Where("uid", cube.Uid).Update(new
            {
                coord_x = cube.CoordF.X,
                coord_y = cube.CoordF.Y,
                coord_z = cube.CoordF.Z,
                home_id = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                item_uid = cube.Item.Uid,
                layout_uid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                plot_number = cube.PlotNumber,
                rotation = cube.Rotation.Z
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;

        private static Cube ReadCube(dynamic data) => new Cube(data.uid, DatabaseManager.Items.FindByUid(data.item_uid), data.plot_number, CoordF.From(data.coord_x, data.coord_y, data.coord_z), data.rotation, data.home_layout_id ?? 0, data.home_id ?? 0);
    }
}
