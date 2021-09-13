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
                coordx = cube.CoordF.X,
                coordy = cube.CoordF.Y,
                coordz = cube.CoordF.Z,
                homeid = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                itemuid = cube.Item.Uid,
                layoutuid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                cube.PlotNumber,
                rotation = cube.Rotation.Z
            });
        }

        public Cube FindById(long uid) => ReadCube(QueryFactory.Query(TableName).Where("uid", uid).FirstOrDefault());

        public Dictionary<long, Cube> FindAllByHomeId(long homeId)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("homeid", homeId).Get();
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
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("layoutuid", layoutUid).Get();
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
                coordx = cube.CoordF.X,
                coordy = cube.CoordF.Y,
                coordz = cube.CoordF.Z,
                homeid = cube.HomeId == 0 ? null : (long?) cube.HomeId,
                itemuid = cube.Item.Uid,
                layoutuid = cube.LayoutUid == 0 ? null : (long?) cube.LayoutUid,
                cube.PlotNumber,
                rotation = cube.Rotation.Z
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;

        private static Cube ReadCube(dynamic data) => new Cube(data.uid, DatabaseManager.Items.FindByUid(data.itemuid), data.plotnumber, CoordF.From(data.coordx, data.coordy, data.coordz), data.rotation, data.homelayoutid ?? 0, data.homeid ?? 0);
    }
}
