using Maple2Storage.Types;
using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCube
    {
        private readonly string TableName = "Cubes";

        public long Insert(Cube cube)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                CoordX = cube.CoordF.X,
                CoordY = cube.CoordF.Y,
                CoordZ = cube.CoordF.Z,
                HomeId = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                ItemUid = cube.Item.Uid,
                LayoutUid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                cube.PlotNumber,
                Rotation = cube.Rotation.Z
            });
        }

        public Cube FindById(long uid) => ReadCube(DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).FirstOrDefault());

        public Dictionary<long, Cube> FindAllByHomeId(long homeId)
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Where("HomeId", homeId).Get();
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
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Where("LayoutUid", layoutUid).Get();
            List<Cube> cubes = new List<Cube>();
            foreach (dynamic data in result)
            {
                cubes.Add((Cube) ReadCube(data));
            }
            return cubes;
        }

        public void Update(Cube cube)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Uid", cube.Uid).Update(new
            {
                CoordX = cube.CoordF.X,
                CoordY = cube.CoordF.Y,
                CoordZ = cube.CoordF.Z,
                HomeId = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                ItemUid = cube.Item.Uid,
                LayoutUid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                cube.PlotNumber,
                Rotation = cube.Rotation.Z
            });
        }

        public bool Delete(long uid) => DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;

        private static Cube ReadCube(dynamic data) => new Cube(data.Uid, DatabaseManager.Items.FindByUid(data.ItemUid), data.PlotNumber, CoordF.From(data.CoordX, data.CoordY, data.CoordZ), data.Rotation, data.HomeLayoutId ?? 0, data.HomeId ?? 0);
    }
}
