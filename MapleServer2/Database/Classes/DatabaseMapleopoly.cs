using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseMapleopoly : DatabaseTable
    {
        public DatabaseMapleopoly() : base("MapleopolyTiles") { }

        public List<MapleopolyTile> FindAllTiles() => QueryFactory.Query(TableName).Get<MapleopolyTile>().OrderBy(x => x.TilePosition).ToList();

        public MapleopolyTile FindTileByPosition(int tilePosition) => QueryFactory.Query(TableName).Where("TilePosition", tilePosition).Get<MapleopolyTile>().FirstOrDefault();
    }
}
